using FluentValidation;
using Messenger.Domain.Aggregates.Chats.Messages.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.SendMessage
{
    internal sealed class SendMessageCommandValidator
        : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty();

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(MessageContent.MaxLength);
        }
    }
}
