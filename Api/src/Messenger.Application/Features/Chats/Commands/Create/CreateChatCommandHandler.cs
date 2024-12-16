using MediatR;
using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.User.Errors;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.Create
{
    internal sealed class CreateChatCommandHandler
        : IRequestHandler<CreateChatCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<CreateChatCommandWrapper, Result<Chat>> _chatMapper;
        private readonly IUnitOfWork _unitOfWork;

        public CreateChatCommandHandler(
            IUserRepository userRepository,
            IChatRepository chatRepository,
            IUserContextService<Guid> userContextService,
            Mapper<CreateChatCommandWrapper, Result<Chat>> chatMapper,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _userContextService = userContextService;
            _chatMapper = chatMapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateChatCommand request,
            CancellationToken cancellationToken)
        {
            var inviterId = new UserId(_userContextService.GetAuthenticatedUserId());

            var inviter = await _userRepository.GetByIdAsync(
                inviterId, cancellationToken);

            if (inviter is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var invitee = await _userRepository.GetByIdAsync(
                new UserId(request.InviteeId.Value), cancellationToken);

            if (invitee is null)
            {
                return Result.Failure<Guid>(UserErrors.NotFound);
            }

            var commandWrapper = new CreateChatCommandWrapper(inviter, invitee);

            var mappingResult = _chatMapper.Map(commandWrapper);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var chat = mappingResult.Value;

            invitee.AddChat(chat);
            inviter.AddChat(chat);

            await _chatRepository.InsertAsync(chat, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(chat.Id.Value);
        }
    }
}
