using MyGit.Core.Application.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MyGit.CLI.Commands
{
    public class TreeCommand : AsyncCommand<TreeCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<path>")]
            public string path { get; set; }

        }

        private readonly FileHandlingOrchestration _fileHandlingOrchestration;

        public TreeCommand(FileHandlingOrchestration fileHandlingOrchestration)
        {
            this._fileHandlingOrchestration = fileHandlingOrchestration;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {


            var hash = this._fileHandlingOrchestration.AssembleTree(settings.path);
            if (hash != null)
            {
                var hex = Convert.ToHexString(hash).ToLowerInvariant();
                AnsiConsole.MarkupLine($"[green]hash: {Markup.Escape(hex)}[/]");
                return 0;
            }


            AnsiConsole.MarkupLine($"[green]nothing to commit[/]");
            return 0;
        }
    }
}
