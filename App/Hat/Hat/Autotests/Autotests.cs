using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HatFramework;

namespace Hat
{
    public class Autotests
    {
        public static List<TreeNode> nodes;

        public static void play(string testFilename)
        {
            Config.browserForm.consoleMsg("Запускается тест из файла " + testFilename);
            Config.browserForm.systemConsoleMsg($"Запуск автотеста: {testFilename}", default, ConsoleColor.DarkCyan, ConsoleColor.White, true);

            try
            {
                Dictionary<string, string> providerOptions = new Dictionary<string, string>
                {
                    {"CompilerVersion", "v4.0"}
                };

                CSharpCodeProvider provider = new CSharpCodeProvider(providerOptions);

                var compilerParams = new CompilerParameters();
                foreach (string library in Config.libraries)
                {
                    compilerParams.ReferencedAssemblies.Add(library);
                }
                compilerParams.GenerateInMemory = true;
                compilerParams.GenerateExecutable = false;

                nodes = new List<TreeNode>();
                readNodes(Config.browserForm.treeViewProject.Nodes);
                string[] files = getCSharpFiles();
                CompilerResults results = provider.CompileAssemblyFromFile(compilerParams, files);

                if (results.Errors.Count != 0)
                {
                    foreach (var error in results.Errors)
                    {
                        Config.browserForm.consoleMsg(error.ToString());
                    }
                }
                else
                {
                    if (testFilename.Contains(".cs"))
                    {
                        object classObj = results.CompiledAssembly.CreateInstance("Hat." + testFilename.Remove(testFilename.Count() - 3, 3));
                        MethodInfo funcMain = classObj.GetType().GetMethod("Main");
                        funcMain.Invoke(classObj, new object[] { Config.browserForm });
                    }
                    else // это будет для папки с автотестами (пока в разработке)
                    {
                        object classObj = results.CompiledAssembly.CreateInstance("Hat.ExampleTest");
                        MethodInfo funcMain = classObj.GetType().GetMethod("Main");
                        funcMain.Invoke(classObj, new object[] { Config.browserForm });
                    }
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsg("Произошла ошибка: " + ex.Message + Environment.NewLine + Environment.NewLine + "Полное описание ошибка: " + ex.ToString());
            }
        }

        public static async Task devTestStutsAsync()
        {
            /*
            HatFrameworkDev.Tester tester = new HatFrameworkDev.Tester(Config.browserForm);
            await tester.TestBegin();
            await tester.BrowserSize(800, 600);
            await tester.GoToUrl("https://somovstudio.github.io/test.html", 5);
            await tester.FindElementById("result", 5);
            await tester.SetValueInElementById("login", "admin");
            await tester.Wait(2);
            await tester.SetValueInElementById("pass", "0000");
            await tester.Wait(2);
            await tester.ClickElementById("buttonLogin");
            await tester.Wait(2);
            string actual = await tester.GetValueFromElementById("textarea");
            string expected = "\"PASSED\"";
            await tester.FindElementById("result", 5);
            await tester.WaitVisibleElementById("result", 5);
            await tester.AssertEquals(expected, actual);
            await tester.TestEnd();
            */
            HatFrameworkDev.Tester tester = new HatFrameworkDev.Tester(Config.browserForm);
            await tester.TestBegin();
            await tester.GoToUrl("https://mgts.ru/", 5);
            await tester.WaitVisibleElementById("headerPartMGTS", 25);
            await tester.ClickElementByCSS("#header > div.header_second-row.header_desktop > div > div > div.header_action-btn > button");
            await tester.WaitVisibleElementById("popup", 5);
            await tester.SetValueInElementByCSS("#popup_name", "Тестирование Зионек");
            await tester.Wait(2);
            await tester.SetValueInElementByCSS("#popup_phone", "9999999999");
            await tester.Wait(2);
            await tester.ClickElementById("SUBMIT_ORDER");
            await tester.Wait(5);
            string order = await tester.GetValueFromElementById("last_order_sended");
            await tester.AssertNotEquals(order, "\"\"");
            await tester.TestEnd();
        }

        public static void readNodes(TreeNodeCollection _nodes)
        {
            try
            {
                foreach (TreeNode node in _nodes)
                {
                    if (node.Nodes.Count > 0)
                    {
                        readNodes(node.Nodes);
                    }
                    else
                    {
                        nodes.Add(node);
                    }
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsg("Ошибка: " + ex.ToString());
            }
        }

        public static string[] getCSharpFiles()
        {
            List<string> files = new List<string>();
            try
            {
                foreach (TreeNode node in nodes)
                {
                    if (node.Text.Contains(".cs")) files.Add(node.Name);
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsg("Ошибка: " + ex.ToString());
            }

            string[] result = new string[files.Count];
            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    result[i] = files[i];
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsg("Ошибка: " + ex.ToString());
            }

            return result;
        }

        public static string getContentFileHelper()
        {
            string content =
@"
using System;

namespace Hat
{
    public static class Helper
    {

    }
}
";
            return content;
        }

        public static string getContentFileExamplePage()
        {
            string content =
@"
using System;

namespace Hat
{
    public static class ExamplePage
    {
        public static string inputSearchName = ""//input[@name='q']"";
        public static string searchResultsClass = ""//div[@class='g']"";
    }
}
";
            return content;
        }

        public static string getContentFileExampleSteps()
        {
            string content =
@"
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Hat
{
    public class ExampleSteps
    {
        public void check()
        {
            MessageBox.Show(ExamplePage.inputSearchName);
        }
    }
}
";
            return content;
        }

        public static string getContentFileExampleTest()
        {
            string content =
@"
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HatFramework;

namespace Hat
{
    public class ExampleTest
    {
        Tester tester;

        public async void Main(Form browserWindow)
        {
            tester = new Tester(browserWindow);
            await setUp();
            await test();
            await tearDown();
        }

        public async Task setUp()
        {
            await tester.BrowserSize(800, 600);
        }

        public async Task test()
        {
            await tester.TestBegin();
            await tester.GoToUrl(""https://somovstudio.github.io/test.html"", 5);
            await tester.FindElementById(""result"", 5);
            await tester.SetValueInElementById(""login"", ""admin"");
            await tester.Wait(2);
            await tester.SetValueInElementById(""pass"", ""0000"");
            await tester.Wait(2);
            await tester.ClickElementById(""buttonLogin"");
            await tester.Wait(2);
            string actual = await tester.GetValueFromElementById(""textarea"");
            string expected = ""\""PASSED\"""";
            await tester.FindElementById(""result"", 5);
            await tester.AssertEquals(expected, actual);
            await tester.TestEnd();
        }

        public async Task tearDown()
        {
            await tester.BrowserClose();
        }
    }
}
";
            return content;
        }

        public static string getContentFileNewTest(string filename)
        {
            string content = "";
            content += "using System;" + Environment.NewLine;
            content += "using System.IO;" + Environment.NewLine;
            content += "using System.Collections.Generic;" + Environment.NewLine;
            content += "using System.ComponentModel;" + Environment.NewLine;
            content += "using System.Data;" + Environment.NewLine;
            content += "using System.Drawing;" + Environment.NewLine;
            content += "using System.Text;" + Environment.NewLine;
            content += "using System.Text.RegularExpressions;" + Environment.NewLine;
            content += "using System.Threading;" + Environment.NewLine;
            content += "using System.Threading.Tasks;" + Environment.NewLine;
            content += "using System.Windows.Forms;" + Environment.NewLine;
            content += "using HatFramework;" + Environment.NewLine;
            content += "" + Environment.NewLine;
            content += "namespace Hat" + Environment.NewLine;
            content += "{" + Environment.NewLine;
            content += "    public class " + filename + Environment.NewLine;
            content += "    {" + Environment.NewLine;
            content += "        Tester tester;" + Environment.NewLine;
            content += "        " + Environment.NewLine;
            content += "        public async void Main(Form browserWindow)" + Environment.NewLine;
            content += "        {" + Environment.NewLine;
            content += "            tester = new Tester(browserWindow);" + Environment.NewLine;
            content += "            await setUp();" + Environment.NewLine;
            content += "            await test();" + Environment.NewLine;
            content += "            await tearDown();" + Environment.NewLine;
            content += "        }" + Environment.NewLine;
            content += "" + Environment.NewLine;
            content += "        public async Task setUp()" + Environment.NewLine;
            content += "        {" + Environment.NewLine;
            content += "            " + Environment.NewLine;
            content += "        }" + Environment.NewLine;
            content += "" + Environment.NewLine;
            content += "        public async Task test()" + Environment.NewLine;
            content += "        {" + Environment.NewLine;
            content += "            " + Environment.NewLine;
            content += "        }" + Environment.NewLine;
            content += "" + Environment.NewLine;
            content += "        public async Task tearDown()" + Environment.NewLine;
            content += "        {" + Environment.NewLine;
            content += "            " + Environment.NewLine;
            content += "        }" + Environment.NewLine;
            content += "    }" + Environment.NewLine;
            content += "}" + Environment.NewLine;
            return content;
        }


    }
}
