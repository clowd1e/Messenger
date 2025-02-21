using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users
{
    public sealed class User : AggregateRoot<UserId>
    {
        private readonly HashSet<Chat> _chats = [];
        private readonly HashSet<Message> _messages = [];
        private Username _username;
        private Name _name;
        private Email _email;

        private User()
            : base(new(Guid.NewGuid())) { }

        private User(
            UserId userId,
            Username username,
            Name name,
            Email email,
            ImageUri? iconUri) : base(userId)
        {
            Username = username;
            Name = name;
            Email = email;
            IconUri = iconUri;
        }

        public Username Username
        {
            get => _username;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _username = value;
            }
        }

        public Name Name
        {
            get => _name;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _name = value;
            }
        }

        public Email Email
        {
            get => _email;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);

                _email = value;
            }
        }

        public ImageUri? IconUri { get; private set; }

        public IReadOnlyCollection<Chat> Chats => _chats;

        public IReadOnlyCollection<Message> Messages => _messages;

        public Result AddChat(Chat chat)
        {
            if (_chats.Contains(chat))
            {
                return Result.Failure(UserErrors.UserAlreadyHasChat);
            }

            _chats.Add(chat);

            return Result.Success();
        }

        public Result AddMessage(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            _messages.Add(message);

            return Result.Success();
        }

        public void SetIconUri(ImageUri? iconUri)
        {
            IconUri = iconUri;
        }

        public void RemoveIconUri()
        {
            IconUri = null;
        }

        public static Result<User> Create(
            UserId userId,
            Username username,
            Name name,
            Email email,
            ImageUri? iconUri)
        {
            return new User(
                userId,
                username,
                name,
                email,
                iconUri);
        }
    }
}
