using Spectre.Console.Cli;

namespace MyGit.CLI.Settings
{
    public class BaseSettings : CommandSettings
    {
        [CommandOption("-r|--repo <path>")]
        public string repo { get; set; }
    }
}
