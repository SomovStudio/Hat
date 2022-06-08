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
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("BrowserGetUserAgentAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("BrowserSetUserAgentAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("BrowserGetErrorsAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("BrowserGetNetworkAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Методы для работы с браузером", new System.Windows.Forms.TreeNode[] {
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22,
            treeNode23,
            treeNode24});
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("ConsoleMsg", 2, 2);
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("ConsoleMsgError", 2, 2);
            System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("ClearMessage", 2, 2);
            System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("SendMessage", 2, 2);
            System.Windows.Forms.TreeNode treeNode30 = new System.Windows.Forms.TreeNode("EditMessage", 2, 2);
            System.Windows.Forms.TreeNode treeNode31 = new System.Windows.Forms.TreeNode("Методы для вывода сообщений", new System.Windows.Forms.TreeNode[] {
            treeNode26,
            treeNode27,
            treeNode28,
            treeNode29,
            treeNode30});
            System.Windows.Forms.TreeNode treeNode32 = new System.Windows.Forms.TreeNode("TestBeginAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode33 = new System.Windows.Forms.TreeNode("TestEndAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode34 = new System.Windows.Forms.TreeNode("TestStopAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode35 = new System.Windows.Forms.TreeNode("DefineTestStop", 2, 2);
            System.Windows.Forms.TreeNode treeNode36 = new System.Windows.Forms.TreeNode("Методы для подготовки и завершению тестирования", new System.Windows.Forms.TreeNode[] {
            treeNode32,
            treeNode33,
            treeNode34,
            treeNode35});
            System.Windows.Forms.TreeNode treeNode37 = new System.Windows.Forms.TreeNode("GetAttributeFromElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode38 = new System.Windows.Forms.TreeNode("GetAttributeFromElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode39 = new System.Windows.Forms.TreeNode("GetAttributeFromElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode40 = new System.Windows.Forms.TreeNode("GetAttributeFromElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode41 = new System.Windows.Forms.TreeNode("GetAttributeFromElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode42 = new System.Windows.Forms.TreeNode("GetAttributeFromElementsByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode43 = new System.Windows.Forms.TreeNode("GetAttributeFromElementsByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode44 = new System.Windows.Forms.TreeNode("GetAttributeFromElementsByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode45 = new System.Windows.Forms.TreeNode("GetAttributeFromElementsByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode46 = new System.Windows.Forms.TreeNode("SetAttributeInElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode47 = new System.Windows.Forms.TreeNode("SetAttributeInElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode48 = new System.Windows.Forms.TreeNode("SetAttributeInElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode49 = new System.Windows.Forms.TreeNode("SetAttributeInElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode50 = new System.Windows.Forms.TreeNode("SetAttributeInElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode51 = new System.Windows.Forms.TreeNode("SetAttributeInElementsByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode52 = new System.Windows.Forms.TreeNode("SetAttributeInElementsByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode53 = new System.Windows.Forms.TreeNode("SetAttributeInElementsByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode54 = new System.Windows.Forms.TreeNode("SetAttributeInElementsByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode55 = new System.Windows.Forms.TreeNode("Атрибуты", new System.Windows.Forms.TreeNode[] {
            treeNode37,
            treeNode38,
            treeNode39,
            treeNode40,
            treeNode41,
            treeNode42,
            treeNode43,
            treeNode44,
            treeNode45,
            treeNode46,
            treeNode47,
            treeNode48,
            treeNode49,
            treeNode50,
            treeNode51,
            treeNode52,
            treeNode53,
            treeNode54});
            System.Windows.Forms.TreeNode treeNode56 = new System.Windows.Forms.TreeNode("GetValueFromElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode57 = new System.Windows.Forms.TreeNode("GetValueFromElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode58 = new System.Windows.Forms.TreeNode("GetValueFromElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode59 = new System.Windows.Forms.TreeNode("GetValueFromElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode60 = new System.Windows.Forms.TreeNode("GetValueFromElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode61 = new System.Windows.Forms.TreeNode("SetValueInElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode62 = new System.Windows.Forms.TreeNode("SetValueInElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode63 = new System.Windows.Forms.TreeNode("SetValueInElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode64 = new System.Windows.Forms.TreeNode("SetValueInElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode65 = new System.Windows.Forms.TreeNode("SetValueInElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode66 = new System.Windows.Forms.TreeNode("Значение", new System.Windows.Forms.TreeNode[] {
            treeNode56,
            treeNode57,
            treeNode58,
            treeNode59,
            treeNode60,
            treeNode61,
            treeNode62,
            treeNode63,
            treeNode64,
            treeNode65});
            System.Windows.Forms.TreeNode treeNode67 = new System.Windows.Forms.TreeNode("ClickElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode68 = new System.Windows.Forms.TreeNode("ClickElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode69 = new System.Windows.Forms.TreeNode("ClickElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode70 = new System.Windows.Forms.TreeNode("ClickElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode71 = new System.Windows.Forms.TreeNode("ClickElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode72 = new System.Windows.Forms.TreeNode("ScrollToElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode73 = new System.Windows.Forms.TreeNode("Нажатие", new System.Windows.Forms.TreeNode[] {
            treeNode67,
            treeNode68,
            treeNode69,
            treeNode70,
            treeNode71,
            treeNode72});
            System.Windows.Forms.TreeNode treeNode74 = new System.Windows.Forms.TreeNode("GetHtmlElementAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode75 = new System.Windows.Forms.TreeNode("GetCountElementsByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode76 = new System.Windows.Forms.TreeNode("GetCountElementsByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode77 = new System.Windows.Forms.TreeNode("GetCountElementsByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode78 = new System.Windows.Forms.TreeNode("GetCountElementsByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode79 = new System.Windows.Forms.TreeNode("Объекты", new System.Windows.Forms.TreeNode[] {
            treeNode74,
            treeNode75,
            treeNode76,
            treeNode77,
            treeNode78});
            System.Windows.Forms.TreeNode treeNode80 = new System.Windows.Forms.TreeNode("WaitAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode81 = new System.Windows.Forms.TreeNode("WaitNotVisibleElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode82 = new System.Windows.Forms.TreeNode("WaitNotVisibleElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode83 = new System.Windows.Forms.TreeNode("WaitNotVisibleElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode84 = new System.Windows.Forms.TreeNode("WaitNotVisibleElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode85 = new System.Windows.Forms.TreeNode("WaitNotVisibleElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode86 = new System.Windows.Forms.TreeNode("WaitVisibleElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode87 = new System.Windows.Forms.TreeNode("WaitVisibleElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode88 = new System.Windows.Forms.TreeNode("WaitVisibleElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode89 = new System.Windows.Forms.TreeNode("WaitVisibleElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode90 = new System.Windows.Forms.TreeNode("WaitVisibleElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode91 = new System.Windows.Forms.TreeNode("Ожидание", new System.Windows.Forms.TreeNode[] {
            treeNode80,
            treeNode81,
            treeNode82,
            treeNode83,
            treeNode84,
            treeNode85,
            treeNode86,
            treeNode87,
            treeNode88,
            treeNode89,
            treeNode90});
            System.Windows.Forms.TreeNode treeNode92 = new System.Windows.Forms.TreeNode("FindElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode93 = new System.Windows.Forms.TreeNode("FindElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode94 = new System.Windows.Forms.TreeNode("FindElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode95 = new System.Windows.Forms.TreeNode("FindElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode96 = new System.Windows.Forms.TreeNode("FindElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode97 = new System.Windows.Forms.TreeNode("FindVisibleElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode98 = new System.Windows.Forms.TreeNode("FindVisibleElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode99 = new System.Windows.Forms.TreeNode("FindVisibleElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode100 = new System.Windows.Forms.TreeNode("FindVisibleElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode101 = new System.Windows.Forms.TreeNode("FindVisibleElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode102 = new System.Windows.Forms.TreeNode("Поиск", new System.Windows.Forms.TreeNode[] {
            treeNode92,
            treeNode93,
            treeNode94,
            treeNode95,
            treeNode96,
            treeNode97,
            treeNode98,
            treeNode99,
            treeNode100,
            treeNode101});
            System.Windows.Forms.TreeNode treeNode103 = new System.Windows.Forms.TreeNode("GetTitleAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode104 = new System.Windows.Forms.TreeNode("GetUrlAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode105 = new System.Windows.Forms.TreeNode("GoToUrlAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode106 = new System.Windows.Forms.TreeNode("Страница", new System.Windows.Forms.TreeNode[] {
            treeNode103,
            treeNode104,
            treeNode105});
            System.Windows.Forms.TreeNode treeNode107 = new System.Windows.Forms.TreeNode("GetTextFromElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode108 = new System.Windows.Forms.TreeNode("GetTextFromElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode109 = new System.Windows.Forms.TreeNode("GetTextFromElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode110 = new System.Windows.Forms.TreeNode("GetTextFromElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode111 = new System.Windows.Forms.TreeNode("GetTextFromElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode112 = new System.Windows.Forms.TreeNode("SetTextInElementByClassAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode113 = new System.Windows.Forms.TreeNode("SetTextInElementByCssAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode114 = new System.Windows.Forms.TreeNode("SetTextInElementByIdAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode115 = new System.Windows.Forms.TreeNode("SetTextInElementByNameAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode116 = new System.Windows.Forms.TreeNode("SetTextInElementByTagAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode117 = new System.Windows.Forms.TreeNode("Текст", new System.Windows.Forms.TreeNode[] {
            treeNode107,
            treeNode108,
            treeNode109,
            treeNode110,
            treeNode111,
            treeNode112,
            treeNode113,
            treeNode114,
            treeNode115,
            treeNode116});
            System.Windows.Forms.TreeNode treeNode118 = new System.Windows.Forms.TreeNode("Методы для выполнения действий", new System.Windows.Forms.TreeNode[] {
            treeNode55,
            treeNode66,
            treeNode73,
            treeNode79,
            treeNode91,
            treeNode102,
            treeNode106,
            treeNode117});
            System.Windows.Forms.TreeNode treeNode119 = new System.Windows.Forms.TreeNode("ExecuteJavaScriptAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode120 = new System.Windows.Forms.TreeNode("Методы для выполнения JavaScript", new System.Windows.Forms.TreeNode[] {
            treeNode119});
            System.Windows.Forms.TreeNode treeNode121 = new System.Windows.Forms.TreeNode("AssertEqualsAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode122 = new System.Windows.Forms.TreeNode("AssertNotEqualsAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode123 = new System.Windows.Forms.TreeNode("AssertTrueAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode124 = new System.Windows.Forms.TreeNode("AssertFalseAsync", 2, 2);
            System.Windows.Forms.TreeNode treeNode125 = new System.Windows.Forms.TreeNode("Методы для проверки результата", new System.Windows.Forms.TreeNode[] {
            treeNode121,
            treeNode122,
            treeNode123,
            treeNode124});
            System.Windows.Forms.TreeNode treeNode126 = new System.Windows.Forms.TreeNode("Класс: Tester", new System.Windows.Forms.TreeNode[] {
            treeNode2,
            treeNode14,
            treeNode17,
            treeNode25,
            treeNode31,
            treeNode36,
            treeNode118,
            treeNode120,
            treeNode125});
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
            treeNode21.Text = "BrowserGetUserAgentAsync";
            treeNode22.ImageIndex = 2;
            treeNode22.Name = "Узел0";
            treeNode22.SelectedImageIndex = 2;
            treeNode22.Text = "BrowserSetUserAgentAsync";
            treeNode23.ImageIndex = 2;
            treeNode23.Name = "Узел0";
            treeNode23.SelectedImageIndex = 2;
            treeNode23.Text = "BrowserGetErrorsAsync";
            treeNode24.ImageIndex = 2;
            treeNode24.Name = "Узел1";
            treeNode24.SelectedImageIndex = 2;
            treeNode24.Text = "BrowserGetNetworkAsync";
            treeNode25.Name = "Узел26";
            treeNode25.Text = "Методы для работы с браузером";
            treeNode26.ImageIndex = 2;
            treeNode26.Name = "Узел16";
            treeNode26.SelectedImageIndex = 2;
            treeNode26.Text = "ConsoleMsg";
            treeNode27.ImageIndex = 2;
            treeNode27.Name = "Узел18";
            treeNode27.SelectedImageIndex = 2;
            treeNode27.Text = "ConsoleMsgError";
            treeNode28.ImageIndex = 2;
            treeNode28.Name = "Узел19";
            treeNode28.SelectedImageIndex = 2;
            treeNode28.Text = "ClearMessage";
            treeNode29.ImageIndex = 2;
            treeNode29.Name = "Узел17";
            treeNode29.SelectedImageIndex = 2;
            treeNode29.Text = "SendMessage";
            treeNode30.ImageIndex = 2;
            treeNode30.Name = "Узел2";
            treeNode30.SelectedImageIndex = 2;
            treeNode30.Text = "EditMessage";
            treeNode31.Name = "Узел15";
            treeNode31.Text = "Методы для вывода сообщений";
            treeNode32.ImageIndex = 2;
            treeNode32.Name = "Узел21";
            treeNode32.SelectedImageIndex = 2;
            treeNode32.Text = "TestBeginAsync";
            treeNode33.ImageIndex = 2;
            treeNode33.Name = "Узел22";
            treeNode33.SelectedImageIndex = 2;
            treeNode33.Text = "TestEndAsync";
            treeNode34.ImageIndex = 2;
            treeNode34.Name = "Узел3";
            treeNode34.SelectedImageIndex = 2;
            treeNode34.Text = "TestStopAsync";
            treeNode35.ImageIndex = 2;
            treeNode35.Name = "Узел4";
            treeNode35.SelectedImageIndex = 2;
            treeNode35.Text = "DefineTestStop";
            treeNode36.Name = "Узел20";
            treeNode36.Text = "Методы для подготовки и завершению тестирования";
            treeNode37.ImageIndex = 2;
            treeNode37.Name = "Узел10";
            treeNode37.SelectedImageIndex = 2;
            treeNode37.Text = "GetAttributeFromElementByClassAsync";
            treeNode38.ImageIndex = 2;
            treeNode38.Name = "Узел11";
            treeNode38.SelectedImageIndex = 2;
            treeNode38.Text = "GetAttributeFromElementByCssAsync";
            treeNode39.ImageIndex = 2;
            treeNode39.Name = "Узел12";
            treeNode39.SelectedImageIndex = 2;
            treeNode39.Text = "GetAttributeFromElementByIdAsync";
            treeNode40.ImageIndex = 2;
            treeNode40.Name = "Узел13";
            treeNode40.SelectedImageIndex = 2;
            treeNode40.Text = "GetAttributeFromElementByNameAsync";
            treeNode41.ImageIndex = 2;
            treeNode41.Name = "Узел14";
            treeNode41.SelectedImageIndex = 2;
            treeNode41.Text = "GetAttributeFromElementByTagAsync";
            treeNode42.ImageIndex = 2;
            treeNode42.Name = "Узел2";
            treeNode42.SelectedImageIndex = 2;
            treeNode42.Text = "GetAttributeFromElementsByClassAsync";
            treeNode43.ImageIndex = 2;
            treeNode43.Name = "Узел15";
            treeNode43.SelectedImageIndex = 2;
            treeNode43.Text = "GetAttributeFromElementsByCssAsync";
            treeNode44.ImageIndex = 2;
            treeNode44.Name = "Узел3";
            treeNode44.SelectedImageIndex = 2;
            treeNode44.Text = "GetAttributeFromElementsByNameAsync";
            treeNode45.ImageIndex = 2;
            treeNode45.Name = "Узел4";
            treeNode45.SelectedImageIndex = 2;
            treeNode45.Text = "GetAttributeFromElementsByTagAsync";
            treeNode46.ImageIndex = 2;
            treeNode46.Name = "Узел35";
            treeNode46.SelectedImageIndex = 2;
            treeNode46.Text = "SetAttributeInElementByClassAsync";
            treeNode47.ImageIndex = 2;
            treeNode47.Name = "Узел36";
            treeNode47.SelectedImageIndex = 2;
            treeNode47.Text = "SetAttributeInElementByCssAsync";
            treeNode48.ImageIndex = 2;
            treeNode48.Name = "Узел37";
            treeNode48.SelectedImageIndex = 2;
            treeNode48.Text = "SetAttributeInElementByIdAsync";
            treeNode49.ImageIndex = 2;
            treeNode49.Name = "Узел38";
            treeNode49.SelectedImageIndex = 2;
            treeNode49.Text = "SetAttributeInElementByNameAsync";
            treeNode50.ImageIndex = 2;
            treeNode50.Name = "Узел39";
            treeNode50.SelectedImageIndex = 2;
            treeNode50.Text = "SetAttributeInElementByTagAsync";
            treeNode51.ImageIndex = 2;
            treeNode51.Name = "Узел0";
            treeNode51.SelectedImageIndex = 2;
            treeNode51.Text = "SetAttributeInElementsByClassAsync";
            treeNode52.ImageIndex = 2;
            treeNode52.Name = "Узел1";
            treeNode52.SelectedImageIndex = 2;
            treeNode52.Text = "SetAttributeInElementsByCssAsync";
            treeNode53.ImageIndex = 2;
            treeNode53.Name = "Узел2";
            treeNode53.SelectedImageIndex = 2;
            treeNode53.Text = "SetAttributeInElementsByNameAsync";
            treeNode54.ImageIndex = 2;
            treeNode54.Name = "Узел3";
            treeNode54.SelectedImageIndex = 2;
            treeNode54.Text = "SetAttributeInElementsByTagAsync";
            treeNode55.Name = "Узел2";
            treeNode55.Text = "Атрибуты";
            treeNode56.ImageIndex = 2;
            treeNode56.Name = "Узел28";
            treeNode56.SelectedImageIndex = 2;
            treeNode56.Text = "GetValueFromElementByClassAsync";
            treeNode57.ImageIndex = 2;
            treeNode57.Name = "Узел29";
            treeNode57.SelectedImageIndex = 2;
            treeNode57.Text = "GetValueFromElementByCssAsync";
            treeNode58.ImageIndex = 2;
            treeNode58.Name = "Узел30";
            treeNode58.SelectedImageIndex = 2;
            treeNode58.Text = "GetValueFromElementByIdAsync";
            treeNode59.ImageIndex = 2;
            treeNode59.Name = "Узел31";
            treeNode59.SelectedImageIndex = 2;
            treeNode59.Text = "GetValueFromElementByNameAsync";
            treeNode60.ImageIndex = 2;
            treeNode60.Name = "Узел32";
            treeNode60.SelectedImageIndex = 2;
            treeNode60.Text = "GetValueFromElementByTagAsync";
            treeNode61.ImageIndex = 2;
            treeNode61.Name = "Узел45";
            treeNode61.SelectedImageIndex = 2;
            treeNode61.Text = "SetValueInElementByClassAsync";
            treeNode62.ImageIndex = 2;
            treeNode62.Name = "Узел46";
            treeNode62.SelectedImageIndex = 2;
            treeNode62.Text = "SetValueInElementByCssAsync";
            treeNode63.ImageIndex = 2;
            treeNode63.Name = "Узел47";
            treeNode63.SelectedImageIndex = 2;
            treeNode63.Text = "SetValueInElementByIdAsync";
            treeNode64.ImageIndex = 2;
            treeNode64.Name = "Узел48";
            treeNode64.SelectedImageIndex = 2;
            treeNode64.Text = "SetValueInElementByNameAsync";
            treeNode65.ImageIndex = 2;
            treeNode65.Name = "Узел49";
            treeNode65.SelectedImageIndex = 2;
            treeNode65.Text = "SetValueInElementByTagAsync";
            treeNode66.Name = "Узел6";
            treeNode66.Text = "Значение";
            treeNode67.ImageIndex = 2;
            treeNode67.Name = "Узел28";
            treeNode67.SelectedImageIndex = 2;
            treeNode67.Text = "ClickElementByClassAsync";
            treeNode68.ImageIndex = 2;
            treeNode68.Name = "Узел29";
            treeNode68.SelectedImageIndex = 2;
            treeNode68.Text = "ClickElementByCssAsync";
            treeNode69.ImageIndex = 2;
            treeNode69.Name = "Узел30";
            treeNode69.SelectedImageIndex = 2;
            treeNode69.Text = "ClickElementByIdAsync";
            treeNode70.ImageIndex = 2;
            treeNode70.Name = "Узел31";
            treeNode70.SelectedImageIndex = 2;
            treeNode70.Text = "ClickElementByNameAsync";
            treeNode71.ImageIndex = 2;
            treeNode71.Name = "Узел32";
            treeNode71.SelectedImageIndex = 2;
            treeNode71.Text = "ClickElementByTagAsync";
            treeNode72.ImageIndex = 2;
            treeNode72.Name = "Узел34";
            treeNode72.SelectedImageIndex = 2;
            treeNode72.Text = "ScrollToElementByCssAsync";
            treeNode73.Name = "Узел0";
            treeNode73.Text = "Нажатие";
            treeNode74.ImageIndex = 2;
            treeNode74.Name = "Узел20";
            treeNode74.SelectedImageIndex = 2;
            treeNode74.Text = "GetHtmlElementAsync";
            treeNode75.ImageIndex = 2;
            treeNode75.Name = "Узел16";
            treeNode75.SelectedImageIndex = 2;
            treeNode75.Text = "GetCountElementsByClassAsync";
            treeNode76.ImageIndex = 2;
            treeNode76.Name = "Узел17";
            treeNode76.SelectedImageIndex = 2;
            treeNode76.Text = "GetCountElementsByCssAsync";
            treeNode77.ImageIndex = 2;
            treeNode77.Name = "Узел18";
            treeNode77.SelectedImageIndex = 2;
            treeNode77.Text = "GetCountElementsByNameAsync";
            treeNode78.ImageIndex = 2;
            treeNode78.Name = "Узел19";
            treeNode78.SelectedImageIndex = 2;
            treeNode78.Text = "GetCountElementsByTagAsync";
            treeNode79.Name = "Узел3";
            treeNode79.Text = "Объекты";
            treeNode80.ImageIndex = 2;
            treeNode80.Name = "Узел50";
            treeNode80.SelectedImageIndex = 2;
            treeNode80.Text = "WaitAsync";
            treeNode81.ImageIndex = 2;
            treeNode81.Name = "Узел51";
            treeNode81.SelectedImageIndex = 2;
            treeNode81.Text = "WaitNotVisibleElementByClassAsync";
            treeNode82.ImageIndex = 2;
            treeNode82.Name = "Узел52";
            treeNode82.SelectedImageIndex = 2;
            treeNode82.Text = "WaitNotVisibleElementByCssAsync";
            treeNode83.ImageIndex = 2;
            treeNode83.Name = "Узел53";
            treeNode83.SelectedImageIndex = 2;
            treeNode83.Text = "WaitNotVisibleElementByIdAsync";
            treeNode84.ImageIndex = 2;
            treeNode84.Name = "Узел54";
            treeNode84.SelectedImageIndex = 2;
            treeNode84.Text = "WaitNotVisibleElementByNameAsync";
            treeNode85.ImageIndex = 2;
            treeNode85.Name = "Узел55";
            treeNode85.SelectedImageIndex = 2;
            treeNode85.Text = "WaitNotVisibleElementByTagAsync";
            treeNode86.ImageIndex = 2;
            treeNode86.Name = "Узел56";
            treeNode86.SelectedImageIndex = 2;
            treeNode86.Text = "WaitVisibleElementByClassAsync";
            treeNode87.ImageIndex = 2;
            treeNode87.Name = "Узел57";
            treeNode87.SelectedImageIndex = 2;
            treeNode87.Text = "WaitVisibleElementByCssAsync";
            treeNode88.ImageIndex = 2;
            treeNode88.Name = "Узел58";
            treeNode88.SelectedImageIndex = 2;
            treeNode88.Text = "WaitVisibleElementByIdAsync";
            treeNode89.ImageIndex = 2;
            treeNode89.Name = "Узел59";
            treeNode89.SelectedImageIndex = 2;
            treeNode89.Text = "WaitVisibleElementByNameAsync";
            treeNode90.ImageIndex = 2;
            treeNode90.Name = "Узел60";
            treeNode90.SelectedImageIndex = 2;
            treeNode90.Text = "WaitVisibleElementByTagAsync";
            treeNode91.Name = "Узел7";
            treeNode91.Text = "Ожидание";
            treeNode92.ImageIndex = 2;
            treeNode92.Name = "Узел0";
            treeNode92.SelectedImageIndex = 2;
            treeNode92.Text = "FindElementByClassAsync";
            treeNode93.ImageIndex = 2;
            treeNode93.Name = "Узел1";
            treeNode93.SelectedImageIndex = 2;
            treeNode93.Text = "FindElementByCssAsync";
            treeNode94.ImageIndex = 2;
            treeNode94.Name = "Узел2";
            treeNode94.SelectedImageIndex = 2;
            treeNode94.Text = "FindElementByIdAsync";
            treeNode95.ImageIndex = 2;
            treeNode95.Name = "Узел3";
            treeNode95.SelectedImageIndex = 2;
            treeNode95.Text = "FindElementByNameAsync";
            treeNode96.ImageIndex = 2;
            treeNode96.Name = "Узел4";
            treeNode96.SelectedImageIndex = 2;
            treeNode96.Text = "FindElementByTagAsync";
            treeNode97.ImageIndex = 2;
            treeNode97.Name = "Узел5";
            treeNode97.SelectedImageIndex = 2;
            treeNode97.Text = "FindVisibleElementByClassAsync";
            treeNode98.ImageIndex = 2;
            treeNode98.Name = "Узел6";
            treeNode98.SelectedImageIndex = 2;
            treeNode98.Text = "FindVisibleElementByCssAsync";
            treeNode99.ImageIndex = 2;
            treeNode99.Name = "Узел7";
            treeNode99.SelectedImageIndex = 2;
            treeNode99.Text = "FindVisibleElementByIdAsync";
            treeNode100.ImageIndex = 2;
            treeNode100.Name = "Узел8";
            treeNode100.SelectedImageIndex = 2;
            treeNode100.Text = "FindVisibleElementByNameAsync";
            treeNode101.ImageIndex = 2;
            treeNode101.Name = "Узел9";
            treeNode101.SelectedImageIndex = 2;
            treeNode101.Text = "FindVisibleElementByTagAsync";
            treeNode102.Name = "Узел1";
            treeNode102.Text = "Поиск";
            treeNode103.ImageIndex = 2;
            treeNode103.Name = "Узел26";
            treeNode103.SelectedImageIndex = 2;
            treeNode103.Text = "GetTitleAsync";
            treeNode104.ImageIndex = 2;
            treeNode104.Name = "Узел27";
            treeNode104.SelectedImageIndex = 2;
            treeNode104.Text = "GetUrlAsync";
            treeNode105.ImageIndex = 2;
            treeNode105.Name = "Узел33";
            treeNode105.SelectedImageIndex = 2;
            treeNode105.Text = "GoToUrlAsync";
            treeNode106.Name = "Узел5";
            treeNode106.Text = "Страница";
            treeNode107.ImageIndex = 2;
            treeNode107.Name = "Узел21";
            treeNode107.SelectedImageIndex = 2;
            treeNode107.Text = "GetTextFromElementByClassAsync";
            treeNode108.ImageIndex = 2;
            treeNode108.Name = "Узел22";
            treeNode108.SelectedImageIndex = 2;
            treeNode108.Text = "GetTextFromElementByCssAsync";
            treeNode109.ImageIndex = 2;
            treeNode109.Name = "Узел23";
            treeNode109.SelectedImageIndex = 2;
            treeNode109.Text = "GetTextFromElementByIdAsync";
            treeNode110.ImageIndex = 2;
            treeNode110.Name = "Узел24";
            treeNode110.SelectedImageIndex = 2;
            treeNode110.Text = "GetTextFromElementByNameAsync";
            treeNode111.ImageIndex = 2;
            treeNode111.Name = "Узел25";
            treeNode111.SelectedImageIndex = 2;
            treeNode111.Text = "GetTextFromElementByTagAsync";
            treeNode112.ImageIndex = 2;
            treeNode112.Name = "Узел40";
            treeNode112.SelectedImageIndex = 2;
            treeNode112.Text = "SetTextInElementByClassAsync";
            treeNode113.ImageIndex = 2;
            treeNode113.Name = "Узел41";
            treeNode113.SelectedImageIndex = 2;
            treeNode113.Text = "SetTextInElementByCssAsync";
            treeNode114.ImageIndex = 2;
            treeNode114.Name = "Узел42";
            treeNode114.SelectedImageIndex = 2;
            treeNode114.Text = "SetTextInElementByIdAsync";
            treeNode115.ImageIndex = 2;
            treeNode115.Name = "Узел43";
            treeNode115.SelectedImageIndex = 2;
            treeNode115.Text = "SetTextInElementByNameAsync";
            treeNode116.ImageIndex = 2;
            treeNode116.Name = "Узел44";
            treeNode116.SelectedImageIndex = 2;
            treeNode116.Text = "SetTextInElementByTagAsync";
            treeNode117.Name = "Узел4";
            treeNode117.Text = "Текст";
            treeNode118.Name = "Узел27";
            treeNode118.Text = "Методы для выполнения действий";
            treeNode119.ImageIndex = 2;
            treeNode119.Name = "Узел6";
            treeNode119.SelectedImageIndex = 2;
            treeNode119.Text = "ExecuteJavaScriptAsync";
            treeNode120.Name = "Узел5";
            treeNode120.Text = "Методы для выполнения JavaScript";
            treeNode121.ImageIndex = 2;
            treeNode121.Name = "Узел38";
            treeNode121.SelectedImageIndex = 2;
            treeNode121.Text = "AssertEqualsAsync";
            treeNode122.ImageIndex = 2;
            treeNode122.Name = "Узел39";
            treeNode122.SelectedImageIndex = 2;
            treeNode122.Text = "AssertNotEqualsAsync";
            treeNode123.ImageIndex = 2;
            treeNode123.Name = "Узел40";
            treeNode123.SelectedImageIndex = 2;
            treeNode123.Text = "AssertTrueAsync";
            treeNode124.ImageIndex = 2;
            treeNode124.Name = "Узел41";
            treeNode124.SelectedImageIndex = 2;
            treeNode124.Text = "AssertFalseAsync";
            treeNode125.Name = "Узел37";
            treeNode125.Text = "Методы для проверки результата";
            treeNode126.Name = "Узел0";
            treeNode126.Text = "Класс: Tester";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode126});
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