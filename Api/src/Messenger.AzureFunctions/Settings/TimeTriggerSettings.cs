using System.ComponentModel.DataAnnotations;

namespace Messenger.AzureFunctions.Settings
{
    public sealed class TimeTriggerSettings
    {
        public const string SectionName = nameof(TimeTriggerSettings);

        [Required]
        [CronExpression]
        public required string RefreshTokenCleanupSchedule { get; set; }
    }
}
