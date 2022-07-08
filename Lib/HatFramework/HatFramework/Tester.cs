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
        public const string PASSED = "Успешно";
        public const string FAILED = "Провально";
        public const string STOPPED = "Остановлено";
        public const string PROCESS = "Выполняется";
        public const string COMPLETED = "Выполнено";
        public const string WARNING = "Предупреждение";
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


        private MethodInfo browserConsoleMsg;       // функция: consoleMsg - вывод сообщения в консоль приложения
        private MethodInfo browserConsoleMsgError;  // функция: consoleMsgErrorReport - вывод сообщения об ошибке в консоль приложения
        private MethodInfo browserSystemConsoleMsg; // функция: systemConsoleMsg - вывод сообщения в системную консоль
        private MethodInfo browserCleadMessageStep; // функция: cleadMessageStep - очистка всех шагов в таблице "тест"
        private MethodInfo browserSendMessageStep;  // функция: sendMessageStep - вывести сообщение в таблицу "тест"
        private MethodInfo browserEditMessageStep;  // функция: editMessageStep - изменить уже выведенное сообщение в таблице "тест"
        private MethodInfo browserResize;           // функция: browserResize - изменить размер браузера
        private MethodInfo browserUserAgent;        // функция: userAgent - настройка user-agent параметра
        private MethodInfo browserGetErrors;        // Функция: getBowserErrors - получить список ошибок и предупреждений браузера
        private MethodInfo checkStopTest;           // функция: checkStopTest - получить статус остановки процесса тестирования
        private MethodInfo resultAutotest;          // функция: resultAutotest - устанавливает флаг общего результата выполнения теста
        private MethodInfo debugJavaScript;         // функция: getDebug - возвращает статус отладки
        private MethodInfo getNameAutotest;         // Функция: getNameAutotest - возвращает имя запущенного автотеста
        private MethodInfo saveReport;              // функция: saveReport - вызывает метод сохранения отчета
        private MethodInfo saveReportScreenshotAsync; // функция: saveReportScreenshotAsync - сохраняет скриншот текущего состояния браузера

        private bool statusPageLoad = false;    // флаг: статус загрузки страницы
        private bool testStop = false;          // флаг: остановка теста
        private string assertStatus = null;     // флаг: рузельтат проверки

        public Tester(Form browserForm)
        {
            try
            {
                BrowserWindow = browserForm;
                browserConsoleMsg = BrowserWindow.GetType().GetMethod("consoleMsg");
                browserConsoleMsgError = BrowserWindow.GetType().GetMethod("consoleMsgErrorReport");
                browserSystemConsoleMsg = BrowserWindow.GetType().GetMethod("systemConsoleMsg");
                browserCleadMessageStep = BrowserWindow.GetType().GetMethod("cleadMessageStep");
                browserSendMessageStep = BrowserWindow.GetType().GetMethod("sendMessageStep");
                browserEditMessageStep = BrowserWindow.GetType().GetMethod("editMessageStep");
                browserResize = BrowserWindow.GetType().GetMethod("browserResize");
                browserUserAgent = BrowserWindow.GetType().GetMethod("userAgent");
                browserGetErrors = BrowserWindow.GetType().GetMethod("getBowserErrors");
                checkStopTest = BrowserWindow.GetType().GetMethod("checkStopTest");
                resultAutotest = BrowserWindow.GetType().GetMethod("resultAutotest");
                debugJavaScript = BrowserWindow.GetType().GetMethod("getStatusDebugJavaScript");
                getNameAutotest = BrowserWindow.GetType().GetMethod("getNameAutotest");
                saveReport = BrowserWindow.GetType().GetMethod("saveReport");
                saveReportScreenshotAsync = BrowserWindow.GetType().GetMethod("saveReportScreenshotAsync");

                MethodInfo mi = BrowserWindow.GetType().GetMethod("getWebView");
                BrowserView = (Microsoft.Web.WebView2.WinForms.WebView2)mi.Invoke(BrowserWindow, null);
                BrowserView.ContentLoading += contentLoading;
                BrowserView.EnsureCoreWebView2Async();

                showNameAutotest();
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        private void contentLoading(object sender, CoreWebView2ContentLoadingEventArgs e)
        {
            statusPageLoad = true;
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

        private void showNameAutotest()
        {
            try
            {
                int step = SendMessage("Message", PROCESS, $"Запуск автотеста", IMAGE_STATUS_MESSAGE);
                string filename = (string)getNameAutotest.Invoke(BrowserWindow, null);
                EditMessage(step, null, COMPLETED, $"Запущен автотест из файла: {filename}", IMAGE_STATUS_MESSAGE);
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
                script += "if (!(elem instanceof Element)) throw Error('DomUtil: elem is not an element.');";
                script += "const style = getComputedStyle(elem);";
                script += "if (style.display === 'none') return false;";
                script += "if (style.visibility !== 'visible') return false;";
                script += "if (style.opacity < 0.1) return false;";
                script += "if (elem.offsetWidth + elem.offsetHeight + elem.getBoundingClientRect().height + elem.getBoundingClientRect().width === 0) return false;";
                script += "const elemCenter = {";
                script += "x: elem.getBoundingClientRect().left + elem.offsetWidth / 2,";
                script += "y: elem.getBoundingClientRect().top + elem.offsetHeight / 2";
                script += "};";
                script += "if (elemCenter.x < 0) return false;";
                script += "if (elemCenter.x > (document.documentElement.clientWidth || window.innerWidth)) return false;";
                script += "if (elemCenter.y < 0) return false;";
                script += "if (elemCenter.y > (document.documentElement.clientHeight || window.innerHeight)) return false;";
                script += "let pointContainer = document.elementFromPoint(elemCenter.x, elemCenter.y);";
                script += "do {";
                script += "if (pointContainer === elem) return true;";
                script += "} while (pointContainer = pointContainer.parentNode);";
                script += "return false;";
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

        private async Task<string> execute(string script, int step, string commentPassed, string commentfailed)
        {
            string result = null;
            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, commentfailed, Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, Tester.PASSED, commentPassed, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
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
                string message;
                if (action != null)
                {
                    message = Environment.NewLine + "Действие: " + action;
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { message, default, default, default, true });
                }

                message = "Статус: ";
                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { message, default, default, default, false });
                if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, ConsoleColor.Black, ConsoleColor.DarkGreen, true });
                else if (status == FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                else if (status == WARNING) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true });
                else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, true });

                message = "Комментарий: " + comment;
                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { message, default, default, default, true });

                int index = (int)browserSendMessageStep.Invoke(BrowserWindow, new object[] { action, status, comment, image });
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
                string message;
                if (action != null)
                {
                    message = Environment.NewLine + "Действие: " + action;
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { message, default, default, default, true });
                }

                message = "Статус: ";
                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { message, default, default, default, false });
                if (status == PASSED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, ConsoleColor.Black, ConsoleColor.DarkGreen, true });
                else if (status == FAILED) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, ConsoleColor.Black, ConsoleColor.DarkRed, true });
                else if (status == WARNING) browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, ConsoleColor.Black, ConsoleColor.DarkYellow, true });
                else browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { status, default, default, default, true });

                message = "Комментарий: " + comment;
                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { message, default, default, default, true });

                browserEditMessageStep.Invoke(BrowserWindow, new object[] { index, action, status, comment, image });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
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
                int step = SendMessage("TestBeginAsync()", PROCESS, "Инициализация теста", IMAGE_STATUS_PROCESS);
                await BrowserView.EnsureCoreWebView2Async();
                Debug = (bool)debugJavaScript.Invoke(BrowserWindow, null);
                EditMessage(step, null, PASSED, "Выполнена инициализация теста", IMAGE_STATUS_PASSED);
                ConsoleMsg("Тест начинается...");

                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "" + Environment.NewLine, default, default, default, false });
                browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-- Тест начинается ------------", default, ConsoleColor.White, ConsoleColor.DarkBlue, true });
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
                int step = SendMessage("TestEndAsync()", PROCESS, "Завершение теста", IMAGE_STATUS_PROCESS);
                if (assertStatus == FAILED)
                {
                    ConsoleMsg("Тест завершен - провельно");
                    EditMessage(step, null, FAILED, "Тест завершен - шаги теста выполнены неуспешно", IMAGE_STATUS_FAILED);
                    resultAutotestSuccess(false);

                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { Environment.NewLine + "Тест завершен - провельно", default, ConsoleColor.DarkRed, ConsoleColor.White, true });
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-------------------------------" + Environment.NewLine, default, default, default, false });

                    Task screenshot = (Task)saveReportScreenshotAsync.Invoke(BrowserWindow, null);
                    await screenshot;
                }
                else
                {
                    ConsoleMsg("Тест завершен - успешено");
                    EditMessage(step, null, PASSED, "Тест завершен - все шаги выполнены успешно", IMAGE_STATUS_PASSED);
                    resultAutotestSuccess(true);

                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { Environment.NewLine + "Тест завершен - успешено", default, ConsoleColor.DarkGreen, ConsoleColor.White, true });
                    browserSystemConsoleMsg.Invoke(BrowserWindow, new object[] { "-------------------------------" + Environment.NewLine, default, default, default, false });
                }

                saveReport.Invoke(BrowserWindow, null);
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
                if (testStop == true && stepIndex < 0) SendMessage("CheckTestStop()", STOPPED, "Выполнение теста остановлено", IMAGE_STATUS_WARNING);
                if (testStop == true && stepIndex >= 0) EditMessage(stepIndex, null, STOPPED, "Выполнение шага остановлено", IMAGE_STATUS_WARNING);
                return testStop;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return testStop;
        }

        public async Task BrowserCloseAsync()
        {
            try
            {
                int step = SendMessage("BrowserCloseAsync()", PROCESS, "Браузер закрывается", IMAGE_STATUS_PROCESS);
                BrowserWindow.Close();
                EditMessage(step, null, COMPLETED, "Закрытие браузера - выполнено", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessage($"BrowserSizeAsync({width}, {height})", PROCESS, "Изменяется размер браузера", IMAGE_STATUS_PROCESS);
                browserResize.Invoke(BrowserWindow, new object[] { width, height });
                EditMessage(step, null, COMPLETED, "Размер браузера изменён", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessage($"BrowserFullScreenAsync()", PROCESS, "Изменяется размер браузера", IMAGE_STATUS_PROCESS);
                browserResize.Invoke(BrowserWindow, new object[] { -1, -1 });
                EditMessage(step, null, COMPLETED, "Размер браузера изменён", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessage($"BrowserSetUserAgentAsync({value})", PROCESS, "Изменяется значение User-Agent", IMAGE_STATUS_PROCESS);
                browserUserAgent.Invoke(BrowserWindow, new object[] { value });
                EditMessage(step, null, COMPLETED, "Значение User-Agent изменено", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessage($"BrowserGetUserAgentAsync()", PROCESS, "Получение значения User-Agent", IMAGE_STATUS_PROCESS);
                userAgent = BrowserView.CoreWebView2.Settings.UserAgent;
                EditMessage(step, null, COMPLETED, $"Из User-Agent получено значение: {userAgent}", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessage($"BrowserGetErrorsAsync()", PROCESS, "Получение списка ошибок и предупреждений браузера", IMAGE_STATUS_PROCESS);
                list = (List<string>)browserGetErrors.Invoke(BrowserWindow, null);
                EditMessage(step, null, COMPLETED, "Получен список ошибок и предупреждений браузера", IMAGE_STATUS_MESSAGE);
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
                int step = SendMessage($"BrowserGetNetworkAsync()", PROCESS, "Получение списка событий браузера (network)", IMAGE_STATUS_PROCESS);
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
                EditMessage(step, null, COMPLETED, "Получен список событий браузера (network)", IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return events;
        }

        public async Task BrowserGoBackAsync(int sec)
        {
            statusPageLoad = false;
            int step = SendMessage($"BrowserGoBackAsync()", PROCESS, "Выполняется действие браузера - назад", IMAGE_STATUS_PROCESS);
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

                if (statusPageLoad == true) EditMessage(step, null, PASSED, "Выполнено действие браузера - назад", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, "Не выполнено действие браузера - назад (cтраница не загружена)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserGoForwardAsync(int sec)
        {
            statusPageLoad = false;
            int step = SendMessage($"BrowserGoForwardAsync()", PROCESS, "Выполняется действие браузера - вперед", IMAGE_STATUS_PROCESS);
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

                if (statusPageLoad == true) EditMessage(step, null, PASSED, "Выполнено действие браузера - вперед", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, "Не выполнено действие браузера - вперед (cтраница не загружена)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserBasicAuthenticationAsync(string user, string pass)
        {
            statusPageLoad = false;
            int step = SendMessage($"BrowserBasicAuthentication(\"{user}\", \"{pass}\")", PROCESS, "Выполняется базовая авторизация", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                BrowserView.CoreWebView2.BasicAuthenticationRequested += delegate (object sender, CoreWebView2BasicAuthenticationRequestedEventArgs args)
                {
                    args.Response.UserName = "zion";
                    args.Response.Password = "newautotestreport";
                    EditMessage(step, null, COMPLETED, $"Баговая авторизация - выполнена", IMAGE_STATUS_MESSAGE);
                };
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> ExecuteJavaScriptAsync(string script)
        {
            string result = null;
            int step = SendMessage($"ExecuteJavaScriptAsync(\"{script}\")", PROCESS, "Выполнение скрипта", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return result;

            try
            {
                if (Debug == true) ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (Debug == true) ConsoleMsg($"[DEBUG] JS результат: {result}");
                EditMessage(step, null, PASSED, "Скрипт выполнен", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
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
            if (by == BY_CSS) step = SendMessage($"GetElementAsync(\"{by}\", '{locator}')", PROCESS, "Полечить элемента", IMAGE_STATUS_PROCESS);
            else step = SendMessage($"GetElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Полечить элемента", IMAGE_STATUS_PROCESS);
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
                    EditMessage(step, null, Tester.FAILED, $"Не удалось получить элемент {locator} ({by})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    htmlElement = new HTMLElement(this, by, locator);
                    htmlElement.Id = el.Id;
                    htmlElement.Name = el.Name;
                    htmlElement.Class = el.Class;
                    htmlElement.Type = el.Type;
                    EditMessage(step, null, PASSED, "Элемент получен", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return htmlElement;
        }

        public async Task GoToUrlAsync(string url, int sec)
        {
            statusPageLoad = false;
            int step = SendMessage($"GoToUrlAsync('{url}', {sec})", PROCESS, "Загрузка страницы", IMAGE_STATUS_PROCESS);
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

                if (statusPageLoad == true) EditMessage(step, null, PASSED, "Страница загружена", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, "Страница не загружена", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetUrlAsync()
        {
            int step = SendMessage("GetUrlAsync()", PROCESS, "Запрашивается текущий URL", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;
            string url = null;
            try
            {
                url = BrowserView.Source.ToString();
                EditMessage(step, null, PASSED, $"Получен текущий URL: {url}", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return url;
        }

        public async Task WaitAsync(int sec)
        {
            int step = SendMessage($"WaitAsync({sec})", PROCESS, $"Ожидание {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                await Task.Delay(sec * 1000);
                EditMessage(step, null, PASSED, $"Ожидание {sec.ToString()} секунд - завершено", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByIdAsync(string id, int sec)
        {
            int step = SendMessage($"WaitVisibleElementByIdAsync('{id}', {sec})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByClassAsync(string _class, int index, int sec)
        {
            int step = SendMessage($"WaitVisibleElementByClassAsync('{_class}', {index}, {sec})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByNameAsync(string name, int index, int sec)
        {
            int step = SendMessage($"WaitVisibleElementByNameAsync('{name}', {index}, {sec})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementByTagAsync(string tag, int index, int sec)
        {
            int step = SendMessage($"WaitVisibleElementByTagAsync('{tag}', {index}, {sec})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementAsync(string by, string locator, int sec)
        {
            int step = SendMessage($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание элемента {sec} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    if (by == BY_CSS) found = await isVisible(BY_CSS, locator);
                    else found = await isVisible(BY_XPATH, locator);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessage(step, null, PASSED, $"Ожидание элемента - завершено (элемент отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание элемента - завершено (элемент не отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByIdAsync(string id, int sec)
        {
            int step = SendMessage($"WaitNotVisibleElementByIdAsync('{id}', {sec})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == false) EditMessage(step, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByClassAsync(string _class, int index, int sec)
        {
            int step = SendMessage($"WaitNotVisibleElementByClassAsync('{_class}', {index}, {sec})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == false) EditMessage(step, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByNameAsync(string name, int index, int sec)
        {
            int step = SendMessage($"WaitNotVisibleElementByNameAsync('{name}', {index}, {sec})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == false) EditMessage(step, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementByTagAsync(string tag, int index, int sec)
        {
            int step = SendMessage($"WaitNotVisibleElementByTagAsync('{tag}', {index}, {sec})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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

                if (found == false) EditMessage(step, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleElementAsync(string by, string locator, int sec)
        {
            int step = SendMessage($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, $"Ожидание скрытия элемента {sec} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    if (by == BY_CSS) found = await isVisible(BY_CSS, locator);
                    else found = await isVisible(BY_XPATH, locator);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) EditMessage(step, null, PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", IMAGE_STATUS_PASSED);
                else
                {
                    EditMessage(step, null, FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<bool> FindElementByIdAsync(string id, int sec)
        {
            int step = SendMessage($"FindElementByIdAsync('{id}', {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementById('{id}');";
                script += "return elem;";
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementByClassAsync(string _class, int index, int sec)
        {
            int step = SendMessage($"FindElementByClassAsync('{_class}', {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementsByClassName('{_class}')[{index}];";
                script += "return elem;";
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementByNameAsync(string name, int index, int sec)
        {
            int step = SendMessage($"FindElementByNameAsync('{name}', {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementsByName('{name}')[{index}];";
                script += "return elem;";
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementByTagAsync(string tag, int index, int sec)
        {
            int step = SendMessage($"FindElementByTagAsync('{tag}', {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.getElementsByTagName('{tag}')[{index}];";
                script += "return elem;";
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindElementAsync(string by, string locator, int sec)
        {
            int step = SendMessage($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                if (by == BY_CSS) script += $"var elem = document.querySelector(\"{locator}\");";
                else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "return elem;";
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByIdAsync(string id, int sec)
        {
            int step = SendMessage($"FindVisibleElementByIdAsync('{id}', {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByClassAsync(string _class, int index, int sec)
        {
            int step = SendMessage($"FindVisibleElementByClassAsync('{_class}', {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByNameAsync(string name, int index, int sec)
        {
            int step = SendMessage($"FindVisibleElementByNameAsync('{name}', {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementByTagAsync(string tag, int index, int sec)
        {
            int step = SendMessage($"FindVisibleElementByTagAsync('{tag}', {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementAsync(string by, string locator, int sec)
        {
            int step = SendMessage($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    if (by == BY_CSS) found = await isVisible(BY_CSS, locator);
                    else found = await isVisible(BY_XPATH, locator);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) EditMessage(step, null, PASSED, "Поиск элемента - завершен (элемент найден)", IMAGE_STATUS_PASSED);
                else EditMessage(step, null, WARNING, "Поиск элемента - завершен (элемент не найден)", IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task ClickElementByIdAsync(string id)
        {
            int step = SendMessage($"ClickElementByIdAsync('{id}')", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementById('" + id + "'); element.click(); return element; }());";
            await execute(script, step, $"Элемент нажат", $"Не удалось найти элемент с ID: {id}");
        }

        public async Task ClickElementByClassAsync(string _class, int index)
        {
            int step = SendMessage($"ClickElementByClassAsync('{_class}', {index})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; element.click(); return element; }());";
            await execute(script, step, $"Элемент нажат", $"Не удалось найти элемент по Class: {_class} (Index: {index})");
        }

        public async Task ClickElementByNameAsync(string name, int index)
        {
            int step = SendMessage($"ClickElementByNameAsync('{name}', {index})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; element.click(); return element; }());";
            await execute(script, step, $"Элемент нажат", $"Не удалось найти элемент по Name: {name} (Index: {index})");
        }

        public async Task ClickElementByTagAsync(string tag, int index)
        {
            int step = SendMessage($"ClickElementByTagAsync('{tag}', {index})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; element.click(); return element; }());";
            await execute(script, step, $"Элемент нажат", $"Не удалось найти элемент по Tag: {tag} (Index: {index})");
        }

        public async Task ClickElementAsync(string by, string locator)
        {
            int step = SendMessage($"ClickElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); element.click(); return element;";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; element.click(); return element;";
            script += "}());";
            await execute(script, step, $"Элемент нажат", $"Не удалось найти элемент по локатору: {locator}");
        }

        public async Task SetValueInElementByIdAsync(string id, string value)
        {
            int step = SendMessage($"SetValueInElementByIdAsync('{id}', '{value}')", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
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
            await execute(script, step, $"Значение введено в элемент", $"Не удалось найти или ввести значение в элемент с ID: {id}");
        }

        public async Task SetValueInElementByClassAsync(string _class, int index, string value)
        {
            int step = SendMessage($"SetValueInElementByClassAsync('{_class}', {index}, '{value}')", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
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
            await execute(script, step, $"Значение введено в элемент", $"Не удалось найти или ввести значение в элемент по Class: {_class} (Index: {index})");
        }

        public async Task SetValueInElementByNameAsync(string name, int index, string value)
        {
            int step = SendMessage($"SetValueInElementByNameAsync('{name}', {index}, '{value}')", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
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
            await execute(script, step, $"Значение введено в элемент", $"Не удалось найти или ввести значение в элемент по Name: {name} (Index: {index})");
        }

        public async Task SetValueInElementByTagAsync(string tag, int index, string value)
        {
            int step = SendMessage($"SetValueInElementByTagAsync('{tag}', {index}, '{value}')", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
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
            await execute(script, step, $"Значение введено в элемент", $"Не удалось найти или ввести значение в элемент по Tag: {tag} (Index: {index})");
        }

        public async Task SetValueInElementAsync(string by, string locator, string value)
        {
            int step = SendMessage($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "element.value = '" + value + "';";
            script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
            script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
            script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            script += "return element.value;";
            script += "}());";
            await execute(script, step, $"Значение введено в элемент", $"Не удалось найти или ввести значение в элемент по локатору: {locator}");
        }

        public async Task<string> GetValueFromElementByIdAsync(string id)
        {
            int step = SendMessage($"GetValueFromElementByIdAsync('{id}')", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementById('" + id + "'); return element.value; }());";
            string value = await execute(script, step, $"Получено значение из элемента", $"Не удалось найти или получить данные из элемента с ID: {id}");
            return value;
        }

        public async Task<string> GetValueFromElementByClassAsync(string _class, int index)
        {
            int step = SendMessage($"GetValueFromElementByClassAsync('{_class}', {index})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.value; }());";
            string value = await execute(script, step, $"Получено значение из элемента", $"Не удалось найти или получить данные из элемента по Class: {_class} (Index: {index})");
            return value;
        }

        public async Task<string> GetValueFromElementByNameAsync(string name, int index)
        {
            int step = SendMessage($"GetValueFromElementByNameAsync('{name}', {index})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.value; }());";
            string value = await execute(script, step, $"Получено значение из элемента", $"Не удалось найти или получить данные из элемента по Name: {name} (Index: {index})");
            return value;
        }

        public async Task<string> GetValueFromElementByTagAsync(string tag, int index)
        {
            int step = SendMessage($"GetValueFromElementByTagAsync('{tag}', {index})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.value; }());";
            string value = await execute(script, step, $"Получено значение из элемента", $"Не удалось найти или получить данные из элемента по Tag: {tag} (Index: {index})");
            return value;
        }

        public async Task<string> GetValueFromElementAsync(string by, string locator)
        {
            int step = SendMessage($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); return element.value;";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.value;";
            script += "}());";
            string value = await execute(script, step, $"Получено значение из элемента", $"Не удалось найти или получить данные из элемента по локатору: {locator}");
            return value;
        }

        public async Task SetTextInElementByIdAsync(string id, string text)
        {
            int step = SendMessage($"SetTextInElementByIdAsync('{id}', '{text}')", PROCESS, "Ввод текста в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, $"Текст введен в элемент", $"Не удалось найти или ввести текст в элемент с ID: {id}");
        }

        public async Task SetTextInElementByClassAsync(string _class, int index, string text)
        {
            int step = SendMessage($"SetTextInElementByClassAsync('{_class}', {index}, '{text}')", PROCESS, "Ввод текста в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, $"Текст введен в элемент", $"Не удалось найти или ввести текст в элемент по Class: {_class} (Index: {index})");
        }

        public async Task SetTextInElementByNameAsync(string name, int index, string text)
        {
            int step = SendMessage($"SetTextInElementByNameAsync('{name}', {index}, '{text}')", PROCESS, "Ввод текста в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, $"Текст введен в элемент", $"Не удалось найти или ввести текст в элемент по Name: {name} (Index: {index})");
        }

        public async Task SetTextInElementByTagAsync(string tag, int index, string text)
        {
            int step = SendMessage($"SetTextInElementByTagAsync('{tag}', {index}, '{text}')", PROCESS, "Ввод текста в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, $"Текст введен в элемент", $"Не удалось найти или ввести текст в элемент по Tag: {tag} (Index: {index})");
        }

        public async Task SetTextInElementAsync(string by, string locator, string text)
        {
            int step = SendMessage($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", PROCESS, "Ввод текста в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, $"Текст введен в элемент", $"Не удалось найти или ввести текст в элемент по локатору: {locator}");
        }

        public async Task<string> GetTextFromElementByIdAsync(string id)
        {
            int step = SendMessage($"GetTextFromElementByIdAsync('{id}')", PROCESS, "Чтение текста из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementById('" + id + "'); return element.innerText; }());";
            string value = await execute(script, step, $"Прочитан текст из элемента", $"Не удалось найти или прочитать текст из элемента с ID: {id}");
            return value;
        }

        public async Task<string> GetTextFromElementByClassAsync(string _class, int index)
        {
            int step = SendMessage($"GetTextFromElementByClassAsync('{_class}', {index})", PROCESS, "Чтение текста из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.innerText; }());";
            string value = await execute(script, step, $"Прочитан текст из элемента", $"Не удалось найти или прочитать текст из элемента по Class: {_class} (Index: {index})");
            return value;
        }

        public async Task<string> GetTextFromElementByNameAsync(string name, int index)
        {
            int step = SendMessage($"GetTextFromElementByNameAsync('{name}', {index})", PROCESS, "Чтение текста из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.innerText; }());";
            string value = await execute(script, step, $"Прочитан текст из элемента", $"Не удалось найти или прочитать текст из элемента по Name: {name} (Index: {index})");
            return value;
        }

        public async Task<string> GetTextFromElementByTagAsync(string tag, int index)
        {
            int step = SendMessage($"GetTextFromElementByTagAsync('{tag}', {index})", PROCESS, "Чтение текста из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.innerText; }());";
            string value = await execute(script, step, $"Прочитан текст из элемента", $"Не удалось найти или прочитать текст из элемента по Tag: {tag} (Index: {index})");
            return value;
        }

        public async Task<string> GetTextFromElementAsync(string by, string locator)
        {
            int step = SendMessage($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", PROCESS, "Чтение текста из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){";
            if (by == BY_CSS) script += "var element = document.querySelector(\"" + locator + "\"); return element.innerText;";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.innerText;";
            script += "}());";
            string value = await execute(script, step, $"Прочитан текст из элемента", $"Не удалось найти или прочитать текст из элемента по локатору: {locator}");
            return value;
        }

        public async Task<int> GetCountElementsByClassAsync(string _class)
        {
            int step = SendMessage($"GetCountElementsByIdAsync('{_class}')", PROCESS, "Получение количества элементов", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "'); return element.length; }());";
            string result = await execute(script, step, $"Получение количества элементов", $"Не удалось найти или получить количество элементов по Class: {_class}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessage(step, null, PASSED, $"Количество элементов {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<int> GetCountElementsByNameAsync(string name)
        {
            int step = SendMessage($"GetCountElementsByNameAsync('{name}')", PROCESS, "Получение количества элементов", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){ var element = document.getElementsByName('" + name + "'); return element.length; }());";
            string result = await execute(script, step, $"Получение количества элементов", $"Не удалось найти или получить количество элементов по Name: {name}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessage(step, null, PASSED, $"Количество элементов {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<int> GetCountElementsByTagAsync(string tag)
        {
            int step = SendMessage($"GetCountElementsByTagAsync('{tag}')", PROCESS, "Получение количества элементов", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "'); return element.length; }());";
            string result = await execute(script, step, $"Получение количества элементов", $"Не удалось найти или получить количество элементов по Tag: {tag}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessage(step, null, PASSED, $"Количество элементов {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<int> GetCountElementsAsync(string by, string locator)
        {
            int step = SendMessage($"GetCountElementsAsync(\"{by}\", \"{locator}\")", PROCESS, "Получение количества элементов", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return -1;

            string script = "(function(){";
            if (by == BY_CSS) script += "var element = document.querySelectorAll(\"" + locator + "\"); return element.length;";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null); return element.snapshotLength;";
            script += "}());";
            string result = await execute(script, step, $"Получение количества элементов", $"Не удалось найти или получить количество элементов по локатору: {locator}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                EditMessage(step, null, PASSED, $"Количество элементов {result}", IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)
        {
            int step = SendMessage($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", PROCESS, "Прокрутить к элементу", IMAGE_STATUS_PROCESS);
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
                else
                {
                    script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                    if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'}); return element;";
                    else script += "element.scrollIntoView(); return element;";
                }
                script += "}());";
                string result = await executeJS(script);
                EditMessage(step, null, PASSED, "Прокрутил к элементу выполнена", IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetTitleAsync()
        {
            int step = SendMessage($"GetTitleAsync()", PROCESS, "Чтение текста из заголовка", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.querySelector('title'); return element.innerText; }());";
            string value = await execute(script, step, $"Прочитан текст из заголовка", $"Не удалось найти заголовок на странице");
            return value;
        }

        public async Task<string> GetAttributeFromElementByIdAsync(string id, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementByIdAsync('{id}', '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementById('" + id + "'); return element.getAttribute('" + attribute + "'); }());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента с ID: {id}");
            return value;
        }

        public async Task<string> GetAttributeFromElementByClassAsync(string _class, int index, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementByClassAsync('{_class}', {index}, '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по Class: {_class} (Index: {index})");
            return value;
        }

        public async Task<string> GetAttributeFromElementByNameAsync(string name, int index, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementByNameAsync('{name}', {index}, '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по Name: {name} (Index: {index})");
            return value;
        }

        public async Task<string> GetAttributeFromElementByTagAsync(string tag, int index, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementByTagAsync('{tag}', {index}, '{attribute}')", PROCESS, $"Получение аттрибута {attribute} из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.getAttribute('" + attribute + "'); }());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по Tag: {tag} (Index: {index})");
            return value;
        }

        public async Task<string> GetAttributeFromElementAsync(string by, string locator, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", PROCESS, $"Получение аттрибута {attribute} из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}");
            return value;
        }

        public async Task<List<string>> GetAttributeFromElementsByClassAsync(string _class, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementsByClassAsync('{_class}', '{attribute}')", PROCESS, $"Получение аттрибутов {attribute} из элементов", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Не удалось найти или получить аттрибуты из элементов по Class: {_class}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsByNameAsync(string name, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementsByNameAsync('{name}', '{attribute}')", PROCESS, $"Получение аттрибутов {attribute} из элементов", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Не удалось найти или получить аттрибуты из элементов по Name: {name}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsByTagAsync(string tag, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementsByTagAsync('{tag}', '{attribute}')", PROCESS, $"Получение аттрибутов {attribute} из элементов", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Не удалось найти или получить аттрибуты из элементов по Tag: {tag}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> GetAttributeFromElementsAsync(string by, string locator, string attribute)
        {
            int step = SendMessage($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", PROCESS, $"Получение аттрибутов {attribute} из элементов", IMAGE_STATUS_PROCESS);
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

            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Не удалось найти или получить аттрибуты из элементов по локатору: {locator}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Получен json {result} из аттрибутов {attribute}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task SetAttributeInElementByIdAsync(string id, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementByIdAsync('{id}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Не удалось найти или ввести аттрибут в элемент с ID: {id}");
        }

        public async Task SetAttributeInElementByClassAsync(string _class, int index, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementByClassAsync('{_class}', {index}, '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Не удалось найти или ввести аттрибут в элемент по Class: {_class} (Index: {index})");
        }

        public async Task SetAttributeInElementByNameAsync(string name, int index, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementByNameAsync('{name}', {index}, '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Не удалось найти или ввести аттрибут в элемент по Name: {name} (Index: {index})");
        }

        public async Task SetAttributeInElementByTagAsync(string tag, int index, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementByTagAsync('{tag}', {index}, '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Не удалось найти или ввести аттрибут в элемент по Tag: {tag} (Index: {index})");
        }

        public async Task SetAttributeInElementAsync(string by, string locator, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", PROCESS, "Добавление аттрибута в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Не удалось найти или ввести аттрибут в элемент по локатору: {locator}");
        }

        public async Task<List<string>> SetAttributeInElementsByClassAsync(string _class, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementsByClassAsync('{_class}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элементы", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Не удалось найти или добавить аттрибут в элементы по Class: {_class}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsByNameAsync(string name, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementsByNameAsync('{name}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элементы", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Не удалось найти или добавить аттрибут в элементы по Name: {name}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsByTagAsync(string tag, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementsByTagAsync('{tag}', '{attribute}', '{value}')", PROCESS, "Добавление аттрибута в элементы", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Не удалось найти или добавить аттрибут в элементы по Tag: {tag}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<List<string>> SetAttributeInElementsAsync(string by, string locator, string attribute, string value)
        {
            int step = SendMessage($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", PROCESS, "Добавление аттрибута в элементы", IMAGE_STATUS_PROCESS);
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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Не удалось найти или добавить аттрибут в элементы по локатору: {locator}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    EditMessage(step, null, PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                    TestStopAsync();
                    ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<string> GetHtmlFromElementByClassAsync(string _class, int index)
        {
            int step = SendMessage($"GetHtmlFromElementByClassAsync('{_class}', '{index}')", PROCESS, $"Получение html из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, step, $"Получено html элемента", $"Не удалось найти или получить html из элемента Class: {_class} (Index: {index})");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementAsync(string by, string locator)
        {
            int step = SendMessage($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", PROCESS, $"Получение html из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\"); return element.outerHTML;";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "return element.outerHTML;";
            script += "}());";
            string value = await execute(script, step, $"Получено html элемента", $"Не удалось найти или получить html из элемента по локатору: {locator}");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementByIdAsync(string id)
        {
            int step = SendMessage($"GetHtmlFromElementByIdAsync('{id}')", PROCESS, $"Получение html из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementById('" + id + "'); return element.outerHTML; }());";
            string value = await execute(script, step, $"Получено html элемента", $"Не удалось найти или получить html из элемента с ID: {id}");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementByNameAsync(string name, int index)
        {
            int step = SendMessage($"GetHtmlFromElementByNameAsync('{name}', '{index}')", PROCESS, $"Получение html из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, step, $"Получено html элемента", $"Не удалось найти или получить html из элемента Name: {name} (Index: {index})");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task<string> GetHtmlFromElementByTagAsync(string tag, int index)
        {
            int step = SendMessage($"GetHtmlFromElementByTagAsync('{tag}', '{index}')", PROCESS, $"Получение html из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.outerHTML; }());";
            string value = await execute(script, step, $"Получено html элемента", $"Не удалось найти или получить html из элемента Tag: {tag} (Index: {index})");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task SetHtmlInElementByClassAsync(string _class, int index, string html)
        {
            int step = SendMessage($"SetHtmlInElementByClassAsync('{_class}', {index}, '{html}')", PROCESS, "Ввод html в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByClassName('{_class}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Не удалось найти или ввести html в элемент по Class: {_class} (Index: {index})");
        }

        public async Task SetHtmlInElementAsync(string by, string locator, string html)
        {
            int step = SendMessage($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", PROCESS, "Ввод html в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (by == BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Не удалось найти или ввести html в элемент по локатору: {locator}");
        }

        public async Task SetHtmlInElementByIdAsync(string id, string html)
        {
            int step = SendMessage($"SetHtmlInElementByIdAsync('{id}', '{html}')", PROCESS, "Ввод html в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementById('{id}');";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Не удалось найти или ввести html в элемент с ID: {id}");
        }

        public async Task SetHtmlInElementByNameAsync(string name, int index, string html)
        {
            int step = SendMessage($"SetHtmlInElementByNameAsync('{name}', {index}, '{html}')", PROCESS, "Ввод html в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByName('{name}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Не удалось найти или ввести html в элемент по Name: {name} (Index: {index})");
        }

        public async Task SetHtmlInElementByTagAsync(string tag, int index, string html)
        {
            int step = SendMessage($"SetHtmlInElementByNameAsync('{tag}', {index}, '{html}')", PROCESS, "Ввод html в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var element = document.getElementsByTagName('{tag}')[{index}];";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Не удалось найти или ввести html в элемент по Tag: {tag} (Index: {index})");
        }





        /*
         * Методы для работы с REST запросами =======================================================
         * https://stackoverflow.com/questions/9620278/how-do-i-make-calls-to-a-rest-api-using-c
         * https://stackoverflow.com/questions/48977317/httpclient-post-with-parameters-in-body
         * https://docs.microsoft.com/en-us/dotnet/framework/network-programming/how-to-send-data-using-the-webrequest-class
         * https://zetcode.com/csharp/httpclient/
         * https://jsonplaceholder.typicode.com/
         */
        public async Task<string> RestGetAsync(string url, string charset = "UTF-8")
        {
            int step = SendMessage($"RestGetAsync(\"{url}\", \"{charset}\")", PROCESS, "Выполнение Get Rest запроса", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

            string result = null;
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
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
                    EditMessage(step, null, PASSED, "Get Rest запрос успешно выполнен", IMAGE_STATUS_PASSED);
                }
                else
                {
                    EditMessage(step, null, FAILED, "Get Rest не выполнен" + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(), IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task<string> RestGetBasicAuthAsync(string login, string pass, string url, string charset = "UTF-8")
        {
            int step = SendMessage($"RestGetAuthAsync(\"{login}\", \"{pass}\", \"{url}\", \"{charset}\")", PROCESS, "Выполнение Get Rest запроса", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

            string result = null;
            try
            {
                string userAgent = BrowserView.CoreWebView2.Settings.UserAgent;
                byte[] authToken = Encoding.ASCII.GetBytes($"{login}:{pass}");

                Uri uri = new Uri(url);
                HttpClient client = new HttpClient();
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
                    EditMessage(step, null, PASSED, "Get Rest запрос успешно выполнен", IMAGE_STATUS_PASSED);
                }
                else
                {
                    EditMessage(step, null, FAILED, "Get Rest не выполнен" + Environment.NewLine + "Статус запроса: " + Environment.NewLine + response.StatusCode.ToString(), IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }




        /* 
         * Методы для проверки результата ===========================================================
         * https://junit.org/junit4/javadoc/4.8/org/junit/Assert.html
         * */
        public async Task<bool> AssertEqualsAsync(string expected, string actual)
        {
            int step = SendMessage("AssertEqualsAsync(" + expected + ", " + actual + ")", PROCESS, "Проверка совпадения ожидаемого и актуального значения", IMAGE_STATUS_PROCESS);
            if (expected == actual)
            {
                EditMessage(step, null, PASSED, "Ожидаемое и актуальное значение совпадают", IMAGE_STATUS_PASSED);
                if (assertStatus == null) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessage(step, null, FAILED, "Ожидаемое и актуальное значение не совпадают", IMAGE_STATUS_FAILED);
                if (assertStatus == null) assertStatus = FAILED;
                return false;
            }
        }

        public async Task<bool> AssertNotEqualsAsync(string expected, string actual)
        {
            int step = SendMessage("AssertNotEqualsAsync(" + expected + ", " + actual + ")", PROCESS, "Проверка не совпадения ожидаемого и актуального значения", IMAGE_STATUS_PROCESS);
            if (expected != actual)
            {
                EditMessage(step, null, PASSED, "Ожидаемое и актуальное значение не совпадают", IMAGE_STATUS_PASSED);
                if (assertStatus == null) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessage(step, null, FAILED, "Ожидаемое и актуальное значение совпадают", IMAGE_STATUS_FAILED);
                if (assertStatus == null) assertStatus = FAILED;
                return false;
            }
        }

        public async Task<bool> AssertTrueAsync(bool condition)
        {
            int step = SendMessage("AssertTrueAsync(" + condition.ToString() + ")", PROCESS, "Проверка значения которое должно быть true", IMAGE_STATUS_PROCESS);
            if (condition == true)
            {
                EditMessage(step, null, PASSED, "Проверенное значение соответствует true", IMAGE_STATUS_PASSED);
                if (assertStatus == null) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessage(step, null, FAILED, "Проверенное значение соответствует false (должно быть true)", IMAGE_STATUS_FAILED);
                if (assertStatus == null) assertStatus = FAILED;
                return false;
            }
        }

        public async Task<bool> AssertFalseAsync(bool condition)
        {
            int step = SendMessage("AssertFalseAsync(" + condition.ToString() + ")", PROCESS, "Проверка значения которое должно быть false", IMAGE_STATUS_PROCESS);
            if (condition == false)
            {
                EditMessage(step, null, PASSED, "Проверенное значение соответствует false", IMAGE_STATUS_PASSED);
                if (assertStatus == null) assertStatus = PASSED;
                return true;
            }
            else
            {
                EditMessage(step, null, FAILED, "Проверенное значение соответствует true (должно быть false)", IMAGE_STATUS_FAILED);
                if (assertStatus == null) assertStatus = FAILED;
                return false;
            }
        }



    }
}
