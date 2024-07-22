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
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                //System.Diagnostics.Process.Start(@"https://opensource.org/licenses/MIT");
                System.Diagnostics.Process.Start(@"https://github.com/SomovStudio/Hat/blob/main/LICENSE");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"mailto:somov.studio@gmail.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"https://somovstudio.github.io/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"https://zionec.ru/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            if (HatSettings.language == HatSettings.RUS)
            {
                label6.Text = $"версия: {Config.currentBrowserVersion}";
                label7.Text = $"дата последнего обновления: {Config.dateBrowserUpdate}";
                this.Text = "О программе Hat | Copyright © 2024 Somov Studio. All Rights Reserved.";
                label3.Text = "Программа: Hat";
                label4.Text = "Разработчик: Сомов Евгений Павлович";
                label8.Text = "Почта:";
                label5.Text = "Лицензия:";
                label9.Text = "Website:";
                label10.Text = "Программа разработана при поддержке компании \"Зионек\"";
                label12.Text = "Компания \"Зионек\" занимается разработкой \"Интернет - магазинов\", \"Корпоративных порталов\", \"CRM - систем\" и собственных решений.";
            }
            else
            {
                label6.Text = $"version: {Config.currentBrowserVersion}";
                label7.Text = $"date of last update: {Config.dateBrowserUpdate}";
                this.Text = "About Hat | Copyright © 2024 Somov Studio. All Rights Reserved.";
                label3.Text = "Program: Hat";
                label4.Text = "Developer: Evgeniy Somov";
                label8.Text = "Email:";
                label5.Text = "License:";
                label9.Text = "Website:";
                label10.Text = "The program was developed with the support of the \"Zionec\"";
                label12.Text = "The \"Zionec\" company develops \"Online stores\", \"Corporate portals\", \"CRM systems\" and its own solutions.";
            }
        }
    }
}
