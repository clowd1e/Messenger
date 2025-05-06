using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Auth.DTO.RequestModel
{
    public sealed record CreateConfirmEmailTokenRequestModel(
        string TokenHash,
        User User);
}
