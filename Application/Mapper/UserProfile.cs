using Application.DTOs.AuthDTOs;
using AutoMapper;
using Domain.Entities;

namespace Backend.Application.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserDTO, User>();
        }
    }
}
