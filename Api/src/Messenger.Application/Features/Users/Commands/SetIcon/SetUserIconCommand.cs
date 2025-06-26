using Messenger.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;

namespace Messenger.Application.Features.Users.Commands.SetIcon
{
    public sealed record SetUserIconCommand(
        IFormFile? Icon) : ICommand;
}
