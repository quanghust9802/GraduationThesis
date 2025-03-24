using Domain.Entities;

namespace Application.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindUserByUserNameAsync(string userName);

        Task<int?> GetAdminUserIdAsync();

    }
}

