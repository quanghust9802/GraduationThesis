using System;
using System.Buffers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.AccessLogDTOs;
using Application.Services.AccessLogServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;

namespace Infrastructure.Messaging
{
    public class MqttService : IHostedService, IDisposable
    {
        private readonly IMqttClient _mqttClient;
        private readonly MqttClientOptions _mqttOptions;
        private readonly ILogger<MqttService> _logger;
        private readonly IServiceProvider _serviceProvider; 

        public MqttService(ILogger<MqttService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(configuration["Mqtt:Broker"], configuration.GetValue<int>("Mqtt:Port"))
                .WithCredentials(configuration["Mqtt:Username"], configuration["Mqtt:Password"])
                .WithCleanSession()
                .Build();
            _serviceProvider = serviceProvider;
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _mqttClient.ConnectedAsync += async e =>
            {
                _logger.LogInformation("Connected to MQTT broker");

                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder()
                    .WithTopic("access/log")
                    .WithAtMostOnceQoS()
                    .Build());

                _logger.LogInformation("Subscribed to topic: access/log");
            };

            _mqttClient.DisconnectedAsync += async e =>
            {
                _logger.LogWarning("Disconnected from MQTT broker. Reconnecting...");
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                try
                {
                    await _mqttClient.ConnectAsync(_mqttOptions, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to reconnect to MQTT broker.");
                }
            };

            _mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload.ToArray());
                var topic = e.ApplicationMessage.Topic;
                _logger.LogInformation($"Received message: {message} on topic: {topic}");

                try
                {
                    var accessLogDto = ParseMessage(message, topic);
                    if (accessLogDto != null)
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var accessLogService = scope.ServiceProvider.GetRequiredService<IAccessLogService>();
                            await accessLogService.ProcessAccessLogAsync(accessLogDto);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                }
            };

            await _mqttClient.ConnectAsync(_mqttOptions, cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_mqttClient.IsConnected)
            {
                var disconnectOptions = new MqttClientDisconnectOptionsBuilder().Build();
                await _mqttClient.DisconnectAsync(disconnectOptions, cancellationToken);
            }
            _mqttClient.Dispose();
        }


        private AccessLogDTO? ParseMessage(string message, string topic)
        {
            try
            {
                var jsonDoc = JsonDocument.Parse(message);
                var root = jsonDoc.RootElement;

                string cccdId = root.GetProperty("cccdId").GetString();
                string timeString = root.GetProperty("accessTime").GetString();
                int status = root.GetProperty("status").GetInt32();

                if (string.IsNullOrEmpty(cccdId) || string.IsNullOrEmpty(timeString))
                {
                    _logger.LogWarning("Invalid JSON format: cccdId or accessTime is missing");
                    return null;
                }

                if (!DateTime.TryParse(timeString, out var accessTime))
                {
                    _logger.LogWarning("Invalid time format: " + timeString);
                    return null;
                }

                return new AccessLogDTO
                {
                    CccdId = cccdId,
                    AccessTime = accessTime,
                    Status = status
                };
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to parse JSON message: " + message);
                return null;
            }
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }
    }
}
