using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.Commands.SendMessage;
using Messenger.Domain.Aggregates.Chats.Messages.ValueObjects;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class SendMessageCommandMapper
        : Mapper<SendMessageCommandWrapper, Result<Message>>
    {
        public override Result<Message> Map(SendMessageCommandWrapper source)
        {
            var timestampResult = MessageTimestamp.UtcNow();

            if (timestampResult.IsFailure)
            {
                return Result.Failure<Message>(timestampResult.Error);
            }

            var timestamp = timestampResult.Value;

            var contentResult = MessageContent.Create(source.Command.Message);

            if (contentResult.IsFailure)
            {
                return Result.Failure<Message>(contentResult.Error);
            }

            var content = contentResult.Value;

            return Message.Create(
                timestamp: timestamp,
                content: content,
                userId: source.UserId);
        }
    }
}
