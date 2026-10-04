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

        public byte[] AssembleTree(MemoryStream body);

        public MemoryStream AssembleEntryBody(List<TreeObj> treeObjs);

        public List<TreeObj> SortEntriesFilesAlphabetically(List<TreeObj> treeObjs);

        public string ConvertToASCII(byte[] data);


    }
}
