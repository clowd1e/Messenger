using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Queries.GetAll
{
    internal sealed class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, UserResponse> _userMapper;

        public GetAllUsersQueryHandler(
            IUserRepository userRepository,
            Mapper<User, UserResponse> userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(cancellationToken);

            var usersMap = _userMapper.Map(users);

            return Result.Success(usersMap);
        }
    }
}
