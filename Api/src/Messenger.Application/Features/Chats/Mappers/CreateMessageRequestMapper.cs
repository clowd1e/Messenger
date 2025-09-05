using Messenger.Application.Abstractions.Data;
using Messenger.Application.Features.Chats.DTO.RequestModels;
using Messenger.Domain.Aggregates.Chats.Messages.ValueObjects;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.Messages.ValueObjects;

namespace Messenger.Application.Features.Chats.Mappers
{
    internal sealed class CreateMessageRequestMapper
        : Mapper<CreateMessageRequestModel, Result<Message>>
    {
        public override Result<Message> Map(CreateMessageRequestModel request)
        {
            var timestampResult = MessageTimestamp.UtcNow();

            if (timestampResult.IsFailure)
            {
                return Result.Failure<Message>(timestampResult.Error);
            }

            var timestamp = timestampResult.Value;

            var contentResult = MessageContent.Create(request.Message);

            if (contentResult.IsFailure)
            {
                return Result.Failure<Message>(contentResult.Error);
            }

            var content = contentResult.Value;

            return Message.Create(
                messageId: new MessageId(Guid.NewGuid()),
                timestamp: timestamp,
                content: content);
        }
    }
}
