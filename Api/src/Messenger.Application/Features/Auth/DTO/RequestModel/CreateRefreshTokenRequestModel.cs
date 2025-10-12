using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Auth.DTO.RequestModel
{
    public sealed record CreateRefreshTokenRequestModel(
        string TokenHash,
        User User,
        string SessionId);
}
