 // See https://aka.ms/new-console-template for more information
using ClipboardPlugin;
using ClipboardPlugin.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


void ConfigureServices(HostBuilderContext context, IServiceCollection services)
{
    services
        .AddSingleton((s) => new ClipboardArguments(args))
        .AddCommands()
        .AddServices()
        .AddSingleton(s => IoStream.ConsoleStream());
}

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(x => x.AddJsonFile("appsettings.json").AddUserSecrets(typeof(Program).Assembly, true))
    .ConfigureLogging(x => x.AddConsole())
    .ConfigureServices(ConfigureServices)
    .Build();

var clipboard = host.Services.GetRequiredService<ICommandParser<ClipboardArguments>>();
await clipboard.ExecuteAsync(CancellationToken.None);
