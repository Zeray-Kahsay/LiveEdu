namespace API.Helpers;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public IEnumerable<string>? Errors { get; }


    protected Result(bool isSuccess, T? value, IEnumerable<string>? errors)
    {
        if (isSuccess && errors != null && errors.Any())
            throw new InvalidOperationException();
        if (!isSuccess && (errors is null || !errors.Any()))
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Value = value;
        Errors = errors;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(string error) => new(false, default, new[] { error });
    public static Result<T> Failure(IEnumerable<string> errors) => new(false, default, errors);
}


public class Result
{
    public bool IsSuccess { get; }
    public IEnumerable<string>? Errors { get; }

    protected Result(bool isSuccess, IEnumerable<string>? errors)
    {
        if (isSuccess && errors != null && errors.Any())
            throw new InvalidOperationException();
        if (!isSuccess && (errors is null || !errors.Any()))
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, null);

    public static Result Failure(string error) => new(false, new[] { error });
    public static Result Failure(IEnumerable<string> errors) => new(false, errors);
}
