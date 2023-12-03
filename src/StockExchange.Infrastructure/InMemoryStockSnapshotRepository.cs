﻿using StockExchange.Application.StockSnapshots;
using System.Collections.Concurrent;

namespace StockExchange.Infrastructure;

public class InMemoryStockSnapshotRepository : IStockSnapshotRepository
{
    private readonly ConcurrentDictionary<string, List<StockSnapshot>> _snapshots = new();

    public Task Add(StockSnapshot snapshot, CancellationToken cancellationToken = default)
    {
        var key = snapshot.TickerSymbol.ToUpper();

        if (!_snapshots.ContainsKey(key))
        {
            _snapshots[key] = new List<StockSnapshot>();
        }

        _snapshots[key].Add(snapshot);

        return Task.CompletedTask;
    }

    public Task<StockSnapshot?> GetLatest(string tickerSymbol, CancellationToken cancellationToken = default)
    {
        var key = tickerSymbol.ToUpper();

        if (!_snapshots.TryGetValue(key, out var snapshots))
            return Task.FromResult((StockSnapshot?)null!);

        return Task.FromResult(snapshots.Last());
    }

    public Task<List<StockSnapshot>> GetAll(CancellationToken cancellationToken = default)
    {
        var snapshots = _snapshots.Values.Select(x => x.Last()).ToList();

        return Task.FromResult(snapshots);
    }

    public Task<List<StockSnapshot>> GetRange(string[] tickerSymbols, CancellationToken cancellationToken = default)
    {
        var snapshots = _snapshots
            .Where(x => tickerSymbols.Select(s => s.ToUpper()).Contains(x.Key))
            .Select(x => x.Value.Last())
            .ToList();

        return Task.FromResult(snapshots);
    }
}
