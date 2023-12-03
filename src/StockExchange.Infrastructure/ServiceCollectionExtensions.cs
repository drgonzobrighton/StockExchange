using Microsoft.Extensions.DependencyInjection;
using StockExchange.Application.StockSnapshots;
using StockExchange.Application.Trades;

namespace StockExchange.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddSingleton<IStockSnapshotRepository, InMemoryStockSnapshotRepository>()
            .AddSingleton<ITradeRepository, InMemoryTradeRepository>();
    }
}