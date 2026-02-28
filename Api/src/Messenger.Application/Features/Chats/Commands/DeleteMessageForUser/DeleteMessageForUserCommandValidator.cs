using FluentValidation;

namespace Messenger.Application.Features.Chats.Commands.DeleteMessageForUser
{
    internal sealed class DeleteMessageForUserCommandValidator
        : AbstractValidator<DeleteMessageForUserCommand>
    {
        public DeleteMessageForUserCommandValidator()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty().WithMessage("Chat ID must not be empty.");

            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("Message ID must not be empty.");
        }
    }
}
