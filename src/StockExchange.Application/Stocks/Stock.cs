using StockExchange.Application.StockSnapshots;

namespace StockExchange.Application.Stocks;

public class Stock
{
    internal Stock(string tickerSymbol, decimal value, DateTime timestamp)
    {
        Value = value;
        TickerSymbol = tickerSymbol;
        Timestamp = timestamp;
    }

    public string TickerSymbol { get; }
    public decimal Value { get; }
    public DateTime Timestamp { get; }

    public static Stock FromSnapshot(StockSnapshot snapshot)
    {
        var stockPrice = snapshot.TotalShares != 0 ? snapshot.TotalValue / snapshot.TotalShares : 0;

        return new(snapshot.TickerSymbol, decimal.Round(stockPrice, 2), snapshot.LatestTradeDate);
    }
}
