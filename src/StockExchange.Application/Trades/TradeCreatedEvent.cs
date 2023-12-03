using MediatR;

namespace StockExchange.Application.Trades;

public sealed record TradeCreatedEvent(string TickerSymbol) : INotification;

