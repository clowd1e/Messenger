using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Chats.DTO.RequestModels;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.Messages;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.CreateGroupChat
{
    internal sealed class CreateGroupChatCommandHandler
        : ICommandHandler<CreateGroupChatCommand, Guid>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly Mapper<CreateGroupChatRequestModel, Result<GroupChat>> _groupChatMapper;
        private readonly Mapper<CreateMessageRequestModel, Result<Message>> _messageMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGroupChatCommandHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository,
            Mapper<CreateGroupChatRequestModel, Result<GroupChat>> groupChatMapper,
            Mapper<CreateMessageRequestModel, Result<Message>> messageMapper,
            IUnitOfWork unitOfWork)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _groupChatMapper = groupChatMapper;
            _messageMapper = messageMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateGroupChatCommand command,
            CancellationToken cancellationToken)
        {
            // Retrieve the authenticated user
            var inviterId = new UserId(_userContextService.GetAuthenticatedUserId());

            var inviter = await _userRepository.GetByIdAsync(inviterId, cancellationToken);

            if (inviter is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            // Retrieve the invitees
            var inviteesIds = command.Invitees
                .Select(id => new UserId(id))
                .ToList();

            var invitees = await _userRepository.GetAllByIdsAsync(
                inviteesIds,
                cancellationToken);

            if (invitees.Length != inviteesIds.Count)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            // Check if inviter is in the invitees list
            if (invitees.Any(invitee => invitee.Id == inviterId))
            {
                return Result.Failure<Guid>(UserErrors.InviterCannotBeInvitee);
            }

            // Map the request to a GroupChat
            var requestModel = new CreateGroupChatRequestModel(
                command.Name,
                command.Description,
                inviter,
                invitees);

            var groupChatMappingResult = _groupChatMapper.Map(requestModel);

            if (groupChatMappingResult.IsFailure)
            {
                return Result.Failure<Guid>(groupChatMappingResult.Error);
            }

            var groupChat = groupChatMappingResult.Value;

            // Map the first message
            var messageRequestModel = new CreateMessageRequestModel(command.Message);

            var messageMappingResult = _messageMapper.Map(messageRequestModel);

            if (messageMappingResult.IsFailure)
            {
                return Result.Failure<Guid>(messageMappingResult.Error);
            }

            var message = messageMappingResult.Value;

            // Add the first message to the group chat

            message.SetUser(inviter);
            message.SetChat(groupChat);

            // Add message to chat and user
            var addMessageToChatResult = groupChat.AddMessage(message);

            if (addMessageToChatResult.IsFailure)
            {
                return Result.Failure<Guid>(addMessageToChatResult.Error);
            }

            var addMessageToUserResult = inviter.AddMessage(message);

            if (addMessageToUserResult.IsFailure)
            {
                return Result.Failure<Guid>(addMessageToUserResult.Error);
            }

            // Insert chat and message
            await _chatRepository.InsertGroupChatAsync(groupChat, cancellationToken);
            await _messageRepository.InsertAsync(message, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(groupChat.Id.Value);
        }
    }
}
