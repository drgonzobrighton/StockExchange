﻿using StockExchange.Application.Stocks;
using StockExchange.Application.StockSnapshots;

namespace StockExchange.Application.UnitTests.Stocks;

public class GetBySymbolsTests
{
    private readonly GetStocksBySymbolsQueryHandler _sut;
    private readonly Mock<IStockSnapshotRepository> _snapshotRepository;

    private const string Apple = "AAPL";

    public GetBySymbolsTests()
    {
        _snapshotRepository = new();
        _sut = new(_snapshotRepository.Object);
    }


    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(", ")]
    [InlineData(", ,  ")]
    public async Task ReturnsError_WhenSymbolsAreNotValid(string symbols)
    {
        //Act
        var result = await _sut.Handle(new GetStocksBySymbolsQuery(symbols));

        //Assert
        result.AssertFailure("At least one ticker symbol is required");
    }
}

