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
        const string STATUS_EDIT = "status_edit";

        public BrowserForm parent;
        List<string[]> files; // [имя файла | путь файла | статус | индекс]

        private void CodeEditorForm_Load(object sender, EventArgs e)
        {
            files = new List<string[]>();
            this.TopMost = Config.editorTopMost;

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
            int count = files.Count;
            if(count > 0)
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
            textEditorControl.Name = "textEditorControl" + index.ToString();
            textEditorControl.Text = reader.readFile(Config.encoding, path);
            textEditorControl.Dock = DockStyle.Fill;
            textEditorControl.SetHighlighting("C#");
            TabPage tab = new TabPage(filename);
            tab.Controls.Add(textEditorControl);
            tabControl1.TabPages.Add(tab);
        }

        
    }
}
