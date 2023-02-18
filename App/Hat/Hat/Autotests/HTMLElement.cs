using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatFrameworkDev
{
    public class HTMLElement
    {
        private Tester _tester;
        private string _by;
        private string _locator;

        public const string BY_INDEX = "BY_INDEX";
        public const string BY_TEXT = "BY_TEXT";
        public const string BY_VALUE = "BY_VALUE";

        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        

        public HTMLElement(Tester tester, string by, string locator)
        {
            _tester = tester;
            _by = by;
            _locator = locator;
        }

        private async Task<string> execute(string script, int step, string commentPassedRus, string commentPassedEng, string commentfailedRus, string commentfailedEng)
        {
            string result = null;
            try
            {
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result == null || result == "null")
                {
                    _tester.EditMessageDebug(step, null, null, Tester.FAILED, 
                        $"{commentfailedRus} " + Environment.NewLine + $"Результат выполнения скрипта: {result}", 
                        $"{commentfailedEng} " + Environment.NewLine + $"The result of the script execution: {result}", 
                        Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else 
                {
                    _tester.EditMessageDebug(step, null, null, Tester.PASSED, 
                        $"{commentPassedRus} " + Environment.NewLine + $"Результат выполнения скрипта: {result}",
                        $"{commentPassedEng} " + Environment.NewLine + $"The result of the script execution: {result}",
                        Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        private async Task<bool> isVisible()
        {
            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                if (_by == Tester.BY_CSS) script += $"var elem = document.querySelector(\"{_locator}\");";
                if (_by == Tester.BY_XPATH) script += $"var elem = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
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

                string result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result != "null" && result != null && result == "true") found = true;
                else found = false;
            }
            catch (Exception ex)
            {
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task ClickAsync()
        {
            int step = _tester.SendMessageDebug("ClickAsync()", "ClickAsync()", Tester.PROCESS, "Нажатие на элемент", "Clicking on an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += "element.click();";
                script += "return element;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "element.click();";
                script += "return element;";
                script += "}());";
            }
            await execute(script, step, "Элемент нажат", "The element is pressed", "Не удалось нажать на элемент", "Failed to click on the element");
        }

        public async Task ClickMouseAsync()
        {
            int step = _tester.SendMessageDebug("ClickMouseAsync()", "ClickMouseAsync()", Tester.PROCESS, "Нажатие (mouse) на элемент", "Clicking (mouse) on an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += "var clickEvent = new MouseEvent(\"click\", { \"view\": window, \"bubbles\": true, \"cancelable\": false });";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(clickEvent);";
                script += "return element;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += "var clickEvent = new MouseEvent(\"click\", { \"view\": window, \"bubbles\": true, \"cancelable\": false });";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(clickEvent);";
                script += "return element;";
                script += "}());";
            }
            await execute(script, step, "Элемент нажат (mouse)", "The element is pressed (mouse)", "Не удалось нажать (mouse) на элемент", "Failed to click (mouse) on the element");
        }

        public async Task<string> GetTextAsync()
        {
            int step = _tester.SendMessageDebug("GetTextAsync()", "GetTextAsync()", Tester.PROCESS, "Чтение текста из элемент", "Reading text from an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string result = "";
            try
            {
                string script = null;
                if (_by == Tester.BY_CSS)
                {
                    script = "(function(){";
                    script += $"var element = document.querySelector(\"{_locator}\"); ";
                    script += "if(element.outerText == '' && element.value != null) { return element.value; } ";
                    script += "else { return element.outerText; } ";
                    script += "}());";
                }
                else if (_by == Tester.BY_XPATH)
                {
                    script = "(function(){";
                    script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                    script += "if(element.outerText == '' && element.value != null) { return element.value; } ";
                    script += "else { return element.outerText; } ";
                    script += "}());";
                }
                result = await execute(script, step, "Текст из элемента прочитан", "Text from the read element", "Не удалось прочитать текст из элемента", "Could not read the text from the element");
                if (result.Length > 1) result = result.Substring(1, result.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            if (result == "") _tester.EditMessageDebug(step, null, null, Tester.COMPLETED, "Не удалось получить текст из элемента", "Couldn't get the text from the element", Tester.IMAGE_STATUS_WARNING);
            return result;
        }

        public async Task SetTextAsync(string text)
        {
            int step = _tester.SendMessageDebug($"SetTextAsync(\"{text}\")", $"SetTextAsync(\"{text}\")", Tester.PROCESS, "Ввод текста в элемент", "Entering text into an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += $"element.innerText = '{text}';";
                script += "return element.outerText;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.innerText = '{text}';";
                script += "return element.outerText;";
                script += "}());";
            }
            await execute(script, step, "Текст введен в элемент", "The text is entered in the element", "Не удалось ввести текст в элемент", "Could not enter text in the element");
        }

        public async Task<string> GetValueAsync()
        {
            int step = _tester.SendMessageDebug("GetValueAsync()", "GetValueAsync()", Tester.PROCESS, "Чтение значения из элемент", "Reading a value from an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string result = "";
            try
            {
                string script = null;
                if (_by == Tester.BY_CSS)
                {
                    script = "(function(){";
                    script += $"var element = document.querySelector(\"{_locator}\");";
                    script += "return element.value;";
                    script += "}());";
                }
                else if (_by == Tester.BY_XPATH)
                {
                    script = "(function(){";
                    script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                    script += "return element.value;";
                    script += "}());";
                }
                result = await execute(script, step, "Прочитано значение из элемента", "Read the value from the element", "Не удалось прочитать значение элемента", "The value of the element could not be read");
                if (result.Length > 1) result = result.Substring(1, result.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task SetValueAsync(string value)
        {
            int step = _tester.SendMessageDebug($"SetValueAsync(\"{value}\")", $"SetValueAsync(\"{value}\")", Tester.PROCESS, "Ввод значения в элемент", "Entering a value into an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += $"element.value = '{value}';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.value = '{value}';";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "return element.value;";
                script += "}());";
            }
            await execute(script, step, "Значение введено в элемент", "The value is entered in the element", "Не удалось ввести значение в элемент", "Failed to enter a value in the element");
        }

        public async Task<string> GetAttributeAsync(string name)
        {
            int step = _tester.SendMessageDebug($"GetAttributeAsync('{name}')", $"GetAttributeAsync('{name}')", Tester.PROCESS, "Получение атрибута из элемента", "Getting an attribute from an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string result = "";
            try
            {
                string script = null;
                if (_by == Tester.BY_CSS)
                {
                    script = "(function(){";
                    script += $"var element = document.querySelector(\"{_locator}\");";
                    script += $"return element.getAttribute('{name}');";
                    script += "}());";
                }
                else if (_by == Tester.BY_XPATH)
                {
                    script = "(function(){";
                    script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                    script += $"return element.getAttribute('{name}');";
                    script += "}());";
                }
                result = await execute(script, step, "Атрибут из элемента прочитан", "Attribute from the element read", "Не удалось прочитать атрибут из элемента", "Failed to read attribute from element");
                if (result.Length > 1) result = result.Substring(1, result.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task SetAttributeAsync(string name, string value)
        {
            int step = _tester.SendMessageDebug($"SetAttributeAsync('{name}', '{value}')", $"SetAttributeAsync('{name}', '{value}')", Tester.PROCESS, "Ввод атрибута в элемент", "Entering an attribute into an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += $"element.setAttribute('{name}', '{value}');";
                script += $"return element.getAttribute('{name}');";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.setAttribute('{name}', '{value}');";
                script += $"return element.getAttribute('{name}');";
                script += "}());";
            }
            await execute(script, step, "Атрибут введен в элемент", "The attribute is entered into the element", "Не удалось ввести атрибут в элемент", "Failed to enter attribute in element");
        }

        public async Task<string> GetHtmlAsync()
        {
            int step = _tester.SendMessageDebug($"GetHtmlAsync()", $"GetHtmlAsync()", Tester.PROCESS, "Получение html из элемент", "Getting html from an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += $"return element.outerHTML;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"return element.outerHTML;";
                script += "}());";
            }
            string result = await execute(script, step, "Получен html из элемента", "Html is obtained from the element", "Не удалось получить html из элемента", "Could not get html from the element");
            result = result.Replace("\\u003C", "<");
            result = result.Replace("\\u003E", ">");
            return result;
        }

        public async Task SetHtmlAsync(string html)
        {
            int step = _tester.SendMessageDebug($"SetHtmlAsync('{html}')", $"SetHtmlAsync('{html}')", Tester.PROCESS, "Ввод html в элемент", "Entering html into an element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += $"element.innerHTML = '{html}';";
                script += "return element.outerHTML;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.innerHTML = '{html}';";
                script += "return element.outerHTML;";
                script += "}());";
            }
            await execute(script, step, "В элемент введен html", "Html has been entered into the element", "Не удалось ввести html в элемент", "Could not enter html into the element");
        }

        public async Task ScrollToAsync(bool behaviorSmooth = false)
        {
            int step = _tester.SendMessageDebug($"ScrollToAsync('{behaviorSmooth}')", $"ScrollToAsync('{behaviorSmooth}')", Tester.PROCESS, "Прокрутить к элементу", "Fasten to the element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'});";
                else script += "element.scrollIntoView();";
                script += $"return element;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'});";
                else script += "element.scrollIntoView();";
                script += $"return element;";
                script += "}());";
            }
            await execute(script, step, "Прокрутка к элементу завершена", "Scrolling to the item is complete", "Не удалось прокрутить к элементу", "Failed to fasten to the element");
        }

        public async Task WaitVisibleAsync(int sec)
        {
            int step = _tester.SendMessageDebug($"WaitVisibleAsync({sec})", $"WaitVisibleAsync({sec})", Tester.PROCESS, $"Ожидание элемента {sec} секунд", $"Waiting for an element for {sec} seconds", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible();
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) _tester.EditMessageDebug(step, null, null, Tester.PASSED, $"Ожидание элемента - завершено (элемент отображается)", "Waiting for an item - completed (the item is displayed)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.EditMessageDebug(step, null, null, Tester.FAILED, $"Ожидание элемента - завершено (элемент не отображается)", "Waiting for an item - completed (the item is not displayed)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleAsync(int sec)
        {
            int step = _tester.SendMessageDebug($"WaitNotVisibleAsync({sec})", $"WaitNotVisibleAsync({sec})", Tester.PROCESS, $"Ожидание скрытия элемента {sec} секунд", $"Waiting for the element to be hidden for {sec} seconds", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible();
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) _tester.EditMessageDebug(step, null, null, Tester.PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", "Waiting for the element to be hidden - completed (the element is not displayed)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.EditMessageDebug(step, null, null, Tester.FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", "Waiting for the element to be hidden - completed (the element is displayed)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessageDebug(step, null, null, Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SelectOptionAsync(string by, string value)
        {
            int step = _tester.SendMessageDebug($"SelectOptionAsync({by}, {value})", $"SelectOptionAsync({by}, {value})", Tester.PROCESS, $"Выбирается опция", "The option is selected", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (_by == Tester.BY_CSS) script += $"var element = document.querySelector(\"{_locator}\");";
            else if (_by == Tester.BY_XPATH) script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (by == BY_INDEX)
            {
                script += $"element.options[{value}].selected = true;";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            }
            if (by == BY_TEXT)
            {
                script += "for (var i = 0; i < element.options.length; ++i) {";
                script += $"if (element.options[i].text === '{value}')";
                script += "{";
                script += "element.options[i].selected = true;";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "break;";
                script += "}";
                script += "}";
            }
            if (by == BY_VALUE)
            {
                script += "for (var i = 0; i < element.options.length; ++i) {";
                script += $"if (element.options[i].value === '{value}')";
                script += "{";
                script += "element.options[i].selected = true;";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
                script += "break;";
                script += "}";
                script += "}";
            }
            script += "return element;";
            script += "}());";
            await execute(script, step, "Опцыя выбрана", "Option selected", "Не удалось выбрать опцию", "Failed to select an option");
        }

        public async Task<string> GetOptionAsync(string by)
        {
            int step = _tester.SendMessageDebug($"GetOntionAsync()", $"GetOntionAsync()", Tester.PROCESS, "Получение данных выбранной опции", "Getting the data of the selected option", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = "(function(){";
            if (_by == Tester.BY_CSS) script += $"var element = document.querySelector(\"{_locator}\");";
            else if (_by == Tester.BY_XPATH) script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (by == BY_INDEX) script += "return element.selectedIndex;";
            if (by == BY_TEXT) script += "return element.options[element.selectedIndex].text;";
            if (by == BY_VALUE) script += "return element.options[element.selectedIndex].value;";
            script += "}());";

            string result = await execute(script, step, "Получен индекс или текст или значение из выбранной опции", "The index or text or value from the selected option is received", "Не удалось получить индекс или текст или значение из выбранной опции", "Could not get index or text or value from selected option");
            return result;
        }

        public async Task<bool> IsClickableAsync()
        {
            int step = _tester.SendMessageDebug($"IsClickableAsync()", $"IsClickableAsync()", Tester.PROCESS, "Определяется кликабельность элемента", "The clickability of the element is determined", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return false;

            bool clickable = false;
            string script = "";
            script += "(function(){ ";
            if (_by == Tester.BY_CSS) script += $"var elem = document.querySelector(\"{_locator}\");";
            if (_by == Tester.BY_XPATH) script += $"var elem = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "if((elem.getAttribute('onclick')!=null)||(elem.getAttribute('href')!=null)) return true;";
            script += "return false;";
            script += "}());";

            string result = await execute(script, step, "Определена кликадельность элемента", "The clickability of the element is determined", "Не удалось определить кликабельность элемента", "The clickability of the element could not be determined");
            if (result != "null" && result != null && result == "true") clickable = true;
            else clickable = false;
            return clickable;
        }

        public async Task<string> GetLocatorAsync()
        {
            int step = _tester.SendMessageDebug($"GetLocatorAsync()", $"GetLocatorAsync()", Tester.PROCESS, "Получить локатор элемента", "Get the element locator", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

            _tester.SendMessageDebug($"GetLocatorAsync()", $"GetLocatorAsync()", Tester.PASSED, "Получен локатор элемента: " + _locator, "The element locator is received: " + _locator, Tester.IMAGE_STATUS_PASSED);
            return _locator;
        }

    }
}
