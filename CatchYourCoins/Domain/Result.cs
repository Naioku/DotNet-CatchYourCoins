namespace Domain;

public class Result<T>
{
    public bool IsSuccess => Value != null;
    public T? Value { get; }
    public Dictionary<string, string> Errors { get; }
    
    private Result(T? value, Dictionary<string, string> errors)
    {
        Value = value;
        Errors = errors;
    }
    
    public static Result<T> SetValue(T value) => new(value, []);
    public static Result<T> Failure(Dictionary<string, string> errors) => new(default, errors);
}

public class Result
{
    public bool IsSuccess { get; }
    public Dictionary<string, string> Errors { get; }
        
    private Result(bool isSuccess, Dictionary<string, string> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }
        
    public static Result Success() => new(true, []);
    public static Result Failure(Dictionary<string, string> errors) => new(false, errors);
}
