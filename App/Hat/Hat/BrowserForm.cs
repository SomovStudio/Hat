using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.Core;
using HatFramework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;

namespace Hat
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();

            StartPage.createStartPage();
            toolStripComboBoxUrl.Text = "file:///" + StartPage.fileStartPage;

            CheckForIllegalCrossThreadCalls = false;
            Config.encoding = WorkOnFiles.UTF_8_BOM;
            toolStripStatusLabelFileEncoding.Text = Config.encoding;
            Config.browserForm = this;
            ConsoleMsg($"Браузер Hat версия {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})");
            SystemConsoleMsg("", default, default, default, true);
            if(Config.languageEngConsole == false) SystemConsoleMsg($"Браузер Hat версия {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
            else SystemConsoleMsg($"Browser Hat version {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);

            if (Config.statucCacheClear == "true")
            {
                if (Config.languageEngConsole == false) SystemConsoleMsg("Кэш браузера очишен", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else SystemConsoleMsg("The browser cache has been cleared", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                ConsoleMsg("Кэш браузера очишен");
            }
            else if (Config.statucCacheClear == "false")
            {
                if (Config.languageEngConsole == false) SystemConsoleMsg("Кэш браузера неудалось очистить, удалите папку " + Config.cacheFolder + " вручную", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else SystemConsoleMsg("Browser cache could not be cleared. Delete the folder " + Config.cacheFolder, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                ConsoleMsg("Кэш браузера неудалось очистить, удалите папку " + Config.cacheFolder + " вручную");
            }
            else
            {
                if (Config.languageEngConsole == false) SystemConsoleMsg("Кэш не очищен, произошла ошибка: " + Config.statucCacheClear, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else SystemConsoleMsg("The cache has not been cleared, an error has occurred: " + Config.statucCacheClear, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                ConsoleMsg("Кэш не очищен, произошла ошибка: " + Config.statucCacheClear);
            }
        }
              

        private bool stopTest = false;
        private bool testSuccess = true;
        private StepTestForm stepTestForm;
        private CodeEditorForm codeEditorForm;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                initWebView();

                this.Width = 1440;
                this.Height = 900;
                numericUpDownBrowserWidth.Value = panel1.Width;
                numericUpDownBrowserHeight.Value = panel1.Height;
                if (Config.openHtmlFile != null) toolStripComboBoxUrl.Text = Config.openHtmlFile;
                webView2.Source = new Uri(toolStripComboBoxUrl.Text);
                toolStripStatusLabelProjectPath.Text = Config.projectPath;

                if (Config.commandLineMode == true)
                {
                    if (Config.languageEngConsole == false) SystemConsoleMsg("Запуск браузера...", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else SystemConsoleMsg("Launching the browser...", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    ConsoleMsg("Запуск браузера Hat из командной строки");
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;
                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);
                    // Отключение интерфейса
                    hiddenInterface();
                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                    if (Config.languageEngConsole == false) SystemConsoleMsg($"Проект открыт (версия проекта: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else SystemConsoleMsg($"The project is open (project version: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    ConsoleMsg($"Проект открыт (версия проекта: {Config.version})");
                    toolStripStatusLabelProjectFolderFile.Text = Config.selectName;
                    PlayTest(Config.selectName);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void BrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            if (codeEditorForm != null)
            {
                MessageBox.Show("Редактор кода по прежнему открыт, возможно есть файлы которые не были сохранены. Закройте похалуйста редактор кода.");
                return;
            }

            if (testSuccess == false)
            {
                SystemConsoleMsg(Environment.NewLine + "==============================", default, default, default, true);
                if (Config.languageEngConsole == false) SystemConsoleMsg("Тестирование завершено ПРОВАЛЬНО", default, ConsoleColor.DarkRed, ConsoleColor.White, true);
                else SystemConsoleMsg("Tests ended. Finished: FAILURE", default, ConsoleColor.DarkRed, ConsoleColor.White, true);
                Environment.Exit(1);
            }
            else
            {
                SystemConsoleMsg(Environment.NewLine + "==============================", default, default, default, true);
                if (Config.languageEngConsole == false) SystemConsoleMsg("Тестирование завершено УСПЕШНО", default, ConsoleColor.DarkGreen, ConsoleColor.White, true);
                else SystemConsoleMsg("Tests ended. Finished: SUCCESS", default, ConsoleColor.DarkGreen, ConsoleColor.White, true);
                Environment.Exit(0);
            }
        }

        /* Инициализация WevView */
        private void initWebView()
        {
            webView2.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            try
            {
                ConsoleMsg("Начало инициализации WebView");
                webView2.EnsureCoreWebView2Async();

                /* Microsoft Edge DevTools Protocol: https://learn.microsoft.com/en-us/microsoft-edge/devtools-protocol-chromium/
                 * Chrome DevTools Protocol: https://chromedevtools.github.io/devtools-protocol/tot/ 
                 */
                webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Log.entryAdded").DevToolsProtocolEventReceived += errorEvents;
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Log.enable", "{}");
                webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Console.messageAdded").DevToolsProtocolEventReceived += errorEvents;
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Console.enable", "{}");
                webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Runtime.consoleAPICalled").DevToolsProtocolEventReceived += errorEvents;
                webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Runtime.exceptionThrown").DevToolsProtocolEventReceived += errorEvents;
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Runtime.enable", "{}");
                //webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Network.loadingFailed").DevToolsProtocolEventReceived += errorEvents;
                //webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.enable", "{}");
                ConsoleMsg("Запущен монитор ошибок на страницах");

                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.enable", "{}");
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.setCacheDisabled", @"{""cacheDisabled"":true}");
                ConsoleMsg("Выполнена очистка кэша WebView");

                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Security.setIgnoreCertificateErrors", "{\"ignore\": true}");
                ConsoleMsg("Включено игнорирование сертификата Security.setIgnoreCertificateErrors (true)");

                if (Config.defaultUserAgent == "") Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
                ConsoleMsg($"Опция User-Agent по умолчанию {Config.defaultUserAgent}");

                webView2.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
                ConsoleMsg("Выполнена настройка WebView (отключаны alert, prompt, confirm)");
                ConsoleMsg("Инициализация WebView - завершена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }

        }

        private void webView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            webView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        private void CoreWebView2_NewWindowRequested(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NewWindowRequestedEventArgs e)
        {
            try
            {
                e.Handled = true;
                toolStripComboBoxUrl.Text = e.Uri.ToString();
                if (toolStripComboBoxUrl.Text.Contains("https://") == false && toolStripComboBoxUrl.Text.Contains("http://") == false)
                {
                    toolStripComboBoxUrl.Text = "https://" + toolStripComboBoxUrl.Text;
                }
                webView2.CoreWebView2.Navigate(toolStripComboBoxUrl.Text);
                updateToolStripComboBoxUrl();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Игнорирование сертификата */
        private async void ignorCertificateErrors()
        {
            try
            {
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Security.setIgnoreCertificateErrors", "{\"ignore\": true}");
                ConsoleMsg("Опция Security.setIgnoreCertificateErrors - включен параметр ignore: true");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Очистка кэша */
        private async void clearBrowserCache()
        {
            try
            {
                await webView2.EnsureCoreWebView2Async();
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.setCacheDisabled", @"{""cacheDisabled"":true}");
                ConsoleMsg("Выполнена очистка кэша");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void errorEvents(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DevToolsProtocolEventReceivedEventArgs e)
        {
            // Сообщения бывают: {"message":{}} | {"args":[]} | {"exceptionDetails":{}} | {"entry":{}} |
            // Определять: "level":"error" | "type":"error" | "subtype":"error" | "className":"TypeError" | "className":"ReferenceError" | "className":"SyntaxError"
            // Пропускать: "level":"log" | "type":"log" | "subtype":"log"
            // Пропускать: "level":"info" | "type":"info" | "subtype":"info"
            // Пропускать: "level":"warning" | "type":"warning" | "subtype":"warning"
            // Пропускать: "level":"verbose" | "type":"verbose" | "subtype":"verbose"
            if (e != null && e.ParameterObjectAsJson != null)
            {
                if (e.ParameterObjectAsJson.Contains("\"level\":\"log\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"type\":\"log\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"subtype\":\"log\"") == true) return;

                if (e.ParameterObjectAsJson.Contains("\"level\":\"info\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"type\":\"info\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"subtype\":\"info\"") == true) return;

                if (e.ParameterObjectAsJson.Contains("\"level\":\"warning\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"type\":\"warning\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"subtype\":\"warning\"") == true) return;

                if (e.ParameterObjectAsJson.Contains("\"level\":\"verbose\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"type\":\"verbose\"") == true) return;
                if (e.ParameterObjectAsJson.Contains("\"subtype\":\"verbose\"") == true) return;

                richTextBoxErrors.AppendText(e.ParameterObjectAsJson + Environment.NewLine);
                richTextBoxErrors.ScrollToCaret();
            }
        }

        private void showMessageConsoleErrors(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DevToolsProtocolEventReceivedEventArgs e)
        {
            /*
             * Chrome DevTools Protocol | Log Domain
             * https://chromedevtools.github.io/devtools-protocol/tot/Log/
             * Log.entryAdded, Log.LogEntry, Log.ViolationSetting, Log.clear, Log.disable, Log.enable, Log.startViolationsReport, Log.stopViolationsReport
             * https://chromedevtools.github.io/devtools-protocol/tot/Runtime/
             * https://chromedevtools.github.io/devtools-protocol/tot/Console/
             */

            if (e != null && e.ParameterObjectAsJson != null)
            {
                // verbose, info, warning, error
                if (e.ParameterObjectAsJson.Contains("\"level\":\"error\"") == true) richTextBoxErrors.AppendText(e.ParameterObjectAsJson + Environment.NewLine);
                else if (e.ParameterObjectAsJson.Contains("\"level\":\"warning\"") == true) richTextBoxErrors.AppendText(e.ParameterObjectAsJson + Environment.NewLine);
                richTextBoxErrors.ScrollToCaret();
            }
        }

        private void webView2_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            richTextBoxErrors.Text = "";
        }

        /* |-----------------------------------------------------------------------------------------------------------|
         * |== ПУБЛИЧНЫЕ МЕТОДЫ ДЛЯ ФРЕЙМВОРКА ========================================================================|
         * |-----------------------------------------------------------------------------------------------------------|
         */
        public Microsoft.Web.WebView2.WinForms.WebView2 GetWebView()
        {
            return webView2; // Возвращает браузер
        }

        public void ConsoleMsg(string message) // вывод сообщения в консоль приложения
        {
            try
            {
                richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] " + message + Environment.NewLine);
                richTextBoxConsole.ScrollToCaret();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void ConsoleMsgError(string message) // вывод сообщения об ошибке
        {
            try
            {
                richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] ОШИБКА: " + message + Environment.NewLine);
                richTextBoxConsole.ScrollToCaret();
                SystemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
                if (Config.languageEngConsole == false) SystemConsoleMsg("Произошла ошибка:", default, ConsoleColor.Black, ConsoleColor.Red, true);
                else SystemConsoleMsg("An error has occurred:", default, ConsoleColor.Black, ConsoleColor.Red, true);
                SystemConsoleMsg(message, default, default, default, true);
                SystemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
                SystemConsoleMsg("", default, default, default, true);
                ResultAutotest(false);
                if (Config.commandLineMode == true) Close();
            }
            catch (Exception ex)
            {

            }
        }

        public void ConsoleMsgErrorReport(string message) // вывод сообщения об ошибке в консоль приложения
        {
            Report.AddStep(Report.ERROR, "", message);
            Report.SaveReport(testSuccess);

            richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] ОШИБКА: " + message + Environment.NewLine);
            richTextBoxConsole.ScrollToCaret();

            SystemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
            if (Config.languageEngConsole == false) SystemConsoleMsg("Произошла ошибка:", default, ConsoleColor.Black, ConsoleColor.Red, true);
            else SystemConsoleMsg("An error has occurred:", default, ConsoleColor.Black, ConsoleColor.Red, true);
            SystemConsoleMsg(message, default, default, default, true);
            SystemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
            SystemConsoleMsg("", default, default, default, true);
            ResultAutotest(false);
            if (Config.commandLineMode == true) Close();
        }

        public void SystemConsoleMsg(string message, Encoding encoding, ConsoleColor backgroundColor, ConsoleColor foregroundColor, bool newLine) // вывод сообщения в системную консоль
        {
            // Console.BackgroundColor - Возвращает или задает цвет фона консоли. (Значение из перечисления, задающее фоновый цвет консоли, то есть цвет, на фоне которого выводятся символы. Значением по умолчанию является Black.)
            // Console.ForegroundColor - Возвращает или задает цвет фона консоли. (Значение из перечисления ConsoleColor, задающее цвет переднего плана консоли, то есть цвет, которым выводятся символы. По умолчанию задано значение Gray.)

            if (encoding == default)
            {
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                System.Console.InputEncoding = System.Text.Encoding.UTF8;
            }
            else
            {
                System.Console.OutputEncoding = encoding;
                System.Console.InputEncoding = encoding;
            }

            if (backgroundColor == default) System.Console.BackgroundColor = ConsoleColor.Black;
            else System.Console.BackgroundColor = backgroundColor;
            if (foregroundColor == default) System.Console.ForegroundColor = ConsoleColor.Gray;
            else System.Console.ForegroundColor = foregroundColor;

            if (backgroundColor == default && foregroundColor == default) System.Console.ResetColor();

            if (newLine == true) System.Console.WriteLine(message);
            else System.Console.Write(message);

            System.Console.ResetColor();
        }

        public void CleadMessageStep() // очистка всех шагов в таблице "тест"
        {
            try
            {
                listViewTest.Items.Clear();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public int SendMessageStep(string action, string status, string comment, int image, bool debug) // отправляет сообщение в таблицу "тест"
        {
            if (debug == true)
            {
                if (Config.fullReport == true || status == Report.ERROR || status == Report.FAILED 
                    || action == "Testing has started" || action == "Testing completed"
                    || action == "Тестирование началось" || action == "Тестирование завершено")
                {
                    Report.AddStep(status, action, comment);
                }
            }
            else
            {
                Report.AddStep(status, action, comment);
            }

            ListViewItem item;
            ListViewItem.ListViewSubItem subitem;
            item = new ListViewItem();
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = action;
            item.SubItems.Add(subitem);
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = status;
            item.SubItems.Add(subitem);
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = comment;
            item.SubItems.Add(subitem);
            item.ImageIndex = image;
            listViewTest.Items.Add(item);
            int index = listViewTest.Items.Count - 1;
            listViewTest.Items[index].Selected = true;
            listViewTest.Items[index].EnsureVisible();
            return index;
        }

        public void EditMessageStep(int index, string action, string status, string comment, int image, bool debug) // изменить уже отправленное сообщение в таблице "тест"
        {
            try
            {
                if (image != null) listViewTest.Items[index].ImageIndex = image;
                if (action != null) listViewTest.Items[index].SubItems[1].Text = action;
                if (status != null) listViewTest.Items[index].SubItems[2].Text = status;
                if (comment != null) listViewTest.Items[index].SubItems[3].Text = comment;

                if (debug == true)
                {
                    if (Config.fullReport == true || listViewTest.Items[index].SubItems[2].Text == Report.ERROR || listViewTest.Items[index].SubItems[2].Text == Report.FAILED 
                        || listViewTest.Items[index].SubItems[1].Text == "Testing has started" || listViewTest.Items[index].SubItems[1].Text == "Testing completed" 
                        || listViewTest.Items[index].SubItems[1].Text == "Тестирование началось" || listViewTest.Items[index].SubItems[1].Text == "Тестирование завершено")
                    {
                        Report.AddStep(listViewTest.Items[index].SubItems[2].Text, listViewTest.Items[index].SubItems[1].Text, listViewTest.Items[index].SubItems[3].Text);
                    }
                }
                else
                {
                    Report.AddStep(listViewTest.Items[index].SubItems[2].Text, listViewTest.Items[index].SubItems[1].Text, listViewTest.Items[index].SubItems[3].Text);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void BrowserResize(int width, int height) // изменить размер браузера
        {
            try
            {
                if (width > 0 && height > 0)
                {
                    radioButton2.Checked = true;
                    numericUpDownBrowserWidth.Value = width;
                    numericUpDownBrowserHeight.Value = height;
                }
                else
                {
                    radioButton1.Checked = true;
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void UserAgent(string value) // Настройка UserAgent
        {
            if (Config.defaultUserAgent == "" && webView2.CoreWebView2.Settings.UserAgent != null)
            {
                Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
            }

            textBoxUserAgent.Text = value;
            webView2.CoreWebView2.Settings.UserAgent = textBoxUserAgent.Text;
            checkBoxUserAgent.Checked = false;
            textBoxUserAgent.ReadOnly = false;
        }

        public List<string> GetBowserErrors() // Возвращает данные из Console браузера (предупреждения и ошибки)
        {
            List<string> list = new List<string>();
            foreach (string line in richTextBoxErrors.Lines)
            {
                list.Add(line);
            }
            return list;
        }

        public bool CheckStopTest() 
        {
            return stopTest; // Возврат статус автотеста (остановка)
        }

        public void ResultAutotest(bool success) // Результат выполнения автотеста
        {
            if (testSuccess == false) return;   // автотест был ранее провален
            testSuccess = success; // true - автотест был выполнен успешно | false - автотест был провелен
        }

        public bool GetStatusDebugJavaScript()
        {
            return Config.debugJavaScript; // возвращает статус отладки
        }

        public string GetNameAutotest()
        {
            return Config.selectName; // Возвращает имя автотеста
        }

        public bool GetlanguageEngConsole()
        {
            return Config.languageEngConsole; // Возвращает выбранный язык вывода в командной строке
        }

        public bool GetlanguageEngReportMail()
        {
            return Config.languageEngReportMail; // Возвращает выбранный язык вывода в отчет и письмо
        }

        public void SaveReport() // Сохранить отчет и скриншот
        {
            Report.SaveReport(testSuccess);
        }

        public async Task SaveReportScreenshotAsync() // Сохраняет скриншот текущего состояния браузера
        {
            await Report.SaveReportScreenshotAsync();
        }

        public void SendMailFailure() // Отправить письмо с отчетом о провале FAILURE
        {
            try
            {
                if (Report.TestSuccess == false)
                {
                    string content = "";

                    if (Config.languageEngReportMail == false)
                    {
                        content = "<h2>Отчет о работе автотеста</h2>";
                        content += $"<p>Описание: {Report.Description}</p>";
                        content += $"<p>Файл: {Report.TestFileName}</p>";
                        content += $"<p>Результат: <b style=\"color:#7F0000\">Провально</b></p>";
                        content += "<br>";
                        content += "<table border=\"1\">";
                        content += "<tr>";
                        content += "<th><b>Статус</b></th>";
                        content += "<th><b>Действие</b></th>";
                        content += "<th><b>Комментарий</b></th>";
                        content += "</tr>";
                    }
                    else
                    {
                        content = "<h2>Autotest Report</h2>";
                        content += $"<p>Description: {Report.Description}</p>";
                        content += $"<p>File: {Report.TestFileName}</p>";
                        content += $"<p>Result: <b style=\"color:#7F0000\">Failure</b></p>";
                        content += "<br>";
                        content += "<table border=\"1\">";
                        content += "<tr>";
                        content += "<th><b>Status</b></th>";
                        content += "<th><b>Action</b></th>";
                        content += "<th><b>Comment</b></th>";
                        content += "</tr>";
                    }

                    if (Report.Steps.Count > 0)
                    {
                        foreach (string[] step in Report.Steps)
                        {
                            content += "<tr>";
                            if (Config.languageEngReportMail == false)
                            {
                                if (step[0] == Report.FAILED) content += $"<td style=\"color:#FF0000\">Провально</td>";
                                else if (step[0] == Report.PASSED) content += $"<td style=\"color:#34AF00\">Успешно</td>";
                                else if (step[0] == Report.ERROR) content += $"<td style=\"color:#FF0000\">ОШИБКА</td>";
                                else if (step[0] == Report.STOPPED) content += $"<td style=\"color:#000000\">Остановлен</td>";
                                else if (step[0] == Report.PROCESS) content += $"<td style=\"color:#000000\">В процессе</td>";
                                else if (step[0] == Report.COMPLETED) content += $"<td style=\"color:#000000\">Выполнено</td>";
                                else if (step[0] == Report.WARNING) content += $"<td style=\"color:#CCAA00\">Предупреждение</td>";
                                else if (step[0] == Report.SCREENSHOT) content += $"<td style=\"color:#000000\">Скриншот</td>";
                                else content += $"<td>{step[0]}</td>";
                            }
                            else
                            {
                                if (step[0] == Report.FAILED) content += $"<td style=\"color:#FF0000\">Failed</td>";
                                else if (step[0] == Report.PASSED) content += $"<td style=\"color:#34AF00\">Passed</td>";
                                else if (step[0] == Report.ERROR) content += $"<td style=\"color:#FF0000\">Error</td>";
                                else if (step[0] == Report.STOPPED) content += $"<td style=\"color:#000000\">Stopped</td>";
                                else if (step[0] == Report.PROCESS) content += $"<td style=\"color:#000000\">Process</td>";
                                else if (step[0] == Report.COMPLETED) content += $"<td style=\"color:#000000\">Completed</td>";
                                else if (step[0] == Report.WARNING) content += $"<td style=\"color:#CCAA00\">Warning</td>";
                                else if (step[0] == Report.SCREENSHOT) content += $"<td style=\"color:#000000\">Screenshot</td>";
                                else content += $"<td>{step[0]}</td>";
                            }
                            content += $"<td>{step[1]}</td>";
                            content += $"<td>{step[2]}</td>";
                            content += "</tr>";
                        }
                    }
                    content += "</table>";
                    if (Config.languageEngReportMail == false) WorkOnEmail.SendEmail("Failed автотест " + Report.TestFileName + " - " + Report.Description, content);
                    else WorkOnEmail.SendEmail("Failed autotest " + Report.TestFileName + " - " + Report.Description, content);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }

        }

        public void SendMailSuccess() // Отправить письмо с отчетом о провале SUCCESS
        {
            try
            {
                if (Report.TestSuccess == true)
                {

                    string content = "";

                    if (Config.languageEngReportMail == false)
                    {
                        content = "<h2>Отчет о работе автотеста</h2>";
                        content += $"<p>Описание: {Report.Description}</p>";
                        content += $"<p>Файл: {Report.TestFileName}</p>";
                        content += $"<p>Результат: <b style=\"color:#34AF00\">Успешно</b></p>";
                        content += "<br>";
                        content += "<table border=\"1\">";
                        content += "<tr>";
                        content += "<th><b>Статус</b></th>";
                        content += "<th><b>Действие</b></th>";
                        content += "<th><b>Комментарий</b></th>";
                        content += "</tr>";
                    }
                    else
                    {
                        content = "<h2>Autotest Report</h2>";
                        content += $"<p>Description: {Report.Description}</p>";
                        content += $"<p>File: {Report.TestFileName}</p>";
                        content += $"<p>Result: <b style=\"color:#34AF00\">Success</b></p>";
                        content += "<br>";
                        content += "<table border=\"1\">";
                        content += "<tr>";
                        content += "<th><b>Status</b></th>";
                        content += "<th><b>Action</b></th>";
                        content += "<th><b>Comment</b></th>";
                        content += "</tr>";
                    }

                    if (Report.Steps.Count > 0)
                    {
                        foreach (string[] step in Report.Steps)
                        {
                            content += "<tr>";
                            if (Config.languageEngReportMail == false)
                            {
                                if (step[0] == Report.FAILED) content += $"<td style=\"color:#FF0000\">Провально</td>";
                                else if (step[0] == Report.PASSED) content += $"<td style=\"color:#34AF00\">Успешно</td>";
                                else if (step[0] == Report.ERROR) content += $"<td style=\"color:#FF0000\">Ошибка</td>";
                                else if (step[0] == Report.STOPPED) content += $"<td style=\"color:#000000\">Остановлен</td>";
                                else if (step[0] == Report.PROCESS) content += $"<td style=\"color:#000000\">В процессе</td>";
                                else if (step[0] == Report.COMPLETED) content += $"<td style=\"color:#000000\">Выполнено</td>";
                                else if (step[0] == Report.WARNING) content += $"<td style=\"color:#CCAA00\">Предупреждение</td>";
                                else if (step[0] == Report.SCREENSHOT) content += $"<td style=\"color:#000000\">Скриншот</td>";
                                else content += $"<td>{step[0]}</td>";
                            }
                            else
                            {
                                if (step[0] == Report.FAILED) content += $"<td style=\"color:#FF0000\">Failed</td>";
                                else if (step[0] == Report.PASSED) content += $"<td style=\"color:#34AF00\">Passed</td>";
                                else if (step[0] == Report.ERROR) content += $"<td style=\"color:#FF0000\">Error</td>";
                                else if (step[0] == Report.STOPPED) content += $"<td style=\"color:#000000\">Stopped</td>";
                                else if (step[0] == Report.PROCESS) content += $"<td style=\"color:#000000\">Process</td>";
                                else if (step[0] == Report.COMPLETED) content += $"<td style=\"color:#000000\">Completed</td>";
                                else if (step[0] == Report.WARNING) content += $"<td style=\"color:#CCAA00\">Warning</td>";
                                else if (step[0] == Report.SCREENSHOT) content += $"<td style=\"color:#000000\">Screenshot</td>";
                                else content += $"<td>{step[0]}</td>";
                            }
                            content += $"<td>{step[1]}</td>";
                            content += $"<td>{step[2]}</td>";
                            content += "</tr>";
                        }
                    }
                    content += "</table>";
                    if (Config.languageEngReportMail == false) WorkOnEmail.SendEmail("Success автотест " + Report.TestFileName + " - " + Report.Description, content);
                    else WorkOnEmail.SendEmail("Success autotest " + Report.TestFileName + " - " + Report.Description, content);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }
        }

        public void SendMail(string subject, string body) // отправка письма на почту
        {
            try
            {
                WorkOnEmail.SendEmail(subject, body);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }
        }

        public void Description(string text) // Описание автотеста (для отчета)
        {
            try
            {
                Report.SetDescription(text);
                if (Config.languageEngConsole == false) SystemConsoleMsg("Описание: " + text, default, default, default, true);
                else SystemConsoleMsg("Description: " + text, default, default, default, true);
                ConsoleMsg("Описание: " + text);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void ReportAddMessage(string status, string action, string comment) // добавляет сообщение в отчет (письмо)
        {
            Report.AddStep(status, action, comment);
        }

        /* |== ПУБЛИЧНЫЕ МЕТОДЫ ДЛЯ ФРЕЙМВОРКА ========================================================================| */


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUserAgent.Checked)
            {
                textBoxUserAgent.ReadOnly = true;
                textBoxUserAgent.Text = Config.defaultUserAgent;
            }
            else
            {
                textBoxUserAgent.ReadOnly = false;
            }
            webView2.CoreWebView2.Settings.UserAgent = textBoxUserAgent.Text;
        }

        private void textBoxUserAgent_TextChanged(object sender, EventArgs e)
        {
            if (checkBoxUserAgent.Checked == false && textBoxUserAgent.ReadOnly == false)
            {
                webView2.CoreWebView2.Settings.UserAgent = textBoxUserAgent.Text;
            }
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.GoBack();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButtonForward_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.GoForward();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripComboBoxUrl.Text.Contains("https://") == false && toolStripComboBoxUrl.Text.Contains("http://") == false)
                {
                    toolStripComboBoxUrl.Text = "https://" + toolStripComboBoxUrl.Text;
                }
                webView2.CoreWebView2.Navigate(toolStripComboBoxUrl.Text);
                updateToolStripComboBoxUrl();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.Reload();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void webView2_ContentLoading(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            try
            {
                toolStripComboBoxUrl.Text = webView2.Source.ToString();
                ConsoleMsg("Загрузка страницы: " + webView2.Source.ToString());
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void webView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                toolStripComboBoxUrl.Text = webView2.Source.ToString();
                ConsoleMsg("Выполнена загрузка страницы: " + webView2.Source.ToString());
                if (webView2.CoreWebView2.Settings.UserAgent != null && Config.defaultUserAgent == "")
                {
                    Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
                    textBoxUserAgent.Text = Config.defaultUserAgent;
                }
                if (Config.defaultUserAgent != webView2.CoreWebView2.Settings.UserAgent) ConsoleMsg("Текущий User-Agent: " + webView2.CoreWebView2.Settings.UserAgent);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripComboBoxUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.GetHashCode().ToString() == "851981")
                {
                    if (toolStripComboBoxUrl.Text.Contains("https://") == false && toolStripComboBoxUrl.Text.Contains("http://") == false)
                    {
                        toolStripComboBoxUrl.Text = "https://" + toolStripComboBoxUrl.Text;
                    }
                    webView2.CoreWebView2.Navigate(toolStripComboBoxUrl.Text);
                    updateToolStripComboBoxUrl();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }
        }

        private void updateToolStripComboBoxUrl()
        {
            try
            {
                bool thisIsNewUrl = true;
                for (int k = 0; k < toolStripComboBoxUrl.Items.Count; k++)
                {
                    if (toolStripComboBoxUrl.Items[k].ToString() == toolStripComboBoxUrl.Text)
                    {
                        thisIsNewUrl = false;
                        break;
                    }
                }
                if (thisIsNewUrl == true) toolStripComboBoxUrl.Items.Add(toolStripComboBoxUrl.Text);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void создатьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void createProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void createProjectVSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projectVSCreate();
        }

        private void createProjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void createProjectVSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            projectVSCreate();
        }

        /* Создать проект */
        private void projectCreate()
        {
            try
            {
                if (folderBrowserDialogProjectCreate.ShowDialog() == DialogResult.OK)
                {
                    Config.projectPath = folderBrowserDialogProjectCreate.SelectedPath;
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;

                    // создание папок
                    string folderSupport = "/support/";
                    string folderSupportPageObjects = "/support/PageObjects/";
                    string folderSupportStepObjects = "/support/StepObjects/";
                    string folderTests = "/tests/";

                    if (!Directory.Exists(Config.projectPath + folderSupport)) Directory.CreateDirectory(Config.projectPath + folderSupport);
                    if (!Directory.Exists(Config.projectPath + folderSupportPageObjects)) Directory.CreateDirectory(Config.projectPath + folderSupportPageObjects);
                    if (!Directory.Exists(Config.projectPath + folderSupportStepObjects)) Directory.CreateDirectory(Config.projectPath + folderSupportStepObjects);
                    if (!Directory.Exists(Config.projectPath + folderTests)) Directory.CreateDirectory(Config.projectPath + folderTests);

                    if (Directory.Exists(Config.projectPath + folderSupport) &&
                        Directory.Exists(Config.projectPath + folderSupportPageObjects) &&
                        Directory.Exists(Config.projectPath + folderSupportStepObjects) &&
                        Directory.Exists(Config.projectPath + folderTests))
                    {
                        ConsoleMsg("Создание проекта: все необходимые папки созданы");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые папки)");
                        return;
                    }

                    // создание файлов
                    string fileProject = "/project.hat";
                    string fileSupportHelper = "/support/Helper.cs";
                    string fileSupportPageObjectsExample = "/support/PageObjects/ExamplePage.cs";
                    string fileSupportStepObjectsExample = "/support/StepObjects/ExampleSteps.cs";
                    string fileTestsExample1 = "/tests/ExampleTest1.cs";
                    string fileTestsExample2 = "/tests/ExampleTest2.cs";

                    WorkOnFiles writer = new WorkOnFiles();
                    if (!File.Exists(Config.projectPath + fileProject))
                    {
                        Config.defaultDataMail();
                        Config.defaultFlags();
                        writer.writeFile(Config.getConfig(), WorkOnFiles.UTF_8_BOM, Config.projectPath + fileProject);
                    }
                    if (!File.Exists(Config.projectPath + fileSupportHelper)) writer.writeFile(Autotests.getContentFileHelper(), Config.encoding, Config.projectPath + fileSupportHelper);
                    if (!File.Exists(Config.projectPath + fileSupportPageObjectsExample)) writer.writeFile(Autotests.getContentFileExamplePage(), Config.encoding, Config.projectPath + fileSupportPageObjectsExample);
                    if (!File.Exists(Config.projectPath + fileSupportStepObjectsExample)) writer.writeFile(Autotests.getContentFileExampleSteps(), Config.encoding, Config.projectPath + fileSupportStepObjectsExample);
                    if (!File.Exists(Config.projectPath + fileTestsExample1)) writer.writeFile(Autotests.getContentFileExampleTest1(), Config.encoding, Config.projectPath + fileTestsExample1);
                    if (!File.Exists(Config.projectPath + fileTestsExample2)) writer.writeFile(Autotests.getContentFileExampleTest2(), Config.encoding, Config.projectPath + fileTestsExample2);

                    if (File.Exists(Config.projectPath + fileProject) && 
                        File.Exists(Config.projectPath + fileSupportHelper) && 
                        File.Exists(Config.projectPath + fileSupportPageObjectsExample) && 
                        File.Exists(Config.projectPath + fileSupportStepObjectsExample) && 
                        File.Exists(Config.projectPath + fileTestsExample1) &&
                        File.Exists(Config.projectPath + fileTestsExample2))
                    {
                        ConsoleMsg("Создание проекта: все необходимые файлы созданы");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые файлы)");
                        return;
                    }

                    ConsoleMsg("Создание проекта: успешно завершено (версия проекта: " + Config.version + ")");

                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + fileProject);
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                    showDataMail();
                    changeLanguage();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void projectVSCreate()
        {
            try
            {
                InputBoxForm inputBox = new InputBoxForm();
                inputBox.label.Text = "Введите пожалуйста имя проекта (например HatTests)";
                inputBox.ShowDialog();
                string projectName = inputBox.textBox.Text;
                if (projectName == "" || projectName == null || projectName.Contains(" ") == true) {
                    ConsoleMsg("Вы ввели некорректное имя проекта");
                    return;
                }

                if (folderBrowserDialogProjectCreate.ShowDialog() == DialogResult.OK)
                {
                    Config.projectPath = folderBrowserDialogProjectCreate.SelectedPath;
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;

                    // создание папок
                    string folderPropertiesVS = "/Properties/";
                    string folderTestsVs = "/Tests/";
                    string folderSupport = "/Tests/support/";
                    string folderSupportPageObjects = "/Tests/support/PageObjects/";
                    string folderSupportStepObjects = "/Tests/support/StepObjects/";
                    string folderTests = "/Tests/tests/";

                    if (!Directory.Exists(Config.projectPath + folderPropertiesVS)) Directory.CreateDirectory(Config.projectPath + folderPropertiesVS);
                    if (!Directory.Exists(Config.projectPath + folderTestsVs)) Directory.CreateDirectory(Config.projectPath + folderTestsVs);
                    if (!Directory.Exists(Config.projectPath + folderSupport)) Directory.CreateDirectory(Config.projectPath + folderSupport);
                    if (!Directory.Exists(Config.projectPath + folderSupportPageObjects)) Directory.CreateDirectory(Config.projectPath + folderSupportPageObjects);
                    if (!Directory.Exists(Config.projectPath + folderSupportStepObjects)) Directory.CreateDirectory(Config.projectPath + folderSupportStepObjects);
                    if (!Directory.Exists(Config.projectPath + folderTests)) Directory.CreateDirectory(Config.projectPath + folderTests);

                    if (Directory.Exists(Config.projectPath + folderPropertiesVS) &&
                        Directory.Exists(Config.projectPath + folderTestsVs) &&
                        Directory.Exists(Config.projectPath + folderSupport) &&
                        Directory.Exists(Config.projectPath + folderSupportPageObjects) &&
                        Directory.Exists(Config.projectPath + folderSupportStepObjects) &&
                        Directory.Exists(Config.projectPath + folderTests))
                    {
                        ConsoleMsg("Создание проекта: все необходимые папки созданы");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые папки)");
                        return;
                    }

                    // создание файлов
                    string fileGitIgnore = "/.gitignore";
                    string fileSLN = "/" + projectName + ".sln";
                    string fileCSPROJ = "/" + projectName + ".csproj";
                    string fileAssemblyInfo = "/Properties/AssemblyInfo.cs";

                    string fileProject = "/Tests/project.hat";
                    string fileSupportHelper = "/Tests/support/Helper.cs";
                    string fileSupportPageObjectsExample = "/Tests/support/PageObjects/ExamplePage.cs";
                    string fileSupportStepObjectsExample = "/Tests/support/StepObjects/ExampleSteps.cs";
                    string fileTestsExample1 = "/Tests/tests/ExampleTest1.cs";
                    string fileTestsExample2 = "/Tests/tests/ExampleTest2.cs";

                    WorkOnFiles writer = new WorkOnFiles();
                    if (!File.Exists(Config.projectPath + fileGitIgnore)) writer.writeFile(Autotests.getContentGitIgnore(), Config.encoding, Config.projectPath + fileGitIgnore);
                    if (!File.Exists(Config.projectPath + fileSLN)) writer.writeFile(Autotests.getContentFileSLN(projectName), Config.encoding, Config.projectPath + fileSLN);
                    if (!File.Exists(Config.projectPath + fileCSPROJ)) writer.writeFile(Autotests.getContentFileCSPROJ(), Config.encoding, Config.projectPath + fileCSPROJ);
                    if (!File.Exists(Config.projectPath + fileAssemblyInfo)) writer.writeFile(Autotests.getContentFileAssemblyInfo(), Config.encoding, Config.projectPath + fileAssemblyInfo);

                    if (!File.Exists(Config.projectPath + fileProject))
                    {
                        Config.defaultDataMail();
                        writer.writeFile(Config.getConfig(), WorkOnFiles.UTF_8_BOM, Config.projectPath + fileProject);
                    }
                    if (!File.Exists(Config.projectPath + fileSupportHelper)) writer.writeFile(Autotests.getContentFileHelper(), Config.encoding, Config.projectPath + fileSupportHelper);
                    if (!File.Exists(Config.projectPath + fileSupportPageObjectsExample)) writer.writeFile(Autotests.getContentFileExamplePage(), Config.encoding, Config.projectPath + fileSupportPageObjectsExample);
                    if (!File.Exists(Config.projectPath + fileSupportStepObjectsExample)) writer.writeFile(Autotests.getContentFileExampleSteps(), Config.encoding, Config.projectPath + fileSupportStepObjectsExample);
                    if (!File.Exists(Config.projectPath + fileTestsExample1)) writer.writeFile(Autotests.getContentFileExampleTest1(), Config.encoding, Config.projectPath + fileTestsExample1);
                    if (!File.Exists(Config.projectPath + fileTestsExample2)) writer.writeFile(Autotests.getContentFileExampleTest2(), Config.encoding, Config.projectPath + fileTestsExample2);

                    if (File.Exists(Config.projectPath + fileGitIgnore) &&
                        File.Exists(Config.projectPath + fileSLN) &&
                        File.Exists(Config.projectPath + fileCSPROJ) &&
                        File.Exists(Config.projectPath + fileAssemblyInfo) &&
                        File.Exists(Config.projectPath + fileProject) &&
                        File.Exists(Config.projectPath + fileSupportHelper) &&
                        File.Exists(Config.projectPath + fileSupportPageObjectsExample) &&
                        File.Exists(Config.projectPath + fileSupportStepObjectsExample) &&
                        File.Exists(Config.projectPath + fileTestsExample1) &&
                        File.Exists(Config.projectPath + fileTestsExample2))
                    {
                        ConsoleMsg("Создание проекта: все необходимые файлы созданы");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые файлы)");
                        return;
                    }

                    ConsoleMsg("Создание проекта: успешно завершено (версия проекта: " + Config.version + ")");

                    Config.projectPath += "\\Tests";
                    fileProject = "\\project.hat";

                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + fileProject);
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                    showDataMail();
                    changeLanguage();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }


        private void открытьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projectOpen();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            projectOpen();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            projectOpen();
        }

        /* Открыть файл проекта */
        private void projectOpen()
        {
            try
            {
                if(openFileProjectDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileProjectDialog.FileName;
                    Config.projectPath = Path.GetDirectoryName(filename);
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;

                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
;                   changeEncoding();
                    changeLanguage();
                    changeEditorTopMost();
                    changeFullShortReport();
                    showDataMail();

                    ConsoleMsg("Проект открыт (версия проекта: " + Config.version + ")");
                    if (Config.languageEngConsole == false) SystemConsoleMsg($"Проект открыт (версия проекта: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else SystemConsoleMsg($"The project is open (project version: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);

                    if (Config.version != Config.currentBrowserVersion)
                    {
                        ConsoleMsg($"Предупреждение: версия проекта {Config.version} не совпадает с версией браузера {Config.currentBrowserVersion}");
                        if (Config.languageEngConsole == false) SystemConsoleMsg($"Предупреждение: версия проекта {Config.version} не совпадает с версией браузера {Config.currentBrowserVersion}", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                        else SystemConsoleMsg($"Warning: project version {Config.version} does not match the browser version {Config.currentBrowserVersion}", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    }
                }
             }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Обновить дерево файлов и папок */
        public void projectUpdate()
        {
            try
            {
                List<string> saveExtensions = TreeViewExtensions.GetExpansionState(treeViewProject.Nodes);
                treeViewProject.Nodes.Clear();
                if (Config.projectPath != "(не открыт)")
                {
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);
                    ConsoleMsg("Данные в проводнике - обновлены");
                    TreeViewExtensions.SetExpansionState(treeViewProject.Nodes, saveExtensions);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Список подключенный библиотек */
        private void showLibs()
        {
            try
            {
                textBoxLibs.Text = "";
                for (int i = 0; i < Config.libraries.Length; i++)
                {
                    textBoxLibs.Text += Config.libraries[i];
                    if (i < Config.libraries.Length-1) textBoxLibs.Text += Environment.NewLine;
                }
                ConsoleMsg("Список библиотек - загружен");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Кодировка из файла конфигурации */
        private void changeEncoding()
        {
            try
            {
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;
                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;
                if (Config.encoding == WorkOnFiles.DEFAULT)
                {
                    dEFAULTToolStripMenuItem.Checked = true;
                    toolStripMenuItemDEFAULT.Checked = true;
                }
                if (Config.encoding == WorkOnFiles.UTF_8)
                {
                    uTF8ToolStripMenuItem.Checked = true;
                    toolStripMenuItemUTF8.Checked = true;
                }
                if (Config.encoding == WorkOnFiles.UTF_8_BOM)
                {
                    uTF8BOMToolStripMenuItem.Checked = true;
                    toolStripMenuItemUTF8BOM.Checked = true;
                }
                if (Config.encoding == WorkOnFiles.WINDOWS_1251)
                {
                    wINDOWS1251ToolStripMenuItem.Checked = true;
                    toolStripMenuItemWINDOWS1251.Checked = true;
                }
                ConsoleMsg("Кодировка файлов - выбрана");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Язык вывода (русский / английский) */
        private void changeLanguage()
        {
            try
            {
                if (Config.languageEngConsole == false)
                {
                    languageRusToolStripMenuItem.Checked = true;
                    languageRusToolStripMenuItem1.Checked = true;
                    languageEngToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem1.Checked = false;
                    ConsoleMsg("Язык вывода в командной строке - русский");
                }
                else
                {
                    languageRusToolStripMenuItem.Checked = false;
                    languageRusToolStripMenuItem1.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                    languageEngToolStripMenuItem1.Checked = true;
                    ConsoleMsg("Язык вывода в командной строке - английский");
                }

                if(Config.languageEngReportMail == false)
                {
                    languageReportEmailRusToolStripMenuItem.Checked = true;
                    languageReportEmailRusToolStripMenuItem1.Checked = true;
                    languageReportEmailEngToolStripMenuItem.Checked = false;
                    languageReportEmailEngToolStripMenuItem1.Checked = false;
                    ConsoleMsg("Язык вывода в отчет и письмо - русский");
                }
                else
                {
                    languageReportEmailRusToolStripMenuItem.Checked = false;
                    languageReportEmailRusToolStripMenuItem1.Checked = false;
                    languageReportEmailEngToolStripMenuItem.Checked = true;
                    languageReportEmailEngToolStripMenuItem1.Checked = true;
                    ConsoleMsg("Язык вывода в отчет и письмо - английский");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Режим вывода сообщений в отчет */
        private void changeFullShortReport()
        {
            try
            {
                if (Config.fullReport == true)
                {
                    fullReportToolStripMenuItem.Checked = true;
                    fullReportToolStripMenuItem1.Checked = true;
                    shortReportToolStripMenuItem.Checked = false;
                    shortReportToolStripMenuItem1.Checked = false;
                    ConsoleMsg("Режим вывода сообщений - полный отчет");
                }
                else
                {
                    fullReportToolStripMenuItem.Checked = false;
                    fullReportToolStripMenuItem1.Checked = false;
                    shortReportToolStripMenuItem.Checked = true;
                    shortReportToolStripMenuItem1.Checked = true;
                    ConsoleMsg("Режим вывода сообщений - краткий отчет");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Способ открытия редактора (поверх окон) */
        private void changeEditorTopMost()
        {
            try
            {
                editorTopMostToolStripMenuItem.Checked = Config.editorTopMost;
                toolStripMenuItemEditorTopMost.Checked = Config.editorTopMost;
                ConsoleMsg("Способ отображение редактора - выбран");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Настройки почты (проект) */
        private void showDataMail()
        {
            try
            {
                fromMailTextBox.Text = Config.dataMail[0];
                fromLoginTextBox.Text = Config.dataMail[1];
                fromPassTextBox.Text = Config.dataMail[2];
                smtpServerTextBox.Text = Config.dataMail[4];
                portSmtpServerTextBox.Text = Config.dataMail[5];
                if (Config.dataMail[6] == "true") sslCheckBox.Checked = true;
                else sslCheckBox.Checked = false;
                toMailsTextBox.Text = Config.dataMail[3];
                ConsoleMsg("Настройки почты - загружены");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Открыть папку проекта */
        public void openProjectFolder(string path, TreeNodeCollection nodes)
        {
            try
            {
                bool currentNode = true;
                if (nodes.Count > 0) currentNode = false;
                else currentNode = true;

                foreach (string folder in Directory.GetDirectories(path))
                {
                    if (currentNode == true) nodes.Add(folder, getFolderName(folder), 0, 0);
                    else nodes[0].Nodes.Add(folder, getFolderName(folder), 0, 0);
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    if (currentNode == true)
                    {
                        if (getFileName(file) == "project.hat") nodes.Add(file, getFileName(file), 3, 3);
                        else if ((file[file.Length - 3].ToString() + file[file.Length - 2].ToString() + file[file.Length - 1].ToString()) == ".cs") nodes.Add(file, getFileName(file), 1, 1);
                        else nodes.Add(file, getFileName(file), 2, 2);
                    }
                    else
                    {
                        if (getFileName(file) == "project.hat") nodes[0].Nodes.Add(file, getFileName(file), 3, 3);
                        else if ((file[file.Length - 3].ToString() + file[file.Length - 2].ToString() + file[file.Length - 1].ToString()) == ".cs") nodes[0].Nodes.Add(file, getFileName(file), 1, 1);
                        else nodes[0].Nodes.Add(file, getFileName(file), 2, 2);
                    }
                }

                int index = 0;
                foreach (string folder in Directory.GetDirectories(path))
                {
                    if (currentNode == true) openProjectFolder(folder, nodes[index].Nodes);
                    else openProjectFolder(folder, nodes[0].Nodes[index].Nodes);
                    index++;
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public string getFolderName(string path)
        {
            // Регулярные выражения онлайн http://regexstorm.net/tester
            //string pattern = @"\w{1,}$";
            string pattern = @"[^\\]{1,}\w{1,}$";
            string value = Regex.Match(path, pattern).Value;
            return value;
        }

        public string getFileName(string path)
        {
            //string pattern = @"\w{1,}.\w{1,}$";
            string pattern = @"[^\\]{1,}\w{1,}$";
            string value = Regex.Match(path, pattern).Value;
            return value;
        }

        private void оПрограммеCrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showAbout();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            showAbout();
        }

        /* Открыть окно о программе */
        private void showAbout()
        {
            try
            {
                AboutForm about = new AboutForm();
                about.ShowDialog();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void dEFAULTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = true;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = true;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.DEFAULT;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = true;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = true;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void uTF8BOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = true;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = true;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8_BOM;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void wINDOWS1251ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = true;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = true;

                Config.encoding = WorkOnFiles.WINDOWS_1251;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripComboBoxUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                webView2.CoreWebView2.Navigate(toolStripComboBoxUrl.Text);
                updateToolStripComboBoxUrl();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)") Process.Start(Config.projectPath);
                else ConsoleMsg("Проект не открыт");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemDEFAULT_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = true;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = true;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.DEFAULT;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;

                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemUTF8_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = true;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = true;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemUTF8BOM_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = true;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = true;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8_BOM;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemWINDOWS1251_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = true;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = true;

                Config.encoding = WorkOnFiles.WINDOWS_1251;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void treeViewProject_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (treeViewProject.SelectedNode != null)
                {
                    Config.selectName = treeViewProject.SelectedNode.Text;  // имя файла или папки
                    Config.selectValue = treeViewProject.SelectedNode.Name; // путь к файлу или к папке
                    fileOpen();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void treeViewProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeViewProject.SelectedNode != null)
                {
                    Config.selectName = treeViewProject.SelectedNode.Text;  // имя файла или папки
                    Config.selectValue = treeViewProject.SelectedNode.Name; // путь к файлу или к папке
                    toolStripStatusLabelProjectFolderFile.Text = Config.selectName;
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void updateProjectTree()
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    // Строится дерево папок и файлов
                    List<string> saveExtensions = TreeViewExtensions.GetExpansionState(treeViewProject.Nodes);

                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    TreeViewExtensions.SetExpansionState(treeViewProject.Nodes, saveExtensions);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                    ConsoleMsg("Обновлено дерево папок и файлов в окне проекта");
                }
                else
                {
                    ConsoleMsg("Проект не открыт");
                }

            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void openTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    treeViewProject.ExpandAll();
                }
                else
                {
                    ConsoleMsg("Проект не открыт");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            updateProjectTree();
        }

        public void PlayTest(string filename)
        {
            try
            {
                testSuccess = true;
                CleadMessageStep();
                if (Config.selectName.Contains(".cs"))
                {
                    stopTest = false;
                    Report.Init();
                    if (filename == null) Autotests.play(Config.selectName);
                    else Autotests.play(filename);
                }
                else
                {
                    ConsoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void StopTest()
        {
            stopTest = true;
            //Autotests.stopAsync();
            ConsoleMsg("Остановка автотеста");
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            StopTest();
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxConsole.Text = "";
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            StopTest();
        }

        private void listViewTest_DoubleClick(object sender, EventArgs e)
        {
            openStep();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            openStep();
        }

        private void openStep()
        {
            try
            {
                if (listViewTest.Items.Count <= 0) return;
                if (stepTestForm == null)
                {
                    stepTestForm = new StepTestForm();
                    stepTestForm.parent = this;
                    stepTestForm.textBox1.Text = listViewTest.SelectedItems[0].SubItems[1].Text;
                    stepTestForm.textBox2.Text = listViewTest.SelectedItems[0].SubItems[2].Text;
                    stepTestForm.textBox3.Text = listViewTest.SelectedItems[0].SubItems[3].Text;
                    stepTestForm.Show();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void СloseStep()
        {
            if (stepTestForm != null) stepTestForm = null;
        }

        public void СloseCodeEditor()
        {
            if (codeEditorForm != null) codeEditorForm = null;
        }

        private void listViewTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listViewTest_Click(object sender, EventArgs e)
        {
            
        }

        private void listViewTest_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if (stepTestForm != null && listViewTest.SelectedItems.Count > 0)
                {
                    stepTestForm.textBox1.Text = listViewTest.SelectedItems[0].SubItems[1].Text;
                    stepTestForm.textBox2.Text = listViewTest.SelectedItems[0].SubItems[2].Text;
                    stepTestForm.textBox3.Text = listViewTest.SelectedItems[0].SubItems[3].Text;
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void createFolder()
        {
            try
            {
                if (Directory.Exists(Config.selectValue) == true)
                {
                    InputBoxForm inputBox = new InputBoxForm();
                    inputBox.label.Text = "Введите пожалуйста имя новой папки";
                    inputBox.ShowDialog();
                    string foldername = inputBox.textBox.Text;
                    if (foldername == "" || Config.selectValue == "" || Config.selectName == "") return;

                    WorkOnFiles folder = new WorkOnFiles();
                    if (folder.folderCreate(Config.selectValue, foldername) == true)
                    {
                        ConsoleMsg($"Папка \"{foldername}\" - успешно создана");
                        projectUpdate();
                    }
                    else
                    {
                        ConsoleMsg($"Папка \"{foldername}\" - не создана");
                    }
                }
                else
                {
                    ConsoleMsg("Вы не выбрали папку");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFolder();
            else ConsoleMsg("Проект не открыт");
        }

        private void переименоватьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void deleteFolder()
        {
            try
            {
                if (Directory.Exists(Config.selectValue) == true)
                {
                    WorkOnFiles folder = new WorkOnFiles();
                    if (folder.folderDelete(Config.selectValue) == true)
                    {
                        ConsoleMsg($"Папка \"{Config.selectName}\" - успешно удалена");
                        projectUpdate();
                    }
                    else
                    {
                        ConsoleMsg($"Папка \"{Config.selectName}\" - не удалена");
                    }
                }
                else
                {
                    ConsoleMsg("Вы не выбрали папку");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void удалитьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (Config.projectPath != "(не открыт)") deleteFolder();
            else ConsoleMsg("Проект не открыт");
        }

        private void fileOpen()
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    if (File.Exists(Config.selectValue))
                    {
                        if (Config.selectName.Contains(".cs"))
                        {
                            if(codeEditorForm == null) codeEditorForm = new CodeEditorForm();
                            codeEditorForm.parent = this;
                            codeEditorForm.Show();
                            codeEditorForm.OpenFile(Config.selectName, Config.selectValue);
                            codeEditorForm.Focus();

                        }
                        else if (Config.selectName.Contains(".jpeg") || 
                            Config.selectName.Contains(".jpg") || 
                            Config.selectName.Contains(".png") || 
                            Config.selectName.Contains(".html"))
                        {
                            try
                            {
                                Process.Start(Config.selectValue);
                            }
                            catch (Exception ex)
                            {
                                ConsoleMsg(ex.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                Process.Start("notepad.exe", Config.selectValue);
                            }
                            catch (Exception ex)
                            {
                                ConsoleMsg(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    ConsoleMsg("Проект еще не открыть, откройте или создайте новый проект.");
                    projectOpen();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") fileOpen();
            else ConsoleMsg("Проект не открыт");
        }

        private void createFile(string type)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    InputBoxForm inputBox = new InputBoxForm();
                    inputBox.label.Text = "Введите пожалуйста имя файла (расширение файла добавляется автоматически, его указывать специально не нужно)";
                    inputBox.ShowDialog();
                    string filename = inputBox.textBox.Text;
                    if (filename == "" || Config.selectValue == "" || Config.selectName == "") return;

                    string path = Config.projectPath;
                    if (Directory.Exists(Config.selectValue) == true) path = Config.selectValue;
                    if (path[path.Count() - 1].ToString() != "/") path += "/";

                    Autotests.nodes = new List<TreeNode>();
                    Autotests.readNodes(Config.browserForm.treeViewProject.Nodes);
                    string[] listfiles = Autotests.getCSharpFiles();
                    foreach (string csFile in listfiles)
                    {
                        if (csFile.Contains(filename))
                        {
                            ConsoleMsg("Не удалось создать файл с именем " + filename + " по скольку он уже существует в проекте");
                            return;
                        }
                    }

                    if (!File.Exists(path + filename + ".cs"))
                    {
                        WorkOnFiles writer = new WorkOnFiles();
                        if (type == "autotest") writer.writeFile(Autotests.getContentFileNewTest(filename), Config.encoding, path + filename + ".cs");
                        if (type == "page_objects") writer.writeFile(Autotests.getContentFileNewPage(filename), Config.encoding, path + filename + ".cs");
                        if (type == "step_objects") writer.writeFile(Autotests.getContentFileNewStep(filename), Config.encoding, path + filename + ".cs");

                        if (File.Exists(path + filename + ".cs"))
                        {
                            ConsoleMsg("Новый файл теста " + filename + ".cs - успешно создан");
                            projectUpdate();
                        }
                        else ConsoleMsg("Новый файл теста " + filename + ".cs - не удалось создать");
                    }
                    else
                    {
                        ConsoleMsg("Файл " + path + filename + ".cs - уже существует");
                    }
                }
                else
                {
                    ConsoleMsg("Проект еще не открыть, откройте или создайте новый проект.");
                    projectOpen();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void создатьФайлCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("autotest");
            else ConsoleMsg("Проект не открыт");
        }

        private void createFileAutotestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("autotest");
            else ConsoleMsg("Проект не открыт");
        }

        private void createFilePageObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("page_objects");
            else ConsoleMsg("Проект не открыт");
        }

        private void createFileStepObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("step_objects");
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("autotest");
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("page_objects");
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("step_objects");
            else ConsoleMsg("Проект не открыт");
        }

        private void переименоватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void deleteFile()
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {

                    if (Directory.Exists(Config.selectValue) == true) return;

                    if (File.Exists(Config.selectValue))
                    {
                        DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить файл {Config.selectName} ?", "Вопрос", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            File.Delete(Config.selectValue);
                            if (!File.Exists(Config.selectValue))
                            {
                                ConsoleMsg("Файл " + Config.selectValue + " - был успешно удалён");
                                projectUpdate();
                            }
                            else ConsoleMsg("Файл " + Config.selectValue + "- не удалось удалить");
                        }
                    }
                    else
                    {
                        ConsoleMsg("Файл " + Config.selectValue + " - не существует существует");
                    }
                }
                else
                {
                    ConsoleMsg("Проект еще не открыть, откройте или создайте новый проект.");
                    projectOpen();
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void удалитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") deleteFile();
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    string[] delimiter = { Environment.NewLine };
                    Config.libraries = textBoxLibs.Text.Split(delimiter, StringSplitOptions.None);
                    Config.saveConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    ConsoleMsg("Скисок библиотек сохранён в файл project.hat");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void editorTopMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (editorTopMostToolStripMenuItem.Checked)
                {
                    editorTopMostToolStripMenuItem.Checked = false;
                    toolStripMenuItemEditorTopMost.Checked = false;
                }
                else
                {
                    editorTopMostToolStripMenuItem.Checked = true;
                    toolStripMenuItemEditorTopMost.Checked = true;
                }
                Config.editorTopMost = editorTopMostToolStripMenuItem.Checked;
                if (codeEditorForm != null) codeEditorForm.TopMost = Config.editorTopMost;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemEditorTopMost_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemEditorTopMost.Checked)
                {
                    editorTopMostToolStripMenuItem.Checked = false;
                    toolStripMenuItemEditorTopMost.Checked = false;
                }
                else
                {
                    editorTopMostToolStripMenuItem.Checked = true;
                    toolStripMenuItemEditorTopMost.Checked = true;
                }
                Config.editorTopMost = editorTopMostToolStripMenuItem.Checked;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void запуститьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void остановитьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopTest = true;
            //Autotests.stopAsync();
            ConsoleMsg("Остановка автотеста");
        }

        /* Поиск по таблице */
        int _tabFindIndex = 0;
        int _tabFindLast = 0;
        String _tabFindText = "";
        private void tabFindValue(ToolStripComboBox _cbox)
        {
            try
            {
                bool resolution = true;
                for (int k = 0; k < _cbox.Items.Count; k++)
                {
                    if (_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
                }
                if (resolution) _cbox.Items.Add(_cbox.Text);
                if (_tabFindText != _cbox.Text)
                {
                    _tabFindIndex = 0;
                    _tabFindLast = 0;
                    _tabFindText = _cbox.Text;
                }

                string _action;
                string _status;
                string _comment;
                int count = listViewTest.Items.Count;
                for (_tabFindIndex = _tabFindLast; _tabFindIndex < count; _tabFindIndex++)
                {
                    ListViewItem item = listViewTest.Items[_tabFindIndex];
                    _action = item.SubItems[1].Text;
                    _status = item.SubItems[2].Text;
                    _comment = item.SubItems[3].Text;

                    ConsoleMsg(_action + " | " + _status + " | " + _comment);

                    if (_action.Contains(_tabFindText) == true || _status.Contains(_tabFindText) == true || _comment.Contains(_tabFindText) == true)
                    {
                        listViewTest.Focus();
                        listViewTest.Items[_tabFindIndex].Selected = true;
                        listViewTest.EnsureVisible(_tabFindIndex);
                        _tabFindLast = _tabFindIndex + 1;
                        break;
                    }
                }

                if (_tabFindIndex >= count)
                {
                    ConsoleMsg("Поиск в таблице шагов выполнения теста - завершен");
                    _tabFindIndex = 0;
                    _tabFindLast = 0;
                    _tabFindText = _cbox.Text;
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabFindValue(toolStripComboBoxFindValue);
        }

        private void запуститьТестToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") fileOpen();
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile("autotest");
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") deleteFile();
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFolder();
            else ConsoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") deleteFolder();
            else ConsoleMsg("Проект не открыт");
        }


        private void подробнаяИнформацияОШагеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openStep();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.selectName.Contains(".cs"))
                {
                    stopTest = false;
                    Autotests.play(Config.selectName);
                }
                else
                {
                    ConsoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void остановитьТестToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stopTest = true;
            ConsoleMsg("Остановка автотеста");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    panel1.Dock = DockStyle.Fill;
                    numericUpDownBrowserWidth.Value = panel1.Width;
                    numericUpDownBrowserWidth.ReadOnly = true;
                    numericUpDownBrowserHeight.Value = panel1.Height;
                    numericUpDownBrowserHeight.ReadOnly = true;
                }
                //ConsoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton2.Checked)
                {
                    panel1.Dock = DockStyle.None;
                    panel1.Width = (int)numericUpDownBrowserWidth.Value;
                    numericUpDownBrowserWidth.ReadOnly = false;
                    panel1.Height = (int)numericUpDownBrowserHeight.Value;
                    numericUpDownBrowserHeight.ReadOnly = false;
                }
                //ConsoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    numericUpDownBrowserWidth.Value = panel1.Width;
                    numericUpDownBrowserHeight.Value = panel1.Height;
                }
                //ConsoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void numericUpDownBrowserWidth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                panel1.Width = (int)numericUpDownBrowserWidth.Value;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void numericUpDownBrowserHeight_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                panel1.Height = (int)numericUpDownBrowserHeight.Value;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            Autotests.devTestAsync();

            //sendMail();

            /*
            CleadMessageStep();
            Report.Init();
            Autotests.devTestStutsAsync();
            */

            /*
            EditorForm editorForm = new EditorForm();
            editorForm.TopMost = Config.editorTopMost;
            editorForm.Show();
            */

            //CodeEditorForm codeeditor = new CodeEditorForm();
            //codeeditor.Show();
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            richTextBoxErrors.Text = "";
        }

        private async void toolStripButton16_Click(object sender, EventArgs e)
        {
            try
            {
                await webView2.EnsureCoreWebView2Async();
                string script =
                @"(function(){
                var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {};
                var network = performance.getEntriesByType('resource') || {};
                var result = JSON.stringify(network);
                return result;
                }());";
                string jsonText = await webView2.CoreWebView2.ExecuteScriptAsync(script);
                dynamic result = JsonConvert.DeserializeObject(jsonText);
                richTextBoxEvents.Text = result;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void средстваРазработкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView2.CoreWebView2.OpenDevToolsWindow();
        }

        private void toolStripButton14_Click_1(object sender, EventArgs e)
        {
            richTextBoxEvents.Text = "";
        }

        private void выводToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileLogDialog.FileName = "";
                if (saveFileLogDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxConsole.SaveFile(saveFileLogDialog.FileName, RichTextBoxStreamType.PlainText);
                    ConsoleMsg($"Лог вывода сохранён в файл: {saveFileLogDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void ошибкиНаСтраницеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileLogDialog.FileName = "";
                if (saveFileLogDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxErrors.SaveFile(saveFileLogDialog.FileName, RichTextBoxStreamType.PlainText);
                    ConsoleMsg($"Лог ошибок на странице сохранён в файл: {saveFileLogDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void событияНаСтраницеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileLogDialog.FileName = "";
                if (saveFileLogDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxEvents.SaveFile(saveFileLogDialog.FileName, RichTextBoxStreamType.PlainText);
                    ConsoleMsg($"Лог событий на странице сохранён в файл: {saveFileLogDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /* Поиск по тексту */
        int _findIndex = 0;
        int _findLast = 0;
        String _findText = "";
        private void findText(ToolStripComboBox _cbox, RichTextBox _richTextBox)
        {
            try
            {
                bool resolution = true;
                for (int k = 0; k < _cbox.Items.Count; k++)
                    if (_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
                if (resolution) _cbox.Items.Add(_cbox.Text);
                if (_findText != _cbox.Text)
                {
                    _findIndex = 0;
                    _findLast = 0;
                    _findText = _cbox.Text;
                }
                if (_richTextBox.Find(_cbox.Text, _findIndex, _richTextBox.TextLength - 1, RichTextBoxFinds.None) >= 0)
                {
                    _richTextBox.Select();
                    _findIndex = _richTextBox.SelectionStart + _richTextBox.SelectionLength;
                    if (_findLast == _richTextBox.SelectionStart)
                    {
                        MessageBox.Show("Поиск завершен");
                        _findIndex = 0;
                        _findLast = 0;
                        _findText = _cbox.Text;
                    }
                    else
                    {
                        _findLast = _richTextBox.SelectionStart;
                    }
                }
                else
                {
                    MessageBox.Show("Поиск завершен");
                    _findIndex = 0;
                    _findLast = 0;
                    _findText = _cbox.Text;
                }

            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            findText(toolStripComboBoxErrors, richTextBoxErrors);
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            findText(toolStripComboBoxEvents, richTextBoxEvents);
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    CreateCmdForm createCmdForm = new CreateCmdForm();
                    createCmdForm.textBox.Text = $"cd {Directory.GetCurrentDirectory()}" + Environment.NewLine;
                    createCmdForm.textBox.Text += $"Hat.exe {toolStripStatusLabelProjectFolderFile.Text} {toolStripStatusLabelProjectPath.Text}";
                    createCmdForm.ShowDialog();
                }
                else
                {
                    ConsoleMsg("Проект не открыт");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }
        
        private void debugJavaScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.debugJavaScript == true)
            {
                Config.debugJavaScript = false;
                ConsoleMsg("Отключен режим отладки при выполнении JavaScript кода");
            }
            else
            {
                Config.debugJavaScript = true;
                ConsoleMsg("Включен режим отладки при выполнении JavaScript кода");
            }
            debugJavaScriptToolStripMenuItem.Checked = Config.debugJavaScript;
            debugJavaScriptToolStripMenuItem1.Checked = Config.debugJavaScript;
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("help.chm");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void testTableClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CleadMessageStep();
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    Config.dataMail[0] = fromMailTextBox.Text;
                    Config.dataMail[1] = fromLoginTextBox.Text;
                    Config.dataMail[2] = fromPassTextBox.Text;
                    Config.dataMail[4] = smtpServerTextBox.Text;
                    Config.dataMail[5] = portSmtpServerTextBox.Text;
                    if (sslCheckBox.Checked == true) Config.dataMail[6] = "true";
                    else Config.dataMail[6] = "false";
                    Config.dataMail[3] = toMailsTextBox.Text;
                    Config.saveConfigJson(Config.projectPath + "/project.hat");
                    showDataMail();
                    ConsoleMsg("Настройки почты сохранены в файл project.hat");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void internetExplorer11ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                WebBrowser webBrowser = new WebBrowser();
                webBrowser.Navigate(toolStripComboBoxUrl.Text, "_blank");
                ConsoleMsg("Открыт браузер Internet Explorer 11");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void hiddenInterface()
        {
            try
            {
                menuStrip1.Visible = false;
                toolStrip1.Visible = false;
                splitContainer1.Panel2Collapsed = true;
                ConsoleMsg("Интерфейс браузера отключен");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void testingPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (testingPanelToolStripMenuItem.Checked == true)
            {
                splitContainer1.Panel2Collapsed = true;
                testingPanelToolStripMenuItem.Checked = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = false;
                testingPanelToolStripMenuItem.Checked = true;
            }
        }

        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (projectToolStripMenuItem.Checked == true)
            {
                splitContainer2.Panel1Collapsed = true;
                projectToolStripMenuItem.Checked = false;
                if (systemToolStripMenuItem.Checked == false) systemToolStripMenuItem.Checked = true;
            }
            else
            {
                splitContainer2.Panel1Collapsed = false;
                projectToolStripMenuItem.Checked = true;
            }
        }

        private void systemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (systemToolStripMenuItem.Checked == true)
            {
                splitContainer2.Panel2Collapsed = true;
                systemToolStripMenuItem.Checked = false;
                if (projectToolStripMenuItem.Checked == false) projectToolStripMenuItem.Checked = true;
            }
            else
            {
                splitContainer2.Panel2Collapsed = false;
                systemToolStripMenuItem.Checked = true;
            }
        }

        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            try { Process.Start("cmd.exe"); }
            catch (Exception ex) { ConsoleMsgError(ex.ToString()); }
        }

        private void languageRusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // для вывода в командной строке выбран русский язык
            try
            {
                languageRusToolStripMenuItem.Checked = true;
                languageRusToolStripMenuItem1.Checked = true;
                languageEngToolStripMenuItem.Checked = false;
                languageEngToolStripMenuItem1.Checked = false;
                Config.languageEngConsole = false;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на русский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void languageRusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // для вывода в командной строке выбран русский язык
            try
            {
                languageRusToolStripMenuItem.Checked = true;
                languageRusToolStripMenuItem1.Checked = true;
                languageEngToolStripMenuItem.Checked = false;
                languageEngToolStripMenuItem1.Checked = false;
                Config.languageEngConsole = false;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на русский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void languageEngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // для вывода в командной строке выбран английский язык
            try
            {
                languageRusToolStripMenuItem.Checked = false;
                languageRusToolStripMenuItem1.Checked = false;
                languageEngToolStripMenuItem.Checked = true;
                languageEngToolStripMenuItem1.Checked = true;
                Config.languageEngConsole = true;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на английский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }
        

        private void languageEngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // для вывода в командной строке выбран английский язык
            try
            {
                languageRusToolStripMenuItem.Checked = false;
                languageRusToolStripMenuItem1.Checked = false;
                languageEngToolStripMenuItem.Checked = true;
                languageEngToolStripMenuItem1.Checked = true;
                Config.languageEngConsole = true;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на английский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void languageReportEmailRusToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // для вывода в отчет и письмо выбран русский язык
            try
            {
                languageReportEmailRusToolStripMenuItem.Checked = true;
                languageReportEmailRusToolStripMenuItem1.Checked = true;
                languageReportEmailEngToolStripMenuItem.Checked = false;
                languageReportEmailEngToolStripMenuItem1.Checked = false;
                Config.languageEngReportMail = false;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на русский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void languageReportEmailEngToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // для вывода в отчет и письмо выбран английский язык
            try
            {
                languageReportEmailRusToolStripMenuItem.Checked = false;
                languageReportEmailRusToolStripMenuItem1.Checked = false;
                languageReportEmailEngToolStripMenuItem.Checked = true;
                languageReportEmailEngToolStripMenuItem1.Checked = true;
                Config.languageEngReportMail = true;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на английский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void languageReportEmailRusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // для вывода в отчет и письмо выбран русский язык
            try
            {
                languageReportEmailRusToolStripMenuItem.Checked = true;
                languageReportEmailRusToolStripMenuItem1.Checked = true;
                languageReportEmailEngToolStripMenuItem.Checked = false;
                languageReportEmailEngToolStripMenuItem1.Checked = false;
                Config.languageEngReportMail = false;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на русский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void languageReportEmailEngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // для вывода в отчет и письмо выбран английский язык
            try
            {
                languageReportEmailRusToolStripMenuItem.Checked = false;
                languageReportEmailRusToolStripMenuItem1.Checked = false;
                languageReportEmailEngToolStripMenuItem.Checked = true;
                languageReportEmailEngToolStripMenuItem1.Checked = true;
                Config.languageEngReportMail = true;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на английский");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void fullReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // полный отчет
            try
            {
                fullReportToolStripMenuItem.Checked = true;
                fullReportToolStripMenuItem1.Checked = true;
                shortReportToolStripMenuItem.Checked = false;
                shortReportToolStripMenuItem1.Checked = false;
                Config.fullReport = true;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - полный отчет");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void shortReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // краткий отчет
            try
            {
                fullReportToolStripMenuItem.Checked = false;
                fullReportToolStripMenuItem1.Checked = false;
                shortReportToolStripMenuItem.Checked = true;
                shortReportToolStripMenuItem1.Checked = true;
                Config.fullReport = false;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - краткий отчет");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItem16_Click(object sender, EventArgs e)
        {
            // полный отчет
            try
            {
                fullReportToolStripMenuItem.Checked = true;
                fullReportToolStripMenuItem1.Checked = true;
                shortReportToolStripMenuItem.Checked = false;
                shortReportToolStripMenuItem1.Checked = false;
                Config.fullReport = true;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - полный отчет");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItem17_Click(object sender, EventArgs e)
        {
            // краткий отчет
            try
            {
                fullReportToolStripMenuItem.Checked = false;
                fullReportToolStripMenuItem1.Checked = false;
                shortReportToolStripMenuItem.Checked = true;
                shortReportToolStripMenuItem1.Checked = true;
                Config.fullReport = false;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - краткий отчет");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void createResultReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath == "(не открыт)") ConsoleMsg("Проект не открыт! Невозможно сформировать отчет с результатами всех тестов");
            else Report.SaveResultReport();
        }
    }
}
