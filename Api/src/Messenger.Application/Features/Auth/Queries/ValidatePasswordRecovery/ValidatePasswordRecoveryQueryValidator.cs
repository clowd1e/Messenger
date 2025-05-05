using FluentValidation;

namespace Messenger.Application.Features.Auth.Queries.ValidatePasswordRecovery
{
    internal sealed class ValidatePasswordRecoveryQueryValidator
        : AbstractValidator<ValidatePasswordRecoveryQuery>
    {
        public ValidatePasswordRecoveryQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.TokenId)
                .NotEmpty();
        }
    }
}
