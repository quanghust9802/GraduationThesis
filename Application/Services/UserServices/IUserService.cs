using Application.Common.Paging;
using Application.DTOs.AuthDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User?> FindUserByUserNameAsync(string email);

        Task<LoginResponse> LoginAsync(LoginDTO dto);

        Task<UserResponse> GetByIdAsync(int id);

        Task<IEnumerable<UserResponse>> GetAllAsync();


        Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

        Task<PaginationResponse<UserResponse>> GetFilterAsync(UserFilterRequest request);

        Task<UserResponse> InsertAsync(UserDTO dto, IFormFile file);

        Task DisableUserAsync(int userId);

        Task<UserResponse> UpdateAsync(int id, UserDTO dto, IFormFile file);

        Task<IEnumerable<UserResponse>> GetStaffList();

        Task<IEnumerable<UserResponse>> GetQualityStaffList();
        Task<IEnumerable<UserResponse>> GetShipperList();
    }
}
