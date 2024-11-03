using FluentValidation;

namespace Messenger.Application.Features.Users.Queries.GetById
{
    internal sealed class GetUserByIdQueryValidator 
        : AbstractValidator<GetUserByIdQuery>
    {
        public GetUserByIdQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
