namespace ClipboardPlugin.Repositories;

internal interface IFileBasedKeyValueRepository : IKeyValueRepository
{
    Task LoadAsync(bool inValidate, CancellationToken cancellationToken);
}

internal interface IKeyValueRepository
{
    Task UpsertAsync((string ,string?) keyValuePair, CancellationToken cancellationToken);
    Task UpsertAsync(string key, string value, CancellationToken cancellationToken);
    Task<(string, string?)?> GetAsync(string key, CancellationToken cancellationToken);
    Task<IEnumerable<(string, string?)>> GetAsync(int? take, CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
