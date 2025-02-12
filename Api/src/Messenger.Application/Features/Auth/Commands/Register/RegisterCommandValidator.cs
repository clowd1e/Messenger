using FluentValidation;
using Messenger.Application.Extensions.Validation;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.Register
{
    internal sealed class RegisterCommandValidator
        : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .MinimumLength(Username.MinLength)
                .MaximumLength(Username.MaxLength)
                .Matches("^[a-zA-Z0-9]+$").WithMessage("Username can only contain letters or digits");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(Name.MinLength)
                .MaximumLength(Name.MaxLength);

            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(Email.MaxLength)
                .EmailAddress();

            RuleFor(x => x.Password)
                .ValidatedPassword();
        }
    }
}
