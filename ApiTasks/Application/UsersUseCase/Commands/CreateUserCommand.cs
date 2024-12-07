using Application.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UsersUseCase.Commands
{
    public record CreateUserCommand : IRequest<UserInfoViewModel>
    {
    }
}
