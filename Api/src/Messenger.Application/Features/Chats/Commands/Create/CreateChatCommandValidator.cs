using FluentValidation;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    internal sealed class CreateChatCommandValidator
        : AbstractValidator<CreateChatCommand>
    {
        public CreateChatCommandValidator()
        {
            RuleFor(x => x.InviteeId)
                .NotEmpty();
        }
    }
}
