using MediatR;
using StockExchange.Application.Trades;

namespace StockExchange.Application.StockSnapshots;

public sealed class TradeCreatedEventHandler(ITradeRepository tradeRepository, IStockSnapshotRepository stockSnapshotRepository)
    : INotificationHandler<TradeCreatedEvent>
{
    public async Task Handle(TradeCreatedEvent notification, CancellationToken cancellationToken)
    {
        var latestSnapshot = await stockSnapshotRepository.GetLatest(notification.TickerSymbol, cancellationToken);

        var newSnapshot = Create(notification.TickerSymbol, latestSnapshot);

        var trades = await tradeRepository.GetAll(notification.TickerSymbol, latestSnapshot?.LatestTradeDate);

        var latestTradeDate = DateTime.UtcNow;

        foreach (var trade in trades)
        {
            newSnapshot.Apply(trade);
            latestTradeDate = trade.CreatedAt;
        }

        newSnapshot.LatestTradeDate = latestTradeDate;

        await stockSnapshotRepository.Add(newSnapshot, cancellationToken);
    }

    private static StockSnapshot Create(string notificationTickerSymbol, StockSnapshot? latestSnapshot)
    {
        return new()
        {
            Id = Guid.NewGuid(),
            TickerSymbol = notificationTickerSymbol.ToUpper(),
            TotalShares = latestSnapshot?.TotalShares ?? 0,
            TotalValue = latestSnapshot?.TotalValue ?? 0
        };
    }
}
