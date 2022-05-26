using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

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
        public const string PASSED = "Успешно";
        public const string FAILED = "Провально";
        public const string STOPPED = "Остановлено";
        public const string PROCESS = "Выполняется";
        public const string COMPLETED = "Выполнено";
        public const string WARNING = "Предупреждение";

        public Form BrowserWindow;      // объект: окно приложения
        public WebView2 BrowserView;    // объект: браузер

        /* Локальные константы и переменные */
        private const string BY_ID = "BY_ID";
        private const string BY_CLASS = "BY_CLASS";
        private const string BY_NAME = "BY_NAME";
        private const string BY_TAG = "BY_TAG";
        private const string BY_CSS = "BY_CSS";

        private MethodInfo browserConsoleMsg;       // функция: consoleMsg - вывод сообщения в консоль приложения
        private MethodInfo browserConsoleMsgError;  // функция: consoleMsgError - вывод сообщения об ошибке в консоль приложения
        private MethodInfo browserSystemConsoleMsg; // функция: systemConsoleMsg - вывод сообщения в системную консоль
        private MethodInfo browserCleadMessageStep; // функция: cleadMessageStep - очистка всех шагов в таблице "тест"
        private MethodInfo browserSendMessageStep;  // функция: sendMessageStep - вывести сообщение в таблицу "тест"
        private MethodInfo browserEditMessageStep;  // функция: editMessageStep - изменить уже выведенное сообщение в таблице "тест"
        private MethodInfo browserResize;           // функция: browserResize - изменить размер браузера
        private MethodInfo browserUserAgent;        // функция: userAgent - настройка user-agent параметра
        private MethodInfo checkStopTest;           // функция: checkStopTest - получить статус остановки процесса тестирования
        private MethodInfo resultAutotest;          // функция: resultAutotest - устанавливает флаг общего результата выполнения теста
        
        private bool statusPageLoad = false;    // флаг: статус загрузки страницы
        private bool testStop = false;          // флаг: остановка теста
        private string assertStatus = null;     // флаг: рузельтат проверки

        public Tester(Form browserForm)
        {
            try
            {
                BrowserWindow = browserForm;
                browserConsoleMsg = BrowserWindow.GetType().GetMethod("consoleMsg");
                browserConsoleMsgError = BrowserWindow.GetType().GetMethod("consoleMsgError");
                browserSystemConsoleMsg = BrowserWindow.GetType().GetMethod("systemConsoleMsg");
                browserCleadMessageStep = BrowserWindow.GetType().GetMethod("cleadMessageStep");
                browserSendMessageStep = BrowserWindow.GetType().GetMethod("sendMessageStep");
                browserEditMessageStep = BrowserWindow.GetType().GetMethod("editMessageStep");
                browserResize = BrowserWindow.GetType().GetMethod("browserResize");
                browserUserAgent = BrowserWindow.GetType().GetMethod("userAgent");
                checkStopTest = BrowserWindow.GetType().GetMethod("checkStopTest");
                resultAutotest = BrowserWindow.GetType().GetMethod("resultAutotest");

                MethodInfo mi = BrowserWindow.GetType().GetMethod("getWebView");
                BrowserView = (Microsoft.Web.WebView2.WinForms.WebView2)mi.Invoke(BrowserWindow, null);
                BrowserView.ContentLoading += contentLoading;
                BrowserView.EnsureCoreWebView2Async();
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

        private async Task<bool> defineVisibleElementAsync(string by, string target, int index = default)
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
                if (by == BY_CSS) script += $"var elem = document.querySelector('{target}');";
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

                string result = await ExecuteJavaScriptAsync(script);
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

        public void ClearMessage()
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
                EditMessage(step, null, PASSED, "Выполнена инициализация теста", IMAGE_STATUS_PASSED);
                ConsoleMsg("Тест запущен");
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
                    EditMessage(step, null, FAILED, "Тест завершен - шаги теста выполнены неуспешно", IMAGE_STATUS_FAILED);
                    resultAutotestSuccess(false);
                }
                else
                {
                    EditMessage(step, null, PASSED, "Тест завершен - все шаги выполнены успешно", IMAGE_STATUS_PASSED);
                    resultAutotestSuccess(true);
                }
                ConsoleMsg("Тест завершен");
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
            BrowserWindow.Close();
        }

        public async Task BrowserSizeAsync(int width, int height)
        {
            try
            {
                browserResize.Invoke(BrowserWindow, new object[] { width, height });
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
                browserResize.Invoke(BrowserWindow, new object[] { -1, -1 });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task BrowserSetUserAgent(string value)
        {
            try
            {
                browserUserAgent.Invoke(BrowserWindow, new object[] { value });
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> BrowserGetUserAgent()
        {
            string userAgent = null;
            try
            {
                userAgent = BrowserView.CoreWebView2.Settings.UserAgent;
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return userAgent;
        }

        public async Task<string> ExecuteJavaScriptAsync(string script)
        {
            string result = null;
            try
            {
                result = await BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                //ConsoleMsg($"Метод ExecuteJavaScriptAsync вернул значение: {result}");
            }
            catch (Exception ex)
            {
                ConsoleMsgError(ex.ToString());
            }
            return result;
        }


        /* 
         * Методы для выполнения действий ============================================================
         * */
        public async Task<HTMLElement> GetHtmlElementAsync(string locator)
        {
            int step = SendMessage($"GetHtmlElementAsync({locator})", PROCESS, "Полечить элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return null;

            HTMLElement htmlElement = new HTMLElement(this);
            try
            {
                HTMLElement el = null;
                var obj = await BrowserView.CoreWebView2.ExecuteScriptAsync("(function(locatorCss = '" + locator + "'){ var el = document.querySelector(locatorCss); var obj = { Locator: locatorCss, Id: el.id, Class: el.class, Name: el.name, Value: el.value }; return obj; }());");
                el = JsonConvert.DeserializeObject<HTMLElement>(obj);
                if (el == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось получить элемент {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    htmlElement = new HTMLElement(this);
                    htmlElement.Locator = el.Locator;
                    htmlElement.Id = el.Id;
                    htmlElement.Name = el.Name;
                    htmlElement.Class = el.Class;
                    htmlElement.Value = el.Value;
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
            int step = SendMessage($"GoToUrlAsync({url}, {sec})", PROCESS, "Загрузка страницы", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                if (BrowserView.Source.ToString() == url)
                {
                    BrowserView.Reload();
                }
                else
                {
                    BrowserView.Source = new Uri(url);
                    BrowserView.Update();
                }

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
            int step = SendMessage($"WaitAsync({sec.ToString()})", PROCESS, $"Ожидание {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
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
            int step = SendMessage($"WaitVisibleElementByIdAsync({id}, {sec.ToString()})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_ID, id);
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
            int step = SendMessage($"WaitVisibleElementByClassAsync({_class}, {index}, {sec.ToString()})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_CLASS, _class, index);
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
            int step = SendMessage($"WaitVisibleElementByNameAsync({name}, {index}, {sec.ToString()})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_NAME, name, index);
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
            int step = SendMessage($"WaitVisibleElementByTagAsync({tag}, {index}, {sec.ToString()})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_TAG, tag, index);
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

        public async Task WaitVisibleElementByCssAsync(string locator, int sec)
        {
            int step = SendMessage($"WaitVisibleElementByCssAsync({locator}, {sec.ToString()})", PROCESS, $"Ожидание элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_CSS, locator);
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
            int step = SendMessage($"WaitNotVisibleElementByIdAsync({id}, {sec.ToString()})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_ID, id);
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
            int step = SendMessage($"WaitNotVisibleElementByClassAsync({_class}, {index}, {sec.ToString()})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_CLASS, _class, index);
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
            int step = SendMessage($"WaitNotVisibleElementByNameAsync({name}, {index}, {sec.ToString()})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_NAME, name, index);
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
            int step = SendMessage($"WaitNotVisibleElementByTagAsync({tag}, {index}, {sec.ToString()})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_TAG, tag, index);
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

        public async Task WaitNotVisibleElementByCssAsync(string locator, int sec)
        {
            int step = SendMessage($"WaitNotVisibleElementByCssAsync({locator}, {sec.ToString()})", PROCESS, $"Ожидание скрытия элемента {sec.ToString()} секунд", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;
            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_CSS, locator);
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
            int step = SendMessage($"FindElementByIdAsync({id}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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
                    result = await ExecuteJavaScriptAsync(script);
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
            int step = SendMessage($"FindElementByClassAsync({_class}, {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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
                    result = await ExecuteJavaScriptAsync(script);
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
            int step = SendMessage($"FindElementByNameAsync({name}, {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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
                    result = await ExecuteJavaScriptAsync(script);
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
            int step = SendMessage($"FindElementByTagAsync({tag}, {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
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
                    result = await ExecuteJavaScriptAsync(script);
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

        public async Task<bool> FindElementByCssAsync(string locator, int sec)
        {
            int step = SendMessage($"FindElementByCssAsync({locator}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var elem = document.querySelector('{locator}');";
                script += "return elem;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    result = await ExecuteJavaScriptAsync(script);
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
            int step = SendMessage($"FindVisibleElementByIdAsync({id}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_ID, id);
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
            int step = SendMessage($"FindVisibleElementByClassAsync({_class}, {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_CLASS, _class, index);
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
            int step = SendMessage($"FindVisibleElementByNameAsync({name}, {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_NAME, name, index);
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
            int step = SendMessage($"FindVisibleElementByTagAsync({tag}, {index}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_TAG, tag, index);
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

        public async Task<bool> FindVisibleElementByCssAsync(string locator, int sec)
        {
            int step = SendMessage($"FindVisibleElementByCssAsync({locator}, {sec})", PROCESS, "Поиск элемента", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return false;

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    found = await defineVisibleElementAsync(BY_CSS, locator);
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
            int step = SendMessage($"ClickElementByIdAsync({id})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); element.click(); return element; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент с ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, "Нажат элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task ClickElementByClassAsync(string _class, int index)
        {
            int step = SendMessage($"ClickElementByClassAsync({_class}, {index})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; element.click(); return element; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось нажать на элемент по Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, "Нажат элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task ClickElementByNameAsync(string name, int index)
        {
            int step = SendMessage($"ClickElementByNameAsync({name}, {index})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; element.click(); return element; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, "Нажат элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task ClickElementByTagAsync(string tag, int index)
        {
            int step = SendMessage($"ClickElementByTagAsync({tag}, {index})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; element.click(); return element; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, "Нажат элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task ClickElementByCssAsync(string locator)
        {
            int step = SendMessage($"ClickElementByCssAsync({locator})", PROCESS, "Нажатие на элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){ var element = document.querySelector('" + locator + "'); element.click(); return element; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по локатору: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, "Нажат элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SetValueInElementByIdAsync(string id, string value)
        {
            int step = SendMessage($"SetValueInElementByIdAsync({id}, {value})", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){";
                script += "var element = document.getElementById('" + id + "');";
                script += "element.value = '" + value + "';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value";
                script += "}());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти или ввести значение в элемент с ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, $"Значение {result} - введено в элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SetValueInElementByClassAsync(string _class, int index, string value)
        {
            int step = SendMessage($"SetValueInElementByClassAsync({_class}, {index}, {value})", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){";
                script += "var element = document.getElementsByClassName('" + _class + "')[" + index + "];";
                script += "element.value = '" + value + "';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value";
                script += "}());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти или ввести значение в элемент по Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, $"Значение {result} - введено в элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SetValueInElementByNameAsync(string name, int index, string value)
        {
            int step = SendMessage($"SetValueInElementByNameAsync({name}, {index}, {value})", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){";
                script += "var element = document.getElementsByName('" + name + "')[" + index + "];";
                script += "element.value = '" + value + "';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value";
                script += "}());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти или ввести значение в элемент по Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, $"Значение {result} - введено в элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SetValueInElementByTagAsync(string tag, int index, string value)
        {
            int step = SendMessage($"SetValueInElementByTagAsync({tag}, {index}, {value})", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){";
                script += "var element = document.getElementsByTagName('" + tag + "')[" + index + "];";
                script += "element.value = '" + value + "';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value";
                script += "}());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти или ввести значение в элемент по Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, $"Значение {result} - введено в элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SetValueInElementByCssAsync(string locator, string value)
        {
            int step = SendMessage($"SetValueInElementByCssAsync({locator}, {value})", PROCESS, "Ввод значения в элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return;

            try
            {
                string script = "(function(){";
                script += "var element = document.querySelector('" + locator + "');";
                script += "element.value = '" + value + "';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value";
                script += "}());";

                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null || result == "")
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти или ввести значение в элемент по локатору: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    EditMessage(step, null, PASSED, $"Значение {result} - введено в элемент", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<string> GetValueFromElementByIdAsync(string id)
        {
            int step = SendMessage($"GetValueFromElementByIdAsync({id})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementById('" + id + "'); return element.value; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null || result == "")
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти или получить данные из элемента с ID: {id}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    value = result;
                    EditMessage(step, null, PASSED, $"Получено значение {result} из элемента", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByClassAsync(string _class, int index)
        {
            int step = SendMessage($"GetValueFromElementByClassAsync({_class}, {index})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByClassName('" + _class + "')[" + index + "]; return element.value; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по Class: {_class} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    value = result;
                    EditMessage(step, null, PASSED, $"Получено значение {result} из элемента", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByNameAsync(string name, int index)
        {
            int step = SendMessage($"GetValueFromElementByNameAsync({name}, {index})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByName('" + name + "')[" + index + "]; return element.value; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по Name: {name} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    value = result;
                    EditMessage(step, null, PASSED, $"Получено значение {result} из элемента", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByTagAsync(string tag, int index)
        {
            int step = SendMessage($"GetValueFromElementByTagAsync({tag}, {index})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.getElementsByTagName('" + tag + "')[" + index + "]; return element.value; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по Tag: {tag} (Index: {index})", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    value = result;
                    EditMessage(step, null, PASSED, $"Получено значение {result} из элемента", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<string> GetValueFromElementByCssAsync(string locator)
        {
            int step = SendMessage($"GetValueFromElementByCSSAsync({locator})", PROCESS, "Получение значения из элемент", IMAGE_STATUS_PROCESS);
            if (DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){ var element = document.querySelector('" + locator + "'); return element.value; }());";
                string result = await ExecuteJavaScriptAsync(script);
                if (result == "null" || result == null)
                {
                    EditMessage(step, null, Tester.FAILED, $"Не удалось найти элемент по локатору: {locator}", Tester.IMAGE_STATUS_FAILED);
                    TestStopAsync();
                }
                else
                {
                    value = result;
                    EditMessage(step, null, PASSED, $"Получено значение {result} из элемента", IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                EditMessage(step, null, FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), IMAGE_STATUS_FAILED);
                TestStopAsync();
                ConsoleMsgError(ex.ToString());
            }
            return value;
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
