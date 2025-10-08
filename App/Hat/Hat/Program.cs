using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hat
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.ResetColor();
            Console.WriteLine("Browser Hat [License]: MIT License");
            Console.WriteLine("Browser Hat [version]: " + Assembly.GetExecutingAssembly().GetName().Version.ToString());
            Console.WriteLine("Browser Hat [update]: " + Config.dateBrowserUpdate);
            Console.WriteLine("Browser Hat [author]: Evgeniy Somov");
            Console.ResetColor();

            try {
                // удаление папки с кэшом
                if (Directory.Exists(Config.cacheFolder)) Directory.Delete(Config.cacheFolder, true);
                if (!Directory.Exists(Config.cacheFolder)) Config.statucCacheClear = "true";
            }
            catch (Exception ex)
            {
                Config.statucCacheClear = ex.Message;
            }
            

            if (args.Length == 2)
            {
                Config.commandLineMode = true;
                Config.selectName = args[0];
                Config.projectPath = args[1];
            }
            if (args.Length == 1)
            {
                if (args[0].Contains(".html") || args[0].Contains(".htm")) Config.openHtmlFile = args[0];
            }

            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(hWnd, 0);

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.ThreadExit += new EventHandler(Application_ThreadExit);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BrowserForm());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Report.AddStep(Report.ERROR, "", e.Exception.ToString());
            Config.testSuccess = false;
            Report.SaveReport(false);

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.ResetColor();
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.ResetColor();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Произошла ошибка:");
            Console.ResetColor();
            Console.WriteLine(e.Exception.ToString());
            Console.WriteLine("- - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Tests ended. Finished: FAILURE");
            Console.ResetColor();

            Environment.Exit(1);
        }

        static void Application_ThreadExit(Object sender, EventArgs e)
        {
            Application.ThreadExit -= Application_ThreadExit;
            if (Config.testSuccess == true) Environment.Exit(0);
            else Environment.Exit(1);
        }

    }
}
