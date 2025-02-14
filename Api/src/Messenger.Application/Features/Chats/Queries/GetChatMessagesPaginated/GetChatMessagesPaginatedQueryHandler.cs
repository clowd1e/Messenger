using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Helpers;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated
{
    internal sealed class GetChatMessagesPaginatedQueryHandler
        : IRequestHandler<GetChatMessagesPaginatedQuery, Result<PaginatedMessagesResponse>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Message, MessageResponse> _messageMapper;

        public GetChatMessagesPaginatedQueryHandler(
            IUserContextService<Guid> userContextService,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            Mapper<Message, MessageResponse> messageMapper)
        {
            _userContextService = userContextService;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _messageMapper = messageMapper;
        }

        public async Task<Result<PaginatedMessagesResponse>> Handle(
            GetChatMessagesPaginatedQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(userId, cancellationToken);

            if (!userExists)
            {
                throw new AuthenticatedUserNotFoundException();
            }

            var chatId = new ChatId(request.ChatId);

            var chatExists = await _chatRepository.ExistsAsync(
                chatId,
                cancellationToken);

            if (!chatExists)
            {
                return Result.Failure<PaginatedMessagesResponse>(ChatErrors.NotFound);
            }

            var isUserInChat = await _chatRepository.IsUserInChatAsync(
                userId,
                chatId,
                cancellationToken);

            if (!isUserInChat)
            {
                return Result.Failure<PaginatedMessagesResponse>(ChatErrors.UserNotInChat);
            }

            var messages = await _chatRepository.GetChatMessagesPaginated(
                chatId,
                request.Page,
                request.PageSize,
                request.RetrievalCutoff,
                cancellationToken);

            var messagesMap = _messageMapper.Map(messages);

            var totalChatMessages = await _chatRepository.CountChatMessagesAsync(
                chatId,
                request.RetrievalCutoff,
                cancellationToken);

            var isLastPage = PaginationCalculator.IsLastPage(
                request.Page,
                request.PageSize,
                totalChatMessages);

            return new PaginatedMessagesResponse(messagesMap, isLastPage);
        }
    }
}
