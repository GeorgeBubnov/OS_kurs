using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OS_kurs
{
    internal static class Program
    {
        static FileSystem sys;
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            Console.WriteLine("Hello World!");
            FileSystem sys = new FileSystem();

            /*string login = "";
            string password = "";
            do
            {
                Console.Write("Введите логин: ");
                login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                password = Console.ReadLine();
                if (sys.IsLogin(login, password) == false)
                    Console.WriteLine("Ошибка! Неверное значение\n");
            } while (!sys.IsLogin(login, password));*/

            while (true)
            {
                Console.Write("root>");

                switch (Console.ReadLine())
                {
                    case "ls":
                        Console.Write(sys.ReadDirectory());
                        break;

                    case string s when Regex.IsMatch(s, @"^touch [a-zA-Z0-9]+\.[a-z]+$"):
                        string fullName = Regex.Replace(s, @"^touch ", "");

                        string name = Path.GetFileNameWithoutExtension(fullName);
                        string expansion = Path.GetExtension(fullName).Split('.')[1];

                        if (name.Length <= 20 && expansion.Length <= 4)
                            sys.CreateFile(name, expansion);
                        break;

                    case string s when Regex.IsMatch(s, @"^mkdir [a-zA-Z0-9]+$"):
                        string dirName = Regex.Replace(s, @"^mkdir ", "");

                        if (dirName.Length <= 20)
                            sys.CreateDir(dirName);
                        break;

                    case string s when Regex.IsMatch(s, @"^rmdir [a-zA-Z0-9]+$"):
                        string ddirName = Regex.Replace(s, @"^rmdir ", "");

                        if (ddirName.Length <= 20)
                            sys.RemoveDir(ddirName);
                        break;

                    case string s when Regex.IsMatch(s, @"^cd ([a-zA-Z0-9]+|\.)$"):
                        string cdirName = Regex.Replace(s, @"^cd ", "");

                        if (cdirName.Length <= 20)
                            if(sys.ChangeDir(cdirName))
                                Console.WriteLine("Переход прошел успешно!");
                        break;

                    case string s when Regex.IsMatch(s, @"^chmod [r\-][w\-][x\-][r\-][w\-][x\-] [a-zA-Z0-9]+\.[a-z]+$"):
                        string[] chmodv = Regex.Replace(s, @"^chmod ", "").Split(' ');
                        if (sys.ChangeRights(chmodv[0], chmodv[1]))
                            Console.WriteLine("Права изменены успешно!");
                        break;

                    // Только для файлов
                    case string s when Regex.IsMatch(s, @"^cp [a-zA-Z0-9]+\.[a-z]+ [a-zA-Z0-9]+\.[a-z]+$"):
                        string[] cpv = Regex.Replace(s, @"^cp ", "").Split(' ');
                        sys.CopyFile(cpv[0], cpv[1]);
                        break;

                    case string s when Regex.IsMatch(s, @"^cpdir [a-zA-Z0-9]+ [a-zA-Z0-9]+$"):
                        string[] dcpv = Regex.Replace(s, @"^cpdir ", "").Split(' ');
                        sys.CopyDir(dcpv[0], dcpv[1]);
                        break;

                    case string s when Regex.IsMatch(s, @"^rm [a-zA-Z0-9]+\.[a-z]+$"):
                        sys.Remove(Regex.Replace(s, @"^rm ", ""));
                        break;

                    case string s when Regex.IsMatch(s, @"^echo .+ >> [a-zA-Z0-9]+\.[a-z]+$"):
                        //Если добавить в конец
                        string EchoVal = Regex.Replace(s, @"^echo ", "");
                        string Values = Regex.Replace(EchoVal, @" >> [a-zA-Z0-9]+\.[a-z]+$", "");
                        string NameFile = Regex.Replace(EchoVal, @".+ >> ", "");
                        sys.WriteInFile(NameFile, Values);
                        break;


                    case "help":
                        // TODO Пресматривать список INode
                        Console.WriteLine(
                            " touch\t<file>\tСоздает новый файл <file> или обновляет время его последнего доступа и модификации.\n" +
                            " ls\tОтображает содержимое корневой директории.\n" +
                            " chmod\t<permissions> <file>\tИзменяет права доступа к файлу в соответствии с указанными <permissions>.\n" +
                            " cp\t<file>\tКопирует файлы  <file> \n" + // TODO Directory
                            " rm\t<file>\tУдаляет указанный файл.\n" +
                            " mkdir \n" +
                            " cd \n" +
                            " rmdir \n" +
                            " cpdir \n" + // TODO WITHOUT DATA
                            " echo\t<text> > <file>\t Может быть использована для дописывания в конец файла с >>.\n" + // TODO BIGDATA
                            "cat\t<file>\tВыводит текст из файла <file> в консоль.\n" +
                            "chown\t<user> <file>\tИзменяет владельца (<user>) файла <file>.\n" +
                            "rename\t<file> <name>\tИзменяет название <name> файла <file>.\n" +
                            "useradd\t<username> <passowrd> <admin>\tСоздает нового пользователя с указанным именем <username>, паролем <passowrd> и правами администратора true или false в <admin>.\n" +
                            "userdel\t<username>\tУдаляет пользователя с указанным именем <username>.\n" +
                            "login\t<username> <passowrd>\tВход в систему под указанным именем пользователя <username> с использованием пароля <passowrd>.\n" +
                            "logout\t-\tВыход из системы.\n" +
                            "users\tОтображает всех существующих пользователей в системе\n" +
                            "clear\t\tОчистить консоль\n"
                            );
                        break;

                    default:
                        Console.WriteLine("Команда не найдена");
                        break;
                }
            }












            Console.WriteLine(DateTime.Now.ToString("ddMMyyyy"));
            //Console.ReadLine();
        }
    }
}
