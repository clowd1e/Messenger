using FluentValidation;
using Messenger.Domain.Aggregates.Chats;
using Messenger.Domain.Aggregates.GroupChats.ValueObjects;

namespace Messenger.Application.Features.Chats.Commands.CreateGroupChat
{
    internal sealed class CreateGroupChatCommandValidator
        : AbstractValidator<CreateGroupChatCommand>
    {
        public CreateGroupChatCommandValidator()
        {
            RuleFor(x => x.Invitees)
                .NotEmpty()
                .Must(invitees => (invitees.Count + 1) >= GroupChat.MinParticipantsCount)
                .WithMessage($"A group chat must have at least {GroupChat.MinParticipantsCount} participants.")
                .Must(invitees => (invitees.Count + 1) <= GroupChat.MaxParticipantsCount)
                .WithMessage($"A group chat cannot have more than {GroupChat.MaxParticipantsCount} participants.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(GroupChatName.MinLength)
                .MaximumLength(GroupChatName.MaxLength);

            RuleFor(x => x.Description)
                .MinimumLength(GroupChatDescription.MinLength)
                .MaximumLength(GroupChatDescription.MaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Message)
                .NotEmpty();
        }
    }
}
