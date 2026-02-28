using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.Responses;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Queries.GetById
{
    internal sealed class GetChatByIdQueryHandler
        : IQueryHandler<GetChatByIdQuery, ChatResponse>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<Chat, ChatResponse> _chatMapper;

        public GetChatByIdQueryHandler(
            IChatRepository chatRepository,
            IUserContextService<Guid> userContextService,
            Mapper<Chat, ChatResponse> chatMapper)
        {
            _chatRepository = chatRepository;
            _userContextService = userContextService;
            _chatMapper = chatMapper;
        }

        public async Task<Result<ChatResponse>> Handle(
            GetChatByIdQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var chat = await _chatRepository.GetByIdWithUsersAndLastMessageAsync(
                requestingUserId: userId,
                chatId: new(request.ChatId), cancellationToken);

            if (chat is null)
            {
                return Result.Failure<ChatResponse>(ChatErrors.NotFound);
            }

            return _chatMapper.Map(chat);
        }
    }
}
