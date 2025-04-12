using System.Text.Json;
using HealthMed.Domain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;

namespace HealthMed.Caching.Extensions;

public static class DistributedCacheExtensions
{
    public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data,
        TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null,
        CancellationToken cancellationToken = default)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromMinutes(1),
            SlidingExpiration = unusedExpireTime
        };

        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(recordId, jsonData, options, cancellationToken);
    }

    public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId,
        CancellationToken cancellationToken = default)
    {
        var jsonData = await cache.GetStringAsync(recordId, cancellationToken)
                       ?? throw new NotFoundException($"Registro '{recordId}' não encontrado.");

        return (string.IsNullOrEmpty(jsonData) ? default! : JsonSerializer.Deserialize<T>(jsonData)) ??
               throw new InvalidOperationException("Não foi possível deserializar o registro.");
    }

    public static async Task RemoveRecordAsync(this IDistributedCache cache, string recordId,
        CancellationToken cancellationToken = default)
        => await cache.RemoveAsync(recordId, cancellationToken);
}