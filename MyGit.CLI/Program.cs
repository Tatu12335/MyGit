using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyGit.CLI;
using MyGit.Core.Application.Interfaces.HandleFiles;
using MyGit.Core.Application.Services;
using MyGit.Core.Infrastructure.Services.HandleFiles;
using MyGit.Core.Middleware;
using Spectre.Console.Cli;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddTransient<IHandleFiles, Assemble_RawFileData>();
        services.AddTransient<FileHandlingOrchestration>();
        services.AddLogging(configure => configure.AddConsole());
        services.AddTransient<InitCommand>();
        services.AddTransient<HashObjectCommand>();

        var register = new TypeRegistrar(services);

        var ui = new CommandApp(register);

        ui.Configure(config =>
        {
            config.SetApplicationName("MyGit");
            config.AddCommand<InitCommand>("init")
                .WithDescription("Creates a new empty repository");

            config.AddCommand<HashObjectCommand>("hash-object")
                .WithDescription("Calculates the hash of a file and stores it if needed");
        });
        return await ui.RunAsync(args);
    }
}
