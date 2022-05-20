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
    public partial class InputBoxForm : Form
    {
        public InputBoxForm()
        {
            InitializeComponent();
        }

        private bool ok = false;

        private void InputBoxForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ok = true;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ok = false;
            textBox.Text = "";
            Close();
        }

        private void InputBoxForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ok == false) textBox.Text = "";
        }
    }
}
