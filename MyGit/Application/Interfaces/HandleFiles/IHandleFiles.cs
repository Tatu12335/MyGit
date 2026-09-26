using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface IHandleFiles
    {
        public byte[] AssembleData(byte[] fileContentSize, byte[] rawFileData);
        public Task<byte[]> HashBlob(FileStream stream);
    }
}
