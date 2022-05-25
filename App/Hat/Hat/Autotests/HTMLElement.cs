using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HatFrameworkDev
{
    public class HTMLElement
    {
        private Tester tester;
        public string Locator { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Value { get; set; }

        public HTMLElement(Tester _tester)
        {
            tester = _tester;
        }

        public async Task ClickAsync()
        {
            int step = tester.SendMessage("ClickAsync()", Tester.PROCESS, $"Нажатие на элемент {Locator}", Tester.IMAGE_STATUS_PROCESS);
            if (tester.CheckTestStop(step) == true) return;

            try
            {
                var result = await tester.BrowserView.CoreWebView2.ExecuteScriptAsync($"document.querySelector('{Locator}').click();");
                tester.EditMessage(step, null, Tester.PASSED, $"Нажатие на элемент {Locator}", Tester.IMAGE_STATUS_PASSED);
                if (result == null)
                {
                    tester.EditMessage(step, null, Tester.FAILED, $"Не удалось нажать на элемент {Locator}", Tester.IMAGE_STATUS_FAILED);
                    tester.TestStopAsync();
                }
            }
            catch (Exception ex)
            {
                tester.EditMessage(step, null, Tester.FAILED, "Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString(), Tester.IMAGE_STATUS_FAILED);
                tester.TestStopAsync();
                tester.ConsoleMsgError(ex.ToString());
            }
        }
    }
}
