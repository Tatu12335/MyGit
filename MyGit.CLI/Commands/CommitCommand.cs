using MyGit.Core.Application.Services;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.CLI.Commands
{
    public class CommitCommand : AsyncCommand<CommitCommand.Settings>
    {
        public class Settings : CommandSettings
        { 
        
        }
        private readonly FileHandlingOrchestration _fileHandlingOrchestration;

        public CommitCommand(FileHandlingOrchestration fileHandlingOrchestration)
        {
            this._fileHandlingOrchestration = fileHandlingOrchestration;
        }
        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            return 0;
        }
    }
}
