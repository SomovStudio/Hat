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

        private async Task<string> execute(string script, string action)
        {
            string result = null;
            try
            {
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result == "null" || result == null)
                {
                    _tester.SendMessageDebug(action, action, Tester.FAILED, 
                        $"В результате выполнения JavaScript получено NULL. Неудалось корректно выполнить JavaScript: {script}" + Environment.NewLine + $"Результат выполнения скрипта: {result}", 
                        $"The result of JavaScript execution is NULL. Failed to execute JavaScript correctly: {script}" + Environment.NewLine + $"The result of the script execution: {result}", 
                        Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                result = "null";
                _tester.SendMessageDebug(action, action, Tester.FAILED,
                    "Ошибка при выполнении JavaScript: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error when executing JavaScript: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result.ToString();
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
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, "ClickAsync()") == "null")
            {
                _tester.SendMessageDebug("ClickAsync()", "ClickAsync()", Tester.FAILED, "Не удалось нажать на элемент", "Failed to click on the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug("ClickAsync()", "ClickAsync()", Tester.PASSED, "Элемент нажат", "The element is pressed", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task ClickMouseAsync()
        {
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, "ClickMouseAsync()") == "null")
            {
                _tester.SendMessageDebug("ClickMouseAsync()", "ClickMouseAsync()", Tester.FAILED, "Не удалось нажать (mouse) на элемент", "Failed to click (mouse) on the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug("ClickMouseAsync()", "ClickMouseAsync()", Tester.PASSED, "Элемент нажат (mouse)", "The element is pressed (mouse)", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetTextAsync()
        {
            if (_tester.DefineTestStop() == true) return null;

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
                result = await execute(script, "GetTextAsync()");
                if (result == "null" || result == null)
                {
                    _tester.SendMessageDebug("GetTextAsync()", "GetTextAsync()", Tester.FAILED, "Не удалось прочитать текст из элемента", "Could not read the text from the element", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (result == "") _tester.SendMessageDebug("GetTextAsync()", "GetTextAsync()", Tester.COMPLETED, "Пустой текст из элемента", "Empty text from element", Tester.IMAGE_STATUS_WARNING);
                    else if (result.Length > 1)
                    {
                        result = result.Substring(1, result.Length - 2);
                        _tester.SendMessageDebug("GetTextAsync()", "GetTextAsync()", Tester.PASSED, "Прочитан текст из элемента | " + result, "The text from the element has been read | " + result, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug("GetTextAsync()", "GetTextAsync()", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task SetTextAsync(string text)
        {
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, $"SetTextAsync(\"{text}\")") == "null")
            {
                _tester.SendMessageDebug($"SetTextAsync(\"{text}\")", $"SetTextAsync(\"{text}\")", Tester.FAILED, "Не удалось ввести текст в элемент", "Could not enter text in the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetTextAsync(\"{text}\")", $"SetTextAsync(\"{text}\")", Tester.PASSED, "Текст введен в элемент", "The text was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetValueAsync()
        {
            if (_tester.DefineTestStop() == true) return null;

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
                result = await execute(script, "GetValueAsync()");
                if (result == "null" || result == null)
                {
                    _tester.SendMessageDebug("GetValueAsync()", "GetValueAsync()", Tester.FAILED, "Не удалось прочитать значение элемента", "The value of the element could not be read", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (result.Length > 1) result = result.Substring(1, result.Length - 2);
                    _tester.SendMessageDebug("GetValueAsync()", "GetValueAsync()", Tester.PASSED, "Получено значение из элемента | " + result, "Got the value from the element | " + result, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug("GetValueAsync()", "GetValueAsync()", Tester.FAILED,
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
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, $"SetValueAsync(\"{value}\")") == "null")
            {
                _tester.SendMessageDebug($"SetValueAsync(\"{value}\")", $"SetValueAsync(\"{value}\")", Tester.FAILED, "Не удалось ввести значение в элемент", "Failed to enter a value in the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetValueAsync(\"{value}\")", $"SetValueAsync(\"{value}\")", Tester.PASSED, "Значение введено в элемент", "The value was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetAttributeAsync(string name)
        {
            if (_tester.DefineTestStop() == true) return null;

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
                result = await execute(script, $"GetAttributeAsync(\"{name}\")");
                if (result == "null" || result == null)
                {
                    _tester.SendMessageDebug($"GetAttributeAsync(\"{name}\")", $"GetAttributeAsync(\"{name}\")", Tester.FAILED, "Не удалось прочитать атрибут из элемента", "Failed to read attribute from element", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (result == "") _tester.SendMessageDebug($"GetAttributeAsync(\"{name}\")", $"GetAttributeAsync(\"{name}\")", Tester.COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", Tester.IMAGE_STATUS_WARNING);
                    else if (result.Length > 1)
                    {
                        result = result.Substring(1, result.Length - 2);
                        _tester.SendMessageDebug($"GetAttributeAsync(\"{name}\")", $"GetAttributeAsync(\"{name}\")", Tester.PASSED, $"Получено значение из аттрибута '{name}' | {result}", $"The value was obtained from the attribute '{name}' | {result}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"GetAttributeAsync(\"{name}\")", $"GetAttributeAsync(\"{name}\")", Tester.FAILED,
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
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, $"SetAttributeAsync(\"{name}\", \"{value}\")") == "null")
            {
                _tester.SendMessageDebug($"SetAttributeAsync(\"{name}\", \"{value}\")", $"SetAttributeAsync(\"{name}\", \"{value}\")", Tester.FAILED, "Не удалось ввести атрибут в элемент", "Failed to enter attribute in element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetAttributeAsync(\"{name}\", \"{value}\")", $"SetAttributeAsync(\"{name}\", \"{value}\")", Tester.PASSED, $"Аттрибут '{name}' добавлен в элемент", $"Attribute '{name}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetHtmlAsync()
        {
            if (_tester.DefineTestStop() == true) return null;

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
            string result = await execute(script, "GetHtmlAsync()");
            if (result == "null" || result == null)
            {
                _tester.SendMessageDebug("GetHtmlAsync()", "GetHtmlAsync()", Tester.FAILED, "Не удалось получить html из элемента", "Could not get html from the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                result = result.Replace("\\u003C", "<");
                result = result.Replace("\\u003E", ">");
                _tester.SendMessageDebug("GetHtmlAsync()", "GetHtmlAsync()", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }

            return result;
        }

        public async Task SetHtmlAsync(string html)
        {
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, $"SetHtmlAsync(\"{html}\")") == "null")
            {
                _tester.SendMessageDebug($"SetHtmlAsync(\"{html}\")", $"SetHtmlAsync(\"{html}\")", Tester.FAILED, "Не удалось ввести html в элемент", "Could not enter html into the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetHtmlAsync(\"{html}\")", $"SetHtmlAsync(\"{html}\")", Tester.PASSED, "В элемент введен html", "Html has been entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task ScrollToAsync(bool behaviorSmooth = false)
        {
            //int step = _tester.SendMessageDebug($"ScrollToAsync(\"{behaviorSmooth}\")", $"ScrollToAsync(\"{behaviorSmooth}\")", Tester.PROCESS, "Прокрутить к элементу", "Fasten to the element", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, $"ScrollToAsync(\"{behaviorSmooth}\")") == "null")
            {
                _tester.SendMessageDebug($"ScrollToAsync(\"{behaviorSmooth}\")", $"ScrollToAsync(\"{behaviorSmooth}\")", Tester.FAILED, "Не удалось прокрутить к элементу", "Failed to fasten to the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"ScrollToAsync(\"{behaviorSmooth}\")", $"ScrollToAsync(\"{behaviorSmooth}\")", Tester.PASSED, "Прокрутка (scroll) к элементу - выполнен", "Scrolled to the element - completed", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task WaitVisibleAsync(int sec)
        {
            if (_tester.DefineTestStop() == true) return;
            _tester.SendMessageDebug($"WaitVisibleAsync({sec})", $"WaitVisibleAsync({sec})", Tester.PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", Tester.IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible();
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) _tester.SendMessageDebug($"WaitVisibleAsync({sec})", $"WaitVisibleAsync({sec})", Tester.PASSED, $"Ожидание элемента - завершено (элемент отображается)", "Waiting for an item - completed (the item is displayed)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.SendMessageDebug($"WaitVisibleAsync({sec})", $"WaitVisibleAsync({sec})", Tester.FAILED, $"Ожидание элемента - завершено (элемент не отображается)", "Waiting for an item - completed (the item is not displayed)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"WaitVisibleAsync({sec})", $"WaitVisibleAsync({sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitNotVisibleAsync(int sec)
        {
            if (_tester.DefineTestStop() == true) return;
            _tester.SendMessageDebug($"WaitNotVisibleAsync({sec})", $"WaitNotVisibleAsync({sec})", Tester.PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", Tester.IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    found = await isVisible();
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) _tester.SendMessageDebug($"WaitNotVisibleAsync({sec})", $"WaitNotVisibleAsync({sec})", Tester.PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", "Waiting for the element to be hidden - completed (the element is not displayed)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.SendMessageDebug($"WaitNotVisibleAsync({sec})", $"WaitNotVisibleAsync({sec})", Tester.FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", "Waiting for the element to be hidden - completed (the element is displayed)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"WaitNotVisibleAsync({sec})", $"WaitNotVisibleAsync({sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task SelectOptionAsync(string by, string value)
        {
            if (_tester.DefineTestStop() == true) return;

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
            if (await execute(script, $"SelectOptionAsync(\"{by}\", \"{value}\")") == "null")
            {
                _tester.SendMessageDebug($"SelectOptionAsync(\"{by}\", \"{value}\")", $"SelectOptionAsync(\"{by}\", \"{value}\")", Tester.FAILED, "Не удалось выбрать опцию", "Failed to select an option", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SelectOptionAsync(\"{by}\", \"{value}\")", $"SelectOptionAsync(\"{by}\", \"{value}\")", Tester.PASSED, "Опция выбрана", "Option selected", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetOptionAsync(string by)
        {
            if (_tester.DefineTestStop() == true) return null;

            string script = "(function(){";
            if (_by == Tester.BY_CSS) script += $"var element = document.querySelector(\"{_locator}\");";
            else if (_by == Tester.BY_XPATH) script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (by == BY_INDEX) script += "return element.selectedIndex;";
            if (by == BY_TEXT) script += "return element.options[element.selectedIndex].text;";
            if (by == BY_VALUE) script += "return element.options[element.selectedIndex].value;";
            script += "}());";
            string result = await execute(script, "GetOntionAsync()");
            if (result == "null" || result == null)
            {
                _tester.SendMessageDebug("GetOntionAsync()", "GetOntionAsync()", Tester.FAILED, "Не удалось получить индекс или текст или значение из выбранной опции", "Could not get index or text or value from selected option", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                if (result == "") _tester.SendMessageDebug("GetOntionAsync()", "GetOntionAsync()", Tester.COMPLETED, "Пустое значение из опции", "Empty value from option", Tester.IMAGE_STATUS_WARNING);
                else _tester.SendMessageDebug("GetOntionAsync()", "GetOntionAsync()", Tester.PASSED, $"Получен индекс или текст или значение из выбранной опции | {result}", $"The index or text or value from the selected option is received | {result}", Tester.IMAGE_STATUS_PASSED);

            }
            return result;
        }
        public async Task<bool> IsClickableAsync()
        {
            if (_tester.DefineTestStop() == true) return false;

            string script = "";
            script += "(function(){ ";
            if (_by == Tester.BY_CSS) script += $"var elem = document.querySelector(\"{_locator}\");";
            if (_by == Tester.BY_XPATH) script += $"var elem = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "if((elem.getAttribute('onclick')!=null)||(elem.getAttribute('href')!=null)) return true;";
            script += "return false;";
            script += "}());";

            string result = await execute(script, "IsClickableAsync()");
            if (result == "null" || result == null)
            {
                _tester.SendMessageDebug("IsClickableAsync()", "IsClickableAsync()", Tester.FAILED, "Не удалось определить кликабельность элемента", "The clickability of the element could not be determined", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                if (result == "") _tester.SendMessageDebug("IsClickableAsync()", "IsClickableAsync()", Tester.COMPLETED, "Пустое значение", "Empty value", Tester.IMAGE_STATUS_WARNING);
                else _tester.SendMessageDebug("IsClickableAsync()", "IsClickableAsync()", Tester.PASSED, $"Определена кликадельность элемента | {result}", $"The clickability of the element is determined | {result}", Tester.IMAGE_STATUS_PASSED);

            }

            if (result != "null" && result != null && result == "true") return true;
            else return false;
        }

        public async Task<string> GetLocatorAsync()
        {
            if (_tester.DefineTestStop() == true) return "";
            _tester.SendMessageDebug($"GetLocatorAsync()", $"GetLocatorAsync()", Tester.PASSED, "Получен локатор элемента: " + _locator, "The element locator is received: " + _locator, Tester.IMAGE_STATUS_PASSED);
            return _locator;
        }

        public async Task<string> GetStyleAsync(string property)
        {
            if (_tester.DefineTestStop() == true) return null;

            string result = "";
            try
            {
                string script = null;
                if (_by == Tester.BY_CSS)
                {
                    script = "(function(){";
                    script += $"var element = document.querySelector(\"{_locator}\"); ";
                    script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                    script += "return style; ";
                    script += "}());";
                }
                else if (_by == Tester.BY_XPATH)
                {
                    script = "(function(){";
                    script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                    script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                    script += "return style; ";
                    script += "}());";
                }
                result = await execute(script, $"GetStyleAsync(\"{property}\")");
                if (result == "null" || result == null)
                {
                    _tester.SendMessageDebug($"GetStyleAsync(\"{property}\")", $"GetStyleAsync(\"{property}\")", Tester.FAILED, $"Не удалось прочитать стиль '{property}' из элемента", $"Could not read the style '{property}' from the element", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (result == "") _tester.SendMessageDebug($"GetStyleAsync(\"{property}\")", $"GetStyleAsync(\"{property}\")", Tester.COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", Tester.IMAGE_STATUS_WARNING);
                    else if (result.Length > 1)
                    {
                        result = result.Substring(1, result.Length - 2);
                        _tester.SendMessageDebug($"GetStyleAsync(\"{property}\")", $"GetStyleAsync(\"{property}\")", Tester.PASSED, "Стиль из элемента прочитан | " + result, "Style from the read element | " + result, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"GetStyleAsync(\"{property}\")", $"GetStyleAsync(\"{property}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
        }
        public async Task SetStyleAsync(string cssText)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{_locator}\");";
                script += $"element.style.cssText = '{cssText}';";
                script += "return element;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.style.cssText = '{cssText}';";
                script += "return element;";
                script += "}());";
            }
            if (await execute(script, $"SetStyleAsync(\"{cssText}\")") == "null")
            {
                _tester.SendMessageDebug($"SetStyleAsync(\"{cssText}\")", $"SetStyleAsync(\"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести стиль в элемент", $"Could not find or enter style in the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetStyleAsync(\"{cssText}\")", $"SetStyleAsync(\"{cssText}\")", Tester.PASSED, "Стиль введен в элемент", "The style is entered in the element", Tester.IMAGE_STATUS_PASSED);
            }
        }
        
    }
}
