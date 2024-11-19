using FluentValidation;
using Messenger.Application.Extensions.Validation;

namespace Messenger.Application.Features.Auth.Commands.Register
{
    internal sealed class RegisterCommandValidator
        : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MaximumLength(30);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(50)
                .EmailAddress();

            RuleFor(x => x.Password)
                .ValidatedPassword();
        }
    }
}
