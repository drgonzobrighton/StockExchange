using Microsoft.Extensions.DependencyInjection;
using StockExchange.Application.Stocks;
using StockExchange.Application.Trades;

namespace StockExchange.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<CreateTradeCommandHandler>()
            .AddScoped<GetStockQueryHandler>()
            .AddScoped<GetAllStockQueryHandler>()
            .AddScoped<GetStockRangeQueryHandler>();
    }
}
