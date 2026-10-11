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
            var stream = new MemoryStream();
            var header = Encoding.UTF8.GetBytes($"commit {body.Length}\0");

            stream.Write(header, 0, header.Length);
            body.Position = 0;
            body.WriteTo(stream);

            var fullData = stream.ToArray();
            return fullData;
        }
        public MemoryStream AssembleCommitBody()
        {
            return new MemoryStream();
        }
    }
}
