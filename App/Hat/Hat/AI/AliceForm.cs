using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hat
{
    public partial class AliceForm : Form
    {
        public AliceForm()
        {
            InitializeComponent();
        }

        private void AliceForm_Load(object sender, EventArgs e)
        {
            try
            {
                webView2.Source = new Uri(@"https://alice.yandex.ru/");
                this.TopMost = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void windowTopMostToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (windowTopMostToolStripMenuItem.Checked)
            {
                this.TopMost = false;
                windowTopMostToolStripMenuItem.Checked = false;
            }
            else
            {
                this.TopMost = true;
                windowTopMostToolStripMenuItem.Checked = true;
            }
        }

        private void siteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                webView2.Source = new Uri(@"https://alice.yandex.ru/");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void hatFrameworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\runtimes");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void webView2_ContentLoading(object sender, Microsoft.Web.WebView2.Core.CoreWebView2ContentLoadingEventArgs e)
        {
            toolStripStatusLabel1.Text = "Подождите, Alice уже загружена...";
        }

        private void webView2_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Alice готова к работе.";
        }
    }
}
