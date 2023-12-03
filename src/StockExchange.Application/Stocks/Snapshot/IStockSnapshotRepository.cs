namespace StockExchange.Application.Stocks.Snapshot;

public interface IStockSnapshotRepository
{
    Task Add(StockSnapshot snapshot, CancellationToken cancellationToken = default);
    Task<StockSnapshot?> GetLatest(string tickerSymbol, CancellationToken cancellationToken = default);
}
