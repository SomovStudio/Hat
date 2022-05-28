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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Tester", 2, 2);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Конструктор", new System.Windows.Forms.TreeNode[] {
            treeNode1});
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_PROCESS", 4, 4);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_PASSED", 4, 4);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_FAILED", 4, 4);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_MESSAGE", 4, 4);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_WARNING", 4, 4);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("PASSED", 4, 4);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("FAILED", 4, 4);
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("STOPPED", 4, 4);
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("PROCESS", 4, 4);
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("COMPLETED", 4, 4);
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("WARNING", 4, 4);
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Константы", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("BrowserView", 3, 3);
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("BrowserWindow", 3, 3);
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Переменные", new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode16});
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("BrowserCloseAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("BrowserSizeAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("BrowserFullScreenAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("BrowserGetUserAgent", 2, 2);
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("BrowserSetUserAgent", 2, 2);
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Методы для работы с браузером", new System.Windows.Forms.TreeNode[] {
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22});
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("ConsoleMsg", 2, 2);
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("ConsoleMsgError", 2, 2);
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("ClearMessage", 2, 2);
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("SendMessage", 2, 2);
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("EditMessage", 2, 2);
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Методы для вывода сообщений", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27,
            treeNode28});
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("TestBeginAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("TestEndAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("TestStopAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("_CheckTestStop", 2, 2);
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("Методы для подготовки и завершению тестирования", new System.Windows.Forms.TreeNode[] {
            treeNode30,
            treeNode31,
            treeNode32,
            treeNode33});
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("GetHtmlElementAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("GoToUrlAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("GetUrlAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("WaitAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("...", 6, 11);
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("Методы выполнения действий", new System.Windows.Forms.TreeNode[] {
            treeNode35,
            treeNode36,
            treeNode37,
            treeNode38,
            treeNode39});
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("ExecuteJavaScriptAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Методы выполнения JavaScript", new System.Windows.Forms.TreeNode[] {
            treeNode41});
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("AssertEqualsAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("AssertNotEqualsAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("AssertTrueAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("AssertFalseAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("Методы для проверки результата", new System.Windows.Forms.TreeNode[] {
            treeNode43,
            treeNode44,
            treeNode45,
            treeNode46});
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("Класс: Tester", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode14,
            treeNode17,
            treeNode23,
            treeNode29,
            treeNode34,
            treeNode40,
            treeNode42,
            treeNode47});
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.textEditorControl1 = new ICSharpCode.TextEditor.TextEditorControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.вставитьВКодToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel5.SuspendLayout();
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
            this.toolStripStatusLabel6,
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
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            this.toolStripStatusLabel6.Size = new System.Drawing.Size(16, 17);
            this.toolStripStatusLabel6.Text = "...";
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
            this.textEditorControl1.Size = new System.Drawing.Size(561, 414);
            this.textEditorControl1.TabIndex = 3;
            this.textEditorControl1.TextChanged += new System.EventHandler(this.textEditorControl1_TextChanged);
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
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Panel2.Controls.Add(this.panel5);
            this.splitContainer1.Size = new System.Drawing.Size(784, 414);
            this.splitContainer1.SplitterDistance = 561;
            this.splitContainer1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.splitContainer2);
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(216, 393);
            this.panel1.TabIndex = 5;
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
            this.splitContainer2.Panel2.BackColor = System.Drawing.Color.White;
            this.splitContainer2.Panel2.Controls.Add(this.richTextBox1);
            this.splitContainer2.Size = new System.Drawing.Size(214, 391);
            this.splitContainer2.SplitterDistance = 176;
            this.splitContainer2.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList2;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode1.ImageIndex = 2;
            treeNode1.Name = "Узел3";
            treeNode1.SelectedImageIndex = 2;
            treeNode1.Text = "Tester";
            treeNode2.Name = "Узел2";
            treeNode2.Text = "Конструктор";
            treeNode3.ImageIndex = 4;
            treeNode3.Name = "Узел2";
            treeNode3.SelectedImageIndex = 4;
            treeNode3.Text = "IMAGE_STATUS_PROCESS";
            treeNode4.ImageIndex = 4;
            treeNode4.Name = "Узел3";
            treeNode4.SelectedImageIndex = 4;
            treeNode4.Text = "IMAGE_STATUS_PASSED";
            treeNode5.ImageIndex = 4;
            treeNode5.Name = "Узел4";
            treeNode5.SelectedImageIndex = 4;
            treeNode5.Text = "IMAGE_STATUS_FAILED";
            treeNode6.ImageIndex = 4;
            treeNode6.Name = "Узел5";
            treeNode6.SelectedImageIndex = 4;
            treeNode6.Text = "IMAGE_STATUS_MESSAGE";
            treeNode7.ImageIndex = 4;
            treeNode7.Name = "Узел6";
            treeNode7.SelectedImageIndex = 4;
            treeNode7.Text = "IMAGE_STATUS_WARNING";
            treeNode8.ImageIndex = 4;
            treeNode8.Name = "Узел7";
            treeNode8.SelectedImageIndex = 4;
            treeNode8.Text = "PASSED";
            treeNode9.ImageIndex = 4;
            treeNode9.Name = "Узел8";
            treeNode9.SelectedImageIndex = 4;
            treeNode9.Text = "FAILED";
            treeNode10.ImageIndex = 4;
            treeNode10.Name = "Узел9";
            treeNode10.SelectedImageIndex = 4;
            treeNode10.Text = "STOPPED";
            treeNode11.ImageIndex = 4;
            treeNode11.Name = "Узел10";
            treeNode11.SelectedImageIndex = 4;
            treeNode11.Text = "PROCESS";
            treeNode12.ImageIndex = 4;
            treeNode12.Name = "Узел0";
            treeNode12.SelectedImageIndex = 4;
            treeNode12.Text = "COMPLETED";
            treeNode13.ImageIndex = 4;
            treeNode13.Name = "Узел1";
            treeNode13.SelectedImageIndex = 4;
            treeNode13.Text = "WARNING";
            treeNode14.Name = "Узел1";
            treeNode14.Text = "Константы";
            treeNode15.ImageIndex = 3;
            treeNode15.Name = "Узел13";
            treeNode15.SelectedImageIndex = 3;
            treeNode15.Text = "BrowserView";
            treeNode16.ImageIndex = 3;
            treeNode16.Name = "Узел12";
            treeNode16.SelectedImageIndex = 3;
            treeNode16.Text = "BrowserWindow";
            treeNode17.Name = "Узел11";
            treeNode17.Text = "Переменные";
            treeNode18.ImageIndex = 2;
            treeNode18.Name = "Узел23";
            treeNode18.SelectedImageIndex = 2;
            treeNode18.Text = "BrowserCloseAsync";
            treeNode19.ImageIndex = 2;
            treeNode19.Name = "Узел24";
            treeNode19.SelectedImageIndex = 2;
            treeNode19.Text = "BrowserSizeAsync";
            treeNode20.ImageIndex = 2;
            treeNode20.Name = "Узел25";
            treeNode20.SelectedImageIndex = 2;
            treeNode20.Text = "BrowserFullScreenAsync";
            treeNode21.ImageIndex = 2;
            treeNode21.Name = "Узел1";
            treeNode21.SelectedImageIndex = 2;
            treeNode21.Text = "BrowserGetUserAgent";
            treeNode22.ImageIndex = 2;
            treeNode22.Name = "Узел0";
            treeNode22.SelectedImageIndex = 2;
            treeNode22.Text = "BrowserSetUserAgent";
            treeNode23.Name = "Узел26";
            treeNode23.Text = "Методы для работы с браузером";
            treeNode24.ImageIndex = 2;
            treeNode24.Name = "Узел16";
            treeNode24.SelectedImageIndex = 2;
            treeNode24.Text = "ConsoleMsg";
            treeNode25.ImageIndex = 2;
            treeNode25.Name = "Узел18";
            treeNode25.SelectedImageIndex = 2;
            treeNode25.Text = "ConsoleMsgError";
            treeNode26.ImageIndex = 2;
            treeNode26.Name = "Узел19";
            treeNode26.SelectedImageIndex = 2;
            treeNode26.Text = "ClearMessage";
            treeNode27.ImageIndex = 2;
            treeNode27.Name = "Узел17";
            treeNode27.SelectedImageIndex = 2;
            treeNode27.Text = "SendMessage";
            treeNode28.ImageIndex = 2;
            treeNode28.Name = "Узел2";
            treeNode28.SelectedImageIndex = 2;
            treeNode28.Text = "EditMessage";
            treeNode29.Name = "Узел15";
            treeNode29.Text = "Методы для вывода сообщений";
            treeNode30.ImageIndex = 2;
            treeNode30.Name = "Узел21";
            treeNode30.SelectedImageIndex = 2;
            treeNode30.Text = "TestBeginAsync";
            treeNode31.ImageIndex = 2;
            treeNode31.Name = "Узел22";
            treeNode31.SelectedImageIndex = 2;
            treeNode31.Text = "TestEndAsync";
            treeNode32.ImageIndex = 2;
            treeNode32.Name = "Узел3";
            treeNode32.SelectedImageIndex = 2;
            treeNode32.Text = "TestStopAsync";
            treeNode33.ImageIndex = 2;
            treeNode33.Name = "Узел4";
            treeNode33.SelectedImageIndex = 2;
            treeNode33.Text = "_CheckTestStop";
            treeNode34.Name = "Узел20";
            treeNode34.Text = "Методы для подготовки и завершению тестирования";
            treeNode35.ImageIndex = 2;
            treeNode35.Name = "Узел28";
            treeNode35.SelectedImageIndex = 2;
            treeNode35.Text = "GetHtmlElementAsync";
            treeNode36.ImageIndex = 2;
            treeNode36.Name = "Узел29";
            treeNode36.SelectedImageIndex = 2;
            treeNode36.Text = "GoToUrlAsync";
            treeNode37.ImageIndex = 2;
            treeNode37.Name = "Узел30";
            treeNode37.SelectedImageIndex = 2;
            treeNode37.Text = "GetUrlAsync";
            treeNode38.ImageIndex = 2;
            treeNode38.Name = "Узел31";
            treeNode38.SelectedImageIndex = 2;
            treeNode38.Text = "WaitAsync";
            treeNode39.ImageIndex = 6;
            treeNode39.Name = "Узел32";
            treeNode39.SelectedImageIndex = 11;
            treeNode39.Text = "...";
            treeNode40.Name = "Узел27";
            treeNode40.Text = "Методы выполнения действий";
            treeNode41.ImageIndex = 2;
            treeNode41.Name = "Узел6";
            treeNode41.SelectedImageIndex = 2;
            treeNode41.Text = "ExecuteJavaScriptAsync";
            treeNode42.Name = "Узел5";
            treeNode42.Text = "Методы выполнения JavaScript";
            treeNode43.ImageIndex = 2;
            treeNode43.Name = "Узел38";
            treeNode43.SelectedImageIndex = 2;
            treeNode43.Text = "AssertEqualsAsync";
            treeNode44.ImageIndex = 2;
            treeNode44.Name = "Узел39";
            treeNode44.SelectedImageIndex = 2;
            treeNode44.Text = "AssertNotEqualsAsync";
            treeNode45.ImageIndex = 2;
            treeNode45.Name = "Узел40";
            treeNode45.SelectedImageIndex = 2;
            treeNode45.Text = "AssertTrueAsync";
            treeNode46.ImageIndex = 2;
            treeNode46.Name = "Узел41";
            treeNode46.SelectedImageIndex = 2;
            treeNode46.Text = "AssertFalseAsync";
            treeNode47.Name = "Узел37";
            treeNode47.Text = "Методы для проверки результата";
            treeNode48.Name = "Узел0";
            treeNode48.Text = "Класс: Tester";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode48});
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(214, 176);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.вставитьВКодToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 26);
            // 
            // вставитьВКодToolStripMenuItem
            // 
            this.вставитьВКодToolStripMenuItem.Name = "вставитьВКодToolStripMenuItem";
            this.вставитьВКодToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.вставитьВКодToolStripMenuItem.Text = "Вставить в код";
            this.вставитьВКодToolStripMenuItem.Click += new System.EventHandler(this.вставитьВКодToolStripMenuItem_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "folder_black.png");
            this.imageList2.Images.SetKeyName(1, "page_white.png");
            this.imageList2.Images.SetKeyName(2, "page_white_text.png");
            this.imageList2.Images.SetKeyName(3, "page_gear.png");
            this.imageList2.Images.SetKeyName(4, "page_white_gear.png");
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox1.BackColor = System.Drawing.Color.White;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(5, 5);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(209, 207);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // panel5
            // 
            this.panel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel5.Controls.Add(this.label3);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(216, 21);
            this.panel5.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(0, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(217, 2);
            this.label3.TabIndex = 2;
            this.label3.Text = "label7";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Справочник";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditorForm_FormClosing);
            this.Load += new System.EventHandler(this.EditorForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem вставитьВКодToolStripMenuItem;
    }
}