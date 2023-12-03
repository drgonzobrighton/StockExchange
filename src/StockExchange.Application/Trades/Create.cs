using MediatR;
using StockExchange.Application.Common;

namespace StockExchange.Application.Trades;

public sealed record CreateTradeCommand(string TickerSymbol, string Type, decimal Price, decimal NumberOfShares, Guid BrokerId, DateTime CreatedAt);

public sealed class CreateTradeCommandHandler(ITradeRepository tradeRepository, IPublisher publisher)
{
    public async Task<Result<Trade>> Handle(CreateTradeCommand command, CancellationToken cancellationToken = default)
    {
        var result = await TradeValidator.Validate(command)
            .Then(_ =>
            {
                var trade = new Trade
                {
                    Id = Guid.NewGuid(),
                    TickerSymbol = command.TickerSymbol,
                    Type = Enum.Parse<TradeType>(command.Type, true),
                    Price = command.Price,
                    NumberOfShares = command.NumberOfShares,
                    BrokerId = command.BrokerId,
                    CreatedAt = command.CreatedAt
                };

                return trade;
            })
            .Then(trade => tradeRepository.Add(trade, cancellationToken));

        if (result.Success)
        {
            await publisher.Publish(new TradeCreatedEvent(command.TickerSymbol), cancellationToken);
        }

        return result;
    }
}