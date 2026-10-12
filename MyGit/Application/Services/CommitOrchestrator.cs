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

        public void OrchestrateAssembly()
        {

        }
    }
}
