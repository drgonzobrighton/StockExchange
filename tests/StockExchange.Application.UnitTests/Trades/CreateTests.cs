using MediatR;
using StockExchange.Application.Common;
using StockExchange.Application.Trades;

namespace StockExchange.Application.UnitTests.Trades;

public class CreateTests
{
    private readonly CreateTradeCommandHandler _sut;
    private readonly Mock<ITradeRepository> _tradeRepository;
    private readonly Mock<IPublisher> _publisher;
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
        _publisher = new Mock<IPublisher>();
        _sut = new(_tradeRepository.Object, _publisher.Object);

        _tradeRepository
            .Setup(x => x.Add(
                It.Is<Trade>(t =>
                    t.TickerSymbol == ValidTrade.TickerSymbol),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValidTrade);

        _publisher
            .Setup(p => p.Publish(It.IsAny<TradeCreatedEvent>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
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

        _publisher.Verify(p => p.Publish(It.Is<TradeCreatedEvent>(t => t.TickerSymbol == command.TickerSymbol), It.IsAny<CancellationToken>()));
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

        _publisher.Verify(p => p.Publish(It.Is<TradeCreatedEvent>(t => t.TickerSymbol == command.TickerSymbol), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Fails_WhenRepositoryReturnsError()
    {
        //Arrange
        var repoErrorMessage = "Oops, something went wrong";
        var command = new CreateTradeCommand(
            ValidTrade.TickerSymbol,
            ValidTrade.Type,
            ValidTrade.Price,
            ValidTrade.NumberOfShares,
            ValidTrade.BrokerId,
            ValidTrade.CreatedAt);

        _tradeRepository
            .Setup(x => x.Add(
                It.IsAny<Trade>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ResultErrors(repoErrorMessage));

        //Act
        var result = await _sut.Handle(command);

        //Assert
        result.AssertFailure(repoErrorMessage);

        _publisher.Verify(p => p.Publish(It.Is<TradeCreatedEvent>(t => t.TickerSymbol == command.TickerSymbol), It.IsAny<CancellationToken>()), Times.Never);

    }
}
