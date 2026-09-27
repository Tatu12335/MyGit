using MyGit.Core.Application.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.CLI
{
    internal class HashObjectCommand : AsyncCommand<HashObjectCommand.Settings>
    {
        private readonly FileHandlingOrchestration _fileHandlingOrchestration;
        public HashObjectCommand(FileHandlingOrchestration fileHandlingOrchestration)
        {
            this._fileHandlingOrchestration = fileHandlingOrchestration;
        }

        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<file>")]
            public required string FilePath { get; set; }

            [CommandOption("-w|--write")]
            public bool Write { get; set; }
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            if (!File.Exists(settings.FilePath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] tiedostoa {Markup.Escape(settings.FilePath)} ei löydy");
                return 1;
            }

            var hash = await this._fileHandlingOrchestration.AssembleFileData(settings.FilePath);
            string hashHex = Convert.ToHexString(hash).ToLowerInvariant();
            if(settings.Write)
            {
                var gitDir = Path.Combine(Directory.GetCurrentDirectory(), ".mygit");
                var objectPath = Path.Combine(gitDir, "objects", hashHex);
                if (!File.Exists(objectPath))
                {
                    await File.WriteAllBytesAsync(objectPath, hash);
                    AnsiConsole.MarkupLine($"[green]Wrote object to {Markup.Escape(objectPath)}[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[yellow]Object already exists at {Markup.Escape(objectPath)}[/]");
                }
            }

            AnsiConsole.WriteLine(hashHex);
            return 0;
        }
    }
}
