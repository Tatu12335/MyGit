namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface ICommit
    {
        public byte[] AssembleCommitHeader(MemoryStream body);

        public MemoryStream AssembleCommitBody();
        public string Assembletz();

    }
}
