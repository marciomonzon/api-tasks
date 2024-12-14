using Application.Interfaces.UnitOfWork;
using Application.Response;
using Application.Services.Interfaces;
using Application.UseCases.UsersUseCase.Commands;
using Application.UseCases.UsersUseCase.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.UseCases.UsersUseCase.Handlers
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(IAuthService authService,
                                          IConfiguration configuration,
                                          IMapper mapper,
                                          IUnitOfWork unitOfWork)
        {
            _authService = authService;
            _configuration = configuration;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(x => x.Username == request.Username);

            if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpirationTime < DateTime.Now)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Token inválido",
                        ErrorDescription = $"Refresh Token inválido ou expirado. Faça login novamente.",
                        HttpStatus = 400
                    },
                    Value = null!
                };
            }

            user.RefreshToken = _authService.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenExpirationTimeInDays"], out int refreshTokenExpirationTimeInDays);
            user.RefreshTokenExpirationTime = DateTime.Now.AddDays(refreshTokenExpirationTimeInDays);

            _unitOfWork.Commit();

            var refreshTokenVM = _mapper.Map<RefreshTokenViewModel>(user);
            refreshTokenVM.TokenJwt = _authService.GenerateJWT(user.Email!, user.Username!);

            return new ResponseBase<RefreshTokenViewModel>
            {
                ResponseInfo = null,
                Value = refreshTokenVM
            };
        }
    }
}
