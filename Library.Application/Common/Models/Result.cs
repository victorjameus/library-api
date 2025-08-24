namespace Library.Application.Common.Models
{
    public sealed class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public string Error { get; }
        public string[] ValidationErrors { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
            Error = string.Empty;
            ValidationErrors = [];
        }

        private Result(string error)
        {
            IsSuccess = false;
            Value = default;
            Error = error ?? string.Empty;
            ValidationErrors = [];
        }

        private Result(string[] validationErrors)
        {
            IsSuccess = false;
            Value = default;
            Error = "Errores de validación";
            ValidationErrors = validationErrors ?? [];
        }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(string error) => new(error);
        public static Result<T> ValidationFailure(params string[] errors) => new(errors);
        public static Result<T> ValidationFailure(IEnumerable<string> errors) => new([.. errors]);

        public static implicit operator Result<T>(T value) => Success(value);
        public static implicit operator Result<T>(string error) => Failure(error);

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<string, TResult> onFailure)
        {
            return IsSuccess
                ? onSuccess(Value!)
                : onFailure(Error);
        }

        public async Task<TResult> MatchAsync<TResult>(Func<T, Task<TResult>> onSuccess, Func<string, Task<TResult>> onFailure)
        {
            return IsSuccess
                ? await onSuccess(Value!)
                : await onFailure(Error);
        }

        public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
        {
            return IsSuccess
                ? Result<TOut>.Success(mapper(Value!))
                : Result<TOut>.Failure(Error);
        }

        public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> binder)
        {
            return IsSuccess
                ? binder(Value!)
                : Result<TOut>.Failure(Error);
        }

        public override string ToString()
        {
            return IsSuccess
                ? $"Success: {Value}"
                : ValidationErrors.Length > 0
                    ? $"ValidationFailure: {string.Join(", ", ValidationErrors)}"
                    : $"Failure: {Error}";
        }
    }

    public sealed class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }
        public string[] ValidationErrors { get; }

        private Result()
        {
            IsSuccess = true;
            Error = string.Empty;
            ValidationErrors = [];
        }

        private Result(string error)
        {
            IsSuccess = false;
            Error = error ?? string.Empty;
            ValidationErrors = [];
        }

        private Result(string[] validationErrors)
        {
            IsSuccess = false;
            Error = "Errores de validación";
            ValidationErrors = validationErrors ?? [];
        }

        public static Result Success() => new();
        public static Result Failure(string error) => new(error);
        public static Result ValidationFailure(params string[] errors) => new(errors);
        public static Result ValidationFailure(IEnumerable<string> errors) => new([.. errors]);

        public static implicit operator Result(string error) => Failure(error);

        public Result<T> ToResult<T>(T value)
        {
            return IsSuccess
                ? Result<T>.Success(value)
                : Result<T>.Failure(Error);
        }

        public override string ToString()
        {
            return IsSuccess
                ? "Success"
                : ValidationErrors.Length > 0
                    ? $"ValidationFailure: {string.Join(", ", ValidationErrors)}"
                    : $"Failure: {Error}";
        }
    }

    public static class ResultExtensions
    {
        public static Result<T> ToResult<T>(this IEnumerable<string> validationErrors)
        {
            var errors = validationErrors.ToArray();

            return errors.Length == 0
                ? Result<T>.Failure("Error de validación desconocido")
                : Result<T>.ValidationFailure(errors);
        }

        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
            {
                action(result.Value!);
            }

            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }

            return result;
        }

        public static Result Combine(params Result[] results)
        {
            var failures = results.Where(r => r.IsFailure).ToArray();

            if (failures.Length == 0)
            {
                return Result.Success();
            }

            var errors = failures.SelectMany(failures => failures.ValidationErrors.Length > 0 ? failures.ValidationErrors : [failures.Error]);

            return Result.ValidationFailure(errors);
        }
    }
}
