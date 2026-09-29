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
            byte[] blob = this._handleFiles.AssembleBlob(content);
            byte[] hash = this._handleFiles.CalculateHash(blob);
            return hash;
        }
    }
}
