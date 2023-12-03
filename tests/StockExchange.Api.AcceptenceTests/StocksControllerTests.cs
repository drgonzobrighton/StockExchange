using Microsoft.AspNetCore.Mvc.Testing;
using StockExchange.Application.Stocks;
using StockExchange.Application.Trades;
using System.Net.Http.Json;
using System.Text.Json;

namespace StockExchange.Api.AcceptenceTests;

public class StocksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    private const string Apple = "aapl";

    public StocksControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Get_ReturnsCorrectValue()
    {
        await AddTrades();

        var response = await _httpClient.GetAsync($"api/stocks/{Apple}");

        var stock = await response.Content.ReadFromJsonAsync<Stock>(new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        });

        Assert.NotNull(stock);
        Assert.Equal(GetStockValue(), stock.Value);
    }

    private async Task AddTrades()
    {
        foreach (var command in _createTradeCommands)
        {
            await _httpClient.PostAsJsonAsync("api/trades/", command);
        }
    }

    private readonly List<CreateTradeCommand> _createTradeCommands = new()
    {
        new(Apple, "buy",10,2, Guid.NewGuid(), new(2023,12,1)),
        new(Apple, "buy",15,2, Guid.NewGuid(), new(2023,12,2)),
        new(Apple, "buy",30,2, Guid.NewGuid(), new(2023,12,4)),
    };

    private decimal GetStockValue()
        => decimal.Round(
            _createTradeCommands.Select(x => x.NumberOfShares * x.Price).Sum() / _createTradeCommands.Select(x => x.NumberOfShares).Sum(), 2);
}
