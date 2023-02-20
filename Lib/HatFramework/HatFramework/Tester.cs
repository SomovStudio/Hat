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
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using HatFramework;

namespace HatFramework
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
        private MethodInfo browserEditMessageStep;  // функция: EditMessageStep - изменить уже выведенное сообщение в таблице "тест"
        private MethodInfo browserResize;           // функция: BrowserResize - изменить размер браузера
        private MethodInfo browserUserAgent;        // функция: UserAgent - настройка user-agent параметра
        private MethodInfo browserGetErrors;        // Функция: GetBowserErrors - получить список ошибок и предупреждений браузера
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

        private bool languageEngConsole = false;    // флаг: английский язык для вывода в командной строке
        private bool languageEngReportEmail = false;// флаг: английский язык для вывода в отчет и письмо
        private bool statusPageLoad = false;        // флаг: статус загрузки страницы
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
                browserEditMessageStep = BrowserWindow.GetType().GetMethod("EditMessageStep");
                browserResize = BrowserWindow.GetType().GetMethod("BrowserResize");
                browserUserAgent = BrowserWindow.GetType().GetMethod("UserAgent");
                browserGetErrors = BrowserWindow.GetType().GetMethod("GetBowserErrors");
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
                int step = SendMessageDebug("Сообщение", "Message", PROCESS, "Запуск автотеста", "Launching the autotest", IMAGE_STATUS_MESSAGE);
                string filename = (string)getNameAutotest.Invoke(BrowserWindow, null);
                EditMessageDebug(step, "Сообщение", "Message", COMPLETED, $"Запущен автотест из файла: {filename}", $"The autotest file is running: {filename}", IMAGE_STATUS_MESSAGE);
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

        private async Task<string> execute(string script, int step, string commentPassedRus, string commentPassedEng, string commentfailedRus, string commentfailedEng)
        {
            string result = null;
            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result == "null" || result == null)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED, commentfailedRus, commentfailedEng, Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessageDebug(step, null, null, Tester.PASSED, commentPassedRus, commentPassedEng, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
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

        public int SendMessage(string action, string status, string comment, int image)
        {
            try
            {
                if (assertStatus != FAILED)
                {
                    // вывод сообщения в системную консоль
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, true }); // вставляется пустая строка

                    if (languageEngConsole == true)
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PASSED", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "FAILED", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "WARNING", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PROCESS", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "COMPLETED", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "STOPPED", default, default, default, false });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                    }
                    else
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "УСПЕШНО", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРОВАЛЬНО", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРЕДУПРЕЖДЕНИЕ", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "В ПРОЦЕССЕ", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ВЫПОЛНЕНО", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ОСТАНОВЛЕНО", default, default, default, false });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                    }

                    if (action != null && action != "") browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + action, default, default, default, false });
                    if (comment != null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + comment, default, default, default, false });
                }

                // вывод сообщения в таблицу браузера
                int index = (int)browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, image });

                // индекс сообщения
                return index;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return -1;
        }

        public void EditMessage(int index, string action, string status, string comment, int image)
        {
            try
            {
                if (assertStatus != FAILED)
                {
                    // вывод сообщения в системную консоль
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, true }); // вставляется пустая строка

                    if (languageEngConsole == true)
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PASSED", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "FAILED", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "WARNING", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PROCESS", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "COMPLETED", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "STOPPED", default, default, default, false });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                    }
                    else
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "УСПЕШНО", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРОВАЛЬНО", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРЕДУПРЕЖДЕНИЕ", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "В ПРОЦЕССЕ", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ВЫПОЛНЕНО", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ОСТАНОВЛЕНО", default, default, default, false });
                        else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                    }

                    if (action != null && action != "") browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + action, default, default, default, false });
                    if (comment != null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + comment, default, default, default, false });

                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, true }); // вставляется пустая строка
                }

                // изменяем сообщения в таблицу браузера
                browserEditMessageStep.Invoke(BrowserWindow, new object[] { index, action, status, comment, image });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }


        public int SendMessageDebug(string actionRus, string actionEng, string status, string commentRus, string commentEng, int image)
        {
            try
            {
                if (assertStatus != FAILED)
                {
                    // вывод сообщения в системную консоль
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, true }); // вставляется пустая строка

                    if (languageEngConsole == true)
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PASSED", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "FAILED", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "WARNING", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PROCESS", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "COMPLETED", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "STOPPED", default, default, default, false });
                        else
                        {
                            //if (Regex.IsMatch(status, @"\p{IsCyrillic}") == false) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false }); // если в статусе не присутствует русский язык
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                        }

                        if (actionEng != null && actionEng != "")
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + actionEng, default, default, default, false });
                        }

                        if (commentEng != null)
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + commentEng, default, default, default, false });
                        }
                    }
                    else
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "УСПЕШНО", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРОВАЛЬНО", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРЕДУПРЕЖДЕНИЕ", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "В ПРОЦЕССЕ", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ВЫПОЛНЕНО", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ОСТАНОВЛЕНО", default, default, default, false });
                        else
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                        }

                        if (actionRus != null && actionRus != "")
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + actionRus, default, default, default, false });
                        }

                        if (commentRus != null)
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + commentRus, default, default, default, false });
                        }
                    }
                }

                // вывод сообщения в таблицу браузера
                string action = "";
                if (actionEng != null) action = actionEng;
                if (actionRus != null) action = actionRus;
                string comment = "";
                if (commentEng != null) comment = commentEng;
                if (commentRus != null) comment = commentRus;

                int index = (int)browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, image });

                // индекс сообщения
                return index;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return -1;
        }

        public void EditMessageDebug(int index, string actionRus, string actionEng, string status, string commentRus, string commentEng, int image)
        {
            try
            {
                if (assertStatus != FAILED)
                {
                    // вывод сообщения в системную консоль
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, true }); // вставляется пустая строка

                    if (languageEngConsole == true)
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PASSED", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "FAILED", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "WARNING", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "PROCESS", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "COMPLETED", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "STOPPED", default, default, default, false });
                        else
                        {
                            //if (Regex.IsMatch(status, @"\p{IsCyrillic}") == false) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false }); // если в статусе не присутствует русский язык
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                        }

                        if (actionEng != null && actionEng != "")
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + actionEng, default, default, default, false });
                        }

                        if (commentEng != null)
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + commentEng, default, default, default, false });
                        }
                    }
                    else
                    {
                        if (status == null) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, false });
                        else if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "УСПЕШНО", default, ConsoleColor.Black, ConsoleColor.DarkGreen, false });
                        else if (status == FAILED && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРОВАЛЬНО", default, ConsoleColor.Black, ConsoleColor.DarkRed, false });
                        else if (status == WARNING && assertStatus != FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ПРЕДУПРЕЖДЕНИЕ", default, ConsoleColor.Black, ConsoleColor.DarkYellow, false });
                        else if (status == PROCESS) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "В ПРОЦЕССЕ", default, default, default, false });
                        else if (status == COMPLETED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ВЫПОЛНЕНО", default, default, default, false });
                        else if (status == STOPPED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "ОСТАНОВЛЕНО", default, default, default, false });
                        else
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, false });
                        }

                        if (actionRus != null && actionRus != "")
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + actionRus, default, default, default, false });
                        }

                        if (commentRus != null)
                        {
                            browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { " - " + commentRus, default, default, default, false });
                        }
                    }

                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "", default, default, default, true }); // вставляется пустая строка
                }

                // изменяем сообщения в таблицу браузера
                string action = null;
                if (actionEng != null) action = actionEng;
                if (actionRus != null) action = actionRus;
                string comment = null;
                if (commentEng != null) comment = commentEng;
                if (commentRus != null) comment = commentRus;

                browserEditMessageStep.Invoke(BrowserWindow, new object[] { index, action, status, comment, image });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        /*
         * Методы для отправки сообщения на почту и телеграм
         */
        public async Task SendMsgToMailAsync(string subject, string body)
        {
            int step = SendMessageDebug($"SendMsgToMail(\"{subject}\", \"{body}\")", $"SendMsgToMail(\"{subject}\", \"{body}\")", PROCESS, "Отправка письма", "Sending a email", IMAGE_STATUS_PROCESS);
            try
            {
                sendMail.Invoke(BrowserWindow, new Object[] { subject, body });
                EditMessageDebug(step, null, null, COMPLETED, "Письмо отправлено", "the email has been sent", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                ConsoleMsgError(ex.Message);
            }
        }

        public async Task SendMsgToTelegramAsync(string botToken, string chatId, string text, string charset = "UTF-8")
        {
            int step = SendMessageDebug($"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")",
                $"SendMsgToTelegramAsync(\"{botToken}\", \"{chatId}\", \"{text}\", \"{charset}\")",
                PROCESS, "Отправка сообщения в Телеграм", "Sending a message in Telegrams", IMAGE_STATUS_PROCESS);

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
                    EditMessageDebug(step, null, null, PASSED, "Сообщение отправлено в Телеграм", "The message was sent in a Telegram", IMAGE_STATUS_PASSED);
                }
                else
                {
                    EditMessageDebug(step, null, null, FAILED,
                        "Не удалось отправить сообщение в Телеграм " + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Failed to send message in Telegram " + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
                int step = SendMessageDebug("TestBeginAsync()", "TestBeginAsync()", PROCESS, "Инициализация теста", "Initializing the test", IMAGE_STATUS_PROCESS);
                await BrowserView.EnsureCoreWebView2Async();
                Debug = (bool)debugJavaScript.Invoke(BrowserWindow, null);
                EditMessageDebug(step, null, null, PASSED, "Выполнена инициализация теста", "Initialization of the test has been performed", IMAGE_STATUS_PASSED);
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
                int step = SendMessageDebug("TestEndAsync()", "TestEndAsync()", PROCESS, "Завершение теста", "Completing the test", IMAGE_STATUS_PROCESS);
                if (assertStatus == FAILED)
                {
                    ConsoleMsg("Тест завершен - провально");
                    EditMessageDebug(step, null, null, FAILED, "Тест завершен - шаги теста выполнены неуспешно", "Test completed - the test steps were executed unsuccessfully", IMAGE_STATUS_FAILED);
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
                    EditMessageDebug(step, null, null, PASSED, "Тест завершен - все шаги выполнены успешно", "The test is completed - all steps are completed successfully", IMAGE_STATUS_PASSED);
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

        public bool DefineTestStop(int stepIndex)
        {
            try
            {
                if (testStop != true) testStop = (bool)checkStopTest.Invoke(BrowserWindow, null);
                if (testStop == true && stepIndex < 0) SendMessageDebug("CheckTestStop()", "CheckTestStop()", STOPPED, "Выполнение теста остановлено", "Test execution stopped", IMAGE_STATUS_WARNING);
                if (testStop == true && stepIndex >= 0) EditMessageDebug(stepIndex, null, null, STOPPED, "Выполнение шага остановлено", "Step execution stopped", IMAGE_STATUS_WARNING);
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
                int step = SendMessageDebug("GetTestResult()", "GetTestResult()", PROCESS, "Определяется результат теста", "The test result is determined", IMAGE_STATUS_MESSAGE);
                if (assertStatus != null) result = assertStatus;
                EditMessageDebug(step, null, null, COMPLETED, $"Результат теста получен: {result}", $"The test result is received: {result}", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug("BrowserCloseAsync()", "BrowserCloseAsync()", PROCESS, "Браузер закрывается", "The browser is closing", IMAGE_STATUS_PROCESS);
                BrowserWindow.Close();
                EditMessageDebug(step, null, null, COMPLETED, "Закрытие браузера - выполнено", "Closing the browser - completed", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug($"BrowserSizeAsync({width}, {height})", $"BrowserSizeAsync({width}, {height})", PROCESS, "Изменяется размер браузера", "Browser size changes", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return;

                browserResize.Invoke(BrowserWindow, new object[] { width, height });
                EditMessageDebug(step, null, null, COMPLETED, "Размер браузера изменён", "Browser size changed", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug($"BrowserFullScreenAsync()", $"BrowserFullScreenAsync()", PROCESS, "Изменяется размер браузера", "Browser size changes", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return;

                browserResize.Invoke(BrowserWindow, new object[] { -1, -1 });
                EditMessageDebug(step, null, null, COMPLETED, "Размер браузера изменён", "Browser size changed", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug($"BrowserSetUserAgentAsync({value})", $"BrowserSetUserAgentAsync({value})", PROCESS, "Изменяется значение User-Agent", "The User-Agent value changes", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return;

                browserUserAgent.Invoke(BrowserWindow, new object[] { value });
                EditMessageDebug(step, null, null, COMPLETED, "Значение User-Agent изменено", "User-Agent value changed", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug($"BrowserGetUserAgentAsync()", $"BrowserGetUserAgentAsync()", PROCESS, "Получение значения User-Agent", "Getting the User-Agent value", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return "";

                userAgent = BrowserView.CoreWebView2.Settings.UserAgent;
                EditMessageDebug(step, null, null, COMPLETED, $"Из User-Agent получено значение: {userAgent}", $"The value was obtained from the User-Agent: {userAgent}", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug($"BrowserGetErrorsAsync()", $"BrowserGetErrorsAsync()", PROCESS, "Получение списка ошибок и предупреждений браузера", "Getting a list of browser errors and warnings", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return list;

                list = (List<string>)browserGetErrors.Invoke(BrowserWindow, null);
                EditMessageDebug(step, null, null, COMPLETED, "Получен список ошибок и предупреждений браузера", "Received a list of browser errors and warnings", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return list;
        }

        public async Task<string> BrowserGetNetworkAsync()
        {
            string events = null;
            try
            {
                int step = SendMessageDebug($"BrowserGetNetworkAsync()", $"BrowserGetNetworkAsync()", PROCESS, "Получение списка событий браузера (network)", "Getting a list of browser events (network)", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return "";

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
                EditMessageDebug(step, null, null, COMPLETED, "Получен список событий браузера (network)", "Received a list of browser events (network)", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessageDebug($"BrowserClearNetworkAsync()", $"BrowserClearNetworkAsync()", PROCESS, "Очистка списка событий браузера (network)", "Clearing the list of browser events (network)", IMAGE_STATUS_PROCESS);
                if (DefineTestStop(step) == true) return "";

                string script =
                @"(function(){
                var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {};
                var network = performance.getEntries() || {}; 
                var result = network.slice(5,10);
                return result;
                }());";

                events = await executeJS(script);
                EditMessageDebug(step, null, null, COMPLETED,
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

        public async Task BrowserGoBackAsync(int sec)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            int step = SendMessageDebug($"BrowserGoBackAsync()", $"BrowserGoBackAsync()", PROCESS, "Выполняется действие браузера - назад", "the browser performs the action - back", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                BrowserView.GoBack();

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop(step) == true) return;
                }

                if (statusPageLoad == true) EditMessageDebug(step, null, null, PASSED, "Выполнено действие браузера - назад", "Browser action performed - back", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Не выполнено действие браузера - назад (cтраница не загружена)", "Browser action failed - back (page not loaded)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserGoForwardAsync(int sec)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            int step = SendMessageDebug($"BrowserGoForwardAsync()", $"BrowserGoForwardAsync()", PROCESS, "Выполняется действие браузера - вперед", "the browser performs the action - forward", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                BrowserView.GoForward();

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop(step) == true) return;
                }

                if (statusPageLoad == true) EditMessageDebug(step, null, null, PASSED, "Выполнено действие браузера - вперед", "Browser action performed - forward", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Не выполнено действие браузера - вперед (cтраница не загружена)", "Browser action failed - forward (page not loaded)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"BrowserBasicAuthentication(\"{user}\", \"{pass}\")",
                $"BrowserBasicAuthentication(\"{user}\", \"{pass}\")",
                PROCESS, "Выполняется базовая авторизация", "Basic authorization is being performed", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                BrowserView.CoreWebView2.BasicAuthenticationRequested += delegate (object sender, CoreWebView2BasicAuthenticationRequestedEventArgs args)
                {
                    args.Response.UserName = user;
                    args.Response.Password = pass;
                    EditMessageDebug(step, null, null, COMPLETED, "Баговая авторизация - выполнена", "Basic authorization - completed", IMAGE_STATUS_MESSAGE);
                };
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserEnableSendMailAsync(bool byFailure = true, bool bySuccess = true)
        {
            int step = SendMessageDebug($"BrowserEnableSendMailAsync(\"{byFailure}\", \"{bySuccess}\")",
                $"BrowserEnableSendMailAsync(\"{byFailure}\", \"{bySuccess}\")",
                PROCESS, "Включение опции отправки отчета на почту", "Enabling the option to send a report to the mail", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                sendFailureReportByMail = byFailure;
                sendSuccessReportByMail = bySuccess;
                EditMessageDebug(step, null, null, PASSED, "Включена опция отправки отчета на почту", "The option to send a report to the mail is enabled", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserPageReloadAsync(int sec)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            int step = SendMessageDebug($"BrowserPageReloadAsync({sec})", $"BrowserPageReloadAsync({sec})", PROCESS, "Перезагрузка страницы", "Page Reload", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
                    if (DefineTestStop(step) == true) return;
                }

                if (statusPageLoad == true) EditMessageDebug(step, null, null, PASSED, "Перезагрузка страницы выполнена", "Page reload completed", IMAGE_STATUS_MESSAGE);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Страница не загружена", "The page is not loaded", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> ExecuteJavaScriptAsync(string script)
        {
            string result = null;
            int step = SendMessageDebug($"ExecuteJavaScriptAsync(\"{script}\")", $"ExecuteJavaScriptAsync(\"{script}\")",
                PROCESS, "Выполнение скрипта", "Script Execution", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return result;

            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
                EditMessageDebug(step, null, null, PASSED, "Скрипт выполнен", "The script is executed", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
        public async Task<HTMLElement> GetElementAsync(string by, string locator)
        {
            int step;

            if (by == BY_XPATH) step = SendMessageDebug($"GetElementAsync(\"{by}\", \"{locator}\")", $"GetElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Получить элемента", "Get an element", IMAGE_STATUS_PROCESS);
            else step = SendMessageDebug($"GetElementAsync(\"{by}\", '{locator}')", $"GetElementAsync(\"{by}\", '{locator}')", PROCESS, "Получить элемента", "Get an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
                    EditMessageDebug(step, null, null, Tester.FAILED, $"Не удалось получить элемент {locator} ({by})", $"Failed to get the element {locator} ({by})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    htmlElement = new HTMLElement(this, by, locator);
                    htmlElement.Id = el.Id;
                    htmlElement.Name = el.Name;
                    htmlElement.Class = el.Class;
                    htmlElement.Type = el.Type;
                    EditMessageDebug(step, null, null, PASSED, "Элемент получен", "The element is received", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            step = SendMessageDebug($"GetFrameAsync({index})", $"GetFrameAsync({index})", PROCESS, "Получить фрейм", "Get a frame", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
                    EditMessageDebug(step, null, null, Tester.FAILED, $"Не удалось получить фрейм с индексом {index}", $"Failed to get a frame with index {index}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    frameElement = new FRAMEElement(this, index);
                    frameElement.Index = el.Index;
                    frameElement.Name = el.Name;
                    EditMessageDebug(step, null, null, PASSED, "Фрейм получен", "Frame received", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return frameElement;
        }

        public async Task GoToUrlAsync(string url, int sec)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            int step = SendMessageDebug($"GoToUrlAsync('{url}', {sec})", $"GoToUrlAsync('{url}', {sec})", PROCESS, "Загрузка страницы", "Page Loading", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                BrowserView.CoreWebView2.Navigate(url);

                for (int i = 0; i < sec; i++)
                {
                    await Task.Delay(1000);
                    if (statusPageLoad == true) break;
                    if (DefineTestStop(step) == true) return;
                }

                if (statusPageLoad == true) EditMessageDebug(step, null, null, PASSED, "Страница загружена", "Page loaded", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Страница не загружена", "The page is not loaded", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task GoToUrlBaseAuthAsync(string url, string login, string pass, int sec)
        {
            listRedirects.Clear();
            statusPageLoad = false;
            int step = SendMessageDebug($"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec})", $"GoToUrlBaseAuthAsync('{url}', '{login}', '{pass}', {sec})",
                PROCESS, "Загрузка страницы (базовая авторизация)", "Page loading (basic authorization)", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string baseUrl = "";
                if (url.Contains("https://") == true)
                {
                    baseUrl = url.Replace("https://", "");
                    baseUrl = $"https://{login}:{pass}@{baseUrl}";
                }
                else if (url.Contains("http://") == true)
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
                    if (DefineTestStop(step) == true) return;
                }

                if (statusPageLoad == true) EditMessageDebug(step, null, null, PASSED, "Страница загружена (базовая авторизация)", "Page loaded (basic authorization)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Страница не загружена (базовая авторизация)", "The page is not loaded (basic authorization)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetUrlAsync()
        {
            int step = SendMessageDebug("GetUrlAsync()", "GetUrlAsync()", PROCESS, "Запрашивается текущий URL", "The current URL is requested", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;
            string url = null;
            try
            {
                url = BrowserView.Source.ToString();
                EditMessageDebug(step, null, null, PASSED, $"Получен текущий URL: {url}", $"The current URL was received: {url}", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug("GetListRedirectUrlAsync()", "GetListRedirectUrlAsync()", PROCESS, "Получаю список редиректов", "I get a list of redirects", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;
            if (listRedirects == null) EditMessageDebug(step, null, null, FAILED, "Список редиректов NULL", "List of redirects is NULL", IMAGE_STATUS_FAILED);
            else EditMessageDebug(step, null, null, PASSED, "Список редиректов получен", "The list of redirects has been received", IMAGE_STATUS_PASSED);
            return listRedirects;
        }

        public async Task<int> GetUrlResponseAsync(string url)
        {
            int step = SendMessageDebug($"GetUrlResponseAsync('{url}')", $"GetUrlResponseAsync('{url}')", PROCESS, "Получаю HTTP ответ запрашиваемого URL", "I get the HTTP response of the requested URL", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return 0;

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

                EditMessageDebug(step, null, null, PASSED, $"Получен HTTP ответ: {statusCode} по URL: {url}", $"HTTP response received: {statusCode} by URL: {url}", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"WaitAsync({sec})", $"WaitAsync({sec})", PROCESS,
                $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                await Task.Delay(sec * 1000);
                EditMessageDebug(step, null, null, PASSED, $"Ожидание {sec.ToString()} секунд - завершено", $"Waiting {sec.ToString()} seconds - completed", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByIdAsync(string id, int sec)
        {
            int step = SendMessageDebug($"WaitVisibleElementByIdAsync('{id}', {sec})", $"WaitVisibleElementByIdAsync('{id}', {sec})",
                PROCESS, $"Ожидание элемента {sec.ToString()} секунд", $"Waiting for an element {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_ID, id);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null,
                    PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByClassAsync(string _class, int index, int sec)
        {
            int step = SendMessageDebug($"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})",
                PROCESS, $"Ожидание элемента {sec.ToString()} секунд", $"Waiting for an element {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_CLASS, _class, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByNameAsync(string name, int index, int sec)
        {
            int step = SendMessageDebug($"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitVisibleElementByNameAsync('{name}', {index}, {sec})",
                PROCESS, $"Ожидание элемента {sec.ToString()} секунд", $"Waiting for an element {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_NAME, name, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByTagAsync(string tag, int index, int sec)
        {
            int step = SendMessageDebug($"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})",
                PROCESS, $"Ожидание элемента {sec.ToString()} секунд", $"Waiting for an element {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_TAG, tag, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementAsync(string by, string locator, int sec)
        {
            int step = SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})",
                PROCESS, $"Ожидание элемента {sec} секунд", $"Waiting for an element {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
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

                if (found == true) EditMessageDebug(step, null, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByIdAsync(string id, int sec)
        {
            int step = SendMessageDebug($"WaitNotVisibleElementByIdAsync('{id}', {sec})", $"WaitNotVisibleElementByIdAsync('{id}', {sec})",
                PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", $"Waiting for the element to be hidden {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_ID, id);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) EditMessageDebug(step, null, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByClassAsync(string _class, int index, int sec)
        {
            int step = SendMessageDebug($"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", $"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})",
                PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", $"Waiting for the element to be hidden {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_CLASS, _class, index);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) EditMessageDebug(step, null, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByNameAsync(string name, int index, int sec)
        {
            int step = SendMessageDebug($"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", $"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})",
                PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", $"Waiting for the element to be hidden {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_NAME, name, index);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) EditMessageDebug(step, null, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByTagAsync(string tag, int index, int sec)
        {
            int step = SendMessageDebug($"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", $"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})",
                PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", $"Waiting for the element to be hidden {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_TAG, tag, index);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) EditMessageDebug(step, null, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementAsync(string by, string locator, int sec)
        {
            int step = SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})",
                PROCESS, $"Ожидание скрытия элемента {sec} секунд", $"Waiting for the element to be hidden {sec.ToString()} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
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

                if (found == false) EditMessageDebug(step, null, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitElementInDomAsync(string by, string locator, int sec)
        {
            int step = SendMessageDebug($"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementInDomAsync(\"{by}\", \"{locator}\", {sec})",
                PROCESS, $"Ожидание присутствия элемента в DOM в течении {sec} секунд", $"Waiting for the presence of an element in the DOM for {sec} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, $"Ожидание элемента - завершено (элемент присутствует в DOM)", $"Waiting for the element - completed (the element is present in the DOM)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание элемента - завершено (элемент не присутствует в DOM)", $"Waiting for the element - completed (the element is not present in the DOM)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitElementNotDomAsync(string by, string locator, int sec)
        {
            int step = SendMessageDebug($"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})", $"WaitElementNotDomAsync(\"{by}\", \"{locator}\", {sec})",
                PROCESS, $"Ожидание отсутствия элемента в DOM в течении {sec} секунд", $"Waiting for the absence of an element in the DOM for {sec} seconds", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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

                if (found == false) EditMessageDebug(step, null, null, PASSED, $"Ожидание отсутствия элемента - завершено (элемент отсутствует в DOM)", $"Waiting for the absence element - completed (the element is absence in the DOM)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessageDebug(step, null, null, FAILED, $"Ожидание отсутствия элемента - завершено (элемент присутствует в DOM)", $"Waiting for the absence of an element - completed (the element is present in the DOM)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<bool> FindElementByIdAsync(string id, int sec)
        {
            int step = SendMessageDebug($"FindElementByIdAsync('{id}', {sec})", $"FindElementByIdAsync('{id}', {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindElementByClassAsync('{_class}', {index}, {sec})", $"FindElementByClassAsync('{_class}', {index}, {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindElementByNameAsync('{name}', {index}, {sec})", $"FindElementByNameAsync('{name}', {index}, {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindElementByTagAsync('{tag}', {index}, {sec})", $"FindElementByTagAsync('{tag}', {index}, {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindVisibleElementByIdAsync('{id}', {sec})", $"FindVisibleElementByIdAsync('{id}', {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_ID, id);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", $"FindVisibleElementByClassAsync('{_class}', {index}, {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_CLASS, _class, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindVisibleElementByNameAsync('{name}', {index}, {sec})", $"FindVisibleElementByNameAsync('{name}', {index}, {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_NAME, name, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", $"FindVisibleElementByTagAsync('{tag}', {index}, {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible(BY_TAG, tag, index);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})",
                PROCESS, "Поиск элемента", "Element Search", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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

                if (found == true) EditMessageDebug(step, null, null, PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"ClickElementByIdAsync('{id}')", $"ClickElementByIdAsync('{id}')", PROCESS, "Нажатие на элемент", "Clicking on an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementById('" + id + "'); element.click(); return element; }());";
            await execute(script, step, "Элемент нажат", "The element is pressed", $"Не удалось найти элемент с ID: {id}", $"Couldn't find an element with ID: {id}");
        }

        public async Task ClickElementByClassAsync(string _class, int index)
        {
            int step = SendMessageDebug($"ClickElementByClassAsync('{_class}', {index})", $"ClickElementByClassAsync('{_class}', {index})", PROCESS, "Нажатие на элемент", "Clicking on an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; element.click(); return element; }());";
            await execute(script, step, "Элемент нажат", "The element is pressed", $"Не удалось найти элемент по Class: {_class} (Index: {index})", $"Couldn't find an element by Class: {_class} (Index: {index})");
        }

        public async Task ClickElementByNameAsync(string name, int index)
        {
            int step = SendMessageDebug($"ClickElementByNameAsync('{name}', {index})", $"ClickElementByNameAsync('{name}', {index})", PROCESS, "Нажатие на элемент", "Clicking on an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; element.click(); return element; }());";
            await execute(script, step, "Элемент нажат", "The element is pressed", $"Не удалось найти элемент по Name: {name} (Index: {index})", $"Couldn't find an element by Name: {name} (Index: {index})");
        }

        public async Task ClickElementByTagAsync(string tag, int index)
        {
            int step = SendMessageDebug($"ClickElementByTagAsync('{tag}', {index})", $"ClickElementByTagAsync('{tag}', {index})", PROCESS, "Нажатие на элемент", "Clicking on an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; element.click(); return element; }());";
            await execute(script, step, "Элемент нажат", "The element is pressed", $"Не удалось найти элемент по Tag: {tag} (Index: {index})", $"Couldn't find an element by Tag: {tag} (Index: {index})");
        }

        public async Task ClickElementAsync(string by, string locator)
        {
            int step = SendMessageDebug($"ClickElementAsync(\"{by}\", \"{locator}\")", $"ClickElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Нажатие на элемент", "Clicking on an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); element.click(); return element;";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; element.click(); return element;";
            script += "}());";
            await execute(script, step, "Элемент нажат", "The element is pressed", $"Не удалось найти элемент по локатору: {locator}", $"The element could not be found by the locator: {locator}");
        }

        public async Task SetValueInElementByIdAsync(string id, string value)
        {
            int step = SendMessageDebug($"SetValueInElementByIdAsync('{id}', '{value}')", $"SetValueInElementByIdAsync('{id}', '{value}')", PROCESS, "Ввод значения в элемент", "Entering a value into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
            await execute(script, step, "Значение введено в элемент", "The value was entered into the element", $"Не удалось найти или ввести значение в элемент с ID: {id}", $"Could not find or enter a value in an element with ID: {id}");
        }

        public async Task SetValueInElementByClassAsync(string _class, int index, string value)
        {
            int step = SendMessageDebug($"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", $"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", PROCESS, "Ввод значения в элемент", "Entering a value into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
            await execute(script, step, "Значение введено в элемент", "The value was entered into the element", $"Не удалось найти или ввести значение в элемент по Class: {_class} (Index: {index})", $"Could not find or enter a value in the element by Class: {_class} (Index: {index})");
        }

        public async Task SetValueInElementByNameAsync(string name, int index, string value)
        {
            int step = SendMessageDebug($"SetValueInElementByNameAsync('{name}', {index}, '{value}')", $"SetValueInElementByNameAsync('{name}', {index}, '{value}')", PROCESS, "Ввод значения в элемент", "Entering a value into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
            await execute(script, step, "Значение введено в элемент", "The value was entered into the element", $"Не удалось найти или ввести значение в элемент по Name: {name} (Index: {index})", $"Could not find or enter a value in the element by Name: {name} (Index: {index})");
        }

        public async Task SetValueInElementByTagAsync(string tag, int index, string value)
        {
            int step = SendMessageDebug($"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", $"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", PROCESS, "Ввод значения в элемент", "Entering a value into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
            await execute(script, step, "Значение введено в элемент", "The value was entered into the element", $"Не удалось найти или ввести значение в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter a value in the element by Tag: {tag} (Index: {index})");
        }

        public async Task SetValueInElementAsync(string by, string locator, string value)
        {
            int step = SendMessageDebug($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", $"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", PROCESS, "Ввод значения в элемент", "Entering a value into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
            await execute(script, step, "Значение введено в элемент", "The value was entered into the element", $"Не удалось найти или ввести значение в элемент по локатору: {locator}", $"Could not find or enter a value in the element by locator: {locator}");
        }

        public async Task<string> GetValueFromElementByIdAsync(string id)
        {
            int step = SendMessageDebug($"GetValueFromElementByIdAsync('{id}')", $"GetValueFromElementByIdAsync('{id}')", PROCESS, "Получение значения из элемент", "Getting a value from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); return element.value; }());";
                value = await execute(script, step, "Получено значение из элемента", "Got the value from the element", $"Не удалось найти или получить данные из элемента с ID: {id}", $"Could not find or get data from an element with ID: {id}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetValueFromElementByClassAsync('{_class}', {index})", $"GetValueFromElementByClassAsync('{_class}', {index})", PROCESS, "Получение значения из элемент", "Getting a value from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.value; }());";
                value = await execute(script, step, "Получено значение из элемента", "Got the value from the element", $"Не удалось найти или получить данные из элемента по Class: {_class} (Index: {index})", $"Could not find or get data from the element by Class: {_class} (Index: {index})");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetValueFromElementByNameAsync('{name}', {index})", $"GetValueFromElementByNameAsync('{name}', {index})", PROCESS, "Получение значения из элемент", "Getting a value from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.value; }());";
                value = await execute(script, step, "Получено значение из элемента", "Got the value from the element", $"Не удалось найти или получить данные из элемента по Name: {name} (Index: {index})", $"Could not find or get data from the element by Name: {name} (Index: {index})");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetValueFromElementByTagAsync('{tag}', {index})", $"GetValueFromElementByTagAsync('{tag}', {index})", PROCESS, "Получение значения из элемент", "Getting a value from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.value; }());";
                value = await execute(script, step, "Получено значение из элемента", "Got the value from the element", $"Не удалось найти или получить данные из элемента по Tag: {tag} (Index: {index})", $"Could not find or get data from the element by Tag: {tag} (Index: {index})");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Получение значения из элемент", "Getting a value from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); return element.value;";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.value;";
                script += "}());";
                value = await execute(script, step, "Получено значение из элемента", "Got the value from the element", $"Не удалось найти или получить данные из элемента по локатору: {locator}", $"Could not find or get data from the element by Locator: {locator}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"SetTextInElementByIdAsync('{id}', '{text}')", $"SetTextInElementByIdAsync('{id}', '{text}')", PROCESS, "Ввод текста в элемент", "Entering text into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, "Текст введен в элемент", "The text was entered into the element", $"Не удалось найти или ввести текст в элемент с ID: {id}", $"Could not find or enter text in the element with ID: {id}");
        }

        public async Task SetTextInElementByClassAsync(string _class, int index, string text)
        {
            int step = SendMessageDebug($"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", $"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", PROCESS, "Ввод текста в элемент", "Entering text into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, "Текст введен в элемент", "The text was entered into the element", $"Не удалось найти или ввести текст в элемент по Class: {_class} (Index: {index})", $"Could not find or enter text in the element by Class: {_class} (Index: {index})");
        }

        public async Task SetTextInElementByNameAsync(string name, int index, string text)
        {
            int step = SendMessageDebug($"SetTextInElementByNameAsync('{name}', {index}, '{text}')", $"SetTextInElementByNameAsync('{name}', {index}, '{text}')", PROCESS, "Ввод текста в элемент", "Entering text into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, "Текст введен в элемент", "The text was entered into the element", $"Не удалось найти или ввести текст в элемент по Name: {name} (Index: {index})", $"Could not find or enter text in the element by Name: {name} (Index: {index})");
        }

        public async Task SetTextInElementByTagAsync(string tag, int index, string text)
        {
            int step = SendMessageDebug($"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", $"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", PROCESS, "Ввод текста в элемент", "Entering text into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, "Текст введен в элемент", "The text was entered into the element", $"Не удалось найти или ввести текст в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter text in the element by Tag: {tag} (Index: {index})");
        }

        public async Task SetTextInElementAsync(string by, string locator, string text)
        {
            int step = SendMessageDebug($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", PROCESS, "Ввод текста в элемент", "Entering text into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, "Текст введен в элемент", "The text was entered into the element", $"Не удалось найти или ввести текст в элемент по локатору: {locator}", $"Could not find or enter text in the element by locator: {locator}");
        }

        public async Task<string> GetTextFromElementByIdAsync(string id)
        {
            int step = SendMessageDebug($"GetTextFromElementByIdAsync('{id}')", $"GetTextFromElementByIdAsync('{id}')", PROCESS, "Чтение текста из элемент", "Reading text from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, step, "Прочитан текст из элемента", "The text from the element has been read", $"Не удалось найти или прочитать текст из элемента с ID: {id}", $"Could not find or read the text from the element with ID: {id}");
                if (value.Length > 1 && value != "null") value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                     "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                     "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                     Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (value == "") EditMessageDebug(step, null, null, COMPLETED, "Не удалось получить текст из элемента", "Couldn't get the text from the element", IMAGE_STATUS_WARNING);
            return value;
        }

        public async Task<string> GetTextFromElementByClassAsync(string _class, int index)
        {
            int step = SendMessageDebug($"GetTextFromElementByClassAsync('{_class}', {index})", $"GetTextFromElementByClassAsync('{_class}', {index})", PROCESS, "Чтение текста из элемент", "Reading text from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.innerText; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, step, "Прочитан текст из элемента", "The text from the element has been read", $"Не удалось найти или прочитать текст из элемента по Class: {_class} (Index: {index})", $"Could not find or read the text from the element by Class: {_class} (Index: {index})");
                if (value.Length > 1 && value != "null") value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (value == "") EditMessageDebug(step, null, null, COMPLETED, "Не удалось получить текст из элемента", "Couldn't get the text from the element", IMAGE_STATUS_WARNING);
            return value;
        }

        public async Task<string> GetTextFromElementByNameAsync(string name, int index)
        {
            int step = SendMessageDebug($"GetTextFromElementByNameAsync('{name}', {index})", $"GetTextFromElementByNameAsync('{name}', {index})", PROCESS, "Чтение текста из элемент", "Reading text from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, step, "Прочитан текст из элемента", "The text from the element has been read", $"Не удалось найти или прочитать текст из элемента по Name: {name} (Index: {index})", $"Could not find or read the text from the element by Name: {name} (Index: {index})");
                if (value.Length > 1 && value != "null") value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (value == "") EditMessageDebug(step, null, null, COMPLETED, "Не удалось получить текст из элемента", "Couldn't get the text from the element", IMAGE_STATUS_WARNING);
            return value;
        }

        public async Task<string> GetTextFromElementByTagAsync(string tag, int index)
        {
            int step = SendMessageDebug($"GetTextFromElementByTagAsync('{tag}', {index})", $"GetTextFromElementByTagAsync('{tag}', {index})", PROCESS, "Чтение текста из элемент", "Reading text from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, step, "Прочитан текст из элемента", "The text from the element has been read", $"Не удалось найти или прочитать текст из элемента по Tag: {tag} (Index: {index})", $"Could not find or read the text from the element by Tag: {tag} (Index: {index})");
                if (value.Length > 1 && value != "null") value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (value == "") EditMessageDebug(step, null, null, COMPLETED, "Не удалось получить текст из элемента", "Couldn't get the text from the element", IMAGE_STATUS_WARNING);
            return value;
        }

        public async Task<string> GetTextFromElementAsync(string by, string locator)
        {
            int step = SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Чтение текста из элемент", "Reading text from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += "var element = document.querySelector(\"" + locator + "\"); ";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, step, "Прочитан текст из элемента", "The text from the element has been read", $"Не удалось найти или прочитать текст из элемента по локатору: {locator}", $"Could not find or read the text from the element by the locator: {locator}");
                if (value.Length > 1 && value != "null") value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }

            if (value == "") EditMessageDebug(step, null, null, COMPLETED, "Не удалось получить текст из элемента", "Couldn't get the text from the element", IMAGE_STATUS_WARNING);
            return value;
        }

        public async Task<int> GetCountElementsByClassAsync(string _class)
        {
            int step = SendMessageDebug($"GetCountElementsByIdAsync('{_class}')", $"GetCountElementsByIdAsync('{_class}')", PROCESS, "Получение количества элементов", "Getting the amount of elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "'); return element.length; }());";
            string result = await execute(script, step, "Получение количества элементов", "Getting the amount of elements", $"Не удалось найти или получить количество элементов по Class: {_class}", $"Could not find or get the amount of elements by Class: {_class}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessageDebug(step, null, null, PASSED, $"Количество элементов {result}", $"Amount of elements {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<int> GetCountElementsByNameAsync(string name)
        {
            int step = SendMessageDebug($"GetCountElementsByNameAsync('{name}')", $"GetCountElementsByNameAsync('{name}')", PROCESS, "Получение количества элементов", "Getting the amount of elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){ var element = document.getElementsByName('" + name + "'); return element.length; }());";
            string result = await execute(script, step, "Получение количества элементов", "Getting the amount of elements", $"Не удалось найти или получить количество элементов по Name: {name}", $"Could not find or get the amount of elements by Name: {name}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessageDebug(step, null, null, PASSED, $"Количество элементов {result}", $"Amount of elements {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<int> GetCountElementsByTagAsync(string tag)
        {
            int step = SendMessageDebug($"GetCountElementsByTagAsync('{tag}')", $"GetCountElementsByTagAsync('{tag}')", PROCESS, "Получение количества элементов", "Getting the amount of elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "'); return element.length; }());";
            string result = await execute(script, step, "Получение количества элементов", "Getting the amount of elements", $"Не удалось найти или получить количество элементов по Tag: {tag}", $"Could not find or get the amount of elements by Tag: {tag}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessageDebug(step, null, null, PASSED, $"Количество элементов {result}", $"Amount of elements {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<int> GetCountElementsAsync(string by, string locator)
        {
            int step = SendMessageDebug($"GetCountElementsAsync(\"{by}\", \"{locator}\")", $"GetCountElementsAsync(\"{by}\", \"{locator}\")", PROCESS, "Получение количества элементов", "Getting the amount of elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){";
            if (by == BY_CSS) script += "var element = document.querySelectorAll(\"" + locator + "\"); return element.length;";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null); return element.snapshotLength;";
            script += "}());";
            string result = await execute(script, step, "Получение количества элементов", "Getting the amount of elements", $"Не удалось найти или получить количество элементов по локатору: {locator}", $"Couldn't find or get the amount of elements by locator: {locator}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessageDebug(step, null, null, PASSED, $"Количество элементов {result}", $"Amount of elements {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)
        {
            int step = SendMessageDebug($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", PROCESS, "Прокрутить к элементу", "Scrolling to the element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

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
                string result = await executeJS(script);
                EditMessageDebug(step, null, null, PASSED, "Прокрутил к элементу - выполнена", "Scrolled to the element - completed", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetTitleAsync()
        {
            int step = SendMessageDebug($"GetTitleAsync()", $"GetTitleAsync()", PROCESS, "Чтение текста из заголовка", "Reading the text from the title", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.querySelector('title'); return element.innerText; }());";
            string value = await execute(script, step, "Прочитан текст из заголовка", "The text from the title has been read", "Не удалось найти заголовок на странице", "Couldn't find the title on the page");
            return value;
        }

        public async Task<string> GetAttributeFromElementByIdAsync(string id, string attribute)
        {
            int step = SendMessageDebug($"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", $"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", $"Getting an attribute {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"The value was obtained from the attribute {attribute}", $"Не удалось найти или получить аттрибут из элемента с ID: {id}", $"Couldn't find or get attribute from element with ID: {id}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", $"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", $"Getting an attribute {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"The value was obtained from the attribute {attribute}", $"Не удалось найти или получить аттрибут из элемента по Class: {_class} (Index: {index})", $"Could not find or get an attribute from an element by Class: {_class} (Index: {index})");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", $"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", $"Getting an attribute {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"The value was obtained from the attribute {attribute}", $"Не удалось найти или получить аттрибут из элемента по Name: {name} (Index: {index})", $"Could not find or get an attribute from an element by Name: {name} (Index: {index})");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", $"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", $"Getting an attribute {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
                value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"The value was obtained from the attribute {attribute}", $"Не удалось найти или получить аттрибут из элемента по Tag: {tag} (Index: {index})", $"Could not find or get an attribute from an element by Tag: {tag} (Index: {index})");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", PROCESS, $"Получение аттрибута {attribute} из элемент", $"Getting an attribute {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
                else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"return element.getAttribute('{attribute}');";
                script += "}());";
                value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"The value was obtained from the attribute {attribute}", $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}", $"Couldn't find or get attribute from element by locator: {locator}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", $"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", PROCESS, $"Получение аттрибутов {attribute} из элементов", $"Getting attributes {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Getting json from attributes {attribute}", $"Не удалось найти или получить аттрибуты из элементов по Class: {_class}", $"Could not find or get attributes from elements by Class: {_class}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsByNameAsync(string name, string attribute)
        {
            int step = SendMessageDebug($"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", $"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", PROCESS, $"Получение аттрибутов {attribute} из элементов", $"Getting attributes {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Getting json from attributes {attribute}", $"Не удалось найти или получить аттрибуты из элементов по Name: {name}", $"Could not find or get attributes from elements by Name: {name}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsByTagAsync(string tag, string attribute)
        {
            int step = SendMessageDebug($"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", $"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", PROCESS, $"Получение аттрибутов {attribute} из элементов", $"Getting attributes {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Getting json from attributes {attribute}", $"Не удалось найти или получить аттрибуты из элементов по Tag: {tag}", $"Could not find or get attributes from elements by Tag: {tag}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsAsync(string by, string locator, string attribute)
        {
            int step = SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", PROCESS, $"Получение аттрибутов {attribute} из элементов", $"Getting attributes {attribute} from elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Getting json from attributes {attribute}", $"Не удалось найти или получить аттрибуты из элементов по локатору: {locator}", $"Couldn't find or get attributes from elements by locator: {locator}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task SetAttributeInElementByIdAsync(string id, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", $"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Attribute '{attribute}' added to the element", $"Не удалось найти или ввести аттрибут в элемент с ID: {id}", $"Could not find or enter attribute in element with ID: {id}");
        }

        public async Task SetAttributeInElementByClassAsync(string _class, int index, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Attribute '{attribute}' added to the element", $"Не удалось найти или ввести аттрибут в элемент по Class: {_class} (Index: {index})", $"Could not find or enter an attribute in an element by Class: {_class} (Index: {index})");
        }

        public async Task SetAttributeInElementByNameAsync(string name, int index, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Attribute '{attribute}' added to the element", $"Не удалось найти или ввести аттрибут в элемент по Name: {name} (Index: {index})", $"Could not find or enter an attribute in an element by Name: {name} (Index: {index})");
        }

        public async Task SetAttributeInElementByTagAsync(string tag, int index, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", $"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Attribute '{attribute}' added to the element", $"Не удалось найти или ввести аттрибут в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter an attribute in an element by Tag: {tag} (Index: {index})");
        }

        public async Task SetAttributeInElementAsync(string by, string locator, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", PROCESS, "Добавление аттрибута в элемент", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Attribute '{attribute}' added to the element", $"Не удалось найти или ввести аттрибут в элемент по локатору: {locator}", $"Could not find or enter attribute in element by locator: {locator}");
        }

        public async Task<List<string>> SetAttributeInElementsByClassAsync(string _class, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", $"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элементы", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Attribute '{attribute}' added to the element", $"Не удалось найти или добавить аттрибут в элементы по Class: {_class}", $"Could not find or add attribute to elements by Class: {_class}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{result}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsByNameAsync(string name, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", $"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элементы", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Attribute '{attribute}' added to the element", $"Не удалось найти или добавить аттрибут в элементы по Name: {name}", $"Could not find or add attribute to elements by Name: {name}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{result}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsByTagAsync(string tag, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", $"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элементы", "Adding an attribute to an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Attribute '{attribute}' added to the element", $"Не удалось найти или добавить аттрибут в элементы по Tag: {tag}", $"Could not find or add attribute to elements by Tag: {tag}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{result}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsAsync(string by, string locator, string attribute, string value)
        {
            int step = SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", PROCESS, "Добавление аттрибута в элементы", "Adding an attribute to elements", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Attribute '{attribute}'added to the elements", $"Не удалось найти или добавить аттрибут в элементы по локатору: {locator}", $"Couldn't find or add attribute to elements by locator: {locator}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessageDebug(step, null, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{result}' - added to the elements and received json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<string> GetHtmlFromElementByClassAsync(string _class, int index)
        {
            int step = SendMessageDebug($"GetHtmlFromElementByClassAsync('{_class}', '{index}')", $"GetHtmlFromElementByClassAsync('{_class}', '{index}')", PROCESS, "Получение html из элемент", "Getting html from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, step, "Получено html элемента", "The html of the element was received", $"Не удалось найти или получить html из элемента Class: {_class} (Index: {index})", $"Could not find or get html from the element by Class: {_class} (Index: {index})");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementAsync(string by, string locator)
        {
            int step = SendMessageDebug($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Получение html из элемент", "Getting html from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); return element.outerHTML;";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "return element.outerHTML;";
            script += "}());";
            string value = await execute(script, step, "Получено html элемента", "The html of the element was received", $"Не удалось найти или получить html из элемента по локатору: {locator}", $"Couldn't find or get html from the element by locator: {locator}");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementByIdAsync(string id)
        {
            int step = SendMessageDebug($"GetHtmlFromElementByIdAsync('{id}')", $"GetHtmlFromElementByIdAsync('{id}')", PROCESS, "Получение html из элемент", "Getting html from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementById('" + id + "'); return element.outerHTML; }());";
            string value = await execute(script, step, "Получено html элемента", "The html of the element was received", $"Не удалось найти или получить html из элемента с ID: {id}", $"Couldn't find or get html from an element with ID: {id}");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementByNameAsync(string name, int index)
        {
            int step = SendMessageDebug($"GetHtmlFromElementByNameAsync('{name}', '{index}')", $"GetHtmlFromElementByNameAsync('{name}', '{index}')", PROCESS, "Получение html из элемент", "Getting html from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, step, "Получено html элемента", "The html of the element was received", $"Не удалось найти или получить html из элемента Name: {name} (Index: {index})", $"Couldn't find or get html from the element by Name: {name} (Index: {index})");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementByTagAsync(string tag, int index)
        {
            int step = SendMessageDebug($"GetHtmlFromElementByTagAsync('{tag}', '{index}')", $"GetHtmlFromElementByTagAsync('{tag}', '{index}')", PROCESS, "Получение html из элемент", "Getting html from an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, step, "Получено html элемента", "The html of the element was received", $"Не удалось найти или получить html из элемента Tag: {tag} (Index: {index})", $"Couldn't find or get html from the element by Tag: {tag} (Index: {index})");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task SetHtmlInElementByClassAsync(string _class, int index, string html)
        {
            int step = SendMessageDebug($"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", $"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", PROCESS, "Ввод html в элемент", "Entering html into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Html {html} has been added to the element", $"Не удалось найти или ввести html в элемент по Class: {_class} (Index: {index})", $"Could not find or enter html in the element by Class: {_class} (Index: {index})");
        }

        public async Task SetHtmlInElementAsync(string by, string locator, string html)
        {
            int step = SendMessageDebug($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", PROCESS, "Ввод html в элемент", "Entering html into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else if (by == BY_XPATH) script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Html {html} has been added to the element", $"Не удалось найти или ввести html в элемент по локатору: {locator}", $"Could not find or enter html into the element by locator: {locator}");
        }

        public async Task SetHtmlInElementByIdAsync(string id, string html)
        {
            int step = SendMessageDebug($"SetHtmlInElementByIdAsync('{id}', '{html}')", $"SetHtmlInElementByIdAsync('{id}', '{html}')", PROCESS, "Ввод html в элемент", "Entering html into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Html {html} has been added to the element", $"Не удалось найти или ввести html в элемент с ID: {id}", $"Could not find or enter html in the element with ID: {id}");
        }

        public async Task SetHtmlInElementByNameAsync(string name, int index, string html)
        {
            int step = SendMessageDebug($"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", $"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", PROCESS, "Ввод html в элемент", "Entering html into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Html {html} has been added to the element", $"Не удалось найти или ввести html в элемент по Name: {name} (Index: {index})", $"Could not find or enter html in the element by Name: {name} (Index: {index})");
        }

        public async Task SetHtmlInElementByTagAsync(string tag, int index, string html)
        {
            int step = SendMessageDebug($"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", $"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", PROCESS, "Ввод html в элемент", "Entering html into an element", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Html {html} has been added to the element", $"Не удалось найти или ввести html в элемент по Tag: {tag} (Index: {index})", $"Could not find or enter html in the element by Tag: {tag} (Index: {index})");
        }

        public async Task<bool> IsClickableElementAsync(string by, string locator)
        {
            int step = SendMessageDebug($"IsClickableElementAsync(\"{by}\", \"{locator}\")", $"IsClickableElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Определяется кликабельность элемента", "The clickability of the element is determined", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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
                EditMessageDebug(step, null, null, PASSED, $"Определена кликадельность элемента: {result}", $"The clickability of the element was determined: {result}", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAsync(\"{url}\", \"{timeout}\", \"{charset}\")", PROCESS, "Выполнение Get Rest запроса", "Executing a Get Rest request", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
                    EditMessageDebug(step, null, null, PASSED, "Get Rest запрос - успешно выполнен", "Get Rest request - completed successfully", IMAGE_STATUS_PASSED);
                }
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Get Rest не выполнен " + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Get Rest failed " + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", $"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{timeout}\", \"{charset}\")", PROCESS, "Выполнение Get Rest запроса", "Executing a Get Rest request", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
                    EditMessageDebug(step, null, null, PASSED, "Get Rest запрос - успешно выполнен", "Get Rest request - completed successfully", IMAGE_STATUS_PASSED);
                }
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Get Rest не выполнен " + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Get Rest failed " + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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

            int step = SendMessageDebug($"RestGetStatusCodeAsync(\"{url}\")", $"RestGetStatusCodeAsync(\"{url}\")", PROCESS, "Выполнение Get Rest запроса и получение статуса", "Executing a Get Rest request and getting the status", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return result;

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

                EditMessageDebug(step, null, null, PASSED, "Get Rest запрос выполнен. Результат: " + result.ToString(), "Get Rest request completed. Result: " + result.ToString(), IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug($"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", $"RestPostAsync(\"{url}\", \"JSON\", \"{timeout}\", \"{charset}\")", PROCESS, "Выполнение Post Rest запроса", "Executing a Post Rest request", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

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
                    EditMessageDebug(step, null, null, PASSED, "Post Rest запрос успешно выполнен" + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Post Rest request completed successfully" + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_PASSED);
                }
                else
                {
                    EditMessageDebug(step, null, null, FAILED, "Post Rest не выполнен" + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(),
                        "Post Rest failed" + Environment.NewLine + "Request status: " + Environment.NewLine + response.StatusCode.ToString(),
                        IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug("TimerStart()", "TimerStart()", PROCESS, "Запуск таймера", "Starting the timer", IMAGE_STATUS_PROCESS);
            DateTime start = default;
            try
            {
                start = DateTime.Now;
                EditMessageDebug(step, null, null, COMPLETED, $"Таймер запущен {start}", $"The timer is running {start}", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
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
            int step = SendMessageDebug("TimerStop()", "TimerStop()", PROCESS, "Остановка таймера", "Stopping the timer", IMAGE_STATUS_PROCESS);
            DateTime stop = default;
            TimeSpan result = default;
            try
            {
                stop = DateTime.Now;
                result = (DateTime)stop - (DateTime)start;
                EditMessageDebug(step, null, null, COMPLETED, $"Таймер остановлен {stop} (затраченное время: {result})", $"Timer stopped {stop} (elapsed time: {result})", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                EditMessageDebug(step, null, null, Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }



        /* 
         * Методы для проверки результата ===========================================================
         * https://junit.org/junit4/javadoc/4.8/org/junit/Assert.html
         * */
        public async Task<bool> AssertEqualsAsync(dynamic expected, dynamic actual)
        {
            int step = SendMessageDebug("AssertEqualsAsync(" + expected + ", " + actual + ")", "AssertEqualsAsync(" + expected + ", " + actual + ")", PROCESS, "Проверка совпадения ожидаемого и актуального значения", "Checking whether the expected and actual values match", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            if (expected == actual)
            {
                EditMessageDebug(step, null, null, PASSED, "Ожидаемое и актуальное значение совпадают", "The expected and actual value are the same", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessageDebug(step, null, null, FAILED, "Ожидаемое и актуальное значение не совпадают", "The expected and actual value do not match", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNotEqualsAsync(dynamic expected, dynamic actual)
        {
            int step = SendMessageDebug("AssertNotEqualsAsync(" + expected + ", " + actual + ")", "AssertNotEqualsAsync(" + expected + ", " + actual + ")", PROCESS, "Проверка не совпадения ожидаемого и актуального значения", "Checking the discrepancy between the expected and actual values", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            if (expected != actual)
            {
                EditMessageDebug(step, null, null, PASSED, "Ожидаемое и актуальное значение не совпадают", "The expected and actual value do not match", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessageDebug(step, null, null, FAILED, "Ожидаемое и актуальное значение совпадают", "The expected and actual value are the same", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertTrueAsync(bool condition)
        {
            int step = SendMessageDebug("AssertTrueAsync(" + condition.ToString() + ")", "AssertTrueAsync(" + condition.ToString() + ")", PROCESS, "Проверка значения которое должно быть true", "Checking the value that should be true", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            if (condition == true)
            {
                EditMessageDebug(step, null, null, PASSED, "Проверенное значение соответствует true", "The verified value corresponds to true", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessageDebug(step, null, null, FAILED, "Проверенное значение соответствует false (должно быть true)", "The checked value corresponds to false (must be true)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertFalseAsync(bool condition)
        {
            int step = SendMessageDebug("AssertFalseAsync(" + condition.ToString() + ")", "AssertFalseAsync(" + condition.ToString() + ")", PROCESS, "Проверка значения которое должно быть false", "Checking the value that should be false", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            if (condition == false)
            {
                EditMessageDebug(step, null, null, PASSED, "Проверенное значение соответствует false", "The checked value corresponds to false", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessageDebug(step, null, null, FAILED, "Проверенное значение соответствует true (должно быть false)", "The checked value corresponds to true (must be false)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNotNullAsync(dynamic obj)
        {
            string value = "null";
            if (obj != null) value = obj.ToString();

            int step = SendMessageDebug("AssertNotNull(" + value + ")", "AssertNotNull(" + value + ")", PROCESS, "Проверка значения которое не должно быть null", "Checking a value that should not be null", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            if (obj != null)
            {
                EditMessageDebug(step, null, null, PASSED, "Проверенное значение не null", "The checked value is not null", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessageDebug(step, null, null, FAILED, "Проверенное значение null (должно быть не null)", "Checked value is null (must be non-null)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNullAsync(dynamic obj)
        {
            string value = "null";
            if (obj != null) value = obj.ToString();

            int step = SendMessageDebug("AssertNull(" + value + ")", "AssertNull(" + value + ")", PROCESS, "Проверка значения которое должно быть null", "Checking the value that should be null", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            if (obj == null)
            {
                EditMessageDebug(step, null, null, PASSED, "Проверенное значение null", "Verified value is null", IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessageDebug(step, null, null, FAILED, "Проверенное значение не null (должно быть null)", "The checked value is not null (must be null)", IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                return false;
            }
        }

        public async Task<bool> AssertNoErrorsAsync()
        {
            List<string> errors = await BrowserGetErrorsAsync();
            int step = SendMessageDebug("AssertNoErrors()", "AssertNoErrors()", PROCESS, "Проверка отсутствия ошибок в консоли", "Checking for errors in the console", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            int countErrors = 0;
            string textErrors = "";
            foreach (string error in errors)
            {
                if (error.Contains("stats.g.doubleclick.net") == true) continue;
                if (error.Contains("\"level\":\"error\"") == true)
                {
                    textErrors += error + Environment.NewLine;
                    countErrors++;
                }
            }

            bool result;
            if (countErrors > 0)
            {
                EditMessageDebug(step, null, null, FAILED, "В консоли присутствует " + countErrors.ToString() + " ошибок." + Environment.NewLine + textErrors,
                    "There are " + countErrors.ToString() + " errors in the console." + Environment.NewLine + textErrors,
                    Tester.IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
                result = false;
            }
            else
            {
                EditMessageDebug(step, null, null, PASSED, "Проверка завершена - ошибок в консоли нет", "The check is completed - there are no errors in the console", Tester.IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
                result = true;
            }
            return result;
        }

        /* presence = true проверить присутствие | presence = false проверить отсутствие (absence) */
        public async Task<bool> AssertNetworkEventsAsync(bool presence, string[] events)
        {
            string network = await BrowserGetNetworkAsync();
            int step = -1;
            if (presence == true) step = SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", PROCESS, "Проверка присутствия событий в Network", "Checking the presence of events in the Network", IMAGE_STATUS_PROCESS);
            else step = SendMessageDebug("AssertNetworkEventsAsync(" + presence + ", [...])", "AssertNetworkEventsAsync(" + presence + ", [...])", PROCESS, "Проверка отсутствия событий в Network", "Checking the presence of events in the Network", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

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
                if (presence == true) EditMessageDebug(step, null, null, PASSED, "Проверка завершена - все события присутствуют " + Environment.NewLine + reportRus, "Verification is complete - all events are present " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_PASSED);
                else EditMessageDebug(step, null, null, PASSED, "Проверка завершена - все события отсутствуют " + Environment.NewLine + reportRus, "Verification completed - all events are missing " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_PASSED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = PASSED;
            }
            else
            {
                if (presence == true) EditMessageDebug(step, null, null, FAILED, "В Network отсутствуют следующие события " + Environment.NewLine + reportRus, "The following events are missing in the Network " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_FAILED);
                else EditMessageDebug(step, null, null, FAILED, "В Network присутствуют следующие события " + Environment.NewLine + reportRus, "The following events are present in the Network " + Environment.NewLine + reportEng, Tester.IMAGE_STATUS_FAILED);
                if (assertStatus == null || assertStatus == PASSED) assertStatus = FAILED;
                TestStopAsync();
            }

            return result;
        }



    }
}
