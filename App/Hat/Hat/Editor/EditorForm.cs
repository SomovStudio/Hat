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
@"Tester
Описание: Конструктор класса
Параметры: Form browserForm
Пример: Tester tester tester = new Tester(browserWindow);",

@"IMAGE_STATUS_PROCESS
Описание: Индекс картинки которая обозначает статус в процессе.
Значение константы: 0",

@"IMAGE_STATUS_PASSED
Описание: Индекс картинки которая обозначает статус успешно.
Значение константы: 1",

@"IMAGE_STATUS_FAILED
Описание: Индекс картинки которая обозначает статус провально.
Значение константы: 2",

@"IMAGE_STATUS_MESSAGE
Описание: Индекс картинки которая обозначает статус сообщение.
Значение константы: 3",

@"IMAGE_STATUS_WARNING
Описание: Индекс картинки которая обозначает статус предупреждение.
Значение константы: 4",

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
                if (treeView1.SelectedNode != null)
                {
                    string value = treeView1.SelectedNode.Text;
                    if (value == "Tester") richTextBox1.Text = handbook[0];
                    if (value == "IMAGE_STATUS_PROCESS") richTextBox1.Text = handbook[1];
                    if (value == "IMAGE_STATUS_PASSED") richTextBox1.Text = handbook[2];
                    if (value == "IMAGE_STATUS_FAILED") richTextBox1.Text = handbook[3];
                    if (value == "IMAGE_STATUS_MESSAGE") richTextBox1.Text = handbook[4];
                    if (value == "IMAGE_STATUS_WARNING") richTextBox1.Text = handbook[5];

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
