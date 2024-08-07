using AutoMapper;
using chat.Server.models;
using chat.Server.models.dtos;
using FluentValidation;

namespace chat.Server.mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<LoginUserDtoApi, User>();
            CreateMap<RegisterUserDtoApi, User>();
        }
    }
}
