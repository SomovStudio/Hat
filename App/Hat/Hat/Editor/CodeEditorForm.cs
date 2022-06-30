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

        private void CodeEditorForm_Load(object sender, EventArgs e)
        {
            TextEditorControl textEditorControl = new TextEditorControl();
            textEditorControl.Name = "textEditorControl";
            textEditorControl.Text = "";
            textEditorControl.Dock = DockStyle.Fill;
            TabPage tab = new TabPage("file.cs");
            tab.Controls.Add(textEditorControl);
            tabControl1.TabPages.Add(tab);

            /*
            // Removes the selected tab:  
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            // Removes all the tabs:  
            tabControl1.TabPages.Clear();
            */
        }
    }
}
