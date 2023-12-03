namespace StockExchange.Application.StockSnapshots;

public interface IStockSnapshotRepository
{
    Task Add(StockSnapshot snapshot, CancellationToken cancellationToken = default);
    Task<StockSnapshot?> GetLatest(string tickerSymbol, CancellationToken cancellationToken = default);
    Task<List<StockSnapshot>> GetAll(CancellationToken cancellationToken = default);
    Task<List<StockSnapshot>> GetRange(string[] tickerSymbols, CancellationToken cancellationToken = default);
}
