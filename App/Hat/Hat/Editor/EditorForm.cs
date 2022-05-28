using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;

namespace Hat
{
    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

        private string[] handbook = new string[] {
@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 Tester\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'f0\'f3\'ea\'f2\'ee\'f0 \'ea\'eb\'e0\'f1\'f1\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : Tester(Form browserForm)\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sl240\slmult1\cf0 Tester tester;\par
public async void Main(Form browserWindow)\par
\{\par
\tab tester = new Tester(browserWindow);\par
\}\par

\pard\sa200\sl276\slmult1\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_PROCESS\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2 \'ef\'f0\'ee\'f6\'e5\'f1\'f1\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_PROCESS =  0\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_PROCESS\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_PASSED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'f3\'f1\'ef\'e5\'f8\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_PASSED =  1\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_PASSED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_FAILED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_FAILED =  2\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_FAILED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_MESSAGE\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'f1\'ee\'ee\'e1\'f9\'e5\'ed\'e8\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_MESSAGE =  3\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_MESSAGE\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 IMAGE_STATUS_WARNING\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'e8\'ed\'e4\'e5\'ea\'f1 \'ea\'e0\'f0\'f2\'e8\'ed\'ea\'e8 \'ea\'ee\'f2\'ee\'f0\'e0\'ff \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : IMAGE_STATUS_WARNING =  4\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.IMAGE_STATUS_WARNING\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 PASSED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'f3\'f1\'ef\'e5\'f8\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : PASSED = ""\'d3\'f1\'ef\'e5\'f8\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.PASSED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 FAILED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : FAILED = ""\'cf\'f0\'ee\'e2\'e0\'eb\'fc\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.FAILED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 STOPPED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ee\'f1\'f2\'e0\'ed\'ee\'e2\'eb\'e5\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : STOPPED = ""\'ce\'f1\'f2\'e0\'ed\'ee\'e2\'eb\'e5\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.STOPPED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 PROCESS\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2\'f1\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : PROCESS = ""\'c2\'fb\'ef\'ee\'eb\'ed\'ff\'e5\'f2\'f1\'ff""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.PROCESS\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 COMPLETED\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'e2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'ee\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : COMPLETED\f0\lang1033  \f1\lang1049 = ""\'c2\'fb\'ef\'ee\'eb\'ed\'e5\'ed\'ee""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.COMPLETED\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 WARNING\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ea\'ee\'ed\'f1\'f2\'e0\'ed\'f2\'e0 \'ee\'e1\'ee\'e7\'ed\'e0\'f7\'e0\'e5\'f2 \'f1\'f2\'e0\'f2\'f3\'f1 \'ef\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e5\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WARNING\f0\lang1033  \f1\lang1049 = ""\'cf\'f0\'e5\'e4\'f3\'ef\'f0\'e5\'e6\'e4\'e5\'ed\'e8\'e5""\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par

\pard\sa200\sl276\slmult1\cf0 Tester.WARNING\f0\fs22\lang9\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserView\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'f1\'f1\'fb\'eb\'e0\'e5\'f2\'f1\'ff \'ed\'e0 \'ee\'e1\'fa\'e5\'ea\'f2 \'ef\'f0\'e5\'e4\'f1\'f2\'e0\'e2\'eb\'ff\'fe\'f9\'e8\'e9 \'ee\'e1\'eb\'e0\'f1\'f2\'fc \'e1\'f0\'e0\'f3\'e7\'e5\'f0\'e0\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : WebView2 BrowserView\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 BrowserView.Reload();\par
BrowserView.Source = new Uri(url);\par
BrowserView.Update();\par
}",

@"{\rtf1\ansi\ansicpg1251\deff0\nouicompat\deflang1049{\fonttbl{\f0\fnil\fcharset0 Calibri;}{\f1\fnil\fcharset204 Calibri;}}
{\colortbl ;\red0\green77\blue187;\red155\green0\blue211;\red0\green0\blue0;}
{\*\generator Riched20 10.0.22000}\viewkind4\uc1 
\pard\sl276\slmult1\cf1\f0\fs20\lang9 BrowserWindow\cf0\par
\cf2\f1\lang1049\'ce\'ef\'e8\'f1\'e0\'ed\'e8\'e5\cf0 : \'ef\'e5\'f0\'e5\'ec\'e5\'ed\'ed\'e0\'ff \'f1\'f1\'fb\'eb\'e0\'e5\'f2\'f1\'ff \'ed\'e0 \'ee\'ea\'ed\'ee \'ef\'f0\'e8\'eb\'ee\'e6\'e5\'ed\'e8\'ff\par
\cf2\'d1\'e8\'ed\'f2\'e0\'ea\'f1\'e8\'f1\cf0 : Form BrowserWindow\par
\cf3\par
\cf2\'cf\'f0\'e8\'ec\'e5\'f0\cf3 :\par
\cf0 Tester tester;\par
public async void Main(Form browserWindow)\par
\{\par
\tab tester = new Tester(browserWindow);\par
\}\f0\fs22\lang9\par
}",

@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
@"",
        };

        private void EditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = Config.selectName;
                this.TopMost = Config.editorTopMost;
                WorkOnFiles reader = new WorkOnFiles();
                textEditorControl1.SetHighlighting("C#");
                textEditorControl1.Text = reader.readFile(Config.encoding, Config.selectValue);
                toolStripStatusLabel2.Text = Config.encoding;
                toolStripStatusLabel5.Text = Config.selectValue;
                toolStripStatusLabel6.Text = "";

                
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.Text == "") return;
            try
            {
                WorkOnFiles write = new WorkOnFiles();
                write.writeFile(textEditorControl1.Text, toolStripStatusLabel2.Text, toolStripStatusLabel5.Text);
                Config.browserForm.consoleMsg($"Файл {this.Text} - сохранён");
                toolStripStatusLabel6.Text = "";
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.Text = Config.browserForm.getFileName(saveFileDialog1.FileName);
                    toolStripStatusLabel5.Text = saveFileDialog1.FileName;

                    WorkOnFiles write = new WorkOnFiles();
                    write.writeFile(textEditorControl1.Text, toolStripStatusLabel2.Text, toolStripStatusLabel5.Text);
                    Config.browserForm.consoleMsg($"Файл {this.Text} - сохранён");
                    Config.browserForm.projectUpdate();
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if(this.Text != "") Config.browserForm.PlayTest(this.Text);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Config.browserForm.StopTest();
        }

        private void setValueInCode()
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    if (treeView1.SelectedNode.Text == "Класс: Tester") return;
                    if (treeView1.SelectedNode.Text == "Конструктор") return;
                    if (treeView1.SelectedNode.Text == "Константы") return;
                    if (treeView1.SelectedNode.Text == "Переменные") return;
                    if (treeView1.SelectedNode.Text == "Методы для работы с браузером") return;
                    if (treeView1.SelectedNode.Text == "Методы для вывода сообщений") return;
                    if (treeView1.SelectedNode.Text == "Методы для подготовки и завершению тестирования") return;
                    if (treeView1.SelectedNode.Text == "Методы выполнения действий") return;
                    if (treeView1.SelectedNode.Text == "Методы выполнения JavaScript") return;
                    if (treeView1.SelectedNode.Text == "Методы для проверки результата") return;
                    
                    Clipboard.SetText(treeView1.SelectedNode.Text);
                    textEditorControl1.Focus();
                    SendKeys.Send("+{INSERT}");
                    //SendKeys.Send("^{v}");
                    //SendKeys.Send("^v");
                    //SendKeys.Send("^(v)");
                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            setValueInCode();
        }

        private void вставитьВКодToolStripMenuItem_Click(object sender, EventArgs e)
        {
            setValueInCode();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                richTextBox1.Text = "";
                richTextBox1.Rtf = "";
                if (treeView1.SelectedNode != null)
                {
                    string value = treeView1.SelectedNode.Text;
                    if (value == "Tester") richTextBox1.Rtf = handbook[0];
                    if (value == "IMAGE_STATUS_PROCESS") richTextBox1.Rtf = handbook[1];
                    if (value == "IMAGE_STATUS_PASSED") richTextBox1.Rtf = handbook[2];
                    if (value == "IMAGE_STATUS_FAILED") richTextBox1.Rtf = handbook[3];
                    if (value == "IMAGE_STATUS_MESSAGE") richTextBox1.Rtf = handbook[4];
                    if (value == "IMAGE_STATUS_WARNING") richTextBox1.Rtf = handbook[5];
                    if (value == "PASSED") richTextBox1.Rtf = handbook[6];
                    if (value == "FAILED") richTextBox1.Rtf = handbook[7];
                    if (value == "STOPPED") richTextBox1.Rtf = handbook[8];
                    if (value == "PROCESS") richTextBox1.Rtf = handbook[9];
                    if (value == "COMPLETED") richTextBox1.Rtf = handbook[10];
                    if (value == "WARNING") richTextBox1.Rtf = handbook[11];

                    if (value == "BrowserView") richTextBox1.Rtf = handbook[12];
                    if (value == "BrowserWindow") richTextBox1.Rtf = handbook[13];

                }
            }
            catch (Exception ex)
            {
                Config.browserForm.consoleMsgError(ex.ToString());
            }
        }

        private void textEditorControl1_TextChanged(object sender, EventArgs e)
        {
            toolStripStatusLabel6.Text = "(изменения не сохранены) |";
        }

        private void EditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(toolStripStatusLabel6.Text == "(изменения не сохранены) |")
            {
                if(MessageBox.Show("Изменения не сохранены, всё равно закрыть редактор?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
