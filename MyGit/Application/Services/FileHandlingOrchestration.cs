using MyGit.Core.Application.Interfaces.HandleFiles;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Application.Services
{
    public class FileHandlingOrchestration
    {
        private readonly IHandleFiles _handleFiles;

        public FileHandlingOrchestration(IHandleFiles handleFiles)
        {
            this._handleFiles = handleFiles;
        }

        public void WriteFileToMemory(byte[] blobData, string outputFilePath)
        {
            if (string.IsNullOrWhiteSpace(outputFilePath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] Output file path is null or empty");
                return;
            }

            if (File.Exists(outputFilePath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] File {Markup.Escape(outputFilePath)} already exists");
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
            this._handleFiles.CompressBlob(blobData, outputFilePath);
        }

        public async Task<byte[]> AssembleFileData(string filePath)
        {
            byte[] content = await this._handleFiles.ReadFile(filePath);
            byte[] blob = this._handleFiles.AssembleBlob(content);
            byte[] hash = this._handleFiles.CalculateHash(blob);

            return hash;
        }

        public string DisplayTree(string path)
        {
            if (!Directory.Exists(path))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] Directory {Markup.Escape(path)} not found");
                return null;
            }

            string tree = this._handleFiles.ListFilesAndDirectories(path);
            AnsiConsole.MarkupLine($"[green]Directory tree for {Markup.Escape(path)}:[/]");
            AnsiConsole.MarkupLine(tree);
            return tree;
        }
    }
}
