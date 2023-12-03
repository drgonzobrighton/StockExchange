using FluentAssertions.Execution;
using StockExchange.Application.Common;

namespace StockExchange.Application.UnitTests;

public static class ResultExtensions
{
    public static void AssertSuccess<T>(this Result<T> result, Action<T> successFunc)
    {
        result.Do(
            successFunc,
            _ => throw new AssertionFailedException("should not be an error"));
    }

    public static void AssertFailure<T>(this Result<T> result, params string[] errors)
    {
        result.Do(
            _ => throw new AssertionFailedException("should not be success"),
            err => err.Should().BeEquivalentTo(errors));
    }
}
