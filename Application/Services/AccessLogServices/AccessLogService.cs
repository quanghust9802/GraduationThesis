using Application.AuthProvide;
using Application.Common;
using Application.DTOs.AccessLogDTOs;
using Application.IRepositories;
using Application.Services.AccessLogServices;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Services.AccessRequestServices
{
    public class AccessLogService : IAccessLogService
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IAccessLogRepository _accessLogRepository;
        public AccessLogService(IUserRepository userRepo, ITokenService tokenService, IMapper mapper, IUserRoleRepository userRoleRepository, IAccessRequestrRepository accessRequestRepository, IAccessLogRepository accessLogRepostiory
            )
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userRoleRepository = userRoleRepository;
            _accessLogRepository = accessLogRepostiory;
        }
        public async Task<AccessLogResponse> GetByIdAsync(int id)
        {
            var entity = await _accessLogRepository.GetByIdAsync(id) ?? throw new NotFoundException();
            var dto = _mapper.Map<AccessLogResponse>(entity);
            return dto;

        }

        public async Task<ResponseApi> GetByUserIdAsync(int userId)
        {
            var entity = await _accessLogRepository.GetByUserId(userId);
            var dto = _mapper.Map<List<AccessLogResponse>>(entity);
            return new ResponseApi
            {
                Data = dto,
                ErrCode = 200
            };

        }

        public async Task<ResponseApi> GetByRequestIdAsync(int requestId)
        {
            var entity = await _accessLogRepository.GetByRequestId(requestId);
            var dto = _mapper.Map<List<AccessLogResponse>>(entity);
            return new ResponseApi
            {
                Data = dto,
                ErrCode = 200
            };

        }
        public async Task<ResponseApi> GetAllAsync()
        {
            var users = await _accessLogRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<AccessLogResponse>>(users);
            return new ResponseApi
            {
                Data = dtos,
                ErrCode = 200,
            };
        }

        public async Task<ResponseApi> InsertAsync(AccessLogDTO dto)
        {
            var accessLog = _mapper.Map<AccessLogs>(dto);
            accessLog.CreatedAt = DateTime.Now;
            var result = await _accessLogRepository.InsertAsync(accessLog);

            var res = _mapper.Map<AccessLogResponse>(accessLog);
            if (result > 0)
            {
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Thêm mới log thành công"
                };
            }
            else
            {
                return new ResponseApi
                {
                    ErrCode = 400,
                    Data = null,
                    ErrDesc = "Thêm mói log không thành công"
                };
            }

        }

        public async Task<ResponseApi> DeleteAsync(int id)
        {
            var accessRequest = await _accessLogRepository.GetByIdAsync(id);
            if (accessRequest == null)
            {
                return new ResponseApi
                {
                    ErrCode = 404,
                    ErrDesc = "Không tìm thấy log"
                };
            }

            var result = await _accessLogRepository.DeleteAsync(accessRequest);

            if (result > 0)
            {
                var res = _mapper.Map<AccessLogResponse>(accessRequest);
                return new ResponseApi
                {
                    ErrCode = 200,
                    Data = res,
                    ErrDesc = "Xóa log thành công"
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

        public async Task<ResponseApi> GetByFilterAsync(DateTime? startDate, DateTime? endDate, int? requestId)
        {
            var data = await _accessLogRepository.GetByFilterAsync(startDate, endDate, requestId);
            var dto = _mapper.Map<List<AccessLogResponse>>(data);
            return new ResponseApi
            {
                Data = dto,
                ErrCode = 200
            };
        }


    }
}
