using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface ICommit
    {
        public byte[] AssembleCommitHeader(MemoryStream body);
        public MemoryStream AssembleCommitBody();
    }
}
