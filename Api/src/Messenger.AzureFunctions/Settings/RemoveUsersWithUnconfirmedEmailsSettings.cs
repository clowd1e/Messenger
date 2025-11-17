using System.ComponentModel.DataAnnotations;

namespace Messenger.AzureFunctions.Settings
{
    public sealed class RemoveUsersWithUnconfirmedEmailsSettings
    {
        public const string SectionName = nameof(RemoveUsersWithUnconfirmedEmailsSettings);

        [Required]
        public required int RegistrationLookbackDays { get; set; }
    }
}
