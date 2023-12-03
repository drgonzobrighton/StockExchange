using StockExchange.Application.StockSnapshots;

namespace StockExchange.Application.Stocks;

public sealed class GetAllStocksQueryHandler(IStockSnapshotRepository repository)
{
    public async Task<List<Stock>> Handle(CancellationToken cancellationToken = default)
    {
        var snapshots = await repository.GetLatest(cancellationToken);

        return snapshots.Select(Stock.FromSnapshot).ToList();
    }
}
