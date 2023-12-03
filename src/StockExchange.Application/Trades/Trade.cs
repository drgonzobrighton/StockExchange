namespace StockExchange.Application.Trades;

public sealed class Trade
{
    public Guid Id { get; internal init; }
    public string TickerSymbol { get; internal init; }
    public TradeType Type { get; internal init; }
    public decimal Price { get; internal init; }
    public decimal NumberOfShares { get; internal init; }
    public Guid BrokerId { get; internal init; }
    public DateTime CreatedAt { get; internal init; }
}

public enum TradeType
{
    Unknown,
    Sell,
    Buy
}

