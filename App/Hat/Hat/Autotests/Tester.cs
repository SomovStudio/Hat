using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using HatFramework;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace HatFrameworkDev
{
    public class Tester
    {
        /* Глобальные константы и переменные */
        public const int IMAGE_STATUS_PROCESS = 0;          // изображение "в процессе"
        public const int IMAGE_STATUS_PASSED = 1;           // изображение "успешно"
        public const int IMAGE_STATUS_FAILED = 2;           // изображение "провально"
        public const int IMAGE_STATUS_MESSAGE = 3;          // изображение "сообщение"
        public const int IMAGE_STATUS_WARNING = 4;          // изображение "предупреждение"
        public const string PASSED = "PASSED";
        public const string FAILED = "FAILED";
        public const string STOPPED = "STOPPED";
        public const string PROCESS = "PROCESS";
        public const string COMPLETED = "COMPLETED";
        public const string WARNING = "WARNING";
        public const string BY_CSS = "BY_CSS";     // тип локатора css
        public const string BY_XPATH = "BY_XPATH"; // тип локатора xpath

        public const string DEFAULT = "DEFAULT";            // кодировка для файлов (по умолчанию)
        public const string UTF8 = "UTF-8";                 // кодировка для файлов UTF-8
        public const string UTF8BOM = "UTF-8 BOM";          // кодировка для файлов UTF-8 BOM
        public const string WINDOWS1251 = "WINDOWS-1251";   // кодировка для файлов WINDOWS-1251

        public Form BrowserWindow;      // объект: окно приложения
        public WebView2 BrowserView;    // объект: браузер
        public bool Debug = false;      // флаг: режим отладки при выполнении JS скриптов

        /* Локальные константы и переменные */
        private const string BY_ID = "BY_ID";
        private const string BY_CLASS = "BY_CLASS";
        private const string BY_NAME = "BY_NAME";
        private const string BY_TAG = "BY_TAG";        

        private MethodInfo browserConsoleMsg;       // функция: ConsoleMsg - вывод сообщения в консоль приложения
        private MethodInfo browserConsoleMsgError;  // функция: ConsoleMsgErrorReport - вывод сообщения об ошибке в консоль приложения
        private MethodInfo browserSystemConsoleMsg; // функция: SystemConsoleMsg - вывод сообщения в системную консоль
        private MethodInfo browserCleadMessageStep; // функция: CleadMessageStep - очистка всех шагов в таблице "тест"
        private MethodInfo browserSendMessageStep;  // функция: SendMessageStep - вывести сообщение в таблицу "тест"
        private MethodInfo browserResize;           // функция: BrowserResize - изменить размер браузера
        private MethodInfo browserUserAgent;        // функция: UserAgent - настройка user-agent параметра
        private MethodInfo browserGetErrors;        // Функция: GetBowserErrors - получить список ошибок и предупреждений браузера
        private MethodInfo disableDebugInReport; // Функция: DisableDebugInReport - Отключен вывод отладочных сообщений SendMessageDebug в отчет
        private MethodInfo checkStopTest;           // функция: CheckStopTest - получить статус остановки процесса тестирования
        private MethodInfo resultAutotest;          // функция: ResultAutotest - устанавливает флаг общего результата выполнения теста
        private MethodInfo debugJavaScript;         // функция: GetStatusDebugJavaScript - возвращает статус отладки
        private MethodInfo getNameAutotest;         // Функция: GetNameAutotest - возвращает имя запущенного автотеста
        private MethodInfo getlanguageEngConsole;   // Функция: GetlanguageEngConsole - возвращает статус английского языка для вывода в командную строку (true/false)
        private MethodInfo getlanguageEngReportMail;// Функция: GetlanguageEngReportMail - возвращает статус английского языка для вывода в отчет и письмо (true/false)
        private MethodInfo saveReport;              // функция: SaveReport - вызывает метод сохранения отчета
        private MethodInfo saveReportScreenshotAsync; // функция: SaveReportScreenshotAsync - сохраняет скриншот текущего состояния браузера
        private MethodInfo sendMailFailure;         // функция: SendMailFailure - отправка отчета о Failure автотеста по почте
        private MethodInfo sendMailSuccess;         // функция: SendMailSuccess - отправка отчета о Success автотеста по почте
        private MethodInfo sendMail;                // функция: SendMail - отправка письма на почту
        private MethodInfo description;             // функция: Description - добавляет описание автотеста для его вывода в отчет
        private MethodInfo reportAddMessage;        // функция: ReportAddMessage - добавляет сообщение в отчет (письмо)

        private bool languageEngConsole = false;    // флаг: английский язык для вывода в командной строке
        private bool languageEngReportEmail = false;// флаг: английский язык для вывода в отчет и письмо
        private bool statusPageLoad = false;        // флаг: статус загрузки страницы
        private bool statusContentLoad = false;     // флаг: статус загрузки контента страницы
        private bool testStop = false;              // флаг: остановка теста
        private bool sendFailureReportByMail = false;  // флаг: отправка Failure отчета по почте
        private bool sendSuccessReportByMail = false;  // флаг: отправка Success отчета по почте
        private string assertStatus = null;         // флаг: рузельтат проверки
        private List<string> listRedirects;         // список редиректов

        public Tester(Form browserForm)
        {
            try
            {
                listRedirects = new List<string>();

                BrowserWindow = browserForm;
                browserConsoleMsg = BrowserWindow.GetType().GetMethod("ConsoleMsg");
                browserConsoleMsgError = BrowserWindow.GetType().GetMethod("ConsoleMsgErrorReport");
                browserSystemConsoleMsg = BrowserWindow.GetType().GetMethod("SystemConsoleMsg");
                browserCleadMessageStep = BrowserWindow.GetType().GetMethod("CleadMessageStep");
                browserSendMessageStep = BrowserWindow.GetType().GetMethod("SendMessageStep");
                browserResize = BrowserWindow.GetType().GetMethod("BrowserResize");
                browserUserAgent = BrowserWindow.GetType().GetMethod("UserAgent");
                browserGetErrors = BrowserWindow.GetType().GetMethod("GetBowserErrors");
                disableDebugInReport = BrowserWindow.GetType().GetMethod("DisableDebugInReport");
                checkStopTest = BrowserWindow.GetType().GetMethod("CheckStopTest");
                resultAutotest = BrowserWindow.GetType().GetMethod("ResultAutotest");
                debugJavaScript = BrowserWindow.GetType().GetMethod("GetStatusDebugJavaScript");
                getNameAutotest = BrowserWindow.GetType().GetMethod("GetNameAutotest");
                getlanguageEngConsole = BrowserWindow.GetType().GetMethod("GetlanguageEngConsole");
                getlanguageEngReportMail = BrowserWindow.GetType().GetMethod("GetlanguageEngReportMail");
                saveReport = BrowserWindow.GetType().GetMethod("SaveReport");
                saveReportScreenshotAsync = BrowserWindow.GetType().GetMethod("SaveReportScreenshotAsync");
                sendMailFailure = BrowserWindow.GetType().GetMethod("SendMailFailure");
                sendMailSuccess = BrowserWindow.GetType().GetMethod("SendMailSuccess");
                sendMail = BrowserWindow.GetType().GetMethod("SendMail");
                description = BrowserWindow.GetType().GetMethod("Description");
                reportAddMessage = BrowserWindow.GetType().GetMethod("ReportAddMessage");

                MethodInfo mi = BrowserWindow.GetType().GetMethod("GetWebView");
                BrowserView = (Microsoft.Web.WebView2.WinForms.WebView2)mi.Invoke(BrowserWindow, null);
                BrowserView.ContentLoading += contentLoading;
                BrowserView.NavigationCompleted += navigationCompleted;
                BrowserView.EnsureCoreWebView2Async();

                changeLanguageAutotest();
                showNameAutotest();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void contentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            try
            {
                statusContentLoad = true; // происходит когда контент страницы загружен
                listRedirects.Add(BrowserView.Source.ToString()); // сохраняется текущий URL в список
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            
        }

        private void navigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            try
            {
                statusPageLoad = true; // происходит когда страницы полностью загружена
                listRedirects.Add(BrowserView.Source.ToString()); // сохраняется текущий URL в список
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void resultAutotestSuccess(bool success)
        {
            try
            {
                resultAutotest.Invoke(BrowserWindow, new object[] { success });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void changeLanguageAutotest()
        {
            try
            {
                languageEngConsole = (bool)getlanguageEngConsole.Invoke(BrowserWindow, null);
                languageEngReportEmail = (bool)getlanguageEngReportMail.Invoke(BrowserWindow, null);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void showNameAutotest()
        {
            try
            {
                SendMessageDebug("Сообщение", "Message", PROCESS, "Запуск автотеста", "Launching the autotest", IMAGE_STATUS_MESSAGE);
                string filename = (string)getNameAutotest.Invoke(BrowserWindow, null);
                SendMessageDebug("Сообщение", "Message", PROCESS, $"Запущен автотест из файла: {filename}", $"The autotest file is running: {filename}", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private async Task<bool> isVisible(string by, string target, int index = default)
        {
            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                if (by == BY_ID) script += $"var elem = document.getElementById('{target}');";
                if (by == BY_CLASS) script += $"var elem = document.getElementsByClassName('{target}')[{index}];";
                if (by == BY_NAME) script += $"var elem = document.getElementsByName('{target}')[{index}];";
                if (by == BY_TAG) script += $"var elem = document.getElementsByTagName('{target}')[{index}];";
                if (by == BY_CSS) script += $"var elem = document.querySelector(\"{target}\");";
                if (by == BY_XPATH) script += $"var elem = document.evaluate(\"{target}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "const style = getComputedStyle(elem);";
                script += "if (style.display === 'none') return false;";
                script += "if (style.visibility !== 'visible') return false;";
                script += "if (style.opacity < 0.1) return false;";
                script += "if (elem.offsetWidth + elem.offsetHeight + elem.getBoundingClientRect().height + elem.getBoundingClientRect().width === 0) return false;";
                script += "const elemCenter = {";
                script += "x: elem.getBoundingClientRect().left,";
                script += "y: elem.getBoundingClientRect().top";
                script += "};";
                script += "if (elemCenter.x < (elem.offsetWidth * -1)) return false;";
                script += "if (elemCenter.x > (document.documentElement.clientWidth || window.innerWidth)) return false;";
                script += "if (elemCenter.y < (elem.offsetHeight * -1)) return false;";
                script += "if (elemCenter.y > (document.documentElement.clientHeight || window.innerHeight)) return false;";
                script += "return true;";
                script += "}());";

                string result = await executeJS(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result != "null" && result != null && result == "true") found = true;
                else found = false;
            }
            catch (Exception ex)
            {
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        private async Task<string> execute(string script, string action)
        {
            string result = null;
            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] {action} - JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] {action} - JS результат: {result}");
                if (result == "null" || result == null)
                {
                    if (Debug == true) SendMessageDebug(action, action, Tester.FAILED, 
                        "В результате выполнения JavaScript получено NULL. Неудалось корректно выполнить JavaScript: " + script, 
                        "The result of JavaScript execution is NULL. Failed to execute JavaScript correctly: " + script, 
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                result = "null";
                SendMessageDebug(action, action, Tester.FAILED,
                    "Ошибка при выполнении JavaScript: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error when executing JavaScript: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result.ToString();
        }

        private async Task<string> executeJS(string script)
        {
            string result = null;
            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        /*
         * Описание автотеста для отчета
         */
        public void Description(string text)
        {
            try
            {
                description.Invoke(BrowserWindow, new object[] { text });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.Message);
            }
        }

        /* 
         * Методы для вывода сообщений о ходе тестирования ==========================================
         * */
        public void ConsoleMsg(string message)
        {
            try
            {
                browserConsoleMsg.Invoke(BrowserWindow, new object[] { message });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ConsoleMsgError(string message)
        {
            try
            {
                browserConsoleMsgError.Invoke(BrowserWindow, new object[] { message });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void ClearMessages()
        {
            try
            {
                browserCleadMessageStep.Invoke(BrowserWindow, null);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public void SendMessage(string action, string status, string comment)
        {
            try
            {
                if (DefineTestStop() == true) return;
                
                // вывод сообщения в системную консоль
                string step = "";
                if (assertStatus != FAILED)
                {
                    if (languageEngConsole == true)
                    {
                        if (status == null) step += "";
                        else if (status == "") step += "";
                        else if (status == PASSED) step += "Step[passed]: ";
                        else if (status == FAILED && assertStatus != FAILED) step += "Step[failed]: ";
                        else if (status == WARNING && assertStatus != FAILED) step += "Step[warning]: ";
                        else if (status == PROCESS) step += "Step[process]: ";
                        else if (status == COMPLETED) step += "Step[completed]: ";
                        else if (status == STOPPED) step += "Step[stopped]: ";
                        else step += status;

                        if (status == null) step += action + " ";
                        else if (status == "") step += action + " ";
                        else if (status == FAILED && assertStatus != FAILED) step += action + " ";
                        else if (status == WARNING && assertStatus != FAILED) step += action + " ";
                        step += comment;

                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                    }
                    else
                    {
                        if (status == null) step += "";
                        else if (status == "") step += "";
                        else if (status == PASSED) step += "Шаг[успешно]: ";
                        else if (status == FAILED && assertStatus != FAILED) step += "Шаг[неудача]: ";
                        else if (status == WARNING && assertStatus != FAILED) step += "Шаг[предупреждение]: ";
                        else if (status == PROCESS) step += "Шаг[в процессе]: ";
                        else if (status == COMPLETED) step += "Шаг[выполнено]: ";
                        else if (status == STOPPED) step += "Шаг[остановлено]: ";
                        else step += status;

                        if (status == null) step += action + " ";
                        else if (status == "") step += action + " ";
                        else if (status == FAILED && assertStatus != FAILED) step += action + " ";
                        else if (status == WARNING && assertStatus != FAILED) step += action + " ";
                        step += comment;

                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                    }
                }

                // вывод сообщения в таблицу браузера
                if (status == PASSED) browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_PASSED, false });
                else if (status == FAILED) browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_FAILED, false });
                else if (status == WARNING) browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_WARNING, false });
                else if (status == STOPPED) browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_WARNING, false });
                else if (status == PROCESS) browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_PROCESS, false });
                else if (status == COMPLETED) browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_MESSAGE, false });
                else browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, IMAGE_STATUS_MESSAGE, false });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }
        
        public void SendMessageDebug(string actionRus, string actionEng, string status, string commentRus, string commentEng, int image)
        {
            try
            {
                // вывод сообщения в системную консоль
                string step = "";
                if (assertStatus != FAILED)
                {
                    if (languageEngConsole == true)
                    {
                        if (status == null) step += "";
                        else if (status == "") step += "";
                        else if (status == PASSED) step += "Step[passed]: ";
                        else if (status == FAILED && assertStatus != FAILED) step += "Step[failed]: ";
                        else if (status == WARNING && assertStatus != FAILED) step += "Step[warning]: ";
                        else if (status == PROCESS) step += "Step[process]: ";
                        else if (status == COMPLETED) step += "Step[completed]: ";
                        else if (status == STOPPED) step += "Step[stopped]: ";
                        else step += status;

                        if (status == null) step += actionEng + " ";
                        else if (status == "") step += actionEng + " ";
                        else if (status == FAILED && assertStatus != FAILED) step += actionEng + " ";
                        else if (status == WARNING && assertStatus != FAILED) step += actionEng + " ";
                        step += commentEng;

                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                    }
                    else
                    {
                        if (status == null) step += "";
                        else if (status == "") step += "";
                        else if (status == PASSED) step += "Шаг[успешно]: ";
                        else if (status == FAILED && assertStatus != FAILED) step += "Шаг[неудача]: ";
                        else if (status == WARNING && assertStatus != FAILED) step += "Шаг[предупреждение]: ";
                        else if (status == PROCESS) step += "Шаг[в процессе]: ";
                        else if (status == COMPLETED) step += "Шаг[выполнено]: ";
                        else if (status == STOPPED) step += "Шаг[остановлено]: ";
                        else step += status;

                        if (status == null) step += actionRus + " ";
                        else if (status == "") step += actionRus + " ";
                        else if (status == FAILED && assertStatus != FAILED) step += actionRus + " ";
                        else if (status == WARNING && assertStatus != FAILED) step += actionRus + " ";
                        step += commentRus;

                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { step, default, default, default, true });
                    }
                }
                else
                {
                    if (languageEngConsole == true && status == FAILED)
                    {
                        browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "Step[failed]: " + actionEng + " " + commentEng, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                    }
                    if (languageEngConsole == false && status == FAILED)
                    {
                        browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "Шаг[неудача]: " + actionRus + " " + commentRus, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                    }
                }

                // вывод сообщения в таблицу браузера
                string action = "";
                string comment = "";
                if (languageEngReportEmail == true)
                {
                    if (actionEng != null) action = actionEng;
                    if (commentEng != null) comment = commentEng;
                }
                else
                {
                    if (actionRus != null) action = actionRus;
                    if (commentRus != null) comment = commentRus;
                }
                browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, image, true });
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
                disableDebugInReport.Invoke(BrowserWindow, null);
                SendMessageDebug("DisableDebugInReportAsync()", "DisableDebugInReportAsync()", COMPLETED, "Отключен вывод отладочных сообщений в отчет", "Disabled output of debugging messages to the report", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug("DisableDebugInReportAsync()", "DisableDebugInReportAsync()", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                ConsoleMsgError(ex.ToString());
            }
        }

        /*
         * Методы для отправки сообщения на почту и телеграм
         */
        public async Task SendMsgToMailAsync(string subject, string body, string filename = "", string addresses = "")
        {
            try
            {
                sendMail.Invoke(BrowserWindow, new Object[] { subject, body, filename, addresses });
                SendMessageDebug($"SendMsgToMailAsync(\"{subject}\", \"{body}\", \"{filename}\", \"{addresses}\")", $"SendMsgToMailAsync(\"{subject}\", \"{body}\", \"{filename}\", \"{addresses}\")", COMPLETED, "Письмо отправлено", "the email has been sent", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"SendMsgToMailAsync(\"{subject}\", \"{body}\", \"{filename}\", \"{addresses}\")", $"SendMsgToMailAsync(\"{subject}\", \"{body}\", \"{filename}\", \"{addresses}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                ConsoleMsgError(ex.Message);
            }
        }

        public async Task SendMsgToTelegramAsync(string botToken, string chatId, string text, string charset = "UTF-8")
        {
            try
            {
                string url = $"https://api.telegram.org/bot{botToken}/sendMessage?chat_id={chatId}&text={text}";

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("charset", charset);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    SendMessageDebug($"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")",
                        $"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")",
                        PASSED, "Сообщение отправлено в Телеграм", "The message was sent in a Telegram", IMAGE_STATUS_PASSED);
                }
                else
                {
                    SendMessageDebug($"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")",
                        $"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")", FAILED,
                        "Не удалось отправить сообщение в Телеграм " + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Failed to send message in Telegram " + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")",
                    $"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                ConsoleMsgError(ex.Message);
            }
        }


        /* 
         * Методы для подготовки к тестированию и его завершению ====================================
         * */
        public async Task TestBeginAsync()
        {
            try
            {
                testStop = false;
                assertStatus = null;
                SendMessageDebug("Тестирование началось", "Testing has started", Tester.COMPLETED, "Инициализация теста", "Initializing the test", IMAGE_STATUS_MESSAGE);
                await BrowserView.EnsureCoreWebView2Async();
                Debug = (bool)debugJavaScript.Invoke(BrowserWindow, null);
                SendMessageDebug("Инициализация теста", "Initializing the test", COMPLETED, "Выполнена инициализация теста", "Initialization of the test has been performed", IMAGE_STATUS_MESSAGE);
                ConsoleMsg("Тест начинается...");

                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "" + Environment.NewLine, default, default, default, false });
                if (languageEngConsole == false) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-- Тест начинается ------------", default, ConsoleColor.White, ConsoleColor.DarkBlue, true });
                else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-- The test begins ------------", default, ConsoleColor.White, ConsoleColor.DarkBlue, true });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task TestEndAsync()
        {
            try
            {
                if (assertStatus == FAILED)
                {
                    ConsoleMsg("Тест завершен - провально");
                    SendMessageDebug("Тестирование завершено", "Testing completed", FAILED, "Тест завершен - шаги теста выполнены неуспешно", "Test completed - the test steps were executed unsuccessfully", IMAGE_STATUS_FAILED);
                    resultAutotestSuccess(false);

                    if (languageEngConsole == false) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { Environment.NewLine + "Тест завершен - провально", default, ConsoleColor.DarkRed, ConsoleColor.White, true });
                    else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { Environment.NewLine + "The test is completed - failed", default, ConsoleColor.DarkRed, ConsoleColor.White, true });
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-------------------------------" + Environment.NewLine, default, default, default, false });

                    Task screenshot = (Task)saveReportScreenshotAsync.Invoke(BrowserWindow, null);
                    await screenshot; // создание скриншота
                }
                else
                {
                    ConsoleMsg("Тест завершен - успешено");
                    SendMessageDebug("Тестирование завершено", "Testing completed", PASSED, "Тест завершен - все шаги выполнены успешно", "The test is completed - all steps are completed successfully", IMAGE_STATUS_PASSED);
                    resultAutotestSuccess(true);

                    if (languageEngConsole == false) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { Environment.NewLine + "Тест завершен - успешено", default, ConsoleColor.DarkGreen, ConsoleColor.White, true });
                    else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { Environment.NewLine + "The test is completed - passed", default, ConsoleColor.DarkGreen, ConsoleColor.White, true });
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-------------------------------" + Environment.NewLine, default, default, default, false });
                }

                saveReport.Invoke(BrowserWindow, null); // сохранение отчета
                if (sendFailureReportByMail == true) sendMailFailure.Invoke(BrowserWindow, null); // отправка Failure отчета по почте
                if (sendSuccessReportByMail == true) sendMailSuccess.Invoke(BrowserWindow, null); // отправка Success отчета по почте
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task TestStopAsync()
        {
            testStop = true;
            assertStatus = FAILED;
            resultAutotestSuccess(false);
        }

        public bool DefineTestStop()
        {
            try
            {
                if (testStop == false)
                {
                    testStop = (bool)checkStopTest.Invoke(BrowserWindow, null);
                    if (testStop == true) SendMessageDebug("DefineTestStop()", "DefineTestStop()", STOPPED, "Выполнение теста остановлено", "Test execution stopped", IMAGE_STATUS_WARNING);
                }
                return testStop;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return testStop;
        }

        public string GetTestResult()
        {
            string result = PASSED;
            try
            {
                if (assertStatus != null) result = assertStatus;
                SendMessageDebug("GetTestResult()", "GetTestResult()", COMPLETED, $"Результат теста получен: {result}", $"The test result is received: {result}", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task BrowserCloseAsync()
        {
            try
            {
                BrowserWindow.Close();
                SendMessageDebug("BrowserCloseAsync()", "BrowserCloseAsync()", COMPLETED, "Браузер закрыт", "Closing the browser - completed", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserSizeAsync(int width, int height)
        {
            try
            {
                if (DefineTestStop() == true) return;
                browserResize.Invoke(BrowserWindow, new object[] { width, height });
                SendMessageDebug($"BrowserSizeAsync({width}, {height})", $"BrowserSizeAsync({width}, {height})", COMPLETED, "Размер браузера изменён", "Browser size changed", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserFullScreenAsync()
        {
            try
            {
                if (DefineTestStop() == true) return;
                browserResize.Invoke(BrowserWindow, new object[] { -1, -1 });
                SendMessageDebug("BrowserFullScreenAsync()", "BrowserFullScreenAsync()", COMPLETED, "Размер браузера изменён", "Browser size changed", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserSetUserAgentAsync(string value)
        {
            try
            {
                if (DefineTestStop() == true) return;
                browserUserAgent.Invoke(BrowserWindow, new object[] { value });
                SendMessageDebug($"BrowserSetUserAgentAsync({value})", $"BrowserSetUserAgentAsync({value})", COMPLETED, "Значение User-Agent изменено", "User-Agent value changed", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> BrowserGetUserAgentAsync()
        {
            string userAgent = null;
            try
            {
                if (DefineTestStop() == true) return "";
                userAgent = BrowserView.CoreWebView2.Settings.UserAgent;
                SendMessageDebug("BrowserGetUserAgentAsync()", "BrowserGetUserAgentAsync()", COMPLETED, $"Из User-Agent получено значение: {userAgent}", $"The value was obtained from the User-Agent: {userAgent}", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return userAgent;
        }

        public async Task<List<string>> BrowserGetErrorsAsync()
        {
            List<string> list = new List<string>();
            try
            {
                if (DefineTestStop() == true) return list;
                list = (List<string>)browserGetErrors.Invoke(BrowserWindow, null);
                SendMessageDebug($"BrowserGetErrorsAsync()", $"BrowserGetErrorsAsync()", COMPLETED, "Получен список ошибок и предупреждений браузера", "Received a list of browser errors and warnings", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return list;
        }

        public async Task<string> BrowserGetNetworkAsync()
        {
            string events = "";
            try
            {
                if (DefineTestStop() == true) return "";

                string script =
                @"(function(){
                var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {};
                var network = performance.getEntriesByType('resource') || {};
                var result = JSON.stringify(network);
                return result;
                }());";
                string jsonText = await executeJS(script);
                dynamic result = JsonConvert.DeserializeObject(jsonText);
                events = result;
                SendMessageDebug("BrowserGetNetworkAsync()", "BrowserGetNetworkAsync()", COMPLETED, "Получен список событий браузера (network)", "Received a list of browser events (network)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return events;
        }

        public async Task<string> BrowserClearNetworkAsync()
        {
            string events = null;
            try
            {
                if (DefineTestStop() == true) return "";

                string script =
                @"(function(){
                var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {};
                var network = performance.getEntries() || {}; 
                var result = network.slice(5,10);
                return result;
                }());";

                events = await executeJS(script);
                SendMessageDebug("BrowserClearNetworkAsync()", "BrowserClearNetworkAsync()", COMPLETED, 
                    "Выполнена очистка событий браузера (network) " + Environment.NewLine + "Текущее состояние Network: " + events.ToString(),
                    "Browser events have been cleared (network) " + Environment.NewLine + "Current status of the Network: " + events.ToString(), 
                    IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return events;
        }

        public async Task BrowserGoBackAsync(int sec, bool abortLoadAfterTime = false)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            statusContentLoad = false;
            if (DefineTestStop() == true) return;

            try
            {
                BrowserView.GoBack();

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop() == true) return;
                }

                if (abortLoadAfterTime == true && statusPageLoad == false)
                {
                    BrowserView.CoreWebView2.Stop();
                    if (statusPageLoad == true || statusContentLoad == true)
                    {
                        SendMessageDebug($"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", $"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", WARNING, "Загрузка страницы остановлена (Выполнено действие браузера - назад)", "Page loading stopped (Browser action performed - back)", IMAGE_STATUS_WARNING);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", $"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", FAILED, "Не выполнено действие браузера - назад (cтраница не загружена)", "Browser action failed - back (page not loaded)", IMAGE_STATUS_FAILED);
                        TestStopAsync();
                    }
                }
                else
                {
                    if (statusPageLoad == true)
                    {
                        SendMessageDebug($"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", $"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", COMPLETED, "Выполнено действие браузера - назад", "Browser action performed - back", IMAGE_STATUS_MESSAGE);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", $"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", FAILED, "Не выполнено действие браузера - назад (cтраница не загружена)", "Browser action failed - back (page not loaded)", IMAGE_STATUS_FAILED);
                        TestStopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", $"BrowserGoBackAsync({sec}, {abortLoadAfterTime})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserGoForwardAsync(int sec, bool abortLoadAfterTime = false)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            statusContentLoad = false;
            if (DefineTestStop() == true) return;

            try
            {
                BrowserView.GoForward();

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop() == true) return;
                }

                if (abortLoadAfterTime == true && statusPageLoad == false)
                {
                    BrowserView.CoreWebView2.Stop();
                    if (statusPageLoad == true || statusContentLoad == true)
                    {
                        SendMessageDebug($"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", $"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", WARNING, "Загрузка страницы остановлена (Выполнено действие браузера - вперед)", "Page loading stopped (Browser action performed - forward)", IMAGE_STATUS_WARNING);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", $"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", FAILED, "Не выполнено действие браузера - вперед (cтраница не загружена)", "Browser action failed - forward (page not loaded)", IMAGE_STATUS_FAILED);
                        TestStopAsync();
                    }
                }
                else
                {
                    if (statusPageLoad == true)
                    {
                        SendMessageDebug($"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", $"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", COMPLETED, "Выполнено действие браузера - вперед", "Browser action performed - forward", IMAGE_STATUS_MESSAGE);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", $"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", FAILED, "Не выполнено действие браузера - вперед (cтраница не загружена)", "Browser action failed - forward (page not loaded)", IMAGE_STATUS_FAILED);
                        TestStopAsync();
                    }
                }

                
            }
            catch (Exception ex)
            {
                SendMessageDebug($"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", $"BrowserGoForwardAsync({sec}, {abortLoadAfterTime})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserBasicAuthenticationAsync(string user, string pass)
        {
            statusPageLoad = false;
            statusContentLoad = false;
            if (DefineTestStop() == true) return;

            try
            {
                BrowserView.CoreWebView2.BasicAuthenticationRequested += delegate (object sender, CoreWebView2BasicAuthenticationRequestedEventArgs args)
                {
                    args.Response.UserName = user;
                    args.Response.Password = pass;
                    SendMessageDebug($"BrowserBasicAuthentication(\"{user}\", \"{pass}\")", $"BrowserBasicAuthentication(\"{user}\", \"{pass}\")", COMPLETED, "Базовая авторизация - выполнена", "Basic authorization - completed", IMAGE_STATUS_MESSAGE);
                };
            }
            catch (Exception ex)
            {
                SendMessageDebug($"BrowserBasicAuthentication(\"{user}\", \"{pass}\")", $"BrowserBasicAuthentication(\"{user}\", \"{pass}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserEnableSendMailAsync(bool byFailure = true, bool bySuccess = true)
        {
            if (DefineTestStop() == true) return;

            try
            {
                sendFailureReportByMail = byFailure;
                sendSuccessReportByMail = bySuccess;
                SendMessageDebug($"BrowserEnableSendMailAsync(\"{byFailure}\", \"{bySuccess}\")", $"BrowserEnableSendMailAsync(\"{byFailure}\", \"{bySuccess}\")", COMPLETED, "Включена опция отправки отчета на почту", "The option to send a report to the mail is enabled", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"BrowserEnableSendMailAsync(\"{byFailure}\", \"{bySuccess}\")", $"BrowserEnableSendMailAsync(\"{byFailure}\", \"{bySuccess}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserPageReloadAsync(int sec, bool abortLoadAfterTime = false)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            statusContentLoad = false;
            if (DefineTestStop() == true) return;

            try
            {
                string locator = "/html";
                string script = "(function(){";
                script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "element.scrollIntoView(); return element;";
                script += "}());";
                string result = await executeJS(script);
                BrowserView.Reload();

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop() == true) return;
                }

                if (abortLoadAfterTime == true && statusPageLoad == false)
                {
                    BrowserView.CoreWebView2.Stop();
                    if (statusPageLoad == true || statusContentLoad == true)
                    {
                        SendMessageDebug($"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", $"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", WARNING, "Перезагрузка страницы остановлена", "Page reload stopped", IMAGE_STATUS_WARNING);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", $"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", FAILED, "Страница не загружена", "The page is not loaded", IMAGE_STATUS_FAILED);
                        TestStopAsync();
                    }
                }
                else
                {
                    if (statusPageLoad == true)
                    {
                        SendMessageDebug($"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", $"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", COMPLETED, "Перезагрузка страницы выполнена", "Page reload completed", IMAGE_STATUS_MESSAGE);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", $"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", FAILED, "Страница не загружена", "The page is not loaded", IMAGE_STATUS_FAILED);
                        TestStopAsync();
                    }
                }
                
            }
            catch (Exception ex)
            {
                SendMessageDebug($"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", $"BrowserPageReloadAsync({sec}, {abortLoadAfterTime})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> BrowserScreenshotAsync(string filename)
        {
            string screenshot = "";
            try
            {
                if (filename == null || filename == "") screenshot = $"image-{DateTime.Now.ToString("dd-mm-yyyy-hh-mm-ss")}.jpeg";
                else screenshot = filename;

                using (System.IO.FileStream file = System.IO.File.Create(screenshot))
                {
                    
                    await BrowserView.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Jpeg, file);
                    if (File.Exists(screenshot))
                    {
                        SendMessageDebug($"BrowserScreenshotAsync({filename})", $"BrowserScreenshotAsync({filename})", COMPLETED, "Скриншот сохранён", "Screenshot saved", IMAGE_STATUS_MESSAGE);
                    }
                    else
                    {
                        SendMessageDebug($"BrowserScreenshotAsync({filename})", $"BrowserScreenshotAsync({filename})", FAILED, "Скриншот неудалось сохранить", "Screenshot could not be saved", IMAGE_STATUS_FAILED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"BrowserScreenshotAsync({filename})", $"BrowserScreenshotAsync({filename})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return screenshot;
        }

        public async Task<string> ExecuteJavaScriptAsync(string script)
        {
            string result = null;
            if (DefineTestStop() == true) return result;

            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
                SendMessageDebug($"ExecuteJavaScriptAsync(\"{script}\")", $"ExecuteJavaScriptAsync(\"{script}\")", PASSED, "Скрипт выполнен. Результат: " + result, "The script is executed. Result: " + result, IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"ExecuteJavaScriptAsync(\"{script}\")", $"ExecuteJavaScriptAsync(\"{script}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }


        /* 
         * Методы для выполнения действий ============================================================
         * */
        public async Task<bool> IsVisibleElementAsync(string by, string locator)
        {
            bool found = false;
            try
            {
                if (by == BY_CSS || by == BY_XPATH)
                {
                    string script = "";
                    script += "(function(){ ";
                    if (by == BY_CSS) script += $"var elem = document.querySelector(\"{locator}\");";
                    if (by == BY_XPATH) script += $"var elem = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                    script += "const style = getComputedStyle(elem);";
                    script += "if (style.display === 'none') return false;";
                    script += "if (style.visibility !== 'visible') return false;";
                    script += "if (style.opacity < 0.1) return false;";
                    script += "if (elem.offsetWidth + elem.offsetHeight + elem.getBoundingClientRect().height + elem.getBoundingClientRect().width === 0) return false;";
                    script += "const elemCenter = {";
                    script += "x: elem.getBoundingClientRect().left,";
                    script += "y: elem.getBoundingClientRect().top";
                    script += "};";
                    script += "if (elemCenter.x < (elem.offsetWidth * -1)) return false;";
                    script += "if (elemCenter.x > (document.documentElement.clientWidth || window.innerWidth)) return false;";
                    script += "if (elemCenter.y < (elem.offsetHeight * -1)) return false;";
                    script += "if (elemCenter.y > (document.documentElement.clientHeight || window.innerHeight)) return false;";
                    script += "return true;";
                    script += "}());";

                    string result = await executeJS(script);
                    if (result != "null" && result != null && result == "true") found = true;
                    else found = false;

                    SendMessageDebug($"IsVisibleElement(\"{by}\", \"{locator}\")", $"IsVisibleElement(\"{by}\", \"{locator}\")", Tester.COMPLETED, "Результат проверки отображения элемента: " + found.ToString(), "Result of checking the display of the element: " + found.ToString(), Tester.IMAGE_STATUS_MESSAGE);
                }
                else
                {
                    SendMessageDebug($"IsVisibleElement(\"{by}\", \"{locator}\")", $"IsVisibleElement(\"{by}\", \"{locator}\")", FAILED, "Неудалось проверить отображение элемента (некорректно указан тип локатора)", "Failed to check the display of the element (the locator type is specified incorrectly)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"IsVisibleElement(\"{by}\", \"{locator}\")", $"IsVisibleElement(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<HTMLElement> GetElementAsync(string by, string locator)
        {
            int step;
            if (DefineTestStop() == true) return null;

            HTMLElement htmlElement = new HTMLElement(this, by, locator);
            try
            {
                HTMLElement el = null;
                string script = "";
                script = "(function(locator = \"" + locator + "\"){";
                if (by == BY_CSS) script += "var el = document.querySelector(locator);";
                else if (by == BY_XPATH) script += "var el = document.evaluate(locator, document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "var obj = {";
                script += "Id: el.id,";
                script += "Class: el.getAttribute('class'),";
                script += "Name: el.name,";
                script += "Type: el.type";
                script += "};";
                script += "return obj;";
                script += "}());";

                var obj = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                el = JsonConvert.DeserializeObject<HTMLElement>(obj);
                if (el == null)
                {
                    SendMessageDebug($"GetElementAsync(\"{by}\", \"{locator}\")", $"GetElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось получить элемент {locator} ({by})", $"Failed to get the element {locator} ({by})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    htmlElement = new HTMLElement(this, by, locator);
                    htmlElement.Id = el.Id;
                    htmlElement.Name = el.Name;
                    htmlElement.Class = el.Class;
                    htmlElement.Type = el.Type;
                    SendMessageDebug($"GetElementAsync(\"{by}\", \"{locator}\")", $"GetElementAsync(\"{by}\", \"{locator}\")", PASSED, "Элемент получен", "The element is received", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetElementAsync(\"{by}\", \"{locator}\")", $"GetElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return htmlElement;
        }

        public async Task<FRAMEElement> GetFrameAsync(int index)
        {
            int step;
            if (DefineTestStop() == true) return null;

            FRAMEElement frameElement = new FRAMEElement(this, index);
            try
            {
                FRAMEElement el = null;
                string script = "(function(){";
                script += $"var frame = window.frames[{index}];";
                script += "var obj = {";
                script += "Name: frame.name,";
                script += $"Index: {index}";
                script += "};";
                script += "return obj;";
                script += "}());";

                var obj = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                el = JsonConvert.DeserializeObject<FRAMEElement>(obj);
                if (el == null)
                {
                    SendMessageDebug($"GetFrameAsync({index})", $"GetFrameAsync({index})", Tester.FAILED, $"Не удалось получить фрейм с индексом {index}", $"Failed to get a frame with index {index}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    frameElement = new FRAMEElement(this, index);
                    frameElement.Index = el.Index;
                    frameElement.Name = el.Name;
                    SendMessageDebug($"GetFrameAsync({index})", $"GetFrameAsync({index})", PASSED, "Фрейм получен", "Frame received", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetFrameAsync({index})", $"GetFrameAsync({index})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return frameElement;
        }

        public async Task GoToUrlAsync(string url, int sec, bool abortLoadAfterTime = false)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            statusContentLoad = false;

            if (DefineTestStop() == true) return;
            SendMessageDebug($"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", $"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", PROCESS, $"Загрузка страницы {url}", $"Page loading {url}", IMAGE_STATUS_MESSAGE);

            try
            {
                BrowserView.CoreWebView2.Navigate(url);
                
                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop() == true) return;
                }

                if (abortLoadAfterTime == true && statusPageLoad == false)
                {
                    BrowserView.CoreWebView2.Stop();
                    if (statusPageLoad == true || statusContentLoad == true)
                    {
                        SendMessageDebug($"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", $"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", WARNING, "Загрузка страницы остановлена", "Page loading stopped", IMAGE_STATUS_WARNING);
                    }
                    else
                    {
                        SendMessageDebug($"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", $"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", FAILED, "Страница не загружена", "The page is not loaded", IMAGE_STATUS_FAILED);
                        await AssertNoErrorsAsync(true);
                        TestStopAsync();
                    }
                }
                else
                {
                    if (statusPageLoad == true)
                    {
                        SendMessageDebug($"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", $"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", PASSED, "Страница загружена", "Page loaded", IMAGE_STATUS_PASSED);
                    }
                    else
                    {
                        SendMessageDebug($"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", $"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", FAILED, "Страница не загружена", "The page is not loaded", IMAGE_STATUS_FAILED);
                        await AssertNoErrorsAsync(true);
                        TestStopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", $"GoToUrlAsync('{url}', {sec}, {abortLoadAfterTime})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task GoToUrlBaseAuthAsync(string url, string login, string pass, int sec, bool abortLoadAfterTime = false)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            statusContentLoad = false;

            if (DefineTestStop() == true) return;
            SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", PROCESS, $"Загрузка страницы (базовая авторизация) {url}", $"Page loading (basic authorization) {url}", IMAGE_STATUS_MESSAGE);

            try
            {
                string baseUrl = "";
                if (url.Contains("https://") == true)
                {
                    baseUrl = url.Replace("https://", "");
                    baseUrl = $"https://{login}:{pass}@{baseUrl}";
                }
                else if(url.Contains("http://") == true)
                {
                    baseUrl = url.Replace("http://", "");
                    baseUrl = $"http://{login}:{pass}@{baseUrl}";
                }
                else
                {
                    baseUrl = $"http://{login}:{pass}@{url}";
                }

                BrowserView.CoreWebView2.Navigate(baseUrl);

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop() == true) return;
                }

                if (abortLoadAfterTime == true && statusPageLoad == false)
                {
                    BrowserView.CoreWebView2.Stop();
                    if (statusPageLoad == true || statusContentLoad == true)
                    {
                        SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", WARNING, "Загрузка страницы остановлена (базовая авторизация)", "Page loading stopped (basic authorization)", IMAGE_STATUS_WARNING);
                    }
                    else
                    {
                        SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", FAILED, "Страница не загружена (базовая авторизация)", "The page is not loaded (basic authorization)", IMAGE_STATUS_FAILED);
                        await AssertNoErrorsAsync(true);
                        TestStopAsync();
                    }
                }
                else
                {
                    if (statusPageLoad == true)
                    {
                        SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", PASSED, "Страница загружена (базовая авторизация)", "Page loaded (basic authorization)", IMAGE_STATUS_PASSED);
                    }
                    else
                    {
                        SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", FAILED, "Страница не загружена (базовая авторизация)", "The page is not loaded (basic authorization)", IMAGE_STATUS_FAILED);
                        await AssertNoErrorsAsync(true);
                        TestStopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec}, {abortLoadAfterTime})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetUrlAsync()
        {
            if (DefineTestStop() == true) return null;
            string url = null;
            try
            {
                url = BrowserView.Source.ToString();
                SendMessageDebug("GetUrlAsync()", "GetUrlAsync()", PASSED, $"Получен текущий URL: {url}", $"The current URL was received: {url}", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug("GetUrlAsync()", "GetUrlAsync()", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return url;
        }

        public async Task<List<string>> GetListRedirectUrlAsync()
        {
            if (DefineTestStop() == true) return null;
            if(listRedirects == null) SendMessageDebug("GetListRedirectUrlAsync()", "GetListRedirectUrlAsync()", FAILED, "Список редиректов NULL", "List of redirects is NULL", IMAGE_STATUS_FAILED);
            else SendMessageDebug("GetListRedirectUrlAsync()", "GetListRedirectUrlAsync()", PASSED, "Список редиректов получен", "The list of redirects has been received", IMAGE_STATUS_PASSED);
            return listRedirects;
        }

        public async Task<int> GetUrlResponseAsync(string url)
        {
            if (DefineTestStop() == true) return 0;

            int statusCode = 0;
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;

                HttpClient client;
                HttpResponseMessage response;
                HttpClientHandler handler = new HttpClientHandler();
                handler.AllowAutoRedirect = false;

                client = new HttpClient(handler);
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
                client.BaseAddress = new Uri(url);

                response = client.GetAsync(url).Result;
                statusCode = (int)response.StatusCode;

                SendMessageDebug($"GetUrlResponseAsync('{url}')", $"GetUrlResponseAsync('{url}')", PASSED, $"Получен HTTP ответ: {statusCode} по URL: {url}", $"HTTP response received: {statusCode} by URL: {url}", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetUrlResponseAsync('{url}')", $"GetUrlResponseAsync('{url}')", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return statusCode;
        }

        public async Task WaitAsync(int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitAsync({sec})", $"WaitAsync({sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                await Task.Delay(sec * 1000);
                SendMessageDebug($"WaitAsync({sec})", $"WaitAsync({sec})", PASSED, $"Ожидание {sec.ToString()} секунд - завершено", $"Waiting {sec.ToString()} seconds - completed", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitAsync({sec})", $"WaitAsync({sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByIdAsync(string id, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitVisibleElementByIdAsync('{id}', {sec})", $"WaitVisibleElementByIdAsync('{id}', {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_ID, id);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"WaitVisibleElementByIdAsync('{id}', {sec})", $"WaitVisibleElementByIdAsync('{id}', {sec})", 
                    PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitVisibleElementByIdAsync('{id}', {sec})", $"WaitVisibleElementByIdAsync('{id}', {sec})", FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitVisibleElementByIdAsync('{id}', {sec})", $"WaitVisibleElementByIdAsync('{id}', {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByClassAsync(string _class, int index, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_CLASS, _class, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByNameAsync(string name, int index, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_NAME, name, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByTagAsync(string tag, int index, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_TAG, tag, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementAsync(string by, string locator, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    if (by == BY_CSS) found = await isVisible(BY_CSS, locator);
                    else if (by == BY_XPATH) found = await isVisible(BY_XPATH, locator);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByIdAsync(string id, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitNotVisibleElementByIdAsync('{id}', {sec})", $"WaitNotVisibleElementByIdAsync('{id}', {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_ID, id);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) SendMessageDebug($"WaitNotVisibleElementByIdAsync('{id}', {sec})", $"WaitNotVisibleElementByIdAsync('{id}', {sec})", PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitNotVisibleElementByIdAsync('{id}', {sec})", $"WaitNotVisibleElementByIdAsync('{id}', {sec})", FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitNotVisibleElementByIdAsync('{id}', {sec})", $"WaitNotVisibleElementByIdAsync('{id}', {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByClassAsync(string _class, int index, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_CLASS, _class, index);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) SendMessageDebug($"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByNameAsync(string name, int index, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_NAME, name, index);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) SendMessageDebug($"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByTagAsync(string tag, int index, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_TAG, tag, index);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) SendMessageDebug($"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementAsync(string by, string locator, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    if (by == BY_CSS) found = await isVisible(BY_CSS, locator);
                    else if (by == BY_XPATH) found = await isVisible(BY_XPATH, locator);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitElementInDomAsync(string by, string locator, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;

                string script = "";
                script += "(function(){ ";
                if (by == BY_CSS) script += $"var elem = document.querySelector(\"{locator}\");";
                else if (by == BY_XPATH) script += $"var elem = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", PASSED, $"Ожидание элемента - завершено (элемент присутствует в DOM)", $"Waiting for the element - completed (the element is present in the DOM)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", FAILED, $"Ожидание элемента - завершено (элемент не присутствует в DOM)", $"Waiting for the element - completed (the element is not present in the DOM)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitElementNotDomAsync(string by, string locator, int sec)
        {
            if (DefineTestStop() == true) return;
            SendMessageDebug($"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;

                string script = "";
                script += "(function(){ ";
                if (by == BY_CSS) script += $"var elem = document.querySelector(\"{locator}\");";
                else if (by == BY_XPATH) script += $"var elem = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result == "null" || result == null)
                    {
                        found = false;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == false) SendMessageDebug($"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", PASSED, $"Ожидание отсутствия элемента - завершено (элемент отсутствует в DOM)", $"Waiting for the absence element - completed (the element is absence in the DOM)", IMAGE_STATUS_PASSED);
                else
                {
                    SendMessageDebug($"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", FAILED, $"Ожидание отсутствия элемента - завершено (элемент присутствует в DOM)", $"Waiting for the absence of an element - completed (the element is present in the DOM)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<bool> FindElementByIdAsync(string id, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindElementByIdAsync('{id}', {sec})", $"FindElementByIdAsync('{id}', {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementById('{id}');";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindElementByIdAsync('{id}', {sec})", $"FindElementByIdAsync('{id}', {sec})", COMPLETED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_MESSAGE);
                else SendMessageDebug($"FindElementByIdAsync('{id}', {sec})", $"FindElementByIdAsync('{id}', {sec})", COMPLETED, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindElementByIdAsync('{id}', {sec})", $"FindElementByIdAsync('{id}', {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementByClassAsync(string _class, int index, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindElementByClassAsync('{_class}', {index}, {sec})", $"FindElementByClassAsync('{_class}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementsByClassName('{_class}')[{index}];";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindElementByClassAsync('{_class}', {index}, {sec})", $"FindElementByClassAsync('{_class}', {index}, {sec})", COMPLETED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_MESSAGE);
                else SendMessageDebug($"FindElementByClassAsync('{_class}', {index}, {sec})", $"FindElementByClassAsync('{_class}', {index}, {sec})", COMPLETED, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindElementByClassAsync('{_class}', {index}, {sec})", $"FindElementByClassAsync('{_class}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementByNameAsync(string name, int index, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindElementByNameAsync('{name}', {index}, {sec})", $"FindElementByNameAsync('{name}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementsByName('{name}')[{index}];";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindElementByNameAsync('{name}', {index}, {sec})", $"FindElementByNameAsync('{name}', {index}, {sec})", COMPLETED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_MESSAGE);
                else SendMessageDebug($"FindElementByNameAsync('{name}', {index}, {sec})", $"FindElementByNameAsync('{name}', {index}, {sec})", COMPLETED, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindElementByNameAsync('{name}', {index}, {sec})", $"FindElementByNameAsync('{name}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementByTagAsync(string tag, int index, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindElementByTagAsync('{tag}', {index}, {sec})", $"FindElementByTagAsync('{tag}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementsByTagName('{tag}')[{index}];";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindElementByTagAsync('{tag}', {index}, {sec})", $"FindElementByTagAsync('{tag}', {index}, {sec})", COMPLETED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_MESSAGE);
                else SendMessageDebug($"FindElementByTagAsync('{tag}', {index}, {sec})", $"FindElementByTagAsync('{tag}', {index}, {sec})", COMPLETED, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindElementByTagAsync('{tag}', {index}, {sec})", $"FindElementByTagAsync('{tag}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementAsync(string by, string locator, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                if (by == BY_CSS) script += $"var elem = document.querySelector(\"{locator}\");";
                else if (by == BY_XPATH) script += $"var elem = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await executeJS(script);
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", COMPLETED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_MESSAGE);
                else SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", COMPLETED, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByIdAsync(string id, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindVisibleElementByIdAsync('{id}', {sec})", $"FindVisibleElementByIdAsync('{id}', {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_ID, id);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindVisibleElementByIdAsync('{id}', {sec})", $"FindVisibleElementByIdAsync('{id}', {sec})", PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else SendMessageDebug($"FindVisibleElementByIdAsync('{id}', {sec})", $"FindVisibleElementByIdAsync('{id}', {sec})", WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindVisibleElementByIdAsync('{id}', {sec})", $"FindVisibleElementByIdAsync('{id}', {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByClassAsync(string _class, int index, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", $"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_CLASS, _class, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", $"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else SendMessageDebug($"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", $"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", $"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByNameAsync(string name, int index, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindVisibleElementByNameAsync('{name}', {index}, {sec})", $"FindVisibleElementByNameAsync('{name}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_NAME, name, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindVisibleElementByNameAsync('{name}', {index}, {sec})", $"FindVisibleElementByNameAsync('{name}', {index}, {sec})", PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else SendMessageDebug($"FindVisibleElementByNameAsync('{name}', {index}, {sec})", $"FindVisibleElementByNameAsync('{name}', {index}, {sec})", WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindVisibleElementByNameAsync('{name}', {index}, {sec})", $"FindVisibleElementByNameAsync('{name}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByTagAsync(string tag, int index, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", $"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_TAG, tag, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", $"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else SendMessageDebug($"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", $"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", $"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementAsync(string by, string locator, int sec)
        {
            if (DefineTestStop() == true) return false;
            SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    if (by == BY_CSS) found = await isVisible(BY_CSS, locator);
                    else if (by == BY_XPATH) found = await isVisible(BY_XPATH, locator);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task ClickElementByIdAsync(string id)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){ var element = document.getElementById('" + id + "'); element.click(); return element; }());";
            if (await execute(script, $"ClickElementByIdAsync('{id}')") == "null")
            {
                SendMessageDebug($"ClickElementByIdAsync('{id}')", $"ClickElementByIdAsync('{id}')", Tester.FAILED, $"Не удалось найти элемент с ID: {id}", $"Couldn't find an element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"ClickElementByIdAsync('{id}')", $"ClickElementByIdAsync('{id}')", PASSED, "Элемент нажат", "The element is pressed", IMAGE_STATUS_PASSED);
            }
        }

        public async Task ClickElementByClassAsync(string _class, int index)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; element.click(); return element; }());";
            if (await execute(script, $"ClickElementByClassAsync('{_class}', {index})") == "null")
            {
                SendMessageDebug($"ClickElementByClassAsync('{_class}', {index})", $"ClickElementByClassAsync('{_class}', {index})", Tester.FAILED, $"Не удалось найти элемент по Class: {_class} (Index: {index})", $"Couldn't find an element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"ClickElementByClassAsync('{_class}', {index})", $"ClickElementByClassAsync('{_class}', {index})", PASSED, "Элемент нажат", "The element is pressed", IMAGE_STATUS_PASSED);
            }
        }

        public async Task ClickElementByNameAsync(string name, int index)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; element.click(); return element; }());";
            if (await execute(script, $"ClickElementByNameAsync('{name}', {index})") == "null")
            {
                SendMessageDebug($"ClickElementByNameAsync('{name}', {index})", $"ClickElementByNameAsync('{name}', {index})", Tester.FAILED, $"Не удалось найти элемент по Name: {name} (Index: {index})", $"Couldn't find an element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"ClickElementByNameAsync('{name}', {index})", $"ClickElementByNameAsync('{name}', {index})", PASSED, "Элемент нажат", "The element is pressed", IMAGE_STATUS_PASSED);
            }
        }

        public async Task ClickElementByTagAsync(string tag, int index)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; element.click(); return element; }());";
            if (await execute(script, $"ClickElementByTagAsync('{tag}', {index})") == "null")
            {
                SendMessageDebug($"ClickElementByTagAsync('{tag}', {index})", $"ClickElementByTagAsync('{tag}', {index})", Tester.FAILED, $"Не удалось найти элемент по Tag: {tag} (Index: {index})", $"Couldn't find an element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"ClickElementByTagAsync('{tag}', {index})", $"ClickElementByTagAsync('{tag}', {index})", PASSED, "Элемент нажат", "The element is pressed", IMAGE_STATUS_PASSED);
            }
        }

        public async Task ClickElementAsync(string by, string locator)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); element.click(); return element;";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; element.click(); return element;";
            script += "}());";
            if (await execute(script, $"ClickElementAsync(\"{by}\", \"{locator}\")") == "null")
            {
                SendMessageDebug($"ClickElementAsync(\"{by}\", \"{locator}\")", $"ClickElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти элемент по локатору: {locator}", $"The element could not be found by the locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"ClickElementAsync(\"{by}\", \"{locator}\")", $"ClickElementAsync(\"{by}\", \"{locator}\")", PASSED, "Элемент нажат", "The element is pressed", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetValueInElementByIdAsync(string id, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += "var element = document.getElementById('" + id + "');";
            script += "element.value = '" + value + "';";
            script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            script += "return element.value;";
            script += "}());";
            if (await execute(script, $"SetValueInElementByIdAsync('{id}', '{value}')") == "null")
            {
                SendMessageDebug($"SetValueInElementByIdAsync('{id}', '{value}')", $"SetValueInElementByIdAsync('{id}', '{value}')", Tester.FAILED, $"Не удалось найти или ввести значение в элемент с ID: {id}", $"Could not find or enter a value in an element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetValueInElementByIdAsync('{id}', '{value}')", $"SetValueInElementByIdAsync('{id}', '{value}')", PASSED, $"Значение '{value}' введено в элемент", $"The value '{value}' was entered into the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetValueInElementByClassAsync(string _class, int index, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += "var element = document.getElementsByClassName('" + _class + "')[" + index + "];";
            script += "element.value = '" + value + "';";
            script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            script += "return element.value;";
            script += "}());";
            if (await execute(script, $"SetValueInElementByClassAsync('{_class}', {index}, '{value}')") == "null")
            {
                SendMessageDebug($"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", $"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", Tester.FAILED, $"Не удалось найти или ввести значение в элемент по Class: {_class} (Index: {index})", $"Could not find or enter a value in the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", $"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", PASSED, $"Значение '{value}' введено в элемент", $"The value '{value}' was entered into the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetValueInElementByNameAsync(string name, int index, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += "var element = document.getElementsByName('" + name + "')[" + index + "];";
            script += "element.value = '" + value + "';";
            script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            script += "return element.value;";
            script += "}());";
            if (await execute(script, $"SetValueInElementByNameAsync('{name}', {index}, '{value}')") == "null")
            {
                SendMessageDebug($"SetValueInElementByNameAsync('{name}', {index}, '{value}')", $"SetValueInElementByNameAsync('{name}', {index}, '{value}')", Tester.FAILED, $"Не удалось найти или ввести значение в элемент по Name: {name} (Index: {index})", $"Could not find or enter a value in the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetValueInElementByNameAsync('{name}', {index}, '{value}')", $"SetValueInElementByNameAsync('{name}', {index}, '{value}')", PASSED, $"Значение '{value}' введено в элемент", $"The value '{value}' was entered into the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetValueInElementByTagAsync(string tag, int index, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += "var element = document.getElementsByTagName('" + tag + "')[" + index + "];";
            script += "element.value = '" + value + "';";
            script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            script += "return element.value;";
            script += "}());";
            if (await execute(script, $"SetValueInElementByTagAsync('{tag}', {index}, '{value}')") == "null")
            {
                SendMessageDebug($"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", $"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", Tester.FAILED, $"Не удалось найти или ввести значение в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter a value in the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", $"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", PASSED, $"Значение '{value}' введено в элемент", $"The value '{value}' was entered into the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetValueInElementAsync(string by, string locator, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "element.value = '" + value + "';";
            script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            script += "return element.value;";
            script += "}());";
            if (await execute(script, $"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")") == "null")
            {
                SendMessageDebug($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", $"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", Tester.FAILED, $"Не удалось найти или ввести значение в элемент по локатору: {locator}", $"Could not find or enter a value in the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", $"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", PASSED, $"Значение '{value}' введено в элемент", $"The value '{value}' was entered into the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetValueFromElementByIdAsync(string id)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); return element.value; }());";
                value = await execute(script, $"GetValueFromElementByIdAsync('{id}')");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetValueFromElementByIdAsync('{id}')", $"GetValueFromElementByIdAsync('{id}')", Tester.FAILED, $"Не удалось найти или получить данные из элемента с ID: {id}", $"Could not find or get data from an element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value.Length > 1) value = value.Substring(1, value.Length - 2);
                    SendMessageDebug($"GetValueFromElementByIdAsync('{id}')", $"GetValueFromElementByIdAsync('{id}')", Tester.PASSED, "Получено значение из элемента | " + value, "Got the value from the element | " + value, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetValueFromElementByIdAsync('{id}')", $"GetValueFromElementByIdAsync('{id}')", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByClassAsync(string _class, int index)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.value; }());";
                value = await execute(script, $"GetValueFromElementByClassAsync('{_class}', {index})");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetValueFromElementByClassAsync('{_class}', {index})", $"GetValueFromElementByClassAsync('{_class}', {index})", Tester.FAILED, $"Не удалось найти или получить данные из элемента по Class: {_class} (Index: {index})", $"Could not find or get data from the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value.Length > 1) value = value.Substring(1, value.Length - 2);
                    SendMessageDebug($"GetValueFromElementByClassAsync('{_class}', {index})", $"GetValueFromElementByClassAsync('{_class}', {index})", Tester.PASSED, "Получено значение из элемента | " + value, "Got the value from the element | " + value, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetValueFromElementByClassAsync('{_class}', {index})", $"GetValueFromElementByClassAsync('{_class}', {index})", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByNameAsync(string name, int index)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.value; }());";
                value = await execute(script, $"GetValueFromElementByNameAsync('{name}', {index})");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetValueFromElementByNameAsync('{name}', {index})", $"GetValueFromElementByNameAsync('{name}', {index})", Tester.FAILED, $"Не удалось найти или получить данные из элемента по Name: {name} (Index: {index})", $"Could not find or get data from the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value.Length > 1) value = value.Substring(1, value.Length - 2);
                    SendMessageDebug($"GetValueFromElementByNameAsync('{name}', {index})", $"GetValueFromElementByNameAsync('{name}', {index})", Tester.PASSED, "Получено значение из элемента | " + value, "Got the value from the element | " + value, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetValueFromElementByNameAsync('{name}', {index})", $"GetValueFromElementByNameAsync('{name}', {index})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByTagAsync(string tag, int index)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.value; }());";
                value = await execute(script, $"GetValueFromElementByTagAsync('{tag}', {index})");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetValueFromElementByTagAsync('{tag}', {index})", $"GetValueFromElementByTagAsync('{tag}', {index})", Tester.FAILED, $"Не удалось найти или получить данные из элемента по Tag: {tag} (Index: {index})", $"Could not find or get data from the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value.Length > 1) value = value.Substring(1, value.Length - 2);
                    SendMessageDebug($"GetValueFromElementByTagAsync('{tag}', {index})", $"GetValueFromElementByTagAsync('{tag}', {index})", Tester.PASSED, "Получено значение из элемента | " + value, "Got the value from the element | " + value, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetValueFromElementByTagAsync('{tag}', {index})", $"GetValueFromElementByTagAsync('{tag}', {index})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementAsync(string by, string locator)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); return element.value;";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.value;";
                script += "}());";
                value = await execute(script, $"GetValueFromElementAsync(\"{by}\", \"{locator}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или получить данные из элемента по локатору: {locator}", $"Could not find or get data from the element by Locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value.Length > 1) value = value.Substring(1, value.Length - 2);
                    SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Получено значение из элемента | " + value, "Got the value from the element | " + value, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task SetTextInElementByIdAsync(string id, string text)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            if (await execute(script, $"SetTextInElementByIdAsync('{id}', '{text}')") == "null")
            {
                SendMessageDebug($"SetTextInElementByIdAsync('{id}', '{text}')", $"SetTextInElementByIdAsync('{id}', '{text}')", Tester.FAILED, $"Не удалось найти или ввести текст в элемент с ID: {id}", $"Could not find or enter text in the element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetTextInElementByIdAsync('{id}', '{text}')", $"SetTextInElementByIdAsync('{id}', '{text}')", Tester.PASSED, $"Текст '{text}' введен в элемент", $"The text '{text}' was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetTextInElementByClassAsync(string _class, int index, string text)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            if (await execute(script, $"SetTextInElementByClassAsync('{_class}', {index}, '{text}')") == "null")
            {
                SendMessageDebug($"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", $"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", Tester.FAILED, $"Не удалось найти или ввести текст в элемент по Class: {_class} (Index: {index})", $"Could not find or enter text in the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", $"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", Tester.PASSED, $"Текст '{text}' введен в элемент", $"The text '{text}' was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetTextInElementByNameAsync(string name, int index, string text)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            if (await execute(script, $"SetTextInElementByNameAsync('{name}', {index}, '{text}')") == "null")
            {
                SendMessageDebug($"SetTextInElementByNameAsync('{name}', {index}, '{text}')", $"SetTextInElementByNameAsync('{name}', {index}, '{text}')", Tester.FAILED, $"Не удалось найти или ввести текст в элемент по Name: {name} (Index: {index})", $"Could not find or enter text in the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetTextInElementByNameAsync('{name}', {index}, '{text}')", $"SetTextInElementByNameAsync('{name}', {index}, '{text}')", Tester.PASSED, $"Текст '{text}' введен в элемент", $"The text '{text}' was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetTextInElementByTagAsync(string tag, int index, string text)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            if (await execute(script, $"SetTextInElementByTagAsync('{tag}', {index}, '{text}')") == "null")
            {
                SendMessageDebug($"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", $"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", Tester.FAILED, $"Не удалось найти или ввести текст в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter text in the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", $"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", Tester.PASSED, $"Текст '{text}' введен в элемент", $"The text '{text}' was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }
        public async Task SetTextInElementAsync(string by, string locator, string text)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            if (await execute(script, $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")") == "null")
            {
                SendMessageDebug($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", Tester.FAILED, $"Не удалось найти или ввести текст в элемент по локатору: {locator}", $"Could not find or enter text in the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", Tester.PASSED, $"Текст '{text}' введен в элемент", $"The text '{text}' was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }
        public async Task<string> GetTextFromElementByIdAsync(string id)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, $"GetTextFromElementByIdAsync('{id}')");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetTextFromElementByIdAsync('{id}')", $"GetTextFromElementByIdAsync('{id}')", Tester.FAILED, $"Не удалось найти или прочитать текст из элемента с ID: {id}", $"Could not find or read the text from the element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetTextFromElementByIdAsync('{id}')", $"GetTextFromElementByIdAsync('{id}')", COMPLETED, "Пустой текст из элемента", "Empty text from element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetTextFromElementByIdAsync('{id}')", $"GetTextFromElementByIdAsync('{id}')", Tester.PASSED, "Прочитан текст из элемента | " + value, "The text from the element has been read | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetTextFromElementByIdAsync('{id}')", $"GetTextFromElementByIdAsync('{id}')", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            
            return value;
        }

        public async Task<string> GetTextFromElementByClassAsync(string _class, int index)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, $"GetTextFromElementByClassAsync('{_class}', {index})");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetTextFromElementByClassAsync('{_class}', {index})", $"GetTextFromElementByClassAsync('{_class}', {index})", Tester.FAILED, $"Не удалось найти или прочитать текст из элемента по Class: {_class} (Index: {index})", $"Could not find or read the text from the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetTextFromElementByClassAsync('{_class}', {index})", $"GetTextFromElementByClassAsync('{_class}', {index})", COMPLETED, "Пустой текст из элемента", "Empty text from element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetTextFromElementByClassAsync('{_class}', {index})", $"GetTextFromElementByClassAsync('{_class}', {index})", Tester.PASSED, "Прочитан текст из элемента | " + value, "The text from the element has been read | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetTextFromElementByClassAsync('{_class}', {index})", $"GetTextFromElementByClassAsync('{_class}', {index})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetTextFromElementByNameAsync(string name, int index)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, $"GetTextFromElementByNameAsync('{name}', {index})");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetTextFromElementByNameAsync('{name}', {index})", $"GetTextFromElementByNameAsync('{name}', {index})", Tester.FAILED, $"Не удалось найти или прочитать текст из элемента по Name: {name} (Index: {index})", $"Could not find or read the text from the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetTextFromElementByNameAsync('{name}', {index})", $"GetTextFromElementByNameAsync('{name}', {index})", COMPLETED, "Пустой текст из элемента", "Empty text from element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetTextFromElementByNameAsync('{name}', {index})", $"GetTextFromElementByNameAsync('{name}', {index})", Tester.PASSED, "Прочитан текст из элемента | " + value, "The text from the element has been read | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetTextFromElementByNameAsync('{name}', {index})", $"GetTextFromElementByNameAsync('{name}', {index})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetTextFromElementByTagAsync(string tag, int index)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, $"GetTextFromElementByTagAsync('{tag}', {index})");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetTextFromElementByTagAsync('{tag}', {index})", $"GetTextFromElementByTagAsync('{tag}', {index})", Tester.FAILED, $"Не удалось найти или прочитать текст из элемента по Tag: {tag} (Index: {index})", $"Could not find or read the text from the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetTextFromElementByTagAsync('{tag}', {index})", $"GetTextFromElementByTagAsync('{tag}', {index})", COMPLETED, "Пустой текст из элемента", "Empty text from element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetTextFromElementByTagAsync('{tag}', {index})", $"GetTextFromElementByTagAsync('{tag}', {index})", Tester.PASSED, "Прочитан текст из элемента | " + value, "The text from the element has been read | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetTextFromElementByTagAsync('{tag}', {index})", $"GetTextFromElementByTagAsync('{tag}', {index})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetTextFromElementAsync(string by, string locator)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += "var element = document.querySelector(\"" + locator + "\"); ";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, $"GetTextFromElementAsync(\"{by}\", \"{locator}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или прочитать текст из элемента по локатору: {locator}", $"Could not find or read the text from the element by the locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", COMPLETED, "Пустой текст из элемента", "Empty text from element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Прочитан текст из элемента | " + value, "The text from the element has been read | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<int> GetCountElementsByClassAsync(string _class)
        {
            if (DefineTestStop() == true) return -1;

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "'); return element.length; }());";
            string result = await execute(script, $"GetCountElementsByIdAsync('{_class}')");
            if (result != "null" && result != null && result != "")
            {
                SendMessageDebug($"GetCountElementsByIdAsync('{_class}')", $"GetCountElementsByIdAsync('{_class}')", PASSED, $"Количество элементов {Int32.Parse(result)}", $"Amount of elements {Int32.Parse(result)}", IMAGE_STATUS_PASSED);
                return Int32.Parse(result);
            }
            else
            {
                SendMessageDebug($"GetCountElementsByIdAsync('{_class}')", $"GetCountElementsByIdAsync('{_class}')", FAILED, $"Не удалось найти или получить количество элементов по Class: {_class}", $"Could not find or get the amount of elements by Class: {_class}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return -1;
        }

        public async Task<int> GetCountElementsByNameAsync(string name)
        {
            if (DefineTestStop() == true) return -1;

            string script = "(function(){ var element = document.getElementsByName('" + name + "'); return element.length; }());";
            string result = await execute(script, $"GetCountElementsByNameAsync('{name}')");
            if (result != "null" && result != null && result != "")
            {
                SendMessageDebug($"GetCountElementsByNameAsync('{name}')", $"GetCountElementsByNameAsync('{name}')", PASSED, $"Количество элементов {Int32.Parse(result)}", $"Amount of elements {Int32.Parse(result)}", IMAGE_STATUS_PASSED);
                return Int32.Parse(result);
            }
            else
            {
                SendMessageDebug($"GetCountElementsByNameAsync('{name}')", $"GetCountElementsByNameAsync('{name}')", FAILED, $"Не удалось найти или получить количество элементов по Name: {name}", $"Could not find or get the amount of elements by Name: {name}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return -1;
        }

        public async Task<int> GetCountElementsByTagAsync(string tag)
        {
            if (DefineTestStop() == true) return -1;

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "'); return element.length; }());";
            string result = await execute(script, $"GetCountElementsByTagAsync('{tag}')");
            if (result != "null" && result != null && result != "")
            {
                SendMessageDebug($"GetCountElementsByTagAsync('{tag}')", $"GetCountElementsByTagAsync('{tag}')", PASSED, $"Количество элементов {Int32.Parse(result)}", $"Amount of elements {Int32.Parse(result)}", IMAGE_STATUS_PASSED);
                return Int32.Parse(result);
            }
            else
            {
                SendMessageDebug($"GetCountElementsByTagAsync('{tag}')", $"GetCountElementsByTagAsync('{tag}')", FAILED, $"Не удалось найти или получить количество элементов по Tag: {tag}", $"Could not find or get the amount of elements by Tag: {tag}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return -1;
        }

        public async Task<int> GetCountElementsAsync(string by, string locator)
        {
            if (DefineTestStop() == true) return -1;

            string script = "(function(){";
            if (by == BY_CSS) script += "var element = document.querySelectorAll(\"" + locator + "\"); return element.length;";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null); return element.snapshotLength;";
            script += "}());";
            string result = await execute(script, $"GetCountElementsAsync(\"{by}\", \"{locator}\")");
            if (result != "null" && result != null && result != "")
            {
                SendMessageDebug($"GetCountElementsAsync(\"{by}\", \"{locator}\")", $"GetCountElementsAsync(\"{by}\", \"{locator}\")", PASSED, $"Количество элементов {Int32.Parse(result)}", $"Amount of elements {Int32.Parse(result)}", IMAGE_STATUS_PASSED);
                return Int32.Parse(result);
            }
            else
            {
                SendMessageDebug($"GetCountElementsAsync(\"{by}\", \"{locator}\")", $"GetCountElementsAsync(\"{by}\", \"{locator}\")", FAILED, $"Не удалось найти или получить количество элементов по локатору: {locator}", $"Couldn't find or get the amount of elements by locator: {locator}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return -1;
        }

        public async Task ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)
        {
            if (DefineTestStop() == true) return;

            try
            {
                string script = "(function(){";
                if (by == BY_CSS)
                {
                    script += $"var element = document.querySelector(\"{locator}\");";
                    if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'}); return element;";
                    else script += "element.scrollIntoView(); return element;";
                }
                else if (by == BY_XPATH)
                {
                    script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                    if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'}); return element;";
                    else script += "element.scrollIntoView(); return element;";
                }
                script += "}());";
                if (await execute(script, $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})") == "null")
                {
                    SendMessageDebug($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", Tester.FAILED, "Не удалось прокрутить к элементу", "Failed to fasten to the element", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    SendMessageDebug($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", Tester.PASSED, "Выполнена прокрутка (scroll) к элементу", "Scrolled to the element - completed", Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetTitleAsync()
        {
            if (DefineTestStop() == true) return "";

            string script = "(function(){ var element = document.querySelector('title'); return element.innerText; }());";
            string value = await execute(script, $"GetTitleAsync()");
            if (value == "null" || value == null)
            {
                SendMessageDebug($"GetTitleAsync()", $"GetTitleAsync()", Tester.FAILED, "Не удалось найти заголовок на странице", "Couldn't find the title on the page", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"GetTitleAsync()", $"GetTitleAsync()", Tester.PASSED, "Прочитан текст из заголовка | " + value, "The text from the title has been read | " + value, Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetAttributeFromElementByIdAsync(string id, string attribute)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, $"GetAttributeFromElementByIdAsync('{id}', '{attribute}')");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", $"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", Tester.FAILED, $"Не удалось найти или получить аттрибут из элемента с ID: {id}", $"Couldn't find or get attribute from element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetAttributeFromElementByIdAsync('{id}')", $"GetAttributeFromElementByIdAsync('{id}')", COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", $"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", Tester.PASSED, $"Получено значение из аттрибута '{attribute}' | {value}", $"The value was obtained from the attribute '{attribute}' | {value}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", $"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetAttributeFromElementByClassAsync(string _class, int index, string attribute)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, $"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", $"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", Tester.FAILED, $"Не удалось найти или получить аттрибут из элемента по Class: {_class} (Index: {index})", $"Could not find or get an attribute from an element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", $"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", $"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", Tester.PASSED, $"Получено значение из аттрибута '{attribute}' | {value}", $"The value was obtained from the attribute '{attribute}' | {value}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", $"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetAttributeFromElementByNameAsync(string name, int index, string attribute)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, $"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", $"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", Tester.FAILED, $"Не удалось найти или получить аттрибут из элемента по Name: {name} (Index: {index})", $"Could not find or get an attribute from an element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", $"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", $"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", Tester.PASSED, $"Получено значение из аттрибута '{attribute}' | {value}", $"The value was obtained from the attribute '{attribute}' | {value}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", $"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetAttributeFromElementByTagAsync(string tag, int index, string attribute)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, $"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", $"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", Tester.FAILED, $"Не удалось найти или получить аттрибут из элемента по Tag: {tag} (Index: {index})", $"Could not find or get an attribute from an element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", $"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", $"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", Tester.PASSED, $"Получено значение из аттрибута '{attribute}' | {value}", $"The value was obtained from the attribute '{attribute}' | {value}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", $"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetAttributeFromElementAsync(string by, string locator, string attribute)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"return element.getAttribute('{attribute}');";
                script += "}());";
                value = await execute(script, $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED, $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}", $"Couldn't find or get attribute from element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.PASSED, $"Получено значение из аттрибута '{attribute}' | {value}", $"The value was obtained from the attribute '{attribute}' | {value}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<List<string>> GetAttributeFromElementsByClassAsync(string _class, string attribute)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}');";
            script += "var json = '[';";
            script += "var attr = '';";
            script += "for (var i = 0; i < element.length; i++){";
            script += $"attr = element[i].getAttribute('{attribute}');";
            script += "json += '\"' + attr + '\",';";
            script += "}";
            script += "json = json.slice(0, -1);";
            script += "json += ']';";
            script += "return json;";
            script += "}());";
            string result = await execute(script, $"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", $"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", $"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", $"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", FAILED, $"Не удалось найти или получить аттрибуты из элементов по Class: {_class}", $"Could not find or get attributes from elements by Class: {_class}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsByNameAsync(string name, string attribute)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}');";
            script += "var json = '[';";
            script += "var attr = '';";
            script += "for (var i = 0; i < element.length; i++){";
            script += $"attr = element[i].getAttribute('{attribute}');";
            script += "json += '\"' + attr + '\",';";
            script += "}";
            script += "json = json.slice(0, -1);";
            script += "json += ']';";
            script += "return json;";
            script += "}());";
            string result = await execute(script, $"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", $"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", $"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", $"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", FAILED, $"Не удалось найти или получить аттрибуты из элементов по Name: {name}", $"Could not find or get attributes from elements by Name: {name}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsByTagAsync(string tag, string attribute)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}');";
            script += "var json = '[';";
            script += "var attr = '';";
            script += "for (var i = 0; i < element.length; i++){";
            script += $"attr = element[i].getAttribute('{attribute}');";
            script += "json += '\"' + attr + '\",';";
            script += "}";
            script += "json = json.slice(0, -1);";
            script += "json += ']';";
            script += "return json;";
            script += "}());";
            string result = await execute(script, $"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", $"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", $"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", $"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", FAILED, $"Не удалось найти или получить аттрибуты из элементов по Tag: {tag}", $"Could not find or get attributes from elements by Tag: {tag}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsAsync(string by, string locator, string attribute)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            if (by == BY_CSS)
            {
                script += $"var element = document.querySelectorAll(\"{locator}\");";
                script += "var json = '[';";
                script += "var attr = '';";
                script += "var count = element.length;";
                script += "for (var i = 0; i < count; i++){";
                script += $"attr = element[i].getAttribute('{attribute}');";
                script += "json += '\"' + attr + '\",';";
                script += "}";
                script += "json = json.slice(0, -1);";
                script += "json += ']';";
                script += "return json;";
            }
            else if (by == BY_XPATH)
            {
                script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);";
                script += "var json = '[';";
                script += "var attr = '';";
                script += "var count = element.snapshotLength;";
                script += "for (var i = 0; i < count; i++){";
                script += $"attr = element.snapshotItem(i).getAttribute('{attribute}');";
                script += "json += '\"' + attr + '\",';";
                script += "}";
                script += "json = json.slice(0, -1);";
                script += "json += ']';";
                script += "return json;";
            }
            script += "}());";
            string result = await execute(script, $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", FAILED, $"Не удалось найти или получить аттрибуты из элементов по локатору: {locator}", $"Couldn't find or get attributes from elements by locator: {locator}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            return Json_Array;
        }

        public async Task SetAttributeInElementByIdAsync(string id, string attribute, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            if (await execute(script, $"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')") == "null")
            {
                SendMessageDebug($"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", $"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", Tester.FAILED, $"Не удалось найти или ввести аттрибут в элемент с ID: {id}", $"Could not find or enter attribute in element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", $"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", Tester.PASSED, $"Аттрибут '{attribute}' со значением '{value}' добавлен в элемент", $"Attribute '{attribute}' with a value of '{value}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetAttributeInElementByClassAsync(string _class, int index, string attribute, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            if (await execute(script, $"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')") == "null")
            {
                SendMessageDebug($"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", Tester.FAILED, $"Не удалось найти или ввести аттрибут в элемент по Class: {_class} (Index: {index})", $"Could not find or enter an attribute in an element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", Tester.PASSED, $"Аттрибут '{attribute}' со значением '{value}' добавлен в элемент", $"Attribute '{attribute}' with a value of '{value}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetAttributeInElementByNameAsync(string name, int index, string attribute, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            if (await execute(script, $"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')") == "null")
            {
                SendMessageDebug($"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", Tester.FAILED, $"Не удалось найти или ввести аттрибут в элемент по Name: {name} (Index: {index})", $"Could not find or enter an attribute in an element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", Tester.PASSED, $"Аттрибут '{attribute}' со значением '{value}' добавлен в элемент", $"Attribute '{attribute}' with a value of '{value}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetAttributeInElementByTagAsync(string tag, int index, string attribute, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            if (await execute(script, $"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')") == "null")
            {
                SendMessageDebug($"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", Tester.FAILED, $"Не удалось найти или ввести аттрибут в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter an attribute in an element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", Tester.PASSED, $"Аттрибут '{attribute}' со значением '{value}' добавлен в элемент", $"Attribute '{attribute}' with a value of '{value}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetAttributeInElementAsync(string by, string locator, string attribute, string value)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            if (await execute(script, $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")") == "null")
            {
                SendMessageDebug($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.FAILED, $"Не удалось найти или ввести аттрибут в элемент по локатору: {locator}", $"Could not find or enter attribute in element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.PASSED, $"Аттрибут '{attribute}' со значением '{value}' добавлен в элемент", $"Attribute '{attribute}' with a value of '{value}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<List<string>> SetAttributeInElementsByClassAsync(string _class, string attribute, string value)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}');";
            script += "var json = '[';";
            script += "var attr = '';";
            script += "for (var i = 0; i < element.length; i++){";
            script += $"element[i].setAttribute('{attribute}', '{value}');";
            script += $"attr = element[i].getAttribute('{attribute}');";
            script += "json += '\"' + attr + '\",';";
            script += "}";
            script += "json = json.slice(0, -1);";
            script += "json += ']';";
            script += "return json;";
            script += "}());";

            string result = await execute(script, $"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", $"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", PASSED, $"Аттрибут '{attribute}' со значением '{value}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{value}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", $"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", $"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", FAILED, $"Не удалось найти или добавить аттрибуты в элементы по Class: {_class}", $"Could not find or add attribute to elements by Class: {_class}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }

            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsByNameAsync(string name, string attribute, string value)
        {
            //int step = SendMessageDebug(, PROCESS, "Добавление аттрибута в элементы", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}');";
            script += "var json = '[';";
            script += "var attr = '';";
            script += "for (var i = 0; i < element.length; i++){";
            script += $"element[i].setAttribute('{attribute}', '{value}');";
            script += $"attr = element[i].getAttribute('{attribute}');";
            script += "json += '\"' + attr + '\",';";
            script += "}";
            script += "json = json.slice(0, -1);";
            script += "json += ']';";
            script += "return json;";
            script += "}());";
            string result = await execute(script, $"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", $"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", PASSED, $"Аттрибут '{attribute}' со значением '{value}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{value}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", $"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", $"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", FAILED, $"Не удалось найти или добавить аттрибут в элементы по Name: {name}", $"Could not find or add attribute to elements by Name: {name}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }

            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsByTagAsync(string tag, string attribute, string value)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}');";
            script += "var json = '[';";
            script += "var attr = '';";
            script += "for (var i = 0; i < element.length; i++){";
            script += $"element[i].setAttribute('{attribute}', '{value}');";
            script += $"attr = element[i].getAttribute('{attribute}');";
            script += "json += '\"' + attr + '\",';";
            script += "}";
            script += "json = json.slice(0, -1);";
            script += "json += ']';";
            script += "return json;";
            script += "}());";
            string result = await execute(script, $"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", $"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", PASSED, $"Аттрибут '{attribute}' со значением '{value}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{value}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", $"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", $"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", FAILED, $"Не удалось найти или добавить аттрибут в элементы по Tag: {tag}", $"Could not find or add attribute to elements by Tag: {tag}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }

            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsAsync(string by, string locator, string attribute, string value)
        {
            if (DefineTestStop() == true) return null;

            string script = "(function(){";
            if (by == BY_CSS)
            {
                script += $"var element = document.querySelectorAll(\"{locator}\");";
                script += "var json = '[';";
                script += "var attr = '';";
                script += "for (var i = 0; i < element.length; i++){";
                script += $"element[i].setAttribute('{attribute}', '{value}');";
                script += $"attr = element[i].getAttribute('{attribute}');";
                script += "json += '\"' + attr + '\",';";
                script += "}";
                script += "json = json.slice(0, -1);";
                script += "json += ']';";
                script += "return json;";
            }
            else if (by == BY_XPATH)
            {
                script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);";
                script += "var json = '[';";
                script += "var attr = '';";
                script += "var count = element.snapshotLength;";
                script += "for (var i = 0; i < count; i++){";
                script += $"element.snapshotItem(i).setAttribute('{attribute}', '{value}');";
                script += $"attr = element.snapshotItem(i).getAttribute('{attribute}');";
                script += "json += '\"' + attr + '\",';";
                script += "}";
                script += "json = json.slice(0, -1);";
                script += "json += ']';";
                script += "return json;";
            }
            script += "}());";
            string result = await execute(script, $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", PASSED, $"Аттрибут '{attribute}' со значением '{value}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{value}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", FAILED, $"Не удалось найти или добавить аттрибут в элементы по локатору: {locator}", $"Couldn't find or add attribute to elements by locator: {locator}", IMAGE_STATUS_FAILED);
                TestStopAsync();
            }

            return Json_Array;
        }

        public async Task<string> GetHtmlFromElementByClassAsync(string _class, int index)
        {
            if (DefineTestStop() == true) return "";

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, $"GetHtmlFromElementByClassAsync('{_class}', '{index}')");
            if (value == "null" || value == null)
            {
                SendMessageDebug($"GetHtmlFromElementByClassAsync('{_class}', '{index}')", $"GetHtmlFromElementByClassAsync('{_class}', '{index}')", Tester.FAILED, $"Не удалось найти или получить html из элемента Class: {_class} (Index: {index})", $"Could not find or get html from the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                value = value.Replace("\\u003C", "<");
                value = value.Replace("\\u003E", ">");
                SendMessageDebug($"GetHtmlFromElementByClassAsync('{_class}', '{index}')", $"GetHtmlFromElementByClassAsync('{_class}', '{index}')", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetHtmlFromElementAsync(string by, string locator)
        {
            if (DefineTestStop() == true) return "";

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); return element.outerHTML;";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "return element.outerHTML;";
            script += "}());";
            string value = await execute(script, $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")");
            if (value == "null" || value == null)
            {
                SendMessageDebug($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или получить html из элемента по локатору: {locator}", $"Couldn't find or get html from the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                value = value.Replace("\\u003C", "<");
                value = value.Replace("\\u003E", ">");
                SendMessageDebug($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetHtmlFromElementByIdAsync(string id)
        {
            if (DefineTestStop() == true) return "";

            string script = "(function(){ var element = document.getElementById('" + id + "'); return element.outerHTML; }());";
            string value = await execute(script, $"GetHtmlFromElementByIdAsync('{id}')");
            if (value == "null" || value == null)
            {
                SendMessageDebug($"GetHtmlFromElementByIdAsync('{id}')", $"GetHtmlFromElementByIdAsync('{id}')", Tester.FAILED, $"Не удалось найти или получить html из элемента с ID: {id}", $"Couldn't find or get html from an element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                value = value.Replace("\\u003C", "<");
                value = value.Replace("\\u003E", ">");
                SendMessageDebug($"GetHtmlFromElementByIdAsync('{id}')", $"GetHtmlFromElementByIdAsync('{id}')", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetHtmlFromElementByNameAsync(string name, int index)
        {
            if (DefineTestStop() == true) return "";

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, $"GetHtmlFromElementByNameAsync('{name}', '{index}')");
            if (value == "null" || value == null)
            {
                SendMessageDebug($"GetHtmlFromElementByNameAsync('{name}', '{index}')", $"GetHtmlFromElementByNameAsync('{name}', '{index}')", Tester.FAILED, $"Не удалось найти или получить html из элемента Name: {name} (Index: {index})", $"Couldn't find or get html from the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                value = value.Replace("\\u003C", "<");
                value = value.Replace("\\u003E", ">");
                SendMessageDebug($"GetHtmlFromElementByNameAsync('{name}', '{index}')", $"GetHtmlFromElementByNameAsync('{name}', '{index}')", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetHtmlFromElementByTagAsync(string tag, int index)
        {
            if (DefineTestStop() == true) return "";

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, $"GetHtmlFromElementByTagAsync('{tag}', '{index}')");
            if (value == "null" || value == null)
            {
                SendMessageDebug($"GetHtmlFromElementByTagAsync('{tag}', '{index}')", $"GetHtmlFromElementByTagAsync('{tag}', '{index}')", Tester.FAILED, $"Не удалось найти или получить html из элемента Tag: {tag} (Index: {index})", $"Couldn't find or get html from the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                value = value.Replace("\\u003C", "<");
                value = value.Replace("\\u003E", ">");
                SendMessageDebug($"GetHtmlFromElementByTagAsync('{tag}', '{index}')", $"GetHtmlFromElementByTagAsync('{tag}', '{index}')", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task SetHtmlInElementByClassAsync(string _class, int index, string html)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            if (await execute(script, $"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')") == "null")
            {
                SendMessageDebug($"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", $"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", Tester.FAILED, $"Не удалось найти или ввести html в элемент по Class: {_class} (Index: {index})", $"Could not find or enter html in the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", $"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", Tester.PASSED, $"В элемент введен html {html}", $"Html {html} has been added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetHtmlInElementAsync(string by, string locator, string html)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            if (await execute(script, $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")") == "null")
            {
                SendMessageDebug($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", Tester.FAILED, $"Не удалось найти или ввести html в элемент по локатору: {locator}", $"Could not find or enter html into the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", Tester.PASSED, $"В элемент введен html {html}", $"Html {html} has been added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetHtmlInElementByIdAsync(string id, string html)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            if (await execute(script, $"SetHtmlInElementByIdAsync('{id}', '{html}')") == "null")
            {
                SendMessageDebug($"SetHtmlInElementByIdAsync('{id}', '{html}')", $"SetHtmlInElementByIdAsync('{id}', '{html}')", Tester.FAILED, $"Не удалось найти или ввести html в элемент с ID: {id}", $"Could not find or enter html in the element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetHtmlInElementByIdAsync('{id}', '{html}')", $"SetHtmlInElementByIdAsync('{id}', '{html}')", Tester.PASSED, $"В элемент введен html {html}", $"Html {html} has been added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetHtmlInElementByNameAsync(string name, int index, string html)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            if (await execute(script, $"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')") == "null")
            {
                SendMessageDebug($"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", $"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", Tester.FAILED, $"Не удалось найти или ввести html в элемент по Name: {name} (Index: {index})", $"Could not find or enter html in the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", $"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", Tester.PASSED, $"В элемент введен html {html}", $"Html {html} has been added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetHtmlInElementByTagAsync(string tag, int index, string html)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            if (await execute(script, $"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')") == "null")
            {
                SendMessageDebug($"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", $"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", Tester.FAILED, $"Не удалось найти или ввести html в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter html in the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", $"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", Tester.PASSED, $"В элемент введен html {html}", $"Html {html} has been added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<bool> IsClickableElementAsync(string by, string locator)
        {
            if (DefineTestStop() == true) return false;

            bool clickable = false;
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "if((element.getAttribute('onclick')!=null)||(element.getAttribute('href')!=null)) return true;";
                script += "return false;";
                script += "}());";
                string result = await executeJS(script);
                if (result != "null" && result != null && result == "true") clickable = true;
                else clickable = false;
                SendMessageDebug($"IsClickableElementAsync(\"{by}\", \"{locator}\")", $"IsClickableElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, $"Определена кликадельность элемента: {result}", $"The clickability of the element was determined: {result}", Tester.IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"IsClickableElementAsync(\"{by}\", \"{locator}\")", $"IsClickableElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return clickable;
        }





        /*
         * Методы для работы с REST запросами =======================================================
         * https://stackoverflow.com/questions/9620278/how-do-i-make-calls-to-a-rest-api-using-c
         * https://stackoverflow.com/questions/48977317/httpclient-post-with-parameters-in-body
         * https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class
         * https://zetcode.com/csharp/httpclient/
         * https://jsonplaceholder.typicode.com/
         */
        public async Task<string> RestGetAsync(string url, TimeSpan timeout, string charset = "UTF-8")
        {
            if (DefineTestStop() == true) return null;

            string result = null;
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                client.Timeout = timeout;
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("charset", charset);
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    SendMessageDebug($"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", PASSED, "Get Rest запрос - успешно выполнен", "Get Rest request - completed successfully", IMAGE_STATUS_PASSED);
                }
                else
                {
                    SendMessageDebug($"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", FAILED, "Get Rest не выполнен " + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Get Rest failed " + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task<string> RestGetBasicAuthAsync(string login, string pass, string url, TimeSpan timeout, string charset = "UTF-8")
        {
            if (DefineTestStop() == true) return null;

            string result = null;
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;
                byte[] authToken = Encoding.ASCII.GetBytes($"{login}:{pass}");

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                client.Timeout = timeout;
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("charset", charset);
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    SendMessageDebug($"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", PASSED, "Get Rest запрос - успешно выполнен", "Get Rest request - completed successfully", IMAGE_STATUS_PASSED);
                }
                else
                {
                    SendMessageDebug($"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", FAILED, "Get Rest не выполнен " + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Get Rest failed " + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task<int> RestGetStatusCodeAsync(string url)
        {
            int result = 0;
            if (DefineTestStop() == true) return result;
            
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("charset", "UTF-8");
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                HttpResponseMessage response = await client.GetAsync(url);
                result = (int)response.StatusCode;

                SendMessageDebug($"RestGetStatusCodeAsync(\"{url}\")", $"RestGetStatusCodeAsync(\"{url}\")", PASSED, "Get Rest запрос выполнен. Результат: " + result.ToString(), "Get Rest request completed. Result: " + result.ToString(), IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"RestGetStatusCodeAsync(\"{url}\")", $"RestGetStatusCodeAsync(\"{url}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task<string> RestPostAsync(string url, string json, TimeSpan timeout, string charset = "UTF-8")
        {
            if (DefineTestStop() == true) return null;

            string result = null;
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
                client.Timeout = timeout;
                client.DefaultRequestHeaders.Add("charset", charset);
                client.DefaultRequestHeaders.Add("User-Agent", userAgent);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    SendMessageDebug($"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", $"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", PASSED, "Post Rest запрос успешно выполнен" + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Post Rest request completed successfully" + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_PASSED);
                }
                else
                {
                    SendMessageDebug($"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", $"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", FAILED, "Post Rest не выполнен" + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Post Rest failed" + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", $"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }


        /* Методы для замера метрик ======================================================= */
        public async Task<DateTime> TimerStart()
        {
            DateTime start = default;
            try
            {
                start = DateTime.Now;
                SendMessageDebug("TimerStart()", "TimerStart()", COMPLETED, $"Таймер запущен {start}", $"The timer is running {start}", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug("TimerStart()", "TimerStart()", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return start;
        }

        public async Task<TimeSpan> TimerStop(DateTime start)
        {
            DateTime stop = default;
            TimeSpan result = default;
            try
            {
                stop = DateTime.Now;
                result = (DateTime)stop - (DateTime)start;
                SendMessageDebug("TimerStop()", "TimerStop()", COMPLETED, $"Таймер остановлен {stop} (затраченное время: {result})", $"Timer stopped {stop} (elapsed time: {result})", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                SendMessageDebug("TimerStop()", "TimerStop()", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task<string> GetStyleFromElementAsync(string by, string locator, string property)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += "var element = document.querySelector(\"" + locator + "\"); ";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                script += "return style; ";
                script += "}());";
                value = await execute(script, $"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось прочитать стиль '{property}' из элемента по локатору: {locator}", $"Could not read the style '{property}' from the element by the locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\")", COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Стиль из элемента прочитан | " + value, "Style from the read element | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetStyleFromElementByIdAsync(string id, string property)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); ";
                script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                script += "return style; ";
                script += "}());";
                value = await execute(script, $"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", $"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", Tester.FAILED, $"Не удалось найти или прочитать стиль '{property}' из элемента с ID: {id}", $"Could not find or read the style '{property}' from the element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", $"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", $"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", Tester.PASSED, "Стиль из элемента прочитан | " + value, "Style from the read element | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", $"GetStyleFromElementByIdAsync(\"{id}\", \"{property}\")", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetStyleFromElementByClassAsync(string _class, int index, string property)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; ";
                script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                script += "return style; ";
                script += "}());";
                value = await execute(script, $"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", $"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", Tester.FAILED, $"Не удалось найти или прочитать стиль '{property}' из элемента по Class: {_class} (Index: {index})", $"Could not find or read the style '{property}' from the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", $"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", $"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", Tester.PASSED, "Стиль из элемента прочитан | " + value, "Style from the read element | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", $"GetStyleFromElementByClassAsync('{_class}', {index}, \"{property}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetStyleFromElementByNameAsync(string name, int index, string property)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; ";
                script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                script += "return style; ";
                script += "}());";
                value = await execute(script, $"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", $"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", Tester.FAILED, $"Не удалось найти или прочитать стиль '{property}' из элемента по Name: {name} (Index: {index})", $"Could not find or read the style '{property}' from the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", $"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", $"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", Tester.PASSED, "Стиль из элемента прочитан | " + value, "Style from the read element | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", $"GetStyleFromElementByNameAsync('{name}', {index}, \"{property}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task<string> GetStyleFromElementByTagAsync(string tag, int index, string property)
        {
            if (DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; ";
                script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                script += "return style; ";
                script += "}());";
                value = await execute(script, $"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")");
                if (value == "null" || value == null)
                {
                    SendMessageDebug($"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", $"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", Tester.FAILED, $"Не удалось найти или прочитать стиль '{property}' из элемента по Tag: {tag} (Index: {index})", $"Could not find or read the style '{property}' from the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    if (value == "") SendMessageDebug($"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", $"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        SendMessageDebug($"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", $"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", Tester.PASSED, "Стиль из элемента прочитан | " + value, "Style from the read element | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", $"GetStyleFromElementByTagAsync('{tag}', {index}, \"{property}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task SetStyleInElementAsync(string by, string locator, string cssText)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.style.cssText = '{cssText}';";
            script += "return element;";
            script += "}());";
            if (await execute(script, $"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")") == "null")
            {
                SendMessageDebug($"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", $"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести стиль в элемент по локатору: {locator}", $"Could not find or enter style in the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", $"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", PASSED, $"Стиль {cssText} введен в элемент", $"The style {cssText} is entered in the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetStyleInElementByIdAsync(string id, string cssText)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.style.cssText = '{cssText}';";
            script += "return element;";
            script += "}());";
            if (await execute(script, $"SetStyleInElementByIdAsync(\"{id}\", \"{cssText}\")") == "null")
            {
                SendMessageDebug($"SetStyleInElementByIdAsync(\"{id}\", \"{cssText}\")", $"SetStyleInElementByIdAsync(\"{id}\", \"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести стиль в элемент с ID: {id}", $"Could not find or enter style in the element with ID: {id}", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetStyleInElementByIdAsync(\"{id}\", \"{cssText}\")", $"SetStyleInElementByIdAsync(\"{id}\", \"{cssText}\")", PASSED, $"Стиль {cssText} введен в элемент", $"The style {cssText} is entered in the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetStyleInElementByClassAsync(string _class, int index, string cssText)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.style.cssText = '{cssText}';";
            script += "return element;";
            script += "}());";
            if (await execute(script, $"SetStyleInElementByClassAsync(\"{_class}\", {index}, \"{cssText}\")") == "null")
            {
                SendMessageDebug($"SetStyleInElementByClassAsync(\"{_class}\", {index}, \"{cssText}\")", $"SetStyleInElementByClassAsync(\"{_class}\", {index}, \"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести стиль в элемент по Class: {_class} (Index: {index})", $"Could not find or enter style in the element by Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetStyleInElementByClassAsync(\"{_class}\", {index}, \"{cssText}\")", $"SetStyleInElementByClassAsync(\"{_class}\", {index}, \"{cssText}\")", PASSED, $"Стиль {cssText} введен в элемент", $"The style {cssText} is entered in the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetStyleInElementByNameAsync(string name, int index, string cssText)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.style.cssText = '{cssText}';";
            script += "return element;";
            script += "}());";
            if (await execute(script, $"SetStyleInElementByNameAsync(\"{name}\", {index}, \"{cssText}\")") == "null")
            {
                SendMessageDebug($"SetStyleInElementByNameAsync(\"{name}\", {index}, \"{cssText}\")", $"SetStyleInElementByNameAsync(\"{name}\", {index}, \"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести стиль в элемент по Name: {name} (Index: {index})", $"Could not find or enter style in the element by Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetStyleInElementByNameAsync(\"{name}\", {index}, \"{cssText}\")", $"SetStyleInElementByNameAsync(\"{name}\", {index}, \"{cssText}\")", PASSED, $"Стиль {cssText} введен в элемент", $"The style {cssText} is entered in the element", IMAGE_STATUS_PASSED);
            }
        }

        public async Task SetStyleInElementByTagAsync(string tag, int index, string cssText)
        {
            if (DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.style.cssText = '{cssText}';";
            script += "return element;";
            script += "}());";
            if (await execute(script, $"SetStyleInElementByTagAsync(\"{tag}\", {index}, \"{cssText}\")") == "null")
            {
                SendMessageDebug($"SetStyleInElementByTagAsync(\"{tag}\", {index}, \"{cssText}\")", $"SetStyleInElementByTagAsync(\"{tag}\", {index}, \"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести текст в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter text in the element by Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
            }
            else
            {
                SendMessageDebug($"SetStyleInElementByTagAsync(\"{tag}\", {index}, \"{cssText}\")", $"SetStyleInElementByTagAsync(\"{tag}\", {index}, \"{cssText}\")", PASSED, $"Стиль {cssText} введен в элемент", $"The style {cssText} is entered in the element", IMAGE_STATUS_PASSED);
            }
        }

        /*
         * Методы для работы с файлами ===========================================================
         */

        public async Task<string> FileReadAsync(string encoding, string filename)
        {
            if (DefineTestStop() == true) return "";

            string content = "";
            try
            {
                StreamReader reader;
                if (encoding == Tester.DEFAULT)
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                else if (encoding == Tester.UTF8)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(false));
                }
                else if (encoding == Tester.UTF8BOM)
                {
                    reader = new StreamReader(filename, new UTF8Encoding(true));
                }
                else if (encoding == Tester.WINDOWS1251)
                {
                    reader = new StreamReader(filename, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    reader = new StreamReader(filename, Encoding.Default);
                }
                content = await reader.ReadToEndAsync();
                reader.Close();
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FileReadAsync(\"{encoding}\"', \"{filename}\")", $"FileReadAsync(\"{encoding}\"', \"{filename}\")", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (content == "") SendMessageDebug($"FileReadAsync(\"{encoding}\"', \"{filename}\")", $"FileReadAsync(\"{encoding}\"', \"{filename}\")", WARNING, "Не удалось прочитать файл (или файл пустой)", "Could not read the file (or the file is empty)", IMAGE_STATUS_WARNING);
            else SendMessageDebug($"FileReadAsync(\"{encoding}\"', \"{filename}\")", $"FileReadAsync(\"{encoding}\"', \"{filename}\")", PASSED, "Файл прочитан", "The file has been read", IMAGE_STATUS_PASSED);

            return content;
        }

        public async Task FileWriteAsync(string content, string encoding, string filename)
        {
            if (DefineTestStop() == true) return;
            try
            {
                StreamWriter writer;
                if (encoding == Tester.DEFAULT)
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                else if (encoding == Tester.UTF8)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(false));
                }
                else if (encoding == Tester.UTF8BOM)
                {
                    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                }
                else if (encoding == Tester.WINDOWS1251)
                {
                    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                }
                else
                {
                    writer = new StreamWriter(filename, false, Encoding.Default);
                }
                await writer.WriteAsync(content);
                writer.Close();

                SendMessageDebug($"FileWriteAsync(\"...\", \"{encoding}\"', \"{filename}\")", $"FileWriteAsync(\"...\", \"{encoding}\"', \"{filename}\")", PASSED, "Файл сохранён", "File saved", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FileWriteAsync(\"...\", \"{encoding}\"', \"{filename}\")", $"FileWriteAsync(\"...\", \"{encoding}\"', \"{filename}\")", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task FileDownloadAsync(string fileURL, string filename, int waitingSec = 60)
        {
            if (DefineTestStop() == true) return;
            try
            {
                int waiting = 0;
                WebClient webClient = new WebClient();

                webClient.DownloadFileAsync(new Uri(fileURL), filename);
                while (webClient.IsBusy)
                {
                    waiting++;
                    await this.WaitAsync(1);
                    if (this.DefineTestStop() == true) break;
                    if (waiting == waitingSec) break;
                }

                if (DefineTestStop() == true) return;

                if (File.Exists(filename) == true)
                {
                    SendMessageDebug($"FileDownloadAsync(\"{fileURL}\"', \"{filename}\", {waitingSec.ToString()})", $"FileDownloadAsync(\"{fileURL}\"', \"{filename}\", {waitingSec.ToString()})", PASSED, "Скачивание файла - завершено", "File download - completed", IMAGE_STATUS_PASSED);
                }
                else
                {
                    SendMessageDebug($"FileDownloadAsync(\"{fileURL}\"', \"{filename}\", {waitingSec.ToString()})", $"FileDownloadAsync(\"{fileURL}\"', \"{filename}\", {waitingSec.ToString()})", FAILED, "Неудалось скачать файл", "Failed to download file", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FileDownloadAsync(\"{fileURL}\"', \"{filename}\", {waitingSec.ToString()})", $"FileDownloadAsync(\"{fileURL}\"', \"{filename}\", {waitingSec.ToString()})", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> FileGetHashMD5Async(string filename)
        {
            if (DefineTestStop() == true) return "";

            try
            {
                using (var md5 = System.Security.Cryptography.MD5.Create())
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        string result = BitConverter.ToString(hash).Replace("-", "");
                        SendMessageDebug($"FileGetHashMD5Async(\"{filename}\")", $"FileGetHashMD5Async(\"{filename}\")", PASSED, "Получен Hash код: " + result, "Got the hash code: " + result, IMAGE_STATUS_PASSED);
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"FileGetHashMD5Async(\"{filename}\")", $"FileGetHashMD5Async(\"{filename}\")", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            SendMessageDebug($"FileGetHashMD5Async(\"{filename}\")", $"FileGetHashMD5Async(\"{filename}\")", WARNING, "Не удалось получить Hash код", "Failed to get Hash code", IMAGE_STATUS_WARNING);
            return "";
        }

        public async Task<string> CreateHashMD5FromTextAsync(string text)
        {
            if (DefineTestStop() == true) return "";
            string result = "";

            try
            {
                var hash = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes((text).ToCharArray()));
                foreach (byte h in hash)
                {
                    result += h.ToString("x2");
                }
            }
            catch (Exception ex)
            {
                SendMessageDebug($"CreateHashMD5FromTextAsync(\"{text}\")", $"CreateHashMD5FromTextAsync(\"{text}\")", Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (result != "") SendMessageDebug($"CreateHashMD5FromTextAsync(\"{text}\")", $"CreateHashMD5FromTextAsync(\"{text}\")", PASSED, "Код Hash MD5 создан " + result, "MD5 Hash code created " + result, IMAGE_STATUS_PASSED);
            else SendMessageDebug($"CreateHashMD5FromTextAsync(\"{text}\")", $"CreateHashMD5FromTextAsync(\"{text}\")", WARNING, "Не удалось создать Hash MD5 код", "Failed to create MD5 Hash code", IMAGE_STATUS_WARNING);
            return result;
        }




        /* 
         * Методы для проверки результата ===========================================================
         * https://junit.org/junit4/javadoc/4.8/org/junit/Assert.html
         * */
        public async Task<bool> AssertEqualsAsync(dynamic expected, dynamic actual)
        {
            if (DefineTestStop() == true) return false;
            if (expected == actual)
            {
                SendMessageDebug("AssertEqualsAsync(" + expected + ", " + actual + ")", "AssertEqualsAsync(" + expected + ", " + actual + ")", PASSED, "Ожидаемое и актуальное значение совпадают", "The expected and actual value are the same", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                SendMessageDebug("AssertEqualsAsync(" + expected + ", " + actual + ")", "AssertEqualsAsync(" + expected + ", " + actual + ")", FAILED, "Ожидаемое и актуальное значение не совпадают", "The expected and actual value do not match", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNotEqualsAsync(dynamic expected, dynamic actual)
        {
            if (DefineTestStop() == true) return false;
            if (expected != actual)
            {
                SendMessageDebug("AssertNotEqualsAsync(" + expected + ", " + actual + ")", "AssertNotEqualsAsync(" + expected + ", " + actual + ")", PASSED, "Ожидаемое и актуальное значение не совпадают", "The expected and actual value do not match", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                SendMessageDebug("AssertNotEqualsAsync(" + expected + ", " + actual + ")", "AssertNotEqualsAsync(" + expected + ", " + actual + ")", FAILED, "Ожидаемое и актуальное значение совпадают", "The expected and actual value are the same", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertTrueAsync(bool condition)
        {
            if (DefineTestStop() == true) return false;
            if (condition == true)
            {
                SendMessageDebug("AssertTrueAsync(" + condition.ToString() + ")", "AssertTrueAsync(" + condition.ToString() + ")", PASSED, "Проверенное значение соответствует true", "The verified value corresponds to true", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                SendMessageDebug("AssertTrueAsync(" + condition.ToString() + ")", "AssertTrueAsync(" + condition.ToString() + ")", FAILED, "Проверенное значение соответствует false (должно быть true)", "The checked value corresponds to false (must be true)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertFalseAsync(bool condition)
        {
            if (DefineTestStop() == true) return false;
            if (condition == false)
            {
                SendMessageDebug("AssertFalseAsync(" + condition.ToString() + ")", "AssertFalseAsync(" + condition.ToString() + ")", PASSED, "Проверенное значение соответствует false", "The checked value corresponds to false", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                SendMessageDebug("AssertFalseAsync(" + condition.ToString() + ")", "AssertFalseAsync(" + condition.ToString() + ")", FAILED, "Проверенное значение соответствует true (должно быть false)", "The checked value corresponds to true (must be false)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNotNullAsync(dynamic obj)
        {
            string value = "null";
            if(obj != null) value = obj.ToString();
            if (DefineTestStop() == true) return false;

            if (obj != null)
            {
                SendMessageDebug("AssertNotNull(" + value + ")", "AssertNotNull(" + value + ")", PASSED, "Проверенное значение не null", "The checked value is not null", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                SendMessageDebug("AssertNotNull(" + value + ")", "AssertNotNull(" + value + ")", FAILED, "Проверенное значение null (должно быть не null)", "Checked value is null (must be non-null)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNullAsync(dynamic obj)
        {
            string value = "null";
            if (obj != null) value = obj.ToString();
            if (DefineTestStop() == true) return false;

            if (obj == null)
            {
                SendMessageDebug("AssertNull(" + value + ")", "AssertNull(" + value + ")", PASSED, "Проверенное значение null", "Verified value is null", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                SendMessageDebug("AssertNull(" + value + ")", "AssertNull(" + value + ")", FAILED, "Проверенное значение не null (должно быть null)", "The checked value is not null (must be null)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNoErrorsAsync(bool showListErrors = false, string[] listIgnored = null)
        {
            List<string> errors = await BrowserGetErrorsAsync();
            if (DefineTestStop() == true) return false;

            int countErrors = 0;
            string textErrors = "";
            bool ignor = false;
            foreach (string error in errors)
            {
                ignor = false;
                
                if (listIgnored != null)
                {
                    if (listIgnored.Length > 0)
                    {
                        foreach (string valueIgnor in listIgnored)
                        {
                            if (error.Contains(valueIgnor) == true)
                            {
                                ignor = true;
                                break;
                            }
                        }
                    }
                }

                //if (error.Contains("stats.g.doubleclick.net") == true) continue;
                if (ignor == true) continue;

                if (error.Contains("error") == true || error.Contains("Error") == true || error.Contains("failed") == true || error.Contains("Failed") == true )
                {
                    textErrors += error + Environment.NewLine;
                    countErrors++;
                }
            }

            bool result;
            if (countErrors > 0)
            {
                if (showListErrors == true)
                {
                    textErrors = textErrors.Replace("\n", "<br>" + Environment.NewLine);
                    SendMessageDebug($"AssertNoErrors({showListErrors}, \"{listIgnored}\")", $"AssertNoErrors({showListErrors}, \"{listIgnored}\")", FAILED, 
                        "В консоли присутствует " + countErrors.ToString() + " ошибок. <br>" + Environment.NewLine + textErrors,
                        "There are " + countErrors.ToString() + " errors in the console. <br>" + Environment.NewLine + textErrors,
                    Tester.IMAGE_STATUS_FAILED);
                }
                else
                {
                    SendMessageDebug($"AssertNoErrors({showListErrors}, \"{listIgnored}\")", $"AssertNoErrors({showListErrors}, \"{listIgnored}\")", FAILED, 
                        "В консоли присутствует " + countErrors.ToString() + " ошибок.",
                        "There are " + countErrors.ToString() + " errors in the console.",
                        Tester.IMAGE_STATUS_FAILED);
                }
                
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                result = false;
            }
            else
            {
                SendMessageDebug($"AssertNoErrors({showListErrors}, \"{listIgnored}\")", $"AssertNoErrors({showListErrors}, \"{listIgnored}\")", PASSED, "Проверка завершена - ошибок в консоли нет", "The check is completed - there are no errors in the console", Tester.IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                result = true;
            }
            return result;
        }

        /* presence = true проверить присутствие | presence = false проверить отсутствие (absence) */
        public async Task<bool> AssertNetworkEventsAsync(bool presence, string[] events)
        {
            string network = await BrowserGetNetworkAsync();
            if (presence == true) SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", PROCESS, "Проверка присутствия событий в Network", "Checking the presence of events in the Network", IMAGE_STATUS_PROCESS);
            else SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", PROCESS, "Проверка отсутствия событий в Network", "Checking the presence of events in the Network", IMAGE_STATUS_PROCESS);
            if (DefineTestStop() == true) return false;

            bool actual;
            bool result = true;
            string reportRus = "";
            string reportEng = "";
            foreach (string eventName in events)
            {
                actual = network.Contains(eventName);
                if (actual == false)
                {
                    reportRus += "событие: " + eventName + " - отсутствует <br>" + Environment.NewLine;
                    reportEng += "event: " + eventName + " - absent <br>" + Environment.NewLine;
                }
                else
                {
                    reportRus += "событие: " + eventName + " - присутствует <br>" + Environment.NewLine;
                    reportEng += "event: " + eventName + " - is present <br>" + Environment.NewLine;
                }
                if (actual != presence) result = false;
            }

            if (result == true)
            {
                if (presence == true) SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", PASSED, "Проверка завершена - все события присутствуют " + Environment.NewLine + reportRus, "Verification is complete - all events are present " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_PASSED);
                else SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", PASSED, "Проверка завершена - все события отсутствуют " + Environment.NewLine + reportRus, "Verification completed - all events are missing " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
            }
            else
            {
                if (presence == true) SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", FAILED, "В Network отсутствуют следующие события " + Environment.NewLine + reportRus, "The following events are missing in the Network " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_FAILED);
                else SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", FAILED, "В Network присутствуют следующие события " + Environment.NewLine + reportRus, "The following events are present in the Network " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
            }
            return result;
        }
                


    }
}
