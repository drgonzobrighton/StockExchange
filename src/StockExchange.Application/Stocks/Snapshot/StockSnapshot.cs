using StockExchange.Application.Trades;

namespace StockExchange.Application.Stocks.Snapshot;

public class StockSnapshot
{
    public Guid Id { get; internal init; }
    public string TickerSymbol { get; internal init; }
    public decimal TotalValue { get; internal set; }
    public decimal TotalShares { get; internal set; }
    public DateTime LatestTradeDate { get; internal set; }

    public void Apply(Trade trade)
    {
        var multiplicator = trade.Type switch
        {
            TradeType.Buy => 1,
            TradeType.Sell => -1,
        };

        var tradeValue = trade.NumberOfShares * trade.Price;
        TotalShares += trade.NumberOfShares * multiplicator;
        TotalValue += tradeValue * multiplicator;
    }
}
