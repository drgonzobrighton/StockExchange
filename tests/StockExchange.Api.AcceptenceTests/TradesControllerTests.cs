using Microsoft.AspNetCore.Mvc.Testing;
using StockExchange.Application.Trades;
using System.Net;
using System.Net.Http.Json;

namespace StockExchange.Api.AcceptenceTests;

public class TradesControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;

    public TradesControllerTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task CreateReturnsAccepted_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateTradeCommand("aapl", "buy", 10, 1, Guid.NewGuid(), DateTime.UtcNow);

        //Act
        var response = await _httpClient.PostAsJsonAsync("api/trades/", command);

        //Assert
        Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
    }


    [Fact]
    public async Task CreateReturnsBadRequest_WhenCommandIsNotValid()
    {
        //Arrange
        var command = new CreateTradeCommand(string.Empty, "buy", 10, 1, Guid.NewGuid(), DateTime.UtcNow);

        //Act
        var response = await _httpClient.PostAsJsonAsync("api/trades/", command);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}