using System;
using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOs.AccessLogDTOs;
using Application.Services.AccessLogServices;
using Microsoft.Extensions.Configuration;
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
        private readonly IAccessLogService _accessLogService;
        private readonly ILogger<MqttService> _logger;

        public MqttService(IAccessLogService accessLogService, ILogger<MqttService> logger, IConfiguration configuration)
        {
            _accessLogService = accessLogService;
            _logger = logger;
            var factory = new MqttClientFactory();
            _mqttClient = factory.CreateMqttClient();

            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(configuration["Mqtt:Broker"], configuration.GetValue<int>("Mqtt:Port"))
                .WithCredentials(configuration["Mqtt:Username"], configuration["Mqtt:Password"])
                .WithCleanSession()
                .Build();
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
                _logger.LogWarning("Disconnected from MQTT broker. Reconnecting in 5 seconds...");
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
                        await _accessLogService.ProcessAccessLogAsync(accessLogDto);
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
                // Ví dụ: Người dùng "123456789" vào lúc 2025-05-12T14:23:00, allowed
                var parts = message.Split(' ');
                if (parts.Length < 6) return null;

                var cccd = parts[2].Trim('"');
                var time = DateTime.Parse(parts[5]);

                int status = message.Contains("allowed") ? 1 : 0;

                return new AccessLogDTO
                {
                    CccdId = cccd,
                    AccessTime = time,
                    Status = status
                };
            }
            catch
            {
                _logger.LogWarning("Failed to parse message: " + message);
                return null;
            }
        }

        public void Dispose()
        {
            _mqttClient?.Dispose();
        }
    }
}
