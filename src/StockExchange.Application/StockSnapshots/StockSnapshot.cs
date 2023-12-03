using StockExchange.Application.Trades;
using System.Diagnostics;

namespace StockExchange.Application.StockSnapshots;

public sealed class StockSnapshot
{
    public Guid Id { get; internal init; }
    public string TickerSymbol { get; internal init; }
    public decimal TotalValue { get; internal set; }
    public decimal TotalShares { get; internal set; }
    public DateTime LatestTradeDate { get; internal set; }

    public void Apply(Trade trade)
    {
        var multiplicand = trade.Type switch
        {
            TradeType.Buy => 1,
            TradeType.Sell => -1,
            _ => throw new UnreachableException()
        };

        var tradeValue = trade.NumberOfShares * trade.Price;
        TotalShares += trade.NumberOfShares * multiplicand;
        TotalValue += tradeValue * multiplicand;
    }
}
