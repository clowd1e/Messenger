﻿using Messenger.Domain.Shared;

namespace Messenger.Domain.Aggregates.User.Errors
{
    public static class UserErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "User.NotFound",
                description: "User not found.");

        public static readonly Error UserWithSameCredentialsAlreadyExists =
            Error.Validation(
                code: "User.UserWithSameCredentialsAlreadyExists",
                description: "User with the same username or email already exists.");

        public static readonly Error UserAlreadyHasChat =
            Error.Validation(
                code: "User.UserAlreadyHasChat",
                description: "User already has this chat.");

        public static readonly Error InvalidAccessToken =
            Error.Validation(
                code: "User.InvalidAccessToken",
                description: "The provided access token is invalid.");

        public static readonly Error InvalidRefreshToken =
            Error.Validation(
                code: "User.InvalidRefreshToken",
                description: "The provided refresh token is invalid.");

        public static readonly Error RefreshTokenExpired =
            Error.Validation(
                code: "User.RefreshTokenExpired",
                description: "The provided refresh token has expired.");

        public static readonly Error InvalidCredentials =
            Error.Validation(
                code: "User.InvalidCredentials",
                description: "The provided credentials are invalid.");

        public static readonly Error EmailAlreadyConfirmed =
            Error.Validation(
                code: "User.EmailAlreadyConfirmed",
                description: "User's email is already confirmed.");

        public static readonly Error EmailNotConfirmed =
            Error.Validation(
                code: "User.EmailNotConfirmed",
                description: "User's email is not confirmed.");

        public static readonly Error InvalidIconDimensions =
            Error.Validation(
                code: "User.InvalidIconDimensions",
                description: "The icon dimensions are invalid.");

        public static readonly Error IconNotSet =
            Error.Validation(
                code: "User.IconNotSet",
                description: "User's icon is not set.");
    }
}
