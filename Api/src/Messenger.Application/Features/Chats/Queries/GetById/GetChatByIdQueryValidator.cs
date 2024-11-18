using FluentValidation;

namespace Messenger.Application.Features.Chats.Queries.GetById
{
    internal sealed class GetChatByIdQueryValidator
        : AbstractValidator<GetChatByIdQuery>
    {
        public GetChatByIdQueryValidator()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty();
        }
    }
}
