namespace Matt.ResultObject;

public abstract class ResultBase : IResult
{
    public bool IsSuccess { get; private init; }

    public bool IsFailure => !IsSuccess;

    public string DisplayMessage { get; protected init; } = string.Empty;

    public Error Error { get; }

    //TODO: consider to remove this one
    public List<Error> Errors { get; protected set; } = new();

    protected internal ResultBase(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new ArgumentException("Cannot supply error for successful result");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new ArgumentException("Must supply error for failed result");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public abstract ResultBase WithError(Error error);
    public abstract ResultBase WithError(Exception error);
}