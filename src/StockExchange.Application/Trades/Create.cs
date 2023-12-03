using StockExchange.Application.Common;

namespace StockExchange.Application.Trades;

public sealed record CreateTradeCommand(string TickerSymbol, TradeType Type, decimal Price, decimal NumberOfShares, Guid BrokerId, DateTime CreatedAt);

public sealed class CreateTradeCommandHandler
{
    private readonly ITradeRepository _tradeRepository;

    public CreateTradeCommandHandler(ITradeRepository tradeRepository)
    {
        _tradeRepository = tradeRepository;
    }

    public async Task<Result<Trade>> Handle(CreateTradeCommand command, CancellationToken cancellationToken = default)
    {
        var result = await TradeValidator.Validate(command)
            .Then(_ =>
            {
                var trade = new Trade
                {
                    Id = Guid.NewGuid(),
                    TickerSymbol = command.TickerSymbol,
                    Type = command.Type,
                    Price = command.Price,
                    NumberOfShares = command.NumberOfShares,
                    BrokerId = command.BrokerId,
                    CreatedAt = command.CreatedAt
                };

                return trade;
            })
            .Then(trade => _tradeRepository.Add(trade, cancellationToken));

        return result;
    }
}