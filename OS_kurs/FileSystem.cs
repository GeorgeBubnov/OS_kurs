using OS_kurs.FS;
using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
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
        public bool IsLogin(string lg, string pw) {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                for(int i = 5062; i < 5480; i += 42)
                {
                    byte[] buffer = new byte[20];
                    fs.Seek(i, SeekOrigin.Begin);
                    fs.Read(buffer, 0, 20);

                    string login = GetValidString(buffer);

                    if (login == lg)
                    {
                        fs.Seek(i + 20, SeekOrigin.Begin);
                        fs.Read(buffer, 0, 20);

                        string password = GetValidString(buffer);

                        if(password == pw)
                            return true;
                    }
                }
            }

            return false;
        }
        public string GetValidString(byte[] buffer) { return Encoding.UTF8.GetString(buffer).Split('\0')[0]; }
        public void CreateDrive()
        {
            if (true/*!File.Exists(path)*/) // Create FS
            {
                WriteSuperBlock();

                WriteRootINode();

                WriteRootUserInfo();

                CreateRootDir();
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
                {
                    SuperBlock.ListINode[i] = (UInt16)(110 + i * 50); //110 = 60 + 50
                    fs.Write(BitConverter.GetBytes(SuperBlock.ListINode[i]), 0, 2); // 2 = sizeof(UInt16)
                }
                fs.Write(BitConverter.GetBytes(SuperBlock.ListBlock), 0, SuperBlock.ListBlockSize);
            }
        }
        public void WriteRootINode()
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                IList[0] = new INode();
                fs.Seek(60, SeekOrigin.Begin);
                fs.Write(Encoding.UTF8.GetBytes(IList[0].Access), 0, INode.AccessSize);
                fs.Write(BitConverter.GetBytes(IList[0].UserID), 0, INode.UserIDSize);
                fs.Write(BitConverter.GetBytes(IList[0].GroupID), 0, INode.GroupIDSize);
                fs.Write(BitConverter.GetBytes(IList[0].SizeInBytes), 0, INode.SizeInBytesSize);
                fs.Write(BitConverter.GetBytes(IList[0].SizeInBlocks), 0, INode.SizeInBlocksSize);
                fs.Write(Encoding.UTF8.GetBytes(IList[0].CreationTime), 0, INode.CreationTimeSize);
                fs.Write(Encoding.UTF8.GetBytes(IList[0].ModificationTime), 0, INode.ModificationTimeSize);
                for (int i = 0; i < 10; i++)
                    fs.Write(BitConverter.GetBytes(IList[0].BlocksAddresses[i]), 0, 2); // 2 = sizeof(UInt16)
                fs.Seek(5060, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
            }
        }
        public void WriteRootUserInfo()
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                UserList[0] = new UserNode();
                fs.Seek(5060, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(UserList[0].ID), 0, UserNode.IDSize);
                fs.Write(BitConverter.GetBytes(UserList[0].GroupID), 0, UserNode.GroupIDSize);
                fs.Write(Encoding.UTF8.GetBytes(UserList[0].Login), 0, UserList[0].Login.Length);
                fs.Seek(15, SeekOrigin.Current);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
                fs.Write(Encoding.UTF8.GetBytes(UserList[0].Password), 0, UserList[0].Password.Length);
                
                //Test Second User Login And Password
                /*fs.Seek(17, SeekOrigin.Current);
                fs.Write(BitConverter.GetBytes(UserList[0].ID), 0, UserNode.IDSize);
                fs.Write(BitConverter.GetBytes(UserList[0].GroupID), 0, UserNode.GroupIDSize);
                fs.Write(Encoding.UTF8.GetBytes("ro ot"), 0, 5);
                fs.Seek(14, SeekOrigin.Current);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
                fs.Write(Encoding.UTF8.GetBytes(UserList[0].Password), 0, UserList[0].Password.Length);*/

                fs.Seek(5480, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
            }
        }
        public void CreateRootDir()
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                fs.Seek(5480, SeekOrigin.Begin);
                FileNode fnode = new FileNode();
                fs.Write(Encoding.UTF8.GetBytes(fnode.Name), 0, fnode.Name.Length);
                fs.Seek(12, SeekOrigin.Current);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
                fs.Write(Encoding.UTF8.GetBytes(fnode.Expansion), 0, fnode.Expansion.Length);
                fs.Seek(56831, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
            }
        }
    }
}
