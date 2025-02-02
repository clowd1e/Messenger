using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Exceptions;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.Chats.ValueObjects;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.SendMessage
{
    internal sealed class SendMessageCommandHandler
        : IRequestHandler<SendMessageCommand, Result<MessageResponse>>
    {
        private readonly IChatRepository _chatRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<SendMessageRequestModel, Result<Message>> _commandMapper;
        private readonly Mapper<Message, MessageResponse> _messageMapper;
        private readonly IUnitOfWork _unitOfWork;

        public SendMessageCommandHandler(
            IChatRepository chatRepository,
            IUserRepository userRepository,
            IUserContextService<Guid> userContextService,
            Mapper<SendMessageRequestModel, Result<Message>> commandMapper,
            Mapper<Message, MessageResponse> messageMapper,
            IUnitOfWork unitOfWork)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _userContextService = userContextService;
            _commandMapper = commandMapper;
            _messageMapper = messageMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<MessageResponse>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var userExists = await _userRepository.ExistsAsync(userId, cancellationToken);

            if (!userExists)
            {
                throw new AuthenticatedUserNotFoundException();
            }

            var chat = await _chatRepository.GetByIdAsync(
                new ChatId(request.ChatId.Value),
                cancellationToken);

            if (chat is null)
            {
                return Result.Failure<MessageResponse>(ChatErrors.NotFound);
            }

            var requestModel = new SendMessageRequestModel(request.Message, userId);

            var mappingResult = _commandMapper.Map(requestModel);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<MessageResponse>(mappingResult.Error);
            }

            var message = mappingResult.Value;

            var addMessageResult = chat.AddMessage(message);

            if (addMessageResult.IsFailure)
            {
                return Result.Failure<MessageResponse>(addMessageResult.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var messageResponse = _messageMapper.Map(message);

            return Result.Success(messageResponse);
        }
    }
}
