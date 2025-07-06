using System.Diagnostics.CodeAnalysis;

namespace Domain;

public abstract class ResultBase(Dictionary<string, string> errors)
{
    public Dictionary<string, string> Errors { get; } = errors;
}

public class Result<T> : ResultBase
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess => Value != null;
    public T? Value { get; }
    
    private Result(T? value, Dictionary<string, string> errors) : base(errors)
    {
        Value = value;
    }
    
    public static Result<T> SetValue(T value) => new(value, []);
    public static Result<T> Failure(Dictionary<string, string> errors) => new(default, errors);
}

public class Result : ResultBase
{
    public bool IsSuccess { get; }

    private Result(bool isSuccess, Dictionary<string, string> errors) : base(errors)
    {
        IsSuccess = isSuccess;
    }
        
    public static Result Success() => new(true, []);
    public static Result Failure(Dictionary<string, string> errors) => new(false, errors);
}
