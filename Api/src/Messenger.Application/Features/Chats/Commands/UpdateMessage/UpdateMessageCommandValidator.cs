using FluentValidation;
using Messenger.Domain.Aggregates.Messages.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.UpdateMessage
{
    internal sealed class UpdateMessageCommandValidator
        : AbstractValidator<UpdateMessageCommand>
    {
        public UpdateMessageCommandValidator()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty();

            RuleFor(x => x.MessageId)
                .NotEmpty();

            RuleFor(x => x.NewContent)
                .NotEmpty()
                .MaximumLength(MessageContent.MaxLength);
        }
    }
}
