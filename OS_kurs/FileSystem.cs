using OS_kurs.FS;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace OS_kurs
{
    class FileSystem
    {
        string path = "GeorgeFS.data";
        public INode[] IList = new INode[SuperBlock.IListSize];
        public UserNode[] UserList = new UserNode[10];

        string[] curUser;

        public FileSystem()
        {
            CreateDrive();
        }


        private void CreateDrive()
        {
            if (true/*!File.Exists(path)*/) // Create FS
            {
                WriteSuperBlock();

                //IList
                for (int i = 60; i < 5060; i += 50)
                    WriteINodes(i);

                //UserList
                for (int i = 5060; i < 5480; i += 42)
                    WriteUserInfos(i);

                //DataList
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    fs.Seek(56831, SeekOrigin.Begin);
                    fs.Write(BitConverter.GetBytes(0), 0, 1);
                }
            }


        }
        public void WriteSuperBlock()
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.Write(Encoding.UTF8.GetBytes(SuperBlock.Type), 0, SuperBlock.TypeSize);
                fs.Write(BitConverter.GetBytes(SuperBlock.SizeInBlocks), 0, SuperBlock.SizeInBlocksSize);
                fs.Write(BitConverter.GetBytes(SuperBlock.IListSize), 0, SuperBlock.IListSizeSize);
                fs.Write(BitConverter.GetBytes(SuperBlock.FreeBlockCount), 0, SuperBlock.FreeBlockCountSize);
                fs.Write(BitConverter.GetBytes(SuperBlock.FreeINodeCount), 0, SuperBlock.FreeINodeCountSize);
                fs.Write(BitConverter.GetBytes(SuperBlock.BlockSize), 0, SuperBlock.BlockSizeSize);
                for (int i = 0; i < 20; i++) // 20 = SuperBlock.ListINodeSize / sizeof(UInt16)
                    fs.Write(BitConverter.GetBytes(SuperBlock.ListINode[i]), 0, 2); // 2 = sizeof(UInt16)
                fs.Write(BitConverter.GetBytes(SuperBlock.ListBlock), 0, SuperBlock.ListBlockSize);
            }
        }

        public void WriteINodes(int address)
        {
            int index = (address - 60) / 50;
            
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                IList[index] = new INode();
                fs.Seek(address, SeekOrigin.Begin);
                fs.Write(Encoding.UTF8.GetBytes(IList[index].Access), 0, INode.AccessSize);
                fs.Write(BitConverter.GetBytes(IList[index].UserID), 0, INode.UserIDSize);
                fs.Write(BitConverter.GetBytes(IList[index].GroupID), 0, INode.GroupIDSize);
                fs.Write(BitConverter.GetBytes(IList[index].SizeInBytes), 0, INode.SizeInBytesSize);
                fs.Write(BitConverter.GetBytes(IList[index].SizeInBlocks), 0, INode.SizeInBlocksSize);
                fs.Write(Encoding.UTF8.GetBytes(IList[index].CreationTime), 0, INode.CreationTimeSize);
                fs.Write(Encoding.UTF8.GetBytes(IList[index].ModificationTime), 0, INode.ModificationTimeSize);
                for (int i = 0; i < 10; i++)
                    fs.Write(BitConverter.GetBytes(IList[index].BlocksAddresses[i]), 0, 2); // 2 = sizeof(UInt16)
            }
        }

        public void WriteUserInfos(int address)
        {
            int index = (address - 5060) / 42;
            
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                UserList[index] = new UserNode();
                fs.Seek(address, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(UserList[index].ID), 0, UserNode.IDSize);
                fs.Write(BitConverter.GetBytes(UserList[index].GroupID), 0, UserNode.GroupIDSize);
                fs.Write(Encoding.UTF8.GetBytes(UserList[index].Login), 0, UserNode.LoginSize);
                fs.Write(Encoding.UTF8.GetBytes(UserList[index].Password), 0, UserNode.PasswordSize);
            }
        }
    }
}
