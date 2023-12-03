using Microsoft.AspNetCore.Mvc;
using StockExchange.Application.Common;
using StockExchange.Application.Trades;

namespace StockExchange.Api.Controllers;

[ApiController]
[Route("api/trades")]
public class TradesController : ControllerBase
{

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType<ResultErrors>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] CreateTradeCommandHandler handler,
        CreateTradeCommand command,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(command, cancellationToken);

        return result.Map<IActionResult>(
            _ => Accepted(),
            err => BadRequest(err));
    }
}
