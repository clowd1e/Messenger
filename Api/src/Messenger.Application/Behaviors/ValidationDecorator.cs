using FluentValidation;
using Messenger.Application.Abstractions.Messaging;

namespace Messenger.Application.Behaviors
{
    internal static class ValidationDecorator
    {
        internal sealed class QueryHandler<TQuery, TResponse>(
            IQueryHandler<TQuery, TResponse> innerHandler,
            IEnumerable<IValidator<TQuery>> validators)
            : IQueryHandler<TQuery, TResponse>
            where TQuery : IQuery<TResponse>
        {
            public async Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
            {
                if (!validators.Any())
                {
                    return await innerHandler.Handle(query, cancellationToken);
                }

                Error[] errors = GetErrors(query, validators);

                if (errors.Length > 0)
                {
                    return CreateValidationResult<Result<TResponse>>(errors);
                }

                return await innerHandler.Handle(query, cancellationToken);
            }
        }

        internal sealed class CommandHandler<TCommand>(
            ICommandHandler<TCommand> innerHandler,
            IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand>
            where TCommand : ICommand
        {
            public async Task<Result> Handle(TCommand command, CancellationToken cancellationToken)
            {
                if (!validators.Any())
                {
                    return await innerHandler.Handle(command, cancellationToken);
                }

                Error[] errors = GetErrors(command, validators);

                if (errors.Length > 0)
                {
                    return CreateValidationResult<Result>(errors);
                }

                return await innerHandler.Handle(command, cancellationToken);
            }
        }

        internal sealed class CommandHandler<TCommand, TResponse>(
            ICommandHandler<TCommand, TResponse> innerHandler,
            IEnumerable<IValidator<TCommand>> validators)
            : ICommandHandler<TCommand, TResponse>
            where TCommand : ICommand<TResponse>
        {
            public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
            {
                if (!validators.Any())
                {
                    return await innerHandler.Handle(command, cancellationToken);
                }

                Error[] errors = GetErrors(command, validators);

                if (errors.Length > 0)
                {
                    return CreateValidationResult<Result<TResponse>>(errors);
                }

                return await innerHandler.Handle(command, cancellationToken);
            }
        }

        private static Error[] GetErrors<TCommand>(
            TCommand request,
            IEnumerable<IValidator<TCommand>> validators)
        {
            return [.. validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => Error.Validation(
                    code: failure.PropertyName,
                    description: failure.ErrorMessage))
                .Distinct()];
        }

        private static TResult CreateValidationResult<TResult>(Error[] errors)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (ValidationResult
                    .WithErrors(errors) as TResult)!;
            }

            object validationResult = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TResult).GenericTypeArguments.First())
                .GetMethod(nameof(ValidationResult.WithErrors))!
                .Invoke(null, [errors])!;

            return (TResult)validationResult;
        }
    }
}
