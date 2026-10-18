using MyGit.Core.Application.Interfaces.Commit;
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
            Console.WriteLine(t.ToString());
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
            var sb = new StringBuilder();
            sb.Append($"tree {entry.treeHex}\n");
            if (!string.IsNullOrEmpty(entry.parent))
                sb.Append($"parent {entry.parent}\n");
            sb.Append($"author {entry.author} <{entry.email}> {entry.seconds} {entry.tz}\n");
            sb.Append($"committer {entry.author} <{entry.email}> {entry.seconds} {entry.tz}\n");
            sb.Append($"\n{entry.message}\n");

            return new MemoryStream(Encoding.UTF8.GetBytes(sb.ToString()));

        }
    }
}
