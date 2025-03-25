using Domain.Entities;

namespace Application.IRepositories
{
    public interface IAccessLogRepository : IBaseRepository<AccessLogs>
    {
        Task<List<AccessLogs>> GetByUserId(int userId);
        Task<List<AccessLogs>> GetByRequestId(int requestId);
        Task<List<AccessLogs>> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? requestId, int? userId);
    }
}
