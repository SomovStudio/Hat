using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HatFrameworkDev
{
    public class FRAMEElement
    {
        private Tester _tester;
        private int _index;

        public const string BY_INDEX = "BY_INDEX";
        public const string BY_TEXT = "BY_TEXT";
        public const string BY_VALUE = "BY_VALUE";

        public string Name { get; set; }
        public int Index { get; set; }

        public FRAMEElement(Tester tester, int index)
        {
            _tester = tester;
            _index = index;
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

        private async Task<bool> isVisible(string by, string locator)
        {
            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var elem = frame.querySelector(\"{locator}\");";
                if (by == Tester.BY_XPATH) script += $"var elem = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
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

        public async Task<bool> IsVisibleElement(string by, string locator)
        {
            bool found = false;
            try
            {
                if (by == Tester.BY_CSS || by == Tester.BY_XPATH)
                {
                    string script = "";
                    script += "(function(){ ";
                    if (by == Tester.BY_CSS) script += $"var elem = document.querySelector(\"{locator}\");";
                    if (by == Tester.BY_XPATH) script += $"var elem = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
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
                    if (result != "null" && result != null && result == "true") found = true;
                    else found = false;

                    _tester.SendMessageDebug($"IsVisibleElement(\"{by}\", \"{locator}\")", $"IsVisibleElement(\"{by}\", \"{locator}\")", Tester.COMPLETED, "Результат проверки отображения элемента: " + found.ToString(), "Result of checking the display of the element: " + found.ToString(), Tester.IMAGE_STATUS_MESSAGE);
                }
                else
                {
                    _tester.SendMessageDebug($"IsVisibleElement(\"{by}\", \"{locator}\")", $"IsVisibleElement(\"{by}\", \"{locator}\")", Tester.FAILED, "Неудалось проверить отображение элемента (некорректно указан тип локатора)", "Failed to check the display of the element (the locator type is specified incorrectly)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"IsVisibleElement(\"{by}\", \"{locator}\")", $"IsVisibleElement(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return found;
        }


        /* Основные методы работы с фреймом */
        public async Task<string> GetAttributeFromElementAsync(string by, string locator, string attribute)
        {
            if (_tester.DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
                else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"return element.getAttribute('{attribute}');";
                script += "}());";
                value = await execute(script, $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")");
                if (value == "null" || value == null)
                {
                    _tester.SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED, $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}", $"Couldn't find or get attribute from element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (value == "") _tester.SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.COMPLETED, "Пустое значение из аттрибута", "Empty value from attribute", Tester.IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        _tester.SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.PASSED, $"Получено значение из аттрибута '{attribute}' | {value}", $"The value was obtained from the attribute '{attribute}' | {value}", Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<List<string>> GetAttributeFromElementsAsync(string by, string locator, string attribute)
        {
            if (_tester.DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS)
            {
                script += $"var element = frame.querySelectorAll(\"{locator}\");";
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
            else if (by == Tester.BY_XPATH)
            {
                script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);";
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
                    _tester.SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.PASSED, $"Получен json {result} из аттрибутов {attribute}", $"Received json {result} from attributes {attribute}", Tester.IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    _tester.SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                    _tester.ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                _tester.SendMessageDebug($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", $"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.FAILED, $"Не удалось найти или получить аттрибуты из элементов по локатору: {locator}", $"Couldn't find or get attributes from elements by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }

            return Json_Array;
        }

        public async Task SetAttributeInElementAsync(string by, string locator, string attribute, string value)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            if (await execute(script, $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")") == "null")
            {
                _tester.SendMessageDebug($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.FAILED, $"Не удалось найти или ввести аттрибут в элемент по локатору: {locator}", $"Could not find or enter attribute in element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.PASSED, $"Аттрибут '{attribute}' добавлен в элемент", $"Attribute '{attribute}' added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<List<string>> SetAttributeInElementsAsync(string by, string locator, string attribute, string value)
        {
            if (_tester.DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS)
            {
                script += $"var element = frame.querySelectorAll(\"{locator}\");";
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
            else if (by == Tester.BY_XPATH)
            {
                script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null);";
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
                    _tester.SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", $"Attribute '{attribute}' with value '{result}' - added to the elements and received json {result}", Tester.IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    _tester.SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.FAILED,
                        "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                        "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                        Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                    _tester.ConsoleMsgError(ex.ToString());
                }
            }
            else
            {
                _tester.SendMessageDebug($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", $"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.FAILED, $"Не удалось найти или добавить аттрибут в элементы по локатору: {locator}", $"Couldn't find or add attribute to elements by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }

            return Json_Array;
        }

        public async Task<string> GetValueFromElementAsync(string by, string locator)
        {
            if (_tester.DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\"); return element.value;";
                else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.value;";
                script += "}());";
                value = await execute(script, $"GetValueFromElementAsync(\"{by}\", \"{locator}\")");
                if (value == "null" || value == null)
                {
                    _tester.SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или получить данные из элемента по локатору: {locator}", $"Could not find or get data from the element by Locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (value.Length > 1) value = value.Substring(1, value.Length - 2);
                    _tester.SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Получено значение из элемента | " + value, "Got the value from the element | " + value, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", $"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task SetValueInElementAsync(string by, string locator, string value)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
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
                _tester.SendMessageDebug($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", $"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", Tester.FAILED, $"Не удалось найти или ввести значение в элемент по локатору: {locator}", $"Could not find or enter a value in the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", $"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", Tester.PASSED, "Значение введено в элемент", "The value was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task ClickElementAsync(string by, string locator)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\"); element.click(); return element;";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; element.click(); return element;";
            script += "}());";
            if (await execute(script, $"ClickElementAsync(\"{by}\", \"{locator}\")") == "null")
            {
                _tester.SendMessageDebug($"ClickElementAsync(\"{by}\", \"{locator}\")", $"ClickElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти элемент по локатору: {locator}", $"The element could not be found by the locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"ClickElementAsync(\"{by}\", \"{locator}\")", $"ClickElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Элемент нажат", "The element is pressed", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<bool> IsClickableElementAsync(string by, string locator)
        {
            //int step = _tester.SendMessageDebug($"IsClickableElementAsync(\"{by}\", \"{locator}\")", $"IsClickableElementAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, "Определяется кликабельность элемента", "The clickability of the element is determined", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop() == true) return false;

            bool clickable = false;
            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
                else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "if((element.getAttribute('onclick')!=null)||(element.getAttribute('href')!=null)) return true;";
                script += "return false;";
                script += "}());";
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                string result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");

                if (result != "null" && result != null && result == "true") clickable = true;
                else clickable = false;
                _tester.SendMessageDebug($"IsClickableElementAsync(\"{by}\", \"{locator}\")", $"IsClickableElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, $"Определена кликадельность элемента: {result}", $"The clickability of the element is determined: {result}", Tester.IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"IsClickableElementAsync(\"{by}\", \"{locator}\")", $"IsClickableElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return clickable;
        }

        public async Task ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)
        {
            if (_tester.DefineTestStop() == true) return;

            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS)
                {
                    script += $"var element = frame.querySelector(\"{locator}\");";
                    if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'}); return element;";
                    else script += "element.scrollIntoView(); return element;";
                }
                else if (by == Tester.BY_XPATH)
                {
                    script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                    if (behaviorSmooth == true) script += "element.scrollIntoView({behavior: 'smooth'}); return element;";
                    else script += "element.scrollIntoView(); return element;";
                }
                script += "}());";
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                string result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");
                _tester.SendMessageDebug($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", Tester.PASSED, "Прокрутил к элементу - выполнена", "Scrolled to the element - completed", Tester.IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", $"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<int> GetCountElementsAsync(string by, string locator)
        {
            if (_tester.DefineTestStop() == true) return -1;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += "var element = frame.querySelectorAll(\"" + locator + "\"); return element.length;";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null); return element.snapshotLength;";
            script += "}());";
            string result = await execute(script, $"GetCountElementsAsync(\"{by}\", \"{locator}\")");
            if (result != "null" && result != null && result != "")
            {
                _tester.SendMessageDebug($"GetCountElementsAsync(\"{by}\", \"{locator}\")", $"GetCountElementsAsync(\"{by}\", \"{locator}\")", Tester.PASSED, $"Количество элементов {Int32.Parse(result)}", $"Amount of elements {Int32.Parse(result)}", Tester.IMAGE_STATUS_PASSED);
                return Int32.Parse(result);
            }
            else
            {
                _tester.SendMessageDebug($"GetCountElementsAsync(\"{by}\", \"{locator}\")", $"GetCountElementsAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или получить количество элементов по локатору: {locator}", $"Couldn't find or get the amount of elements by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            return -1;
        }

        public async Task<string> GetHtmlFromElementAsync(string by, string locator)
        {
            if (_tester.DefineTestStop() == true) return "";

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\"); return element.outerHTML;";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "return element.outerHTML;";
            script += "}());";
            string value = await execute(script, $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")");
            if (value == "null" || value == null)
            {
                _tester.SendMessageDebug($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или получить html из элемента по локатору: {locator}", $"Couldn't find or get html from the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                value = value.Replace("\\u003C", "<");
                value = value.Replace("\\u003E", ">");
                _tester.SendMessageDebug($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", $"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Получен html элемента", "The html of the element was received", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task SetHtmlInElementAsync(string by, string locator, string html)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            if (await execute(script, $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")") == "null")
            {
                _tester.SendMessageDebug($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", Tester.FAILED, $"Не удалось найти или ввести html в элемент по локатору: {locator}", $"Could not find or enter html into the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", $"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", Tester.PASSED, $"В элемент введен html {html}", $"Html {html} has been added to the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task WaitNotVisibleElementAsync(string by, string locator, int sec)
        {
            if (_tester.DefineTestStop() == true) return;
            _tester.SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", Tester.IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = true;
                for (int i = 0; i < sec; i++)
                {
                    if (by == Tester.BY_CSS) found = await isVisible(Tester.BY_CSS, locator);
                    else if (by == Tester.BY_XPATH) found = await isVisible(Tester.BY_XPATH, locator);
                    if (found == false) break;
                    await Task.Delay(1000);
                }

                if (found == false) _tester.SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", $"Waiting for the element to be hidden - completed (the element is not displayed)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", $"Waiting for the element to be hidden - completed (the element is displayed)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementAsync(string by, string locator, int sec)
        {
            if (_tester.DefineTestStop() == true) return;
            _tester.SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, $"Ожидание {sec.ToString()} секунд", $"Waiting {sec.ToString()} seconds", Tester.IMAGE_STATUS_MESSAGE);

            try
            {
                bool found = false;
                for (int i = 0; i < sec; i++)
                {
                    if (by == Tester.BY_CSS) found = await isVisible(Tester.BY_CSS, locator);
                    else if (by == Tester.BY_XPATH) found = await isVisible(Tester.BY_XPATH, locator);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) _tester.SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PASSED, $"Ожидание элемента - завершено (элемент отображается)", $"Waiting for an element - completed (the element is displayed)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED, $"Ожидание элемента - завершено (элемент не отображается)", $"Waiting for an element - completed (the element is not displayed)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<bool> FindElementAsync(string by, string locator, int sec)
        {
            if (_tester.DefineTestStop() == true) return false;
            _tester.SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", Tester.IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var elem = frame.querySelector(\"{locator}\");";
                else if (by == Tester.BY_XPATH) script += $"var elem = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "return elem.innerHTML;";
                script += "}());";

                string result = null;
                for (int i = 0; i < sec; i++)
                {
                    if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                    result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                    if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");
                    if (result != "null" && result != null)
                    {
                        found = true;
                        break;
                    }
                    await Task.Delay(1000);
                }

                if (found == true) _tester.SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.COMPLETED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", Tester.IMAGE_STATUS_MESSAGE);
                else _tester.SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.COMPLETED, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", Tester.IMAGE_STATUS_MESSAGE);
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementAsync(string by, string locator, int sec)
        {
            if (_tester.DefineTestStop() == true) return false;
            _tester.SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, $"Ожидание {sec.ToString()} секунд (поиск)", $"Waiting {sec.ToString()} seconds (search)", Tester.IMAGE_STATUS_MESSAGE);

            bool found = false;
            try
            {
                for (int i = 0; i < sec; i++)
                {
                    if (by == Tester.BY_CSS) found = await isVisible(Tester.BY_CSS, locator);
                    else if (by == Tester.BY_XPATH) found = await isVisible(Tester.BY_XPATH, locator);
                    if (found) break;
                    await Task.Delay(1000);
                }

                if (found == true) _tester.SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PASSED, "Поиск элемента - завершен (элемент найден)", "Element search - completed (element found)", Tester.IMAGE_STATUS_PASSED);
                else _tester.SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.WARNING, "Поиск элемента - завершен (элемент не найден)", "Element search - completed (element not found)", Tester.IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", $"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<string> GetTitleAsync()
        {
            if (_tester.DefineTestStop() == true) return "";

            string script = "";
            script += "(function(){ ";
            script += $"var frame = window.frames[{_index}].document;";
            script += "var element = frame.querySelector('title');";
            script += "return element.innerText;";
            script += "}());";
            string value = await execute(script, $"GetTitleAsync()");
            if (value == "null" || value == null)
            {
                _tester.SendMessageDebug($"GetTitleAsync()", $"GetTitleAsync()", Tester.FAILED, "Не удалось найти заголовок на странице", "Couldn't find the title on the page", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"GetTitleAsync()", $"GetTitleAsync()", Tester.PASSED, "Прочитан текст из заголовка | " + value, "The text from the title has been read | " + value, Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetUrlAsync()
        {
            if (_tester.DefineTestStop() == true) return null;
            string url = null;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var frame = window.frames[{_index}].document;";
                script += "return frame.URL;";
                script += "}());";
                url = await execute(script, "GetUrlAsync()");
                if (url == "null" || url == null)
                {
                    _tester.SendMessageDebug("GetUrlAsync()", "GetUrlAsync()", Tester.FAILED, "Не удалось получить URL фрейма", "Failed to get frame URL", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    _tester.SendMessageDebug("GetUrlAsync()", "GetUrlAsync()", Tester.PASSED, "Получен URL фрейма | " + url, "Frame URL received | " + url, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug("GetUrlAsync()", "GetUrlAsync()", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return url;
        }

        public async Task<string> GetTextFromElementAsync(string by, string locator)
        {
            if (_tester.DefineTestStop() == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += "var element = frame.querySelector(\"" + locator + "\"); ";
                else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                script += "if(element.innerText == '' && element.value != null) { return element.value; } ";
                script += "else { return element.innerText; } ";
                script += "}());";
                value = await execute(script, $"GetTextFromElementAsync(\"{by}\", \"{locator}\")");
                if (value == "null" || value == null)
                {
                    _tester.SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED, $"Не удалось найти или прочитать текст из элемента по локатору: {locator}", $"Could not find or read the text from the element by the locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (value == "") _tester.SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.COMPLETED, "Пустой текст из элемента", "Empty text from element", Tester.IMAGE_STATUS_WARNING);
                    else if (value.Length > 1)
                    {
                        value = value.Substring(1, value.Length - 2);
                        _tester.SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.PASSED, "Прочитан текст из элемента | " + value, "The text from the element has been read | " + value, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", $"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return value;
        }

        public async Task SetTextInElementAsync(string by, string locator, string text)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            if (await execute(script, $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")") == "null")
            {
                _tester.SendMessageDebug($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", Tester.FAILED, $"Не удалось найти или ввести текст в элемент по локатору: {locator}", $"Could not find or enter text in the element by locator: {locator}", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", $"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", Tester.PASSED, "Текст введен в элемент", "The text was entered into the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetOptionAsync(string by, string locator, string type)
        {
            if (_tester.DefineTestStop() == true) return null;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (type == BY_INDEX) script += "return element.selectedIndex;";
            if (type == BY_TEXT) script += "return element.options[element.selectedIndex].text;";
            if (type == BY_VALUE) script += "return element.options[element.selectedIndex].value;";
            script += "}());";

            string result = await execute(script, $"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")");
            if (result == "null" || result == null)
            {
                _tester.SendMessageDebug($"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")", $"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")", Tester.FAILED, "Не удалось получить индекс или текст или значение из выбранной опции", "Could not get index or text or value from selected option", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                if (result == "") _tester.SendMessageDebug($"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")", $"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")", Tester.COMPLETED, "Пустое значение из опции", "Empty value from option", Tester.IMAGE_STATUS_WARNING);
                else _tester.SendMessageDebug($"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")", $"GetOntionAsync(\"{by}\", \"{locator}\", \"{type}\")", Tester.PASSED, $"Получен индекс или текст или значение из выбранной опции | {result}", $"The index or text or value from the selected option is received | {result}", Tester.IMAGE_STATUS_PASSED);

            }

            return result;
        }

        public async Task SelectOptionAsync(string by, string locator, string type, string value)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (type == BY_INDEX)
            {
                script += $"element.options[{value}].selected = true;";
                script += "element.dispatchEvent(new KeyboardEvent('keydown', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keypress', { bubbles: true }));";
                script += "element.dispatchEvent(new KeyboardEvent('keyup', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('input', { bubbles: true }));";
                script += "element.dispatchEvent(new Event('change', { bubbles: true }));";
            }
            if (type == BY_TEXT)
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
            if (type == BY_VALUE)
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
            if (await execute(script, $"SelectOptionAsync({by}, {locator}, {type}, {value})") == "null")
            {
                _tester.SendMessageDebug($"SelectOptionAsync({by}, {locator}, {type}, {value})", $"SelectOptionAsync({by}, {locator}, {type}, {value})", Tester.FAILED, "Не удалось выбрать опцию", "Failed to select an option", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SelectOptionAsync({by}, {locator}, {type}, {value})", $"SelectOptionAsync({by}, {locator}, {type}, {value})", Tester.PASSED, "Опция выбрана", "Option selected", Tester.IMAGE_STATUS_PASSED);
            }
        }

        public async Task<string> GetStyleFromElementAsync(string by, string locator, string property)
        {
            if (_tester.DefineTestStop() == true) return null;

            string result = "";
            try
            {
                string script = null;
                if (by == Tester.BY_CSS)
                {
                    script = "(function(){";
                    script += $"var element = document.querySelector(\"{locator}\"); ";
                    script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                    script += "return style; ";
                    script += "}());";
                }
                else if (by == Tester.BY_XPATH)
                {
                    script = "(function(){";
                    script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; ";
                    script += $"var style = window.getComputedStyle(element).getPropertyValue(\"{property}\"); ";
                    script += "return style; ";
                    script += "}());";
                }
                result = await execute(script, $"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")");
                if (result == "null" || result == null)
                {
                    _tester.SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", Tester.FAILED, $"Не удалось прочитать стиль '{property}' из элемента", $"Could not read the style '{property}' from the element", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    if (result == "") _tester.SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", Tester.COMPLETED, "Пустое значение стиля из элемента", "Empty style value from the element", Tester.IMAGE_STATUS_WARNING);
                    else if (result.Length > 1)
                    {
                        result = result.Substring(1, result.Length - 2);
                        _tester.SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", Tester.PASSED, "Стиль из элемента прочитан | " + result, "Style from the read element | " + result, Tester.IMAGE_STATUS_PASSED);
                    }
                }
            }
            catch (Exception ex)
            {
                _tester.SendMessageDebug($"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", $"GetStyleFromElementAsync(\"{by}\", \"{locator}\", \"{property}\")", Tester.FAILED,
                    "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(),
                    "Error: " + ex.Message + Environment.NewLine + Environment.NewLine + "Full description of the error: " + ex.ToString(),
                    Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            return result;
        }
        public async Task SetStyleInElementAsync(string by, string locator, string cssText)
        {
            if (_tester.DefineTestStop() == true) return;

            string script = null;
            if (by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector(\"{locator}\");";
                script += $"element.style.cssText = '{cssText}';";
                script += "return element;";
                script += "}());";
            }
            else if (by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.style.cssText = '{cssText}';";
                script += "return element;";
                script += "}());";
            }
            if (await execute(script, $"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")") == "null")
            {
                _tester.SendMessageDebug($"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", $"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", Tester.FAILED, $"Не удалось найти или ввести стиль в элемент", $"Could not find or enter style in the element", Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
            }
            else
            {
                _tester.SendMessageDebug($"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", $"SetStyleInElementAsync(\"{by}\", \"{locator}\", \"{cssText}\")", Tester.PASSED, "Стиль введен в элемент", "The style is entered in the element", Tester.IMAGE_STATUS_PASSED);
            }
        }

    }
}
