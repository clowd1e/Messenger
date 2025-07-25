﻿using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;

namespace Messenger.Application.Features.Chats.Queries.GetById
{
    internal sealed class GetChatByIdQueryHandler
        : IQueryHandler<GetChatByIdQuery, ChatResponse>
    {
        private readonly IChatRepository _chatRepository;
        private readonly Mapper<Chat, ChatResponse> _chatMapper;

        public GetChatByIdQueryHandler(
            IChatRepository chatRepository,
            Mapper<Chat, ChatResponse> chatMapper)
        {
            _chatRepository = chatRepository;
            _chatMapper = chatMapper;
        }

        public async Task<Result<ChatResponse>> Handle(
            GetChatByIdQuery request,
            CancellationToken cancellationToken)
        {
            var chat = await _chatRepository.GetByIdWithUsersAsync(
                chatId: new(request.ChatId), cancellationToken);

            if (chat is null)
            {
                return Result.Failure<ChatResponse>(ChatErrors.NotFound);
            }

            return _chatMapper.Map(chat);
        }
    }
}
