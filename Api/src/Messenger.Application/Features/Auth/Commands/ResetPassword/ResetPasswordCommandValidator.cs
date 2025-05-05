using FluentValidation;
using Messenger.Application.Extensions.Validation;

namespace Messenger.Application.Features.Auth.Commands.ResetPassword
{
    internal sealed class ResetPasswordCommandValidator
        : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.TokenId)
                .NotEmpty();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .ValidatedPassword();
        }
    }
}
