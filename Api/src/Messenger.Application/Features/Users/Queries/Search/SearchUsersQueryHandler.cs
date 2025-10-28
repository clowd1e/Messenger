using Messenger.Application.Abstractions.Data;
using Messenger.Application.Abstractions.Identity;
using Messenger.Application.Abstractions.Messaging;
using Messenger.Application.Features.Users.DTO;
using Messenger.Domain.Aggregates.Users;
using Messenger.Domain.Aggregates.Users.ValueObjects;

namespace Messenger.Application.Features.Users.Queries.Search
{
    internal sealed class SearchUsersQueryHandler
        : IQueryHandler<SearchUsersQuery, IEnumerable<SearchUserResponse>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, SearchUserResponse> _mapper;

        public SearchUsersQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<User, SearchUserResponse> mapper)  
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<SearchUserResponse>>> Handle(
            SearchUsersQuery query,
            CancellationToken cancellationToken)
        {
            var currentUserId = _userContextService.GetAuthenticatedUserId();
            var userId = new UserId(currentUserId);

            var cleanedSearchTerm = CleanSearchTerm(query.SearchTerm);

            var users = await _userRepository.SearchUsersByNameOrUsernameAsync(
                cleanedSearchTerm,
                userId,
                cancellationToken);

            var response = _mapper.Map(users);

            return Result.Success(response);
        }

        private static string CleanSearchTerm(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return string.Empty;

            if (searchTerm[0] == '@')
                searchTerm = searchTerm[1..];

            return searchTerm.Trim().ToLower();
        }
    }
}
