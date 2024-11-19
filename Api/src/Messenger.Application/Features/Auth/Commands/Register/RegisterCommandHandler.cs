using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Identity;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Auth.Commands.Register
{
    internal sealed class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<RegisterCommand, Result<User>> _commandMapper;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            Mapper<RegisterCommand, Result<User>> commandMapper,
            Mapper<User, ApplicationUser> userMapper,
            IUnitOfWork unitOfWork)
        {
            _identityService = identityService;
            _userRepository = userRepository;
            _commandMapper = commandMapper;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var user = mappingResult.Value;

            var userExists = await _userRepository.ExistsAsync(
                user.Username,
                user.Email,
                cancellationToken);

            if (userExists)
            {
                return UserErrors.UserWithSameCredentialsAlreadyExists;
            }

            var identityUser = _userMapper.Map(user);

            await _userRepository.InsertAsync(user, cancellationToken);

            await _identityService.CreateAsync(identityUser, request.Password);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
