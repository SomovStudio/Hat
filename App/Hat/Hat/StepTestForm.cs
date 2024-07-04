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
            if (HatSettings.language == HatSettings.RUS)
            {
                this.Text = "Подробная информация о шаге";
                label1.Text = "Шаг:";
                label3.Text = "Действие:";
                label4.Text = "Статус:";
                label5.Text = "Комментарий:";
            }
            else
            {
                this.Text = "Detailed information about the step";
                label1.Text = "Step:";
                label3.Text = "Action:";
                label4.Text = "Status:";
                label5.Text = "Comment:";
            }
        }

        private void StepTestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parent.СloseStep();
        }
    }
}
