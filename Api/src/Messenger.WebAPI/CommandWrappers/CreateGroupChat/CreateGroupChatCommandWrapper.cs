using Messenger.Application.Features.Chats.Commands.CreateGroupChat;
using Messenger.WebAPI.MultipartJsonSupport;

namespace Messenger.WebAPI.CommandWrappers.CreateGroupChat
{
    public sealed class CreateGroupChatCommandWrapper
    {
        public IFormFile? Icon { get; init; }

        [FromJson]
        public required CreateGroupChatRequest Request { get; init; }

        public CreateGroupChatCommand ToCommand() =>
            new(
                Invitees: Request.Invitees,
                Name: Request.Name,
                Description: Request.Description,
                Message: Request.Message,
                Icon: Icon);
    }
}
