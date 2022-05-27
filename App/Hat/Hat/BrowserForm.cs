using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Web.WebView2.Core;

namespace Hat
{
    public partial class BrowserForm : Form
    {
        public BrowserForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Config.encoding = WorkOnFiles.UTF_8_BOM;
            toolStripStatusLabelFileEncoding.Text = Config.encoding;
            Config.browserForm = this;
            consoleMsg("Браузер Hat версия 1.0");
            systemConsoleMsg("", default, default, default, true);
            systemConsoleMsg("Браузер Hat версия 1.0", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
        }

        private bool stopTest = false;
        private bool testSuccess = true;
        private StepTestForm stepTestForm;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                initWebView();

                this.Width = 1440;
                this.Height = 900;
                numericUpDownBrowserWidth.Value = panel1.Width;
                numericUpDownBrowserHeight.Value = panel1.Height;
                webView2.Source = new Uri(toolStripComboBoxUrl.Text);
                toolStripStatusLabelProjectPath.Text = Config.projectPath;

                if (Config.commandLineMode == true)
                {
                    systemConsoleMsg("Запуск браузера...", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    consoleMsg("Запуск браузера Hat из командной строки");
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;
                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);
                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                    systemConsoleMsg($"Проект успешно открыт (версия проекта: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                    consoleMsg($"Проект успешно открыт (версия проекта: {Config.version})");
                    toolStripStatusLabelProjectFolderFile.Text = Config.selectName;
                    PlayTest(Config.selectName);
                }

                
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BrowserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void BrowserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (testSuccess == false)
            {
                systemConsoleMsg(Environment.NewLine + "==============================", default, default, default, true);
                systemConsoleMsg("Tests ended. Finished: FAILURE", default, ConsoleColor.DarkRed, ConsoleColor.White, true);
                Environment.Exit(1);
            }
            else
            {
                systemConsoleMsg(Environment.NewLine + "==============================", default, default, default, true);
                systemConsoleMsg("Tests ended. Finished: SUCCESS", default, ConsoleColor.DarkGreen, ConsoleColor.White, true);
                Environment.Exit(0);
            }
        }

        /* Инициализация WevView */
        private void initWebView()
        {
            webView2.CoreWebView2InitializationCompleted += WebView_CoreWebView2InitializationCompleted;
        }

        private void WebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            try
            {
                consoleMsg("Инициализация WebView завершена");
                webView2.EnsureCoreWebView2Async();
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.setCacheDisabled", @"{""cacheDisabled"":true}");
                consoleMsg("Выполнена очистка кэша");
                webView2.EnsureCoreWebView2Async();
                webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Log.entryAdded").DevToolsProtocolEventReceived += showMessageConsoleErrors;
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Log.enable", "{}");
                consoleMsg("Запусщен монитор ошибок на страницах");
                webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Security.setIgnoreCertificateErrors", "{\"ignore\": true}");
                consoleMsg("Опция Security.setIgnoreCertificateErrors - включен параметр ignore: true");
                if (Config.defaultUserAgent == "") Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
                consoleMsg($"Опция User-Agent по умолчанию {Config.defaultUserAgent}");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }

        }

        /* Игнорирование сертификата */
        private async void ignorCertificateErrors()
        {
            try
            {
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Security.setIgnoreCertificateErrors", "{\"ignore\": true}");
                consoleMsg("Опция Security.setIgnoreCertificateErrors - включен параметр ignore: true");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Очистка кэша */
        private async void clearBrowserCache()
        {
            try
            {
                await webView2.EnsureCoreWebView2Async();
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.clearBrowserCache", "{}");
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Network.setCacheDisabled", @"{""cacheDisabled"":true}");
                consoleMsg("Выполнена очистка кэша");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Мониторинг ошибок на загруженной странице */
        private async void startMonitorConsoleErrors()
        {
            try
            {
                await webView2.EnsureCoreWebView2Async();
                webView2.CoreWebView2.GetDevToolsProtocolEventReceiver("Log.entryAdded").DevToolsProtocolEventReceived += showMessageConsoleErrors;
                await webView2.CoreWebView2.CallDevToolsProtocolMethodAsync("Log.enable", "{}");
                //webView2.CoreWebView2.OpenDevToolsWindow();
                //webView2.CoreWebView2.Navigate("https://stackoverflow.com");
                consoleMsg("Запусщен монитор ошибок на страницах");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void showMessageConsoleErrors(object sender, Microsoft.Web.WebView2.Core.CoreWebView2DevToolsProtocolEventReceivedEventArgs e)
        {
            if (e != null && e.ParameterObjectAsJson != null)
            {
                richTextBoxErrors.AppendText(e.ParameterObjectAsJson + Environment.NewLine);
                richTextBoxErrors.ScrollToCaret();
            }
        }

        private void webView2_SourceChanged(object sender, Microsoft.Web.WebView2.Core.CoreWebView2SourceChangedEventArgs e)
        {
            richTextBoxErrors.Text = "";
        }

        /* Сообщение в консоль */
        public void consoleMsg(string message)
        {
            try
            {
                richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] " + message + Environment.NewLine);
                richTextBoxConsole.ScrollToCaret();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        public void consoleMsgError(string message)
        {
            richTextBoxConsole.AppendText("[" + DateTime.Now.ToString() + "] ОШИБКА: " + message + Environment.NewLine);
            richTextBoxConsole.ScrollToCaret();
            systemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
            systemConsoleMsg("Произошла ошибка:", default, ConsoleColor.Black, ConsoleColor.Red, true);
            systemConsoleMsg(message, default, default, default, true);
            systemConsoleMsg("- - - - - - - - - - - - - - - - - - - - - - - - - - - -", default, default, default, true);
            systemConsoleMsg("", default, default, default, true);
            resultAutotest(false);
            if (Config.commandLineMode == true) Close();
        }


        public void systemConsoleMsg(string message, Encoding encoding, ConsoleColor backgroundColor, ConsoleColor foregroundColor, bool newLine)
        {
            // Console.BackgroundColor - Возвращает или задает цвет фона консоли. (Значение из перечисления, задающее фоновый цвет консоли, то есть цвет, на фоне которого выводятся символы. Значением по умолчанию является Black.)
            // Console.ForegroundColor - Возвращает или задает цвет фона консоли. (Значение из перечисления ConsoleColor, задающее цвет переднего плана консоли, то есть цвет, которым выводятся символы. По умолчанию задано значение Gray.)

            if (encoding == default)
            {
                System.Console.OutputEncoding = System.Text.Encoding.UTF8;
                System.Console.InputEncoding = System.Text.Encoding.UTF8;
            }
            else
            {
                System.Console.OutputEncoding = encoding;
                System.Console.InputEncoding = encoding;
            }

            if (backgroundColor == default) System.Console.BackgroundColor = ConsoleColor.Black;
            else System.Console.BackgroundColor = backgroundColor;
            if (foregroundColor == default) System.Console.ForegroundColor = ConsoleColor.Gray;
            else System.Console.ForegroundColor = foregroundColor;

            if (backgroundColor == default && foregroundColor == default) System.Console.ResetColor();

            if (newLine == true) System.Console.WriteLine(message);
            else System.Console.Write(message);

            System.Console.ResetColor();
        }

        /* Сообщение в таблицу */
        public void cleadMessageStep()
        {
            try
            {
                listViewTest.Items.Clear();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        public int sendMessageStep(string step, string status, string comment, int image)
        {
            ListViewItem item;
            ListViewItem.ListViewSubItem subitem;
            item = new ListViewItem();
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = step;
            item.SubItems.Add(subitem);
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = status;
            item.SubItems.Add(subitem);
            subitem = new ListViewItem.ListViewSubItem();
            subitem.Text = comment;
            item.SubItems.Add(subitem);
            item.ImageIndex = image;
            listViewTest.Items.Add(item);
            int index = listViewTest.Items.Count-1;
            return index;
        }

        public void editMessageStep(int index, string step, string status, string comment, int image)
        {
            try
            {
                if (image != null) listViewTest.Items[index].ImageIndex = image;
                if (step != null) listViewTest.Items[index].SubItems[1].Text = step;
                if (status != null) listViewTest.Items[index].SubItems[2].Text = status;
                if (comment != null) listViewTest.Items[index].SubItems[3].Text = comment;
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Возвращает браузер */
        public Microsoft.Web.WebView2.WinForms.WebView2 getWebView()
        {
            return webView2;
        }

        /* Возврат статус автотеста (остановка) */
        public bool checkStopTest()
        {
            return stopTest;
        }

        /* Результат выполнения автотеста */
        public void resultAutotest(bool success)
        {
            if (testSuccess == false) return;   // автотест был ранее провален
            testSuccess = success; // true - автотест был выполнен успешно | false - автотест был провелен
        }

        /* Настройка UserAgent */
        public void userAgent(string value)
        {
            if (Config.defaultUserAgent == "" && webView2.CoreWebView2.Settings.UserAgent != null)
            {
                Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
            }
            
            textBoxUserAgent.Text = value;
            webView2.CoreWebView2.Settings.UserAgent = textBoxUserAgent.Text;
            checkBoxUserAgent.Checked = false;
            textBoxUserAgent.ReadOnly = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxUserAgent.Checked)
            {
                textBoxUserAgent.ReadOnly = true;
                textBoxUserAgent.Text = Config.defaultUserAgent;
            }
            else
            {
                textBoxUserAgent.ReadOnly = false;
            }
            webView2.CoreWebView2.Settings.UserAgent = textBoxUserAgent.Text;
        }

        private void textBoxUserAgent_TextChanged(object sender, EventArgs e)
        {
            if (checkBoxUserAgent.Checked == false && textBoxUserAgent.ReadOnly == false)
            {
                webView2.CoreWebView2.Settings.UserAgent = textBoxUserAgent.Text;
            }
        }

        private void toolStripButtonBack_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.GoBack();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButtonForward_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.GoForward();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButtonGo_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.Source = new Uri(toolStripComboBoxUrl.Text);
                webView2.Update();
                updateToolStripComboBoxUrl();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButtonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.Reload();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void webView2_ContentLoading(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            try
            {
                toolStripComboBoxUrl.Text = webView2.Source.ToString();
                consoleMsg("Загружена страница: " + webView2.Source.ToString());
                if (webView2.CoreWebView2.Settings.UserAgent != null && Config.defaultUserAgent == "")
                {
                    Config.defaultUserAgent = webView2.CoreWebView2.Settings.UserAgent;
                    textBoxUserAgent.Text = Config.defaultUserAgent;
                }
                if (Config.defaultUserAgent != webView2.CoreWebView2.Settings.UserAgent) consoleMsg("Текущий User-Agent: " + webView2.CoreWebView2.Settings.UserAgent);
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripComboBoxUrl_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar.GetHashCode().ToString() == "851981")
                {
                    webView2.Source = new Uri(toolStripComboBoxUrl.Text);
                    webView2.Update();
                    updateToolStripComboBoxUrl();
                }
            }
            catch (Exception ex)
            {
                consoleMsg(ex.Message);
            }
        }

        private void updateToolStripComboBoxUrl()
        {
            try
            {
                bool thisIsNewUrl = true;
                for (int k = 0; k < toolStripComboBoxUrl.Items.Count; k++)
                {
                    if (toolStripComboBoxUrl.Items[k].ToString() == toolStripComboBoxUrl.Text)
                    {
                        thisIsNewUrl = false;
                        break;
                    }
                }
                if (thisIsNewUrl == true) toolStripComboBoxUrl.Items.Add(toolStripComboBoxUrl.Text);
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void создатьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            projectCreate();
        }

        /* Создать проект */
        private void projectCreate()
        {
            try
            {
                if (folderBrowserDialogProjectCreate.ShowDialog() == DialogResult.OK)
                {
                    Config.projectPath = folderBrowserDialogProjectCreate.SelectedPath;
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;

                    // создание папок
                    string folderSupport = "/support/";
                    string folderSupportPageObjects = "/support/PageObjects/";
                    string folderSupportStepObjects = "/support/StepObjects/";
                    string folderTests = "/tests/";

                    if (!Directory.Exists(Config.projectPath + folderSupport)) Directory.CreateDirectory(Config.projectPath + folderSupport);
                    if (!Directory.Exists(Config.projectPath + folderSupportPageObjects)) Directory.CreateDirectory(Config.projectPath + folderSupportPageObjects);
                    if (!Directory.Exists(Config.projectPath + folderSupportStepObjects)) Directory.CreateDirectory(Config.projectPath + folderSupportStepObjects);
                    if (!Directory.Exists(Config.projectPath + folderTests)) Directory.CreateDirectory(Config.projectPath + folderTests);

                    if (Directory.Exists(Config.projectPath + folderSupport) &&
                        Directory.Exists(Config.projectPath + folderSupportPageObjects) &&
                        Directory.Exists(Config.projectPath + folderSupportStepObjects) &&
                        Directory.Exists(Config.projectPath + folderTests))
                    {
                        consoleMsg("Создание проекта: все необходимые папки созданы");
                    }
                    else
                    {
                        consoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые папки)");
                        return;
                    }

                    // создание файлов
                    string fileProject = "/project.hat";
                    string fileSupportHelper = "/support/Helper.cs";
                    string fileSupportPageObjectsExample = "/support/PageObjects/ExamplePage.cs";
                    string fileSupportStepObjectsExample = "/support/StepObjects/ExampleSteps.cs";
                    string fileTestsExample1 = "/tests/ExampleTest1.cs";
                    string fileTestsExample2 = "/tests/ExampleTest2.cs";

                    WorkOnFiles writer = new WorkOnFiles();
                    if (!File.Exists(Config.projectPath + fileProject)) writer.writeFile(Config.getConfig(), WorkOnFiles.UTF_8_BOM, Config.projectPath + fileProject);
                    if (!File.Exists(Config.projectPath + fileSupportHelper)) writer.writeFile(Autotests.getContentFileHelper(), Config.encoding, Config.projectPath + fileSupportHelper);
                    if (!File.Exists(Config.projectPath + fileSupportPageObjectsExample)) writer.writeFile(Autotests.getContentFileExamplePage(), Config.encoding, Config.projectPath + fileSupportPageObjectsExample);
                    if (!File.Exists(Config.projectPath + fileSupportStepObjectsExample)) writer.writeFile(Autotests.getContentFileExampleSteps(), Config.encoding, Config.projectPath + fileSupportStepObjectsExample);
                    if (!File.Exists(Config.projectPath + fileTestsExample1)) writer.writeFile(Autotests.getContentFileExampleTest1(), Config.encoding, Config.projectPath + fileTestsExample1);
                    if (!File.Exists(Config.projectPath + fileTestsExample2)) writer.writeFile(Autotests.getContentFileExampleTest2(), Config.encoding, Config.projectPath + fileTestsExample2);

                    if (File.Exists(Config.projectPath + fileProject) && 
                        File.Exists(Config.projectPath + fileSupportHelper) && 
                        File.Exists(Config.projectPath + fileSupportPageObjectsExample) && 
                        File.Exists(Config.projectPath + fileSupportStepObjectsExample) && 
                        File.Exists(Config.projectPath + fileTestsExample1) &&
                        File.Exists(Config.projectPath + fileTestsExample2))
                    {
                        consoleMsg("Создание проекта: все необходимые файлы созданы");
                    }
                    else
                    {
                        consoleMsg("Создание проекта: процесс прерван (невозможно создать все необходимые файлы)");
                        return;
                    }

                    consoleMsg("Создание проекта: успешно завершено (версия проекта: " + Config.version + ")");

                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + fileProject);
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        
        private void открытьПроектToolStripMenuItem_Click(object sender, EventArgs e)
        {
            projectOpen();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            projectOpen();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            projectOpen();
        }

        /* Открыть файл проекта */
        private void projectOpen()
        {
            try
            {
                if(openFileProjectDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = openFileProjectDialog.FileName;
                    Config.projectPath = Path.GetDirectoryName(filename);
                    toolStripStatusLabelProjectPath.Text = Config.projectPath;

                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
;                   changeEncoding();
                    changeEditorTopMost();

                    consoleMsg("Проект успешно открыт (версия проекта: " + Config.version + ")");
                    systemConsoleMsg($"Проект успешно открыт (версия проекта: {Config.version})", default, ConsoleColor.DarkGray, ConsoleColor.White, true);
                }
             }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Обновить дерево файлов и папок */
        public void projectUpdate()
        {
            try
            {
                treeViewProject.Nodes.Clear();
                if (Config.projectPath != "(не открыт)")
                {
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);
                    consoleMsg("Данные в проводнике - обновлены");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Список подключенный библиотек */
        public void showLibs()
        {
            try
            {
                textBoxLibs.Text = "";
                for (int i = 0; i < Config.libraries.Length; i++)
                {
                    textBoxLibs.Text += Config.libraries[i];
                    if (i < Config.libraries.Length-1) textBoxLibs.Text += Environment.NewLine;
                }
                consoleMsg("Список библиотек - загружен");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Кодировка из файла конфигурации */
        public void changeEncoding()
        {
            try
            {
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;
                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;
                if (Config.encoding == WorkOnFiles.DEFAULT)
                {
                    dEFAULTToolStripMenuItem.Checked = true;
                    toolStripMenuItemDEFAULT.Checked = true;
                }
                if (Config.encoding == WorkOnFiles.UTF_8)
                {
                    uTF8ToolStripMenuItem.Checked = true;
                    toolStripMenuItemUTF8.Checked = true;
                }
                if (Config.encoding == WorkOnFiles.UTF_8_BOM)
                {
                    uTF8BOMToolStripMenuItem.Checked = true;
                    toolStripMenuItemUTF8BOM.Checked = true;
                }
                if (Config.encoding == WorkOnFiles.WINDOWS_1251)
                {
                    wINDOWS1251ToolStripMenuItem.Checked = true;
                    toolStripMenuItemWINDOWS1251.Checked = true;
                }
                consoleMsg("Кодировка файлов - выбрана");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Способ открытия редактора (поверх окон) */
        public void changeEditorTopMost()
        {
            try
            {
                editorTopMostToolStripMenuItem.Checked = Config.editorTopMost;
                toolStripMenuItemEditorTopMost.Checked = Config.editorTopMost;
                consoleMsg("Способ отображение редактора - выбран");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Открыть папку проекта */
        public void openProjectFolder(string path, TreeNodeCollection nodes)
        {
            try
            {
                bool currentNode = true;
                if (nodes.Count > 0) currentNode = false;
                else currentNode = true;

                foreach (string folder in Directory.GetDirectories(path))
                {
                    if (currentNode == true) nodes.Add(folder, getFolderName(folder), 0, 0);
                    else nodes[0].Nodes.Add(folder, getFolderName(folder), 0, 0);
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    if (currentNode == true)
                    {
                        if (getFileName(file) == "project.hat") nodes.Add(file, getFileName(file), 3, 3);
                        else if ((file[file.Length - 3].ToString() + file[file.Length - 2].ToString() + file[file.Length - 1].ToString()) == ".cs") nodes.Add(file, getFileName(file), 1, 1);
                        else nodes.Add(file, getFileName(file), 2, 2);
                    }
                    else
                    {
                        if (getFileName(file) == "project.hat") nodes[0].Nodes.Add(file, getFileName(file), 3, 3);
                        else if ((file[file.Length - 3].ToString() + file[file.Length - 2].ToString() + file[file.Length - 1].ToString()) == ".cs") nodes[0].Nodes.Add(file, getFileName(file), 1, 1);
                        else nodes[0].Nodes.Add(file, getFileName(file), 2, 2);
                    }
                }

                int index = 0;
                foreach (string folder in Directory.GetDirectories(path))
                {
                    if (currentNode == true) openProjectFolder(folder, nodes[index].Nodes);
                    else openProjectFolder(folder, nodes[0].Nodes[index].Nodes);
                    index++;
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        public string getFolderName(string path)
        {
            // Регулярные выражения онлайн http://regexstorm.net/tester
            //string pattern = @"\w{1,}$";
            string pattern = @"[^\\]{1,}\w{1,}$";
            string value = Regex.Match(path, pattern).Value;
            return value;
        }

        public string getFileName(string path)
        {
            string pattern = @"\w{1,}.\w{1,}$";
            string value = Regex.Match(path, pattern).Value;
            return value;
        }

        private void оПрограммеCrackerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showAbout();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            showAbout();
        }

        /* Открыть окно о программе */
        private void showAbout()
        {
            try
            {
                AboutForm about = new AboutForm();
                about.ShowDialog();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void dEFAULTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = true;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = true;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.DEFAULT;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void uTF8ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = true;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = true;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void uTF8BOMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = true;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = true;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8_BOM;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void wINDOWS1251ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = true;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = true;

                Config.encoding = WorkOnFiles.WINDOWS_1251;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripComboBoxUrl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                webView2.Source = new Uri(toolStripComboBoxUrl.Text);
                webView2.Update();
                updateToolStripComboBoxUrl();
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)") Process.Start(Config.projectPath);
                else consoleMsg("Проект не открыт");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemDEFAULT_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = true;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = true;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.DEFAULT;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;

                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemUTF8_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = true;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = true;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemUTF8BOM_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = true;
                wINDOWS1251ToolStripMenuItem.Checked = false;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = true;
                toolStripMenuItemWINDOWS1251.Checked = false;

                Config.encoding = WorkOnFiles.UTF_8_BOM;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemWINDOWS1251_Click(object sender, EventArgs e)
        {
            try
            {
                dEFAULTToolStripMenuItem.Checked = false;
                uTF8ToolStripMenuItem.Checked = false;
                uTF8BOMToolStripMenuItem.Checked = false;
                wINDOWS1251ToolStripMenuItem.Checked = true;

                toolStripMenuItemDEFAULT.Checked = false;
                toolStripMenuItemUTF8.Checked = false;
                toolStripMenuItemUTF8BOM.Checked = false;
                toolStripMenuItemWINDOWS1251.Checked = true;

                Config.encoding = WorkOnFiles.WINDOWS_1251;
                toolStripStatusLabelFileEncoding.Text = Config.encoding;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
                consoleMsg("Кодировка файлов - изменена");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void treeViewProject_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (treeViewProject.SelectedNode != null)
                {
                    Config.selectName = treeViewProject.SelectedNode.Text;
                    Config.selectValue = treeViewProject.SelectedNode.Name;
                    fileOpen();
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void treeViewProject_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (treeViewProject.SelectedNode != null)
                { 
                    Config.selectName = treeViewProject.SelectedNode.Text;
                    Config.selectValue = treeViewProject.SelectedNode.Name;
                    toolStripStatusLabelProjectFolderFile.Text = Config.selectName;
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    // Строится дерево папок и файлов
                    treeViewProject.Nodes.Clear();
                    treeViewProject.Nodes.Add(Config.projectPath, getFolderName(Config.projectPath), 0, 0);
                    openProjectFolder(Config.projectPath, treeViewProject.Nodes);

                    // Чтение файла конфигурации
                    Config.readConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    changeEncoding();
                    changeEditorTopMost();
                    consoleMsg("Обновлено дерево папок и файлов в окне проекта");
                }
                else
                {
                    consoleMsg("Проект не открыт");
                }
                
            }
            catch (Exception ex)
            {
                consoleMsg("Ошибка: " + ex.Message);
            }
        }

        public void PlayTest(string filename)
        {
            try
            {
                if (Config.selectName.Contains(".cs"))
                {
                    stopTest = false;
                    if (filename == null) Autotests.play(Config.selectName);
                    else Autotests.play(filename);
                }
                else
                {
                    consoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        public void StopTest()
        {
            stopTest = true;
            //Autotests.stopAsync();
            consoleMsg("Остановка автотеста");
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            StopTest();
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBoxConsole.Text = "";
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            StopTest();
        }

        private void listViewTest_DoubleClick(object sender, EventArgs e)
        {
            openStep();
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            openStep();
        }

        private void openStep()
        {
            try
            {
                if (listViewTest.Items.Count <= 0) return;
                if (stepTestForm == null)
                {
                    stepTestForm = new StepTestForm();
                    stepTestForm.parent = this;
                    stepTestForm.textBox1.Text = listViewTest.SelectedItems[0].SubItems[1].Text;
                    stepTestForm.textBox2.Text = listViewTest.SelectedItems[0].SubItems[2].Text;
                    stepTestForm.textBox3.Text = listViewTest.SelectedItems[0].SubItems[3].Text;
                    stepTestForm.Show();
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        public void closeStep()
        {
            if (stepTestForm != null) stepTestForm = null;
        }

        private void listViewTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listViewTest_Click(object sender, EventArgs e)
        {
            
        }

        private void listViewTest_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            try
            {
                if (stepTestForm != null && listViewTest.SelectedItems.Count > 0)
                {
                    stepTestForm.textBox1.Text = listViewTest.SelectedItems[0].SubItems[1].Text;
                    stepTestForm.textBox2.Text = listViewTest.SelectedItems[0].SubItems[2].Text;
                    stepTestForm.textBox3.Text = listViewTest.SelectedItems[0].SubItems[3].Text;
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void createFolder()
        {
            try
            {
                if (Directory.Exists(Config.selectValue) == true)
                {
                    InputBoxForm inputBox = new InputBoxForm();
                    inputBox.label.Text = "Введите пожалуйста имя новой папки";
                    inputBox.ShowDialog();
                    string foldername = inputBox.textBox.Text;
                    if (foldername == "" || Config.selectValue == "" || Config.selectName == "") return;

                    WorkOnFiles folder = new WorkOnFiles();
                    if (folder.folderCreate(Config.selectValue, foldername) == true)
                    {
                        consoleMsg($"Папка \"{foldername}\" - успешно создана");
                        projectUpdate();
                    }
                    else
                    {
                        consoleMsg($"Папка \"{foldername}\" - не создана");
                    }
                }
                else
                {
                    consoleMsg("Вы не выбрали папку");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFolder();
            else consoleMsg("Проект не открыт");
        }

        private void переименоватьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void deleteFolder()
        {
            try
            {
                if (Directory.Exists(Config.selectValue) == true)
                {
                    WorkOnFiles folder = new WorkOnFiles();
                    if (folder.folderDelete(Config.selectValue) == true)
                    {
                        consoleMsg($"Папка \"{Config.selectName}\" - успешно удалена");
                        projectUpdate();
                    }
                    else
                    {
                        consoleMsg($"Папка \"{Config.selectName}\" - не удалена");
                    }
                }
                else
                {
                    consoleMsg("Вы не выбрали папку");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void удалитьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (Config.projectPath != "(не открыт)") deleteFolder();
            else consoleMsg("Проект не открыт");
        }

        private void fileOpen()
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    if (File.Exists(Config.selectValue))
                    {
                        if (Config.selectName.Contains(".cs"))
                        {
                            EditorForm editorForm = new EditorForm();
                            editorForm.TopMost = Config.editorTopMost;
                            editorForm.Show();
                        }
                        else
                        {
                            try
                            {
                                Process.Start("notepad.exe", Config.selectValue);
                            }
                            catch (Exception ex)
                            {
                                consoleMsg(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    consoleMsg("Проект еще не открыть, откройте или создайте новый проект.");
                    projectOpen();
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") fileOpen();
            else consoleMsg("Проект не открыт");
        }

        private void createFile()
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    InputBoxForm inputBox = new InputBoxForm();
                    inputBox.label.Text = "Введите пожалуйста имя файла (расширение файла добавляется автоматически, его указывать специально не нужно)";
                    inputBox.ShowDialog();
                    string filename = inputBox.textBox.Text;
                    if (filename == "" || Config.selectValue == "" || Config.selectName == "") return;

                    string path = Config.projectPath;
                    if (Directory.Exists(Config.selectValue) == true) path = Config.selectValue;
                    if (path[path.Count() - 1].ToString() != "/") path += "/";

                    Autotests.nodes = new List<TreeNode>();
                    Autotests.readNodes(Config.browserForm.treeViewProject.Nodes);
                    string[] listfiles = Autotests.getCSharpFiles();
                    foreach (string csFile in listfiles)
                    {
                        if (csFile.Contains(filename))
                        {
                            consoleMsg("Не удалось создать файл с именем " + filename + " по скольку он уже существует в проекте");
                            return;
                        }
                    }

                    if (!File.Exists(path + filename + ".cs"))
                    {
                        WorkOnFiles writer = new WorkOnFiles();
                        writer.writeFile(Autotests.getContentFileNewTest(filename), Config.encoding, path + filename + ".cs");
                        if (File.Exists(path + filename + ".cs"))
                        {
                            consoleMsg("Новый файл теста " + filename + ".cs - успешно создан");
                            projectUpdate();
                        }
                        else consoleMsg("Новый файл теста " + filename + ".cs - не удалось создать");
                    }
                    else
                    {
                        consoleMsg("Файл " + path + filename + ".cs - уже существует");
                    }
                }
                else
                {
                    consoleMsg("Проект еще не открыть, откройте или создайте новый проект.");
                    projectOpen();
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void создатьФайлCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile();
            else consoleMsg("Проект не открыт");
        }

        private void переименоватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void deleteFile()
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {

                    if (Directory.Exists(Config.selectValue) == true) return;

                    if (File.Exists(Config.selectValue))
                    {
                        DialogResult dialogResult = MessageBox.Show($"Вы действительно хотите удалить файл {Config.selectName} ?", "Вопрос", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            File.Delete(Config.selectValue);
                            if (!File.Exists(Config.selectValue))
                            {
                                consoleMsg("Файл " + Config.selectValue + " - был успешно удалён");
                                projectUpdate();
                            }
                            else consoleMsg("Файл " + Config.selectValue + "- не удалось удалить");
                        }
                    }
                    else
                    {
                        consoleMsg("Файл " + Config.selectValue + " - не существует существует");
                    }
                }
                else
                {
                    consoleMsg("Проект еще не открыть, откройте или создайте новый проект.");
                    projectOpen();
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void удалитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") deleteFile();
            else consoleMsg("Проект не открыт");
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    string[] delimiter = { Environment.NewLine };
                    Config.libraries = textBoxLibs.Text.Split(delimiter, StringSplitOptions.None);
                    Config.saveConfigJson(Config.projectPath + "/project.hat");
                    showLibs();
                    consoleMsg("Скисок библиотек успешно сохранён в файл project.hat");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void editorTopMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (editorTopMostToolStripMenuItem.Checked)
                {
                    editorTopMostToolStripMenuItem.Checked = false;
                    toolStripMenuItemEditorTopMost.Checked = false;
                }
                else
                {
                    editorTopMostToolStripMenuItem.Checked = true;
                    toolStripMenuItemEditorTopMost.Checked = true;
                }
                Config.editorTopMost = editorTopMostToolStripMenuItem.Checked;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripMenuItemEditorTopMost_Click(object sender, EventArgs e)
        {
            try
            {
                if (toolStripMenuItemEditorTopMost.Checked)
                {
                    editorTopMostToolStripMenuItem.Checked = false;
                    toolStripMenuItemEditorTopMost.Checked = false;
                }
                else
                {
                    editorTopMostToolStripMenuItem.Checked = true;
                    toolStripMenuItemEditorTopMost.Checked = true;
                }
                Config.editorTopMost = editorTopMostToolStripMenuItem.Checked;
                if (Config.projectPath != "(не открыт)") Config.saveConfigJson(Config.projectPath + "/project.hat");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void запуститьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.selectName.Contains(".cs"))
                {
                    stopTest = false;
                    Autotests.play(Config.selectName);
                }
                else
                {
                    consoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void остановитьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopTest = true;
            //Autotests.stopAsync();
            consoleMsg("Остановка автотеста");
        }

        /* Поиск по таблице */
        int _tabFindIndex = 0;
        int _tabFindLast = 0;
        String _tabFindText = "";
        private void tabFindValue(ToolStripComboBox _cbox)
        {
            try
            {
                bool resolution = true;
                for (int k = 0; k < _cbox.Items.Count; k++)
                {
                    if (_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
                }
                if (resolution) _cbox.Items.Add(_cbox.Text);
                if (_tabFindText != _cbox.Text)
                {
                    _tabFindIndex = 0;
                    _tabFindLast = 0;
                    _tabFindText = _cbox.Text;
                }

                string _action;
                string _status;
                string _comment;
                int count = listViewTest.Items.Count;
                for (_tabFindIndex = _tabFindLast; _tabFindIndex < count; _tabFindIndex++)
                {
                    ListViewItem item = listViewTest.Items[_tabFindIndex];
                    _action = item.SubItems[1].Text;
                    _status = item.SubItems[2].Text;
                    _comment = item.SubItems[3].Text;

                    consoleMsg(_action + " | " + _status + " | " + _comment);

                    if (_action.Contains(_tabFindText) == true || _status.Contains(_tabFindText) == true || _comment.Contains(_tabFindText) == true)
                    {
                        listViewTest.Focus();
                        listViewTest.Items[_tabFindIndex].Selected = true;
                        listViewTest.EnsureVisible(_tabFindIndex);
                        _tabFindLast = _tabFindIndex + 1;
                        break;
                    }
                }

                if (_tabFindIndex >= count)
                {
                    consoleMsg("Поиск в таблице шагов выполнения теста - завершен");
                    _tabFindIndex = 0;
                    _tabFindLast = 0;
                    _tabFindText = _cbox.Text;
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tabFindValue(toolStripComboBoxFindValue);
        }

        private void запуститьТестToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PlayTest(null);
        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") fileOpen();
            else consoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFile();
            else consoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") deleteFile();
            else consoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") createFolder();
            else consoleMsg("Проект не открыт");
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (Config.projectPath != "(не открыт)") deleteFolder();
            else consoleMsg("Проект не открыт");
        }

        private void подробнаяИнформацияОШагеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openStep();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.selectName.Contains(".cs"))
                {
                    stopTest = false;
                    Autotests.play(Config.selectName);
                }
                else
                {
                    consoleMsg("Вы не выбрали файл для запуска. (выберите *.cs файл автотеста в окне проекта)");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void остановитьТестToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stopTest = true;
            consoleMsg("Остановка автотеста");
        }

        public void browserResize(int width, int height)
        {
            try
            {
                if (width > 0 && height > 0)
                {
                    radioButton2.Checked = true;
                    numericUpDownBrowserWidth.Value = width;
                    numericUpDownBrowserHeight.Value = height;
                }
                else
                {
                    radioButton1.Checked = true;
                }
                //consoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    panel1.Dock = DockStyle.Fill;
                    numericUpDownBrowserWidth.Value = panel1.Width;
                    numericUpDownBrowserWidth.ReadOnly = true;
                    numericUpDownBrowserHeight.Value = panel1.Height;
                    numericUpDownBrowserHeight.ReadOnly = true;
                }
                //consoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (radioButton2.Checked)
                {
                    panel1.Dock = DockStyle.None;
                    panel1.Width = (int)numericUpDownBrowserWidth.Value;
                    numericUpDownBrowserWidth.ReadOnly = false;
                    panel1.Height = (int)numericUpDownBrowserHeight.Value;
                    numericUpDownBrowserHeight.ReadOnly = false;
                }
                //consoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked)
                {
                    numericUpDownBrowserWidth.Value = panel1.Width;
                    numericUpDownBrowserHeight.Value = panel1.Height;
                }
                //consoleMsg("Размер браузера - изменён");
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void numericUpDownBrowserWidth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                panel1.Width = (int)numericUpDownBrowserWidth.Value;
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void numericUpDownBrowserHeight_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                panel1.Height = (int)numericUpDownBrowserHeight.Value;
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            Autotests.devTestStutsAsync();
            /*
            EditorForm editorForm = new EditorForm();
            editorForm.TopMost = Config.editorTopMost;
            editorForm.Show();
            */
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            richTextBoxErrors.Text = "";
        }

        private async void toolStripButton16_Click(object sender, EventArgs e)
        {
            try
            {
                await webView2.EnsureCoreWebView2Async();
                string script =
                @"(function(){
                var performance = window.performance || window.mozPerformance || window.msPerformance || window.webkitPerformance || {};
                var network = performance.getEntriesByType('resource') || {};
                var result = JSON.stringify(network);
                return result;
                }());";
                string jsonText = await webView2.CoreWebView2.ExecuteScriptAsync(script);
                dynamic result = JsonConvert.DeserializeObject(jsonText);
                richTextBoxEvents.Text = result;
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void средстваРазработкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView2.CoreWebView2.OpenDevToolsWindow();
        }

        private void toolStripButton14_Click_1(object sender, EventArgs e)
        {
            richTextBoxEvents.Text = "";
        }

        private void выводToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileLogDialog.FileName = "";
                if (saveFileLogDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxConsole.SaveFile(saveFileLogDialog.FileName, RichTextBoxStreamType.PlainText);
                    consoleMsg($"Лог вывода сохранён в файл: {saveFileLogDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void ошибкиНаСтраницеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileLogDialog.FileName = "";
                if (saveFileLogDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxErrors.SaveFile(saveFileLogDialog.FileName, RichTextBoxStreamType.PlainText);
                    consoleMsg($"Лог ошибок на странице сохранён в файл: {saveFileLogDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        private void событияНаСтраницеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileLogDialog.FileName = "";
                if (saveFileLogDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBoxEvents.SaveFile(saveFileLogDialog.FileName, RichTextBoxStreamType.PlainText);
                    consoleMsg($"Лог событий на странице сохранён в файл: {saveFileLogDialog.FileName}");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }

        /* Поиск по тексту */
        int _findIndex = 0;
        int _findLast = 0;
        String _findText = "";
        private void findText(ToolStripComboBox _cbox, RichTextBox _richTextBox)
        {
            try
            {
                bool resolution = true;
                for (int k = 0; k < _cbox.Items.Count; k++)
                    if (_cbox.Items[k].ToString() == _cbox.Text) resolution = false;
                if (resolution) _cbox.Items.Add(_cbox.Text);
                if (_findText != _cbox.Text)
                {
                    _findIndex = 0;
                    _findLast = 0;
                    _findText = _cbox.Text;
                }
                if (_richTextBox.Find(_cbox.Text, _findIndex, _richTextBox.TextLength - 1, RichTextBoxFinds.None) >= 0)
                {
                    _richTextBox.Select();
                    _findIndex = _richTextBox.SelectionStart + _richTextBox.SelectionLength;
                    if (_findLast == _richTextBox.SelectionStart)
                    {
                        MessageBox.Show("Поиск завершен");
                        _findIndex = 0;
                        _findLast = 0;
                        _findText = _cbox.Text;
                    }
                    else
                    {
                        _findLast = _richTextBox.SelectionStart;
                    }
                }
                else
                {
                    MessageBox.Show("Поиск завершен");
                    _findIndex = 0;
                    _findLast = 0;
                    _findText = _cbox.Text;
                }

            }
            catch (Exception ex)
            {
                consoleMsgError(ex.Message);
            }
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            findText(toolStripComboBoxErrors, richTextBoxErrors);
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            findText(toolStripComboBoxEvents, richTextBoxEvents);
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            try
            {
                if (Config.projectPath != "(не открыт)")
                {
                    CreateCmdForm createCmdForm = new CreateCmdForm();
                    createCmdForm.textBox.Text = $"cd {Directory.GetCurrentDirectory()}" + Environment.NewLine;
                    createCmdForm.textBox.Text += $"Hat.exe {toolStripStatusLabelProjectFolderFile.Text} {toolStripStatusLabelProjectPath.Text}";
                    createCmdForm.ShowDialog();
                }
                else
                {
                    consoleMsg("Проект не открыт");
                }
            }
            catch (Exception ex)
            {
                consoleMsgError(ex.ToString());
            }
        }
    }
}
