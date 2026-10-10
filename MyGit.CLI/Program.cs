using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyGit.CLI.Commands;
using MyGit.Core.Application.Interfaces.HandleFiles;
using MyGit.Core.Application.Services;
using MyGit.Core.Infrastructure.Services.HandleFiles;
using MyGit.Core.Infrastructure.Services.NewFolder;
using MyGit.Core.Middleware;
using Spectre.Console;
using Spectre.Console.Cli;

class Program
{
    public static async Task<int> Main(string[] args)
    {
        var services = new ServiceCollection();

        services.AddTransient<IHandleFiles, Assemble_RawFileData>();
        services.AddTransient<ICommit, Commit>();
        services.AddTransient<CommitOrchestrator>();
        services.AddTransient<FileHandlingOrchestration>();
        services.AddLogging(configure => configure.AddConsole());
        services.AddTransient<InitCommand>();
        services.AddTransient<HashObjectCommand>();
        services.AddTransient<TreeCommand>();
        services.AddTransient<CommitCommand>();

        var register = new TypeRegistrar(services);

        var ui = new CommandApp(register);
        // Commands
        ui.Configure(config =>
        {
            config.SetApplicationName("mygit");

            config.AddCommand<InitCommand>("init")
                .WithDescription("Creates a new empty repository");

            config.AddCommand<HashObjectCommand>("hash-object")
                .WithDescription("Calculates the hash of a file and stores it if needed");

            config.AddCommand<TreeCommand>("tree")
                .WithDescription("Displays the contents of a directory and its subdirectories");

            config.AddCommand<CommitCommand>("commit")
                .WithDescription("Commit changes");


            config.SetExceptionHandler((ex, context) =>
            {
                AnsiConsole.MarkupLine($"[red]error:[/] {Markup.Escape(ex.Message)}");

                #if DEBUG
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything | ExceptionFormats.ShowLinks);
                #endif
                return 1;

            });
        });
        return await ui.RunAsync(args);
    }
}
