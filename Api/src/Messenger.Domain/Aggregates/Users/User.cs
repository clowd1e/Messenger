using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users
{
    public sealed class User : AggregateRoot<UserId>
    {
        private readonly HashSet<Chat> _chats = [];
        private Username _username;
        private Email _email;

        private User()
            : base(new(Guid.NewGuid())) { }

        private User(
            UserId userId,
            Username username,
            Email email,
            ImageUri? iconUri) : base(userId)
        {
            Username = username;
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

        public Result AddChat(Chat chat)
        {
            if (_chats.Contains(chat))
            {
                return Result.Failure(UserErrors.UserAlreadyHasChat);
            }

            _chats.Add(chat);

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
            Email email,
            ImageUri? iconUri)
        {
            return new User(
                userId,
                username,
                email,
                iconUri);
        }
    }
}
