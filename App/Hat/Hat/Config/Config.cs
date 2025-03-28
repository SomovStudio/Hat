﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

namespace Hat
{
    public class JsonConfig
    {
        public string Version { get; set; }
        public string Encoding { get; set; }
        public bool LanguageEngConsole { get; set; }
        public bool LanguageEngReportMail { get; set; }
        public bool EditorTopMost { get; set; }
        public bool FullReport { get; set; }
        public string[] Libraries { get; set; }
        public string[] DataMail { get; set; }
    }

    public static class Config
    {
        /* переменные для браузера */
        public static BrowserForm browserForm;                  // окно браузера (форма)
        public static bool testSuccess = true;                  // статус тестирования
        public static string defaultUserAgent = "";             // значение user-agent по умолчанию
        public static string currentBrowserVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();   // текущая версия браузера 1.4.*
        public static string dateBrowserUpdate = "06.03.2025";   // дата последнего обновления

        public static string openHtmlFile = null;               // имя открываемого html файла при запуске браузера
        public static bool commandLineMode = false;             // флаг показывающий запуск приложения из командной строки
        public static string projectPath = "(не открыт)";       // полный путь к папке проекта
        public static string selectName = "";                   // имя выбранного файла или папки
        public static string selectValue = "";                  // полный путь к выбранному файлу или папке
        public static bool debugJavaScript = false;             // отладка javascript при выполнении автотеста

        /* кэш браузера */
        public static string cacheFolder = AppDomain.CurrentDomain.FriendlyName + ".WebView2";  // кэш папка
        public static string statucCacheClear = "false";        // статус очистки кэша

        /* переменные для файла project.hat */
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();                 // версия проекта 1.3.*
        public static string encoding = WorkOnFiles.UTF_8_BOM;  // кодировка
        public static bool languageEngConsole = true;           // поддержка английского языка в консоли (по умолчанию английский)
        public static bool languageEngReportMail = false;       // поддержка английского языка в отече и письме (по умолчанию русский)
        public static bool editorTopMost = false;               // настройка отображения редактора
        public static bool fullReport = true;                   // режим вывода (полный отчет или краткий отчет)
        public static string[] libraries = new string[]         // библиотека подключаемых dll файлов
        {
            "HatFramework.dll",
            "Microsoft.Web.WebView2.Core.dll",
            "Microsoft.Web.WebView2.WinForms.dll",
            //"Microsoft.Web.WebView2.Wpf.dll",
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
        public static string[] dataMail = new string[]
        {
            "from@mail.com",                // 0 - Почта отправителя
            "user",                         // 1 - Имя отправителя
            "pass",                         // 2 - Пароль отправителя
            "to1@mail.com to2@mail.com",    // 3 - Почта получателя
            "smtp.yandex.ru",               // 4 - smtp
            "587",                          // 5 - port (587)
            "true"                          // 6 - ssl
        };

        public static string getConfig()
        {
            string content = "";
            try
            {
                version = currentBrowserVersion;

                JsonConfig jsonConfig = new JsonConfig();
                jsonConfig.Version = version;
                jsonConfig.Encoding = encoding;
                jsonConfig.LanguageEngConsole= languageEngConsole;
                jsonConfig.LanguageEngReportMail = languageEngReportMail;
                jsonConfig.EditorTopMost = editorTopMost;
                jsonConfig.FullReport = fullReport;
                jsonConfig.Libraries = libraries;
                jsonConfig.DataMail = dataMail;
                content = JsonConvert.SerializeObject(jsonConfig);
            }
            catch (Exception ex)
            {
                browserForm.ConsoleMsgError(ex.ToString());
            }
            return content;
        }

        public static void defaultDataMail()
        {
            dataMail = new string[] {
                "from@mail.com",                // 0 - Почта отправителя
                "user",                         // 1 - Имя отправителя
                "pass",                         // 2 - Пароль отправителя
                "to1@mail.com to2@mail.com",    // 3 - Почта получателя
                "smtp.yandex.ru",               // 4 - smtp
                "587",                          // 5 - port (587)
                "true"                          // 6 - ssl
            };
        }

        public static void defaultFlags()
        {
            if (HatSettings.language == HatSettings.RUS)
            {
                Config.languageEngConsole = true;
                Config.languageEngReportMail = false;
            }
            else
            {
                Config.languageEngConsole = true;
                Config.languageEngReportMail = true;
            }
            Config.fullReport = true;
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
                browserForm.ConsoleMsgError(ex.ToString());
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
                languageEngConsole = jsonConfig.LanguageEngConsole;
                languageEngReportMail = jsonConfig.LanguageEngReportMail;
                editorTopMost = jsonConfig.EditorTopMost;
                fullReport = jsonConfig.FullReport;
                libraries = jsonConfig.Libraries;
                dataMail = jsonConfig.DataMail;
            }
            catch (Exception ex)
            {
                browserForm.ConsoleMsgError(ex.ToString());
            }
        }

    }
}
