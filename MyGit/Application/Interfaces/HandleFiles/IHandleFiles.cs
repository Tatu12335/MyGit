using MyGit.Core.Domain;

namespace MyGit.Core.Application.Interfaces.HandleFiles
{
    public interface IHandleFiles
    {
        public Task<byte[]> ReadFile(string filePath);

        public byte[] AssembleBlob(byte[] fileContent);

        public byte[] CalculateHash(byte[] data);

        public Task CompressData(byte[] data, string outputFilePath);

        public byte[] AssembleTree(MemoryStream body);

        public MemoryStream AssembleEntryBody(List<TreeObj> treeObjs);

        public List<TreeObj> SortEntries(List<TreeObj> treeObjs);

        public string ConvertToASCII(byte[] data);

        public string CreateObjectPath(string hashHex, string path);


    }
}
