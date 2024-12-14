using Application.Response;
using Application.Services.Interfaces;
using Application.Services;
using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UsersUseCase.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public LoginUserCommandHandler(IAuthService authService, 
                                       IConfiguration configuration,
                                       IMapper mapper)
        {
            _authService = authService;
            _configuration = configuration;
            _mapper = mapper;
        }

        public Task<ResponseBase<RefreshTokenViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.Get(x => x.Email == request.Email);

            if (user is null)
            {
                return new ResponseBase<RefreshTokenViewModel>()
                {
                    ResponseInfo = new()
                    {
                        Title = "Usuário não encontrado",
                        ErrorDescription = "Nenhum usuário encontrado com o email informado",
                        HttpStatus = 404
                    },
                    Value = null
                };
            }

            var hashPasswordRequest = _authService.HashingPassword(request.Password!);

            if (hashPasswordRequest != user.PasswordHash)
            {
                return new ResponseBase<RefreshTokenViewModel>()
                {
                    ResponseInfo = new()
                    {
                        Title = "Senha incorreta",
                        ErrorDescription = "A senha informada está incorreta.",
                        HttpStatus = 404
                    },
                    Value = null
                };
            }

            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = _authService.GenerateRefreshToken();
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Commit();

            RefreshTokenViewModel refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.Username!);

            return new ResponseBase<RefreshTokenViewModel>()
            {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
    }
}
