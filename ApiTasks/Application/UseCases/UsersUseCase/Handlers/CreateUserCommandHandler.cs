using Application.Interfaces.UnitOfWork;
using Application.Response;
using Application.Services.Interfaces;
using Application.UseCases.UsersUseCase.Commands;
using Application.UseCases.UsersUseCase.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.UseCases.UsersUseCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork,
                                        IMapper mapper,
                                        IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<ResponseBase<RefreshTokenViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await ValidateUniqueUsernameAndEmailAsync(request);
            if (validationResult is not null)
                return validationResult;

            var user = _mapper.Map<User>(request);
            user.RefreshToken = _authService.GenerateRefreshToken();
            user.PasswordHash = _authService.HashingPassword(user.PasswordHash!);

            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.CommitAsync();

            var userMapped = _mapper.Map<RefreshTokenViewModel>(user);
            userMapped.TokenJwt = _authService.GenerateJWT(user.Email!, user.Username!);

            var userInfo = new ResponseBase<RefreshTokenViewModel>()
            {
                ResponseInfo = null,
                Value = userMapped
            };

            return userInfo;
        }

        private async Task<ResponseBase<RefreshTokenViewModel>> ValidateUniqueUsernameAndEmailAsync(CreateUserCommand request)
        {
            var isUniqueEmailAndUsername = await _authService.UniqueEmailAndUsernameAsync(request.Email!, request.Username!);

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameAndEmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Username e Email indisponíveis.",
                        ErrorDescription = "O username e o email apresentados já estão sendo utilizados. Tente outros.",
                        HttpStatus = 400
                    },
                    Value = null!
                };
            }

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.EmailUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Email indisponível.",
                        ErrorDescription = "O Email apresentado já está sendo utilizado. Tente outro.",
                        HttpStatus = 400
                    },
                    Value = null!
                };
            }

            if (isUniqueEmailAndUsername is ValidationFieldsUserEnum.UsernameUnavailable)
            {
                return new ResponseBase<RefreshTokenViewModel>
                {
                    ResponseInfo = new()
                    {
                        Title = "Username indisponível.",
                        ErrorDescription = "O username apresentado já está sendo utilizado. Tente outro.",
                        HttpStatus = 400
                    },
                    Value = null!
                };
            }

            return null!;
        }
    }
}
