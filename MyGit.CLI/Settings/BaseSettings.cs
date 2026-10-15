using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.CLI.Settings
{
    public class BaseSettings : CommandSettings
    {
        [CommandOption("-r|--repo <path>")]
        public string repo { get; set; }
    }
}
