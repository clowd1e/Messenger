using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Auth.DTO.Response;

namespace Messenger.Application.Features.Auth.Queries.ValidateEmailConfirmation
{
    public sealed record ValidateEmailConfirmationQuery(
        Guid UserId,
        Guid TokenId) : IQuery<ValidateEmailConfirmationResponse>;
}
