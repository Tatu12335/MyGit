using MyGit.Core.Application.Interfaces.HandleFiles;
using Spectre.Console;

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

        public void timeTest()
        {
            this._commit.Assembletz();
        }

        public string GetAndAssembleTreeHash(string path, string outputfilePath)
        {
            var tree = this._fileHandling.AssembleTree(path, outputfilePath);
            return Convert.ToHexString(tree).ToLowerInvariant();
        }

        public string OrchestrateAssembly(string outputfile,string path, string message, string author, string email)
        {
            string parentHex = string.Empty;
            var treeHex = this.GetAndAssembleTreeHash(path, outputfile);
            if (File.Exists(".mygit\\refs\\heads\\main"))
            {
                parentHex = File.ReadAllText(".mygit\\refs\\heads\\main").Replace("\n", ""); 
            }

            var obj = this._commit.AssembleCommitObj(parentHex, treeHex, message, author, email);
            var commitStream = this._commit.AssembleCommitBody(obj);
            var commit = this._commit.AssembleCommitHeader(commitStream);
            var hash = this._handleFiles.CalculateHash(commit);
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
