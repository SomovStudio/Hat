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

        private async Task<string> execute(string script, int step, string comment)
        {
            string result = null;
            try
            {
                result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (result == null)
                {
                    _tester.EditMessage(step, null, Tester.FAILED, comment, Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else 
                {
                    _tester.EditMessage(step, null, Tester.PASSED, comment, Tester.IMAGE_STATUS_PASSED);
                }
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
            return result;
        }

        public async Task ClickAsync()
        {
            int step = _tester.SendMessage("ClickAsync()", Tester.PROCESS, "Нажатие на элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS) script = $"document.querySelector('{_locator}').click();";
            else if (_by == Tester.BY_XPATH) script = $"document.evaluate('{_locator}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click();";
            await execute(script, step, $"Нажатие на элемент {_locator}");
        }

        public async Task<string> GetTextAsync()
        {
            int step = _tester.SendMessage("GetTextAsync()", Tester.PROCESS, "Чтение текста из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return "";

            string script = null;
            if (_by == Tester.BY_CSS) script = "(function(){ var element = document.querySelector('" + _locator + "'); return element.outerText; }());";
            else if (_by == Tester.BY_XPATH) script = "(function(){ var element = document.evaluate('" + _locator + "', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue; return element.outerText; }());";
            string result = await execute(script, step, $"Прочитан текст из элемента");
            return result;
        }

        public async Task SetTextAsync(string text)
        {
            int step = _tester.SendMessage($"SetTextAsync('{text}')", Tester.PROCESS, "Ввод текста в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
                script += $"element.innerText = '{text}';";
                script += "return element.outerText;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate('{_locator}', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.innerText = '{text}';";
                script += "return element.outerText;";
                script += "}());";
            }
            await execute(script, step, "Текст введен в элемент");
        }

        public async Task<string> GetValueAsync()
        {
            return "";
        }

        public async Task SetValueAsync(string value)
        {

        }

        public async Task<string> GetAttributeAsync(string name)
        {
            return "";
        }

        public async Task SetAttributeAsync(string name, string value)
        {

        }

        public async Task<string> GetHtmlAsync()
        {
            return "";
        }

        public async Task SetHtmlAsync(string html)
        {

        }

        public async Task ScrollToAsync(bool behaviorSmooth = false)
        {

        }

        public async Task WaitVisibleAsync(int sec)
        {

        }

        public async Task WaitNotVisibleAsync(int sec)
        {

        }
    }
}
