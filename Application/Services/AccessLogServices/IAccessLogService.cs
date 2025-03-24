using Application.Common;
using Application.DTOs.AccessLogDTOs;

namespace Application.Services.AccessLogServices
{
    public interface IAccessLogService
    {
        Task<AccessLogResponse> GetByIdAsync(int id);
        Task<ResponseApi> GetAllAsync();

        Task<ResponseApi> InsertAsync(AccessLogDTO dto);

        Task<ResponseApi> DeleteAsync(int id);

        Task<ResponseApi> GetByUserIdAsync(int userId);
        Task<ResponseApi> GetByRequestIdAsync(int requestId);

        Task<ResponseApi> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? requestId);
    }

}
