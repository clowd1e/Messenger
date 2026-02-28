using FluentValidation;

namespace Messenger.Application.Features.Chats.Commands.DeleteMessageForEveryone
{
    internal sealed class DeleteMessageForEveryoneCommandValidator
        : AbstractValidator<DeleteMessageForEveryoneCommand>
    {
        public DeleteMessageForEveryoneCommandValidator()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty().WithMessage("Chat ID must not be empty.");

            RuleFor(x => x.MessageId)
                .NotEmpty().WithMessage("Message ID must not be empty.");
        }
    }
}
