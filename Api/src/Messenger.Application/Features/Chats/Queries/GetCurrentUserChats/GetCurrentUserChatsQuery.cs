﻿using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO;

namespace Messenger.Application.Features.Chats.Queries.GetCurrentUserChats
{
    public sealed record GetCurrentUserChatsQuery()
        : IQuery<IEnumerable<ChatResponse>>;
}
