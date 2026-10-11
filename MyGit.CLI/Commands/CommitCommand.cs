using MyGit.Core.Application.Services;
using Spectre.Console;
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
            [CommandOption("-m|--message <MESSAGE>")]
            public required string message { get; set; }

            [CommandOption("--author <AUTHOR>")]
            public required string author { get; set; }

            [CommandOption("--email <EMAIL>")]
            public required string email { get; set; }
        }

        private readonly FileHandlingOrchestration _fileHandlingOrchestration;

        public CommitCommand(FileHandlingOrchestration fileHandlingOrchestration)
        {
            this._fileHandlingOrchestration = fileHandlingOrchestration;
        }

        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var validatedMessage = Validate(context, settings);

            if (!validatedMessage.Successful)
            {
                AnsiConsole.MarkupLine("[red] commit message cannot be empty![/]");
                return 1;
            }

            return 0;
        }
    }
}
