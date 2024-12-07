using Application.UsersUseCase.Commands;
using Application.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UsersUseCase.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserInfoViewModel>
    {
        public Task<UserInfoViewModel> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
