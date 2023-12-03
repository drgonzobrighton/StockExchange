using StockExchange.Application.Common;
using StockExchange.Application.StockSnapshots;

namespace StockExchange.Application.Stocks;

public record GetStocksBySymbolsQuery(string Symbols);

public sealed class GetStocksBySymbolsQueryHandler(IStockSnapshotRepository repository)
{
    public async Task<Result<List<Stock>>> Handle(GetStocksBySymbolsQuery query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Symbols))
            return new ResultErrors("At least one ticker symbol is required");

        var symbols = query.Symbols.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

        if (!symbols.Any())
            return new ResultErrors("At least one ticker symbol is required");

        var snapshots = await repository.GetLatest(symbols, cancellationToken);

        return snapshots.Select(Stock.FromSnapshot).ToList();
    }
}
