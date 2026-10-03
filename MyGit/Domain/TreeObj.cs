using System;
using System.Collections.Generic;
using System.Text;

namespace MyGit.Core.Domain
{
    public class TreeObj
    {
        public string mode { get; set; }

        public string filename { get; set; }

        public byte[] hash { get; set; }
    }
}
