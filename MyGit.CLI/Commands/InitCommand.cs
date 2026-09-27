using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.CLI.Commands
{
    public class InitCommand : Command<InitCommand.Settings>
    {
        public class Settings : CommandSettings
        {
            [CommandArgument(0, "<path>")]
            public string path { get; set; } = string.Empty;
        }

        protected override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
        {
            var path = settings.path ?? Directory.GetCurrentDirectory();
            var gitDir = Path.Combine(path, ".mygit");

            if (Directory.Exists(gitDir))
            {
                // Used markup to escape the gitDir path to prevent any special characters from being interpreted as markup.
                AnsiConsole.MarkupLine($"[yellow]Reinitialized existing repository in {Markup.Escape(gitDir)}[/]");
                return 0;
            }

            Directory.CreateDirectory(gitDir);
            Directory.CreateDirectory(Path.Combine(gitDir, "objects"));
            Directory.CreateDirectory(Path.Combine(gitDir, "refs"));

            AnsiConsole.MarkupLine($"[green]Initialized empty MyGit repository in {Markup.Escape(gitDir)}[/]");
            return 0;
        }
    }
}
