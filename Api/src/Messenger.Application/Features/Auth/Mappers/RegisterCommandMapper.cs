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

            var nameResult = Name.Create(source.Name);

            if (nameResult.IsFailure)
            {
                return Result.Failure<User>(nameResult.Error);
            }

            var name = nameResult.Value;

            var emailResult = Email.Create(source.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<User>(emailResult.Error);
            }

            var email = emailResult.Value;

            var registrationDateResult = RegistrationDate.UtcNow();

            if (registrationDateResult.IsFailure)
            {
                return Result.Failure<User>(registrationDateResult.Error);
            }

            var registrationDate = registrationDateResult.Value;

            UserId userId = new(Guid.NewGuid());

            return User.Create(
                userId: userId,
                username: username,
                name: name,
                email: email,
                registrationDate: registrationDate,
                iconUri: null);
        }
    }
}
