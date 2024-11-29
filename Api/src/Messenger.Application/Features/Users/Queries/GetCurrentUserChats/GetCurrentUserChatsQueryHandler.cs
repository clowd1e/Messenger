using MediatR;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Application.Abstractions.Data;
using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Users.Queries.GetCurrentUserChats
{
    internal sealed class GetCurrentUserChatsQueryHandler
        : IRequestHandler<GetCurrentUserChatsQuery, Result<IEnumerable<ChatResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<Chat, ChatResponse> _chatMapper;

        public GetCurrentUserChatsQueryHandler(
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IUserContextService<Guid> userContextService,
            Mapper<Chat, ChatResponse> chatMapper)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _userContextService = userContextService;
            _chatMapper = chatMapper;
        }

        public async Task<Result<IEnumerable<ChatResponse>>> Handle(
            GetCurrentUserChatsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(userId, cancellationToken);

            if (!userExists)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            var chats = await _chatRepository.GetUserChats(userId, cancellationToken);

            return Result.Success(_chatMapper.Map(chats));
        }
    }
}
