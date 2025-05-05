using FluentValidation;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery
{
    internal sealed class RequestPasswordRecoveryCommandValidator
        : AbstractValidator<RequestPasswordRecoveryCommand>
    {
        public RequestPasswordRecoveryCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(Email.MaxLength)
                .EmailAddress();
        }
    }
}
