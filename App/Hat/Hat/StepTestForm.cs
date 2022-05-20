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
    public partial class StepTestForm : Form
    {
        public StepTestForm()
        {
            InitializeComponent();
        }

        public BrowserForm parent;

        private void StepTestForm_Load(object sender, EventArgs e)
        {

        }

        private void StepTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.closeStep();
        }
    }
}
