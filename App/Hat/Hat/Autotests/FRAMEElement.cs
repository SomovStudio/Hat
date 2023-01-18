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

        private async Task<string> execute(string script, int step, string commentPassed, string commentfailed)
        {
            string result = null;
            try
            {
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS скрипт: {script}");
                result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (_tester.Debug == true) _tester.ConsoleMsg($"[DEBUG] JS результат: {result}");
                if (result == null || result == "null")
                {
                    _tester.EditMessage(step, null, Tester.FAILED, $"{commentfailed} " + Environment.NewLine + $"Результат выполнения скрипта: {result}", Tester.IMAGE_STATUS_FAILED);
                    //_tester.EditMessage(step, null, Tester.FAILED, commentfailed, Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else
                {
                    _tester.EditMessage(step, null, Tester.PASSED, $"{commentPassed} " + Environment.NewLine + $"Результат выполнения скрипта: {result}", Tester.IMAGE_STATUS_PASSED);
                    //_tester.EditMessage(step, null, Tester.PASSED, commentPassed, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + " " + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
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


        /* Основные методы работы с фреймом */
        public async Task<string> GetAttributeFromElementAsync(string by, string locator, string attribute)
        {
            int step = _tester.SendMessage($"GetAttributeFromElementAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.PROCESS, $"Получение аттрибута {attribute} из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
                else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"return element.getAttribute('{attribute}');";
                script += "}());";
                value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task<List<string>> GetAttributeFromElementsAsync(string by, string locator, string attribute)
        {
            int step = _tester.SendMessage($"GetAttributeFromElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\")", Tester.PROCESS, $"Получение аттрибутов {attribute} из элементов", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

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

            string result = await execute(script, step, $"Получение json из аттрибутов {attribute}", $"Не удалось найти или получить аттрибуты из элементов по локатору: {locator}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    _tester.EditMessage(step, null, Tester.PASSED, $"Получен json {result} из аттрибутов {attribute}", Tester.IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                    _tester.ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task SetAttributeInElementAsync(string by, string locator, string attribute, string value)
        {
            int step = _tester.SendMessage($"SetAttributeInElementAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.PROCESS, "Добавление аттрибута в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.setAttribute('{attribute}', '{value}');";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            await execute(script, step, $"Аттрибут '{attribute}' добавлен в элемент", $"Не удалось найти или ввести аттрибут в элемент по локатору: {locator}");
        }

        public async Task<List<string>> SetAttributeInElementsAsync(string by, string locator, string attribute, string value)
        {
            int step = _tester.SendMessage($"SetAttributeInElementsAsync(\"{by}\", \"{locator}\", \"{attribute}\", \"{value}\")", Tester.PROCESS, "Добавление аттрибута в элементы", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

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
            string result = await execute(script, step, $"Аттрибут '{attribute}' добавлен в элементы", $"Не удалось найти или добавить аттрибут в элементы по локатору: {locator}");
            List<string> Json_Array = null;
            if (result != "null" && result != null)
            {
                try
                {
                    result = JsonConvert.DeserializeObject(result).ToString();
                    Json_Array = JsonConvert.DeserializeObject<List<string>>(result);
                    _tester.EditMessage(step, null, Tester.PASSED, $"Аттрибут '{attribute}' со значением '{result}' - добавлен в элементы и получен json {result}", Tester.IMAGE_STATUS_PASSED);
                }
                catch (Exception ex)
                {
                    _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                    _tester.ConsoleMsgError(ex.ToString());
                }
            }
            return Json_Array;
        }

        public async Task<string> GetValueFromElementAsync(string by, string locator)
        {
            int step = _tester.SendMessage($"GetValueFromElementAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, "Получение значения из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

            string value = "";
            try
            {
                string script = "(function(){";
                script += $"var frame = window.frames[{_index}].document;";
                if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\"); return element.value;";
                else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.value;";
                script += "}());";
                value = await execute(script, step, $"Получено значение из элемента", $"Не удалось найти или получить данные из элемента по локатору: {locator}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return value;
        }

        public async Task SetValueInElementAsync(string by, string locator, string value)
        {
            int step = _tester.SendMessage($"SetValueInElementAsync(\"{by}\", \"{locator}\", \"{value}\")", Tester.PROCESS, "Ввод значения в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

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
            await execute(script, step, $"Значение введено в элемент", $"Не удалось найти или ввести значение в элемент по локатору: {locator}");
        }

        public async Task ClickElementAsync(string by, string locator)
        {
            int step = _tester.SendMessage($"ClickElementAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, "Нажатие на элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\"); element.click(); return element;";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; element.click(); return element;";
            script += "}());";
            await execute(script, step, $"Элемент нажат", $"Не удалось найти элемент по локатору: {locator}");
        }

        public async Task<bool> IsClickableElementAsync(string by, string locator)
        {
            int step = _tester.SendMessage($"IsClickableElementAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, "Определяется кликабельность элемента", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return false;

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
                _tester.EditMessage(step, null, Tester.PASSED, $"Определена кликадельность элемента: {result}", Tester.IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return clickable;
        }

        public async Task ScrollToElementAsync(string by, string locator, bool behaviorSmooth = false)
        {
            int step = _tester.SendMessage($"ScrollToElementAsync(\"{by}\", \"{locator}\", {behaviorSmooth})", Tester.PROCESS, "Прокрутить к элементу", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

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
                _tester.EditMessage(step, null, Tester.PASSED, "Прокрутил к элементу выполнена", Tester.IMAGE_STATUS_PASSED);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<int> GetCountElementsAsync(string by, string locator)
        {
            int step = _tester.SendMessage($"GetCountElementsAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, "Получение количества элементов", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return -1;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += "var element = frame.querySelectorAll(\"" + locator + "\"); return element.length;";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.ORDERED_NODE_SNAPSHOT_TYPE, null); return element.snapshotLength;";
            script += "}());";
            string result = await execute(script, step, $"Получение количества элементов", $"Не удалось найти или получить количество элементов по локатору: {locator}");
            int value = -1;
            if (result != "null" && result != null && result != "")
            {
                value = Int32.Parse(result);
                _tester.EditMessage(step, null, Tester.PASSED, $"Количество элементов {result}", Tester.IMAGE_STATUS_PASSED);
            }
            return value;
        }

        public async Task<string> GetHtmlFromElementAsync(string by, string locator)
        {
            int step = _tester.SendMessage($"GetHtmlFromElementAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, $"Получение html из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\"); return element.outerHTML;";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "return element.outerHTML;";
            script += "}());";
            string value = await execute(script, step, $"Получено html элемента", $"Не удалось найти или получить html из элемента по локатору: {locator}");
            value = value.Replace("\\u003C", "<");
            value = value.Replace("\\u003E", ">");
            return value;
        }

        public async Task SetHtmlInElementAsync(string by, string locator, string html)
        {
            int step = _tester.SendMessage($"SetHtmlInElementAsync(\"{by}\", \"{locator}\", \"{html}\")", Tester.PROCESS, "Ввод html в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerHTML = '{html}';";
            script += $"return element.outerHTML;";
            script += "}());";
            await execute(script, step, $"В элемент введен html {html}", $"Не удалось найти или ввести html в элемент по локатору: {locator}");
        }

        public async Task WaitNotVisibleElementAsync(string by, string locator, int sec)
        {
            int step = _tester.SendMessage($"WaitNotVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, $"Ожидание скрытия элемента {sec} секунд", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;
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

                if (found == false) _tester.EditMessage(step, null, Tester.PASSED, $"Ожидание скрытия элемента - завершено (элемент не отображается)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.EditMessage(step, null, Tester.FAILED, $"Ожидание скрытия элемента - завершено (элемент отображается)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task WaitVisibleElementAsync(string by, string locator, int sec)
        {
            int step = _tester.SendMessage($"WaitVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, $"Ожидание элемента {sec} секунд", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;
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

                if (found == true) _tester.EditMessage(step, null, Tester.PASSED, $"Ожидание элемента - завершено (элемент отображается)", Tester.IMAGE_STATUS_PASSED);
                else
                {
                    _tester.EditMessage(step, null, Tester.FAILED, $"Ожидание элемента - завершено (элемент не отображается)", Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
        }

        public async Task<bool> FindElementAsync(string by, string locator, int sec)
        {
            int step = _tester.SendMessage($"FindElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, "Поиск элемента", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return false;

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

                if (found == true) _tester.EditMessage(step, null, Tester.PASSED, "Поиск элемента - завершен (элемент найден)", Tester.IMAGE_STATUS_PASSED);
                else _tester.EditMessage(step, null, Tester.WARNING, "Поиск элемента - завершен (элемент не найден)", Tester.IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<bool> FindVisibleElementAsync(string by, string locator, int sec)
        {
            int step = _tester.SendMessage($"FindVisibleElementAsync(\"{by}\", \"{locator}\", {sec})", Tester.PROCESS, "Поиск элемента", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return false;

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

                if (found == true) _tester.EditMessage(step, null, Tester.PASSED, "Поиск элемента - завершен (элемент найден)", Tester.IMAGE_STATUS_PASSED);
                else _tester.EditMessage(step, null, Tester.WARNING, "Поиск элемента - завершен (элемент не найден)", Tester.IMAGE_STATUS_WARNING);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return found;
        }

        public async Task<string> GetTitleAsync()
        {
            int step = _tester.SendMessage($"GetTitleAsync()", Tester.PROCESS, "Чтение заголовка из фрейма", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

            string script = "";
            script += "(function(){ ";
            script += $"var frame = window.frames[{_index}].document;";
            script += "var element = frame.querySelector('title');";
            script += "return element.innerText;";
            script += "}());";
            string value = await execute(script, step, $"Прочитан заголовок фрейма", $"Не удалось прочитать заголовок фрейма");
            return value;
        }

        public async Task<string> GetUrlAsync()
        {
            int step = _tester.SendMessage("GetUrlAsync()", Tester.PROCESS, "Запрашивается URL фрейма", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;
            string url = null;
            try
            {
                string script = "";
                script += "(function(){ ";
                script += $"var frame = window.frames[{_index}].document;";
                script += "return frame.URL;";
                script += "}());";
                url = await execute(script, step, $"Получен URL фрейма", $"Не удалось получить URL фрейма");
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return url;
        }

        public async Task<string> GetTextFromElementAsync(string by, string locator)
        {
            int step = _tester.SendMessage($"GetTextFromElementAsync(\"{by}\", \"{locator}\")", Tester.PROCESS, "Чтение текста из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

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
                value = await execute(script, step, $"Прочитан текст из элемента", $"Не удалось найти или прочитать текст из элемента по локатору: {locator}");
                if (value.Length > 1) value = value.Substring(1, value.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }

            if (value == "") _tester.EditMessage(step, null, Tester.COMPLETED, "Не удалось получить текст из элемента", Tester.IMAGE_STATUS_WARNING);
            return value;
        }

        public async Task SetTextInElementAsync(string by, string locator, string text)
        {
            int step = _tester.SendMessage($"SetTextInElementAsync(\"{by}\", \"{locator}\", \"{text}\")", Tester.PROCESS, "Ввод текста в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"element.innerText = '{text}';";
            script += "return element.innerText;";
            script += "}());";
            await execute(script, step, $"Текст введен в элемент", $"Не удалось найти или ввести текст в элемент по локатору: {locator}");
        }

        public async Task<string> GetOptionAsync(string by, string locator, string type)
        {
            int step = _tester.SendMessage($"GetOntionAsync()", Tester.PROCESS, "Получение данных выбранной опции", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (type == BY_INDEX) script += "return element.selectedIndex;";
            if (type == BY_TEXT) script += "return element.options[element.selectedIndex].text;";
            if (type == BY_VALUE) script += "return element.options[element.selectedIndex].value;";
            script += "}());";

            string result = await execute(script, step, "Получен индекс или текст или значение из выбранной опции", "Не удалось получить индекс или текст или значение из выбранной опции");
            return result;
        }

        public async Task SelectOptionAsync(string by, string locator, string type, string value)
        {
            int step = _tester.SendMessage($"SelectOptionAsync({by}, {value})", Tester.PROCESS, $"Выбирается опция", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

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
            await execute(script, step, "Опцыя выбрана", "Не удалось выбрать опцию");
        }



    }
}
