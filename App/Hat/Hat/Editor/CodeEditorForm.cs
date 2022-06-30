using ICSharpCode.TextEditor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hat
{
    public partial class CodeEditorForm : Form
    {
        public CodeEditorForm()
        {
            InitializeComponent();
        }

        const string STATUS_SAVED = "status_saved";
        const string STATUS_NOT_SAVE = "status_not_saved";

        public BrowserForm parent;
        List<object[]> files; // [имя файла | путь файла | статус | индекс | TabPage (вкладка) | TextEditorControl (редактор)]

        private void CodeEditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                files = new List<object[]>();
                this.TopMost = Config.editorTopMost;
                toolStripStatusLabel2.Text = Config.encoding;
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }

            /*
            // Removes the selected tab:  
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            // Removes all the tabs:  
            tabControl1.TabPages.Clear();
            */
        }

        private void CodeEditorForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.СloseCodeEditor();
        }

        public void OpenFile(string filename, string path)
        {
            try
            {
                int count = files.Count;
                if (count > 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (files[i][1].ToString() == path)
                        {
                            parent.consoleMsg($"Файл {filename} уже открыт в редакторе");
                            return;
                        }
                    }
                }

                int index = tabControl1.TabPages.Count;
                
                WorkOnFiles reader = new WorkOnFiles();

                TextEditorControl textEditorControl = new TextEditorControl();
                textEditorControl.Tag = index.ToString();
                textEditorControl.Name = "textEditorControl" + index.ToString();
                textEditorControl.Text = reader.readFile(Config.encoding, path);
                textEditorControl.Dock = DockStyle.Fill;
                textEditorControl.SetHighlighting("C#");
                textEditorControl.TextChanged += new System.EventHandler(this.textEditorControl_TextChanged);

                TabPage tab = new TabPage(filename);
                tab.Controls.Add(textEditorControl);
                tabControl1.TabPages.Add(tab);

                files.Add(new object[] { filename, path, STATUS_SAVED, index, tab, textEditorControl }); // [имя файла | путь файла | статус | индекс | TabPage (вкладка) | TextEditorControl (редактор)]

                toolStripStatusLabel5.Text = path;
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }
        }

        private void textEditorControl_TextChanged(object sender, EventArgs e)
        {
            TextEditorControl textEditorControl = (TextEditorControl)sender;
            int index = Convert.ToInt32(textEditorControl.Tag);
            files[index][2] = STATUS_NOT_SAVE;
            (files[index][4] as TabPage).Text = files[index][0].ToString() + " *";
        }

        private void closeFile()
        {
            try
            {
                
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                string filename = "";
                if (Convert.ToInt32(files[index][3]) == index)
                {
                    filename = files[index][0].ToString();
                   
                    if (files[index][2].ToString() == STATUS_NOT_SAVE)
                    {
                        DialogResult dialogResult = MessageBox.Show($"Закрывается файл {filename} {Environment.NewLine}Вы хотите сохранить изменения в файле?", "Вопрос", MessageBoxButtons.YesNoCancel);

                        if (dialogResult == DialogResult.Cancel)
                        {
                            //parent.consoleMsg("Отмена закрытия файла");
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                            files.RemoveAt(index);
                            parent.consoleMsg($"Файл {filename} - закрыт без сохранения");
                            updateListFiles();
                        }
                        else if (dialogResult == DialogResult.Yes)
                        {
                            parent.consoleMsg($"Файл {filename} - сохранён и закрыт");
                        }
                    }
                    else
                    {
                        tabControl1.TabPages.Remove(tabControl1.SelectedTab);
                        files.RemoveAt(index);
                        parent.consoleMsg($"Файл {filename} - закрыт");
                        updateListFiles();
                    }
                }
                else
                {
                    parent.consoleMsg($"Файл {filename} - неудалось закрыть");
                }
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }
        }

        private void updateListFiles()
        {
            try
            {
                int count = tabControl1.TabPages.Count;
                for (int i = 0; i < count; i++)
                {
                    files[i][3] = i.ToString();
                    (files[i][5] as TextEditorControl).Tag = i.ToString();
                    //parent.consoleMsg($"{files[i][0]} | {files[i][1]} | {files[i][2]} | {files[i][3]} | {files[i][4]} | {(files[i][5] as TextEditorControl).Tag} | ");
                }
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }
        }

        private void fileCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            closeFile();
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                int index = tabControl1.SelectedIndex;
                toolStripStatusLabel5.Text = files[index][1].ToString();
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }
            
        }

        private void saveFile()
        {
            
            /*
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
                parent.consoleMsg(ex.ToString());
            }
            */
        }

        private void fileSaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void fileSaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            saveFile();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        
    }
}
