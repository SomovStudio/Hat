using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatFramework
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
                //script += "if (!(elem instanceof Element)) throw Error('DomUtil: elem is not an element.');";
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

        public async Task ClickAsync()
        {
            int step = _tester.SendMessage("ClickAsync()", Tester.PROCESS, "Нажатие на элемент", Tester.IMAGE_STATUS_PROCESS);
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
            await execute(script, step, "Элемент нажат", "Не удалось нажать на элемент");
        }

        public async Task<string> GetTextAsync()
        {
            int step = _tester.SendMessage("GetTextAsync()", Tester.PROCESS, "Чтение текста из элемент", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string result = "";
            try
            {
                string script = null;
                if (_by == Tester.BY_CSS)
                {
                    script = "(function(){";
                    script += $"var element = document.querySelector(\"{_locator}\");";
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
                result = await execute(script, step, "Текст из элемента прочитан", "Не удалось прочитать текст из элемента");
                if (result.Length > 1) result = result.Substring(1, result.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
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
            await execute(script, step, "Текст введен в элемент", "Не удалось ввести текст в элемент");
        }

        public async Task<string> GetValueAsync()
        {
            int step = _tester.SendMessage("GetValueAsync()", Tester.PROCESS, "Чтение значения из элемент", Tester.IMAGE_STATUS_PROCESS);
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
                result = await execute(script, step, "Прочитано значение из элемента", "Не удалось прочитать значение элемента");
                if (result.Length > 1) result = result.Substring(1, result.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
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
            await execute(script, step, "Значение введено в элемент", "Не удалось ввести значение в элемент");
        }

        public async Task<string> GetAttributeAsync(string name)
        {
            int step = _tester.SendMessage($"GetAttributeAsync('{name}')", Tester.PROCESS, "Получение атрибута из элемента", Tester.IMAGE_STATUS_PROCESS);
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
                result = await execute(script, step, "Атрибут из элемента прочитан", "Не удалось прочитать атрибут из элемента");
                if (result.Length > 1) result = result.Substring(1, result.Length - 2);
            }
            catch (Exception ex)
            {
                _tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                _tester.TestStopAsync();
                _tester.ConsoleMsgError(ex.ToString());
            }
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
            string result = await execute(script, step, "Получен html из элемента", "Не удалось получить html из элемента");
            result = result.Replace("\\u003C", "<");
            result = result.Replace("\\u003E", ">");
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
            await execute(script, step, "В элемент введен html", "Не удалось ввести html в элемент");
        }

        public async Task ScrollToAsync(bool behaviorSmooth = false)
        {
            int step = _tester.SendMessage($"ScrollToAsync('{behaviorSmooth}')", Tester.PROCESS, "Прокрутить к элементу", Tester.IMAGE_STATUS_PROCESS);
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
            await execute(script, step, "Прокрутка к элементу завершена", "Не удалось прокрутить к элементу");
        }

        public async Task WaitVisibleAsync(int sec)
        {
            int step = _tester.SendMessage($"WaitVisibleAsync({sec})", Tester.PROCESS, $"Ожидание элемента {sec} секунд", Tester.IMAGE_STATUS_PROCESS);
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

        public async Task WaitNotVisibleAsync(int sec)
        {
            int step = _tester.SendMessage($"WaitNotVisibleAsync({sec})", Tester.PROCESS, $"Ожидание скрытия элемента {sec} секунд", Tester.IMAGE_STATUS_PROCESS);
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

        public async Task SelectOptionAsync(string by, string value)
        {
            int step = _tester.SendMessage($"SelectOptionAsync({by}, {value})", Tester.PROCESS, $"Выбирается опция", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return;

            string script = "(function(){";
            if (_by == Tester.BY_CSS) script += $"var element = document.querySelector(\"{_locator}\");";
            else if (_by == Tester.BY_XPATH) script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (by == BY_INDEX) script += $"element.options[{value}].selected = true;";
            if (by == BY_TEXT)
            {
                script += "for (var i = 0; i < element.options.length; ++i) {";
                script += $"if (element.options[i].text === '{value}')";
                script += "{";
                script += "element.options[i].selected = true;";
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
                script += "break;";
                script += "}";
                script += "}";
            }
            script += "return element;";
            script += "}());";
            await execute(script, step, $"Опцыя выбрана", $"Не удалось выбрать опцию");
        }

        public async Task<string> GetOptionAsync(string by)
        {
            int step = _tester.SendMessage($"GetOntionAsync()", Tester.PROCESS, "Получение данных выбранной опции", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return null;

            string script = "(function(){";
            if (_by == Tester.BY_CSS) script += $"var element = document.querySelector(\"{_locator}\");";
            else if (_by == Tester.BY_XPATH) script += $"var element = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            if (by == BY_INDEX) script += "return element.selectedIndex;";
            if (by == BY_TEXT) script += "return element.options[element.selectedIndex].text;";
            if (by == BY_VALUE) script += "return element.options[element.selectedIndex].value;";
            script += "}());";

            string result = await execute(script, step, "Получен индекс или текст или значение из выбранной опции", "Не удалось получить индекс или текст или значение из выбранной опции");
            return result;
        }

        public async Task<bool> IsClickableAsync()
        {
            int step = _tester.SendMessage($"IsClickableAsync()", Tester.PROCESS, "Определяется кликабельность элемента", Tester.IMAGE_STATUS_PROCESS);
            if (_tester.DefineTestStop(step) == true) return false;

            bool clickable = false;
            string script = "";
            script += "(function(){ ";
            if (_by == Tester.BY_CSS) script += $"var elem = document.querySelector(\"{_locator}\");";
            if (_by == Tester.BY_XPATH) script += $"var elem = document.evaluate(\"{_locator}\", document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue;";
            script += "if((elem.getAttribute('onclick')!=null)||(elem.getAttribute('href')!=null)) return true;";
            script += "return false;";
            script += "}());";

            string result = await execute(script, step, "Определена кликадельность элемента", "Не удалось определить кликабельность элемента");
            if (result != "null" && result != null && result == "true") clickable = true;
            else clickable = false;
            return clickable;
        }


    }
}
