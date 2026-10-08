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
                TreeObj treeObj = new TreeObj();

                treeObj.name = Path.GetFileName(dir);
                treeObj.mode = "040000";
                treeObj.hash = this.AssembleTree(dir);

                treeObjects.Add(treeObj);
            }

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                TreeObj treeObj = new TreeObj();

                treeObj.name = Path.GetFileName(file);
                var fileData = this.AssembleFileData(file).GetAwaiter().GetResult();
                treeObj.hash = fileData;
                treeObj.mode = "100644";

                treeObjects.Add(treeObj);
            }

            List<TreeObj> sortedTreeObjects = this._handleFiles.SortEntries(treeObjects);
            MemoryStream entryBody = this._handleFiles.AssembleEntryBody(sortedTreeObjects);

            byte[] tree = this._handleFiles.AssembleTree(entryBody);
            byte[] hash = this._handleFiles.CalculateHash(tree);
            string ascii = this._handleFiles.ConvertToASCII(tree);

            AnsiConsole.MarkupLine($"[blue]info:[/]  string tree for directory {Markup.Escape(directoryPath)} : {Markup.Escape(ascii)}");
            //AnsiConsole.MarkupLine($"[blue]info:[/]  Assembled tree for directory {Markup.Escape(directoryPath)} : {Markup.Escape(BitConverter.ToString(hash).Replace("-", ""))}");
            return hash; // Placeholder return value
        }
    }
}
