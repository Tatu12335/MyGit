using MyGit.Core.Application.Interfaces.HandleFiles;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.CLI
{
    public class UI
    {
        private readonly IHandleFiles _handleFiles;

        public UI(IHandleFiles handleFiles)
        {
            _handleFiles = handleFiles;
        }

        public void DisplayWelcomeMessage()
        {
            AnsiConsole.MarkupLine("[bold green]Welcome to MyGit![/]");
            AnsiConsole.MarkupLine("[bold yellow]A simple Git implementation in C#.[/]");
            AnsiConsole.MarkupLine("[bold blue]Type 'help' to see available commands.[/]");
        }

        public void DisplayHelp()
        {
            AnsiConsole.MarkupLine("[bold cyan]Available Commands:[/]");
            AnsiConsole.MarkupLine("[bold yellow]init[/] - Initialize a new MyGit repository.");
            AnsiConsole.MarkupLine("[bold yellow]add <file>[/] - Add a file to the staging area.");
            AnsiConsole.MarkupLine("[bold yellow]commit -m \"message\"[/] - Commit changes with a message.");
            AnsiConsole.MarkupLine("[bold yellow]status[/] - Show the status of the repository.");
            AnsiConsole.MarkupLine("[bold yellow]log[/] - Show commit history.");
            AnsiConsole.MarkupLine("[bold yellow]help[/] - Show this help message.");
            AnsiConsole.MarkupLine("[bold yellow]exit[/] - Exit the application.");
        }

        public void DisplayError(string message)
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {message}");
        }

        public void DisplayInfo(string message)
        {
            AnsiConsole.MarkupLine($"[bold blue]Info:[/] {message}");
        }

        public void DisplaySuccess(string message)
        {
            AnsiConsole.MarkupLine($"[bold green]Success:[/] {message}");
        }

        public void DisplayWarning(string message)
        {
            AnsiConsole.MarkupLine($"[bold yellow]Warning:[/] {message}");
        }

        public void DisplayPrompt(string message)
        {
            AnsiConsole.MarkupLine($"[bold cyan]{message}[/]");
        }

        public void DisplayCommandResult(string command, string result)
        {
            AnsiConsole.MarkupLine($"[bold magenta]Command:[/] {command}");
            AnsiConsole.MarkupLine($"[bold magenta]Result:[/] {result}");
        }

        public void DisplayCommandError(string command, string error)
        {
            AnsiConsole.MarkupLine($"[bold magenta]Command:[/] {command}");
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {error}");
        }

        public string GetUserInput()
        {
            return Console.ReadLine();
        }

        public void start()
        {
            this.DisplayWelcomeMessage();
            var input = this.GetUserInput();

            switch (input)
            {
                case "help":
                    this.DisplayHelp();
                    break;
                case "exit":
                    this.DisplayInfo("Exiting MyGit. Goodbye!");
                    Environment.Exit(0);
                    break;
                default:
                    this.DisplayError($"Unknown command: {input}. Type 'help' for a list of commands.");
                    break;
            }
        }
    }
}
