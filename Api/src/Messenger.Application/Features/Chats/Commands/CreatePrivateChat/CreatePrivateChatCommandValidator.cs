using FluentValidation;

namespace Messenger.Application.Features.Chats.Commands.CreatePrivateChat
{
    internal sealed class CreatePrivateChatCommandValidator
        : AbstractValidator<CreatePrivateChatCommand>
    {
        public CreatePrivateChatCommandValidator()
        {
            RuleFor(x => x.InviteeId)
                .NotEmpty();

            RuleFor(x => x.Message)
                .NotEmpty();
        }
    }
}
