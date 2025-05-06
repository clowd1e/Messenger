using FluentValidation;

namespace Messenger.Application.Features.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandValidator
        : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.TokenId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();
        }
    }
}
