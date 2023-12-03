using StockExchange.Application.StockSnapshots;
using System.Text.Json.Serialization;

namespace StockExchange.Application.Stocks;

public class Stock
{
    internal Stock(string tickerSymbol, decimal value, DateTime timestamp)
    {
        Value = value;
        TickerSymbol = tickerSymbol;
        Timestamp = timestamp;
    }

    [JsonConstructor]
    private Stock()
    {
    }

    [JsonInclude]
    public string TickerSymbol { get; private set; }

    [JsonInclude]
    public decimal Value { get; private set; }

    [JsonInclude]
    public DateTime Timestamp { get; private set; }

    internal static Stock FromSnapshot(StockSnapshot snapshot)
    {
        var stockPrice = snapshot.TotalShares != 0 ? snapshot.TotalValue / snapshot.TotalShares : 0;

        return new(snapshot.TickerSymbol, decimal.Round(stockPrice, 2), snapshot.LatestTradeDate);
    }
}
