using Application.UsersUseCase.Commands;
using FluentValidation;

namespace Application.UsersUseCase.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O campo email não pode ser vazio.")
                .EmailAddress().WithMessage("O campo email não é válido.");

            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("O campo username não pode ser vazio.");
        }
    }
}
