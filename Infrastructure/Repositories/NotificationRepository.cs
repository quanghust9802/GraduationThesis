using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class NotificatioRepository : BaseRepository<Notification>, INotificationRepostiory
    {
        public NotificatioRepository(AccessControllContext context) : base(context)
        {
        }



    }
}
