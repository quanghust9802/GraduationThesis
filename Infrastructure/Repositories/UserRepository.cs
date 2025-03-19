using Application.Common.Paging;
using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(AccessControllContext context) : base(context)
        {
        }
        public async Task<User?> FindUserByUserNameAsync(string email) => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserName == email);

        public async Task<PaginationResponse<User>> GetFilterAsync(UserFilterRequest request)
        {
            IQueryable<User> query = _table.Where(u => u.IsDeleted != true);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p =>
                    (p.FirstName + " " + p.LastName).Contains(request.SearchTerm));
            }

            if (request.SortOrder?.ToLower() == "descend")
            {
                query = query.OrderByDescending(GetSortProperty(request));
            }
            else
            {
                query = query.OrderBy(GetSortProperty(request));
            }
            var totalCount = await query.CountAsync();
            var items = await query.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize).AsNoTracking().ToListAsync();
            return new(items, totalCount);
        }


        private static Expression<Func<User, object>> GetSortProperty(UserFilterRequest request) =>
        request.SortColumn?.ToLower() switch
        {
            "name" => user => user.FirstName + " " + user.LastName,
            "type" => user => user.RoleType,
            _ => user => user.FirstName + " " + user.LastName
        };

        public async Task<IEnumerable<User>> GetStaffList()
        {
            var users = await _context.Users
                .Where(u => u.RoleType != Role.Customer && u.IsDeleted != true)
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<User>> GetQualityStaffList()
        {
            var users = await _context.Users
                .Where(u => u.RoleType == Role.QualityControl && u.IsDeleted != true)
                .ToListAsync();

            return users;
        }

        public async Task<IEnumerable<User>> GetShipperList()
        {
            var users = await _context.Users
                .Where(u => u.RoleType == Role.Shipper && u.IsDeleted != true)
                .ToListAsync();

            return users;
        }


    }
}
