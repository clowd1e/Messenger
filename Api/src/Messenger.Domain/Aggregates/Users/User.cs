using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Common.ImageUri;
using Messenger.Domain.Aggregates.ConfirmEmailTokens;
using Messenger.Domain.Aggregates.GroupChats;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.RefreshTokens;
using Messenger.Domain.Aggregates.ResetPasswordTokens;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.Users
{
    public sealed class User : AggregateRoot<UserId>
    {
        private readonly HashSet<Chat> _chats = [];
        private readonly HashSet<GroupMember> _groupMembers = [];
        private readonly HashSet<Message> _messages = [];
        private readonly HashSet<RefreshToken> _refreshTokens = [];
        private readonly HashSet<ConfirmEmailToken> _confirmEmailTokens = [];
        private readonly HashSet<ResetPasswordToken> _resetPasswordTokens = [];
        private Username _username;
        private Name _name;
        private Email _email;
        private RegistrationDate _registrationDate;

        private User()
            : base(new(Guid.NewGuid())) { }

        private User(
            UserId userId,
            Username username,
            Name name,
            Email email,
            RegistrationDate registrationDate,
            ImageUri? iconUri) : base(userId)
        {
            Username = username;
            Name = name;
            Email = email;
            RegistrationDate = registrationDate;
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

        public RegistrationDate RegistrationDate
        {
            get => _registrationDate;
            private set
            {
                ArgumentNullException.ThrowIfNull(value);
                _registrationDate = value;
            }
        }

        public ImageUri? IconUri { get; private set; }

        public bool EmailConfirmed { get; private set; } = false;

        public IReadOnlyCollection<Chat> Chats => _chats;

        public IReadOnlyCollection<GroupMember> GroupMembers => _groupMembers;

        public IReadOnlyCollection<Message> Messages => _messages;

        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens;

        public IReadOnlyCollection<ConfirmEmailToken> ConfirmEmailTokens => _confirmEmailTokens;

        public IReadOnlyCollection<ResetPasswordToken> ResetPasswordTokens => _resetPasswordTokens;

        public Result AddChat(Chat newChat)
        {
            if (_chats.Any(c => c.Id == newChat.Id))
            {
                return Result.Failure(ChatErrors.UserAlreadyInChat);
            }

            _chats.Add(newChat);

            return Result.Success();
        }

        public Result AddMessage(Message message)
        {
            ArgumentNullException.ThrowIfNull(message);

            _messages.Add(message);

            return Result.Success();
        }

        public Result ConfirmEmail()
        {
            if (EmailConfirmed)
            {
                return Result.Failure(UserErrors.EmailAlreadyConfirmed);
            }

            EmailConfirmed = true;

            return Result.Success();
        }

        public Result AddResetPasswordToken(
            ResetPasswordToken resetPasswordToken)
        {
            ArgumentNullException.ThrowIfNull(resetPasswordToken);

            _resetPasswordTokens.Add(resetPasswordToken);

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
            RegistrationDate registrationDate,
            ImageUri? iconUri)
        {
            return new User(
                userId,
                username,
                name,
                email,
                registrationDate,
                iconUri);
        }
    }
}
