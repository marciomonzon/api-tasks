using Application.Response;
using Application.UsersUseCase.ViewModels;
using MediatR;

namespace Application.UsersUseCase.Commands
{
    public record CreateUserCommand : IRequest<ResponseBase<RefreshTokenViewModel>>
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
    }
}
