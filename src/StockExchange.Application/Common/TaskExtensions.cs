namespace StockExchange.Application.Common;

public static class TaskExtensions
{
    public static async Task<Result<T>> Then<T>(this Task<Result<T>> task, Func<T, Task<Result<T>>> next)
    {
        var taskResult = await task;
        return taskResult.TryGetValue(out var result) ? await next(result) : taskResult;
    }

    public static async Task<Result<T>> Then<T>(this Task<Result<T>> task, Func<T, Result<T>> next)
    {
        var taskResult = await task;
        return taskResult.TryGetValue(out var result) ? next(result) : taskResult;
    }

    public static async Task<Result<TNew>> Convert<TOriginal, TNew>(
        this Task<Result<TOriginal>> task,
        Func<TOriginal, Result<TNew>> next)
    {
        var taskResult = await task;
        return taskResult.TryGetValue(out var result) ? next(result) : new Result<TNew>(taskResult.GetErrors());
    }

    public static async Task<Result<TNew>> Convert<TOriginal, TNew>(
        this Task<Result<TOriginal>> task,
        Func<TOriginal, Task<Result<TNew>>> next)
    {
        var taskResult = await task;
        return taskResult.TryGetValue(out var result) ? await next(result) : new Result<TNew>(taskResult.GetErrors());
    }

    public static async Task<Result<T>> AsResult<T>(this Task<T?> task, string? failureMessage = null)
    {
        var obj = await task;
        return obj is null
            ? new ResultErrors(failureMessage ?? $"{typeof(T).Name} not found")
            : new Result<T>(obj);
    }
}
