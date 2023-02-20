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
    public partial class CreateCmdForm : Form
    {
        public CreateCmdForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(textBox.Text);
                Config.browserForm.ConsoleMsg("Команда запуска скопирована в буфер обмена");
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg(ex.Message);
            }
        }
    }
}
