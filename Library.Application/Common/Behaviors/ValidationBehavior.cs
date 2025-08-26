using Library.Application.Common.Models;

namespace Library.Application.Common.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
        where TResponse : class
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
        {
            if (!validators.Any())
            {
                return await next(ct);
            }

            var context = new ValidationContext<TRequest>(request);
            var validationTasks = validators.Select(v => v.ValidateAsync(context, ct));
            var validationResults = await Task.WhenAll(validationTasks);

            var failures = validationResults
                .Where(r => !r.IsValid)
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count > 0)
            {
                return CreateValidationResult<TResponse>(failures);
            }

            return await next(ct);
        }

        private static TResult CreateValidationResult<TResult>(List<ValidationFailure> failures) where TResult : class
        {
            var errors = failures
                .Select(f => f.ErrorMessage)
                .ToArray();

            if (typeof(TResult).IsGenericType)
            {
                var resultType = typeof(TResult).GetGenericTypeDefinition();

                if (resultType == typeof(Result<>))
                {
                    var valueType = typeof(TResult).GetGenericArguments()[0];
                    var failureMethod = typeof(Result<>)
                        .MakeGenericType(valueType)
                        .GetMethod(nameof(Result<object>.ValidationFailure), [typeof(string[])]);

                    return (TResult)failureMethod!.Invoke(null, [errors])!;
                }
            }

            if (typeof(TResult) == typeof(Result))
            {
                return (TResult)(object)Result.ValidationFailure(errors);
            }

            throw new ValidationException
            (
                "Validation failed",
                failures.Select(f => new ValidationFailure(f.PropertyName, f.ErrorMessage))
            );
        }
    }
}
