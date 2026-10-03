using MyGit.Core.Application.Interfaces.HandleFiles;
using MyGit.Core.Domain;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

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

        public void BuildTreeString(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] Directory {Markup.Escape(directoryPath)} not found");
                return;
            }

            if (directoryPath.Contains(".mygit") || directoryPath.Contains(".mygit/") || directoryPath.Contains("MyGit.CLI"))
            {
                return;
            }

            var sb = new StringBuilder();
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                sb.AppendLine($"dir: {dir}");
                this.BuildTreeString(dir);
            }

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                sb.AppendLine($"file: {file}");
            }

            AnsiConsole.MarkupLine(sb.ToString());
        }

        public byte[] AssembleTree(string directoryPath)
        {
            var treeObjects = new List<TreeObj>();
            if (!Directory.Exists(directoryPath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] Directory {Markup.Escape(directoryPath)} not found");
                return new byte[0];
            }

            if (directoryPath.Contains(".mygit") || directoryPath.Contains(".mygit/") || directoryPath.Contains("MyGit.CLI"))
            {
                AnsiConsole.MarkupLine($"[yellow]warning:[/]  Skipping directory {Markup.Escape(directoryPath)}");
                return new byte[0];
            }

            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
               // AnsiConsole.MarkupLine($"[blue]info:[/]  Processing directory {Markup.Escape(dir)}");
                this.AssembleTree(dir);
            }

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                TreeObj treeObj = new TreeObj();

                treeObj.filename = Path.GetFileName(file);

                //AnsiConsole.MarkupLine($"[blue]info:[/]  Processing file {Markup.Escape(file)}");

                treeObj.hash =  this.AssembleFileData(file).GetAwaiter().GetResult();
                treeObj.mode = "100644";

                treeObjects.Add(treeObj);

            }

            var fileEntries = new List<byte[]>();
            foreach (var treeObjs in treeObjects)
            {

                var fileEntry = this._handleFiles.AssembleFileEntry(treeObjs);
                fileEntries.Add(fileEntry);
                AnsiConsole.MarkupLine($"[green]success:[/]  Assembled file entry for {Markup.Escape(treeObjs.filename)}");

            }

            return new byte[0]; // Placeholder return value
        }
    }
}
