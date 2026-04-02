namespace Korp.Shared.Abstractions;

public readonly record struct Result<T, E>
{
    public readonly bool IsSuccess;
    public bool IsFailure { get => !IsSuccess; }

    public readonly E? Error;
    public readonly T? Value;

    private Result(T? value, bool isSuccess, E? error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<T, E> Success(T value) => new Result<T, E>(value, true, default);
    public static Result<T, E> Fail(E error) => new Result<T, E>(default, false, error);


    public static implicit operator Result<T, E>(T value) => Success(value);
    public static implicit operator Result<T, E>(E error) => Fail(error);

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<E, TResult> onFailure)
        => IsSuccess ? onSuccess(Value!) : onFailure(Error!);
}
