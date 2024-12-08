using Application.Response;
using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;

namespace Application.UsersUseCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseBase<UserInfoViewModel>>
    {
        private readonly TasksDbContext _dbContext;

        public CreateUserCommandHandler(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResponseBase<UserInfoViewModel>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Name = request.Name,
                Email = request.Email,
                Surname = request.Surname,
                Username = request.Username,
                PasswordHash = request.Password,
                RefreshToken = Guid.NewGuid().ToString(),
                RefreshTokenExpirationTime = DateTime.Now.AddDays(5)
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            var userInfo = new ResponseBase<UserInfoViewModel>()
            {
                ResponseInfo = null,
                Value = new()
                {
                    Name = user.Name,
                    Email = user.Email,
                    Surname = user.Surname,
                    Username = user.Username,
                    RefreshToken = user.RefreshToken,
                    RefreshTokenExpirationTime = user.RefreshTokenExpirationTime,
                    TokenJwt = Guid.NewGuid().ToString()
                }
            };

            return userInfo;
        }
    }
}
