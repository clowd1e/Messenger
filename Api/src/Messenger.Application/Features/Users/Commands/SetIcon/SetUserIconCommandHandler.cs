using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Abstractions.Storage;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Messenger.Application.Features.Users.Commands.SetIcon
{
    internal sealed class SetUserIconCommandHandler
        : ICommandHandler<SetUserIconCommand>
    {
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;

        public SetUserIconCommandHandler(
            IImageService imageService,
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            IUnitOfWork unitOfWork)
        {
            _imageService = imageService;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            SetUserIconCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            var iconDimensionsValid = IconDimensionsValid(request.Icon!);

            if (!iconDimensionsValid)
            {
                return UserErrors.InvalidIconDimensions;
            }

            if (user.IconUri is not null)
            {
                await _imageService.DeleteImageAsync(
                    user.IconUri, cancellationToken);
            }

            var iconUri = await _imageService.UploadImageAsync(
                request.Icon, cancellationToken);

            user.SetIconUri(iconUri);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        private bool IconDimensionsValid(IFormFile icon)
        {
            (int width, int height) = GetImageDimensions(icon);

            return width == height;
        }

        private (int Width, int Height) GetImageDimensions(IFormFile file)
        {
            using var stream = file.OpenReadStream();

            using var image = Image.Load(stream);

            return (image.Width, image.Height);
        }
    }
}
