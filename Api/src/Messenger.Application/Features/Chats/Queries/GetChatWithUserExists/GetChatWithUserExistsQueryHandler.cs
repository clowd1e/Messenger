using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Queries.GetChatWithUserExists
{
    internal sealed class GetChatWithUserExistsQueryHandler
        : IQueryHandler<GetChatWithUserExistsQuery, ChatExistsResponse>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserContextService<Guid> _userContextService;

        public GetChatWithUserExistsQueryHandler(
            IChatRepository chatRepository,
            IUserContextService<Guid> userContextService)
        {
            _chatRepository = chatRepository;
            _userContextService = userContextService;
        }

        public async Task<Result<ChatExistsResponse>> Handle(
            GetChatWithUserExistsQuery query,
            CancellationToken cancellationToken)
        {
            var currentUserId = new UserId(_userContextService.GetAuthenticatedUserId());
            var queryUserId = new UserId(query.UserId);

            var chat = await _chatRepository.GetChatBetweenUsersAsync(
                currentUserId,
                queryUserId,
                cancellationToken);

            var result = chat is null ?
                new ChatExistsResponse(null) :
                new ChatExistsResponse(chat.Id.Value);

            return result;
        }
    }
}
