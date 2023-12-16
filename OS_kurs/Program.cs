using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace OS_kurs
{
    internal static class Program
    {
        static FileSystem sys;
        static string login = "";
        static string password = "";
        static void Main()
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());*/

            Console.WriteLine("Hello World!");
            sys = new FileSystem();


            Login();

            while (true)
            {
                Console.Write($"{login}>");

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

                    case string s when Regex.IsMatch(s, @"^move [a-zA-Z0-9]+\.[a-z]+ ([a-zA-Z0-9]+|\.)$"):
                        string[] mv = Regex.Replace(s, @"^move ", "").Split(' ');
                        sys.MoveFile(mv[0], mv[1]);
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

                    case string s when Regex.IsMatch(s, @"^cat [a-zA-Z0-9]+\.[a-z]+$"):
                        Console.WriteLine(sys.ReadFile(Regex.Replace(s, @"^cat ", "")));
                        break;

                    case string s when Regex.IsMatch(s, @"^rename [a-zA-Z0-9]+\.[a-z]+ [a-zA-Z0-9]+\.[a-z]+$"):
                        string[] rv = Regex.Replace(s, @"^rename ", "").Split(' ');
                        sys.Rename(rv[0], rv[1]);
                        break;

                    case string s when Regex.IsMatch(s, @"^renamedir [a-zA-Z0-9]+ [a-zA-Z0-9]+$"):
                        string[] rdv = Regex.Replace(s, @"^renamedir ", "").Split(' ');
                        sys.RenameDir(rdv[0], rdv[1]);
                        break;

                    case "users":
                        Console.Write(sys.GetAllUsers());
                        break;

                    case string s when Regex.IsMatch(s, @"^adduser [a-zA-Z0-9]+ [a-zA-Z0-9]+$"):
                        string[] auv = Regex.Replace(s, @"^adduser ", "").Split(' ');
                        sys.AddUser(auv[0], auv[1]);
                        break;

                    case "login":
                        Login();
                        break;

                    case string s when Regex.IsMatch(s, @"^chgroup [a-zA-Z0-9]+ [0-9]+$"):
                        string[] gv = Regex.Replace(s, @"^chgroup ", "").Split(' ');
                        sys.ChangeGroup(gv[0], gv[1]);
                        break;

                    case "exit":
                        return;

                    case "help":
// TODO Пресматривать список INode
                        Console.WriteLine(
                            " touch\t<file>\tСоздает новый файл <file> или обновляет время его последнего доступа и модификации.\n" +
                            " ls\tОтображает содержимое корневой директории.\n" +
                            " chmod\t<permissions> <file>\tИзменяет права доступа к файлу в соответствии с указанными <permissions>.\n" +
                            " cp\t<file>\tКопирует файлы  <file> \n" +
                            " rm\t<file>\tУдаляет указанный файл.\n" +
                            " mkdir \n" +
                            " cd \n" +
                            " rmdir \n" +
                            " cpdir \n" + // TODO WITHOUT DATA
                            " echo\t<text> > <file>\t Может быть использована для дописывания в конец файла с >>.\n" + // TODO BIGDATA
                            " cat\t<file>\tВыводит текст из файла <file> в консоль.\n" + // TODO BIGDATA
                            " move \n" + // MB TODO with slash / || TODO Directory
                            " rename\t<file> <name>\tИзменяет название <name> файла <file>.\n" +
                            " renamedir \n" +
                            " users\tОтображает всех существующих пользователей в системе\n" +
                            " adduser \n" +
                            " login \n" +
                            " chgroup \n" + // TODO FUNCTIONALITY чтобы читались права доступа в ls и т.д.
                            " exit \n"
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

        static void Login()
        {
            do
            {
                Console.Write("Введите логин: ");
                login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                password = Console.ReadLine();
                if (sys.IsLogin(login, password) == false)
                    Console.WriteLine("Ошибка! Неверное значение\n");
            } while (!sys.IsLogin(login, password));
        }
    }
}
