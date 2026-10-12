using MyGit.Core.Domain;

namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface ICommit
    {
        public byte[] AssembleCommitHeader(MemoryStream body);

        public MemoryStream AssembleCommitBody(Domain.Commit entry);

        public string Assembletz();

        public Commit AssembleCommitObj(string parentHex, string treeHex, string message, string author, string email);
    }
}
