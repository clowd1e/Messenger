using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;

namespace Messenger.Application.Features.Users.Queries.Search
{
    public sealed record SearchUsersQuery(
        string SearchTerm) : IQuery<IEnumerable<SearchUserResponse>>;
}
