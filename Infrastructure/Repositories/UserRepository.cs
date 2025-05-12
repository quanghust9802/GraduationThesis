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
        public async Task<User?> FindUserByUserNameAsync(string username)
            => await _context.Users
                .AsNoTracking()
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.Username == username);

        public async Task<int?> GetAdminUserIdAsync()
        {
            return await _context.Users
                .Where(user => user.UserRoleId == 2)
                .Select(user => user.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByCccdId(string cccdId)
    => await _context.Users
        .AsNoTracking()
        .Include(u => u.UserRole)
        .FirstOrDefaultAsync(u => u.CccdId == cccdId);




    }
}
