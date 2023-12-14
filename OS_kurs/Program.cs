using System;
using System.Windows.Forms;

namespace OS_kurs
{
    internal static class Program
    {
        static FileSystem sys;
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Console.WriteLine("Hello World!");
            FileSystem sys = new FileSystem();

            string login = "";
            string password = "";
            do
            {
                Console.Write("Введите логин: ");
                login = Console.ReadLine();
                Console.Write("Введите пароль: ");
                password = Console.ReadLine();
                if (sys.IsLogin(login, password) == false)
                    Console.WriteLine("Ошибка! Неверное значение\n");
            } while (!sys.IsLogin(login, password));
            













            Console.WriteLine(DateTime.Now.ToString("ddMMyyyy"));
            Console.ReadLine();
        }
    }
}
