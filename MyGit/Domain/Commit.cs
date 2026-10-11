using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Domain
{
    public class Commit
    {
        public string treeHex { get; set; } // 40 character hex so, no byte[]

        public string parent { get; set; } // Just a note to me, this is just last commits hash as 40 char hex. Also this should be null if there is no "later commit"
        
        public string author { get; set; }

        public long unixSeconds { get; set; }
 
    }
}
