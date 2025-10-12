using System.ComponentModel.DataAnnotations;

namespace Messenger.Infrastructure.Services.Security.Options
{
    public sealed class HashSettings
    {
        public const string SectionName = nameof(HashSettings);

        [Required]
        [Range(1, 20)]
        public int WorkFactor { get; set; }
    }
}
