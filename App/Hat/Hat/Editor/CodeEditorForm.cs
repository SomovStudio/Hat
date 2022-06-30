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
        List<string[]> files; // [имя файла | путь файла | статус | индекс]

        private void CodeEditorForm_Load(object sender, EventArgs e)
        {
            try
            {
                files = new List<string[]>();
                this.TopMost = Config.editorTopMost;
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
                        if (files[i][1] == path)
                        {
                            parent.consoleMsg($"Файл {filename} уже открыт в редакторе");
                            return;
                        }
                    }
                }

                int index = tabControl1.TabPages.Count;
                files.Add(new string[] { filename, path, STATUS_SAVED, index.ToString() }); // [имя файла | путь файла | статус | индекс]

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
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }
        }

        private void textEditorControl_TextChanged(object sender, EventArgs e)
        {
            TextEditorControl textEditorControl = (TextEditorControl)sender;
            MessageBox.Show(textEditorControl.Tag.ToString());
        }

        public void CloseFile()
        {
            try
            {
                
                int index = tabControl1.SelectedIndex;
                int count = files.Count;
                if (index < 0 && count <= 0) return;

                string filename = "";
                if (Convert.ToInt32(files[index][3]) == index)
                {
                    filename = files[index][0];
                   
                    if (files[index][2] == STATUS_NOT_SAVE)
                    {
                        DialogResult dialogResult = MessageBox.Show($"Вы закрываете файла {filename} {Environment.NewLine}Вы хотите сохранить изменения в файле?", "Вопрос", MessageBoxButtons.YesNoCancel);

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
                }
            }
            catch (Exception ex)
            {
                parent.consoleMsg(ex.ToString());
            }
        }

        private void fileCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        
    }
}
