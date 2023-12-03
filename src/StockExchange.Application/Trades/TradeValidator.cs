using StockExchange.Application.Common;

namespace StockExchange.Application.Trades;

internal static class TradeValidator
{
    internal static Result<Trade> Validate(CreateTradeCommand command)
    {
        var validationErrors = ResultErrors.Empty;

        if (string.IsNullOrWhiteSpace(command.TickerSymbol))
            validationErrors.AddError("Ticker symbol is required");

        if (command.Type is TradeType.Unknown)
            validationErrors.AddError("Type is required");

        if (command.Price <= 0)
            validationErrors.AddError("Price must be greater than 0");

        if (command.NumberOfShares <= 0)
            validationErrors.AddError("Number of shares must be greater than 0");

        if (command.BrokerId == Guid.Empty)
            validationErrors.AddError("Broker id is required");

        if (command.CreatedAt == default)
            validationErrors.AddError("Created date is required");

        return validationErrors.Any() ? validationErrors : Result<Trade>.Default;
    }
}
