using MyGit.Core.Application.Interfaces.Commit;
using MyGit.Core.Application.Interfaces.HandleFiles;

namespace MyGit.Core.Application.Services
{
    public class CommitOrchestrator
    {
        private readonly IHandleFiles _handleFiles;
        private readonly ICommit _commit;
        private readonly FileHandlingOrchestration _fileHandling;

        public CommitOrchestrator(IHandleFiles handleFiles, ICommit commit, FileHandlingOrchestration fileHandling)
        {
            this._commit = commit;
            this._handleFiles = handleFiles;
            this._fileHandling = fileHandling;
        }

        public string GetAndAssembleTreeHash(string path, string toBeObjPath)
        {
            var tree = this._fileHandling.AssembleTree(path, toBeObjPath);
            return Convert.ToHexString(tree).ToLowerInvariant();
        }

        public string OrchestrateAssembly(string toBeObjPath, string path, string message, string author, string email)
        {
            string parentHex = string.Empty;
            var treeHex = this.GetAndAssembleTreeHash(path, toBeObjPath);
            if (File.Exists(Path.Combine(path, ".mygit\\refs\\heads\\main")))
            {
                parentHex = File.ReadAllText(Path.Combine(path, ".mygit\\refs\\heads\\main")).Replace("\n", "");
            }

            var obj = this._commit.AssembleCommitObj(parentHex, treeHex, message, author, email);
            var commitStream = this._commit.AssembleCommitBody(obj);
            var commit = this._commit.AssembleCommit(commitStream);
            var hash = this._handleFiles.CalculateHash(commit);
            var hashHex = Convert.ToHexString(hash).ToLowerInvariant();
            var objPath = this._fileHandling.GiveAndValidateObjectPath(hashHex, path);
            this._fileHandling.WriteFileToMemory(hash, objPath);
            this._commit.Assembletz();
            return hashHex;
        }
    }
}
