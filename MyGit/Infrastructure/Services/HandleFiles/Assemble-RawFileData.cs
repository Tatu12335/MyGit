using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Infrastructure.Services.HandleFiles
{
    public class Assemble_RawFileData
    {
        // used byte array to handle raw file data, Because UTF-8 chars might differ in size, so we need to handle the raw data as bytes.
        public string AssembleData(int fileContentSize, byte[] rawFileData) 
        {
            rawFileData = rawFileData ?? throw new ArgumentNullException(nameof(rawFileData));



            foreach(var character in rawFileData)
            {
                if (character == '\0')
                    return Encoding.UTF8.GetString(rawFileData);
                 
            }
        }
    }
}
