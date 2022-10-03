using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BrowserForm());
        }
    }
}
