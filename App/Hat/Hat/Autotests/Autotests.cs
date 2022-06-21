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
            Config.browserForm.consoleMsg($"Запущен файл автотеста: {testFilename}");
            Config.browserForm.systemConsoleMsg($"Запущен файл автотеста: {testFilename}", default, ConsoleColor.DarkCyan, ConsoleColor.White, true);

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
                        //object classObj = results.CompiledAssembly.CreateInstance("Hat.ExampleTest");
                        //MethodInfo funcMain = classObj.GetType().GetMethod("Main");
                        //funcMain.Invoke(classObj, new object[] { Config.browserForm });
                    }
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        public static async Task devTestStutsAsync()
        {
            HatFrameworkDev.Tester tester = new HatFrameworkDev.Tester(Config.browserForm);
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test.html", 5);

            //string result = await tester.RestGetAsync(@"https://mgts.ru/ajax/api/files.php?key=s82Kuxr2");
            string result = await tester.RestGetAsync(@"https://jsonplaceholder.typicode.com/posts/1/", "UTF-8");
            tester.ConsoleMsg(result);
            
            await tester.TestEndAsync();

            /*
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test.html", 5);
            await tester.WaitNotVisibleElementAsync(HatFrameworkDev.Tester.BY_CSS, "div[id='result']", 2);
            await tester.FindElementAsync(HatFrameworkDev.Tester.BY_CSS, "div[id='result']", 2);
            await tester.FindVisibleElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth #buttonLogin", 2);
            await tester.SetValueInElementAsync(HatFrameworkDev.Tester.BY_CSS, "input[id='login']", "admin");
            await tester.SetValueInElementAsync(HatFrameworkDev.Tester.BY_CSS, "input[id='pass']", "0000");
            string value_css = await tester.GetValueFromElementAsync(HatFrameworkDev.Tester.BY_CSS, "input[id='login']");
            tester.ConsoleMsg(value_css);
            await tester.SetTextInElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth > h2", "Тестовый заголовок");
            string text_css = await tester.GetTextFromElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth > h2");
            tester.ConsoleMsg(text_css);
            int count_css = await tester.GetCountElementsAsync(HatFrameworkDev.Tester.BY_CSS, "input");
            tester.ConsoleMsg(count_css.ToString());
            string attr_css = await tester.GetAttributeFromElementAsync(HatFrameworkDev.Tester.BY_CSS, "input", "name");
            tester.ConsoleMsg(attr_css);
            List<string> attrs_css = await tester.GetAttributeFromElementsAsync(HatFrameworkDev.Tester.BY_CSS, "input", "name");
            if(attrs_css != null) 
            {
                foreach (string attr in attrs_css)
                    tester.ConsoleMsg(attr);
            }
            await tester.SetAttributeInElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth > h2", "name", "textH1");
            await tester.SetAttributeInElementsAsync(HatFrameworkDev.Tester.BY_CSS, "input", "class", "test-class");
            string html_css = await tester.GetHtmlFromElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth > h2");
            tester.ConsoleMsg(html_css);
            await tester.SetHtmlInElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth > h2", "<div>Тестовый блок</div>");
            await tester.ClickElementAsync(HatFrameworkDev.Tester.BY_CSS, "#auth #buttonLogin");
            await tester.WaitVisibleElementAsync(HatFrameworkDev.Tester.BY_CSS, "div[id='result']", 2);
            await tester.TestEndAsync();
            */

            /*
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test.html", 5);
            await tester.WaitNotVisibleElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='result']", 2);
            await tester.FindElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='result']", 2);
            await tester.FindVisibleElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//input[@id='buttonLogin']", 2);
            await tester.SetValueInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='login']", "admin");
            await tester.SetValueInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='pass']", "0000");
            string value_xpath = await tester.GetValueFromElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='login']");
            tester.ConsoleMsg(value_xpath);
            await tester.SetTextInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//h2", "Тестовый заголовок");
            string text_xpath = await tester.GetTextFromElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//h2");
            tester.ConsoleMsg(text_xpath);
            int count_xpath = await tester.GetCountElementsAsync(HatFrameworkDev.Tester.BY_XPATH, "//input");
            tester.ConsoleMsg(count_xpath.ToString());
            string attr_xpath = await tester.GetAttributeFromElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input", "name");
            tester.ConsoleMsg(attr_xpath);
            List<string> attrs_xpath = await tester.GetAttributeFromElementsAsync(HatFrameworkDev.Tester.BY_XPATH, "//input", "name");
            if(attrs_xpath != null)
            {
                foreach (string attr in attrs_xpath)
                    tester.ConsoleMsg(attr);
            }
            await tester.SetAttributeInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//h2", "name", "textH1");
            await tester.SetAttributeInElementsAsync(HatFrameworkDev.Tester.BY_XPATH, "//input", "class", "test-class");
            string html_xpath = await tester.GetHtmlFromElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//h2");
            tester.ConsoleMsg(html_xpath);
            await tester.SetHtmlInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//h2", "<div>Тестовый блок</div>");
            await tester.ClickElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='auth']//input[@id='buttonLogin']");
            await tester.WaitVisibleElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//div[@id='result']", 2);
            await tester.TestEndAsync();
            */
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
                Config.browserForm.consoleMsgError(ex.ToString());
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
                Config.browserForm.consoleMsgError(ex.ToString());
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
                Config.browserForm.consoleMsgError(ex.ToString());
            }

            return result;
        }

        public static string getContentFileHelper()
        {
            string content =
@"using System;
using HatFramework;

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
@"using System;
using HatFramework;

namespace Hat
{
    public static class ExamplePage
    {
        public static string URL = @""https://somovstudio.github.io/test.html"";        
        public static string InputLogin = ""login"";
        public static string InputPass = ""pass"";
        public static string ButtonLogin = ""buttonLogin"";
        public static string Result = ""result"";
        public static string Textarea = ""textarea"";
    }
}
";
            return content;
        }

        public static string getContentFileExampleSteps()
        {
            string content =
@"using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using HatFramework;

namespace Hat
{
    public class ExampleSteps : Tester
    {
        public ExampleSteps(Form browserWindow): base(browserWindow) {}

        public async Task FillForm()
        {
            await this.WaitVisibleElementByIdAsync(ExamplePage.InputLogin, 15);
            await this.SetValueInElementByIdAsync(ExamplePage.InputLogin, ""admin"");
            await this.WaitAsync(2);
            await this.SetValueInElementByIdAsync(ExamplePage.InputPass, ""0000"");
            await this.WaitAsync(2);
        }
    }
}
";
            return content;
        }

        public static string getContentFileExampleTest1()
        {
            string content =
@"using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using HatFramework;

namespace Hat
{
    public class ExampleTest1
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
            await tester.BrowserFullScreenAsync();
        }

        public async Task test()
        {
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync(""https://somovstudio.github.io/test.html"", 5);
            await tester.WaitVisibleElementByIdAsync(""login"", 15);
            await tester.SetValueInElementByIdAsync(""login"", ""admin"");
            await tester.WaitAsync(2);
            await tester.SetValueInElementByIdAsync(""pass"", ""0000"");
            await tester.WaitAsync(2);
            await tester.ClickElementByIdAsync(""buttonLogin"");
            await tester.WaitVisibleElementByIdAsync(""result"", 5);
            string actual = await tester.GetValueFromElementByIdAsync(""textarea"");
            string expected = ""\""Вы успешно авторизованы\"""";
            await tester.AssertEqualsAsync(expected, actual);
            await tester.TestEndAsync();
        }

        public async Task tearDown()
        {
            // await tester.BrowserCloseAsync();
        }
    }
}
";
            return content;
        }

        public static string getContentFileExampleTest2()
        {
            string content =
@"using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Newtonsoft.Json;
using HatFramework;

namespace Hat
{
    public class ExampleTest2
    {
        ExampleSteps tester;

        public async void Main(Form browserWindow)
        {
            tester = new ExampleSteps(browserWindow);

            await setUp();
            await test();
            await tearDown();
        }

        public async Task setUp()
        {
            await tester.BrowserFullScreenAsync();
        }

        public async Task test()
        {
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync(ExamplePage.URL, 5);
            await tester.FillForm();
            await tester.ClickElementByIdAsync(ExamplePage.ButtonLogin);
            await tester.WaitVisibleElementByIdAsync(ExamplePage.Result, 5);
            string actual = await tester.GetValueFromElementByIdAsync(ExamplePage.Textarea);
            string expected = ""\""Вы успешно авторизованы\"""";
            await tester.AssertEqualsAsync(expected, actual);
            await tester.TestEndAsync();
        }

        public async Task tearDown()
        {
            // await tester.BrowserCloseAsync();
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
            content += "using System.Collections.Generic;" + Environment.NewLine;
            content += "using System.ComponentModel;" + Environment.NewLine;
            content += "using System.Windows.Forms;" + Environment.NewLine;
            content += "using System.Threading;" + Environment.NewLine;
            content += "using System.Threading.Tasks;" + Environment.NewLine;
            content += "using System.IO" + Environment.NewLine;
            content += "using System.Data;" + Environment.NewLine;
            content += "using System.Drawing;" + Environment.NewLine;
            content += "using System.Linq;" + Environment.NewLine;
            content += "using System.Text;" + Environment.NewLine;
            content += "using System.Text.RegularExpressions;" + Environment.NewLine;
            content += "using System.Net;" + Environment.NewLine;
            content += "using System.Net.Http;" + Environment.NewLine;
            content += "using System.Net.Http.Headers;" + Environment.NewLine;
            content += "using System.Reflection;" + Environment.NewLine;
            content += "using Newtonsoft.Json;" + Environment.NewLine;
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
