using Microsoft.AspNetCore.Mvc;
using StockExchange.Application.Common;
using StockExchange.Application.Stocks;

namespace StockExchange.Api.Controllers;

[ApiController]
[Route("api/stocks")]
public class StocksController : ControllerBase
{
    [HttpGet]
    [Route("{tickerSymbol}")]
    [ProducesResponseType<Stock>(StatusCodes.Status200OK)]
    [ProducesResponseType<ResultErrors>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromServices] GetStockQueryHandler handler,
        string tickerSymbol,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(new GetStockQuery(tickerSymbol), cancellationToken);

        return result.Map<IActionResult>(
            stock => stock is not null ? Ok(stock) : NotFound(),
            err => BadRequest(err));
    }

    [HttpGet]
    [Route("all")]
    [ProducesResponseType<List<Stock>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] GetAllStocksQueryHandler handler,
        CancellationToken cancellationToken = default)
    {
        var stocks = await handler.Handle(cancellationToken);

        return Ok(stocks);
    }

    [HttpGet]
    [Route("range/{tickerSymbols}")]
    [ProducesResponseType<List<Stock>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ResultErrors>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetRange(
        [FromServices] GetStocksBySymbolsQueryHandler handler,
        string tickerSymbols,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(new GetStocksBySymbolsQuery(tickerSymbols), cancellationToken);

        return result.Map<IActionResult>(
            stock => Ok(stock),
            err => BadRequest(err));
    }
}
