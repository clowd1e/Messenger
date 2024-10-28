using Messenger.Domain.Aggregates.PrivateChat.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.PrivateChat
{
    public sealed class PrivateChat : AggregateRoot<PrivateChatId>
    {
        private CreationDate _creationDate;

        private PrivateChat()
            : base(new(Guid.NewGuid())) { }

        private PrivateChat(
            PrivateChatId privateChatId,
            CreationDate creationDate) : base(privateChatId)
        {
            CreationDate = creationDate;
        }

        public CreationDate CreationDate
        {
            get => _creationDate;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _creationDate = value;
            }
        }

        public static Result<PrivateChat> Create(
            PrivateChatId privateChatId,
            CreationDate creationDate)
        {
            return new PrivateChat(
                privateChatId,
                creationDate);
        }
    }
}
