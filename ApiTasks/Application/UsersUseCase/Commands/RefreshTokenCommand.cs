using Application.Response;
using Application.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UsersUseCase.Commands
{
    public record RefreshTokenCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }
    }
}
