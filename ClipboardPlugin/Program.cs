using ClipboardPlugin.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ClipboardPlugin;

public partial class Program
{
    public async static Task<int> Main(string[] args)
    {
        AddServices(args);
        var startup = serviceProvider!.GetRequiredService<IStartup>();
        await startup.RunAsync();
        return 0;
    }
}