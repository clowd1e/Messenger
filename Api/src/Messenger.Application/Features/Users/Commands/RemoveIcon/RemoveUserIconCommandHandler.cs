using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Abstractions.Storage;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Users.Commands.RemoveIcon
{
    internal sealed class RemoveUserIconCommandHandler
        : ICommandHandler<RemoveUserIconCommand>
    {
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveUserIconCommandHandler(
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
            RemoveUserIconCommand command,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            if (user.IconUri is null)
            {
                return UserErrors.IconNotSet;
            }

            await _imageService.DeleteImageAsync(
                user.IconUri, cancellationToken);

            user.RemoveIconUri();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
