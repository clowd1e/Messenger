using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Queries.GetById
{
    internal sealed class GetUserByIdQueryHandler
        : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, UserResponse> _userMapper;

        public GetUserByIdQueryHandler(
            IUserRepository userRepository,
            Mapper<User, UserResponse> userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public async Task<Result<UserResponse>> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                userId: new(request.UserId.Value), cancellationToken);

            if (user is null)
            {
                return Result.Failure<UserResponse>(UserErrors.NotFound);
            }

            return _userMapper.Map(user);
        }
    }
}
