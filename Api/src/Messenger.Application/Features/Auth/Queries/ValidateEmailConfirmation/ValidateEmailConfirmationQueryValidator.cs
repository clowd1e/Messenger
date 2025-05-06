using FluentValidation;

namespace Messenger.Application.Features.Auth.Queries.ValidateEmailConfirmation
{
    internal sealed class ValidateEmailConfirmationQueryValidator
        : AbstractValidator<ValidateEmailConfirmationQuery>
    {
        public ValidateEmailConfirmationQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();

            RuleFor(x => x.TokenId)
                .NotEmpty();
        }
    }
}
