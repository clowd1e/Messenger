using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;

namespace Messenger.Application.Features.Auth.Queries.ValidatePasswordRecovery
{
    public sealed record ValidatePasswordRecoveryQuery(
        Guid UserId,
        Guid TokenId) : IQuery<ValidatePasswordRecoveryResponse>;
}
