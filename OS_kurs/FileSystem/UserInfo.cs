using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_kurs.FileSystem
{
    internal class UserInfo
    {
        public User[] users = new User[999];
        public UserInfo()
        {
            users[0] = new User();
            users[0].ID = 0;
            users[0].GroupID = 0;
            users[0].Login = "root";
            users[0].Password = "123";
        }
        public struct User
        {
            public Int32 ID;
            public Int32 GroupID;
            public string Login;
            public string Password;
        }
    }
}
