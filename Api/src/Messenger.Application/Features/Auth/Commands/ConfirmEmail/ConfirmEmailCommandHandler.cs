using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Commands.ConfirmEmail
{
    internal sealed class ConfirmEmailCommandHandler
        : IRequestHandler<ConfirmEmailCommand, Result>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmEmailCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ConfirmEmailCommand request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(request.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            if (user.EmailConfirmed)
            {
                return UserErrors.EmailAlreadyConfirmed;
            }

            var identityUser = await _identityService.GetByEmailAsync(user.Email);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            var result = user.ConfirmEmail();

            if (result.IsFailure)
            {
                return result.Error;
            }

            var unescapedToken = Uri.UnescapeDataString(request.Token);

            var identityResult = await _identityService.ConfirmEmailAsync(identityUser, unescapedToken);

            if (identityResult.IsFailure)
            {
                return identityResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
