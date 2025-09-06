using System.Collections.Concurrent;
using System.Text.Json;

namespace ClipboardPlugin;

internal class JsonFileKeyValueRepository(string path, IFileProvider fileProvider) : IKeyValueRepository
{
    private readonly ConcurrentDictionary<string, string> jsonKeyValueCache = [];
    private bool IsLoaded { get; } = false;

    public async Task Load(bool inValidate = false, CancellationToken cancellationToken)
    {
        if (IsLoaded && !inValidate)
        {
            return;
        }

        if (inValidate)
        {
            jsonKeyValueCache.Clear();
        }

        var elements = JsonSerializer.Deserialize<IDictionary<string, string>>(
            await fileProvider.GetTextAsync(path, cancellationToken));

        if(elements is null)
        {
            return;
        }

        foreach(var (key, value) in elements)
        {
            jsonKeyValueCache.TryAdd(key, value);
        }
    }

    public async Task<(string, string?)?> GetAsync(string key, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (jsonKeyValueCache.TryGetValue(key, out var value))
        {
            return (key, value);
        }

        return null;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var serialized = JsonSerializer.Serialize(jsonKeyValueCache);
        await fileProvider.SetTextAsync(path, serialized, cancellationToken);
        return 1;
    }

    public Task UpsertAsync((string, string?) keyValuePair, CancellationToken cancellationToken)
    {
        var (key, value) = keyValuePair;

        if(value is null)
        {
            return Task.CompletedTask;
        }

        return UpsertAsync(key, value, cancellationToken);
    }

    public Task UpsertAsync(string key, string value, CancellationToken cancellationToken)
    {
        if (!jsonKeyValueCache.TryAdd(key, value))
        {
            jsonKeyValueCache[key] = value;
        }

        return Task.CompletedTask;
    }
}