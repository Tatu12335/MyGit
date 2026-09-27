using MyGit.Core.Application.Interfaces.HandleFiles;
using System;
using System.Collections.Generic;
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

        public async Task<byte[]> AssembleFileData(string filePath)
        {
            byte[] content = await this._handleFiles.ReadFile(filePath);
            byte[] fileContentSize = await this._handleFiles.GetFileContentSize(filePath);
            return await this._handleFiles.AssembleBlob(fileContentSize, content);
        }
    }
}
