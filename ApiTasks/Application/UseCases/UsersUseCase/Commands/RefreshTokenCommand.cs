using Application.Response;
using Application.UseCases.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UseCases.UsersUseCase.Commands
{
    public record RefreshTokenCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string? Username { get; set; }
        public string? RefreshToken { get; set; }
    }
}
