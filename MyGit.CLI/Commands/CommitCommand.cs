using MyGit.Core.Application.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace MyGit.CLI.Commands
{
    public class CommitCommand : AsyncCommand<CommitCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandOption("-m|--message <message>")]
            public required string message { get; set; }

            [CommandOption("--author <author>")]
            public required string author { get; set; }

            [CommandOption("--email <email>")]
            public required string email { get; set; }
        }

        private readonly CommitOrchestrator _orchestrator;

        public CommitCommand(CommitOrchestrator orchestrator)
        {
            this._orchestrator = orchestrator;
        }

        // Remember to validate the commit message, commit message should not be empty, unless otherwise stated.
        protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var validatedMessage = this.Validate(context, settings);

            if (!validatedMessage.Successful)
            {
                AnsiConsole.MarkupLine("[red] commit message cannot be empty![/]");
                return 1;
            }

            this._orchestrator.timeTest();
            //this._orchestrator.OrchestrateAssembly()
            return 0;
        }
    }
}
