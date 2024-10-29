using Messenger.Domain.Aggregates.PrivateChat.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.PrivateChat
{
    public sealed class PrivateChat : AggregateRoot<PrivateChatId>
    {
        private PrivateChatCreationDate _creationDate;

        private PrivateChat()
            : base(new(Guid.NewGuid())) { }

        private PrivateChat(
            PrivateChatId privateChatId,
            PrivateChatCreationDate creationDate) : base(privateChatId)
        {
            CreationDate = creationDate;
        }

        public PrivateChatCreationDate CreationDate
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
            PrivateChatCreationDate creationDate)
        {
            return new PrivateChat(
                privateChatId,
                creationDate);
        }
    }
}
