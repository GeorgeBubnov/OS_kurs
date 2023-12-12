using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_kurs.FileSystem
{
    internal class IList
    {
        public INode[] nodes;

        public IList()
        {
            nodes = new INode[SuperBlock.IListSize];
        }
    }
}
