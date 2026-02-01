using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Queries.GetById
{
    internal sealed class GetUserByIdQueryHandler
        : IQueryHandler<GetUserByIdQuery, ShortUserResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, ShortUserResponse> _userMapper;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            Mapper<User, ShortUserResponse> userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public async Task<Result<ShortUserResponse>> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                userId: new(request.UserId), cancellationToken);

            if (user is null)
            {
                return Result.Failure<ShortUserResponse>(UserErrors.NotFound);
            }

            return _userMapper.Map(user);
        }
    }
}
