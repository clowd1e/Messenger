using FluentValidation;

namespace Messenger.Application.Features.Chats.Queries.GetChatWithUserExists
{
    internal sealed class GetChatWithUserExistsQueryValidator
        : AbstractValidator<GetChatWithUserExistsQuery>
    {
        public GetChatWithUserExistsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
