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
    using Domain;

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

        public string[] SortEntriesFilesAlphabetically(string[] filenames)
        {
            Array.Sort(filenames, StringComparer.Ordinal);
            return filenames;
        }

        public byte[] AssembleFileEntry(TreeObj treeObj)
        {
            var header  = Encoding.UTF8.GetBytes($"{treeObj.mode} {treeObj.filename}\0");

            using var memoryStream = new MemoryStream();

            memoryStream.Write(header, 0, header.Length);
            memoryStream.Write(treeObj.hash, 0, treeObj.hash.Length);

            var data = memoryStream.ToArray();
            return data;

        }

        public void AssembleTreeEntry(TreeObj treeObjects, MemoryStream memoryStream)
        {
           
        }

        public byte[] AssembleTree(byte[] body)
        {
            byte[] header = Encoding.UTF8.GetBytes($"tree {body.Length}\0");

            using var memoryStream = new MemoryStream();

            memoryStream.Write(header, 0, header.Length);
            memoryStream.Write(body, 0, body.Length);

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
