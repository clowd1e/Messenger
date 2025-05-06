using MediatR;
using Messenger.Application.Features.Auth.DTO.Response;

namespace Messenger.Application.Features.Auth.Queries.ValidatePasswordRecovery
{
    public sealed record ValidatePasswordRecoveryQuery(
        Guid UserId,
        Guid TokenId) : IRequest<Result<ValidatePasswordRecoveryResponse>>;
}
