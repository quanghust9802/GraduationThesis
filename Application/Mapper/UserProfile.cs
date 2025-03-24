using Application.DTOs.AccessLogDTOs;
using Application.DTOs.AccessRequestDTOs;
using Application.DTOs.AuthDTOs;
using Application.DTOs.NotificationDTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserDTO, User>();
            CreateMap<AccessRequest, AccessRequestDTO>().ReverseMap();
            CreateMap<Notification, NotificationResponse>().ReverseMap();
            CreateMap<AccessLogDTO, AccessLogs>();
            CreateMap<AccessLogs, AccessLogResponse>();
            CreateMap<UserRole, UserRoleReponse>();
        }
    }
}
