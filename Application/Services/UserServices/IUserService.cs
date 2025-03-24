using Application.Common;
using Application.DTOs.AuthDTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public interface IUserService
    {
        Task<User?> FindUserByUserNameAsync(string email);

        Task<ResponseApi> LoginAsync(LoginDTO dto);

        Task<UserResponse> GetByIdAsync(int id);

        Task<ResponseApi> GetAllAsync();


        Task<ResponseApi> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO);

        Task<ResponseApi> InsertAsync(UserDTO dto, IFormFile file);

        Task<ResponseApi> RegisterAsync(UserDTO dto, IFormFile file);

        Task<ResponseApi> DisableUserAsync(int userId);

        Task<ResponseApi> UpdateAsync(int id, UserDTO dto, IFormFile file);

        Task<ResponseApi> GetAllUserRole();
    }
}
