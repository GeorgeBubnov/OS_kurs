using OS_kurs.FS;
using System;
using System.IO;
using System.Text;

namespace OS_kurs
{
    class FileSystem
    {
        string path = "GeorgeFS.data";
        public INode[] IList = new INode[SuperBlock.IListSize];
        public UserNode[] UserList = new UserNode[10];
        public byte UserID = 0;
        public byte GroupID = 0;

        public FileSystem()
        {
            CreateDrive();
            CreateFile("file.txt");
        }

        public void CreateFile(string name)
        {
            UInt16 addr = WriteNewINode("TFr-x-w-", 0, 0);
            /*WriteNewINode("TFrwxrwx", 10, 1);
            WriteNewINode("TFr-----", 8, 0);*/

            WriteNewData(addr, "file", "txt", Encoding.UTF8.GetBytes("Hello World!"));

        }
        public UInt16 WriteNewINode(string access, UInt16 sizeInBytes, UInt16 sizeInBlocks)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                UInt16 address = 0;
                byte[] buffer = new byte[2];
                int i = 0;
                while(i < 20 && address == 0)
                {
                    fs.Seek(18 + i*2, SeekOrigin.Begin);
                    fs.Read(buffer, 0, 2);
                    address = (UInt16)BitConverter.ToInt16(buffer, 0);
                    i++;
                }
                if (address != 0)
                {
                    // Superblock
                    fs.Seek(-2, SeekOrigin.Current); // Затираем адрес в ListINode
                    fs.Write(BitConverter.GetBytes((UInt16)0), 0, 2);

                    fs.Seek(14, SeekOrigin.Begin); // Уменьшаем FreeINodeCount
                    fs.Read(buffer, 0, 2);
                    UInt16 freeINodeCount = (UInt16)BitConverter.ToInt16(buffer, 0);
                    freeINodeCount -= 1;
                    fs.Seek(-2, SeekOrigin.Current);
                    fs.Write(BitConverter.GetBytes(freeINodeCount), 0, 2);

                    // IList
                    fs.Seek(address, SeekOrigin.Begin); // Записываем атрибуты INode
                    fs.Write(Encoding.UTF8.GetBytes(access), 0, INode.AccessSize);
                    fs.Write(BitConverter.GetBytes(UserID), 0, INode.UserIDSize);
                    fs.Write(BitConverter.GetBytes(GroupID), 0, INode.GroupIDSize);
                    fs.Write(BitConverter.GetBytes(sizeInBytes), 0, INode.SizeInBytesSize); //TODO
                    fs.Write(BitConverter.GetBytes(sizeInBlocks), 0, INode.SizeInBlocksSize); //TODO
                    fs.Write(Encoding.UTF8.GetBytes(DateTime.Now.ToString("ddMMyyyy")), 0, INode.CreationTimeSize);
                    fs.Write(Encoding.UTF8.GetBytes(DateTime.Now.ToString("ddMMyyyy")), 0, INode.ModificationTimeSize);
                    return address;
                }
                return 0;
            }
        }
        public void WriteNewData(UInt16 address, string name, string expansion, byte[] data)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] buffer = new byte[2];
                //ListBlock
                fs.Seek(12, SeekOrigin.Begin); // Уменьшаем FreeBlockCount
                fs.Read(buffer, 0, 2);
                UInt16 freeBlockCount = (UInt16)BitConverter.ToInt16(buffer, 0);
                freeBlockCount -= 1;
                fs.Seek(-2, SeekOrigin.Current);
                fs.Write(BitConverter.GetBytes(freeBlockCount), 0, 2);

                fs.Seek(58, SeekOrigin.Begin); // Находим адрес списка в памяти
                fs.Read(buffer, 0, 2);
                UInt16 bladdress = (UInt16)BitConverter.ToInt16(buffer, 0);
                fs.Seek(bladdress, SeekOrigin.Begin);

                fs.Read(buffer, 0, 2); // Проверяем продолжение списка
                UInt16 newBladdress = (UInt16)BitConverter.ToInt16(buffer, 0);
                if (newBladdress == 0)
                {
                    fs.Seek(bladdress + (freeBlockCount + 1) * 2, SeekOrigin.Begin);
                    fs.Read(buffer, 0, 2);
                    newBladdress = (UInt16)BitConverter.ToInt16(buffer, 0);

                    fs.Seek(-2, SeekOrigin.Current); // Затираем адрес в ListBlock
                    fs.Write(BitConverter.GetBytes((UInt16)0), 0, 2);
                } // TODO

                fs.Seek(newBladdress, SeekOrigin.Begin); // Записываем имя и расширение
                fs.Write(Encoding.UTF8.GetBytes(name), 0, name.Length);
                fs.Seek(newBladdress + 20, SeekOrigin.Begin);
                fs.Write(Encoding.UTF8.GetBytes(expansion), 0, expansion.Length);

                fs.Seek(newBladdress + 24, SeekOrigin.Begin); // Записываем данные
                fs.Write(data, 0, data.Length);

                if (data.Length < 488)
                {
                    fs.Seek(newBladdress + 512, SeekOrigin.Begin); // Заполняем нулями до конца блока
                    fs.Write(BitConverter.GetBytes((UInt16)0), 0, 2);
                }

                fs.Seek(address + 10, SeekOrigin.Begin); // Записываем атрибуты INode
                fs.Write(BitConverter.GetBytes(data.Length), 0, INode.SizeInBytesSize);
                fs.Write(BitConverter.GetBytes((data.Length + 24) / 512 + 1), 0, INode.SizeInBlocksSize);
                fs.Seek(address + 22, SeekOrigin.Begin);
                fs.Write(Encoding.UTF8.GetBytes(DateTime.Now.ToString("ddMMyyyy")), 0, INode.ModificationTimeSize);
                fs.Write(BitConverter.GetBytes(newBladdress), 0, 2);
            }
        }
        public bool IsLogin(string login, string password) {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                if (login == "")
                    return false;

                for(int i = 5062; i < 5480; i += 42)
                {
                    byte[] buffer = new byte[20];
                    fs.Seek(i, SeekOrigin.Begin);
                    fs.Read(buffer, 0, 20);

                    string exLogin = GetValidString(buffer);

                    if (exLogin == login)
                    {
                        fs.Seek(i + 20, SeekOrigin.Begin);
                        fs.Read(buffer, 0, 20);

                        string exPassword = GetValidString(buffer);

                        if(exPassword == password)
                        {
                            fs.Seek(-2, SeekOrigin.Current);
                            fs.Read(buffer, 0, 1);

                            UserID = (byte)BitConverter.ToInt16(buffer, 0);
                            fs.Read(buffer, 0, 1);

                            GroupID = (byte)BitConverter.ToInt16(buffer, 0);
                            return true;
                        }
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

                CreateListBlock();
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
                fs.Write(Encoding.UTF8.GetBytes(IList[0].CreationDate), 0, INode.CreationTimeSize);
                fs.Write(Encoding.UTF8.GetBytes(IList[0].ModificationDate), 0, INode.ModificationTimeSize);
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
                fs.Seek(5992, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
            }
        }
        public void CreateListBlock()
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                fs.Seek(5992, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(0), 0, 2);
                for (UInt16 i = 6504; i < 56680; i += 512)
                    fs.Write(BitConverter.GetBytes(i), 0, 2);

                fs.Seek(56831, SeekOrigin.Begin);
                fs.Write(BitConverter.GetBytes(0), 0, 1);
            }
        }
    }
}
