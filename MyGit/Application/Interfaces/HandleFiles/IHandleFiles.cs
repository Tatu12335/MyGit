using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface IHandleFiles
    {
        public Task<byte[]> ReadFile(string filePath);
        public Task<byte[]> AssembleBlob(byte[] fileContentSize, byte[] fileContent);
        public Task<byte[]> HashBlob(FileStream stream);
        public Task<byte[]> GetFileContentSize(string filePath);
    }
}
