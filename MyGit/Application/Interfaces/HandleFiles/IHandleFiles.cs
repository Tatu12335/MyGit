using MyGit.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface IHandleFiles
    {
        public Task<byte[]> ReadFile(string filePath);

        public byte[] AssembleBlob(byte[] fileContent);

        public byte[] CalculateHash(byte[] data);

        public Task CompressBlob(byte[] blobData, string outputFilePath);

        public byte[] AssembleTree(byte[] tree);

        public byte[] AssembleFileEntry(TreeObj treeObj);

        public string[] SortEntriesFilesAlphabetically(string[] filenames);


    }
}
