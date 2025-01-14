﻿using FluentValidation;

namespace Messenger.Application.Features.Auth.Commands.RefreshToken
{
    internal sealed class RefreshTokenCommandValidator
        : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.AccessToken)
                .NotEmpty()
                .WithMessage("Access token is required.");

            RuleFor(x => x.RefreshToken)
                .NotEmpty()
                .WithMessage("Refresh token is required.");
        }
    }
}
