﻿using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccessRequestRepository : BaseRepository<AccessRequest>, IAccessRequestrRepository
    {
        public AccessRequestRepository(AccessControllContext context) : base(context)
        {
        }
        public async Task<int> UpdateStatus(int id, int status, int userId)
        {
            var entity = await _context.AccessRequests
                .SingleOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return 0;
            }

            entity.status = status;
            entity.UserApprovalid = userId;
            _context.Update(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<AccessRequest>> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? status, int? userId)
        {
            var query = _context.AccessRequests.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(request => request.StartTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(request => request.EndTime <= endDate.Value);
            }

            if (status.HasValue)
            {
                query = query.Where(request => request.status == status.Value);
            }
            if (userId.HasValue)
            {
                query = query.Where(request => request.UserRequestId == userId.Value);
            }
            return await query.ToListAsync();
        }

        public async Task<List<AccessRequest>> GetByStatus(int? userId, int? status)
        {
            var query = _context.AccessRequests.AsQueryable();

            if (userId.HasValue)
            {
                query = query.Where(request => request.UserRequestId == userId);
            }

            if (status.HasValue)
            {
                query = query.Where(request => request.status == status);
            }

            return await query.ToListAsync();
        }

        public async Task<(AccessRequest, string)> VerifyInfor(string cccd)
        {
            if (string.IsNullOrEmpty(cccd))
            {
                return (null, null);
            }

            var user = await _context.Users
                .Where(x => x.CccdId == cccd)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return (null, null);
            }

            var accessRequest = await _context.AccessRequests
                .Where(r => r.UserRequestId == user.Id)
                .FirstOrDefaultAsync();

            return (accessRequest, user.Mrz);
        }


    }
}
