namespace MyGit.Core.Infrastructure.Services.HandleFiles
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using MyGit.Core.Application.Interfaces.HandleFiles;

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

        public async Task<byte[]> AssembleBlob(byte[] fileContentSize, byte[] fileContent)
        {
            
            byte[] header = Encoding.UTF8.GetBytes($"blob {fileContentSize.Length}\0");

            using var memoryStream = new MemoryStream();

            await memoryStream.WriteAsync(header, 0, header.Length);
            await memoryStream.WriteAsync(fileContent, 0, fileContent.Length);

            byte[] fullData = memoryStream.ToArray();
            return fullData;
        }

        public async Task<byte[]> HashBlob(FileStream stream)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                return await sha1.ComputeHashAsync(stream);
            }
        }



        public async Task<byte[]> GetFileContentSize(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            long fileSizeInBytes = fileInfo.Length;
            Debug.WriteLine($"File size in bytes: {fileSizeInBytes}");
            return BitConverter.GetBytes(fileSizeInBytes);
        }
    }
}
