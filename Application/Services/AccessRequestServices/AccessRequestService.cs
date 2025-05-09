using Application.AuthProvide;
using Application.Common;
using Application.DTOs.AccessRequestDTOs;
using Application.DTOs.AuthDTOs;
using Application.Hubs;
using Application.IRepositories;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.SignalR;

namespace Application.Services.AccessRequestServices
{
    public class AccessRequestService : IAccessRequestService
    {
        private readonly IUserRepository _userRepo;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDTO> _validator;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IAccessRequestrRepository _accessRequestRepo;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AccessRequestService(IHubContext<NotificationHub> hubContext, IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IValidator<UserDTO> validator, IUserRoleRepository userRoleRepository, IAccessRequestrRepository accessRequestRepository
            )
        {
            _userRepo = userRepo;
            _tokenService = tokenService;
            _mapper = mapper;
            _validator = validator;
            _userRoleRepository = userRoleRepository;
            _accessRequestRepo = accessRequestRepository;
        }
        public async Task<AccessRequestDTO> GetByIdAsync(int id)
        {
            var entity = await _accessRequestRepo.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<AccessRequestDTO>(entity);
            return dto;

        }

        public async Task<ResponseApi> GetAllAsync()
        {
            var users = await _accessRequestRepo.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<AccessRequestDTO>>(users);
            return new ResponseApi
            {
                Data = dtos,
                ErrCode = 200,
            };
        }

        public async Task<ResponseApi> InsertAsync(AccessRequestDTO dto)
        {
            var accessRequest = _mapper.Map<AccessRequest>(dto);
            accessRequest.CreatedAt = DateTime.Now;
            var result = await _accessRequestRepo.InsertAsync(accessRequest);

            var res = _mapper.Map<AccessRequestDTO>(accessRequest);

            //if (result > 0)
            //{
            //    var adminId = await _userRepo.GetAdminUserIdAsync();

            //    if (adminId != null)
            //    {
            //        var notification = new NotificationResponse
            //        {
            //            UserId = adminId.Value,
            //            Message = $"Có một yêu cầu truy cập mới từ người dùng ID: {accessRequest.UserRequestId}.",
            //            SendAt = DateTime.UtcNow
            //        };
            //        if (_hubContext == null)
            //        {
            //            throw new InvalidOperationException("HubContext is not initialized.");
            //        }

            //        await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification.Message);
            //    }

            //    return new ResponseApi
            //    {
            //        ErrCode = 200,
            //        Data = res,
            //        ErrDesc = "Thêm mới yêu cầu thành công"
            //    };
            //}
            //else
            if (result > 0)
            {
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Thêm mới yêu cầu không thành công"
                };
            }
            return new ResponseApi
            {
                ErrCode = 400,
                ErrDesc = "Thêm mới yêu cầu không thành công"
            };
        }


        public async Task<ResponseApi> UpdateAsync(int id, AccessRequestDTO dto)
        {
            var accessRequest = await _accessRequestRepo.GetByIdAsync(id);
            if (accessRequest == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy yêu cầu"
                };
            }

            _mapper.Map(dto, accessRequest);
            accessRequest.ModifiedAt = DateTime.Now;

            var result = await _accessRequestRepo.UpdateAsync(accessRequest);

            if (result > 0)
            {
                var res = _mapper.Map<AccessRequestDTO>(accessRequest);
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Cập nhật yêu cầu thành công"
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

        public async Task<ResponseApi> DeleteAsync(int id)
        {
            var accessRequest = await _accessRequestRepo.GetByIdAsync(id);
            if (accessRequest == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy yêu cầu"
                };
            }

            var result = await _accessRequestRepo.DeleteAsync(accessRequest);

            if (result > 0)
            {
                var res = _mapper.Map<AccessRequestDTO>(accessRequest);
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Xóa yêu cầu thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Xóa không thành công"
                };
            }
        }
        public async Task<ResponseApi> UpdateStatus(int id, int status)
        {
            var accessRequest = await _accessRequestRepo.GetByIdAsync(id);
            if (accessRequest == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy yêu cầu"
                };
            }

            var result = await _accessRequestRepo.UpdateStatus(id, status);

            if (result > 0)
            {
                // Lấy chuỗi trạng thái tương ứng
                //string statusMessage = status switch
                //{
                //    1 => "Chờ duyệt",
                //    2 => "Đã duyệt",
                //    3 => "Bị từ chối",
                //};

                //var notification = new NotificationResponse
                //{
                //    UserId = (int)accessRequest.UserRequestId,
                //    Message = $"Yêu cầu của bạn đã được cập nhật trạng thái: {statusMessage}",
                //    SendAt = DateTime.UtcNow
                //};
                //if (_hubContext == null)
                //{
                //    throw new InvalidOperationException("HubContext is not initialized.");
                //}
                //await _hubContext.Clients.User(notification.UserId.ToString()).SendAsync("ReceiveNotification", notification.Message);

                return new ResponseApi
                {
                    ErrCode = 200,
                    ErrDesc = "Cập nhật trạng thái yêu cầu thành công"
                };
            }
            return new ResponseApi
            {
                ErrCode = 400,
                ErrDesc = "Cập nhật trạng thái không thành công"
            };
        }



        public async Task<ResponseApi> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? status, int? userId)
        {
            var data = await _accessRequestRepo.GetByFilterAsync(startDate, endDate, status, userId);
            var dto = _mapper.Map<List<AccessRequestDTO>>(data);
            return new ResponseApi
            {
                Data = dto,
                ErrCode = 200
            };
        }

        public async Task<ResponseApi> GetByStatus(int? userId, int? status)
        {
            var data = await _accessRequestRepo.GetByStatus(userId, status);
            var dto = _mapper.Map<List<AccessRequestDTO>>(data);
            return new ResponseApi
            {
                Data = dto,
                ErrCode = 200
            };
        }

        public async Task<ResponseApi> VerifyInfor(string cccd)
        {
            var (accessRequest, mrz) = await _accessRequestRepo.VerifyInfor(cccd);

            if (accessRequest == null)
            {
                return new ResponseApi
                {
                    Data = null,
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy thông tin xác thực"
                };
            }

            var dto = _mapper.Map<AccessRequestDTO>(accessRequest);
            dto.Mrz = mrz;

            return new ResponseApi
            {
                Data = new List<AccessRequestDTO> { dto },
                ErrCode = 200,
                ErrDesc = "Tìm thấy thông tin xác thực"
            };
        }



    }
}
