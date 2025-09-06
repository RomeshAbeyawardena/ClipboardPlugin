namespace ClipboardPlugin;

internal interface IKeyValueRepository
{
    Task UpsertAsync((string ,string?) keyValuePair, CancellationToken cancellationToken);
    Task UpsertAsync(string key, string value, CancellationToken cancellationToken);
    Task<(string, string?)?> GetAsync(string key, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
