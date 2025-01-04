using MediatR;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Features.Users.Commands.SetIcon
{
    public sealed record SetUserIconCommand(
        IFormFile? Icon) : IRequest<Result>;
}
