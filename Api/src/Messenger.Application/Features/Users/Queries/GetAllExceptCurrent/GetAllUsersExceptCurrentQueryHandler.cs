using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Users.Queries.GetAllExceptCurrent
{
    internal sealed class GetAllUsersExceptCurrentQueryHandler
        : IRequestHandler<GetAllUsersExceptCurrentQuery, Result<IEnumerable<ShortUserResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<User, ShortUserResponse> _mapper;
        public GetAllUsersExceptCurrentQueryHandler(
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            Mapper<User, ShortUserResponse> mapper)
        {
            _userRepository = userRepository;
            _userContextService = userContextService;
            _mapper = mapper;
        }
        public async Task<Result<IEnumerable<ShortUserResponse>>> Handle(
            GetAllUsersExceptCurrentQuery request,
            CancellationToken cancellationToken)
        {
            var currentUserId = new UserId(_userContextService.GetAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(currentUserId, cancellationToken);

            if (!userExists)
            {
                throw new AuthenticatedUserNotFoundException();
            }

            var users = await _userRepository.GetAllExceptCurrentAsync(
                currentUserId,
                cancellationToken);

            var usersMap = _mapper.Map(users);

            return Result.Success(usersMap);
        }
    }
}
