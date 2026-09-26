using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyGit.CLI;
using MyGit.Core.Application.Interfaces.HandleFiles;
using MyGit.Core.Infrastructure.Services.HandleFiles;
using Spectre.Console.Cli;

class Program
{
    public static async Task Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddTransient<IHandleFiles, Assemble_RawFileData>();
        builder.Services.AddTransient<UI>();

        var app = builder.Build();
        var ui = app.Services.GetRequiredService<UI>();
        ui.start();
    }
}
