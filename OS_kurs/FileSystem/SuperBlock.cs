using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_kurs.FileSystem
{
    internal class SuperBlock
    {
        public const byte TypeSize = 8;
        public const byte SizeInBlocksSize = 4;
        public const byte IListSizeSize = 3;
        public const byte FreeBlockCountSize = 3;
        public const byte FreeINodeCountSize = 3;
        public const byte BlockSizeSize = 3;
        public const byte ListINodeSize = 60;
        public const byte ListBlockSize = 3;

        public const string Type = "GeorgeFS";
        public const Int32 SizeInBlocks = 1024;
        public const Int16 IListSize = 700;
        public const Int16 FreeBlockCount = 999;
        public const Int16 FreeINodeCount = 700;
        public const Int16 BlockSize = 512;
        public const Int16 ListINode = new Int16(); // todo
        public const Int16 ListBlock = 26; // todo address
    }
}
