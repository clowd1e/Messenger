﻿namespace Messenger.Domain.Shared
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.OperationFailure);

        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.OperationFailure);

        private Error(
            string code,
            string description,
            ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }

        public string Code { get; init; }

        public string Description { get; init; }

        public ErrorType ErrorType { get; init; }

        public static Error InternalFailure(string code, string description) =>
            new(code, description, ErrorType.InternalFailure);

        public static Error InternalFailure() =>
            new(string.Empty, string.Empty, ErrorType.InternalFailure);

        public static Error OperationFailure(string code, string description) =>
            new(code, description, ErrorType.OperationFailure);

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Validation(string code, string description) =>
            new(code, description, ErrorType.Validation);

        public static Error Unauthorized(string code, string description) =>
           new(code, description, ErrorType.Unauthorized);

        public static implicit operator Result(Error error) => Result.Failure(error);
    }
}
