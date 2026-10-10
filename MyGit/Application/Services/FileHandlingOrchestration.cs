using MyGit.Core.Application.Interfaces.HandleFiles;
using MyGit.Core.Domain;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

            if (directoryPath.Contains(".mygit") 
                || directoryPath.Contains(".mygit/") 
                || directoryPath.Contains("MyGit.CLI")
                || directoryPath.Contains(".git"))
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
        
        public byte[]? AssembleTree(string directoryPath)
        {
            var treeObjects = new List<TreeObj>();
            if (!Directory.Exists(directoryPath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] Directory {Markup.Escape(directoryPath)} not found");
                return null;
            }
         
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                var name = Path.GetFileName(dir); 
                if(name == ".git" || name == ".mygit")
                    continue;

                var Hash = this.AssembleTree(dir);

                if (Hash == null)
                    continue;

                TreeObj treeObj = new TreeObj();

                treeObj.name = name;

                treeObj.mode = "40000";
                treeObj.hash = Hash;

                treeObjects.Add(treeObj);
            }

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                var name = Path.GetFileName(file);
                if (name == ".mygit" || name == ".git")
                    continue;

                var fileData = this.AssembleFileData(file).GetAwaiter().GetResult();
                TreeObj treeObj = new TreeObj();

                treeObj.name = name;
                treeObj.hash = fileData;
                treeObj.mode = "100644";



                treeObjects.Add(treeObj);


            }

            if (treeObjects.Count == 0)
                 return null;
            
            foreach(var entry in treeObjects)
            {
                Console.WriteLine($"{entry.mode} {entry.name} {Convert.ToHexString(entry.hash).ToLowerInvariant()}");
            }

            var hash = this.SortAndAssemble(treeObjects);
            return hash;
        }

        public byte[]? SortAndAssemble(List<TreeObj> objects)
        {
            var sorted = this._handleFiles.SortEntries(objects);
            MemoryStream body = this._handleFiles.AssembleEntryBody(sorted);
            byte[] tree = this._handleFiles.AssembleTree(body);
            byte[] hash = this._handleFiles.CalculateHash(tree);
            

            return hash; 
        }
    }
}
