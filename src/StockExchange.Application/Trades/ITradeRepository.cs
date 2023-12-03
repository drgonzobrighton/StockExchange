using StockExchange.Application.Common;

namespace StockExchange.Application.Trades;

public interface ITradeRepository
{
    Task<Result<Trade>> Add(Trade trade, CancellationToken cancellationToken);
    Task<List<Trade>> GetAll(string tickerSymbol, DateTime? from = null);
}
