using FluentValidation;

namespace Messenger.Application.Features.Chats.Queries.GetCurrentUserChatsPaginated
{
    internal sealed class GetCurrentUserChatsPaginatedQueryValidator
        : AbstractValidator<GetCurrentUserChatsPaginatedQuery>
    {
        public GetCurrentUserChatsPaginatedQueryValidator()
        {
            RuleFor(x => x.Page)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .NotEmpty()
                .InclusiveBetween(1, 20);

            RuleFor(x => x.RetrievalCutoff)
                .NotEmpty()
                .LessThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
