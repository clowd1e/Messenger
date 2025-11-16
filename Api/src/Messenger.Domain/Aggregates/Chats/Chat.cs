using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Common.Timestamp;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Chats
{
    public abstract class Chat : AggregateRoot<ChatId>
    {
        protected readonly HashSet<Message> _messages = [];
        protected readonly HashSet<Users.User> _participants = [];
        protected Timestamp _creationDate;

        protected Chat()
            : base(new(Guid.NewGuid())) { }

        protected Chat(
            ChatId chatId,
            Timestamp creationDate) : base(chatId)
        {
            CreationDate = creationDate;
        }

        public Timestamp CreationDate
        {
            get => _creationDate;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _creationDate = value;
            }
        }

        public IReadOnlyCollection<Message> Messages => _messages;

        public IReadOnlyCollection<Users.User> Participants => _participants;

        public virtual Result AddMessage(Message message)
        {
            if (!Participants.Any(participant => message.User?.Id == participant.Id))
            {
                return ChatErrors.UserNotInChat;
            }

            _messages.Add(message);

            return Result.Success();
        }

        protected void SetParticipants(List<Users.User> participants)
        {
            foreach (var participant in participants)
            {
                _participants.Add(participant);
            }
        }
    }
}
