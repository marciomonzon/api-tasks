using Application.Response;
using Application.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UsersUseCase.Commands
{
    public record LoginUserCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
