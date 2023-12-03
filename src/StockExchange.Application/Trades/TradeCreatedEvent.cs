using MediatR;

namespace StockExchange.Application.Trades;

public record TradeCreatedEvent(string TickerSymbol) : INotification;

