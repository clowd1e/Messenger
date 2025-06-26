﻿using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Queries.GetById
{
    public sealed record GetChatByIdQuery(
        Guid ChatId) : IQuery<ChatResponse>;
}
