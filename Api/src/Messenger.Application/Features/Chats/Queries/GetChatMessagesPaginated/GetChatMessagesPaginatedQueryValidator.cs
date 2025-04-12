using FluentValidation;

namespace Messenger.Application.Features.Chats.Queries.GetChatMessagesPaginated
{
    internal sealed class GetChatMessagesPaginatedQueryValidator
        : AbstractValidator<GetChatMessagesPaginatedQuery>
    {
        public GetChatMessagesPaginatedQueryValidator()
        {
            RuleFor(x => x.ChatId)
                .NotEmpty();

            RuleFor(x => x.Page)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .NotEmpty()
                .InclusiveBetween(1, 70);

            RuleFor(x => x.RetrievalCutoff)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow.AddSeconds(2));
        }
    }
}
