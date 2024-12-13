using Application.Response;
using Application.Services.Interfaces;
using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;
using MediatR;

namespace Application.UsersUseCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseBase<RefreshTokenViewModel>>
    {
        private readonly TasksDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public CreateUserCommandHandler(TasksDbContext dbContext,
                                        IMapper mapper,
                                        IAuthService authService)
        {
            _dbContext = dbContext;
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

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

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
