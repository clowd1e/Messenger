using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Application.Helpers;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Queries.GetCurrentUserChatsPaginated
{
    internal sealed class GetCurrentUserChatsPaginatedQueryHandler
        : IRequestHandler<GetCurrentUserChatsPaginatedQuery, Result<PaginatedChatsResponse>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<Chat, ShortChatResponse> _chatMapper;

        public GetCurrentUserChatsPaginatedQueryHandler(
            IUserContextService<Guid> userContextService,
            IChatRepository chatRepository,
            IUserRepository userRepository,
            Mapper<Chat, ShortChatResponse> chatMapper)
        {
            _userContextService = userContextService;
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _chatMapper = chatMapper;
        }

        public async Task<Result<PaginatedChatsResponse>> Handle(
            GetCurrentUserChatsPaginatedQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(userId, cancellationToken);

            if (!userExists)
            {
                throw new AuthenticatedUserNotFoundException();
            }

            var chats = await _chatRepository.GetUserChatsPaginated(
                userId,
                request.Page,
                request.PageSize,
                request.RetrievalCutoff,
                cancellationToken);

            var chatResponses = _chatMapper.Map(chats);

            var totalUserChats = await _chatRepository.CountUserChatsAsync(
                userId,
                request.RetrievalCutoff,
                cancellationToken);

            var isLastPage = PaginationCalculator.IsLastPage(
                request.Page,
                request.PageSize,
                totalUserChats);

            return new PaginatedChatsResponse(chatResponses, isLastPage);
        }
    }
}
