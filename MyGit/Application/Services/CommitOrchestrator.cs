using MyGit.Core.Application.Interfaces.HandleFiles;

namespace MyGit.Core.Application.Services
{
    public class CommitOrchestrator
    {
        private readonly IHandleFiles _handleFiles;
        private readonly ICommit _commit;

        public CommitOrchestrator(IHandleFiles handleFiles, ICommit commit)
        {
            this._commit = commit;
            this._handleFiles = handleFiles;
        }

        public void timeTest()
        {
            this._commit.Assembletz();
        }

        public string OrchestrateAssembly(string parentHex, string treeHex, string message, string author, string email)
        {
            var obj = this._commit.AssembleCommitObj(parentHex, treeHex, message, author, email);
            var commitStream = this._commit.AssembleCommitBody(obj);
            var commit = this._commit.AssembleCommitHeader(commitStream);
            var hash = this._handleFiles.CalculateHash(commit);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
