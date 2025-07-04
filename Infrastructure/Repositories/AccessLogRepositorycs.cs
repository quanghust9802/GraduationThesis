﻿using Application.IRepositories;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AccessLogRepository : BaseRepository<AccessLogs>, IAccessLogRepository
    {
        public AccessLogRepository(AccessControllContext context) : base(context)
        {

        }
        public async Task<List<AccessLogs>> GetByUserId(int userId)
        {
            var data = await _context.AccessLogs.AsNoTracking().Where(u => u.UserId == userId).ToListAsync();
            return data;
        }
        public async Task<List<AccessLogs>> GetByRequestId(int requestId)
        {
            var data = await _context.AccessLogs.AsNoTracking().Where(u => u.AccessRequestId == requestId).ToListAsync();
            return data;
        }

        public async Task<List<AccessLogs>> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? requestId, int? userId)
        {
            var query = _context.AccessLogs.AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(log => log.AccessTime >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(log => log.AccessTime <= endDate.Value);
            }

            if (requestId.HasValue)
            {
                query = query.Where(log => log.AccessRequestId == requestId.Value);
            }
            if (userId.HasValue)
            {
                query = query.Where(log => log.UserId == userId.Value);
            }

            return await query.Include(log => log.User) 
                                .ToListAsync();
        }

        public async Task<List<AccessLogs>> GetAllWithUser()
        {
            var query = _context.AccessLogs.AsQueryable();
            return await query
                .Include(log => log.User)
                .OrderByDescending(log => log.AccessTime)
                .ToListAsync();
        }




    }

}
