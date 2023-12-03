using Microsoft.AspNetCore.Mvc;
using StockExchange.Application.Common;
using StockExchange.Application.Trades;

namespace StockExchange.Api.Controllers;

[ApiController]
[Route("api/trades")]
public class TradesController : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ResultErrors>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] CreateTradeCommandHandler handler,
        CreateTradeCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.Map<IActionResult>(
            _ => Ok(),
            err => BadRequest(err));
    }
}
