using MyGit.Core.Application.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Reflection.Metadata;

namespace MyGit.CLI.Commands
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
                AnsiConsole.MarkupLine($"[red]error:[/] File {Markup.Escape(settings.FilePath)} not found");
                return 1;
            }

            var blob = await this._fileHandlingOrchestration.AssembleFileData(settings.FilePath);
            var hash = this._fileHandlingOrchestration.CalculateHash(blob);
            string hashHex = Convert.ToHexString(hash).ToLowerInvariant();
            string folder = hashHex.Substring(0, 2);
            string fileName = hashHex.Substring(2);

            var gitDir = Path.Combine(Directory.GetCurrentDirectory(), ".mygit");
            var objectPath = Path.Combine(gitDir, "objects", folder, fileName);
            if (settings.Write)
            {

                if (!File.Exists(objectPath))
                {
                    this._fileHandlingOrchestration.WriteFileToMemory(blob, objectPath);
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
