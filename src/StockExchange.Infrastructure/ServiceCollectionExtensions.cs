using Microsoft.Extensions.DependencyInjection;

namespace StockExchange.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services
            .AddSingleton<InMemoryStockSnapshotRepository>()
            .AddSingleton<InMemoryTradeRepository>();
    }
}