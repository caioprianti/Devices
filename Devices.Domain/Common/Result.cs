namespace Devices.Domain.Common;

public class Result
{
    public bool IsSuccess { get; }
    public Error? Error { get; }

    protected Result()
        => IsSuccess = true;

    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }
    
    public static Result Success()
        => new();
    
    public static Result Failure(Error error)
        => new(error);
}



