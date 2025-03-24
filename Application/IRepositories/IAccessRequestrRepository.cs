using Domain.Entities;

namespace Application.IRepositories
{
    public interface IAccessRequestrRepository : IBaseRepository<AccessRequest>
    {
        Task<int> UpdateStatus(int id, int status);

        Task<List<AccessRequest>> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? status);

        Task<List<AccessRequest>> GetByStatus(int? userId, int? status);
    }
}
