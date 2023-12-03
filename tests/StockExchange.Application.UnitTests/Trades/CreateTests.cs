using StockExchange.Application.Trades;

namespace StockExchange.Application.UnitTests.Trades;

public class CreateTests
{
    private readonly CreateTradeCommandHandler _sut;
    private readonly Mock<ITradeRepository> _tradeRepository;
    private static readonly Trade ValidTrade = new Trade
    {
        Id = Guid.NewGuid(),
        TickerSymbol = "AAPL",
        Price = 200m,
        Type = TradeType.Sell,
        NumberOfShares = 3,
        BrokerId = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
    };

    public CreateTests()
    {
        _tradeRepository = new();
        _sut = new(_tradeRepository.Object);

        _tradeRepository
            .Setup(x => x.Add(
                It.Is<Trade>(t =>
                t.TickerSymbol == ValidTrade.TickerSymbol),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValidTrade);
    }

    [Fact]
    public async Task Succeeds_WhenParamsAreValid()
    {
        //Arrange
        var command = new CreateTradeCommand(
            ValidTrade.TickerSymbol,
            ValidTrade.Type,
            ValidTrade.Price,
            ValidTrade.NumberOfShares,
            ValidTrade.BrokerId,
            ValidTrade.CreatedAt);

        //Act
        var result = await _sut.Handle(command);

        //Assert
        result.AssertSuccess(trade => trade.Should().BeEquivalentTo(ValidTrade, opt => opt.Excluding(t => t.Id)));
    }

    [Fact]
    public async Task Fails_WhenParamsAreNotValid()
    {
        //Arrange
        var command = new CreateTradeCommand(
            "",
            ValidTrade.Type,
            ValidTrade.Price,
            ValidTrade.NumberOfShares,
            ValidTrade.BrokerId,
            ValidTrade.CreatedAt);

        //Act
        var result = await _sut.Handle(command);

        //Assert
        result.AssertFailure("Ticker symbol is required");
    }
}
