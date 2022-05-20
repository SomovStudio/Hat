namespace Hat
{
    partial class EditorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditorForm));
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_PROCESS", 12, 17);
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_PASSED", 12, 17);
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_FAILED", 12, 17);
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_MESSAGE", 12, 17);
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_WARNING", 12, 17);
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("PASSED", 12, 17);
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("FAILED", 12, 17);
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("STOPPED", 12, 17);
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("PROCESS", 12, 17);
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("Константы", new System.Windows.Forms.TreeNode[] {
            treeNode43,
            treeNode44,
            treeNode45,
            treeNode46,
            treeNode47,
            treeNode48,
            treeNode49,
            treeNode50,
            treeNode51});
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("browserForm", 12, 17);
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("browserView", 12, 17);
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("browserContext", 12, 17);
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("Переменные", new System.Windows.Forms.TreeNode[] {
            treeNode53,
            treeNode54,
            treeNode55});
            System.Windows.Forms.TreeNode treeNode57 = new System.Windows.Forms.TreeNode("browserClose", 6, 11);
            System.Windows.Forms.TreeNode treeNode58 = new System.Windows.Forms.TreeNode("browserSize", 6, 11);
            System.Windows.Forms.TreeNode treeNode59 = new System.Windows.Forms.TreeNode("browserSizeFullScreen", 6, 11);
            System.Windows.Forms.TreeNode treeNode60 = new System.Windows.Forms.TreeNode("Методы для работы с браузером", new System.Windows.Forms.TreeNode[] {
            treeNode57,
            treeNode58,
            treeNode59});
            System.Windows.Forms.TreeNode treeNode61 = new System.Windows.Forms.TreeNode("consoleMsg", 6, 11);
            System.Windows.Forms.TreeNode treeNode62 = new System.Windows.Forms.TreeNode("sendMessage", 6, 11);
            System.Windows.Forms.TreeNode treeNode63 = new System.Windows.Forms.TreeNode("editMessage", 6, 11);
            System.Windows.Forms.TreeNode treeNode64 = new System.Windows.Forms.TreeNode("clearMessage", 6, 11);
            System.Windows.Forms.TreeNode treeNode65 = new System.Windows.Forms.TreeNode("Методы для вывода сообщений", new System.Windows.Forms.TreeNode[] {
            treeNode61,
            treeNode62,
            treeNode63,
            treeNode64});
            System.Windows.Forms.TreeNode treeNode66 = new System.Windows.Forms.TreeNode("testBegin", 6, 11);
            System.Windows.Forms.TreeNode treeNode67 = new System.Windows.Forms.TreeNode("testEnd", 6, 11);
            System.Windows.Forms.TreeNode treeNode68 = new System.Windows.Forms.TreeNode("Методы для подготовки и завершению тестирования", new System.Windows.Forms.TreeNode[] {
            treeNode66,
            treeNode67});
            System.Windows.Forms.TreeNode treeNode69 = new System.Windows.Forms.TreeNode("goToUrl", 6, 11);
            System.Windows.Forms.TreeNode treeNode70 = new System.Windows.Forms.TreeNode("getURL", 6, 11);
            System.Windows.Forms.TreeNode treeNode71 = new System.Windows.Forms.TreeNode("getHtmlElement", 6, 11);
            System.Windows.Forms.TreeNode treeNode72 = new System.Windows.Forms.TreeNode("clickHtmlElement", 6, 11);
            System.Windows.Forms.TreeNode treeNode73 = new System.Windows.Forms.TreeNode("setValueFromInput", 6, 11);
            System.Windows.Forms.TreeNode treeNode74 = new System.Windows.Forms.TreeNode("getValueFromInput", 6, 11);
            System.Windows.Forms.TreeNode treeNode75 = new System.Windows.Forms.TreeNode("getValueFromTextarea", 6, 11);
            System.Windows.Forms.TreeNode treeNode76 = new System.Windows.Forms.TreeNode("getTextFromHtmlElement", 6, 11);
            System.Windows.Forms.TreeNode treeNode77 = new System.Windows.Forms.TreeNode("wait", 6, 11);
            System.Windows.Forms.TreeNode treeNode78 = new System.Windows.Forms.TreeNode("Методы выполнения действий", new System.Windows.Forms.TreeNode[] {
            treeNode69,
            treeNode70,
            treeNode71,
            treeNode72,
            treeNode73,
            treeNode74,
            treeNode75,
            treeNode76,
            treeNode77});
            System.Windows.Forms.TreeNode treeNode79 = new System.Windows.Forms.TreeNode("assertEquals", 6, 11);
            System.Windows.Forms.TreeNode treeNode80 = new System.Windows.Forms.TreeNode("assertNotEquals", 6, 11);
            System.Windows.Forms.TreeNode treeNode81 = new System.Windows.Forms.TreeNode("assertTrue", 6, 11);
            System.Windows.Forms.TreeNode treeNode82 = new System.Windows.Forms.TreeNode("assertFalse", 6, 11);
            System.Windows.Forms.TreeNode treeNode83 = new System.Windows.Forms.TreeNode("Методы для проверки результата", new System.Windows.Forms.TreeNode[] {
            treeNode79,
            treeNode80,
            treeNode81,
            treeNode82});
            System.Windows.Forms.TreeNode treeNode84 = new System.Windows.Forms.TreeNode("Класс: Tester", new System.Windows.Forms.TreeNode[] {
            treeNode52,
            treeNode56,
            treeNode60,
            treeNode65,
            treeNode68,
            treeNode78,
            treeNode83});
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton3,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Сохранить";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Сохранить как...";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Запустить тест";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "Остановить тест";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5});
            this.statusStrip1.Location = new System.Drawing.Point(0, 439);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(69, 17);
            this.toolStripStatusLabel1.Text = "Кодировка:";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel2.Text = "...";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel3.Text = "|";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusLabel4.Text = "Файл:";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel5.Text = "...";
            // 
            // textEditorControl1
            // 
            this.textEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textEditorControl1.IsReadOnly = false;
            this.textEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.textEditorControl1.Name = "textEditorControl1";
            this.textEditorControl1.Size = new System.Drawing.Size(520, 414);
            this.textEditorControl1.TabIndex = 3;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.textEditorControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(784, 414);
            this.splitContainer1.SplitterDistance = 520;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(260, 414);
            this.splitContainer2.SplitterDistance = 187;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList1;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode43.ImageIndex = 12;
            treeNode43.Name = "Узел2";
            treeNode43.SelectedImageIndex = 17;
            treeNode43.Text = "IMAGE_STATUS_PROCESS";
            treeNode44.ImageIndex = 12;
            treeNode44.Name = "Узел3";
            treeNode44.SelectedImageIndex = 17;
            treeNode44.Text = "IMAGE_STATUS_PASSED";
            treeNode45.ImageIndex = 12;
            treeNode45.Name = "Узел4";
            treeNode45.SelectedImageIndex = 17;
            treeNode45.Text = "IMAGE_STATUS_FAILED";
            treeNode46.ImageIndex = 12;
            treeNode46.Name = "Узел5";
            treeNode46.SelectedImageIndex = 17;
            treeNode46.Text = "IMAGE_STATUS_MESSAGE";
            treeNode47.ImageIndex = 12;
            treeNode47.Name = "Узел6";
            treeNode47.SelectedImageIndex = 17;
            treeNode47.Text = "IMAGE_STATUS_WARNING";
            treeNode48.ImageIndex = 12;
            treeNode48.Name = "Узел7";
            treeNode48.SelectedImageIndex = 17;
            treeNode48.Text = "PASSED";
            treeNode49.ImageIndex = 12;
            treeNode49.Name = "Узел8";
            treeNode49.SelectedImageIndex = 17;
            treeNode49.Text = "FAILED";
            treeNode50.ImageIndex = 12;
            treeNode50.Name = "Узел9";
            treeNode50.SelectedImageIndex = 17;
            treeNode50.Text = "STOPPED";
            treeNode51.ImageIndex = 12;
            treeNode51.Name = "Узел10";
            treeNode51.SelectedImageIndex = 17;
            treeNode51.Text = "PROCESS";
            treeNode52.Name = "Узел1";
            treeNode52.Text = "Константы";
            treeNode53.ImageIndex = 12;
            treeNode53.Name = "Узел12";
            treeNode53.SelectedImageIndex = 17;
            treeNode53.Text = "browserForm";
            treeNode54.ImageIndex = 12;
            treeNode54.Name = "Узел13";
            treeNode54.SelectedImageIndex = 17;
            treeNode54.Text = "browserView";
            treeNode55.ImageIndex = 12;
            treeNode55.Name = "Узел14";
            treeNode55.SelectedImageIndex = 17;
            treeNode55.Text = "browserContext";
            treeNode56.Name = "Узел11";
            treeNode56.Text = "Переменные";
            treeNode57.ImageIndex = 6;
            treeNode57.Name = "Узел23";
            treeNode57.SelectedImageIndex = 11;
            treeNode57.Text = "browserClose";
            treeNode58.ImageIndex = 6;
            treeNode58.Name = "Узел24";
            treeNode58.SelectedImageIndex = 11;
            treeNode58.Text = "browserSize";
            treeNode59.ImageIndex = 6;
            treeNode59.Name = "Узел25";
            treeNode59.SelectedImageIndex = 11;
            treeNode59.Text = "browserSizeFullScreen";
            treeNode60.Name = "Узел26";
            treeNode60.Text = "Методы для работы с браузером";
            treeNode61.ImageIndex = 6;
            treeNode61.Name = "Узел16";
            treeNode61.SelectedImageIndex = 11;
            treeNode61.Text = "consoleMsg";
            treeNode62.ImageIndex = 6;
            treeNode62.Name = "Узел18";
            treeNode62.SelectedImageIndex = 11;
            treeNode62.Text = "sendMessage";
            treeNode63.ImageIndex = 6;
            treeNode63.Name = "Узел19";
            treeNode63.SelectedImageIndex = 11;
            treeNode63.Text = "editMessage";
            treeNode64.ImageIndex = 6;
            treeNode64.Name = "Узел17";
            treeNode64.SelectedImageIndex = 11;
            treeNode64.Text = "clearMessage";
            treeNode65.Name = "Узел15";
            treeNode65.Text = "Методы для вывода сообщений";
            treeNode66.ImageIndex = 6;
            treeNode66.Name = "Узел21";
            treeNode66.SelectedImageIndex = 11;
            treeNode66.Text = "testBegin";
            treeNode67.ImageIndex = 6;
            treeNode67.Name = "Узел22";
            treeNode67.SelectedImageIndex = 11;
            treeNode67.Text = "testEnd";
            treeNode68.Name = "Узел20";
            treeNode68.Text = "Методы для подготовки и завершению тестирования";
            treeNode69.ImageIndex = 6;
            treeNode69.Name = "Узел28";
            treeNode69.SelectedImageIndex = 11;
            treeNode69.Text = "goToUrl";
            treeNode70.ImageIndex = 6;
            treeNode70.Name = "Узел29";
            treeNode70.SelectedImageIndex = 11;
            treeNode70.Text = "getURL";
            treeNode71.ImageIndex = 6;
            treeNode71.Name = "Узел30";
            treeNode71.SelectedImageIndex = 11;
            treeNode71.Text = "getHtmlElement";
            treeNode72.ImageIndex = 6;
            treeNode72.Name = "Узел31";
            treeNode72.SelectedImageIndex = 11;
            treeNode72.Text = "clickHtmlElement";
            treeNode73.ImageIndex = 6;
            treeNode73.Name = "Узел32";
            treeNode73.SelectedImageIndex = 11;
            treeNode73.Text = "setValueFromInput";
            treeNode74.ImageIndex = 6;
            treeNode74.Name = "Узел33";
            treeNode74.SelectedImageIndex = 11;
            treeNode74.Text = "getValueFromInput";
            treeNode75.ImageIndex = 6;
            treeNode75.Name = "Узел34";
            treeNode75.SelectedImageIndex = 11;
            treeNode75.Text = "getValueFromTextarea";
            treeNode76.ImageIndex = 6;
            treeNode76.Name = "Узел35";
            treeNode76.SelectedImageIndex = 11;
            treeNode76.Text = "getTextFromHtmlElement";
            treeNode77.ImageIndex = 6;
            treeNode77.Name = "Узел36";
            treeNode77.SelectedImageIndex = 11;
            treeNode77.Text = "wait";
            treeNode78.Name = "Узел27";
            treeNode78.Text = "Методы выполнения действий";
            treeNode79.ImageIndex = 6;
            treeNode79.Name = "Узел38";
            treeNode79.SelectedImageIndex = 11;
            treeNode79.Text = "assertEquals";
            treeNode80.ImageIndex = 6;
            treeNode80.Name = "Узел39";
            treeNode80.SelectedImageIndex = 11;
            treeNode80.Text = "assertNotEquals";
            treeNode81.ImageIndex = 6;
            treeNode81.Name = "Узел40";
            treeNode81.SelectedImageIndex = 11;
            treeNode81.Text = "assertTrue";
            treeNode82.ImageIndex = 6;
            treeNode82.Name = "Узел41";
            treeNode82.SelectedImageIndex = 11;
            treeNode82.Text = "assertFalse";
            treeNode83.Name = "Узел37";
            treeNode83.Text = "Методы для проверки результата";
            treeNode84.Name = "Узел0";
            treeNode84.Text = "Класс: Tester";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode84});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(260, 187);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "folder.png");
            this.imageList1.Images.SetKeyName(1, "folder_add.png");
            this.imageList1.Images.SetKeyName(2, "folder_delete.png");
            this.imageList1.Images.SetKeyName(3, "folder_edit.png");
            this.imageList1.Images.SetKeyName(4, "folder_page.png");
            this.imageList1.Images.SetKeyName(5, "folder_page_white.png");
            this.imageList1.Images.SetKeyName(6, "page_white_horizontal.png");
            this.imageList1.Images.SetKeyName(7, "page_white_csharp.png");
            this.imageList1.Images.SetKeyName(8, "page_white_add.png");
            this.imageList1.Images.SetKeyName(9, "page_white_delete.png");
            this.imageList1.Images.SetKeyName(10, "page_white_edit.png");
            this.imageList1.Images.SetKeyName(11, "page_white_gear.png");
            this.imageList1.Images.SetKeyName(12, "page.png");
            this.imageList1.Images.SetKeyName(13, "page_code.png");
            this.imageList1.Images.SetKeyName(14, "page_add.png");
            this.imageList1.Images.SetKeyName(15, "page_delete.png");
            this.imageList1.Images.SetKeyName(16, "page_edit.png");
            this.imageList1.Images.SetKeyName(17, "page_gear.png");
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(260, 223);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "*.cs|*.cs|*.*|*.*";
            // 
            // EditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditorForm";
            this.Text = "Редактор кода";
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private ICSharpCode.TextEditor.TextEditorControl textEditorControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ImageList imageList1;
    }
}