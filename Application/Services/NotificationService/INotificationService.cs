using Application.Common;
using Application.DTOs.NotificationDTOs;

namespace Application.Services.NotificationService
{
    public interface INotificationService
    {
        Task<NotificationResponse> GetByIdAsync(int id);
        Task<ResponseApi> GetAllAsync();

        Task<ResponseApi> AddNotification(NotificationResponse dto);

        Task<ResponseApi> DeleteAsync(int id);

        Task<ResponseApi> GetNotificationsByUserId(int userId);
    }
}
