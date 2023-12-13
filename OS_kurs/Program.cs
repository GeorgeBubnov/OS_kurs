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
            if (sys.IsLogin("ro ot", "123"))
            {
                Console.WriteLine("\nTRUE\n");
            }


            //Console.WriteLine(DateTime.Now.ToString("ddmmyyyy"));
            Console.ReadLine();
        }
    }
}
