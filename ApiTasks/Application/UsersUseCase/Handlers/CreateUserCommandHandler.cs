using Application.Response;
using Application.Services.Interfaces;
using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;

namespace Application.UsersUseCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel>>
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

        public async Task<ResponseBase<UserInfoViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request);

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var userMapped = _mapper.Map<UserInfoViewModel>(user);
            userMapped.RefreshToken = _authService.GenerateJWT(user.Email!, user.Username!);

            var userInfo = new ResponseBase<UserInfoViewModel>()
            {
                ResponseInfo = null,
                Value = userMapped
            };

            return userInfo;
        }
    }
}
