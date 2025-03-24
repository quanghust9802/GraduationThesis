using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AccessControllContext context) : base(context)
        {
        }
        public async Task<User?> FindUserByUserNameAsync(string username) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username);

        public async Task<int?> GetAdminUserIdAsync()
        {
            return await _context.Users
                .Where(user => user.UserRole.RoleType == RoleType.Admin)
                .Select(user => user.Id)
                .FirstOrDefaultAsync();
        }




    }
}
