using Messenger.Domain.Aggregates.User.ValueObjects;
using Messenger.Domain.Primitives;
using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.User
{
    public sealed class User : AggregateRoot<UserId>
    {
        private Username _username;
        private Email _email;

        private User() 
            : base(new(Guid.NewGuid())) { }

        private User(
            UserId userId,
            Username username,
            Email email) : base(userId)
        {
            Username = username;
            Email = email;
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

        public static Result<User> Create(
            UserId userId,
            Username username,
            Email email)
        {
            return new User(
                userId,
                username,
                email);
        }
    }
}
