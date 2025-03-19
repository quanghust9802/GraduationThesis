using Application.Common.Paging;
using Domain.Entities;

namespace Application.IRepositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User?> FindUserByUserNameAsync(string userName);
        Task<User> GenerateUserInformation(User user);
        Task<PaginationResponse<User>> GetFilterAsync(UserFilterRequest request);

        Task<IEnumerable<User>> GetStaffList();

        Task<IEnumerable<User>> GetQualityStaffList();

        Task<IEnumerable<User>> GetShipperList();
    }
}

