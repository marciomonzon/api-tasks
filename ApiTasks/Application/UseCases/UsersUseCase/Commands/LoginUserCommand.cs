using Application.Response;
using Application.UseCases.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UseCases.UsersUseCase.Commands
{
    public record LoginUserCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
