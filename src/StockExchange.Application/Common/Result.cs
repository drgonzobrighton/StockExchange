using System.Collections;

namespace StockExchange.Application.Common;

public class Result<T>
{
    private readonly bool _success;
    private readonly T _value;
    private readonly ResultErrors _errors;

    public Result(T value)
    {
        _success = true;
        _value = value;
        _errors = null!;
    }

    public Result(ResultErrors errors)
    {
        _success = false;
        _errors = errors;
        _value = default!;
    }

    public static implicit operator Result<T>(T val) => new(val);
    public static implicit operator Result<T>(ResultErrors err) => new(err);

    public static Result<T> Default => new(default(T)!);

    public void Do(Action<T> success, Action<ResultErrors> failure)
    {
        if (_success)
            success(_value);
        else
            failure(_errors);
    }

    public TResult Map<TResult>(Func<T, TResult> success, Func<ResultErrors, TResult> failure)
        => _success ? success(_value) : failure(_errors);

    public Result<T> Then(Func<T, Result<T>> next)
        => _success ? next(_value) : this;

    public async Task<Result<T>> Then(Func<T, Task<Result<T>>> next)
        => _success ? await next(_value) : this;

    internal bool Success => _success;
}

public class ResultErrors : IEnumerable<string>
{
    private List<string> _errors;

    public ResultErrors(params string[] errors) : this() => _errors.AddRange(errors);
    private ResultErrors() => _errors = new();

    public static ResultErrors Empty => new();
    public void AddError(string error) => _errors.Add(error);
    public void AddErrors(ResultErrors errors) => _errors.AddRange(errors);

    public IEnumerator<string> GetEnumerator() => _errors.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _errors.GetEnumerator();
}