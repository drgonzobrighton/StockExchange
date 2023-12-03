using StockExchange.Application.Common;
using StockExchange.Application.StockSnapshots;

namespace StockExchange.Application.Stocks;

public sealed record GetStockQuery(string TickerSymbol);

public sealed class GetStockQueryHandler(IStockSnapshotRepository repository)
{
    public async Task<Result<Stock?>> Handle(GetStockQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.TickerSymbol))
            return new ResultErrors("Ticker symbol is required");

        var latestSnapshot = await repository.GetLatest(query.TickerSymbol, cancellationToken);

        if (latestSnapshot is null)
            return Result<Stock?>.Default;

        return Stock.FromSnapshot(latestSnapshot);
    }
}
