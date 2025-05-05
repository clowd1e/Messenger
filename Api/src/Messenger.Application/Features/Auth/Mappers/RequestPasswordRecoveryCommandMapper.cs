using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Auth.Mappers
{
    internal sealed class RequestPasswordRecoveryCommandMapper
        : Mapper<RequestPasswordRecoveryCommand, Result<Email>>
    {
        public override Result<Email> Map(RequestPasswordRecoveryCommand source)
        {
            var emailResult = Email.Create(source.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<Email>(emailResult.Error);
            }

            var email = emailResult.Value;

            return email;
        }
    }
}
