using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JenkinsAgentRun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public delegate void AddConsoleItem(String message);
        public AddConsoleItem myDelegate;
        Process P;

        private bool CancelClose = true;

        public void addConsoleItemMethod(String message)
        {
            consoleMessage(message);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            myDelegate = new AddConsoleItem(addConsoleItemMethod);

            try
            {
                if (File.Exists("readme.txt") == false)
                {
                    createReadme();
                }

                if (File.Exists("run-agent.bat") == false)
                {
                    consoleMessage("Файл run-agent.bat - отсутствует");
                    createBatFile();
                    return;
                }

                if (File.Exists("agent.jar") == false)
                {
                    consoleMessage("Файл agent.jar - отсутствует");
                    return;
                }

                if (File.Exists("slave-agent.jnlp") == false)
                {
                    consoleMessage("Файл slave-agent.jnlp - отсутствует");
                    return;
                }


                P = new Process();
                P.StartInfo.FileName = "run-agent.bat";
                P.StartInfo.Arguments = "/k";
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.RedirectStandardInput = true;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.CreateNoWindow = true;
                P.StartInfo.UseShellExecute = false;
                P.ErrorDataReceived += P_ErrorDataReceived;
                P.OutputDataReceived += P_OutputDataReceived;
                P.Start();
                P.BeginErrorReadLine();
                P.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                this.Invoke(this.myDelegate, new object[] { e.Data.ToString() });
            }
            catch (Exception ex)
            {

            }
        }
        void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            try
            {
                this.Invoke(this.myDelegate, new object[] { e.Data.ToString() });
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (P != null)
                {
                    P.Kill();
                    Process[] processlist = Process.GetProcesses();
                    foreach (Process theprocess in processlist)
                    {
                        if (theprocess.ProcessName == "java" && theprocess.SessionId > 0)
                        {
                            theprocess.Kill();
                            consoleMessage("Kill process: " + theprocess.ProcessName + " | " + theprocess.Id.ToString() + " | " + theprocess.SessionId.ToString() + " | " + theprocess.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = CancelClose;
            if (CancelClose == true) this.Visible = false;
        }

        private void consoleMessage(String message)
        {
            consoleRichTextBox.Text = consoleRichTextBox.Text + message + Environment.NewLine;
            consoleRichTextBox.Select(consoleRichTextBox.Text.Length, consoleRichTextBox.Text.Length);
            consoleRichTextBox.ScrollToCaret();
        }

        private void выключитьАгентаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CancelClose = false;
            Close();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void выключитьАгентаToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CancelClose = false;
            Close();
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (File.Exists("readme.txt") == false)
                {
                    createReadme();
                }

                if (File.Exists("run-agent.bat") == false)
                {
                    consoleMessage("Файл run-agent.bat - отсутствует");
                    createBatFile();
                    return;
                }

                if (File.Exists("agent.jar") == false)
                {
                    consoleMessage("Файл agent.jar - отсутствует");
                    return;
                }

                if (File.Exists("slave-agent.jnlp") == false)
                {
                    consoleMessage("Файл slave-agent.jnlp - отсутствует");
                    return;
                }

                P = new Process();
                P.StartInfo.FileName = "run-agent.bat";
                P.StartInfo.Arguments = "/k";
                P.StartInfo.RedirectStandardError = true;
                P.StartInfo.RedirectStandardInput = true;
                P.StartInfo.RedirectStandardOutput = true;
                P.StartInfo.CreateNoWindow = true;
                P.StartInfo.UseShellExecute = false;
                P.ErrorDataReceived += P_ErrorDataReceived;
                P.OutputDataReceived += P_OutputDataReceived;
                P.Start();
                P.BeginErrorReadLine();
                P.BeginOutputReadLine();
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process[] processlist = Process.GetProcesses();
                foreach (Process theprocess in processlist)
                {
                    if (theprocess.ProcessName == "java" && theprocess.SessionId > 0)
                    {
                        theprocess.Kill();
                        consoleMessage("Kill process: " + theprocess.ProcessName + " | " + theprocess.Id.ToString() + " | " + theprocess.SessionId.ToString() + " | " + theprocess.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void createBatFile()
        {
            try
            {
                StreamWriter writer;
                //    writer = new StreamWriter(filename, false, Encoding.Default);
                writer = new StreamWriter("run-agent.bat", false, new UTF8Encoding(false));
                //    writer = new StreamWriter(filename, false, new UTF8Encoding(true));
                //    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                //    writer = new StreamWriter(filename, false, Encoding.Default);
                writer.Write("java -Dfile.encoding=UTF8 -jar agent.jar -jnlpUrl http://localhost:8080/computer/agent/slave-agent.jnlp -secret 0000000000000000000000000000000000000000000000000000000000000000 -workDir \"C:\\Hat\\jenkins\\workspace_proxy\"");
                writer.Close();
                consoleMessage("Создан файл run-agent.bat со значениями по умолчанию");
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }

        private void createReadme()
        {
            try
            {
                StreamWriter writer;
                //    writer = new StreamWriter(filename, false, Encoding.Default);
                //    writer = new StreamWriter("readme.txt", false, new UTF8Encoding(false));
                writer = new StreamWriter("readme.txt", false, new UTF8Encoding(true));
                //    writer = new StreamWriter(filename, false, Encoding.GetEncoding("Windows-1251"));
                //    writer = new StreamWriter(filename, false, Encoding.Default);

                string readme = @"
НАСТРОЙКА И ЗАПУСК АГЕНТА
=========================
1. Открыть ""Настройки Jenkins"" http://localhost:8080/manage
2. Перейти в ""Глобальные настройки безопасности"" http://localhost:8080/configureSecurity/
3. В разделе Agents включить флаг TCP port for JNLP agents в состояние Случайный/Random
4. Сохранить настройки
5. Вернуться в ""Настройки Jenkins"" http://localhost:8080/manage
6. Перейти в ""Управление средами сборки"" http://localhost:8080/computer/
7. Нажать на кнопку ""Новый узел"" (New Node)
	7.1 ввести наименование (например: proxy)
	7.2 включить флаг Permanent Agent
	7.3 нажать ОК
	7.4 Описание: Тестирование с помощью BrowserMob Proxy
	7.5 Количество процессов-исполнителей: 1
	7.6 Корень удаленной ФС: C:\Program Files (x86)\Jenkins\workspace_proxy
	7.7 Метки: proxy
	7.8 Использование: Use this node as much as possible
	7.9 Способ запуска: Launch agent via Java Web Start (все параметры оставить по умолчанию)
	7.10 Доступность: Keep this agent online as much as possible
	7.11 Node Properties: не включать флаги
	7.12 нажать кнопку Save
8. Вернуться в ""Управление средами сборки"" http://localhost:8080/computer/
9. В таблице нажать на proxy
10. Несколько способов запуска агента
	10.1 способ №1: выполнить команду
		javaws http://localhost:8080/computer/proxy/slave-agent.jnlp
		
	10.2 способ №2: скачать файл agent.jar по ссылке http://localhost:8080/jnlpJars/agent.jar	и выполнить команду
		java -jar agent.jar -jnlpUrl http://localhost:8080/computer/proxy/slave-agent.jnlp -secret 0000000000000000000000000000000000000000000000000000000000000000 -workDir ""C:\Program Files (x86)\Jenkins\workspace_proxy""
		
11. Настройка Job для работы с агентом
	11.1 включить параметр ""Ограничить лейблы сборщиков, которые могут исполнять данную задачу""
	11.2 в поле Label Expression ввести метку proxy
";

                writer.Write(readme);
                writer.Close();
                consoleMessage("Создан файл readme.txt");
            }
            catch (Exception ex)
            {
                consoleMessage(ex.Message);
            }
        }
    }
}
