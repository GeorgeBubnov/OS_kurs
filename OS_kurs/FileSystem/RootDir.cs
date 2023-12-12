using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_kurs.FileSystem
{
    internal class RootDir
    {
        public Dir dir;
        public RootDir()
        {
            dir = new Dir();
            dir.Name = "root";
            dir.Expansion = "";
            dir.INodeNum = 0;
        }
        public struct Dir
        {
            public string Name;
            public string Expansion;
            public Int32 INodeNum;
        }
    }
}
