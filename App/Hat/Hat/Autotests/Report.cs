﻿using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Shapes;

namespace Hat
{
    public class Report
    {
        public const string PASSED = "PASSED";
        public const string FAILED = "FAILED";
        public const string STOPPED = "STOPPED";
        public const string PROCESS = "PROCESS";
        public const string COMPLETED = "COMPLETED";
        public const string WARNING = "WARNING";

        public const string ERROR = "ERROR";
        public const string SCREENSHOT = "SCREENSHOT";

        public const string SUCCESS = "SUCCESS";
        public const string FAILURE = "FAILURE";
        public const string AT_WORK = "AT_WORK";

        public const string DEFAULT = "DEFAULT";
        public const string UTF_8 = "UTF-8";
        public const string UTF_8_BOM = "UTF-8 BOM";
        public const string WINDOWS_1251 = "WINDOWS-1251";

        public static string Description;
        public static string Date;
        public static string TestFileName;
        public static string FileName;
        public static string FolderName;
        public static string FolderImagesName;
        public static int CountErrors;
        public static bool TestSuccess;
        public static List<string[]> Steps;
        public static string Log;

        public static void Init()
        {
            try
            {
                Report.Description = "";
                Report.Date = DateTime.Now.ToString();
                Report.TestFileName = Config.selectName;
                Report.FileName = $"Report-{Report.TestFileName}.html";
                Report.FileName = Report.FileName.Replace(".cs", "");
                Report.FolderName = Config.projectPath + "/reports/";
                Report.FolderImagesName = Config.projectPath + "/reports/screenshots/";
                Report.CountErrors = 0;
                Report.TestSuccess = false;
                Report.Steps = new List<string[]>();
                Report.Log = "";

                SaveReport(Report.TestSuccess, true);
            }
            catch (Exception ex)
            {
                Report.ShowErrorInSystemConsole(ex.ToString());
            }
        }
        
        public static void SetDescription(string text)
        {
            Description = text;
        }

        public static void ShowErrorInSystemConsole(string message)
        {
            Config.browserForm.SystemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg("Произошла ошибка:", default, ConsoleColor.Black, ConsoleColor.Red, true);
            else Config.browserForm.SystemConsoleMsg("An error has occurred:", default, ConsoleColor.Black, ConsoleColor.Red, true);
            Config.browserForm.SystemConsoleMsg(message, default, default, default, true);
            Config.browserForm.SystemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
            Config.browserForm.SystemConsoleMsg("", default, default, default, true);
        }

        public static void AddStep(string status, string action, string comment)
        {
            try
            {
                if (status == null)
                {
                    Report.Steps.Add(new string[] { "", action, comment });
                }
                else
                {
                    if (status == Report.FAILED || status == Report.ERROR) Report.CountErrors++;
                    if (Report.Steps != null && status != Report.STOPPED && status != Report.PROCESS) Report.Steps.Add(new string[] { status, action, comment });
                }
            }
            catch (Exception ex)
            {
                Report.ShowErrorInSystemConsole(ex.ToString());
            }
        }

        public static void SaveLogFailed()
        {
            try
            {
                if (Report.TestSuccess == true) return;
                if (Report.FolderName != "")
                {
                    if (!Directory.Exists(Report.FolderName)) Directory.CreateDirectory(Report.FolderName);

                    Report.Log = $"TEST [{Report.Date}][{Report.TestFileName}]: {Report.Description}" + Environment.NewLine + Report.Log;
                    Report.Log += "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" + Environment.NewLine;
                    File.AppendAllText(Report.FolderName + "log.txt", Report.Log);
                    if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg("Новая запись в файле log.txt" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else Config.browserForm.SystemConsoleMsg("New entry in the file log.txt" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg("Сохранение log файла: " + Report.FolderName + "log.txt - не удалось сохранить из за ошибки: " + ex.Message,
                    "Saving a log file: " + Report.FolderName + "log.txt - failed to save due to an error: " + ex.Message);
                //Config.browserForm.SystemConsoleMsg("Сохранение log файла: " + Report.FolderName + "log.txt - неудалось сохранить из за ошибки: " + ex.Message, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true);
                Report.ShowErrorInSystemConsole(ex.ToString());
            }
        }

        public static void SaveReport(bool testSuccess, bool init = false)
        {
            try
            {
                Report.TestSuccess = testSuccess;
                
                if (!Directory.Exists(Report.FolderName))
                {
                    Directory.CreateDirectory(Report.FolderName);
                    Directory.CreateDirectory(Report.FolderImagesName);
                    Config.browserForm.ConsoleMsg("Создана папка для отчетов", "A folder for reports has been created");
                    Config.browserForm.updateProjectTree();
                }
                if (Directory.Exists(Report.FolderName))
                {
                    if (!File.Exists(Report.FolderName + Report.FileName))
                    {
                        Report.writeFile(GetHead(init) + GetBody() + GetFooter(), Report.UTF_8_BOM, Report.FolderName + Report.FileName);
                        if (File.Exists(Report.FolderName + Report.FileName))
                        {
                            Config.browserForm.ConsoleMsg($"Создан файл отчета {Report.FileName}", $"A report file has been created {Report.FileName}");
                            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Создан файл отчета {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.SystemConsoleMsg($"Report file created {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                        else
                        {
                            Config.browserForm.ConsoleMsg($"Не удалось создать файл отчета {Report.FileName} по адресу {Report.FolderName}", 
                                $"The report file could not be created {Report.FileName} at the address {Report.FolderName}");
                            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Не удалось создать файл отчета {Report.FileName} по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.SystemConsoleMsg($"Failed to create report file {Report.FileName} in the folder {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                        Config.browserForm.updateProjectTree();
                    }
                    else
                    {
                        File.Delete(Report.FolderName + Report.FileName);
                        Report.writeFile(GetHead(init) + GetBody() + GetFooter(), Report.UTF_8_BOM, Report.FolderName + Report.FileName);
                        if (File.Exists(Report.FolderName + Report.FileName))
                        {
                            Config.browserForm.ConsoleMsg($"Обновлен файл отчета {Report.FileName}", $"The report file has been updated {Report.FileName}");
                            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Обновлен файл отчета {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.SystemConsoleMsg($"Updated report file {Report.FileName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                        else
                        {
                            Config.browserForm.ConsoleMsg($"Отсутствует файл отчета {Report.FileName} по адресу {Report.FolderName}", 
                                $"The report file is missing {Report.FileName} at the address {Report.FolderName}");
                            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Отсутствует файл отчета {Report.FileName} по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.SystemConsoleMsg($"The report file is missing {Report.FileName} in the folder {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                    }
                }
                else
                {
                    Config.browserForm.ConsoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}", 
                        $"Failed to create a folder for reports at {Report.FolderName}");
                    if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else Config.browserForm.SystemConsoleMsg($"Failed to create a folder for reports at {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }

                if (init == false) SaveLogFailed();
            }
            catch (Exception ex)
            {
                Report.ShowErrorInSystemConsole(ex.ToString());
            }

            Report.SaveResultReport();
        }

        public static async Task SaveReportScreenshotAsync()
        {
            try
            {
                string filename = $"image-{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.jpeg";
                if (Directory.Exists(Report.FolderImagesName) == false)
                {
                    Directory.CreateDirectory(Report.FolderImagesName);
                }

                if (Directory.Exists(Report.FolderImagesName))
                {
                    if (Config.languageEngReportMail == false) Report.AddStep(Report.SCREENSHOT, $"Файл: <a href=\"./screenshots/{filename}\">{filename}</a>", $"<img src=\"./screenshots/{filename}\" />");
                    else Report.AddStep(Report.SCREENSHOT, $"File: <a href=\"./screenshots/{filename}\">{filename}</a>", $"<img src=\"./screenshots/{filename}\" />");

                    using (System.IO.FileStream file = System.IO.File.Create(Report.FolderImagesName + filename))
                    {
                        await Config.browserForm.GetWebView().CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Jpeg, file);
                        if (File.Exists(Report.FolderImagesName + filename))
                        {
                            Config.browserForm.ConsoleMsg($"Скриншот {filename} - сохранён", $"Screenshot {filename} - is saved");
                            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Скриншот {filename}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.SystemConsoleMsg($"Screenshot {filename}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            Config.browserForm.updateProjectTree();
                        }
                        else
                        {
                            Config.browserForm.ConsoleMsg($"Не удалось сохранить скриншот {filename} по адресу {Report.FolderImagesName}", 
                                $"Failed to save screenshot {filename} at the address {Report.FolderImagesName}");
                            if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Не удалось сохранить скриншот {filename} по адресу {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                            else Config.browserForm.SystemConsoleMsg($"Failed to save screenshot {filename} in the folder {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        }
                    }
                }
                else
                {
                    Config.browserForm.ConsoleMsg($"Не удалось сохранить скриншот потому что отсутствует папка {Report.FolderImagesName}", 
                        $"The screenshot could not be saved because the folder is missing {Report.FolderImagesName}");
                    if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Не удалось сохранить скриншот потому что отсутствует папка {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else Config.browserForm.SystemConsoleMsg($"The screenshot could not be saved because the folder is missing {Report.FolderImagesName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }
            }
            catch (Exception ex)
            {
                Report.ShowErrorInSystemConsole(ex.ToString());
            }
        }

        /* =======================================================================================
         * Страница отчета автотестов
         * */

        public static string GetHead(bool init = false)
        {
            string content = "<!--" + Environment.NewLine;
            if (init == true) content += Report.AT_WORK + Environment.NewLine;
            if (init == false && Report.TestSuccess == true) content += Report.SUCCESS + Environment.NewLine;
            if (init == false && Report.TestSuccess == false) content += Report.FAILURE + Environment.NewLine;
            content += Report.TestFileName + Environment.NewLine;
            content += Report.Description + Environment.NewLine;
            content += Report.Date + Environment.NewLine;
            content += DateTime.Now.ToString() + Environment.NewLine;
            content += "-->" + Environment.NewLine;

            if (Config.languageEngReportMail == false)
            {
                content += "<!DOCTYPE html>" + Environment.NewLine;
                content += "<html lang=\"ru-RU\">" + Environment.NewLine;
                content += "<head>" + Environment.NewLine;
                content += "<title>Отчет</title>" + Environment.NewLine;
            }
            else
            {
                content += "<!DOCTYPE html>" + Environment.NewLine;
                content += "<html lang=\"en-EN\">" + Environment.NewLine;
                content += "<head>" + Environment.NewLine;
                content += "<title>Report</title>" + Environment.NewLine;
            }
            content +=
@"<meta charset=""UTF-8"" />
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
<meta http-equiv=""X-UA-Compatible"" content=""ie=edge"" />
<style type=""text/css"">
html { margin: 0; padding: 0; border: 0;}
body { background-color: #F9F7FF; font-family: ""Source Sans Pro"", Helvetica, sans-serif; font-size: 10pt; }
.wrapper { margin-left: auto; margin-right: auto; position: relative; max-width: 1440px; }
header { display: block; position: fixed; top: 0px; background-color: #F9F7FF; border-bottom: 1px solid #eaeff2; min-width: 1400px; max-width: 1400px; z-index: 1000; }
section { display: block; position: relative; min-width: 1400px; max-width: 1400px; }
table { margin: 0px; min-width: 1400px; max-width: 1400px; }
thead { background-color: #4d545d; color: #FFF; }
tr:hover .content-hidden{  background: #EFF7FF; }
.content-hidden { overflow: hidden; }
.content-scroll { overflow: scroll; }
.table { position: relative; top: 180px; z-index: 1; overflow: hidden; }
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
.table-footer { padding-top: 50px; padding-right: 10px; padding-bottom: 20px; float: right; }
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

            if (Config.languageEngReportMail == false)
            {
                content += "<h2>Отчет о работе автотеста</h2>" + Environment.NewLine;
                content += $"<b>Описание: </b>{Report.Description}" + Environment.NewLine;
                content += $"<br><b>Файл: </b>{Report.TestFileName}" + Environment.NewLine;
                content += $"<br><b>Дата: </b>{Report.Date}" + Environment.NewLine;
                if (Report.TestSuccess == true) content += "<h3>Результат: <span class=\"result-passed\">Успешно</span></h3>" + Environment.NewLine;
                else content += "<h3>Результат: <span class=\"result-failed\">Неудачно</span></h3>" + Environment.NewLine;
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
            }
            else
            {
                content += "<h2>Autotest Report</h2>" + Environment.NewLine;
                content += $"<b>Description: </b>{Report.Description}" + Environment.NewLine;
                content += $"<br><b>File: </b>{Report.TestFileName}" + Environment.NewLine;
                content += $"<br><b>Date: </b>{Report.Date}" + Environment.NewLine;
                if (Report.TestSuccess == true) content += "<h3>Result: <span class=\"result-passed\">Success</span></h3>" + Environment.NewLine;
                else content += "<h3>Result: <span class=\"result-failed\">Failure</span></h3>" + Environment.NewLine;
                content += "<table>" + Environment.NewLine;
                content += "<thead>" + Environment.NewLine;
                content += "<tr>" + Environment.NewLine;
                content += "<th class=\"table-status\">Status</th>" + Environment.NewLine;
                content += "<th class=\"table-action\">Action</th>" + Environment.NewLine;
                content += "<th class=\"table-comment\">Comment</th>" + Environment.NewLine;
                content += "</tr> " + Environment.NewLine;
                content += "</thead>" + Environment.NewLine;
                content += "</table>" + Environment.NewLine;
                content += "</header>" + Environment.NewLine;
            }
            

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
                            content += $"<td class=\"table-status table-row-empty content-hidden\">{step[0]}</td>" + Environment.NewLine;
                            content += $"<td class=\"table-action table-row-empty content-hidden\">{step[1]}</td>" + Environment.NewLine;
                            if (step[2].Length > 1000) content += $"<td class=\"table-comment table-row-empty content-scroll\">{step[2]}</td>" + Environment.NewLine;
                            else content += $"<td class=\"table-comment table-row-empty content-hidden\">{step[2]}</td>" + Environment.NewLine;
                            content += "</tr>" + Environment.NewLine;
                        }
                        else
                        {
                            if (Config.languageEngReportMail == false)
                            {
                                if (step[0] == Report.PASSED) content += $"<td class=\"table-status table-row status-passed\">Успешно</td>" + Environment.NewLine;
                                if (step[0] == Report.FAILED) content += $"<td class=\"table-status table-row status-failed\">Неудача</td>" + Environment.NewLine;
                                if (step[0] == Report.STOPPED) content += $"<td class=\"table-status table-row status-stopped\">Остановлен</td>" + Environment.NewLine;
                                if (step[0] == Report.PROCESS) content += $"<td class=\"table-status table-row status-process\">В процессе</td>" + Environment.NewLine;
                                if (step[0] == Report.COMPLETED) content += $"<td class=\"table-status table-row status-completed\">Выполнено</td>" + Environment.NewLine;
                                if (step[0] == Report.WARNING) content += $"<td class=\"table-status table-row status-warning\">Предупреждение</td>" + Environment.NewLine;
                                if (step[0] == Report.ERROR) content += $"<td class=\"table-status table-row status-error\">Ошибка</td>" + Environment.NewLine;
                                if (step[0] == Report.SCREENSHOT) content += $"<td class=\"table-status table-row status-screenshot\">Скриншот</td>" + Environment.NewLine;
                            }
                            else
                            {
                                if (step[0] == Report.PASSED) content += $"<td class=\"table-status table-row status-passed\">Passed</td>" + Environment.NewLine;
                                if (step[0] == Report.FAILED) content += $"<td class=\"table-status table-row status-failed\">Failed</td>" + Environment.NewLine;
                                if (step[0] == Report.STOPPED) content += $"<td class=\"table-status table-row status-stopped\">Stopped</td>" + Environment.NewLine;
                                if (step[0] == Report.PROCESS) content += $"<td class=\"table-status table-row status-process\">Process</td>" + Environment.NewLine;
                                if (step[0] == Report.COMPLETED) content += $"<td class=\"table-status table-row status-completed\">Completed</td>" + Environment.NewLine;
                                if (step[0] == Report.WARNING) content += $"<td class=\"table-status table-row status-warning\">Warning</td>" + Environment.NewLine;
                                if (step[0] == Report.ERROR) content += $"<td class=\"table-status table-row status-error\">Error</td>" + Environment.NewLine;
                                if (step[0] == Report.SCREENSHOT) content += $"<td class=\"table-status table-row status-screenshot\">Screenshot</td>" + Environment.NewLine;
                            }
                            content += $"<td class=\"table-action table-row content-hidden\">{step[1]}</td>" + Environment.NewLine;
                            if (step[2].Length > 1000) content += $"<td class=\"table-comment table-row content-scroll\">{step[2]}</td>" + Environment.NewLine;
                            else content += $"<td class=\"table-comment table-row content-hidden\">{step[2]}</td>" + Environment.NewLine;
                            content += "</tr>" + Environment.NewLine;
                        }

                        if (step[0] == Report.FAILED || step[0] == Report.ERROR || step[0] == Report.WARNING) Report.Log += $"{step[0]} | {step[1]} - {step[2]}" + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    Config.browserForm.ConsoleMsg(ex.ToString(), ex.ToString());
                }
                content += "</tbody>" + Environment.NewLine;
                content += "<tfoot>" + Environment.NewLine;
                /*
                if (Config.languageEngReportMail == false) content += "<td class=\"table-status\">Ошибок: </td>" + Environment.NewLine;
                else content += "<td class=\"table-status\">Errors: </td>" + Environment.NewLine;
                content += $"<td class=\"table-action\">{Report.CountErrors}</td>" + Environment.NewLine;
                content += "<td class=\"table-comment\"></td>" + Environment.NewLine;
                */
                content += "<td></td>" + Environment.NewLine;
                content += "<td></td>" + Environment.NewLine;
                content += $"<td class=\"table-footer\">Browser Hat {Config.currentBrowserVersion}</td>" + Environment.NewLine;
                content += "</tfoot>" + Environment.NewLine;
                content += "</table>" + Environment.NewLine;
                content += "</section>" + Environment.NewLine;
            }
            content += "</div>" + Environment.NewLine;
            content += "</body>" + Environment.NewLine;
            return content;
        }

        /* =======================================================================================
         * Страница полного отчета для всех автотестов
         * */

        public static void SaveResultReport()
        {
            try
            {
                if (Report.FolderName == null) Report.FolderName = Config.projectPath + "/reports/";
                if (Report.FolderImagesName == null) Report.FolderImagesName = Config.projectPath + "/reports/screenshots/";

                if (!Directory.Exists(Report.FolderName))
                {
                    Directory.CreateDirectory(Report.FolderName);
                    Directory.CreateDirectory(Report.FolderImagesName);
                    Config.browserForm.ConsoleMsg("Создана папка для отчетов", "A folder for reports has been created");
                    Config.browserForm.updateProjectTree();
                }

                if (Directory.Exists(Report.FolderName))
                {
                    List<List<string>> tests = new List<List<string>>();
                    List<string> test = new List<string>();
                    double successRate = 0;
                    double failureRate = 0;
                    double workRate = 0;
                    double amountTests = 0;
                    double amountSuccessTests = 0;
                    double amountFailureTests = 0;
                    double amountWorkTests = 0;

                    List<string> lines = new List<string>();

                    foreach (string filename in Directory.GetFiles(Report.FolderName))
                    {

                        /* 0    <!--
                         * 1    FAILURE
                         * 2    ExampleTest1.cs
                         * 3    Тест проверяет авторизацию на сайте
                         * 4    21.02.2023 10:32:11
                         * 5    21.02.2023 10:35:45
                         * 6    -->
                         */

                        if (File.Exists(filename) == false) continue;

                        lines = new List<string>();
                        lines = Report.readFileLines(Report.UTF_8_BOM, filename, 7);
                        if (lines.Count > 0)
                        {
                            //Config.browserForm.ConsoleMsg($"{filename} | {lines[0]} | {lines[1]} | {lines[2]} | {lines[3]} | {lines[4]} | {lines[5]} | {lines[6]}");

                            if (lines[0] == "<!--" && lines[6] == "-->")
                            {
                                amountTests++;
                                test = new List<string>();
                                if (lines[1] == Report.SUCCESS)
                                {
                                    test.Add(Report.SUCCESS);   // Статус теста
                                    amountSuccessTests++;
                                }
                                else if (lines[1] == Report.FAILURE)
                                {
                                    test.Add(Report.FAILURE);   // Статус теста
                                    amountFailureTests++;
                                }
                                else if (lines[1] == Report.AT_WORK)
                                {
                                    test.Add(Report.AT_WORK);   // Статус теста
                                    amountWorkTests++;
                                }
                                else
                                {
                                    test.Add("");               // Статус теста
                                    amountWorkTests++;
                                }

                                test.Add(lines[3]); // Описание теста
                                test.Add(lines[4] + "<br>" + lines[5]); // Дата запуска и завершения
                                test.Add(lines[2]); // Файл
                                test.Add(filename); // Отчет
                                tests.Add(test);
                            }
                        }
                    }

                    successRate = (amountSuccessTests / amountTests) * 100;
                    failureRate = (amountFailureTests / amountTests) * 100;
                    workRate = (amountWorkTests / amountTests) * 100;

                    //Config.browserForm.ConsoleMsg($"{amountTests} | {amountSuccessTests} | {amountFailureTests} | {amountWorkTests}");
                    //Config.browserForm.ConsoleMsg($"{successRate} | {failureRate} | {workRate}");

                    Report.writeFile(GetResultHead() + GetResultBody(tests, (int)successRate, (int)failureRate, (int)workRate, (int)amountSuccessTests, (int)amountFailureTests, (int)amountWorkTests, (int)amountTests) + GetResultFooter(), Report.UTF_8_BOM, Report.FolderName + "index.html");
                    if (File.Exists(Report.FolderName + "index.html"))
                    {
                        Config.browserForm.ConsoleMsg("Создан отчет с результатами всех тестов", "A report has been created with the results of all tests");
                        if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg("Создан отчет с результатами всех тестов" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        else Config.browserForm.SystemConsoleMsg("A report has been created with the results of all tests" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    }
                    else
                    {
                        Config.browserForm.ConsoleMsg($"Не удалось создать отчет с результатами всех тестов по адресу {Report.FolderName}/index.html", 
                            $"It was not possible to create a report with the results of all tests at {Report.FolderName}/index.html");
                        if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Не удалось создать отчет с результатами всех тестов по адресу {Report.FolderName}/index.html" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        else Config.browserForm.SystemConsoleMsg($"It was not possible to create a report with the results of all tests at folder {Report.FolderName}/index.html" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    }

                    Config.browserForm.updateProjectTree();
                }
                else
                {
                    Config.browserForm.ConsoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}", 
                        $"Failed to create a folder for reports at {Report.FolderName}");
                    if (Config.languageEngConsole == false) Config.browserForm.SystemConsoleMsg($"Не удалось создать папку для отчетов по адресу {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else Config.browserForm.SystemConsoleMsg($"Failed to create a folder for reports at {Report.FolderName}" + Environment.NewLine, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }
            }
            catch (Exception ex)
            {
                Report.ShowErrorInSystemConsole(ex.ToString());
            }
        }


        public static string GetResultHead()
        {
            string content = "";
            if (Config.languageEngReportMail == false)
            {
                content += "<!DOCTYPE html>" + Environment.NewLine;
                content += "<html lang=\"ru-RU\">" + Environment.NewLine;
                content += "<head>" + Environment.NewLine;
                content += "<title>Полный список результатов всех тестов</title>" + Environment.NewLine;
            }
            else
            {
                content += "<!DOCTYPE html>" + Environment.NewLine;
                content += "<html lang=\"en-EN\">" + Environment.NewLine;
                content += "<head>" + Environment.NewLine;
                content += "<title>Full list of all test results</title>" + Environment.NewLine;
            }
            content +=
@"<meta charset=""UTF-8"" />
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
<meta http-equiv=""X-UA-Compatible"" content=""ie=edge"" />
<style type=""text/css"">
html { margin: 0; padding: 0; border: 0;}
body { background-color: #F9F7FF; font-family: ""Source Sans Pro"", Helvetica, sans-serif; font-size: 10pt; }
.wrapper { margin-left: auto; margin-right: auto; position: relative; max-width: 1440px; }
header { display: block; position: fixed; top: 0px; background-color: #F9F7FF; border-bottom: 1px solid #eaeff2; min-width: 1400px; max-width: 1400px; z-index: 1000; }
#Hat { text-align: right;  margin-right: 80px; margin-top: 10px; float: right;}
#HatImage { display: block; margin: 0 auto; }
#HatTitle { font-weight: bold; font-family: ""Arial"", sens-serif; font-size: 25px; margin-right: -40px; }
#Diagram { float: left; box-shadow: 0 0 5px rgba(0,0,0,0.3); border: 1px solid #CCCCCC; padding: 10px; margin-bottom:10px; margin-left: 5px; min-width: 45%; min-height: 150px; background-color: #FFFFFF; }
#DiagramCanvas {min-width: 300px; min-height: 150px;}
#DiagramDescription { float: right; min-width: 300px; }
#Description { float: right; box-shadow: 0 0 5px rgba(0,0,0,0.3); border: 1px solid #CCCCCC; padding: 10px; min-width: 50%; min-height: 150px; margin-right: 5px; background-color: #FFFFFF;}
section { display: block; position: relative; min-width: 1400px; max-width: 1400px; }
table { margin: 0px; min-width: 1400px; max-width: 1400px; }
thead { background-color: #4d545d; color: #FFF; }
tr:hover .content-hidden{  background: #EFF7FF; }
.table { position: relative; top: 270px; z-index: 1; overflow: hidden; }
.table-status { padding: 10px; min-width: 60px; max-width: 60px; }
.table-description { padding: 10px; min-width: 350px; max-width: 350px; }
.table-date { padding: 10px; min-width: 100px; max-width: 100px; }
.table-file { padding: 10px; min-width: 100px; max-width: 100px; overflow: hidden; }
.table-report { padding: 10px; min-width: 100px; max-width: 100px; overflow: hidden; }
.table-row { background-color: #FFF; border-bottom: 1px solid #eaeff2; }
.table-row-empty { background-color: #F9F7FF;  }
.status-passed { background-color: #98C900; color: #FFFFFF; }
.status-failed { background-color: #E94B31; color: #FFFFFF; }
.status-process { background-color: #858585; color: #FFFFFF; }
.status-error { background-color: #F4CCCC; color: #FF0000; }
.table-footer { padding-top: 50px; padding-right: 10px; padding-bottom: 20px; float: right; }
img { min-width: 700px; max-width: 700px; }
.result-passed { color: #007F0E; }
.result-failed { color: #7F0000; }
.content-hidden { overflow: hidden; }
.content-scroll { overflow: scroll; }
</style>
</head>";
            return content;
        }

        public static string GetResultFooter()
        {
            string content = "</html>";
            return content;
        }

        public static string GetResultBody(List<List<string>> tests, int successRate, int failureRate, int workRate, int amountSuccessTests, int amountFailureTests, int amountWorkTests, int amountTests)
        {
            string content = Environment.NewLine + "<body>" + Environment.NewLine;
            content += "<div class=\"wrapper\">" + Environment.NewLine;
            content += "<header>" + Environment.NewLine;
            if (Config.languageEngReportMail == false)
            {
                content += "<h2>Полный список результатов всех тестов</h2>" + Environment.NewLine;
                content += "<div id=\"Diagram\">" + Environment.NewLine;
                content += "<canvas id=\"DiagramCanvas\"></canvas>" + Environment.NewLine;
                content += "<div id=\"DiagramDescription\">" + Environment.NewLine;
                content += "<p>График результатов всех тестов в процентах:</p><br>" + Environment.NewLine;
                content += $"<p>Успех: {successRate.ToString()}%</p>" + Environment.NewLine;
                content += $"<p>Неудача: {failureRate.ToString()}%</p>" + Environment.NewLine;
                content += $"<p>В работе: {workRate.ToString()}%</p>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;
                content += "<div id=\"Description\">" + Environment.NewLine;
                content +=
@"<div id=""Hat"">
<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""64px"" height=""64px"" viewBox=""0 0 64 64"" enable-background=""new 0 0 64 64"" xml:space=""preserve""><image id=""HatImage"" width=""64"" height=""64"" x=""0"" y=""0""
href=""data:image/png;base64,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"" />
</svg>
<div id=""HatTitle"">Browser Hat</div>
</div>" + Environment.NewLine;
                content += "<div>" + Environment.NewLine;
                content += $"<p><b>Всего тестов: </b>{amountTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Успешных тестов: </b>{amountSuccessTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Неудачных тестов: </b>{amountFailureTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Тесты в работе: </b>{amountWorkTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Дата отчета: </b>{DateTime.Now.ToString()}</p>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;

                content += "<table>" + Environment.NewLine;
                content += "<thead>" + Environment.NewLine;
                content += "<tr>" + Environment.NewLine;
                content += "<th class=\"table-status\">Статус теста</th>" + Environment.NewLine;
                content += "<th class=\"table-description\">Описание теста</th>" + Environment.NewLine;
                content += "<th class=\"table-date\">Дата</th>" + Environment.NewLine;
                content += "<th class=\"table-file\">Файл</th>" + Environment.NewLine;
                content += "<th class=\"table-report\">Отчет</th>" + Environment.NewLine;
                content += "</tr> " + Environment.NewLine;
                content += "</thead>" + Environment.NewLine;
                content += "</table>" + Environment.NewLine;

                content += "</header>" + Environment.NewLine;
            }
            else
            {
                content += "<h2>Full list of all test results</h2>" + Environment.NewLine;
                content += "<div id=\"Diagram\">" + Environment.NewLine;
                content += "<canvas id=\"DiagramCanvas\"></canvas>" + Environment.NewLine;
                content += "<div id=\"DiagramDescription\">" + Environment.NewLine;
                content += "<p>Chart of the results of all tests as a percentage:</p><br>" + Environment.NewLine;
                content += $"<p>Success: {successRate.ToString()}%</p>" + Environment.NewLine;
                content += $"<p>Failure: {failureRate.ToString()}%</p>" + Environment.NewLine;
                content += $"<p>At work: {workRate.ToString()}%</p>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;
                content += "<div id=\"Description\">" + Environment.NewLine;
                content +=
@"<div id=""Hat"">
<svg version=""1.1"" id=""Layer_1"" xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"" x=""0px"" y=""0px"" width=""64px"" height=""64px"" viewBox=""0 0 64 64"" enable-background=""new 0 0 64 64"" xml:space=""preserve""><image id=""HatImage"" width=""64"" height=""64"" x=""0"" y=""0""
href=""data:image/png;base64,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"" />
</svg>
<div id=""HatTitle"">Browser Hat</div>
</div>" + Environment.NewLine;
                content += "<div>" + Environment.NewLine;
                content += $"<p><b>Total tests: </b>{(amountSuccessTests + amountFailureTests + amountWorkTests).ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Successful tests: </b>{amountSuccessTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Failed tests: </b>{amountFailureTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Tests in progress: </b>{amountWorkTests.ToString()}</p>" + Environment.NewLine;
                content += $"<p><b>Report date: </b>{DateTime.Now.ToString()}</p>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;
                content += "</div>" + Environment.NewLine;

                content += "<table>" + Environment.NewLine;
                content += "<thead>" + Environment.NewLine;
                content += "<tr>" + Environment.NewLine;
                content += "<th class=\"table-status\">Test status</th>" + Environment.NewLine;
                content += "<th class=\"table-description\">Test Description</th>" + Environment.NewLine;
                content += "<th class=\"table-date\">Completion date</th>" + Environment.NewLine;
                content += "<th class=\"table-file\">File</th>" + Environment.NewLine;
                content += "<th class=\"table-report\">Report</th>" + Environment.NewLine;
                content += "</tr> " + Environment.NewLine;
                content += "</thead>" + Environment.NewLine;
                content += "</table>" + Environment.NewLine;

                content += "</header>" + Environment.NewLine;
            }

            content += "<section>" + Environment.NewLine;
            content += "<table class=\"table\">" + Environment.NewLine;
            content += "<tbody>" + Environment.NewLine;

            try
            {
                if (tests.Count > 0)
                {
                    foreach (List<string> test in tests)
                    {
                        content += "<tr>" + Environment.NewLine;
                        if (Config.languageEngReportMail == false)
                        {
                            if (test[0] == Report.SUCCESS) content += $"<td class=\"table-status table-row status-passed\">Успех</td>" + Environment.NewLine;
                            if (test[0] == Report.FAILURE) content += $"<td class=\"table-status table-row status-failed\">Неудача</td>" + Environment.NewLine;
                            if (test[0] == Report.AT_WORK) content += $"<td class=\"table-status table-row status-process\">В работе</td>" + Environment.NewLine;
                        }
                        else
                        {
                            if (test[0] == Report.SUCCESS) content += $"<td class=\"table-status table-row status-passed\">Success</td>" + Environment.NewLine;
                            if (test[0] == Report.FAILURE) content += $"<td class=\"table-status table-row status-failed\">Failure</td>" + Environment.NewLine;
                            if (test[0] == Report.AT_WORK) content += $"<td class=\"table-status table-row status-process\">At work</td>" + Environment.NewLine;
                        }
                        content += $"<td class=\"table-description table-row content-hidden\">{test[1]}</td>" + Environment.NewLine;
                        content += $"<td class=\"table-date table-row content-hidden\">{test[2]}</td>" + Environment.NewLine;
                        content += $"<td class=\"table-file table-row content-hidden\">{test[3]}</td>" + Environment.NewLine;
                        content += $"<td class=\"table-report table-row content-hidden\"><a href=\"{test[4]}\">{Config.browserForm.getFolderName2(test[4])}</a></td>" + Environment.NewLine;
                        content += "</tr>" + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg(ex.ToString(), ex.ToString());
            }

            content += "</tbody>" + Environment.NewLine;
            content += "<tfoot>" + Environment.NewLine;
            content += "<td></td>" + Environment.NewLine;
            content += "<td></td>" + Environment.NewLine;
            content += "<td></td>" + Environment.NewLine;
            content += "<td></td>" + Environment.NewLine;
            content += $"<td class=\"table-footer\">Browser Hat {Config.currentBrowserVersion}</td>" + Environment.NewLine;
            content += "</tfoot>" + Environment.NewLine;
            content += "</table>" + Environment.NewLine;
            content += "</section>" + Environment.NewLine;
            content += "</div>" + Environment.NewLine;

            content += "<script type=\"text/javascript\">" + Environment.NewLine;
            content += "(function(){" + Environment.NewLine;
            content += "var canvas = document.getElementById('DiagramCanvas');" + Environment.NewLine;
            content += "var ctx = canvas.getContext('2d');" + Environment.NewLine;
            content += "ctx.fillStyle = \"black\";" + Environment.NewLine;
            content += "ctx.lineWidth = 2.0;" + Environment.NewLine;
            content += "ctx.beginPath();" + Environment.NewLine;
            content += "ctx.moveTo(35, 10);" + Environment.NewLine;
            content += "ctx.lineTo(35, 135);" + Environment.NewLine;
            content += "ctx.lineTo(250, 135);" + Environment.NewLine;
            content += "ctx.stroke();" + Environment.NewLine;
            
            content += "ctx.fillStyle = \"black\";" + Environment.NewLine;
            content += "for(let i = 0; i < 6; i++) {" + Environment.NewLine;
            content += "ctx.fillText((5 - i) * 20 + \"%\", 0, i * 25 + 10);" + Environment.NewLine;
            content += "ctx.beginPath();" + Environment.NewLine;
            content += "ctx.moveTo(30, i * 25 + 10);" + Environment.NewLine;
            content += "ctx.lineTo(35, i * 25 + 10);" + Environment.NewLine;
            content += "ctx.stroke(); " + Environment.NewLine;
            content += "}" + Environment.NewLine;
            
            if (Config.languageEngReportMail == false)
            {
                content += "let labels = [\"Успех\", \"Неудача\", \"В работе\"];" + Environment.NewLine;
                content += "for(var i=0; i<3; i++) { " + Environment.NewLine;
                content += "ctx.fillText(labels[i], 50 + (i * 70), 148);" + Environment.NewLine;
                content += "}" + Environment.NewLine;
            }
            else
            {
                content += "let labels = [\"Passed\", \"Failed\", \"At work\"];" + Environment.NewLine;
                content += "for(var i=0; i<3; i++) { " + Environment.NewLine;
                content += "ctx.fillText(labels[i], 50 + (i * 70), 148);" + Environment.NewLine;
                content += "}" + Environment.NewLine;
            }


            content += $"let dataValue = [ {successRate}, {failureRate}, {workRate} ];" + Environment.NewLine;
            content += "let dataColor = [ \"green\", \"red\", \"gray\" ];" + Environment.NewLine;
            content += "for(var i=0; i<dataValue.length; i++) {" + Environment.NewLine;
            content += "ctx.fillStyle = dataColor[i];" + Environment.NewLine;
            content += "var value = dataValue[i];" + Environment.NewLine;
            content += "ctx.fillRect(40 + (i * 75), 134 - (125 / 100 * value), 45, (125 / 100 * value));" + Environment.NewLine;
            content += "}" + Environment.NewLine;

            content += "}());" + Environment.NewLine;
            content += "</script>" + Environment.NewLine;

            content += "</body>" + Environment.NewLine;
            return content;
        }

        public static string readFile(string encoding, string filename)
        {
            string content = "";
            try
            {
                StreamReader reader;
                if (encoding == DEFAULT)
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                content = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg("Чтение файла: " + filename + " - неудалось прочитать из за ошибки: " + ex.Message,
                    "Reading a file: " + filename + " - could not be read due to an error: " + ex.Message);
                AddStep(WARNING, "Чтение файла: " + filename, "Неудалось прочитать файл из за ошибки: " + ex.Message);
            }
            return content;
        }

        public static List<string> readFileLines(string encoding, string filename, int count)
        {
            List<string> lines = new List<string>();
            try
            {
                StreamReader reader;
                if (encoding == DEFAULT)
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }

                for (int i = 0; i < count; i++)
                {
                    lines.Add(reader.ReadLine());
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg("Чтение файла: " + filename + " - неудалось прочитать из за ошибки: " + ex.Message,
                    "Reading a file: " + filename + " - could not be read due to an error: " + ex.Message);
                AddStep(WARNING, "Чтение файла: " + filename, "Неудалось прочитать файл из за ошибки: " + ex.Message);
            }
            return lines;
        }


        public static void writeFile(string content, string encoding, string filename)
        {
            try
            {
                StreamWriter writer;
                if (encoding == DEFAULT)
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                else if (encoding == UTF_8)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(false));
                }
                else if (encoding == UTF_8_BOM)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                }
                else if (encoding == WINDOWS_1251)
                {
                    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                writer.Write(content);
                writer.Close();
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg("Сохранение файла: " + filename + " - неудалось сохранить из за ошибки: " + ex.Message,
                    "Saving a file: " + filename + " - failed to save due to an error: " + ex.Message);
                AddStep(WARNING, "Сохранение файла: " + filename, "Неудалось сохранить файл из за ошибки: " + ex.Message);
            }
        }

        

    }
}

