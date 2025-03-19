using Application.AuthProvide;
using Application.Common;
using Application.Common.Paging;
using Application.DTOs.AuthDTOs;
using Application.IRepositories;
using Application.Services.ImageServices;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDTO> _validator;
        private readonly IImageService _imageService;

        public UserService(IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IValidator<UserDTO> validator, IImageService imageService
            )
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _validator = validator;
            _imageService = imageService;
        }
        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var entity = await _userRepo.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<UserResponse>(entity);
            return dto;

        }

        public async Task<IEnumerable<UserResponse>> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(users);
            return dtos;
        }

        public async Task<UserResponse> InsertAsync(UserDTO dto, IFormFile file)
        {
            var imageUrl = await _imageService.UploadImageAsync(file);

            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }

            var user = _mapper.Map<User>(dto);
            user.ImageUrl = imageUrl;
            user = await _userRepo.GenerateUserInformation(user);

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedAt = DateTime.Now;
            await _userRepo.InsertAsync(user);

            user = await FindUserByUserNameAsync(user.Username);
            var res = _mapper.Map<UserResponse>(user);
            return res;

        }

        //change password
        public async Task<bool> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userRepo.GetByIdAsync(changePasswordDTO.Id) ?? throw new NotFoundException();
            bool checkPassword = BCrypt.Net.BCrypt.Verify(changePasswordDTO.OldPassword, user.Password);
            if (!checkPassword)
            {
                throw new DataInvalidException("Password is incorrect");
            }
            if (changePasswordDTO.NewPassword == changePasswordDTO.OldPassword)
            {
                throw new DataInvalidException("Do not re-enter the old password");
            }
            var check = new PasswordRegex();
            if (!check.IsPasswordValid(changePasswordDTO.NewPassword))
                throw new DataInvalidException("The password having at least 8 characters, 1 uppercase letter, 1 lowercase letter, 1 number, and 1 symbol");

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);
            user.ModifiedAt = DateTime.UtcNow;
            user.ModifiedBy = user.FirstName;

            await _userRepo.UpdateAsync(user);

            return true;
        }


        public async Task<User?> FindUserByUserNameAsync(string email) => await _userRepo.FindUserByUserNameAsync(email);

        public async Task<LoginResponse> LoginAsync(LoginDTO dto)
        {
            var getUser = await _userRepo.FindUserByUserNameAsync(dto.UserName!);
            if (getUser == null)
                return new LoginResponse(false, "User not found");

            if (getUser.IsDeleted == true)
                throw new ForbiddenException("Your account has been disabled");

            var checkPassword = BCrypt.Net.BCrypt.Verify(dto.Password, getUser.Password);

            return checkPassword ? new LoginResponse(true, "Login success", _tokenService.GenerateJWTWithUser(getUser)) : new LoginResponse(false, "Invalid username or password. Please try again.");
        }

        public async Task<PaginationResponse<UserResponse>> GetFilterAsync(UserFilterRequest request)
        {
            var res = await _userRepo.GetFilterAsync(request);
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(res.Data);
            return new(dtos, res.TotalCount);

        }

        public async Task DisableUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (user.IsDeleted == true)
            {
                throw new NotAllowedException("User has been disabled");
            }


            user.IsDeleted = true;
            await _userRepo.UpdateAsync(user);
        }

        public async Task<UserResponse> UpdateAsync(int id, UserDTO dto, IFormFile file)
        {
            var imageUrl = await _imageService.UploadImageAsync(file);
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }

            user.DateOfBirth = (DateTime)dto.DateOfBirth!;
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Address = dto.Address;
            user.Gender = dto.Gender;
            user.PhoneNumber = dto.PhoneNumber;
            user.RoleType = dto.RoleType;
            user.ModifiedAt = DateTime.UtcNow;
            user.ImageUrl = imageUrl;

            try
            {
                await _userRepo.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                throw new DataInvalidException(ex.Message);
            }

            var res = _mapper.Map<UserResponse>(user);
            return res;
        }

        public async Task<IEnumerable<UserResponse>> GetStaffList()
        {
            var user = await _userRepo.GetStaffList();
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(user);
            return dtos;
        }

        public async Task<IEnumerable<UserResponse>> GetQualityStaffList()
        {
            var user = await _userRepo.GetQualityStaffList();
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(user);
            return dtos;
        }

        public async Task<IEnumerable<UserResponse>> GetShipperList()
        {
            var user = await _userRepo.GetShipperList();
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(user);
            return dtos;
        }

    }
}
