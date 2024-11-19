using FluentValidation;

namespace Messenger.Application.Extensions.Validation
{
    public static class PasswordValidationExtensions
    {
        public static IRuleBuilderOptions<T, string?> ValidatedPassword<T>(this IRuleBuilder<T, string?> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .MaximumLength(30).WithMessage("Password must not be longer than 30 characters.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

            return options;
        }
    }
}
