namespace MyGit.Core.Infrastructure.Services.HandleFiles
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO.Compression;
    using System.Reflection.Metadata;
    using System.Security.Cryptography;
    using System.Text;
    using Microsoft.VisualBasic;
    using MyGit.Core.Application.Interfaces.HandleFiles;
    using Spectre.Console;

    public class Assemble_RawFileData : IHandleFiles
    {
        // Note to me : remember to give the size of the file content in bytes,
        // it shouldnt be the amount of characters in the file content,
        // because some characters can be more than 1 byte in size.
        // Also remember to use the correct encoding when converting the file content to bytes.
        public async Task<byte[]> ReadFile(string filePath)
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        public byte[] AssembleTree(byte[] tree)
        {
            byte[] header = Encoding.UTF8.GetBytes($"tree {tree.Length}\0");

            using var memoryStream = new MemoryStream();

            memoryStream.Write(header, 0, header.Length);
            memoryStream.Write(tree, 0, tree.Length);

            var data = memoryStream.ToArray();
            return data;
        }

        public byte[] AssembleBlob(byte[] fileContent)
        {
            byte[] header = Encoding.UTF8.GetBytes($"blob {fileContent.Length}\0");

            using var memoryStream = new MemoryStream();

            memoryStream.Write(header, 0, header.Length);
            memoryStream.Write(fileContent, 0, fileContent.Length);

            var data = memoryStream.ToArray();

            return data;
        }

        public string ListFilesAndDirectories(string directoryPath)
        {
            if (directoryPath.Contains(".mygit") || directoryPath.Contains(".mygit/") || directoryPath.Contains("MyGit.CLI"))
            {
                AnsiConsole.MarkupLine($"[yellow]warning:[/]  Skipping directory {Markup.Escape(directoryPath)}");
                directoryPath = string.Empty;
                return directoryPath;
            }

            if (directoryPath == string.Empty)
            {
                AnsiConsole.MarkupLine($"[red]error:[/]  Directory path is null or empty");
                return string.Empty;
            }

            var sb = new StringBuilder();
            foreach (var dir in Directory.GetDirectories(directoryPath))
            {
                sb.AppendLine($"dir: {dir}");
                sb.AppendLine(ListFilesAndDirectories(dir));
            }

            foreach (var file in Directory.GetFiles(directoryPath))
            {
                sb.AppendLine($"file: {file}");
            }

            return sb.ToString();
        }

        public byte[] CalculateHash(byte[] data)
        {
            using var sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(data);
            return hash.ToArray();
        }

        public async Task CompressBlob(byte[] blobData, string outputFilePath)
        {
            using var outputFileStream = new FileStream(outputFilePath, FileMode.Create);
            using var compressionStream = new ZLibStream(outputFileStream, CompressionLevel.Optimal);
            await compressionStream.WriteAsync(blobData, 0, blobData.Length);
        }
    }
}
