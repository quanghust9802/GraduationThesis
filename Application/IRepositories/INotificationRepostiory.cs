using Domain.Entities;

namespace Application.IRepositories
{
    public interface INotificationRepostiory : IBaseRepository<Notification>
    {
        Task<List<Notification>> GetNotificationsByUserId(int userId);
    }
}