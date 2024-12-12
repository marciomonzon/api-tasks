using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class ProfileMappings : Profile
    {
        public ProfileMappings()
        {
            CreateMap<CreateUserCommand, User>()
                .ForMember(x => x.RefreshToken, x => x.AllowNull())
                .ForMember(x => x.RefreshTokenExpirationTime, x => x.MapFrom(x => AddFiveDays()))
                .ForMember(x => x.PasswordHash, x => x.MapFrom(x => x.Password));

            CreateMap<User, UserInfoViewModel>()
                .ForMember(x => x.TokenJwt, x => x.AllowNull());
        }

        private DateTime AddFiveDays()
        {
            return DateTime.Now.AddDays(10);
        }
    }
}
