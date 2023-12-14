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


            switch (Console.ReadLine())
            {
                case "ls":
                    //var LSlist = os.GetListFiles();
                    //Console.WriteLine($"итого {LSlist.Count}");
                    //Console.WriteLine(string.Join("", LSlist));
                    break;

                case string s when Regex.IsMatch(s, @"^touch ([a-zA-Z0-9]+( *[a-zA-Z0-9])*(\.[a-zA-Z0-9]+)*)$"):
                    string fullName = Regex.Replace(s, @"^touch ", "");

                    string name = Path.GetFileNameWithoutExtension(fullName);
                    string expansion = Path.GetExtension(fullName).Split('.')[1];

                    if (name.Length <= 20 && expansion.Length <= 4)
                        sys.CreateFile(name, expansion);

                    break;

                case "help":

                    Console.WriteLine(
                        "ls\tОтображает содержимое корневой директории.\n" +
                        "cp\t<file>\tКопирует файлы  <file> \n" +
                        "touch\t<file>\tСоздает новый файл <file> или обновляет время его последнего доступа и модификации.\n" +
                        "rm\t<file>\tУдаляет указанный файл.\n" +
                        "echo\t<text> > <file>\tЗаписывает текст <text> в файл <file>. Может быть использована для дописывания в конец файла с >>.\n" +
                        "cat\t<file>\tВыводит текст из файла <file> в консоль.\n" +
                        "chmod\t<permissions> <file>\tИзменяет права доступа к файлу в соответствии с указанными <permissions>.\n" +
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



            









            Console.WriteLine(DateTime.Now.ToString("ddMMyyyy"));
            //Console.ReadLine();
        }
    }
}
