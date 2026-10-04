using MyGit.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;
using Spectre.Console;

namespace MyGit.CLI.Commands
{
    public class TreeCommand : AsyncCommand<TreeCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<path>")]
            public string path { get; set; }

            [CommandOption("-w|--write")]
            public bool Write { get; set; }
        }

        private readonly FileHandlingOrchestration _fileHandlingOrchestration;

        public TreeCommand(FileHandlingOrchestration fileHandlingOrchestration)
        {
            this._fileHandlingOrchestration = fileHandlingOrchestration;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            // this._fileHandlingOrchestration.BuildTreeString(settings.path);
            if (settings.Write)
            {
                var ksadksad = this._fileHandlingOrchestration.AssembleTree(settings.path);
                AnsiConsole.MarkupLine($"[green]hash: {Markup.Escape(Convert.ToHexString(ksadksad))}[/]");
            }

            return 0;
        }
    }
}
