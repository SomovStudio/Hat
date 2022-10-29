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

        public string[] DataMail { get; set; }
    }

    public static class Config
    {
        /* переменные для браузера */
        public static BrowserForm browserForm;                  // окно браузера (форма)
        public static string defaultUserAgent = "";             // значение user-agent по умолчанию
        public static string currentBrowserVersion = "1.1.7";   // текущая версия браузера
        public static string dateBrowserUpdate = "28.10.2022";  // дата последнего обновления

        public static string openHtmlFile = null;             // имя открываемого html файла при запуске браузера
        public static bool commandLineMode = false;         // флаг показывающий запуск приложения из командной строки
        public static string projectPath = "(не открыт)";   // полный путь к папке проекта
        public static string selectName = "";               // имя выбранного файла или папки
        public static string selectValue = "";              // полный путь к выбранному файлу или папке
        public static bool debugJavaScript = false;         // отладка javascript при выполнении автотеста
        public static string fileStartPage = Directory.GetCurrentDirectory() + "/readme.html"; // стартовая html страница

        /* кэш браузера */
        public static string cacheFolder = "Hat.exe.WebView2";  // кэш папка
        public static string statucCacheClear = "false";        // статус очистки кэша

        /* переменные для файла project.hat */
        public static string version = "1.1.6";                 // версия проекта
        public static string encoding = WorkOnFiles.UTF_8_BOM;  // кодировка
        public static bool editorTopMost = false;               // настройка отображения редактора
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
                jsonConfig.EditorTopMost = editorTopMost;
                jsonConfig.Libraries = libraries;
                jsonConfig.DataMail = dataMail;
                content = JsonConvert.SerializeObject(jsonConfig);
            }
            catch (Exception ex)
            {
                browserForm.consoleMsgError(ex.ToString());
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

        public static void saveConfigJson(string filename)
        {
            try
            {
                WorkOnFiles writer = new WorkOnFiles();
                writer.writeFile(Config.getConfig(), WorkOnFiles.UTF_8_BOM, filename);
            }
            catch (Exception ex)
            {
                browserForm.consoleMsgError(ex.ToString());
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
                dataMail = jsonConfig.DataMail;
            }
            catch (Exception ex)
            {
                browserForm.consoleMsgError(ex.ToString());
            }
        }

        public static void createStartPage()
        {
            try
            {
                if (!File.Exists(fileStartPage))
                {
                    string content =
@"
<!DOCTYPE html>
<html lang=""en"">
<head>
	<title>Приветствие</title>
	<meta charset=""UTF-8"" />
	<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
	<meta http-equiv=""X-UA-Compatible"" content=""ie=edge"" />
	<style type=""text/css"">
		html { margin: 0; padding: 0; border: 0;}
		body {
			background-color: #F9F7FF;
		    font-family: ""Source Sans Pro"", Helvetica, sans-serif;
		    font-size: 9pt;
		}
		#info {
			border: 1px solid #F1EDFF; 
			border-radius: 10px; 
			padding: 15px; 
			position: relative; 
			min-width: 400px;
			max-width: 400px; 
			margin-left: auto;
    		margin-right: auto;
    		margin-top: 5em;
    		text-align: center;
    		background-color: #F1EDFF;
    		box-shadow: 0 0 10px rgba(90,147,224,0.8);
    		color: #004A7F;
		}
		p{ text-align: left; }
	</style>
</head>
<body>
	<div id=""info"">
        <svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""64px"" height=""64px"" viewBox=""0 0 64 64"" enable-background=""new 0 0 64 64"" xml:space=""preserve"">
        <image id=""image0"" width=""64"" height=""64"" x=""0"" y=""0"" href=""data: image / png; base64,iVBORw0KGgoAAAANSUhEUgAAAEAAAABACAYAAACqaXHeAAAABGdBTUEAALGPC / xhBQAAACBjSFJN
		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
		AAAASUVORK5CYII="" />
		</svg>
		<h2>Browser Hat</h2>
        <p>
			Браузер со встроенной технологией автоматизированного тестирования Web приложений.
			<br>
            Особенность браузера в том что автотесты напрямую выполняются в браузере без Selenium и WebDriver.
			<br><br>
			Встроенный фреймворк HatFramework содержит достаточное количество методов необходимых для выполнения основных задач автоматизации тестирования. Для описания скриптов автотестов используется язык программирования C# и встроенный редактор кода. Так же в качестве редактора можно воспользоваться Visual Studio. Удобный интерфейс браузера отображает все шаги выполнения теста с подробным описанием событий. Результат проверки формируется в отчет и отправляются на указаную почту. Запуск автотестов возможен из командной строки операционной системы Windows это пригодится при использовании автотестов в популярных средствах непрерывной интеграции таких как: Jenkins, TeamCity, GitLab CI/CD.
		</p>
		<p>
			Программа разработана при поддержке компании <a href=""https://zionec.ru/"">Зионек</a>
		</p>
		<p>
			Благодарим вас за использование нашего приложения.
		</p>
		<h4>
			Copyright © 2022 Somov Studio. All Rights Reserved.
		</h4>
	</div>
</body>
</html>
";
                    StreamWriter writer;
                    writer = new StreamWriter(fileStartPage, false, new UTF8Encoding(true));
                    writer.Write(content);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

    }
}
