using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Queries.GetAll
{
    internal sealed class GetAllUsersQueryHandler
        : IQueryHandler<GetAllUsersQuery, IEnumerable<ShortUserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, ShortUserResponse> _userMapper;

        public GetAllUsersQueryHandler(
            IUserRepository userRepository,
            Mapper<User, ShortUserResponse> userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public async Task<Result<IEnumerable<ShortUserResponse>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            var usersMap = _userMapper.Map(users);

            return Result.Success(usersMap);
        }
    }
}
