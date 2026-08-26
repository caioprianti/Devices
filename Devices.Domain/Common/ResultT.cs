namespace Devices.Domain.Common;

public sealed class ResultT<T> : Result
{
    public T? Value { get; }
    
    private ResultT(T value) : base()
    {
        Value = value;
    }

    private ResultT(Error error) : base(error)
    {
    }

    public static ResultT<T> Success(T value)
        => new(value);
    
    public new static ResultT<T> Failure(Error error)
        => new(error);
}