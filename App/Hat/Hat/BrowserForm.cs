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

namespace Hat
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();

            HatSettings.load();
            StartPage.createStartPage();
            toolStripComboBoxUrl.Text = "file:///" + StartPage.fileStartPage;

            CheckForIllegalCrossThreadCalls = false;
            Config.encoding = WorkOnFiles.UTF_8_BOM;
            toolStripStatusLabelFileEncoding.Text = Config.encoding;
            Config.browserForm = this;
            ConsoleMsg($"Браузер {AppDomain.CurrentDomain.FriendlyName} версия {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})",
                $"Browser {AppDomain.CurrentDomain.FriendlyName} version {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})");
            SystemConsoleMsg("", default, default, default, true);
            if(Config.languageEngConsole == false) SystemConsoleMsg($"Браузер Hat версия {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
            else SystemConsoleMsg($"Browser Hat version {Config.currentBrowserVersion} ({Config.dateBrowserUpdate})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);

            if (Config.statucCacheClear == "true")
            {
                if (Config.languageEngConsole == false) SystemConsoleMsg("Кэш браузера очишен", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else SystemConsoleMsg("The browser cache has been cleared", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                ConsoleMsg("Кэш браузера очишен", "The browser cache has been cleared");
            }
            else if (Config.statucCacheClear == "false")
            {
                if (Config.languageEngConsole == false) SystemConsoleMsg("Кэш браузера неудалось очистить, удалите папку " + Config.cacheFolder + " вручную", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else SystemConsoleMsg("Browser cache could not be cleared. Delete the folder " + Config.cacheFolder, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                ConsoleMsg("Кэш браузера неудалось очистить, удалите папку " + Config.cacheFolder + " вручную", "The browser cache could not be cleared, delete folder " + Config.cacheFolder + " manually");
            }
            else
            {
                if (Config.languageEngConsole == false) SystemConsoleMsg("Кэш не очищен, произошла ошибка: " + Config.statucCacheClear, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                else SystemConsoleMsg("The cache has not been cleared, an error has occurred: " + Config.statucCacheClear, default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                ConsoleMsg("Кэш не очищен, произошла ошибка: " + Config.statucCacheClear, "The cache has not been cleared, an error has occurred:" + Config.statucCacheClear);
            }
        }

        private bool stopTest = false;
        private StepTestForm stepTestForm;
        private CodeEditorForm codeEditorForm;
        private int step = 0;
        private ListViewItem item;
        private ListViewItem.ListViewSubItem subitem;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                initWebView();
                BrowserLanguage();

                if (HatSettings.language == HatSettings.RUS)
                {
                    if (Config.commandLineMode == false) Config.projectPath = "(не открыт)";
                    languageReportEmailRusToolStripMenuItem.Checked = true;
                    languageReportEmailEngToolStripMenuItem.Checked = false;
                    languageRusToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                }
                else
                {
                    if (Config.commandLineMode == false) Config.projectPath = "(not opened)";
                    languageReportEmailRusToolStripMenuItem.Checked = false;
                    languageReportEmailEngToolStripMenuItem.Checked = true;
                    languageRusToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                }

                this.Width = 1440;
                this.Height = 900;
                numericUpDownBrowserWidth.Value = panel1.Width;
                numericUpDownBrowserHeight.Value = panel1.Height;
                if (Config.openHtmlFile != null) toolStripComboBoxUrl.Text = Config.openHtmlFile;
                webView2.Source = new Uri(toolStripComboBoxUrl.Text);
                toolStripStatusLabelProjectPath.Text = Config.projectPath;

                if (Config.commandLineMode == true)
                {
                    if (Config.languageEngConsole == false) SystemConsoleMsg($"Запуск браузера {AppDomain.CurrentDomain.FriendlyName} ...", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else SystemConsoleMsg($"Launching the browser {AppDomain.CurrentDomain.FriendlyName} ...", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    ConsoleMsg($"Запуск браузера {AppDomain.CurrentDomain.FriendlyName} из командной строки", $"Starting the browser {AppDomain.CurrentDomain.FriendlyName} from the command line");
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
                    ConsoleMsg($"Проект открыт (версия проекта: {Config.version})", $"The project is open (project version: {Config.version})");
                    toolStripStatusLabelProjectFolderFile.Text = Config.selectName;
                    PlayTest(Config.selectName);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void BrowserForm_Resize(object sender, EventArgs e)
        {
            toolStripComboBoxUrl.Width = this.Width / 2;
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
            //e.Cancel = true;

            if (codeEditorForm != null)
            {
                e.Cancel = true;
                MessageBox.Show("Редактор кода по прежнему открыт, возможно есть файлы которые не были сохранены. Закройте похалуйста редактор кода.");
                return;
            }

            if (Config.testSuccess == false)
            {
                SystemConsoleMsg(Environment.NewLine + "==============================", default, default, default, true);
                if (Config.languageEngConsole == false) SystemConsoleMsg("Тестирование завершено ПРОВАЛЬНО", default, ConsoleColor.DarkRed, ConsoleColor.White, true);
                else SystemConsoleMsg("Tests ended. Finished: FAILURE", default, ConsoleColor.DarkRed, ConsoleColor.White, true);
                //Environment.Exit(1);
            }
            else
            {
                SystemConsoleMsg(Environment.NewLine + "==============================", default, default, default, true);
                if (Config.languageEngConsole == false) SystemConsoleMsg("Тестирование завершено УСПЕШНО", default, ConsoleColor.DarkGreen, ConsoleColor.White, true);
                else SystemConsoleMsg("Tests ended. Finished: SUCCESS", default, ConsoleColor.DarkGreen, ConsoleColor.White, true);
                //Environment.Exit(0);
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
                ConsoleMsg("Начало инициализации WebView", "The beginning of WebView initialization");
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
                ConsoleMsg("Запущен монитор ошибок на страницах", "The error monitor on the pages is running");

                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.enable", "{}");
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.setCacheDisabled", @"{""cacheDisabled"":true}");
                ConsoleMsg("Выполнена очистка кэша WebView", "The WebView cache has been cleared");

                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Security.setIgnoreCertificateErrors", "{\"ignore\": true}");
                ConsoleMsg("Включено игнорирование сертификата | Security.setIgnoreCertificateErrors (true)", "Certificate ignoring is enabled | Security.setIgnoreCertificateErrors (true)");

                if (Config.defaultUserAgent == "") Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
                ConsoleMsg($"Опция User-Agent по умолчанию {Config.defaultUserAgent}", $"The default User-Agent option {Config.defaultUserAgent}");

                webView2.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
                ConsoleMsg("Выполнена настройка WebView (отключаны alert, prompt, confirm)", "WebView has been configured (alert, prompt, confirm are disabled)");
                ConsoleMsg("Инициализация WebView - завершена", "WebView initialization is completed");
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
                ConsoleMsg("Опция Security.setIgnoreCertificateErrors - включен параметр ignore: true", "Option Security.setIgnoreCertificateErrors - ignore parameter is enabled: true");
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
                ConsoleMsg("Выполнена очистка кэша", "The cache has been cleared");
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

        public void ConsoleMsg(string messageRus, string messageEng) // вывод сообщения в консоль приложения
        {
            try
            {
                if (HatSettings.language == HatSettings.RUS)
                {
                    richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] " + messageRus + Environment.NewLine);
                }
                else
                {
                    richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] " + messageEng + Environment.NewLine);
                }
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
            Config.testSuccess = false;
            Report.AddStep(Report.ERROR, "", message);
            Report.SaveReport(Config.testSuccess);

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

        public void SendMessageStep(string action, string status, string comment, int image, bool debug) // отправляет сообщение в таблицу "тест"
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
            
            if (Config.commandLineMode == false) // в консольном режиме выполнения автотеста - шаги в таблицу не добавляются
            {
                this.item = new ListViewItem();
                this.subitem = new ListViewItem.ListViewSubItem();
                this.subitem.Text = action;
                this.item.SubItems.Add(subitem);
                this.subitem = new ListViewItem.ListViewSubItem();
                if (HatSettings.language == HatSettings.RUS)
                {
                    if (status == Tester.PASSED) this.subitem.Text = "Успешно";
                    else if (status == Tester.FAILED) this.subitem.Text = "Неудача";
                    else if (status == Tester.STOPPED) this.subitem.Text = "Остановлено";
                    else if (status == Tester.PROCESS) this.subitem.Text = "В процессе";
                    else if (status == Tester.COMPLETED) this.subitem.Text = "Выполнено";
                    else if (status == Tester.WARNING) this.subitem.Text = "Предупреждение";
                    else this.subitem.Text = "";
                }
                else
                {
                    if (status == Tester.PASSED) this.subitem.Text = "Passed";
                    else if (status == Tester.FAILED) this.subitem.Text = "Failed";
                    else if (status == Tester.STOPPED) this.subitem.Text = "Stopped";
                    else if (status == Tester.PROCESS) this.subitem.Text = "In process";
                    else if (status == Tester.COMPLETED) this.subitem.Text = "Completed";
                    else if (status == Tester.WARNING) this.subitem.Text = "Warning";
                    else this.subitem.Text = "";
                }
                this.item.SubItems.Add(subitem);
                this.subitem = new ListViewItem.ListViewSubItem();
                this.subitem.Text = comment;
                this.item.SubItems.Add(this.subitem);
                this.item.ImageIndex = image;
                listViewTest.Items.Add(this.item);
                this.step = listViewTest.Items.Count - 1;
                listViewTest.Items[step].Selected = true;
                listViewTest.Items[step].EnsureVisible();
            }

            GC.Collect(); // очистка памяти
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

        public void DisableDebugInReport()
        {
            try
            {
                Config.fullReport = false;
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
            if (Config.testSuccess == false) return;   // автотест был ранее провален
            Config.testSuccess = success; // true - автотест был выполнен успешно | false - автотест был провелен
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
            Report.SaveReport(Config.testSuccess);
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
                    string filename = "";
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
                            if (step[0] == Report.SCREENSHOT) filename = step[2];

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
                    if (Config.languageEngReportMail == false) WorkOnEmail.SendEmail("Failed автотест " + Report.TestFileName + " - " + Report.Description, content, filename);
                    else WorkOnEmail.SendEmail("Failed autotest " + Report.TestFileName + " - " + Report.Description, content, filename);
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
                    string filename = "";
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
                            if (step[0] == Report.SCREENSHOT) filename = step[2];

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
                    if (Config.languageEngReportMail == false) WorkOnEmail.SendEmail("Success автотест " + Report.TestFileName + " - " + Report.Description, content, filename);
                    else WorkOnEmail.SendEmail("Success autotest " + Report.TestFileName + " - " + Report.Description, content, filename);
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }
        }

        public void SendMail(string subject, string body, string filename, string addresses = "") // отправка письма на почту
        {
            try
            {
                WorkOnEmail.SendEmail(subject, body, filename, addresses);
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
                ConsoleMsg("Описание: " + text, "Description: " + text);
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
                ConsoleMsg("Загрузка страницы: " + webView2.Source.ToString(), "Page Loading: " + webView2.Source.ToString());
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
                ConsoleMsg("Выполнена загрузка страницы: " + webView2.Source.ToString(), "The page has been loaded: " + webView2.Source.ToString());
                if (webView2.CoreWebView2.Settings.UserAgent != null && Config.defaultUserAgent == "")
                {
                    Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
                    textBoxUserAgent.Text = Config.defaultUserAgent;
                }
                if (Config.defaultUserAgent != webView2.CoreWebView2.Settings.UserAgent) ConsoleMsg("Текущий User-Agent: " + webView2.CoreWebView2.Settings.UserAgent, "Current User-Agent: " + webView2.CoreWebView2.Settings.UserAgent);
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
                if (HatSettings.language == HatSettings.RUS)
                {
                    languageReportEmailRusToolStripMenuItem.Checked = true;
                    languageReportEmailEngToolStripMenuItem.Checked = false;
                    languageRusToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                    Config.defaultFlags();
                }
                else
                {
                    languageReportEmailRusToolStripMenuItem.Checked = false;
                    languageReportEmailEngToolStripMenuItem.Checked = true;
                    languageRusToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                    Config.defaultFlags();
                }

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
                        ConsoleMsg("Создание проекта: все необходимые папки созданы", "Creating a project: all necessary folders have been created");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые папки)", "Project creation: the process is interrupted (it is impossible to create all the necessary folders)");
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
                        ConsoleMsg("Создание проекта: все необходимые файлы созданы", "Creating a project: all necessary files have been created");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые файлы)", "Project creation: the process is interrupted (it is impossible to create all the necessary files)");
                        return;
                    }

                    ConsoleMsg("Создание проекта: успешно завершено (версия проекта: " + Config.version + ")", "Project creation: successfully completed (project version: " + Config.version + ")");

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
                if (HatSettings.language == HatSettings.RUS)
                {
                    languageReportEmailRusToolStripMenuItem.Checked = true;
                    languageReportEmailEngToolStripMenuItem.Checked = false;
                    languageRusToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                    Config.defaultFlags();
                }
                else
                {
                    languageReportEmailRusToolStripMenuItem.Checked = false;
                    languageReportEmailEngToolStripMenuItem.Checked = true;
                    languageRusToolStripMenuItem.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                    Config.defaultFlags();
                }

                InputBoxForm inputBox = new InputBoxForm();
                if (HatSettings.language == HatSettings.RUS) inputBox.label.Text = "Введите пожалуйста имя проекта (например HatTests)";
                else inputBox.label.Text = "Please enter the name of the project (for example HatTests)";
                inputBox.ShowDialog();
                string projectName = inputBox.textBox.Text;
                if (projectName == "" || projectName == null || projectName.Contains(" ") == true) {
                    ConsoleMsg("Вы ввели некорректное имя проекта", "You entered an incorrect project name");
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
                        ConsoleMsg("Создание проекта: все необходимые папки созданы", "Creating a project: all necessary folders have been created");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые папки)", "Project creation: the process is interrupted (it is impossible to create all the necessary folders)");
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
                        Config.defaultFlags();
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
                        ConsoleMsg("Создание проекта: все необходимые файлы созданы", "Creating a project: all necessary files have been created");
                    }
                    else
                    {
                        ConsoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые файлы)", "Project creation: the process is interrupted (it is impossible to create all the necessary files)");
                        return;
                    }

                    ConsoleMsg("Создание проекта: успешно завершено (версия проекта: " + Config.version + ")", "Project creation: successfully completed (project version: " + Config.version + ")");

                    Config.projectPath += "\\Tests";
                    fileProject = "\\project.hat";
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;

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

                    ConsoleMsg("Проект открыт (версия проекта: " + Config.version + ")", "The project is open (project version: " + Config.version + ")");
                    if (Config.languageEngConsole == false) SystemConsoleMsg($"Проект открыт (версия проекта: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    else SystemConsoleMsg($"The project is open (project version: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);

                    if (Config.version != Config.currentBrowserVersion)
                    {
                        ConsoleMsg($"Предупреждение: версия проекта {Config.version} не совпадает с версией браузера {Config.currentBrowserVersion}",
                            $"Warning: The project version {Config.version} does not match the browser version {Config.currentBrowserVersion}");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
                {
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);
                    ConsoleMsg("Данные в проводнике - обновлены", "The data in the explorer is updated");
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
                ConsoleMsg("Список библиотек - загружен", "List of libraries - downloaded");
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
                ConsoleMsg("Кодировка файлов - выбрана", "File encoding is selected");
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
                    ConsoleMsg("Язык вывода в командной строке - русский", "The command line output language is Russian");
                }
                else
                {
                    languageRusToolStripMenuItem.Checked = false;
                    languageRusToolStripMenuItem1.Checked = false;
                    languageEngToolStripMenuItem.Checked = true;
                    languageEngToolStripMenuItem1.Checked = true;
                    ConsoleMsg("Язык вывода в командной строке - английский", "The command line output language is English");
                }

                if(Config.languageEngReportMail == false)
                {
                    languageReportEmailRusToolStripMenuItem.Checked = true;
                    languageReportEmailRusToolStripMenuItem1.Checked = true;
                    languageReportEmailEngToolStripMenuItem.Checked = false;
                    languageReportEmailEngToolStripMenuItem1.Checked = false;
                    ConsoleMsg("Язык вывода в отчет и письмо - русский", "The language of output to the report and the letter is Russian");
                }
                else
                {
                    languageReportEmailRusToolStripMenuItem.Checked = false;
                    languageReportEmailRusToolStripMenuItem1.Checked = false;
                    languageReportEmailEngToolStripMenuItem.Checked = true;
                    languageReportEmailEngToolStripMenuItem1.Checked = true;
                    ConsoleMsg("Язык вывода в отчет и письмо - английский", "The language of output to the report and the letter is English");
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
                    ConsoleMsg("Режим вывода сообщений - полный отчет", "Message output mode - full report");
                }
                else
                {
                    fullReportToolStripMenuItem.Checked = false;
                    fullReportToolStripMenuItem1.Checked = false;
                    shortReportToolStripMenuItem.Checked = true;
                    shortReportToolStripMenuItem1.Checked = true;
                    ConsoleMsg("Режим вывода сообщений - краткий отчет", "Message output mode - summary report");
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
                ConsoleMsg("Способ отображение редактора - выбран", "The method of displaying the editor is selected");
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
                ConsoleMsg("Настройки почты - загружены", "Mail settings - uploaded");
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

        public string getFolderName2(string path)
        {
            // Регулярные выражения онлайн http://regexstorm.net/tester
            //string pattern = @"\w{1,}$";
            string pattern = @"[^//]{1,}\w{1,}$";
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Process.Start(Config.projectPath);
                else ConsoleMsg("Проект не открыт", "The project is not open");
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

                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Кодировка файлов - изменена", "File encoding has been changed");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
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
                    ConsoleMsg("Обновлено дерево папок и файлов в окне проекта", "Updated the tree of folders and files in the project window");
                }
                else
                {
                    ConsoleMsg("Проект не открыт", "The project is not open");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
                {
                    treeViewProject.ExpandAll();
                }
                else
                {
                    ConsoleMsg("Проект не открыт", "The project is not open");
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
                Config.testSuccess = true;
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
                    ConsoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)", "You have not selected a file to run. (select the *.cs autotest file in the project window)");
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
            ConsoleMsg("Остановка автотеста", "Stopping the autotest");
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
                    if (HatSettings.language == HatSettings.RUS) inputBox.label.Text = "Введите пожалуйста имя новой папки";
                    else inputBox.label.Text = "Please enter the name of the new folder";
                    inputBox.ShowDialog();
                    string foldername = inputBox.textBox.Text;
                    if (foldername == "" || Config.selectValue == "" || Config.selectName == "") return;

                    WorkOnFiles folder = new WorkOnFiles();
                    if (folder.folderCreate(Config.selectValue, foldername) == true)
                    {
                        ConsoleMsg($"Папка \"{foldername}\" - успешно создана", $"Folder \"{foldername}\" - successfully created");
                        projectUpdate();
                    }
                    else
                    {
                        ConsoleMsg($"Папка \"{foldername}\" - не создана", $"Folder \"{foldername}\" - not created");
                    }
                }
                else
                {
                    ConsoleMsg("Вы не выбрали папку", "You have not selected a folder");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFolder();
            else ConsoleMsg("Проект не открыт", "The project is not open");
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
                        ConsoleMsg($"Папка \"{Config.selectName}\" - успешно удалена", $"Folder \"{Config.selectName}\" - successfully deleted");
                        projectUpdate();
                    }
                    else
                    {
                        ConsoleMsg($"Папка \"{Config.selectName}\" - не удалена", $"Folder \"{Config.selectName}\" - not deleted");
                    }
                }
                else
                {
                    ConsoleMsg("Вы не выбрали папку", "You have not selected a folder");
                }
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void удалитьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") deleteFolder();
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void fileOpen()
        {
            try
            {
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
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
                                ConsoleMsg(ex.Message, ex.Message);
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
                                ConsoleMsg(ex.Message, ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    ConsoleMsg("Проект еще не открыть, откройте или создайте новый проект.", "The project has not been opened yet, open or create a new project.");
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
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") fileOpen();
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void createFile(string type)
        {
            try
            {
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
                {
                    InputBoxForm inputBox = new InputBoxForm();
                    if (HatSettings.language == HatSettings.RUS) inputBox.label.Text = "Введите пожалуйста имя файла (расширение файла добавляется автоматически, его указывать специально не нужно)";
                    else inputBox.label.Text = "Please enter the file name (the file extension is added automatically, you do not need to specify it specifically)";
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
                            ConsoleMsg("Не удалось создать файл с именем " + filename + " по скольку он уже существует в проекте",
                                "It was not possible to create a file named " + filename + " because it already exists in the project");
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
                            ConsoleMsg("Новый файл теста " + filename + ".cs - успешно создан", "New test file " + filename + ".cs - successfully created");
                            projectUpdate();
                        }
                        else ConsoleMsg("Новый файл теста " + filename + ".cs - не удалось создать", "New test file " + filename + ".cs - could not be created");
                    }
                    else
                    {
                        ConsoleMsg("Файл " + path + filename + ".cs - уже существует", "File " + path + filename + ".cs - already exists");
                    }
                }
                else
                {
                    ConsoleMsg("Проект еще не открыть, откройте или создайте новый проект.", "The project has not been opened yet, open or create a new project");
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
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("autotest");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void createFileAutotestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("autotest");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void createFilePageObjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("page_objects");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void createFileStepObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("step_objects");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("autotest");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem13_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("page_objects");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem14_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("step_objects");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void переименоватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void deleteFile()
        {
            try
            {
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
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
                                ConsoleMsg("Файл " + Config.selectValue + " - был успешно удалён", "File " + Config.selectValue + " - it was successfully deleted");
                                projectUpdate();
                            }
                            else ConsoleMsg("Файл " + Config.selectValue + " - не удалось удалить", "File " + Config.selectValue + " - failed to delete");
                        }
                    }
                    else
                    {
                        ConsoleMsg("Файл " + Config.selectValue + " - не существует", "File " + Config.selectValue + " - does not exist");
                    }
                }
                else
                {
                    ConsoleMsg("Проект еще не открыть, откройте или создайте новый проект.", "The project has not been opened yet, open or create a new project.");
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
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") deleteFile();
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
                {
                    string[] delimiter = { Environment.NewLine };
                    Config.libraries = textBoxLibs.Text.Split(delimiter, StringSplitOptions.None);
                    Config.saveConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    ConsoleMsg("Список библиотек сохранён в файл project.hat", "The list of libraries is saved in file project.hat");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
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
            ConsoleMsg("Остановка автотеста", "Stopping the autotest");
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

                    ConsoleMsg(_action + " | " + _status + " | " + _comment, _action + " | " + _status + " | " + _comment);

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
                    ConsoleMsg("Поиск в таблице шагов выполнения теста - завершен", "The search in the test execution steps table is completed");
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
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") fileOpen();
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFile("autotest");
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") deleteFile();
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") createFolder();
            else ConsoleMsg("Проект не открыт", "The project is not open");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") deleteFolder();
            else ConsoleMsg("Проект не открыт", "The project is not open");
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
                    ConsoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)",
                        "You have not selected a file to run. (select the *.cs autotest file in the project window)");
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
            ConsoleMsg("Остановка автотеста", "Stopping the autotest");
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
                    ConsoleMsg($"Лог вывода сохранён в файл: {saveFileLogDialog.FileName}", $"The output log is saved to a file: {saveFileLogDialog.FileName}");
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
                    ConsoleMsg($"Лог ошибок на странице сохранён в файл: {saveFileLogDialog.FileName}", $"The error log on the page is saved to a file: {saveFileLogDialog.FileName}");
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
                    ConsoleMsg($"Лог событий на странице сохранён в файл: {saveFileLogDialog.FileName}", $"The event log on the page is saved to a file: {saveFileLogDialog.FileName}");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
                {
                    CreateCmdForm createCmdForm = new CreateCmdForm();
                    createCmdForm.textBox.Text = $"cd {Directory.GetCurrentDirectory()}" + Environment.NewLine;
                    createCmdForm.textBox.Text += $"Hat.exe {toolStripStatusLabelProjectFolderFile.Text} {toolStripStatusLabelProjectPath.Text}";
                    createCmdForm.ShowDialog();
                }
                else
                {
                    ConsoleMsg("Проект не открыт", "The project is not open");
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
                ConsoleMsg("Отключен режим отладки при выполнении JavaScript кода", "Debugging mode is disabled when executing JavaScript code");
            }
            else
            {
                Config.debugJavaScript = true;
                ConsoleMsg("Включен режим отладки при выполнении JavaScript кода", "Debugging mode is enabled when executing JavaScript code");
            }
            debugJavaScriptToolStripMenuItem.Checked = Config.debugJavaScript;
            debugJavaScriptToolStripMenuItem1.Checked = Config.debugJavaScript;
        }

        private void documentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void testTableClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CleadMessageStep();
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)")
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
                    ConsoleMsg("Настройки почты сохранены в файл project.hat", "The mail settings are saved to file project.hat");
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
                ConsoleMsg("Открыт браузер Internet Explorer 11", "The Internet Explorer 11 browser is open");
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
                //menuStrip1.Visible = false;
                //toolStrip1.Visible = false;
                testingPanelToolStripMenuItem.Checked = false;
                splitContainer1.Panel2Collapsed = true;
                ConsoleMsg("Интерфейс браузера отключен", "The browser interface is disabled");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на русский", "The command line output language has been changed to Russian");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на русский", "The command line output language has been changed to Russian");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на английский", "The command line output language has been changed to English");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в командной строке изменен на английский", "The command line output language has been changed to English");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на русский", "The language of the output to the report and the letter has been changed to Russian");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на английский", "The output language in the report and the letter has been changed to English");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на русский", "The language of the output to the report and the letter has been changed to Russian");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Язык вывода в отчет и письмо изменен на английский", "The output language in the report and the letter has been changed to English");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - полный отчет", "Message output mode - full report");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - краткий отчет", "Message output mode - short report");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - полный отчет", "Message output mode - full report");
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
                if (Config.projectPath != "(не открыт)" && Config.projectPath != "(not opened)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                ConsoleMsg("Режим вывода сообщений - краткий отчет", "Message output mode - short report");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void createResultReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath == "(не открыт)" && Config.projectPath != "(not opened)") ConsoleMsg("Проект не открыт! Невозможно сформировать отчет с результатами всех тестов", "The project is not open! It is not possible to generate a report with the results of all tests");
            else Report.SaveResultReport();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"https://github.com/SomovStudio/Hat/releases");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void pluginMySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"https://github.com/SomovStudio/HatPluginMySql");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void BrowserLanguage()
        {
            if (HatSettings.language == HatSettings.RUS)
            {
                русскийToolStripMenuItem.Checked = true;
                englishToolStripMenuItem.Checked = false;

                файлToolStripMenuItem.Text = "Файл";
                сохранитьЛогиToolStripMenuItem.Text = "Сохранить логи";
                выводToolStripMenuItem.Text = "Вывод";
                ошибкиНаСтраницеToolStripMenuItem.Text = "Ошибки на странице";
                событияНаСтраницеToolStripMenuItem.Text = "События на странице";
                закрытьToolStripMenuItem.Text = "Закрыть";
                проектToolStripMenuItem.Text = "Проект";
                toolStripMenuItem7.Text = "Создать проект";
                createProjectToolStripMenuItem1.Text = "Создать простой проект";
                createProjectVSToolStripMenuItem1.Text = "Создать проект совместимый с Visual Studio";
                создатьПроектToolStripMenuItem.Text = "Создать проект";
                открытьПроектToolStripMenuItem.Text = "Открыть проект";
                запуститьТестToolStripMenuItem.Text = "Запустить тест";
                остановитьТестToolStripMenuItem.Text = "Остановить тест";
                настройкиToolStripMenuItem.Text = "Настройки";
                toolStripMenuItem4.Text = "Окно редактора";
                toolStripMenuItemEditorTopMost.Text = "Отображать поверх окон";
                кодировкаФайловToolStripMenuItem.Text = "Кодировка файлов:";
                toolStripMenuItem15.Text = "Отчет (настройка вывода сообщений)";
                fullReportToolStripMenuItem1.Text = "Полный отчет (сообщения всех шагов)";
                shortReportToolStripMenuItem1.Text = "Краткий отчет (только ошибки и пользовательские сообщения)";
                languageReportMailToolStripMenuItem1.Text = "Язык вывода в отчете и письме";
                languageEngRusToolStripMenuItem1.Text = "Язык вывода в командной строке (cmd):";
                debugJavaScriptToolStripMenuItem.Text = "Отладка выполнения JavaScript";
                браузерToolStripMenuItem.Text = "Браузер";
                назадToolStripMenuItem.Text = "Назад";
                впередToolStripMenuItem.Text = "Вперед";
                перейтиToolStripMenuItem.Text = "Перейти";
                обновитьToolStripMenuItem.Text = "Обновить";
                средстваРазработкиToolStripMenuItem.Text = "Средства разработки";
                настройкиToolStripMenuItem1.Text = "Настройки";
                языкToolStripMenuItem.Text = "Язык";
                окноToolStripMenuItem.Text = "Окно";
                testingPanelToolStripMenuItem.Text = "Панель тестировщика";
                projectToolStripMenuItem.Text = "Окно Проект";
                systemToolStripMenuItem.Text = "Окно Система";
                справкаToolStripMenuItem.Text = "Справка";
                updateToolStripMenuItem.Text = "Проверить обновление";
                pluginMySQLToolStripMenuItem.Text = "Плагин MySQL для браузера";
                documentationToolStripMenuItem.Text = "Документация";
                оПрограммеCrackerToolStripMenuItem.Text = "О программе Hat";

                toolStripButtonBack.Text = "Назад";
                toolStripButtonBack.ToolTipText = "Назад";
                toolStripButtonForward.Text = "Вперед";
                toolStripButtonForward.ToolTipText = "Вперед";
                toolStripButtonGo.Text = "Перейти";
                toolStripButtonGo.ToolTipText = "Перейти";
                toolStripButtonUpdate.Text = "Обновить";
                toolStripButtonUpdate.ToolTipText = "Обновить";
                toolStripButton3.Text = "О программе Hat";
                toolStripButton3.ToolTipText = "О программе Hat";

                label1.Text = "Браузер";
                label4.Text = "Проект";
                label6.Text = "Система";
                tabPage5.Text = "Проводник";
                tabPage6.Text = "Тесты";
                tabPage7.Text = "Библиотеки";
                tabPage8.Text = "Почта";
                tabPage1.Text = "Вывод";
                tabPage3.Text = "Ошибки на странице";
                tabPage4.Text = "События на странице";
                tabPage2.Text = "Настройки браузера";
                toolStripDropDownButton4.Text = "Создать проект";
                toolStripDropDownButton4.ToolTipText = "Создать проект";
                createProjectToolStripMenuItem.Text = "Создать простой проект";
                createProjectVSToolStripMenuItem.Text = "Создать проект совместимый с Visual Studio";
                toolStripButton4.Text = "Создать проект";
                toolStripButton4.ToolTipText = "Создать проект";
                toolStripButton5.Text = "Открыть проект";
                toolStripButton5.ToolTipText = "Открыть проект";
                toolStripButton1.Text = "Проводник";
                toolStripButton1.ToolTipText = "Проводник";
                toolStripButton21.Text = "Сформировать команду запуска";
                toolStripButton21.ToolTipText = "Сформировать команду запуска";
                toolStripButton23.Text = "Командная строка (cmd)";
                toolStripButton23.ToolTipText = "Командная строка (cmd)";
                toolStripDropDownButton1.Text = "Настройки";
                toolStripDropDownButton1.ToolTipText = "Настройки";
                окноРедактораToolStripMenuItem.Text = "Окно редактора";
                editorTopMostToolStripMenuItem.Text = "Отображать поверх окон";
                toolStripMenuItem1.Text = "Кодировка файлов:";
                showEventsReportEmailToolStripMenuItem.Text = "Отчет (настройка вывода сообщений)";
                fullReportToolStripMenuItem.Text = "Полный отчет (сообщения всех шагов)";
                shortReportToolStripMenuItem.Text = "Краткий отчет (только ошибки и пользовательские сообщения)";
                createResultReportToolStripMenuItem.Text = "Сформировать отчет с результатами всех тестов";
                languageReportMailToolStripMenuItem.Text = "Язык вывода в отчете и письме:";
                languageEngRusToolStripMenuItem.Text = "Язык вывода в командной строке (cmd):";
                debugJavaScriptToolStripMenuItem1.Text = "Отладка выполнения JavaScript";
                openTreeToolStripMenuItem.Text = "Развернуть дерево папок";
                toolStripDropDownButton2.Text = "Папки";
                toolStripDropDownButton2.ToolTipText = "Папки";
                создатьПапкуToolStripMenuItem.Text = "Создать папку";
                удалитьПапкуToolStripMenuItem.Text = "Удалить папку";
                toolStripDropDownButton3.Text = "Файлы";
                toolStripDropDownButton3.ToolTipText = "Файлы";
                создатьФайлCToolStripMenuItem.Text = "Создать файл автотеста";
                toolStripMenuItem10.Text = "Создать файл:";
                createFileAutotestToolStripMenuItem.Text = "Создать файл Autotest";
                createFilePageObjectsToolStripMenuItem.Text = "Создать файл Page Objects";
                createFileStepObjectToolStripMenuItem.Text = "Создать файл Step Objects";
                удалитьФайлToolStripMenuItem.Text = "Удалить файл";
                открытьФайлToolStripMenuItem.Text = "Открыть файл";
                toolStripButton7.Text = "Обновить";
                toolStripButton7.ToolTipText = "Обновить";
                toolStripButton2.Text = "Запустить тест";
                toolStripButton2.ToolTipText = "Запустить тест";
                toolStripButton6.Text = "Остановить тест";
                toolStripButton6.ToolTipText = "Остановить тест";
                toolStripButton8.Text = "Поиск";
                toolStripButton8.ToolTipText = "Поиск";
                toolStripButton9.Text = "Запустить тест";
                toolStripButton9.ToolTipText = "Запустить тест";
                toolStripButton10.Text = "Остановить тест";
                toolStripButton10.ToolTipText = "Остановить тест";
                toolStripButton11.Text = "Подробная информация о шаге";
                toolStripButton11.ToolTipText = "Подробная информация о шаге";
                toolStripButton12.Text = "Сохранить";
                toolStripButton12.ToolTipText = "Сохранить";
                toolStripButton22.Text = "Сохранить";
                toolStripButton22.ToolTipText = "Сохранить";
                groupBox3.Text = "Отправитель:";
                label9.Text = "Почта:";
                label10.Text = "логин:";
                label11.Text = "пароль:";
                label13.Text = "SMTP-Сервер:";
                label14.Text = "Порт:";
                groupBox4.Text = "Получатель:";
                label12.Text = "Почта получателя:";
                label15.Text = "(для перечисления нескольких почтовых адресов используйте пробел между ними)";
                toolStripButton18.Text = "Очистить";
                toolStripButton18.ToolTipText = "Очистить";
                toolStripButton19.Text = "Сохранить";
                toolStripButton19.ToolTipText = "Сохранить";
                toolStripButton15.Text = "Поиск";
                toolStripButton15.ToolTipText = "Поиск";
                toolStripButton16.Text = "Выгрузить события";
                toolStripButton16.ToolTipText = "Выгрузить события";
                toolStripButton14.Text = "Очистить";
                toolStripButton14.ToolTipText = "Очистить";
                toolStripButton20.Text = "Сохранить";
                toolStripButton20.ToolTipText = "Сохранить";
                toolStripButton17.Text = "Поиск";
                toolStripButton17.ToolTipText = "Поиск";
                groupBox1.Text = "Настройки браузера";
                checkBoxUserAgent.Text = "User-Agent по умолчанию";
                groupBox2.Text = "Размер браузера";
                radioButton1.Text = "На весь экран";
                radioButton2.Text = "Выборочно";
                label7.Text = "ширина:";
                label8.Text = "высота:";

                toolStripStatusLabel5.Text = "Кодировка:";
                if (toolStripStatusLabelFileEncoding.Text == "(not selected)") toolStripStatusLabelFileEncoding.Text = "(не выбрана)";
                toolStripStatusLabel3.Text = "Проект:";
                if (toolStripStatusLabelProjectPath.Text == "(not opened)") toolStripStatusLabelProjectPath.Text = "(не открыт)";
                toolStripStatusLabel4.Text = "файл:";
                if (toolStripStatusLabelProjectFolderFile.Text == "(not selected)") toolStripStatusLabelProjectFolderFile.Text = "(не выбран)";

                очиститьToolStripMenuItem.Text = "Очистить";
                запуститьТестToolStripMenuItem1.Text = "Запустить тест";
                toolStripMenuItem9.Text = "Открыть файл";
                toolStripMenuItem6.Text = "Создать файл автотеста";
                toolStripMenuItem11.Text = "Создать файл";
                toolStripMenuItem12.Text = "Создать файл Autotest";
                toolStripMenuItem13.Text = "Создать файл Page Objects";
                toolStripMenuItem14.Text = "Создать файл Step Objects";
                toolStripMenuItem8.Text = "Удалить файл";
                toolStripMenuItem2.Text = "Создать папку";
                toolStripMenuItem5.Text = "Удалить папку";
                подробнаяИнформацияОШагеToolStripMenuItem.Text = "Подробная информация о шаге";
                toolStripMenuItem3.Text = "Запустить тест";
                остановитьТестToolStripMenuItem1.Text = "Остановить тест";
                testTableClearToolStripMenuItem.Text = "Очистить таблицу";

                columnHeader1.Text = "...";
                columnHeader2.Text = "Действие";
                columnHeader3.Text = "Статус";
                columnHeader4.Text = "Комментарий";
            }
            else
            {
                englishToolStripMenuItem.Checked = true;
                русскийToolStripMenuItem.Checked = false;

                файлToolStripMenuItem.Text = "File";
                сохранитьЛогиToolStripMenuItem.Text = "Save logs";
                выводToolStripMenuItem.Text = "Output";
                ошибкиНаСтраницеToolStripMenuItem.Text = "Errors on the page";
                событияНаСтраницеToolStripMenuItem.Text = "Events on the page";
                закрытьToolStripMenuItem.Text = "Close";
                проектToolStripMenuItem.Text = "Project";
                toolStripMenuItem7.Text = "Create a project";
                createProjectToolStripMenuItem1.Text = "Create a simple project";
                createProjectVSToolStripMenuItem1.Text = "Create a project compatible with Visual Studio";
                создатьПроектToolStripMenuItem.Text = "Create a project";
                открытьПроектToolStripMenuItem.Text = "Open a project";
                запуститьТестToolStripMenuItem.Text = "Run the test";
                остановитьТестToolStripMenuItem.Text = "Stop the test";
                настройкиToolStripMenuItem.Text = "Settings";
                toolStripMenuItem4.Text = "Editor Window";
                toolStripMenuItemEditorTopMost.Text = "Display over windows (top most)";
                кодировкаФайловToolStripMenuItem.Text = "Encoding of files:";
                toolStripMenuItem15.Text = "Report (message output settings)";
                fullReportToolStripMenuItem1.Text = "Full report (messages of all steps)";
                shortReportToolStripMenuItem1.Text = "Short report (errors and user messages only)";
                languageReportMailToolStripMenuItem1.Text = "The output language in the report and the letter:";
                languageEngRusToolStripMenuItem1.Text = "Command line output language (cmd):";
                debugJavaScriptToolStripMenuItem.Text = "Debugging JavaScript execution";
                браузерToolStripMenuItem.Text = "Browser";
                назадToolStripMenuItem.Text = "Back";
                впередToolStripMenuItem.Text = "Next";
                перейтиToolStripMenuItem.Text = "Enter";
                обновитьToolStripMenuItem.Text = "Update";
                средстваРазработкиToolStripMenuItem.Text = "Development tools";
                настройкиToolStripMenuItem1.Text = "Settings";
                языкToolStripMenuItem.Text = "Language";
                окноToolStripMenuItem.Text = "Window";
                testingPanelToolStripMenuItem.Text = "Tester panel";
                projectToolStripMenuItem.Text = "The Project window";
                systemToolStripMenuItem.Text = "The System window";
                справкаToolStripMenuItem.Text = "Help";
                updateToolStripMenuItem.Text = "Check the update";
                pluginMySQLToolStripMenuItem.Text = "Plugin MySQL for browser";
                documentationToolStripMenuItem.Text = "Documentation";
                оПрограммеCrackerToolStripMenuItem.Text = "About Hat";

                toolStripButtonBack.Text = "Back";
                toolStripButtonBack.ToolTipText = "Back";
                toolStripButtonForward.Text = "Next";
                toolStripButtonForward.ToolTipText = "Next";
                toolStripButtonGo.Text = "Enter";
                toolStripButtonGo.ToolTipText = "Enter";
                toolStripButtonUpdate.Text = "Update";
                toolStripButtonUpdate.ToolTipText = "Update";
                toolStripButton3.Text = "About Hat";
                toolStripButton3.ToolTipText = "About Hat";

                label1.Text = "Browser";
                label4.Text = "Project";
                label6.Text = "System";
                tabPage5.Text = "File Explorer";
                tabPage6.Text = "Tests";
                tabPage7.Text = "Libraries";
                tabPage8.Text = "Mail";
                tabPage1.Text = "Output";
                tabPage3.Text = "Errors";
                tabPage4.Text = "Events";
                tabPage2.Text = "Browser Settings";
                toolStripDropDownButton4.Text = "Create a project";
                toolStripDropDownButton4.ToolTipText = "Create a project";
                createProjectToolStripMenuItem.Text = "Create a simple project";
                createProjectVSToolStripMenuItem.Text = "Create a project compatible with Visual Studio";
                toolStripButton4.Text = "Create a project";
                toolStripButton4.ToolTipText = "Create a project";
                toolStripButton5.Text = "Open a project";
                toolStripButton5.ToolTipText = "Open a project";
                toolStripButton1.Text = "File Explorer";
                toolStripButton1.ToolTipText = "File Explorer";
                toolStripButton21.Text = "Create a launch command";
                toolStripButton21.ToolTipText = "Create a launch command";
                toolStripButton23.Text = "The command line (cmd)";
                toolStripButton23.ToolTipText = "The command line (cmd)";
                toolStripDropDownButton1.Text = "Settings";
                toolStripDropDownButton1.ToolTipText = "Settings";
                окноРедактораToolStripMenuItem.Text = "Editor Window";
                editorTopMostToolStripMenuItem.Text = "Display over windows (top most)";
                toolStripMenuItem1.Text = "Encoding of files:";
                showEventsReportEmailToolStripMenuItem.Text = "Report (message output settings)";
                fullReportToolStripMenuItem.Text = "Full report (messages of all steps)";
                shortReportToolStripMenuItem.Text = "Short report (errors and user messages only)";
                createResultReportToolStripMenuItem.Text = "Generate a report with the results of all tests";
                languageReportMailToolStripMenuItem.Text = "The output language in the report and the letter:";
                languageEngRusToolStripMenuItem.Text = "Command line output language (cmd):";
                debugJavaScriptToolStripMenuItem1.Text = "Debugging JavaScript execution";
                openTreeToolStripMenuItem.Text = "Expand the folder tree";
                toolStripDropDownButton2.Text = "Folders";
                toolStripDropDownButton2.ToolTipText = "Folders";
                создатьПапкуToolStripMenuItem.Text = "Create a folder";
                удалитьПапкуToolStripMenuItem.Text = "Delete a folder";
                toolStripDropDownButton3.Text = "Files";
                toolStripDropDownButton3.ToolTipText = "Files";
                создатьФайлCToolStripMenuItem.Text = "Create an autotest file";
                toolStripMenuItem10.Text = "Create a file:";
                createFileAutotestToolStripMenuItem.Text = "Create a file - Autotest";
                createFilePageObjectsToolStripMenuItem.Text = "Create a file - Page Objects";
                createFileStepObjectToolStripMenuItem.Text = "Create a file - Step Objects";
                удалитьФайлToolStripMenuItem.Text = "Delete a file";
                открытьФайлToolStripMenuItem.Text = "Open a file";
                toolStripButton7.Text = "Update";
                toolStripButton7.ToolTipText = "Update";
                toolStripButton2.Text = "Run the test";
                toolStripButton2.ToolTipText = "Run the test";
                toolStripButton6.Text = "Stop the test";
                toolStripButton6.ToolTipText = "Stop the test";
                toolStripButton8.Text = "Search";
                toolStripButton8.ToolTipText = "Search";
                toolStripButton9.Text = "Run the test";
                toolStripButton9.ToolTipText = "Run the test";
                toolStripButton10.Text = "Stop the test";
                toolStripButton10.ToolTipText = "Stop the test";
                toolStripButton11.Text = "Detailed information about the step";
                toolStripButton11.ToolTipText = "Detailed information about the step";
                toolStripButton12.Text = "Save";
                toolStripButton12.ToolTipText = "Save";
                toolStripButton22.Text = "Save";
                toolStripButton22.ToolTipText = "Save";
                groupBox3.Text = "Sender:";
                label9.Text = "Email:";
                label10.Text = "Login:";
                label11.Text = "Password:";
                label13.Text = "SMTP-Server:";
                label14.Text = "Port:";
                groupBox4.Text = "Recipient:";
                label12.Text = "Recipient's email:";
                label15.Text = "(to list multiple email addresses, use a space between them)";
                toolStripButton18.Text = "Clear";
                toolStripButton18.ToolTipText = "Clear";
                toolStripButton19.Text = "Save";
                toolStripButton19.ToolTipText = "Save";
                toolStripButton15.Text = "Search";
                toolStripButton15.ToolTipText = "Search";
                toolStripButton16.Text = "Get events";
                toolStripButton16.ToolTipText = "Get events";
                toolStripButton14.Text = "Clear";
                toolStripButton14.ToolTipText = "Clear";
                toolStripButton20.Text = "Save";
                toolStripButton20.ToolTipText = "Save";
                toolStripButton17.Text = "Search";
                toolStripButton17.ToolTipText = "Search";
                groupBox1.Text = "Browser settings";
                checkBoxUserAgent.Text = "User-Agent (default)";
                groupBox2.Text = "Browser size";
                radioButton1.Text = "Full screen";
                radioButton2.Text = "Optional";
                label7.Text = "width:";
                label8.Text = "height:";

                toolStripStatusLabel5.Text = "Encoding:";
                if (toolStripStatusLabelFileEncoding.Text == "(не выбрана)") toolStripStatusLabelFileEncoding.Text = "(not selected)";
                toolStripStatusLabel3.Text = "Project:";
                if (toolStripStatusLabelProjectPath.Text == "(не открыт)") toolStripStatusLabelProjectPath.Text = "(not opened)";
                toolStripStatusLabel4.Text = "File:";
                if (toolStripStatusLabelProjectFolderFile.Text == "(не выбран)") toolStripStatusLabelProjectFolderFile.Text = "(not selected)";

                очиститьToolStripMenuItem.Text = "Clear";
                запуститьТестToolStripMenuItem1.Text = "Run the test";
                toolStripMenuItem9.Text = "Open a file";
                toolStripMenuItem6.Text = "Create a file - Autotest";
                toolStripMenuItem11.Text = "Create a file";
                toolStripMenuItem12.Text = "Create a file - Autotest";
                toolStripMenuItem13.Text = "Create a file - Page Objects";
                toolStripMenuItem14.Text = "Create a file - Step Objects";
                toolStripMenuItem8.Text = "Delete a file";
                toolStripMenuItem2.Text = "Create a folder";
                toolStripMenuItem5.Text = "Delete a folder";
                подробнаяИнформацияОШагеToolStripMenuItem.Text = "Detailed information about the step";
                toolStripMenuItem3.Text = "Run the test";
                остановитьТестToolStripMenuItem1.Text = "Stop the test";
                testTableClearToolStripMenuItem.Text = "Clear the table";

                columnHeader1.Text = "...";
                columnHeader2.Text = "Action";
                columnHeader3.Text = "Status";
                columnHeader4.Text = "Comment";
            }
        }

        private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HatSettings.language = HatSettings.RUS;
            if (HatSettings.save())
            {
                englishToolStripMenuItem.Checked = false;
                русскийToolStripMenuItem.Checked = true;
                BrowserLanguage();
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HatSettings.language = HatSettings.ENG;
            if (HatSettings.save())
            {
                русскийToolStripMenuItem.Checked = false;
                englishToolStripMenuItem.Checked = true;
                BrowserLanguage();
            }
        }

        private void offlineToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (HatSettings.language == HatSettings.RUS)
                {
                    System.Diagnostics.Process.Start(@"https://somovstudio.github.io/help/Hat/index.html");
                }
                else
                {
                    System.Diagnostics.Process.Start(@"https://somovstudio.github.io/help/Hat/index.html?Description2.html");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
