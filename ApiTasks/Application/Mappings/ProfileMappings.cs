using Application.UseCases.UsersUseCase.Commands;
using Application.UseCases.UsersUseCase.ViewModels;
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
                .ForMember(x => x.RefreshTokenExpirationTime, x => x.MapFrom(x => AddDays()))
                .ForMember(x => x.PasswordHash, x => x.MapFrom(x => x.Password));

            CreateMap<User, RefreshTokenViewModel>()
                .ForMember(x => x.TokenJwt, x => x.AllowNull());

            CreateMap<RefreshTokenViewModel, UserInfoViewModel>();
        }

        private DateTime AddDays()
        {
            return DateTime.Now.AddDays(10);
        }
    }
}
