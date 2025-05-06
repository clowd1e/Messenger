using MediatR;
using Messenger.Application.Features.Auth.DTO.Response;

namespace Messenger.Application.Features.Auth.Queries.ValidateEmailConfirmation
{
    public sealed record ValidateEmailConfirmationQuery(
        Guid UserId,
        Guid TokenId) : IRequest<Result<ValidateEmailConfirmationResponse>>;
}
