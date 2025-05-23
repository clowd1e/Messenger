﻿using Messenger.Application.Abstractions.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Messenger.Infrastructure.Services
{
    public sealed class UserContextService : IUserContextService<Guid>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetAuthenticatedUserId()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User;

            if (userClaims?.Identity is null || !userClaims.Identity.IsAuthenticated)
            {
                throw new InvalidOperationException("User is not authenticated.");
            }

            var idClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);

            if (idClaim is null)
            {
                throw new InvalidOperationException($"Claim missing: id.");
            }

            var idString = idClaim.Value;

            if (Guid.TryParse(idString, out Guid id))
            {
                return id;
            }

            throw new InvalidOperationException("Failed to parse ID to GUID.");
        }
    }
}
