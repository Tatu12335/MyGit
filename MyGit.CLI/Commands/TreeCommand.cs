using MyGit.Core.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Cli;

namespace MyGit.CLI.Commands
{
    public class TreeCommand : Command<TreeCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<path>")]
            public string path { get; set; } = string.Empty;
            
            [CommandOption("-w|--write")]
            public bool Write { get; set; }
        }

        private readonly FileHandlingOrchestration _fileHandlingOrchestration;

        public TreeCommand(FileHandlingOrchestration fileHandlingOrchestration)
        {
            this._fileHandlingOrchestration = fileHandlingOrchestration;
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken = default)
        {
            this._fileHandlingOrchestration.DisplayTree(settings.path);
            
            if (settings.Write)
            {

            }

            return 0;
        }
    }
}
