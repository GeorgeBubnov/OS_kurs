using System;

namespace OS_kurs.FileSystem
{
    internal class INode
    {
        public string Access;
        public Int32 UserID;
        public Int32 GroupID;
        public Int32 SizeInBytes;
        public Int32 SizeInBlocks;
        public string CreationTime;
        public string ModificationTime;
        public string BlocksAddresses;

        public INode()
        {
            Access = "FFrwx---";
            UserID = 0;
            GroupID = 0;
            SizeInBytes = 0;
            SizeInBlocks = 0;
            CreationTime = "01011990";
            ModificationTime = "01012023";
            BlocksAddresses = null;
        }

        public INode(string access, int userID, int groupID, int sizeInBytes, int sizeInBlocks, string creationTime, string modificationTime, string blocksAddresses)
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
