using Application.AuthProvide;
using Application.Common;
using Application.DTOs.NotificationDTOs;
using Application.Hubs;
using Application.IRepositories;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly INotificationRepostiory _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IHubContext<NotificationHub> hubContext, IMapper mapper, INotificationRepostiory notificationRepository
            )
        {
            _mapper = mapper;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
        }
        public async Task<NotificationResponse> GetByIdAsync(int id)
        {
            var entity = await _notificationRepository.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<NotificationResponse>(entity);
            return dto;

        }

        public async Task<ResponseApi> GetAllAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<NotificationResponse>>(notifications);
            return new ResponseApi
            {
                Data = dtos,
                ErrCode = 200,
            };
        }

        public async Task<ResponseApi> AddNotification(NotificationResponse dto)
        {
            try
            {
                var notification = _mapper.Map<Notification>(dto);

                var result = await _notificationRepository.InsertAsync(notification);

                if (result > 0)
                {
                    await _hubContext.Clients.User(dto.UserId.ToString())
                        .SendAsync("ReceiveNotification", dto.Message);

                    var response = _mapper.Map<NotificationResponse>(notification);

                    return new ResponseApi
                    {
                        ErrCode = 200,
                        Data = response,
                        ErrDesc = "Thêm mới thông báo thành công"
                    };
                }

                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Thêm mới thông báo không thành công"
                };
            }
            catch (Exception ex)
            {
                return new ResponseApi
                {
                    ErrCode = 500,
                    Data = null,
                    ErrDesc = $"Đã xảy ra lỗi: {ex.Message}"
                };
            }
        }

        public async Task<ResponseApi> DeleteAsync(int id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy thông báo"
                };
            }

            var result = await _notificationRepository.DeleteAsync(notification);

            if (result > 0)
            {
                var res = _mapper.Map<NotificationResponse>(notification);
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Xóa thông báo thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Xóa không thành công"
                };
            }
        }
        public async Task SendNotificationAsync(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
