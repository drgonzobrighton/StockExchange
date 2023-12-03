using StockExchange.Application.Stocks;
using StockExchange.Application.StockSnapshots;

namespace StockExchange.Application.UnitTests.Stocks;

public class GetTests
{
    private readonly GetStockQueryHandler _sut;
    private readonly Mock<IStockSnapshotRepository> _snapshotRepository;

    private const string Apple = "AAPL";

    public GetTests()
    {
        _snapshotRepository = new();
        _sut = new(_snapshotRepository.Object);
    }

    [Fact]
    public async Task ReturnsStock_WhenSnapshotFound()
    {
        //Arrange
        var snapshot = new StockSnapshot
        {
            TickerSymbol = Apple,
            TotalShares = 100,
            TotalValue = 1000
        };

        _snapshotRepository
            .Setup(x => x.GetLatest(Apple, It.IsAny<CancellationToken>()))
            .ReturnsAsync(snapshot);

        //Act
        var result = await _sut.Handle(new GetStockQuery(Apple));

        //Assert
        result.AssertSuccess(stock =>
        {
            stock.TickerSymbol.Should().Be(Apple);
            stock.Value.Should().Be(snapshot.TotalValue / snapshot.TotalShares);
            stock.Timestamp.Should().Be(snapshot.LatestTradeDate);
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public async Task ReturnsError_WhenTickerSymbolIsNullOrEmpty(string symbol)
    {
        //Act
        var result = await _sut.Handle(new GetStockQuery(symbol));

        //Assert
        result.AssertFailure("Ticker symbol is required");
    }
}
