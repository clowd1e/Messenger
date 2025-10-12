using System.ComponentModel.DataAnnotations;

namespace Messenger.AzureFunctions.Settings
{
    public sealed class CronExpressionAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string cronExpression || string.IsNullOrWhiteSpace(cronExpression))
            {
                return new ValidationResult("Cron expression must be a non-empty string.");
            }

            var parts = cronExpression.Split(' ');

            if (parts.Length < 5 || parts.Length > 6)
            {
                return new ValidationResult("Cron expression must have 5 or 6 parts.");
            }

            foreach (var part in parts)
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    return new ValidationResult("Each part of the cron expression must be non-empty.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
