namespace MyGit.Core.Infrastructure.Services.HandleFiles
{
    using Domain;
    using Microsoft.VisualBasic;
    using MyGit.Core.Application.Interfaces.HandleFiles;
    using Spectre.Console;
    using Spectre.Console.Cli;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO.Compression;
    using System.Reflection.Metadata;
    using System.Security.Cryptography;
    using System.Text;

    public class Assemble_RawFileData : IHandleFiles
    {
        // Note to me : remember to give the size of the file content in bytes,
        // it shouldnt be the amount of characters in the file content,
        // because some characters can be more than 1 byte in size.
        // Also remember to use the correct encoding when converting the file content to bytes.

        // I realize that the ReadFile is not really necessary, but i will keep it for now, because it is already in use.
        public async Task<byte[]> ReadFile(string filePath)
        {
            return await File.ReadAllBytesAsync(filePath);
        }

        // this seems to be the problem as of now, fix it later!
        public List<TreeObj> SortEntriesFilesAlphabetically(List<TreeObj> treeObjs)
        {
            var ordered = treeObjs.OrderBy(obj => obj.mode == "40000" ? obj.name + "/" : obj.name)
                .ToList();

            return treeObjs;
        }

        public MemoryStream AssembleEntryBody(List<TreeObj> treeObjs)
        {
            var memoryStream = new MemoryStream();
            foreach (var treeObj in treeObjs)
            {
                var header = Encoding.UTF8.GetBytes($"{treeObj.mode} {treeObj.name}\0");

                memoryStream.Write(header, 0, header.Length);
                memoryStream.Write(treeObj.hash, 0, treeObj.hash.Length);
            }

            return memoryStream;

        }

        public string ConvertToASCII(byte[] data)
        {
            var sb = new StringBuilder();

            foreach (var b in data)
            {
                if (b >= 32 && b <= 126)
                {
                    sb.Append(Convert.ToChar(b));
                }

                sb.Append(b.ToString("X2")); // Convert to hexadecimal string representation
            }

            return sb.ToString();
        }

        public byte[] AssembleTree(MemoryStream body)
        {
            using var memoryStream = new MemoryStream();
            byte[] header = Encoding.UTF8.GetBytes($"tree {body.Length}\0");

            memoryStream.Write(header, 0, header.Length);
            body.Position = 0; // Reset the position of the body stream to the beginning
            body.WriteTo(memoryStream);

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
