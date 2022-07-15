using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatFrameworkDev
{
    public class FRAMEElement
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

        public FRAMEElement(Tester tester, string by, string locator)
        {
            _tester = tester;
            _by = by;
            _locator = locator;
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

        private async Task<bool> isVisible()
        {
            bool found = false;
            try
            {
                string script = "";
                script += "(function(){ ";
                if (_by == Tester.BY_CSS) script += $"var elem = document.querySelector(\"{_locator}\");";
                if (_by == Tester.BY_XPATH) script += $"var elem = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
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

            //string script = "(function(){";
            //if (by == Tester.BY_CSS) script += $"var element = document.querySelector(\"{locator}\");";
            //else script += $"var element = document.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            //script += $"return element.getAttribute('{attribute}');";
            //script += "}());";
            //string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}");
            //return value;

            string script = "(function(){";
            if (_by == Tester.BY_CSS) script += $"var frame = document.querySelector(\"{_locator}\");";
            else if (_by == Tester.BY_XPATH) script += $"var frame = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";

            if (by == Tester.BY_CSS) script += $"var element = frame.querySelector(\"{locator}\");";
            else if (by == Tester.BY_XPATH) script += $"var element = frame.evaluate(\"{locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";

            script += $"return element.getAttribute('{attribute}');";
            script += "}());";
            string value = await execute(script, step, $"Получено значение из аттрибута {attribute}", $"Не удалось найти или получить аттрибут из элемента по локатору: {locator}");
            return value;
        }



    }
}
