using Application.AuthProvide;
using Application.Common;
using Application.DTOs.AuthDTOs;
using Application.DTOs.NotificationDTOs;
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
        private readonly IUserRoleRepository _userRoleRepository;

        public UserService(IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IValidator<UserDTO> validator, IImageService imageService, IUserRoleRepository userRoleRepository
            )
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _validator = validator;
            _imageService = imageService;
            _userRoleRepository = userRoleRepository;
        }
        public async Task<UserResponse> GetByIdAsync(int id)
        {
            var entity = await _userRepo.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<UserResponse>(entity);
            return dto;

        }

        public async Task<ResponseApi> GetAllAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<UserResponse>>(users);
            return new ResponseApi
            {
                Data = dtos,
                ErrCode = 200,
            };
        }
        public async Task<ResponseApi> GetAllUserRole()
        {
            var userRoles = await _userRoleRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<UserRoleReponse>>(userRoles);
            return new ResponseApi
            {
                Data = dtos,
                ErrCode = 200,
            };
        }
        public async Task<ResponseApi> InsertAsync(UserDTO dto, IFormFile file)
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
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedAt = DateTime.Now;
            var result = await _userRepo.InsertAsync(user);

            user = await FindUserByUserNameAsync(user.Username);
            var res = _mapper.Map<UserResponse>(user);
            if (result > 0)
            {
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Thêm mới người dùng thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Thêm mói người dùng không thành công"
                };
            }

        }

        public async Task<ResponseApi> RegisterAsync(UserDTO dto, IFormFile file)
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
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.CreatedAt = DateTime.Now;
            //register guest
            user.UserRoleId = 4;
            var result = await _userRepo.InsertAsync(user);

            user = await FindUserByUserNameAsync(user.Username);
            var res = _mapper.Map<UserResponse>(user);
            if (result > 0)
            {
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Thêm mới người dùng thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Thêm mói người dùng không thành công"
                };
            }

        }

        //change password
        public async Task<ResponseApi> ChangePasswordAsync(ChangePasswordDTO changePasswordDTO)
        {
            var user = await _userRepo.GetByIdAsync(changePasswordDTO.Id);

            if (user == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy người dùng"
                };
            }

            bool checkPassword = BCrypt.Net.BCrypt.Verify(changePasswordDTO.OldPassword, user.Password);
            if (!checkPassword)
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    ErrDesc = "Mật khẩu không chính xác"
                };
            }

            if (changePasswordDTO.NewPassword == changePasswordDTO.OldPassword)
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    ErrDesc = "Không được nhập lại mật khẩu cũ"
                };
            }

            var check = new PasswordRegex();
            if (!check.IsPasswordValid(changePasswordDTO.NewPassword))
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    ErrDesc = "Mật khẩu phải có ít nhất 8 ký tự, 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt"
                };
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePasswordDTO.NewPassword);
            user.ModifiedAt = DateTime.UtcNow;

            var result = await _userRepo.UpdateAsync(user);

            if (result > 0)
            {
                return new ResponseApi
                {
                    ErrCode = 200,
                    ErrDesc = "Đổi mật khẩu thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 500,
                    ErrDesc = "Có lỗi xảy ra, vui lòng thử lại"
                };
            }
        }



        public async Task<User?> FindUserByUserNameAsync(string email) => await _userRepo.FindUserByUserNameAsync(email);

        public async Task<ResponseApi> LoginAsync(LoginDTO dto)
        {
            var getUser = await _userRepo.FindUserByUserNameAsync(dto.Username!);

            if (getUser == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy người dùng"
                };
            }

            if (getUser.IsLocked)
            {
                return new ResponseApi
                {
                    ErrCode = 403,
                    ErrDesc = "Tài khoản của bạn đã bị khóa"
                };
            }

            var checkPassword = BCrypt.Net.BCrypt.Verify(dto.Password, getUser.Password);

            if (!checkPassword)
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    ErrDesc = "Tên đăng nhập hoặc mật khẩu không chính xác. Vui lòng thử lại"
                };
            }

            var token = _tokenService.GenerateJWTWithUser(getUser);

            return new ResponseApi
            {
                ErrCode = 200,
                Data = new
                {
                    Token = token,
                    UserId = getUser.Id,
                    FullName = getUser.FullName,
                    Role = getUser.UserRole.RoleName
                },
                ErrDesc = "Đăng nhập thành công"
            };
        }

        public async Task<ResponseApi> DisableUserAsync(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);

            if (user == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy người dùng"
                };
            }

            if (user.IsLocked)
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    ErrDesc = "Người dùng này đã bị khóa trước đó"
                };
            }

            user.IsDeleted = true;
            await _userRepo.UpdateAsync(user);

            return new ResponseApi
            {
                ErrCode = 200,
                ErrDesc = "Khóa người dùng thành công"
            };
        }


        public async Task<ResponseApi> UpdateAsync(int id, UserDTO dto, IFormFile file)
        {
            var imageUrl = await _imageService.UploadImageAsync(file);
            var user = await _userRepo.GetByIdAsync(id);
            if (user == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy  người dùng"
                };
            }
            var validationResult = await _validator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new DataInvalidException(string.Join(", ", errors));
            }

            user.DateOfBirth = (DateTime)dto.DateOfBirth!;
            user.FullName = dto.FullName;
            user.Address = dto.Address;
            user.Gender = (AccessControllSystem.Domain.Enum.Gender)dto.Gender;
            user.PhoneNumber = dto.PhoneNumber;
            user.ModifiedAt = DateTime.UtcNow;
            user.ImageUrl = imageUrl;
            if (dto.UserRoleId != null)
            {
                var userRole = await _userRoleRepository.GetByIdAsync((int)dto.UserRoleId);
                if (userRole == null)
                {
                    return new ResponseApi
                    {
                        ErrCode = 404,
                        ErrDesc = "Không tìm thấy vai trò người dùng"
                    };
                }
                user.UserRoleId = userRole.Id;
            }


            var res = _mapper.Map<UserResponse>(user);
            var result = await _userRepo.UpdateAsync(user);

            if (result > 0)
            {
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Cập nhật người dùng thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Cập nhật không thành công"
                };
            }

        }

    }
}
