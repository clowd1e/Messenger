using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Auth.Commands.Register;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Mappers
{
    internal sealed class RegisterCommandMapper
        : Mapper<RegisterCommand, Result<User>>
    {
        public override Result<User> Map(RegisterCommand source)
        {
            var usernameResult = Username.Create(source.Username);

            if (usernameResult.IsFailure)
            {
                return Result.Failure<User>(usernameResult.Error);
            }

            var username = usernameResult.Value;

            var emailResult = Email.Create(source.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<User>(emailResult.Error);
            }

            var email = emailResult.Value;

            UserId userId = new(Guid.NewGuid());

            return User.Create(
                userId: userId,
                username: username,
                email: email,
                iconUri: null);
        }
    }
}
