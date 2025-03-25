using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class NotificatioRepository : BaseRepository<Notification>, INotificationRepostiory
    {
        public NotificatioRepository(AccessControllContext context) : base(context)
        {
        }

        public async Task<List<Notification>> GetNotificationsByUserId(int userId)
    => await _context.Notifications
        .AsNoTracking()
        .Where(u => u.UserId == userId).ToListAsync();

    }
}
