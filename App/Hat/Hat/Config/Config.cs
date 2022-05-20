using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace Hat
{
    public class JsonConfig
    {
        public string Version { get; set; }
        public string Encoding { get; set; }
        public bool EditorTopMost { get; set; }

        public string[] Libraries { get; set; }
    }

    public static class Config
    {
        public static BrowserForm browserForm;
        public static IntPtr consoleHandle;


        public static bool commandLineMode = false;         // флаг показывающий запуск приложения из командной строки
        public static string projectPath = "(не открыть)";  // полный путь к папке проекта
        public static string selectName = "";               // имя выбранного файла или папки
        public static string selectValue = "";              // полный путь к выбранному файлу или папке

        public static string version = "1.0.0";
        public static string encoding = WorkOnFiles.UTF_8_BOM;
        public static bool editorTopMost = true;
        public static string[] libraries = new string[]
        {
            "HatFramework.dll",
            "Microsoft.Web.WebView2.Core.dll",
            "Microsoft.Web.WebView2.WinForms.dll",
            "Microsoft.Web.WebView2.Wpf.dll",
            "Newtonsoft.Json.dll",
            "System.dll",
            "System.ComponentModel.DataAnnotations.dll",
            "System.Core.dll",
            "System.Data.dll",
            "System.Data.DataSetExtensions.dll",
            "System.Deployment.dll",
            "System.Drawing.dll",
            "System.Net.Http.dll",
            "System.Numerics.dll",
            "System.Runtime.dll",
            "System.Windows.Forms.dll",
            "System.Xml.dll",
            "System.Xml.Linq.dll"
        };

        public static string getConfig()
        {
            string content = "";
            try
            {
                JsonConfig jsonConfig = new JsonConfig();
                jsonConfig.Version = version;
                jsonConfig.Encoding = encoding;
                jsonConfig.EditorTopMost = editorTopMost;
                jsonConfig.Libraries = libraries;
                content = JsonConvert.SerializeObject(jsonConfig);
            }
            catch (Exception ex)
            {
                browserForm.consoleMsg("Ошибка: " + ex.ToString());
            }
            return content;
        }

        public static void saveConfigJson(string filename)
        {
            try
            {
                WorkOnFiles writer = new WorkOnFiles();
                writer.writeFile(Config.getConfig(), WorkOnFiles.UTF_8_BOM, filename);
            }
            catch (Exception ex)
            {
                browserForm.consoleMsg("Ошибка: " + ex.ToString());
            }
        }

        /* Как сериализировать и десериализировать (маршалирование и демаршалирование) JSON в .NET
         * https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-how-to?pivots=dotnet-6-0
         * https://www.nuget.org/packages/System.Text.Json
         */
        public static void readConfigJson(string filename)
        {
            try
            {
                WorkOnFiles reader = new WorkOnFiles();
                string content = reader.readFile(WorkOnFiles.UTF_8_BOM, filename);
                JsonConfig jsonConfig = JsonConvert.DeserializeObject<JsonConfig>(content);
                version = jsonConfig.Version;
                encoding = jsonConfig.Encoding;
                editorTopMost = jsonConfig.EditorTopMost;
                libraries = jsonConfig.Libraries;
            }
            catch (Exception ex)
            {
                browserForm.consoleMsg("Ошибка: " + ex.ToString());
            }
        }



    }
}
