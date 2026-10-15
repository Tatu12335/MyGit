using MyGit.Core.Application.Interfaces.HandleFiles;
using Spectre.Console;
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

        public string Assembletz()
        {
            var now = DateTimeOffset.Now;
            long seconds = now.ToUnixTimeSeconds();

            string timezone = now.ToString("zzz").Replace(":", "");

            var t = DateTimeOffset.FromUnixTimeSeconds(1700000000).ToOffset(TimeSpan.FromHours(2));

            return timezone;

        }

        public MyGit.Core.Domain.Commit AssembleCommitObj(string parentHex, string treeHex, string message, string author, string email)
        {
            MyGit.Core.Domain.Commit commit = new Domain.Commit
            {
                treeHex = treeHex,
                parent = parentHex,
                author = author,
                email = email,
                message = message,
                tz = this.Assembletz(),
                seconds = DateTimeOffset
                    .Now
                    .ToUnixTimeSeconds()

            };

            return commit;
        }

        public MemoryStream AssembleCommitBody(Domain.Commit entry)
        {
            var memStream = new MemoryStream();

            memStream.Position = 0;
            byte[] bodybytes;
            byte[] bodybytes2;
            // The parent commit hash should probably be string.empty at this point and not null.


            if (string.IsNullOrEmpty(entry.parent))
            {
                bodybytes = Encoding.UTF8.GetBytes
                     ($"tree {entry.treeHex}\nparent {entry.parent}\nauthor {entry.author} <{entry.email}>{entry.seconds} {entry.tz}\n");
                bodybytes2 = Encoding.UTF8.GetBytes($"comitter{entry.author}<{entry.email}>{entry.seconds}\n{entry.message}\n");
            }

            bodybytes = Encoding.UTF8.GetBytes
                    ($"tree {entry.treeHex}\nauthor {entry.author} <{entry.email}>{entry.seconds} {entry.tz}\n");
            bodybytes2 = Encoding.UTF8.GetBytes($"comitter{entry.author}<{entry.email}>{entry.seconds}\n{entry.message}\n");

            memStream.Write(bodybytes);
            memStream.Write(bodybytes2);


            return memStream;
        }
    }
}
