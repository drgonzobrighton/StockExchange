using StockExchange.Application.StockSnapshots;
using StockExchange.Application.Trades;

namespace StockExchange.Application.UnitTests.StockSnapshots;

public class TradeCreatedEventHandlerTests
{
    private readonly TradeCreatedEventHandler _sut;
    private readonly Mock<IStockSnapshotRepository> _snapshotRepository;
    private readonly Mock<ITradeRepository> _tradeRepository;

    private const string Apple = "AAPL";

    public TradeCreatedEventHandlerTests()
    {
        _tradeRepository = new();
        _snapshotRepository = new();
        _sut = new(_tradeRepository.Object, _snapshotRepository.Object);
    }

    [Fact]
    public async Task SnapshotOnlyAppliesNewTrades_WhenPreviousSnapshotFound()
    {
        //Arrange
        _snapshotRepository
            .Setup(x => x.GetLatest(Apple, It.IsAny<CancellationToken>()))
            .ReturnsAsync(LatestSnapshot);

        _tradeRepository
            .Setup(x => x.GetAll(Apple, LatestSnapshot.LatestTradeDate))
            .ReturnsAsync(TradesFrom(LatestSnapshot.LatestTradeDate));

        //Act
        await _sut.Handle(new(Apple), default);

        //Assert
        _snapshotRepository
            .Verify(x => x.Add(
                It.Is<StockSnapshot>(s =>
                    s.TickerSymbol == Apple &&
                    s.LatestTradeDate == AllTrades.Last().CreatedAt &&
                    s.TotalShares == GetExpectedStockTotalShares() &&
                    s.TotalValue == GetExpectedStockValue()),
                It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task SnapshotAppliesAllTrades_WhenNoPreviousSnapshotFound()
    {
        //Arrange
        _snapshotRepository
            .Setup(x => x.GetLatest(Apple, It.IsAny<CancellationToken>()))
            .ReturnsAsync((StockSnapshot)null!);

        _tradeRepository
            .Setup(x => x.GetAll(Apple, null))
            .ReturnsAsync(AllTrades);

        //Act
        await _sut.Handle(new(Apple), default);

        //Assert
        _snapshotRepository
            .Verify(x => x.Add(
                It.Is<StockSnapshot>(s =>
                    s.TickerSymbol == Apple &&
                    s.LatestTradeDate == AllTrades.Last().CreatedAt &&
                    s.TotalShares == GetExpectedStockTotalShares() &&
                    s.TotalValue == GetExpectedStockValue()),
                It.IsAny<CancellationToken>()));
    }

    private static StockSnapshot LatestSnapshot = new()
    {
        TickerSymbol = Apple,
        LatestTradeDate = new(2023, 12, 2),
        TotalShares = 6,
        TotalValue = 610
    };

    private static List<Trade> AllTrades => new()
    {
        new()
        {
            TickerSymbol = Apple,
            Price = 100,
            Type = TradeType.Buy,
            NumberOfShares = 5,
            CreatedAt = new(2023,12,1)
        },
        new()
        {
            TickerSymbol = Apple,
            Price = 110,
            Type = TradeType.Buy,
            NumberOfShares = 1,
            CreatedAt = new(2023,12,2)
        },
        new()
        {
            TickerSymbol = Apple,
            Price = 100,
            Type = TradeType.Sell,
            NumberOfShares = 1,
            CreatedAt = new(2023,12,3)
        },
        new()
        {
            TickerSymbol = Apple,
            Price = 90,
            Type = TradeType.Sell,
            NumberOfShares = 3,
            CreatedAt = new(2023,12,4)
        }
    };

    private static List<Trade> TradesFrom(DateTime date) => AllTrades.Where(x => x.CreatedAt > date).ToList();

    private static decimal GetExpectedStockValue()
    {
        var totalValue = 0m;
        foreach (var trade in AllTrades)
        {
            var value = trade.NumberOfShares * trade.Price;

            if (trade.Type is TradeType.Buy)
            {
                totalValue += value;
            }
            else
            {
                totalValue -= value;
            }
        }

        return totalValue;
    }

    private static decimal GetExpectedStockTotalShares()
    {
        var totalShares = 0m;
        foreach (var trade in AllTrades)
        {
            if (trade.Type is TradeType.Buy)
            {
                totalShares += trade.NumberOfShares;
            }
            else
            {
                totalShares -= trade.NumberOfShares;
            }
        }

        return totalShares;
    }
}
