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
                if (by == Tester.BY_XPATH) script += $"var elem = frame.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
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

            string script = "(function(){";
            script += $"var frame = window.frames[{_index}].document;";
            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", frame, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}");
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


    }
}
