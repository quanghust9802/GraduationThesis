﻿using Domain.Entities;

namespace Application.IRepositories
{
    public interface IAccessRequestrRepository : IBaseRepository<AccessRequest>
    {
        Task<int> UpdateStatus(int id, int status, int userId);

        Task<List<AccessRequest>> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? status, int? userId);

        Task<List<AccessRequest>> GetByStatus(int? userId, int? status);

        Task<(AccessRequest, string)> VerifyInfor(string cccd);

        Task<IEnumerable<AccessRequest>> GetAllWithUserAsync();
    }
}
