namespace Matt.ResultObject;

public class Result<T> : ResultBase, IHasErrorDetail where T : class
{
    private readonly T? _value;

    public T? Value => IsSuccess ? _value : null;

    private Result(T? value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    /// <summary>
    /// Static factory method to create a successful result.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(value, true, Error.None);
    }

    /// <summary>
    /// Static factory method to create a successful result with a message.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Result<T> Success(T value, string message)
    {
        return new Result<T>(value, true, Error.None) { DisplayMessage = message };
    }

    /// <summary>
    /// Static factory method to create a failed result with an error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> Fail(Error error)
    {
        return new Result<T>(null, false, error);
    }

    /// <summary>
    /// Static factory method to create a failed result with an error.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result<T> Fail(T value, Error error)
    {
        return new Result<T>(value, false, error);
    }

    /// <summary>
    /// Static factory method to create a failed result with an error message.
    /// It calls Result.Fail(string errorMessage) method.
    /// Then the result is converted to Result<![CDATA[T]]> >implicitly.
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static Result<T> Fail(string errorMessage)
    {
        return Result.Fail(errorMessage);
    }

    #region implicit operators

    /// <summary>
    /// A result object can be implicitly converted to Result<![CDATA[T]]>.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(Result result)
    {
        return new Result<T>(default, result.IsSuccess, result.Error);
    }

    /// <summary>
    /// Implicitly convert an exception to a failed result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(Exception error)
    {
        return new Result<T>(default, false, new Error("Unexpected error with exception", error.Message));
    }

    /// <summary>
    /// Implicitly convert a value to its possible result.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(T? value)
    {
        return value is not null
            ? new Result<T>(value, true, Error.None)
            : new Result<T>(default, false, Error.NullValue);
    }

    /// <summary>
    /// Implicitly convert an error to a failed result.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(null, false, error);
    }

    #endregion


    public override Result<T> WithError(Error error)
    {
        Errors.Add(error);
        return this;
    }

    public override Result<T> WithError(Exception error)
    {
        Errors.Add(
            new Error("Unexpected error with exception", error.Message)
        );
        return this;
    }
}