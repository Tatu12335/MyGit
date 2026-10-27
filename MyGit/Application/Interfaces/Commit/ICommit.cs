namespace MyGit.Core.Application.Interfaces.Commit
{
    public interface ICommit
    {
        public byte[] AssembleCommit(MemoryStream body);

        public MemoryStream AssembleCommitBody(Domain.Commit entry);

        public string Assembletz();

        public Domain.Commit AssembleCommitObj(string parentHex, string treeHex, string message, string author, string email);
    }
}
