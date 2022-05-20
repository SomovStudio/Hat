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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_PROCESS", 12, 17);
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_PASSED", 12, 17);
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_FAILED", 12, 17);
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_MESSAGE", 12, 17);
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("IMAGE_STATUS_WARNING", 12, 17);
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("PASSED", 12, 17);
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("FAILED", 12, 17);
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("STOPPED", 12, 17);
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("PROCESS", 12, 17);
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Константы", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("browserForm", 12, 17);
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("browserView", 12, 17);
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("browserContext", 12, 17);
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Переменные", new System.Windows.Forms.TreeNode[] {
            treeNode11,
            treeNode12,
            treeNode13});
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("browserClose", 6, 11);
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("browserSize", 6, 11);
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("browserSizeFullScreen", 6, 11);
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Методы для работы с браузером", new System.Windows.Forms.TreeNode[] {
            treeNode15,
            treeNode16,
            treeNode17});
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("consoleMsg", 6, 11);
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("sendMessage", 6, 11);
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("editMessage", 6, 11);
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("clearMessage", 6, 11);
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Методы для вывода сообщений", new System.Windows.Forms.TreeNode[] {
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22});
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("testBegin", 6, 11);
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("testEnd", 6, 11);
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Методы для подготовки и завершению тестирования", new System.Windows.Forms.TreeNode[] {
            treeNode24,
            treeNode25});
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("goToUrl", 6, 11);
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("getURL", 6, 11);
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("getHtmlElement", 6, 11);
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("clickHtmlElement", 6, 11);
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("setValueFromInput", 6, 11);
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("getValueFromInput", 6, 11);
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("getValueFromTextarea", 6, 11);
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("getTextFromHtmlElement", 6, 11);
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("wait", 6, 11);
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Методы выполнения действий", new System.Windows.Forms.TreeNode[] {
            treeNode27,
            treeNode28,
            treeNode29,
            treeNode30,
            treeNode31,
            treeNode32,
            treeNode33,
            treeNode34,
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("assertEquals", 6, 11);
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("assertNotEquals", 6, 11);
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("assertTrue", 6, 11);
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("assertFalse", 6, 11);
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("Методы для проверки результата", new System.Windows.Forms.TreeNode[] {
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode40});
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("Класс: Tester", new System.Windows.Forms.TreeNode[] {
            treeNode10,
            treeNode14,
            treeNode18,
            treeNode23,
            treeNode26,
            treeNode36,
            treeNode41});
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
            treeNode1.ImageIndex = 12;
            treeNode1.Name = "Узел2";
            treeNode1.SelectedImageIndex = 17;
            treeNode1.Text = "IMAGE_STATUS_PROCESS";
            treeNode2.ImageIndex = 12;
            treeNode2.Name = "Узел3";
            treeNode2.SelectedImageIndex = 17;
            treeNode2.Text = "IMAGE_STATUS_PASSED";
            treeNode3.ImageIndex = 12;
            treeNode3.Name = "Узел4";
            treeNode3.SelectedImageIndex = 17;
            treeNode3.Text = "IMAGE_STATUS_FAILED";
            treeNode4.ImageIndex = 12;
            treeNode4.Name = "Узел5";
            treeNode4.SelectedImageIndex = 17;
            treeNode4.Text = "IMAGE_STATUS_MESSAGE";
            treeNode5.ImageIndex = 12;
            treeNode5.Name = "Узел6";
            treeNode5.SelectedImageIndex = 17;
            treeNode5.Text = "IMAGE_STATUS_WARNING";
            treeNode6.ImageIndex = 12;
            treeNode6.Name = "Узел7";
            treeNode6.SelectedImageIndex = 17;
            treeNode6.Text = "PASSED";
            treeNode7.ImageIndex = 12;
            treeNode7.Name = "Узел8";
            treeNode7.SelectedImageIndex = 17;
            treeNode7.Text = "FAILED";
            treeNode8.ImageIndex = 12;
            treeNode8.Name = "Узел9";
            treeNode8.SelectedImageIndex = 17;
            treeNode8.Text = "STOPPED";
            treeNode9.ImageIndex = 12;
            treeNode9.Name = "Узел10";
            treeNode9.SelectedImageIndex = 17;
            treeNode9.Text = "PROCESS";
            treeNode10.Name = "Узел1";
            treeNode10.Text = "Константы";
            treeNode11.ImageIndex = 12;
            treeNode11.Name = "Узел12";
            treeNode11.SelectedImageIndex = 17;
            treeNode11.Text = "browserForm";
            treeNode12.ImageIndex = 12;
            treeNode12.Name = "Узел13";
            treeNode12.SelectedImageIndex = 17;
            treeNode12.Text = "browserView";
            treeNode13.ImageIndex = 12;
            treeNode13.Name = "Узел14";
            treeNode13.SelectedImageIndex = 17;
            treeNode13.Text = "browserContext";
            treeNode14.Name = "Узел11";
            treeNode14.Text = "Переменные";
            treeNode15.ImageIndex = 6;
            treeNode15.Name = "Узел23";
            treeNode15.SelectedImageIndex = 11;
            treeNode15.Text = "browserClose";
            treeNode16.ImageIndex = 6;
            treeNode16.Name = "Узел24";
            treeNode16.SelectedImageIndex = 11;
            treeNode16.Text = "browserSize";
            treeNode17.ImageIndex = 6;
            treeNode17.Name = "Узел25";
            treeNode17.SelectedImageIndex = 11;
            treeNode17.Text = "browserSizeFullScreen";
            treeNode18.Name = "Узел26";
            treeNode18.Text = "Методы для работы с браузером";
            treeNode19.ImageIndex = 6;
            treeNode19.Name = "Узел16";
            treeNode19.SelectedImageIndex = 11;
            treeNode19.Text = "consoleMsg";
            treeNode20.ImageIndex = 6;
            treeNode20.Name = "Узел18";
            treeNode20.SelectedImageIndex = 11;
            treeNode20.Text = "sendMessage";
            treeNode21.ImageIndex = 6;
            treeNode21.Name = "Узел19";
            treeNode21.SelectedImageIndex = 11;
            treeNode21.Text = "editMessage";
            treeNode22.ImageIndex = 6;
            treeNode22.Name = "Узел17";
            treeNode22.SelectedImageIndex = 11;
            treeNode22.Text = "clearMessage";
            treeNode23.Name = "Узел15";
            treeNode23.Text = "Методы для вывода сообщений";
            treeNode24.ImageIndex = 6;
            treeNode24.Name = "Узел21";
            treeNode24.SelectedImageIndex = 11;
            treeNode24.Text = "testBegin";
            treeNode25.ImageIndex = 6;
            treeNode25.Name = "Узел22";
            treeNode25.SelectedImageIndex = 11;
            treeNode25.Text = "testEnd";
            treeNode26.Name = "Узел20";
            treeNode26.Text = "Методы для подготовки и завершению тестирования";
            treeNode27.ImageIndex = 6;
            treeNode27.Name = "Узел28";
            treeNode27.SelectedImageIndex = 11;
            treeNode27.Text = "goToUrl";
            treeNode28.ImageIndex = 6;
            treeNode28.Name = "Узел29";
            treeNode28.SelectedImageIndex = 11;
            treeNode28.Text = "getURL";
            treeNode29.ImageIndex = 6;
            treeNode29.Name = "Узел30";
            treeNode29.SelectedImageIndex = 11;
            treeNode29.Text = "getHtmlElement";
            treeNode30.ImageIndex = 6;
            treeNode30.Name = "Узел31";
            treeNode30.SelectedImageIndex = 11;
            treeNode30.Text = "clickHtmlElement";
            treeNode31.ImageIndex = 6;
            treeNode31.Name = "Узел32";
            treeNode31.SelectedImageIndex = 11;
            treeNode31.Text = "setValueFromInput";
            treeNode32.ImageIndex = 6;
            treeNode32.Name = "Узел33";
            treeNode32.SelectedImageIndex = 11;
            treeNode32.Text = "getValueFromInput";
            treeNode33.ImageIndex = 6;
            treeNode33.Name = "Узел34";
            treeNode33.SelectedImageIndex = 11;
            treeNode33.Text = "getValueFromTextarea";
            treeNode34.ImageIndex = 6;
            treeNode34.Name = "Узел35";
            treeNode34.SelectedImageIndex = 11;
            treeNode34.Text = "getTextFromHtmlElement";
            treeNode35.ImageIndex = 6;
            treeNode35.Name = "Узел36";
            treeNode35.SelectedImageIndex = 11;
            treeNode35.Text = "wait";
            treeNode36.Name = "Узел27";
            treeNode36.Text = "Методы выполнения действий";
            treeNode37.ImageIndex = 6;
            treeNode37.Name = "Узел38";
            treeNode37.SelectedImageIndex = 11;
            treeNode37.Text = "assertEquals";
            treeNode38.ImageIndex = 6;
            treeNode38.Name = "Узел39";
            treeNode38.SelectedImageIndex = 11;
            treeNode38.Text = "assertNotEquals";
            treeNode39.ImageIndex = 6;
            treeNode39.Name = "Узел40";
            treeNode39.SelectedImageIndex = 11;
            treeNode39.Text = "assertTrue";
            treeNode40.ImageIndex = 6;
            treeNode40.Name = "Узел41";
            treeNode40.SelectedImageIndex = 11;
            treeNode40.Text = "assertFalse";
            treeNode41.Name = "Узел37";
            treeNode41.Text = "Методы для проверки результата";
            treeNode42.Name = "Узел0";
            treeNode42.Text = "Класс: Tester";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode42});
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