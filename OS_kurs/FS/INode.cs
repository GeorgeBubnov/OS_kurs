using System;

namespace OS_kurs.FS
{
    public class INode
    {
        public static byte AccessSize = 8;
        public static byte UserIDSize = 1;
        public static byte GroupIDSize = 1;
        public static byte SizeInBytesSize = 2;
        public static byte SizeInBlocksSize = 2;
        public static byte CreationTimeSize = 8;
        public static byte ModificationTimeSize = 8;
        public static byte BlocksAddressesSize = 20;

        public string Access;
        public byte UserID;
        public byte GroupID;
        public UInt16 SizeInBytes;
        public UInt16 SizeInBlocks;
        public string CreationTime;
        public string ModificationTime;
        public UInt16[] BlocksAddresses = new UInt16[10];

        public INode()
        {
            Access = "FFrwx---";
            UserID = 0;
            GroupID = 0;
            SizeInBytes = 0;
            SizeInBlocks = 0;
            CreationTime = "01011990";
            ModificationTime = "01012023";
            BlocksAddresses = new UInt16[10];
        }

        public INode(string access, byte userID, byte groupID, UInt16 sizeInBytes, UInt16 sizeInBlocks, 
            string creationTime, string modificationTime, UInt16[] blocksAddresses)
        {
            Access = access;
            UserID = userID;
            GroupID = groupID;
            SizeInBytes = sizeInBytes;
            SizeInBlocks = sizeInBlocks;
            CreationTime = creationTime;
            ModificationTime = modificationTime;
            BlocksAddresses = blocksAddresses;
        }
    }
}
