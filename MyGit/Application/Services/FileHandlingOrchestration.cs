using MyGit.Core.Application.Interfaces.HandleFiles;
using MyGit.Core.Domain;
using Spectre.Console;
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
                AnsiConsole.MarkupLine($"[yellow]warning:[/] Output file path '{outputFilePath}' already exist");
                return;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));
            this._handleFiles.CompressData(blobData, outputFilePath);
        }

        public byte[] CalculateHash(byte[] data)
        {
            return this._handleFiles.CalculateHash(data);
        }

        public async Task<byte[]> AssembleFileData(string filePath)
        {
            byte[] content = await this._handleFiles.ReadFile(filePath);
            byte[] blob = this._handleFiles.AssembleBlob(content);
            return blob;
        }

        public string GiveAndValidateObjectPath(string hashHex, string path)
        {
            if (!Directory.Exists(path))
            {
                AnsiConsole.MarkupLine($"[red]Directory[/]: {Markup.Escape(path)}, doesnt exist. Try init <path>, To run");
                return null;
            }

            string toBeObjPath = this._handleFiles.CreateObjectPath(hashHex, path);
            return toBeObjPath;
        }

        public byte[]? AssembleTree(string directoryPath, string objPath)
        {

            var treeObjects = new List<TreeObj>();
            if (!Directory.Exists(directoryPath))
            {
                AnsiConsole.MarkupLine($"[red]error:[/] Directory {Markup.Escape(directoryPath)} not found");
                return null;
            }

            // Goes through directories unless the name is .mygit or .git, and adds the treeobj to the list.
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                var name = Path.GetFileName(dir);
                if (name == ".git" || name == ".mygit")
                    continue;

                var Hash = this.AssembleTree(dir, objPath);

                if (Hash == null)
                    continue;

                TreeObj treeObj = new TreeObj();

                treeObj.name = name;

                treeObj.mode = "40000";
                treeObj.hash = Hash;

                treeObjects.Add(treeObj);

            }

            // Goes through files and adds them to the treeobjects list.
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

            var hash = this.SortAndAssemble(treeObjects, objPath);

            return hash;
        }

        public byte[]? SortAndAssemble(List<TreeObj> objects, string path)
        {
            List<TreeObj> sorted = this._handleFiles.SortEntries(objects);
            MemoryStream body = this._handleFiles.AssembleEntryBody(sorted);
            byte[] tree = this._handleFiles.AssembleTree(body);
            byte[] hash = this._handleFiles.CalculateHash(tree);
            var hashHex = Convert.ToHexString(hash);
            var outputFile = this.GiveAndValidateObjectPath(hashHex, path);
            Console.WriteLine(outputFile);
            this.WriteFileToMemory(tree, outputFile);


            return hash;
        }
    }
}
