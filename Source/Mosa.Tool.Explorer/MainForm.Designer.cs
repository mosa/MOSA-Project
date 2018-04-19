namespace Mosa.Tool.Explorer
{
	partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label stageLabel;
            System.Windows.Forms.Label label1;
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.snippetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableSSA = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableOptimizations = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableSparseConditionalConstantPropagation = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableInlinedMethods = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableTwoPassOptimizations = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableIRLongExpansion = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableBinaryCodeGeneration = new System.Windows.Forms.ToolStripMenuItem();
            this.displayOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showOperandTypes = new System.Windows.Forms.ToolStripMenuItem();
            this.padInstructions = new System.Windows.Forms.ToolStripMenuItem();
            this.showSizes = new System.Windows.Forms.ToolStripMenuItem();
            this.advanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpAllMethodStagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cbPlatform = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tbExceptions = new System.Windows.Forms.TabPage();
            this.rbException = new System.Windows.Forms.RichTextBox();
            this.tbErrors = new System.Windows.Forms.TabPage();
            this.rbErrors = new System.Windows.Forms.RichTextBox();
            this.tbLogs = new System.Windows.Forms.TabPage();
            this.rbLog = new System.Windows.Forms.RichTextBox();
            this.tbGlobalCounters = new System.Windows.Forms.TabPage();
            this.rbGlobalCounters = new System.Windows.Forms.RichTextBox();
            this.tbMethodCounters = new System.Windows.Forms.TabPage();
            this.rbMethodCounters = new System.Windows.Forms.RichTextBox();
            this.tbDebug = new System.Windows.Forms.TabPage();
            this.rbDebugResult = new System.Windows.Forms.RichTextBox();
            this.cbDebugStages = new System.Windows.Forms.ComboBox();
            this.tbStages = new System.Windows.Forms.TabPage();
            this.tbResult = new System.Windows.Forms.RichTextBox();
            this.cbStages = new System.Windows.Forms.ComboBox();
            this.cbLabels = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            label3 = new System.Windows.Forms.Label();
            stageLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tbExceptions.SuspendLayout();
            this.tbErrors.SuspendLayout();
            this.tbLogs.SuspendLayout();
            this.tbGlobalCounters.SuspendLayout();
            this.tbMethodCounters.SuspendLayout();
            this.tbDebug.SuspendLayout();
            this.tbStages.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 479);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(891, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(200, 16);
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.displayOptionsToolStripMenuItem,
            this.advanceToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(891, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.quitToolStripMenuItem.Text = "&Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.QuitToolStripMenuItem_Click);
            // 
            // compileToolStripMenuItem
            // 
            this.compileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nowToolStripMenuItem,
            this.snippetToolStripMenuItem});
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.compileToolStripMenuItem.Text = "Compile";
            // 
            // nowToolStripMenuItem
            // 
            this.nowToolStripMenuItem.Name = "nowToolStripMenuItem";
            this.nowToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.nowToolStripMenuItem.Text = "Now";
            this.nowToolStripMenuItem.Click += new System.EventHandler(this.NowToolStripMenuItem_Click);
            // 
            // snippetToolStripMenuItem
            // 
            this.snippetToolStripMenuItem.Name = "snippetToolStripMenuItem";
            this.snippetToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.snippetToolStripMenuItem.Text = "Snippet";
            this.snippetToolStripMenuItem.Click += new System.EventHandler(this.SnippetToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbEnableSSA,
            this.cbEnableOptimizations,
            this.cbEnableSparseConditionalConstantPropagation,
            this.cbEnableInlinedMethods,
            this.cbEnableTwoPassOptimizations,
            this.cbEnableIRLongExpansion,
            this.cbEnableBinaryCodeGeneration});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.optionsToolStripMenuItem.Text = "Optimizations";
            // 
            // cbEnableSSA
            // 
            this.cbEnableSSA.Checked = true;
            this.cbEnableSSA.CheckOnClick = true;
            this.cbEnableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableSSA.Name = "cbEnableSSA";
            this.cbEnableSSA.Size = new System.Drawing.Size(293, 22);
            this.cbEnableSSA.Text = "Enable SSA";
            // 
            // cbEnableOptimizations
            // 
            this.cbEnableOptimizations.Checked = true;
            this.cbEnableOptimizations.CheckOnClick = true;
            this.cbEnableOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableOptimizations.Name = "cbEnableOptimizations";
            this.cbEnableOptimizations.Size = new System.Drawing.Size(293, 22);
            this.cbEnableOptimizations.Text = "Enable Optimizations";
            // 
            // cbEnableSparseConditionalConstantPropagation
            // 
            this.cbEnableSparseConditionalConstantPropagation.Checked = true;
            this.cbEnableSparseConditionalConstantPropagation.CheckOnClick = true;
            this.cbEnableSparseConditionalConstantPropagation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableSparseConditionalConstantPropagation.Name = "cbEnableSparseConditionalConstantPropagation";
            this.cbEnableSparseConditionalConstantPropagation.Size = new System.Drawing.Size(293, 22);
            this.cbEnableSparseConditionalConstantPropagation.Text = "Enable Conditional Constant Propagation";
            // 
            // cbEnableInlinedMethods
            // 
            this.cbEnableInlinedMethods.Checked = true;
            this.cbEnableInlinedMethods.CheckOnClick = true;
            this.cbEnableInlinedMethods.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableInlinedMethods.Name = "cbEnableInlinedMethods";
            this.cbEnableInlinedMethods.Size = new System.Drawing.Size(293, 22);
            this.cbEnableInlinedMethods.Text = "Enable Inlined Methods";
            // 
            // cbEnableTwoPassOptimizations
            // 
            this.cbEnableTwoPassOptimizations.Checked = true;
            this.cbEnableTwoPassOptimizations.CheckOnClick = true;
            this.cbEnableTwoPassOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableTwoPassOptimizations.Name = "cbEnableTwoPassOptimizations";
            this.cbEnableTwoPassOptimizations.Size = new System.Drawing.Size(293, 22);
            this.cbEnableTwoPassOptimizations.Text = "Enable Two Pass Optimizations";
            // 
            // cbEnableIRLongExpansion
            // 
            this.cbEnableIRLongExpansion.Checked = true;
            this.cbEnableIRLongExpansion.CheckOnClick = true;
            this.cbEnableIRLongExpansion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableIRLongExpansion.Name = "cbEnableIRLongExpansion";
            this.cbEnableIRLongExpansion.Size = new System.Drawing.Size(293, 22);
            this.cbEnableIRLongExpansion.Text = "Enable IR Long Expansion";
            // 
            // cbEnableBinaryCodeGeneration
            // 
            this.cbEnableBinaryCodeGeneration.Checked = true;
            this.cbEnableBinaryCodeGeneration.CheckOnClick = true;
            this.cbEnableBinaryCodeGeneration.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableBinaryCodeGeneration.Name = "cbEnableBinaryCodeGeneration";
            this.cbEnableBinaryCodeGeneration.Size = new System.Drawing.Size(293, 22);
            this.cbEnableBinaryCodeGeneration.Text = "Enable Binary Code Generation";
            // 
            // displayOptionsToolStripMenuItem
            // 
            this.displayOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOperandTypes,
            this.padInstructions,
            this.showSizes});
            this.displayOptionsToolStripMenuItem.Name = "displayOptionsToolStripMenuItem";
            this.displayOptionsToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.displayOptionsToolStripMenuItem.Text = "Display";
            // 
            // showOperandTypes
            // 
            this.showOperandTypes.CheckOnClick = true;
            this.showOperandTypes.Name = "showOperandTypes";
            this.showOperandTypes.Size = new System.Drawing.Size(185, 22);
            this.showOperandTypes.Text = "Show Operand Types";
            this.showOperandTypes.CheckStateChanged += new System.EventHandler(this.showOperandTypes_CheckStateChanged);
            // 
            // padInstructions
            // 
            this.padInstructions.Checked = true;
            this.padInstructions.CheckOnClick = true;
            this.padInstructions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.padInstructions.Name = "padInstructions";
            this.padInstructions.Size = new System.Drawing.Size(185, 22);
            this.padInstructions.Text = "Pad Instructions";
            this.padInstructions.CheckStateChanged += new System.EventHandler(this.padInstructions_CheckStateChanged);
            // 
            // showSizes
            // 
            this.showSizes.Checked = true;
            this.showSizes.CheckOnClick = true;
            this.showSizes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSizes.Name = "showSizes";
            this.showSizes.Size = new System.Drawing.Size(185, 22);
            this.showSizes.Text = "Show Sizes";
            this.showSizes.Click += new System.EventHandler(this.showSizesToolStripMenuItem_Click);
            // 
            // advanceToolStripMenuItem
            // 
            this.advanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpAllMethodStagesToolStripMenuItem});
            this.advanceToolStripMenuItem.Name = "advanceToolStripMenuItem";
            this.advanceToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.advanceToolStripMenuItem.Text = "Advance";
            // 
            // dumpAllMethodStagesToolStripMenuItem
            // 
            this.dumpAllMethodStagesToolStripMenuItem.Name = "dumpAllMethodStagesToolStripMenuItem";
            this.dumpAllMethodStagesToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.dumpAllMethodStagesToolStripMenuItem.Text = "Dump All Method Stages";
            this.dumpAllMethodStagesToolStripMenuItem.Click += new System.EventHandler(this.DumpAllMethodStagesToolStripMenuItem_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.DefaultExt = "exe";
            this.openFileDialog.Filter = "Executable|*.exe|Library|*.dll|All Files|*.*";
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(239, 427);
            this.treeView.TabIndex = 3;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(0, 52);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(887, 424);
            this.splitContainer1.SplitterDistance = 236;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 26;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton4,
            this.toolStripSeparator2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(891, 25);
            this.toolStrip1.TabIndex = 27;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(56, 22);
            this.toolStripButton1.Text = "Open";
            this.toolStripButton1.Click += new System.EventHandler(this.ToolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton2.Text = "Snippet";
            this.toolStripButton2.Click += new System.EventHandler(this.ToolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(72, 22);
            this.toolStripButton4.Text = "Compile";
            this.toolStripButton4.Click += new System.EventHandler(this.ToolStripButton4_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cbPlatform
            // 
            this.cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlatform.FormattingEnabled = true;
            this.cbPlatform.Items.AddRange(new object[] {
            "x86",
            "ARMv6"});
            this.cbPlatform.Location = new System.Drawing.Point(221, 27);
            this.cbPlatform.Name = "cbPlatform";
            this.cbPlatform.Size = new System.Drawing.Size(78, 21);
            this.cbPlatform.TabIndex = 28;
            this.cbPlatform.SelectedIndexChanged += new System.EventHandler(this.CbPlatform_SelectedIndexChanged);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // tbExceptions
            // 
            this.tbExceptions.Controls.Add(this.rbException);
            this.tbExceptions.Location = new System.Drawing.Point(4, 25);
            this.tbExceptions.Name = "tbExceptions";
            this.tbExceptions.Padding = new System.Windows.Forms.Padding(3);
            this.tbExceptions.Size = new System.Drawing.Size(637, 395);
            this.tbExceptions.TabIndex = 5;
            this.tbExceptions.Text = "Exceptions";
            this.tbExceptions.UseVisualStyleBackColor = true;
            // 
            // rbException
            // 
            this.rbException.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbException.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbException.Location = new System.Drawing.Point(0, 0);
            this.rbException.Name = "rbException";
            this.rbException.Size = new System.Drawing.Size(633, 391);
            this.rbException.TabIndex = 2;
            this.rbException.Text = "";
            this.rbException.WordWrap = false;
            // 
            // tbErrors
            // 
            this.tbErrors.BackColor = System.Drawing.Color.Gainsboro;
            this.tbErrors.Controls.Add(this.rbErrors);
            this.tbErrors.Location = new System.Drawing.Point(4, 25);
            this.tbErrors.Name = "tbErrors";
            this.tbErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tbErrors.Size = new System.Drawing.Size(637, 395);
            this.tbErrors.TabIndex = 2;
            this.tbErrors.Text = "Errors";
            // 
            // rbErrors
            // 
            this.rbErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbErrors.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbErrors.Location = new System.Drawing.Point(0, 0);
            this.rbErrors.Name = "rbErrors";
            this.rbErrors.Size = new System.Drawing.Size(633, 391);
            this.rbErrors.TabIndex = 0;
            this.rbErrors.Text = "";
            this.rbErrors.WordWrap = false;
            // 
            // tbLogs
            // 
            this.tbLogs.BackColor = System.Drawing.Color.Gainsboro;
            this.tbLogs.Controls.Add(this.rbLog);
            this.tbLogs.Location = new System.Drawing.Point(4, 25);
            this.tbLogs.Name = "tbLogs";
            this.tbLogs.Padding = new System.Windows.Forms.Padding(3);
            this.tbLogs.Size = new System.Drawing.Size(637, 395);
            this.tbLogs.TabIndex = 3;
            this.tbLogs.Text = "Log";
            // 
            // rbLog
            // 
            this.rbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbLog.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbLog.Location = new System.Drawing.Point(0, 0);
            this.rbLog.Name = "rbLog";
            this.rbLog.Size = new System.Drawing.Size(633, 391);
            this.rbLog.TabIndex = 1;
            this.rbLog.Text = "";
            this.rbLog.WordWrap = false;
            // 
            // tbGlobalCounters
            // 
            this.tbGlobalCounters.BackColor = System.Drawing.Color.Gainsboro;
            this.tbGlobalCounters.Controls.Add(this.rbGlobalCounters);
            this.tbGlobalCounters.Location = new System.Drawing.Point(4, 25);
            this.tbGlobalCounters.Name = "tbGlobalCounters";
            this.tbGlobalCounters.Padding = new System.Windows.Forms.Padding(3);
            this.tbGlobalCounters.Size = new System.Drawing.Size(637, 395);
            this.tbGlobalCounters.TabIndex = 4;
            this.tbGlobalCounters.Text = "Global Counters";
            // 
            // rbGlobalCounters
            // 
            this.rbGlobalCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbGlobalCounters.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbGlobalCounters.Location = new System.Drawing.Point(0, 0);
            this.rbGlobalCounters.Name = "rbGlobalCounters";
            this.rbGlobalCounters.Size = new System.Drawing.Size(633, 391);
            this.rbGlobalCounters.TabIndex = 1;
            this.rbGlobalCounters.Text = "";
            this.rbGlobalCounters.WordWrap = false;
            // 
            // tbMethodCounters
            // 
            this.tbMethodCounters.Controls.Add(this.rbMethodCounters);
            this.tbMethodCounters.Location = new System.Drawing.Point(4, 25);
            this.tbMethodCounters.Name = "tbMethodCounters";
            this.tbMethodCounters.Size = new System.Drawing.Size(637, 395);
            this.tbMethodCounters.TabIndex = 6;
            this.tbMethodCounters.Text = "Counters";
            this.tbMethodCounters.UseVisualStyleBackColor = true;
            // 
            // rbMethodCounters
            // 
            this.rbMethodCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbMethodCounters.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbMethodCounters.Location = new System.Drawing.Point(1, 1);
            this.rbMethodCounters.Name = "rbMethodCounters";
            this.rbMethodCounters.Size = new System.Drawing.Size(633, 391);
            this.rbMethodCounters.TabIndex = 2;
            this.rbMethodCounters.Text = "";
            this.rbMethodCounters.WordWrap = false;
            // 
            // tbDebug
            // 
            this.tbDebug.BackColor = System.Drawing.Color.Gainsboro;
            this.tbDebug.Controls.Add(this.cbDebugStages);
            this.tbDebug.Controls.Add(label3);
            this.tbDebug.Controls.Add(this.rbDebugResult);
            this.tbDebug.Location = new System.Drawing.Point(4, 25);
            this.tbDebug.Margin = new System.Windows.Forms.Padding(0);
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.Size = new System.Drawing.Size(637, 395);
            this.tbDebug.TabIndex = 1;
            this.tbDebug.Text = "Debug";
            // 
            // rbDebugResult
            // 
            this.rbDebugResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbDebugResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDebugResult.Location = new System.Drawing.Point(0, 32);
            this.rbDebugResult.Name = "rbDebugResult";
            this.rbDebugResult.Size = new System.Drawing.Size(633, 360);
            this.rbDebugResult.TabIndex = 32;
            this.rbDebugResult.Text = "";
            this.rbDebugResult.WordWrap = false;
            // 
            // label3
            // 
            label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.Location = new System.Drawing.Point(4, 8);
            label3.Margin = new System.Windows.Forms.Padding(4);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(50, 20);
            label3.TabIndex = 41;
            label3.Text = "Stage:";
            // 
            // cbDebugStages
            // 
            this.cbDebugStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDebugStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDebugStages.FormattingEnabled = true;
            this.cbDebugStages.Location = new System.Drawing.Point(55, 7);
            this.cbDebugStages.Margin = new System.Windows.Forms.Padding(4);
            this.cbDebugStages.MaxDropDownItems = 20;
            this.cbDebugStages.Name = "cbDebugStages";
            this.cbDebugStages.Size = new System.Drawing.Size(387, 21);
            this.cbDebugStages.TabIndex = 40;
            this.cbDebugStages.SelectedIndexChanged += new System.EventHandler(this.CbDebugStages_SelectedIndexChanged);
            // 
            // tbStages
            // 
            this.tbStages.BackColor = System.Drawing.Color.Gainsboro;
            this.tbStages.Controls.Add(label1);
            this.tbStages.Controls.Add(this.cbLabels);
            this.tbStages.Controls.Add(this.cbStages);
            this.tbStages.Controls.Add(stageLabel);
            this.tbStages.Controls.Add(this.tbResult);
            this.tbStages.Location = new System.Drawing.Point(4, 25);
            this.tbStages.Margin = new System.Windows.Forms.Padding(0);
            this.tbStages.Name = "tbStages";
            this.tbStages.Size = new System.Drawing.Size(637, 395);
            this.tbStages.TabIndex = 0;
            this.tbStages.Text = "Instructions";
            // 
            // tbResult
            // 
            this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbResult.Location = new System.Drawing.Point(0, 32);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(633, 360);
            this.tbResult.TabIndex = 31;
            this.tbResult.Text = "";
            this.tbResult.WordWrap = false;
            // 
            // stageLabel
            // 
            stageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            stageLabel.Location = new System.Drawing.Point(4, 8);
            stageLabel.Margin = new System.Windows.Forms.Padding(4);
            stageLabel.Name = "stageLabel";
            stageLabel.Size = new System.Drawing.Size(50, 20);
            stageLabel.TabIndex = 39;
            stageLabel.Text = "Stage:";
            // 
            // cbStages
            // 
            this.cbStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStages.FormattingEnabled = true;
            this.cbStages.ItemHeight = 13;
            this.cbStages.Location = new System.Drawing.Point(55, 7);
            this.cbStages.Margin = new System.Windows.Forms.Padding(4);
            this.cbStages.MaxDropDownItems = 40;
            this.cbStages.Name = "cbStages";
            this.cbStages.Size = new System.Drawing.Size(242, 21);
            this.cbStages.TabIndex = 38;
            this.cbStages.SelectedIndexChanged += new System.EventHandler(this.CbStages_SelectedIndexChanged);
            // 
            // cbLabels
            // 
            this.cbLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLabels.FormattingEnabled = true;
            this.cbLabels.Location = new System.Drawing.Point(354, 7);
            this.cbLabels.Margin = new System.Windows.Forms.Padding(4);
            this.cbLabels.MaxDropDownItems = 20;
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new System.Drawing.Size(105, 21);
            this.cbLabels.TabIndex = 40;
            this.cbLabels.SelectedIndexChanged += new System.EventHandler(this.CbLabels_SelectedIndexChanged);
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(302, 8);
            label1.Margin = new System.Windows.Forms.Padding(4);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(48, 20);
            label1.TabIndex = 41;
            label1.Text = "Label:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbStages);
            this.tabControl1.Controls.Add(this.tbDebug);
            this.tabControl1.Controls.Add(this.tbMethodCounters);
            this.tabControl1.Controls.Add(this.tbGlobalCounters);
            this.tabControl1.Controls.Add(this.tbLogs);
            this.tabControl1.Controls.Add(this.tbErrors);
            this.tabControl1.Controls.Add(this.tbExceptions);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(645, 424);
            this.tabControl1.TabIndex = 38;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 501);
            this.Controls.Add(this.cbPlatform);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "MOSA Explorer";
            this.Load += new System.EventHandler(this.Main_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tbExceptions.ResumeLayout(false);
            this.tbErrors.ResumeLayout(false);
            this.tbLogs.ResumeLayout(false);
            this.tbGlobalCounters.ResumeLayout(false);
            this.tbMethodCounters.ResumeLayout(false);
            this.tbDebug.ResumeLayout(false);
            this.tbStages.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nowToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripMenuItem snippetToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ComboBox cbPlatform;
		private System.Windows.Forms.ToolStripMenuItem cbEnableSSA;
		private System.Windows.Forms.ToolStripMenuItem cbEnableBinaryCodeGeneration;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem cbEnableOptimizations;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private System.Windows.Forms.ToolStripMenuItem cbEnableSparseConditionalConstantPropagation;
		private System.Windows.Forms.ToolStripMenuItem cbEnableInlinedMethods;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ToolStripMenuItem advanceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dumpAllMethodStagesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cbEnableIRLongExpansion;
		private System.Windows.Forms.ToolStripMenuItem cbEnableTwoPassOptimizations;
		private System.Windows.Forms.ToolStripMenuItem displayOptionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showOperandTypes;
		private System.Windows.Forms.ToolStripMenuItem showSizes;
		private System.Windows.Forms.ToolStripMenuItem padInstructions;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tbStages;
		private System.Windows.Forms.ComboBox cbLabels;
		private System.Windows.Forms.ComboBox cbStages;
		private System.Windows.Forms.RichTextBox tbResult;
		private System.Windows.Forms.TabPage tbDebug;
		private System.Windows.Forms.ComboBox cbDebugStages;
		private System.Windows.Forms.RichTextBox rbDebugResult;
		private System.Windows.Forms.TabPage tbMethodCounters;
		private System.Windows.Forms.RichTextBox rbMethodCounters;
		private System.Windows.Forms.TabPage tbGlobalCounters;
		private System.Windows.Forms.RichTextBox rbGlobalCounters;
		private System.Windows.Forms.TabPage tbLogs;
		private System.Windows.Forms.RichTextBox rbLog;
		private System.Windows.Forms.TabPage tbErrors;
		private System.Windows.Forms.RichTextBox rbErrors;
		private System.Windows.Forms.TabPage tbExceptions;
		private System.Windows.Forms.RichTextBox rbException;
	}
}
