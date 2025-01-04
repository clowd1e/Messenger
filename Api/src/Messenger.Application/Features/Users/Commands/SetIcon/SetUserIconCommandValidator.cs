using FluentValidation;
using Messenger.Application.Images.Options;
using Microsoft.Extensions.Options;

namespace Messenger.Application.Features.Users.Commands.SetIcon
{
    internal sealed class SetUserIconCommandValidator
        : AbstractValidator<SetUserIconCommand>
    {
        private const int MaxIconLengthInBytes = 250_000;
        
        private readonly ImageSettings _imageSettings;

        public SetUserIconCommandValidator(
            IOptions<ImageSettings> imageSettings)
        {
            _imageSettings = imageSettings.Value;

            RuleFor(x => x.Icon)
                .NotEmpty()
                .WithMessage("Icon is required.");

            RuleFor(x => x.Icon.ContentType)
                .Must(ValidImageType)
                .When(x => x.Icon is not null)
                .WithMessage($"Icon format should be one of {GetAllowedContentTypes()}.");

            RuleFor(x => x.Icon.Length)
                .GreaterThan(0)
                .LessThanOrEqualTo(MaxIconLengthInBytes)
                .When(x => x.Icon is not null)
                .WithMessage("Icon size is invalid.");
        }

        private bool ValidImageType(string contentType) =>
            _imageSettings.AllowedContentTypes.Contains(contentType);

        private string GetAllowedContentTypes() =>
            string.Join(", ", _imageSettings.AllowedContentTypes
                .Select(type => $".{type.Split('/').Last()}"));
    }
}
