using MyGit.Core.Application.Interfaces.HandleFiles;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Infrastructure.Services.NewFolder
{
    public class Commit : ICommit
    {
        public byte[] AssembleCommitHeader(MemoryStream body)
        {
            var header = Encoding.UTF8.GetBytes($"commit {body.Length}\0");
        }
        public MemoryStream AssembleCommitBody()
        {

        }
    }
}
