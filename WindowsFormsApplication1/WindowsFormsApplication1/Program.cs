using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public static class Data
    {
        public delegate void MyEvent(int number, string data);
        public static MyEvent EventHandler;
    }
    public static class Data1
    {
        public delegate bool MyEvent(string data);
        public static MyEvent EventHandler;
    }
    public static class Data2
    {
        public delegate void MyEvent(string data);
        public static MyEvent EventHandler;
    }
}
