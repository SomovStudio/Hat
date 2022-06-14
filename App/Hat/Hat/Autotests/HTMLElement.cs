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

        private async Task<string> execute(string script, int step, string commentPassed, string commentfailed)
        {
            string result = null;
            try
            {
                result = await _tester.BrowserView.CoreWebView2.ExecuteScriptAsync(script);
                if (result == null)
                {
                    _tester.EditMessage(step, null, Tester.FAILED, commentfailed, Tester.IMAGE_STATUS_FAILED);
                    _tester.TestStopAsync();
                }
                else 
                {
                    _tester.EditMessage(step, null, Tester.PASSED, commentPassed, Tester.IMAGE_STATUS_PASSED);
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
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"document.querySelector('{_locator}').click();";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.click();";
                script += "return element.outerText;";
                script += "}());";
            }
            await execute(script, step, "Элемент нажат", "Не удалось нажать на элемент");
        }

        public async Task<string> GetTextAsync()
        {
            int step = _tester.SendMessage("GetTextAsync()", Tester.PROCESS, "Чтение текста из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
                script += "return element.outerText;";
                script += "}());";
            }
            else if (_by == Tester.BY_XPATH)
            {
                script = "(function(){";
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += "return element.outerText;";
                script += "}());";
            }
            string result = await execute(script, step, "Текст из элемента прочитан", "Не удалось прочитать текст из элемента");
            return result;
        }

        public async Task SetTextAsync(string text)
        {
            int step = _tester.SendMessage($"SetTextAsync(\"{text}\")", Tester.PROCESS, "Ввод текста в элемент", Tester.IMAGE_STATUS_PROCESS);
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
                script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
                script += $"element.innerText = '{text}';";
                script += "return element.outerText;";
                script += "}());";
            }
            await execute(script, step, "Текст введен в элемент", "Не удалось ввести текст в элемент");
        }

        public async Task<string> GetValueAsync()
        {
            int step = _tester.SendMessage("GetValueAsync()", Tester.PROCESS, "Чтение значения из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
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
            string result = await execute(script, step, "Прочитано значение из элемента", "Не удалось прочитать значение элемента");
            return result;
        }

        public async Task SetValueAsync(string value)
        {
            int step = _tester.SendMessage($"SetValueAsync(\"{value}\")", Tester.PROCESS, "Ввод значения в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
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
            await execute(script, step, "Значение введено в элемент", "Не удалось ввести значение в элемент");
        }

        public async Task<string> GetAttributeAsync(string name)
        {
            int step = _tester.SendMessage($"GetAttributeAsync('{name}')", Tester.PROCESS, "Получение атрибута из элемента", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
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
            string result = await execute(script, step, "Атрибут из элемента прочитан", "Не удалось прочитать атрибут из элемента");
            return result;
        }

        public async Task SetAttributeAsync(string name, string value)
        {
            int step = _tester.SendMessage($"SetAttributeAsync('{name}', '{value}')", Tester.PROCESS, "Ввод атрибута в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
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
            await execute(script, step, "Атрибут введен в элемент", "Не удалось ввести атрибут в элемент");
        }

        public async Task<string> GetHtmlAsync()
        {
            int step = _tester.SendMessage($"GetHtmlAsync()", Tester.PROCESS, "Получение html из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
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
            string result = await execute(script, step, "Получен html из элемента", "Не удалось получить html из элемента");
            return result;
        }

        public async Task SetHtmlAsync(string html)
        {
            int step = _tester.SendMessage($"SetHtmlAsync('{html}')", Tester.PROCESS, "Ввод html в элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = null;
            if (_by == Tester.BY_CSS)
            {
                script = "(function(){";
                script += $"var element = document.querySelector('{_locator}');";
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
            await execute(script, step, "В элемент введен html", "Не удалось ввести html в элемент");
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
