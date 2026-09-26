using MyGit.Core.Application.Interfaces.HandleFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Infrastructure.Services.HandleFiles
{
    public class Assemble_RawFileData : IHandleFiles
    {
        // Note to me : remember to give the size of the file content in bytes,
        // it shouldnt be the amount of characters in the file content,
        // because some characters can be more than 1 byte in size.
        public byte[] AssembleData(byte[] fileContentSize, byte[] rawFileData) 
        {
            rawFileData = rawFileData ?? throw new ArgumentNullException(nameof(rawFileData));

            string fileContentAsString = Encoding.UTF8.GetString(rawFileData, 0, fileContentSize.Length);
            char[] fileAsCharArray = fileContentAsString.ToCharArray(0, fileContentSize.Length);
            fileContentAsString = "blob " + fileContentSize + "\0" + fileContentAsString;

            return Encoding.UTF8.GetBytes(fileContentAsString);
        }

        public async Task<byte[]> HashBlob(FileStream stream)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                return await sha1.ComputeHashAsync(stream);
            }
        }

        public async Task<FileStream> OpenFileStream(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"The file '{filePath}' does not exist.");
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }
    }
}
