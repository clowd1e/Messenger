using Messenger.Domain.Aggregates.Users;

namespace Messenger.Application.Features.Auth.DTO.RequestModel
{
    public sealed record CreateResetPasswordTokenRequestModel(
        string TokenHash,
        User User);
}
