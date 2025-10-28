using FluentValidation;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Users.Queries.Search
{
    internal sealed class SearchUsersQueryValidator
        : AbstractValidator<SearchUsersQuery>
    {
        // Select the maximum length between Username and Name for validation
        public static readonly int MaxSearchTermLength = Math.Max(Username.MaxLength, Name.MaxLength);

        public SearchUsersQueryValidator()
        {
            RuleFor(x => x.SearchTerm)
                .NotEmpty()
                .MaximumLength(MaxSearchTermLength);
        }
    }
}
