using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Features.Chats.DTO;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Chats.Errors;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;
using Messenger.Domain.Aggregates.ValueObjects.Chats.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    internal sealed class CreateChatCommandHandler
        : IRequestHandler<CreateChatCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<CreateChatRequestModel, Result<Chat>> _chatMapper;
        private readonly Mapper<SendMessageRequestModel, Result<Message>> _messageMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateChatCommandHandler(
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IUserContextService<Guid> userContextService,
            Mapper<CreateChatRequestModel, Result<Chat>> chatMapper,
            Mapper<SendMessageRequestModel, Result<Message>> messageMapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _userContextService = userContextService;
            _chatMapper = chatMapper;
            _messageMapper = messageMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateChatCommand request,
            CancellationToken cancellationToken)
        {
            var inviterId = new UserId(_userContextService.GetAuthenticatedUserId());

            var inviter = await _userRepository.GetByIdWithChatsAsync(
                inviterId, cancellationToken);

            if (inviter is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var inviteeId = new UserId(request.InviteeId.Value);

            var invitee = await _userRepository.GetByIdAsync(inviteeId, cancellationToken);

            if (invitee is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            if (inviter.Id == invitee.Id)
            {
                return Result.Failure<Guid>(ChatErrors.ChatWithSameUser);
            }

            var chatExists = await _chatRepository.ExistsAsync(inviterId, inviteeId, cancellationToken);

            if (chatExists)
            {
                return Result.Failure<Guid>(ChatErrors.ChatAlreadyExists);
            }

            var requestModel = new CreateChatRequestModel(inviter, invitee);

            var mappingResult = _chatMapper.Map(requestModel);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var chat = mappingResult.Value;

            invitee.AddChat(chat);
            inviter.AddChat(chat);

            var messageRequestModel = new SendMessageRequestModel(
                request.Message, inviter.Id);

            var messageMappingResult = _messageMapper.Map(messageRequestModel);

            if (messageMappingResult.IsFailure)
            {
                return Result.Failure<Guid>(messageMappingResult.Error);
            }

            var message = messageMappingResult.Value;

            var addMessageResult = chat.AddMessage(message);

            if (addMessageResult.IsFailure)
            {
                return Result.Failure<Guid>(addMessageResult.Error);
            }

            await _chatRepository.InsertAsync(chat, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(chat.Id.Value);
        }
    }
}
