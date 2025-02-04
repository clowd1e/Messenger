using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.Persistence.Options
{
    public sealed class DbSettings
    {
        public const string SectionName = nameof(DbSettings);

        [Required]
        public string? ConnectionString { get; set; }
    }
}
