﻿using MediatR;
using Messenger.Application.Features.Auth.DTO;

namespace Messenger.Application.Features.Auth.Commands.Login
{
    public sealed record LoginCommand(
        string? Email,
        string? Password) : IRequest<Result<LoginResponse>>;
}
