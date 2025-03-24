using Application.Common;
using Application.DTOs.AccessRequestDTOs;

namespace Application.Services.AccessRequestServices
{
    public interface IAccessRequestService
    {
        Task<AccessRequestDTO> GetByIdAsync(int id);
        Task<ResponseApi> GetAllAsync();

        Task<ResponseApi> InsertAsync(AccessRequestDTO dto);

        Task<ResponseApi> UpdateAsync(int id, AccessRequestDTO dto);

        Task<ResponseApi> DeleteAsync(int id);

        Task<ResponseApi> UpdateStatus(int id, int status);

        Task<ResponseApi> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? requestId);


        Task<ResponseApi> GetByStatus(int? userId, int? status);
    }
}
