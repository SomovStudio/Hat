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
using System.IO;

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
                        Config.browserForm.consoleMsgErrorReport(error.ToString());
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
                Config.browserForm.consoleMsgErrorReport(ex.Message);
            }
        }

        public static async Task devTestAsync()
        {
            HatFrameworkDev.Tester tester = new HatFrameworkDev.Tester(Config.browserForm);
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test2.html", 5);
            HatFrameworkDev.FRAMEElement frame = await tester.GetFrameAsync(0);
            tester.ConsoleMsg("Index: " + frame.Index);
            tester.ConsoleMsg("Name: " + frame.Name);

            /* 1.
            string name = await frame.GetAttributeFromElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='login']", "name");
            tester.ConsoleMsg("Attribute: " + name);
            */

            /* 2.
            List<string> values = await frame.GetAttributeFromElementsAsync(HatFrameworkDev.Tester.BY_XPATH, "//input", "name");
            if (values != null)
            {
                foreach (string attr in values)
                    tester.ConsoleMsg(attr);
            }
            */

            /* 3.
            await frame.SetAttributeInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='buttonLogin']", "name", "NameButtonLogin");
            */

            /* 4.
            await frame.SetAttributeInElementsAsync(HatFrameworkDev.Tester.BY_XPATH, "//input", "name", "test");
            */

            /* 5.
            await frame.SetValueInElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='login']", "Тестировщик");
            string value = await frame.GetValueFromElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//input[@id='login']");
            */




            await tester.TestEndAsync();




            /*
            HatFrameworkDev.Tester tester = new HatFrameworkDev.Tester(Config.browserForm);
            await tester.BrowserEnableSendMailAsync();
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test.html", 5);
            await tester.SetValueInElementByIdAsync("login", "admin");
            await tester.WaitAsync(2);
            await tester.SetValueInElementByIdAsync("pass", "0001");
            await tester.WaitAsync(2);
            await tester.ClickElementByIdAsync("buttonLogin");
            await tester.WaitAsync(2);
            string actual = await tester.GetValueFromElementByIdAsync("textarea");
            string expected = "\"Вы успешно авторизованы\"";
            await tester.WaitVisibleElementByIdAsync("result", 5);
            await tester.AssertEqualsAsync(expected, actual);
            await tester.TestEndAsync();
            */

            /*
            HatFrameworkDev.Tester tester = new HatFrameworkDev.Tester(Config.browserForm);
            await tester.TestBeginAsync();
            await tester.GoToUrlAsync("https://somovstudio.github.io/test2.html", 5);
            HatFrameworkDev.HTMLElement element = await tester.GetElementAsync(HatFrameworkDev.Tester.BY_XPATH, "//*[@id='MySelect']");
            await element.SelectOptionAsync(HatFrameworkDev.HTMLElement.BY_INDEX, "2");
            await tester.WaitAsync(5);
            await element.SelectOptionAsync(HatFrameworkDev.HTMLElement.BY_VALUE, "Mobile");
            await tester.WaitAsync(5);
            await element.SelectOptionAsync(HatFrameworkDev.HTMLElement.BY_TEXT, "Other");

            string index = await element.GetOptionAsync(HatFrameworkDev.HTMLElement.BY_INDEX);
            string text = await element.GetOptionAsync(HatFrameworkDev.HTMLElement.BY_TEXT);
            string value = await element.GetOptionAsync(HatFrameworkDev.HTMLElement.BY_VALUE);

            await tester.AssertEqualsAsync("2", index);
            await tester.AssertEqualsAsync("\"Other\"", text);
            await tester.AssertEqualsAsync("\"Other\"", value);
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
            content += "using System.IO;" + Environment.NewLine;
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

        public static string getContentFileNewPage(string filename)
        {
            string content = "";
            content += "using System;" + Environment.NewLine;
            content += "using HatFramework;" + Environment.NewLine;
            content += "" + Environment.NewLine;
            content += "namespace Hat" + Environment.NewLine;
            content += "{" + Environment.NewLine;
            content += "    public static class " + filename + Environment.NewLine;
            content += "    {" + Environment.NewLine;
            content += "        public static string URL = \"https://test.com/\";" + Environment.NewLine;
            content += "        public static string ButtonLogin = \"buttonLogin\";" + Environment.NewLine;
            content += "    }" + Environment.NewLine;
            content += "}" + Environment.NewLine;
            return content;
        }

        public static string getContentFileNewStep(string filename)
        {
            string content = "";
            content += "using System;" + Environment.NewLine;
            content += "using System.Collections.Generic;" + Environment.NewLine;
            content += "using System.ComponentModel;" + Environment.NewLine;
            content += "using System.Windows.Forms;" + Environment.NewLine;
            content += "using System.Threading;" + Environment.NewLine;
            content += "using System.Threading.Tasks;" + Environment.NewLine;
            content += "using System.IO;" + Environment.NewLine;
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
            content += "    public class " + filename + " : Tester" + Environment.NewLine;
            content += "    {" + Environment.NewLine;
            content += "        public " + filename + "(Form browserWindow): base(browserWindow) {}" + Environment.NewLine;
            content += "" + Environment.NewLine;
            content += "        public async Task Test()" + Environment.NewLine;
            content += "        {" + Environment.NewLine;
            content += "            await this.AssertTrueAsync(true);" + Environment.NewLine;
            content += "        }" + Environment.NewLine;
            content += "    }" + Environment.NewLine;
            content += "}" + Environment.NewLine;
            return content;
        }

        public static string getContentGitIgnore()
        {
            string content = "";
            content += "/.vs/" + Environment.NewLine;
            content += "/bin/" + Environment.NewLine;
            content += "/obj/";
            return content;
        }

        public static string getContentFileSLN(string projectName)
        {
            string content = "";
            content += "Microsoft Visual Studio Solution File, Format Version 12.00" + Environment.NewLine;
            content += "# Visual Studio Version 17" + Environment.NewLine;
            content += "VisualStudioVersion = 17.0.31912.275" + Environment.NewLine;
            content += "MinimumVisualStudioVersion = 10.0.40219.1" + Environment.NewLine;
            content += "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"" + projectName +"\", \"" + projectName + ".csproj\", \"{FB1744EF-7E49-4425-BBD7-F03E8F7B79FE}\"" + Environment.NewLine;
            content += "EndProject" + Environment.NewLine;
            content += "Global" + Environment.NewLine;
            content += "	GlobalSection(SolutionConfigurationPlatforms) = preSolution" + Environment.NewLine;
            content += "		Debug|Any CPU = Debug|Any CPU" + Environment.NewLine;
            content += "		Release|Any CPU = Release|Any CPU" + Environment.NewLine;
            content += "	EndGlobalSection" + Environment.NewLine;
            content += "	GlobalSection(ProjectConfigurationPlatforms) = postSolution" + Environment.NewLine;
            content += "		{FB1744EF-7E49-4425-BBD7-F03E8F7B79FE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU" + Environment.NewLine;
            content += "		{FB1744EF-7E49-4425-BBD7-F03E8F7B79FE}.Debug|Any CPU.Build.0 = Debug|Any CPU" + Environment.NewLine;
            content += "		{FB1744EF-7E49-4425-BBD7-F03E8F7B79FE}.Release|Any CPU.ActiveCfg = Release|Any CPU" + Environment.NewLine;
            content += "		{FB1744EF-7E49-4425-BBD7-F03E8F7B79FE}.Release|Any CPU.Build.0 = Release|Any CPU" + Environment.NewLine;
            content += "	EndGlobalSection" + Environment.NewLine;
            content += "	GlobalSection(SolutionProperties) = preSolution" + Environment.NewLine;
            content += "		HideSolutionNode = FALSE" + Environment.NewLine;
            content += "	EndGlobalSection" + Environment.NewLine;
            content += "	GlobalSection(ExtensibilityGlobals) = postSolution" + Environment.NewLine;
            content += "		SolutionGuid = {0E635FC4-DAA6-4998-BF49-711898655671}" + Environment.NewLine;
            content += "	EndGlobalSection" + Environment.NewLine;
            content += "EndGlobal";
            return content;
        }

        public static string getContentFileCSPROJ()
        {
            string appPath = Directory.GetCurrentDirectory();

            string content = "";
            content += "<?xml version=\"1.0\" encoding=\"utf-8\"?>" + Environment.NewLine;
            content += "<Project ToolsVersion=\"15.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">" + Environment.NewLine;
            content += @"  <Import Project=""$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props"" Condition=""Exists('$(MSBuildExtensionsPath)\$(MSBuildToolsVersion)\Microsoft.Common.props')"" />" + Environment.NewLine;
            content += "  <PropertyGroup>" + Environment.NewLine;
            content += "    <Configuration Condition=\" '$(Configuration)' == '' \">Debug</Configuration>" + Environment.NewLine;
            content += "    <Platform Condition=\" '$(Platform)' == '' \">AnyCPU</Platform>" + Environment.NewLine;
            content += "    <ProjectGuid>{FB1744EF-7E49-4425-BBD7-F03E8F7B79FE}</ProjectGuid>" + Environment.NewLine;
            content += "    <OutputType>Library</OutputType>" + Environment.NewLine;
            content += "    <AppDesignerFolder>Properties</AppDesignerFolder>" + Environment.NewLine;
            content += "    <RootNamespace>HatTests</RootNamespace>" + Environment.NewLine;
            content += "    <AssemblyName>HatTests</AssemblyName>" + Environment.NewLine;
            content += "    <TargetFrameworkVersion>v4.8</TargetFrameworkVersion>" + Environment.NewLine;
            content += "    <FileAlignment>512</FileAlignment>" + Environment.NewLine;
            content += "    <Deterministic>true</Deterministic>" + Environment.NewLine;
            content += "  </PropertyGroup>" + Environment.NewLine;
            content += "  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' \">" + Environment.NewLine;
            content += "    <DebugSymbols>true</DebugSymbols>" + Environment.NewLine;
            content += "    <DebugType>full</DebugType>" + Environment.NewLine;
            content += "    <Optimize>false</Optimize>" + Environment.NewLine;
            content += @"    <OutputPath>bin\Debug\</OutputPath>" + Environment.NewLine;
            content += "    <DefineConstants>DEBUG;TRACE</DefineConstants>" + Environment.NewLine;
            content += "    <ErrorReport>prompt</ErrorReport>" + Environment.NewLine;
            content += "    <WarningLevel>4</WarningLevel>" + Environment.NewLine;
            content += "  </PropertyGroup>" + Environment.NewLine;
            content += "  <PropertyGroup Condition=\" '$(Configuration)|$(Platform)' == 'Release|AnyCPU' \">" + Environment.NewLine;
            content += "    <DebugType>pdbonly</DebugType>" + Environment.NewLine;
            content += "    <Optimize>true</Optimize>" + Environment.NewLine;
            content += @"    <OutputPath>bin\Release\</OutputPath>" + Environment.NewLine;
            content += "    <DefineConstants>TRACE</DefineConstants>" + Environment.NewLine;
            content += "    <ErrorReport>prompt</ErrorReport>" + Environment.NewLine;
            content += "    <WarningLevel>4</WarningLevel>" + Environment.NewLine;
            content += "  </PropertyGroup>" + Environment.NewLine;
            content += "  <ItemGroup>" + Environment.NewLine;
            content += "    <Reference Include=\"HatFramework\">" + Environment.NewLine;
            content += "      <HintPath>" + appPath + "\\HatFramework.dll</HintPath>" + Environment.NewLine;
            content += "    </Reference>" + Environment.NewLine;
            content += "    <Reference Include=\"Microsoft.Web.WebView2.Core\">" + Environment.NewLine;
            content += "      <HintPath>" + appPath + "\\Microsoft.Web.WebView2.Core.dll</HintPath>" + Environment.NewLine;
            content += "    </Reference>" + Environment.NewLine;
            content += "    <Reference Include=\"Microsoft.Web.WebView2.WinForms\">" + Environment.NewLine;
            content += "      <HintPath>" + appPath + "\\Microsoft.Web.WebView2.WinForms.dll</HintPath>" + Environment.NewLine;
            content += "    </Reference>" + Environment.NewLine;
            content += "    <Reference Include=\"Microsoft.Web.WebView2.Wpf\">" + Environment.NewLine;
            content += "      <HintPath>" + appPath + "\\Microsoft.Web.WebView2.Wpf.dll</HintPath>" + Environment.NewLine;
            content += "    </Reference>" + Environment.NewLine;
            content += "    <Reference Include=\"Newtonsoft.Json\">" + Environment.NewLine;
            content += "      <HintPath>" + appPath + "\\Newtonsoft.Json.dll</HintPath>" + Environment.NewLine;
            content += "    </Reference>" + Environment.NewLine;
            content += "    <Reference Include=\"System\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Core\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Data\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Data.DataSetExtensions\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Deployment\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Drawing\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Net.Http\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Xml\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Xml.Linq\" />" + Environment.NewLine;
            content += "    <Reference Include=\"System.Windows.Forms\" />" + Environment.NewLine;
            content += "    <Reference Include=\"Microsoft.CSharp\" />" + Environment.NewLine;
            content += "  </ItemGroup>" + Environment.NewLine;
            content += "  <ItemGroup>" + Environment.NewLine;
            content += @"    <Compile Include=""Properties\AssemblyInfo.cs"" />" + Environment.NewLine;
            content += @"    <Compile Include=""Tests\support\Helper.cs"" />" + Environment.NewLine;
            content += @"    <Compile Include=""Tests\support\PageObjects\ExamplePage.cs"" />" + Environment.NewLine;
            content += @"    <Compile Include=""Tests\support\StepObjects\ExampleSteps.cs"" />" + Environment.NewLine;
            content += @"    <Compile Include=""Tests\tests\ExampleTest1.cs"" />" + Environment.NewLine;
            content += @"    <Compile Include=""Tests\tests\ExampleTest2.cs"" />" + Environment.NewLine;
            content += "  </ItemGroup>" + Environment.NewLine;
            content += "  <ItemGroup>" + Environment.NewLine;
            content += @"    <None Include=""Tests\project.hat"" />" + Environment.NewLine;
            content += "  </ItemGroup>" + Environment.NewLine;
            content += "  <ItemGroup />" + Environment.NewLine;
            content += @"  <Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />" + Environment.NewLine;
            content += "</Project>";
            return content;
        }

        public static string getContentFileAssemblyInfo()
        {
            string content =
@"
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("""")]
[assembly: AssemblyDescription("""")]
[assembly: AssemblyConfiguration("""")]
[assembly: AssemblyCompany("""")]
[assembly: AssemblyProduct("""")]
[assembly: AssemblyCopyright(""Copyright © 2022"")]
[assembly: AssemblyTrademark("""")]
[assembly: AssemblyCulture("""")]
[assembly: ComVisible(false)]
[assembly: Guid(""fb1744ef-7e49-4425-bbd7-f03e8f7b79fe"")]
[assembly: AssemblyVersion(""1.0.0.0"")]
[assembly: AssemblyFileVersion(""1.0.0.0"")]
";
            return content;
        }





    }
}
