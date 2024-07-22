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
                Config.browserForm.ConsoleMsg("Команда запуска скопирована в буфер обмена", "The launch command has been copied to the clipboard");
            }
            catch (Exception ex)
            {
                Config.browserForm.ConsoleMsg(ex.Message, ex.Message);
            }
        }

        private void CreateCmdForm_Load(object sender, EventArgs e)
        {
            if (HatSettings.language == HatSettings.RUS)
            {
                this.Text = "Команда запуска";
                button1.Text = "Копировать в буфер";
            }
            else
            {
                this.Text = "The launch command";
                button1.Text = "Copy to Clipboard";
            }
        }
    }
}
