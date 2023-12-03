using StockExchange.Application.Common;
using StockExchange.Application.Trades;
using System.Collections.Concurrent;

namespace StockExchange.Infrastructure;

public class InMemoryTradeRepository : ITradeRepository
{
    private readonly ConcurrentDictionary<string, List<Trade>> _tradeStreams = new();

    public Task<Result<Trade>> Add(Trade trade, CancellationToken cancellationToken)
    {
        var key = trade.TickerSymbol.ToUpper();

        if (!_tradeStreams.ContainsKey(key))
        {
            _tradeStreams[key] = new();
        }

        _tradeStreams[key].Add(trade);

        return Task.FromResult(new Result<Trade>(trade));
    }

    public Task<List<Trade>> GetAll(string tickerSymbol, DateTime? from = null)
    {
        var key = tickerSymbol.ToUpper();

        if (!_tradeStreams.TryGetValue(key, out var trades))
        {
            return Task.FromResult(new List<Trade>());
        }

        if (from is not null)
        {
            trades = trades.Where(x => x.CreatedAt > from.Value).ToList();
        }

        return Task.FromResult(trades);
    }
}
