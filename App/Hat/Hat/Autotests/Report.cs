using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hat
{
    public class Report
    {
        public const string PASSED = "Успешно";
        public const string FAILED = "Провально";
        public const string STOPPED = "Остановлено";
        public const string PROCESS = "Выполняется";
        public const string COMPLETED = "Выполнено";
        public const string WARNING = "Предупреждение";
        public const string ERROR = "ОШИБКА";
        public const string SCREENSHOT = "Скриншот";

        public static string TestFileName;
        public static string FileName;
        public static string FolderName;
        public static string FolderImagesName;
        public static int CountErrors;
        public static bool TestSuccess;
        public static List<string[]> Steps;

        public static void Init()
        {
            try
            {
                Report.TestFileName = Config.selectName;
                Report.FileName = $"Report-{Report.TestFileName}.html";
                Report.FileName = Report.FileName.Replace(".cs", "");
                Report.FolderName = Config.projectPath + "/reports/";
                Report.FolderImagesName = Config.projectPath + "/reports/screenshots/";
                Report.CountErrors = 0;
                Report.TestSuccess = false;
                Report.Steps = new List<string[]>();

                SaveReport(Report.TestSuccess);
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }
                
        public static void AddStep(string status, string action, string comment)
        {
            try
            {
                if (status == Report.FAILED || status == Report.ERROR) Report.CountErrors++;
                if (Report.Steps != null && status != Report.STOPPED && status != Report.PROCESS) Report.Steps.Add(new string[] { status, action, comment });
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        public static void SaveReport(bool testSuccess)
        {
            try
            {
                Report.TestSuccess = testSuccess;
                
                if (!Directory.Exists(Report.FolderName))
                {
                    Directory.CreateDirectory(Report.FolderName);
                    Directory.CreateDirectory(Report.FolderImagesName);
                    Config.browserForm.consoleMsg("Создана папка для отчетов");
                    Config.browserForm.updateProjectTree();
                }
                if (Directory.Exists(Report.FolderName))
                {
                    WorkOnFiles writer = new WorkOnFiles();
                    if (!File.Exists(Report.FolderName + Report.FileName))
                    {
                        writer.writeFile(GetHead() + GetBody() + GetFooter(), WorkOnFiles.UTF_8_BOM, Report.FolderName + Report.FileName);
                        if (File.Exists(Report.FolderName + Report.FileName))
                        {
                            Config.browserForm.consoleMsg($"Создан файл отчета {Report.FileName}");
                            if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Создан файл отчета {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.systemConsoleMsg($"Report file created {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                        else
                        {
                            Config.browserForm.consoleMsg($"Не удалось создать файл отчета {Report.FileName} по адресу {Report.FolderName}");
                            if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Не удалось создать файл отчета {Report.FileName} по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.systemConsoleMsg($"Failed to create report file {Report.FileName} in the folder {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                        Config.browserForm.updateProjectTree();
                    }
                    else
                    {
                        File.Delete(Report.FolderName + Report.FileName);
                        writer.writeFile(GetHead() + GetBody() + GetFooter(), WorkOnFiles.UTF_8_BOM, Report.FolderName + Report.FileName);
                        if (File.Exists(Report.FolderName + Report.FileName))
                        {
                            Config.browserForm.consoleMsg($"Обновлен файл отчета {Report.FileName}");
                            if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Обновлен файл отчета {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.systemConsoleMsg($"Updated report file {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                        else
                        {
                            Config.browserForm.consoleMsg($"Отсутствует файл отчета {Report.FileName} по адресу {Report.FolderName}");
                            if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Отсутствует файл отчета {Report.FileName} по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.systemConsoleMsg($"The report file is missing {Report.FileName} in the folder {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                    }
                }
                else
                {
                    Config.browserForm.consoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}");
                    if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else Config.browserForm.systemConsoleMsg($"Failed to create a folder for reports at {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        public static async Task SaveReportScreenshotAsync()
        {
            try
            {
                string filename = $"image-{DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss")}.jpeg";
                if (Directory.Exists(Report.FolderImagesName) == false)
                {
                    Directory.CreateDirectory(Report.FolderImagesName);
                }

                if (Directory.Exists(Report.FolderImagesName))
                {
                    Report.AddStep(Report.SCREENSHOT, $"Файл: <a href=\"./screenshots/{filename}\">{filename}</a>", $"<img src=\"./screenshots/{filename}\" />");
                    using (System.IO.FileStream file = System.IO.File.Create(Report.FolderImagesName + filename))
                    {
                        await Config.browserForm.getWebView().CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Jpeg, file);
                        if (File.Exists(Report.FolderImagesName + filename))
                        {
                            Config.browserForm.consoleMsg($"Скриншот {filename} - сохранён");
                            if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Скриншот {filename}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.systemConsoleMsg($"Screenshot {filename}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            Config.browserForm.updateProjectTree();
                        }
                        else
                        {
                            Config.browserForm.consoleMsg($"Не удалось сохранить скриншот {filename} по адресу {Report.FolderImagesName}");
                            if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Не удалось сохранить скриншот {filename} по адресу {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.systemConsoleMsg($"Failed to save screenshot {filename} in the folder {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                    }
                }
                else
                {
                    Config.browserForm.consoleMsg($"Не удалось сохранить скриншот потому что отсутствует папка {Report.FolderImagesName}");
                    if (Config.languageEng == false) Config.browserForm.systemConsoleMsg($"Не удалось сохранить скриншот потому что отсутствует папка {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else Config.browserForm.systemConsoleMsg($"The screenshot could not be saved because the folder is missing {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        public static string GetHead()
        {
            string content =
@"<!DOCTYPE html>
<html lang=""ru"">
<head>
<title>Отчет</title>
<meta charset=""UTF-8"" />
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
<meta http-equiv=""X-UA-Compatible"" content=""ie=edge"" />
<style type=""text/css"">
html { margin: 0; padding: 0; border: 0;}
body { background-color: #F9F7FF; font-family: ""Source Sans Pro"", Helvetica, sans-serif; font-size: 9pt; }
.wrapper { margin-left: auto; margin-right: auto; position: relative; max-width: 1440px; }
header { display: block; position: fixed; top: 0px; background-color: #F9F7FF; border-bottom: 1px solid #eaeff2; min-width: 1400px; max-width: 1400px; z-index: 1000; }
section { display: block; position: relative; min-width: 1400px; max-width: 1400px; }
table { margin: 0px; min-width: 1400px; max-width: 1400px; }
thead { background-color: #4d545d; color: #FFF; }
.table { position: relative; top: 140px; z-index: 1; }
.table-status { padding: 10px; min-width: 100px; max-width: 100px; }
.table-action { padding: 10px; min-width: 450px; max-width: 450px; }
.table-comment { padding: 10px; min-width: 700px; max-width: 700px; }
.table-row { background-color: #FFF; border-bottom: 1px solid #eaeff2; }
.table-row-empty { background-color: #F9F7FF;  }
.status-passed { background-color: #98C900; color: #FFFFFF; }
.status-failed { background-color: #E94B31; color: #FFFFFF; }
.status-stopped { background-color: #858585; color: #FFFFFF; }
.status-process { background-color: #FFFFFF; color: #222222; }
.status-completed { background-color: #0094FF; color: #FFFFFF; }
.status-warning { background-color: #FFE97F; color: #222222; }
.status-error { background-color: #F4CCCC; color: #FF0000; }
.status-screenshot { background-color: #FFFFFF; color: #222222; }
img { min-width: 700px; max-width: 700px; }
.result-passed { color: #007F0E; }
.result-failed { color: #7F0000; }
</style>
</head>";
            return content;
        }

        public static string GetFooter()
        {
            string content = "</html>";
            return content;
        }

        public static string GetBody()
        {
            string content = Environment.NewLine + "<body>" + Environment.NewLine;
            content += "<div class=\"wrapper\">" + Environment.NewLine;
            content += "<header>" + Environment.NewLine;
            content += "<h2>Отчет о работе автотеста</h2>" + Environment.NewLine;
            content += $"<h3>Файл: {Report.TestFileName}</h3>" + Environment.NewLine;
            if (Report.TestSuccess == true) content += "<h3>Результат: <span class=\"result-passed\">Успешно</span></h3>" + Environment.NewLine;
            else content += "<h3>Результат: <span class=\"result-failed\">Провально</span></h3>" + Environment.NewLine;
            content += "<table>" + Environment.NewLine;
            content += "<thead>" + Environment.NewLine;
            content += "<tr>" + Environment.NewLine;
            content += "<th class=\"table-status\">Статус</th>" + Environment.NewLine;
            content += "<th class=\"table-action\">Действие</th>" + Environment.NewLine;
            content += "<th class=\"table-comment\">Комментарий</th>" + Environment.NewLine;
            content += "</tr> " + Environment.NewLine;
            content += "</thead>" + Environment.NewLine;
            content += "</table>" + Environment.NewLine;
            content += "</header>" + Environment.NewLine;

            if (Report.Steps.Count > 0)
            {
                content += "<section>" + Environment.NewLine;
                content += "<table class=\"table\">" + Environment.NewLine;
                content += "<tbody>" + Environment.NewLine;

                try
                {
                    foreach (string[] step in Report.Steps)
                    {
                        content += "<tr>" + Environment.NewLine;
                        if(step[0] == "")
                        {
                            content += "<tr>" + Environment.NewLine;
                            content += $"<td class=\"table-status table-row-empty\">{step[0]}</td>" + Environment.NewLine;
                            content += $"<td class=\"table-action table-row-empty\">{step[1]}</td>" + Environment.NewLine;
                            content += $"<td class=\"table-comment table-row-empty\">{step[2]}</td>" + Environment.NewLine;
                            content += "</tr>" + Environment.NewLine;
                        }
                        else
                        {
                            if (step[0] == Report.PASSED) content += $"<td class=\"table-status table-row status-passed\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.FAILED) content += $"<td class=\"table-status table-row status-failed\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.STOPPED) content += $"<td class=\"table-status table-row status-stopped\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.PROCESS) content += $"<td class=\"table-status table-row status-process\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.COMPLETED) content += $"<td class=\"table-status table-row status-completed\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.WARNING) content += $"<td class=\"table-status table-row status-warning\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.ERROR) content += $"<td class=\"table-status table-row status-error\">{step[0]}</td>" + Environment.NewLine;
                            if (step[0] == Report.SCREENSHOT) content += $"<td class=\"table-status table-row status-screenshot\">{step[0]}</td>" + Environment.NewLine;
                            content += $"<td class=\"table-action table-row\">{step[1]}</td>" + Environment.NewLine;
                            content += $"<td class=\"table-comment table-row\">{step[2]}</td>" + Environment.NewLine;
                            content += "</tr>" + Environment.NewLine;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Config.browserForm.consoleMsg(ex.ToString());
                }
                content += "</tbody>" + Environment.NewLine;
                content += "<tfoot>" + Environment.NewLine;
                content += "<td class=\"table-status\">Ошибок: </td>" + Environment.NewLine;
                content += $"<td class=\"table-action\">{Report.CountErrors}</td>" + Environment.NewLine;
                content += "<td class=\"table-comment\"></td>" + Environment.NewLine;
                content += "</tfoot>" + Environment.NewLine;
                content += "</table>" + Environment.NewLine;
                content += "</section>" + Environment.NewLine;
            }

            content += "</div>" + Environment.NewLine;
            content += "</body>" + Environment.NewLine;
            return content;
        }


    }
}

