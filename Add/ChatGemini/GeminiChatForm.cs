using GenerativeAI;
using GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;

/*
 * Google_GenerativeAI
 * NuGet: https://www.nuget.org/packages/Google_GenerativeAI
 * Git: https://github.com/gunpal5/Google_GenerativeAI/wiki/initialization
 * Gemeni: https://gemini.google.com/
 * Get api key: https://aistudio.google.com/app/apikey
 * */

namespace ChatGemini
{
    public partial class GeminiChatForm : Form
    {
        GoogleAi ai;
        GenerativeModel model;
        string fileConfig = "";
        string fileFramework = "";

        public GeminiChatForm()
        {
            InitializeComponent();
        }

        public string systemCurrentLanguage()
        {
            try
            {
                if (CultureInfo.InstalledUICulture.Name == "ru-RU" && CultureInfo.CurrentUICulture.Name == "ru-RU" && CultureInfo.CurrentCulture.Name == "ru-RU")
                {
                    return "Русский";
                }
                else
                {
                    return "English";
                }
            }
            catch (Exception ex)
            {
                sendMessage(ex.Message, "Error");
            }
            return "English";
        }

        private void changeLanguage(string value)
        {
            if (value == "Русский")
            {
                newChatToolStripMenuItem.Text = "Новый чат";
                saveChatToolStripMenuItem.Text = "Сохранить чат";
                settingsToolStripMenuItem.Text = "Настройки";
                languageToolStripMenuItem.Text = "Язык";
                shouldISendTheFrameworkDataWhenYouAskQuestionsToolStripMenuItem.Text = "Отправлять данные HatFramework, когда вы задаете вопрос ?";
                yesToolStripMenuItem.Text = "Да";
                noToolStripMenuItem.Text = "Нет";
                label2.Text = "Чат";
                label6.Text = "Пользователь";
                checkBox2.Text = "Файл:";
                checkBox3.Text = "URL путь:";
                attachBtn.Text = "Прикрепить";
                tabPage1.Text = "Запрос";
                tabPage2.Text = "Настройки";
                label3.Text = "Ключь API:";
                label8.Text = "Сгенерируйте API key по адресу:";
                label4.Text = "Модель:";
                label7.Text = "Язык:";
                checkBox1.Text = "Отправлять данные HatFramework, когда вы задаете вопрос ?";
                displayOnTopOfOtherWindowsToolStripMenuItem.Text = "Отображать поверх других окон";
                checkBox4.Text = "Отображать поверх других окон";
                saveBtn.Text = "Сохранить";
                if (fileHatFramework.Text == "The hatframework file.txt is attached to queries") fileHatFramework.Text = "Файл hatframework.txt прикреплен к запросам";
                if (fileHatFramework.Text == "The file hatframework.txt not found at " + fileFramework) fileHatFramework.Text = "Файл hatframework.txt не найден по адресу " + fileFramework;
            }
            else
            {
                newChatToolStripMenuItem.Text = "New chat";
                saveChatToolStripMenuItem.Text = "Save chat";
                settingsToolStripMenuItem.Text = "Settings";
                languageToolStripMenuItem.Text = "Language";
                shouldISendTheFrameworkDataWhenYouAskQuestionsToolStripMenuItem.Text = "Send HatFramework data when you ask a question ?";
                yesToolStripMenuItem.Text = "Yes";
                noToolStripMenuItem.Text = "No";
                label2.Text = "Chat";
                label6.Text = "User";
                checkBox2.Text = "Local file:";
                checkBox3.Text = "URL file:";
                attachBtn.Text = "Attach";
                tabPage1.Text = "Request";
                tabPage2.Text = "Settings";
                label3.Text = "Api key:";
                label8.Text = "You can generate an API key at:";
                label4.Text = "Model:";
                label7.Text = "Language:";
                checkBox1.Text = "Send HatFramework data when you ask a question ?";
                displayOnTopOfOtherWindowsToolStripMenuItem.Text = "Display on top of other windows";
                checkBox4.Text = "Display on top of other windows";
                saveBtn.Text = "Save";
                if (fileHatFramework.Text == "Файл hatframework.txt прикреплен к запросам") fileHatFramework.Text = "The hatframework file.txt is attached to queries";
                if (fileHatFramework.Text == "Файл hatframework.txt не найден по адресу " + fileFramework) fileHatFramework.Text = "The file hatframework.txt not found at " + fileFramework;
            }
        }

        private void sendMessage(string text, string name)
        {
            if (languageCB.Text == "Русский") 
            {
                if (name == "User") responseRTB.AppendText($"- - [ Пользователь ] - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                if (name == "AI") responseRTB.AppendText($"- - [ Gemini ] - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                if (name == "Error") responseRTB.AppendText($"- - [ Ошибка ] - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            }
            else
            {
                if (name == "User") responseRTB.AppendText($"- - [ User ] - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                if (name == "AI") responseRTB.AppendText($"- - [ Gemini ] - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
                if (name == "Error") responseRTB.AppendText($"- - [ Error ] - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            }

            responseRTB.AppendText(Environment.NewLine);
            responseRTB.AppendText(text);
            responseRTB.AppendText(Environment.NewLine);
            responseRTB.AppendText("- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -");
            responseRTB.AppendText(Environment.NewLine);
            responseRTB.ScrollToCaret();
        }

        private void initGeminiChat()
        {
            try
            {
                if (apiKeyTB.Text == string.Empty)
                {
                    if (languageCB.Text == "Русский") sendMessage("В настройках отсутствует ключ API. Вам нужно сгенерированный ключ API в Google AI Studio и указать в настройках.", "Error");
                    else sendMessage("The API key is missing from the settings. You need the generated API key in Google AI Studio and specify it in the settings.", "Error");
                }
                ai = new GoogleAi(apiKeyTB.Text); // GOOGLE_API_KEY: ключ API, сгенерированный в Google AI Studio. Он необходим для доступа к API Gemini.
                model = ai.CreateGenerativeModel(modelTB.Text); // GoogleAIModels.Gemini2Flash
            }
            catch (Exception ex)
            {
                sendMessage(ex.Message, "Error");
            }
        }

        private async Task<string> requestGemini(string question)
        {
            if (ai == null)
            {
                requestTB.Clear();
                requestTB.Text = "";
                if (languageCB.Text == "Русский") return "Чат не готов к работе.";
                else return "The chat is not ready to work.";
            }
            if (model == null)
            {
                requestTB.Clear();
                requestTB.Text = "";
                if (languageCB.Text == "Русский") return "Модуль не готов к работе.";
                else return "The module is not ready for operation.";
            }

            string responseText = "";

            try
            {
                var request = new GenerateContentRequest();
                request.AddText(question);
                if (checkBox1.Checked) request.AddInlineFile(fileFramework);
                if (checkBox2.Checked) request.AddInlineFile(localfileTB.Text);
                if (checkBox3.Checked) request.AddRemoteFile(urlfileTB.Text, typefileTB.Text);

                requestTB.ReadOnly = true;
                if (languageCB.Text == "Русский") requestTB.Text = "Пожалуйста дождитесь ответа...";
                else requestTB.Text = "Please wait for a response...";
                var response = await model.GenerateContentAsync(request);
                responseText = response.Text();
                requestTB.ReadOnly = false;
                requestTB.Clear();
                requestTB.Text = "";
                return responseText;
            }
            catch (Exception ex)
            {
                responseText = ex.Message;
                sendMessage(ex.Message, "Error");
            }

            if (responseText.Contains("User location is not supported for the API use") == true)
            {
                
                if (languageCB.Text == "Русский") return "Google AI Studio недоступна в вашем регионе. Подробнее о доступных регионах смотрите на странице https://ai.google.dev/gemini-api/docs/available-regions?hl=ru";
                else return "Google AI Studio is not available in your region. For more information about the available regions, see the page https://ai.google.dev/gemini-api/docs/available-regions?hl=en";
            }

            return "Hеудалось выполнить запрос к Gemini";
        }

        private void GeminiChatForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Directory.GetCurrentDirectory() + "\\gemini\\") == true)
                {
                    fileConfig = Directory.GetCurrentDirectory() + "\\gemini\\gemini.cfg";
                    fileFramework = Directory.GetCurrentDirectory() + "\\gemini\\hatframework.txt";
                }
                else
                {
                    fileConfig = Directory.GetCurrentDirectory() + "\\gemini.cfg";
                    fileFramework = Directory.GetCurrentDirectory() + "\\hatframework.txt";
                }
                
                if (!File.Exists(fileConfig))
                {
                    string content = "";
                    content += "apikey=";
                    content += Environment.NewLine;
                    content += "model=models/gemini-2.0-flash";
                    content += Environment.NewLine;
                    content += "language=" + systemCurrentLanguage();
                    content += Environment.NewLine;
                    content += "framework=False";
                    content += Environment.NewLine;
                    content += "topmost=False";
                    StreamWriter writer;
                    writer = new StreamWriter(fileConfig, false, new UTF8Encoding(true));
                    writer.Write(content);
                    writer.Close();

                    apiKeyTB.Text = "";
                    modelTB.Text = "models/gemini-2.0-flash";
                    englishToolStripMenuItem.Checked = true;
                    русскийToolStripMenuItem.Checked = false;
                    languageCB.SelectedIndex = 0;
                    yesToolStripMenuItem.Checked = false;
                    noToolStripMenuItem.Checked = true;
                    checkBox1.Checked = false;
                    displayOnTopOfOtherWindowsToolStripMenuItem.Checked = false;
                    checkBox4.Checked = false;
                    this.TopMost = false;
                }
                else
                {
                    string content = "";
                    List<string[]> rows = File.ReadLines(fileConfig).Select(line => line.Split('\t')).ToList();
                    foreach (string[] row in rows)
                    {
                        content = row[0];
                        if (content.Contains("apikey=") == true)
                        {
                            apiKeyTB.Text = content.Substring("apikey=".Length, content.Length - "apikey=".Length);
                        }
                        if (content.Contains("model=") == true)
                        {
                            modelTB.Text = content.Substring("model=".Length, content.Length - "model=".Length);
                        }
                        if (content.Contains("language=English") == true)
                        {
                            englishToolStripMenuItem.Checked = true;
                            русскийToolStripMenuItem.Checked = false;
                            languageCB.SelectedIndex = 0;
                        }
                        if (content.Contains("language=Русский") == true)
                        {
                            englishToolStripMenuItem.Checked = false;
                            русскийToolStripMenuItem.Checked = true;
                            languageCB.SelectedIndex = 1;
                        }
                        if (content.Contains("framework=False") == true)
                        {
                            yesToolStripMenuItem.Checked = false;
                            noToolStripMenuItem.Checked = true;
                            checkBox1.Checked = false;
                        }
                        if (content.Contains("framework=True") == true)
                        {
                            yesToolStripMenuItem.Checked = true;
                            noToolStripMenuItem.Checked = false;
                            checkBox1.Checked = true;
                        }
                        if (content.Contains("topmost=False") == true)
                        {
                            displayOnTopOfOtherWindowsToolStripMenuItem.Checked = false;
                            checkBox4.Checked = false;
                        }
                        if (content.Contains("topmost=True") == true)
                        {
                            displayOnTopOfOtherWindowsToolStripMenuItem.Checked = true;
                            checkBox4.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sendMessage(ex.Message, "Error");
            }

            addContextMenu(responseRTB);
            initGeminiChat();
        }

        private async void textBoxRequest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                sendMessage(requestTB.Text, "User");
                sendMessage(await requestGemini(requestTB.Text), "AI");
            }
        }

        private void textBoxRequest_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                localfileTB.Text = openFileDialog1.FileName;
            }
        }

        private void newChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            responseRTB.Clear();
            initGeminiChat();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                localfileTB.Enabled = true;
                attachBtn.Enabled = true;
            }
            else
            {
                localfileTB.Enabled = false;
                attachBtn.Enabled = false;
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                urlfileTB.Enabled = true;
                typefileTB.Enabled = true;
            }
            else
            {
                urlfileTB.Enabled = false;
                typefileTB.Enabled = false;
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            englishToolStripMenuItem.Checked = true;
            русскийToolStripMenuItem.Checked = false;
            languageCB.SelectedIndex = 0;
            changeLanguage("English");
        }

        private void русскийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            englishToolStripMenuItem.Checked = false;
            русскийToolStripMenuItem.Checked = true;
            languageCB.SelectedIndex = 1;
            changeLanguage("Русский");
        }

        private void languageCB_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void languageCB_SelectedValueChanged(object sender, EventArgs e)
        {
            if (languageCB.Text == "Русский")
            {
                englishToolStripMenuItem.Checked = false;
                русскийToolStripMenuItem.Checked = true;
            }
            else
            {
                englishToolStripMenuItem.Checked = true;
                русскийToolStripMenuItem.Checked = false;
            }
            changeLanguage(languageCB.Text);
        }

        private void yesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem.Checked = true;
            noToolStripMenuItem.Checked = false;
            checkBox1.Checked = true;
        }

        private void noToolStripMenuItem_Click(object sender, EventArgs e)
        {
            yesToolStripMenuItem.Checked = false;
            noToolStripMenuItem.Checked = true;
            checkBox1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                yesToolStripMenuItem.Checked = true;
                noToolStripMenuItem.Checked = false;
                if (File.Exists(fileFramework))
                {
                    fileHatFramework.Text = fileFramework;
                    if (languageCB.Text == "Русский") fileHatFramework.Text = "Файл hatframework.txt прикреплен к запросам";
                    else fileHatFramework.Text = "The hatframework file.txt is attached to queries";
                }
                else
                {
                    if (languageCB.Text == "Русский") fileHatFramework.Text = "Файл hatframework.txt не найден по адресу " + fileFramework;
                    else fileHatFramework.Text = "The file hatframework.txt not found at " + fileFramework;
                }
            }
            else
            {
                fileHatFramework.Text = "...";
                yesToolStripMenuItem.Checked = false;
                noToolStripMenuItem.Checked = true;
            }
        }

        private void displayOnTopOfOtherWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (displayOnTopOfOtherWindowsToolStripMenuItem.Checked) checkBox4.Checked = false;
            else checkBox4.Checked = true;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                displayOnTopOfOtherWindowsToolStripMenuItem.Checked = true;
                this.TopMost = true;
            }
            else
            {

                displayOnTopOfOtherWindowsToolStripMenuItem.Checked = false;
                this.TopMost = false;
            }
        }

        private void saveChatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                responseRTB.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                if (File.Exists(saveFileDialog1.FileName) == true)
                {
                    if (languageCB.Text == "Русский") MessageBox.Show("Файл успешно сохранён", "Сообщение");
                    else MessageBox.Show("The file was saved successfully", "Message");
                }
                else
                {
                    if (languageCB.Text == "Русский") MessageBox.Show("Не удалось сохранить файл: " + saveFileDialog1.FileName, "Сообщение");
                    else MessageBox.Show("Couldn't save the file: " + saveFileDialog1.FileName, "Message");
                }

            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            string content = "";
            StreamWriter writer;
            writer = new StreamWriter(fileConfig, false, new UTF8Encoding(true));
            content += "apikey=" + apiKeyTB.Text;
            content += Environment.NewLine;
            content += "model=" + modelTB.Text;
            content += Environment.NewLine;
            content += "language=" + languageCB.Text;
            content += Environment.NewLine;
            content += "framework=" + checkBox1.Checked.ToString();
            content += Environment.NewLine;
            content += "topmost=" + checkBox4.Checked.ToString();
            writer.Write(content);
            writer.Close();

            if (languageCB.Text == "Русский") MessageBox.Show("Настройки сохранены", "Сообщение");
            else MessageBox.Show("The settings are saved", "Message");
        }

        private void addContextMenu(RichTextBox rtb)
        {
            try
            {
                string undo = "";
                string redo = "";
                string cut = "";
                string copy = "";
                string paste = "";
                string delete = "";
                string selectall = "";

                if (languageCB.Text == "Русский")
                {
                    undo = "Отменить";
                    redo = "Вернуть";
                    cut = "Вырезать";
                    copy = "Копировать";
                    paste = "Вставить";
                    delete = "Удалить";
                    selectall = "Выделить всё";
                }
                else
                {
                    undo = "Undo";
                    redo = "Redo";
                    cut = "Cut";
                    copy = "Copy";
                    paste = "Paste";
                    delete = "Delete";
                    selectall = "Select All";
                }


                if (rtb.ContextMenuStrip == null)
                {
                    ContextMenuStrip cms = new ContextMenuStrip()
                    {
                        ShowImageMargin = false
                    };

                    ToolStripMenuItem tsmiUndo = new ToolStripMenuItem(undo);
                    tsmiUndo.Click += (sender, e) => rtb.Undo();
                    cms.Items.Add(tsmiUndo);

                    ToolStripMenuItem tsmiRedo = new ToolStripMenuItem(redo);
                    tsmiRedo.Click += (sender, e) => rtb.Redo();
                    cms.Items.Add(tsmiRedo);

                    cms.Items.Add(new ToolStripSeparator());

                    ToolStripMenuItem tsmiCut = new ToolStripMenuItem(cut);
                    tsmiCut.Click += (sender, e) => rtb.Cut();
                    cms.Items.Add(tsmiCut);

                    ToolStripMenuItem tsmiCopy = new ToolStripMenuItem(copy);
                    tsmiCopy.Click += (sender, e) => rtb.Copy();
                    cms.Items.Add(tsmiCopy);

                    ToolStripMenuItem tsmiPaste = new ToolStripMenuItem(paste);
                    tsmiPaste.Click += (sender, e) => rtb.Paste();
                    cms.Items.Add(tsmiPaste);

                    ToolStripMenuItem tsmiDelete = new ToolStripMenuItem(delete);
                    tsmiDelete.Click += (sender, e) => rtb.SelectedText = "";
                    cms.Items.Add(tsmiDelete);

                    cms.Items.Add(new ToolStripSeparator());

                    ToolStripMenuItem tsmiSelectAll = new ToolStripMenuItem(selectall);
                    tsmiSelectAll.Click += (sender, e) => rtb.SelectAll();
                    cms.Items.Add(tsmiSelectAll);

                    cms.Opening += (sender, e) =>
                    {
                        tsmiUndo.Enabled = !rtb.ReadOnly && rtb.CanUndo;
                        tsmiRedo.Enabled = !rtb.ReadOnly && rtb.CanRedo;
                        tsmiCut.Enabled = !rtb.ReadOnly && rtb.SelectionLength > 0;
                        tsmiCopy.Enabled = rtb.SelectionLength > 0;
                        tsmiPaste.Enabled = !rtb.ReadOnly && Clipboard.ContainsText();
                        tsmiDelete.Enabled = !rtb.ReadOnly && rtb.SelectionLength > 0;
                        tsmiSelectAll.Enabled = rtb.TextLength > 0 && rtb.SelectionLength < rtb.TextLength;
                    };

                    rtb.ContextMenuStrip = cms;
                }
            }
            catch (Exception ex)
            {
                sendMessage(ex.Message, "Error");
            }

            
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"https://aistudio.google.com/app/apikey");
            }
            catch (Exception ex)
            {
                sendMessage(ex.Message, "Error");
            }
        }

        private void responseRTB_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                sendMessage(ex.Message, "Error");
            }
        }
    }
}
