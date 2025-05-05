using MediatR;

namespace Messenger.Application.Features.Auth.Commands.RequestPasswordRecovery
{
    public sealed record RequestPasswordRecoveryCommand(
        string Email) : IRequest<Result>;
}
