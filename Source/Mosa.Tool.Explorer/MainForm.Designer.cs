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
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label stageLabel;
            System.Windows.Forms.Label label1;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
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
            this.cbEnableIROptimizations = new System.Windows.Forms.ToolStripMenuItem();
            this.cbEnableValueNumbering = new System.Windows.Forms.ToolStripMenuItem();
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
            this.cbEnableMethodScanner = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpAllMethodStagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.treeView = new System.Windows.Forms.TreeView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFilter = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabStages = new System.Windows.Forms.TabPage();
            this.cbLabels = new System.Windows.Forms.ComboBox();
            this.cbStages = new System.Windows.Forms.ComboBox();
            this.tbInstructions = new System.Windows.Forms.RichTextBox();
            this.tabStageDebug = new System.Windows.Forms.TabPage();
            this.cbDebugStages = new System.Windows.Forms.ComboBox();
            this.rbDebugResult = new System.Windows.Forms.RichTextBox();
            this.tabMethodCounters = new System.Windows.Forms.TabPage();
            this.rbMethodCounters = new System.Windows.Forms.RichTextBox();
            this.tabGlobalCounters = new System.Windows.Forms.TabPage();
            this.rbGlobalCounters = new System.Windows.Forms.RichTextBox();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.tbEvents = new System.Windows.Forms.RichTextBox();
            this.tabDebug = new System.Windows.Forms.TabPage();
            this.tbDebug = new System.Windows.Forms.RichTextBox();
            this.rbLog = new System.Windows.Forms.RichTextBox();
            this.tabErrors = new System.Windows.Forms.TabPage();
            this.rbErrors = new System.Windows.Forms.RichTextBox();
            this.tabExceptions = new System.Windows.Forms.TabPage();
            this.rbException = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cbPlatform = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            label3 = new System.Windows.Forms.Label();
            stageLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabStages.SuspendLayout();
            this.tabStageDebug.SuspendLayout();
            this.tabMethodCounters.SuspendLayout();
            this.tabGlobalCounters.SuspendLayout();
            this.tabEvents.SuspendLayout();
            this.tabDebug.SuspendLayout();
            this.tabErrors.SuspendLayout();
            this.tabExceptions.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 456);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(856, 22);
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
            this.menuStrip1.Size = new System.Drawing.Size(856, 24);
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
            this.cbEnableIROptimizations,
            this.cbEnableValueNumbering,
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
            // cbEnableIROptimizations
            // 
            this.cbEnableIROptimizations.Checked = true;
            this.cbEnableIROptimizations.CheckOnClick = true;
            this.cbEnableIROptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableIROptimizations.Name = "cbEnableIROptimizations";
            this.cbEnableIROptimizations.Size = new System.Drawing.Size(293, 22);
            this.cbEnableIROptimizations.Text = "Enable Optimizations";
            // 
            // cbEnableValueNumbering
            // 
            this.cbEnableValueNumbering.Checked = true;
            this.cbEnableValueNumbering.CheckOnClick = true;
            this.cbEnableValueNumbering.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnableValueNumbering.Name = "cbEnableValueNumbering";
            this.cbEnableValueNumbering.Size = new System.Drawing.Size(293, 22);
            this.cbEnableValueNumbering.Text = "Enable Value Numbering";
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
            this.advanceToolStripMenuItem.CheckOnClick = true;
            this.advanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbEnableMethodScanner,
            this.dumpAllMethodStagesToolStripMenuItem});
            this.advanceToolStripMenuItem.Name = "advanceToolStripMenuItem";
            this.advanceToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.advanceToolStripMenuItem.Text = "Advance";
            // 
            // cbEnableMethodScanner
            // 
            this.cbEnableMethodScanner.Name = "cbEnableMethodScanner";
            this.cbEnableMethodScanner.Size = new System.Drawing.Size(206, 22);
            this.cbEnableMethodScanner.Text = "Enable Method Scanner";
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
            this.treeView.Location = new System.Drawing.Point(0, 25);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(229, 379);
            this.treeView.TabIndex = 3;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
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
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.tbFilter);
            this.splitContainer1.Panel1.Controls.Add(this.treeView);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(852, 401);
            this.splitContainer1.SplitterDistance = 226;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 26;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Filter:";
            // 
            // tbFilter
            // 
            this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbFilter.Location = new System.Drawing.Point(33, 4);
            this.tbFilter.Name = "tbFilter";
            this.tbFilter.Size = new System.Drawing.Size(193, 20);
            this.tbFilter.TabIndex = 4;
            this.tbFilter.TextChanged += new System.EventHandler(this.tbFilter_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabStages);
            this.tabControl1.Controls.Add(this.tabStageDebug);
            this.tabControl1.Controls.Add(this.tabMethodCounters);
            this.tabControl1.Controls.Add(this.tabGlobalCounters);
            this.tabControl1.Controls.Add(this.tabEvents);
            this.tabControl1.Controls.Add(this.tabDebug);
            this.tabControl1.Controls.Add(this.tabErrors);
            this.tabControl1.Controls.Add(this.tabExceptions);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(0, 0);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(618, 401);
            this.tabControl1.TabIndex = 38;
            // 
            // tabStages
            // 
            this.tabStages.BackColor = System.Drawing.Color.Gainsboro;
            this.tabStages.Controls.Add(label1);
            this.tabStages.Controls.Add(this.cbLabels);
            this.tabStages.Controls.Add(this.cbStages);
            this.tabStages.Controls.Add(stageLabel);
            this.tabStages.Controls.Add(this.tbInstructions);
            this.tabStages.Location = new System.Drawing.Point(4, 25);
            this.tabStages.Margin = new System.Windows.Forms.Padding(0);
            this.tabStages.Name = "tabStages";
            this.tabStages.Size = new System.Drawing.Size(633, 250);
            this.tabStages.TabIndex = 0;
            this.tabStages.Text = "Instructions";
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
            // tbInstructions
            // 
            this.tbInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbInstructions.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInstructions.Location = new System.Drawing.Point(0, 32);
            this.tbInstructions.Name = "tbInstructions";
            this.tbInstructions.Size = new System.Drawing.Size(645, 215);
            this.tbInstructions.TabIndex = 31;
            this.tbInstructions.Text = "";
            this.tbInstructions.WordWrap = false;
            // 
            // tabStageDebug
            // 
            this.tabStageDebug.BackColor = System.Drawing.Color.Gainsboro;
            this.tabStageDebug.Controls.Add(this.cbDebugStages);
            this.tabStageDebug.Controls.Add(label3);
            this.tabStageDebug.Controls.Add(this.rbDebugResult);
            this.tabStageDebug.Location = new System.Drawing.Point(4, 25);
            this.tabStageDebug.Margin = new System.Windows.Forms.Padding(0);
            this.tabStageDebug.Name = "tabStageDebug";
            this.tabStageDebug.Size = new System.Drawing.Size(633, 250);
            this.tabStageDebug.TabIndex = 1;
            this.tabStageDebug.Text = "Debug";
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
            // rbDebugResult
            // 
            this.rbDebugResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbDebugResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDebugResult.Location = new System.Drawing.Point(0, 32);
            this.rbDebugResult.Name = "rbDebugResult";
            this.rbDebugResult.Size = new System.Drawing.Size(645, 215);
            this.rbDebugResult.TabIndex = 32;
            this.rbDebugResult.Text = "";
            this.rbDebugResult.WordWrap = false;
            // 
            // tabMethodCounters
            // 
            this.tabMethodCounters.Controls.Add(this.rbMethodCounters);
            this.tabMethodCounters.Location = new System.Drawing.Point(4, 25);
            this.tabMethodCounters.Name = "tabMethodCounters";
            this.tabMethodCounters.Size = new System.Drawing.Size(633, 250);
            this.tabMethodCounters.TabIndex = 6;
            this.tabMethodCounters.Text = "Counters";
            this.tabMethodCounters.UseVisualStyleBackColor = true;
            // 
            // rbMethodCounters
            // 
            this.rbMethodCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbMethodCounters.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbMethodCounters.Location = new System.Drawing.Point(0, 0);
            this.rbMethodCounters.Name = "rbMethodCounters";
            this.rbMethodCounters.Size = new System.Drawing.Size(633, 246);
            this.rbMethodCounters.TabIndex = 2;
            this.rbMethodCounters.Text = "";
            this.rbMethodCounters.WordWrap = false;
            // 
            // tabGlobalCounters
            // 
            this.tabGlobalCounters.BackColor = System.Drawing.Color.Gainsboro;
            this.tabGlobalCounters.Controls.Add(this.rbGlobalCounters);
            this.tabGlobalCounters.Location = new System.Drawing.Point(4, 25);
            this.tabGlobalCounters.Name = "tabGlobalCounters";
            this.tabGlobalCounters.Padding = new System.Windows.Forms.Padding(3);
            this.tabGlobalCounters.Size = new System.Drawing.Size(633, 250);
            this.tabGlobalCounters.TabIndex = 4;
            this.tabGlobalCounters.Text = "Global Counters";
            // 
            // rbGlobalCounters
            // 
            this.rbGlobalCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbGlobalCounters.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbGlobalCounters.Location = new System.Drawing.Point(0, 0);
            this.rbGlobalCounters.Name = "rbGlobalCounters";
            this.rbGlobalCounters.Size = new System.Drawing.Size(633, 246);
            this.rbGlobalCounters.TabIndex = 1;
            this.rbGlobalCounters.Text = "";
            this.rbGlobalCounters.WordWrap = false;
            // 
            // tabEvents
            // 
            this.tabEvents.Controls.Add(this.tbEvents);
            this.tabEvents.Location = new System.Drawing.Point(4, 25);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Size = new System.Drawing.Size(633, 250);
            this.tabEvents.TabIndex = 7;
            this.tabEvents.Text = "Events";
            this.tabEvents.UseVisualStyleBackColor = true;
            // 
            // tbEvents
            // 
            this.tbEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbEvents.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.tbEvents.Location = new System.Drawing.Point(0, 0);
            this.tbEvents.Name = "tbEvents";
            this.tbEvents.Size = new System.Drawing.Size(633, 246);
            this.tbEvents.TabIndex = 3;
            this.tbEvents.Text = "";
            this.tbEvents.WordWrap = false;
            // 
            // tabDebug
            // 
            this.tabDebug.BackColor = System.Drawing.Color.Gainsboro;
            this.tabDebug.Controls.Add(this.tbDebug);
            this.tabDebug.Controls.Add(this.rbLog);
            this.tabDebug.Location = new System.Drawing.Point(4, 25);
            this.tabDebug.Name = "tabDebug";
            this.tabDebug.Padding = new System.Windows.Forms.Padding(3);
            this.tabDebug.Size = new System.Drawing.Size(610, 372);
            this.tabDebug.TabIndex = 3;
            this.tabDebug.Text = "Debug";
            // 
            // tbDebug
            // 
            this.tbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDebug.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.tbDebug.Location = new System.Drawing.Point(0, 0);
            this.tbDebug.Name = "tbDebug";
            this.tbDebug.Size = new System.Drawing.Size(610, 368);
            this.tbDebug.TabIndex = 2;
            this.tbDebug.Text = "";
            this.tbDebug.WordWrap = false;
            // 
            // rbLog
            // 
            this.rbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbLog.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbLog.Location = new System.Drawing.Point(0, 0);
            this.rbLog.Name = "rbLog";
            this.rbLog.Size = new System.Drawing.Size(622, 368);
            this.rbLog.TabIndex = 1;
            this.rbLog.Text = "";
            this.rbLog.WordWrap = false;
            // 
            // tabErrors
            // 
            this.tabErrors.BackColor = System.Drawing.Color.Gainsboro;
            this.tabErrors.Controls.Add(this.rbErrors);
            this.tabErrors.Location = new System.Drawing.Point(4, 25);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tabErrors.Size = new System.Drawing.Size(633, 250);
            this.tabErrors.TabIndex = 2;
            this.tabErrors.Text = "Errors";
            // 
            // rbErrors
            // 
            this.rbErrors.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbErrors.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbErrors.Location = new System.Drawing.Point(0, 0);
            this.rbErrors.Name = "rbErrors";
            this.rbErrors.Size = new System.Drawing.Size(633, 246);
            this.rbErrors.TabIndex = 0;
            this.rbErrors.Text = "";
            this.rbErrors.WordWrap = false;
            // 
            // tabExceptions
            // 
            this.tabExceptions.Controls.Add(this.rbException);
            this.tabExceptions.Location = new System.Drawing.Point(4, 25);
            this.tabExceptions.Name = "tabExceptions";
            this.tabExceptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabExceptions.Size = new System.Drawing.Size(633, 250);
            this.tabExceptions.TabIndex = 5;
            this.tabExceptions.Text = "Exceptions";
            this.tabExceptions.UseVisualStyleBackColor = true;
            // 
            // rbException
            // 
            this.rbException.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbException.Font = new System.Drawing.Font("Lucida Console", 8F);
            this.rbException.Location = new System.Drawing.Point(0, 0);
            this.rbException.Name = "rbException";
            this.rbException.Size = new System.Drawing.Size(633, 246);
            this.rbException.TabIndex = 2;
            this.rbException.Text = "";
            this.rbException.WordWrap = false;
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
            this.toolStrip1.Size = new System.Drawing.Size(856, 25);
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
            "x64",
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
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 478);
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
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabStages.ResumeLayout(false);
            this.tabStageDebug.ResumeLayout(false);
            this.tabMethodCounters.ResumeLayout(false);
            this.tabGlobalCounters.ResumeLayout(false);
            this.tabEvents.ResumeLayout(false);
            this.tabDebug.ResumeLayout(false);
            this.tabErrors.ResumeLayout(false);
            this.tabExceptions.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
		private System.Windows.Forms.ToolStripMenuItem cbEnableIROptimizations;
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
		private System.Windows.Forms.TabPage tabStages;
		private System.Windows.Forms.ComboBox cbLabels;
		private System.Windows.Forms.ComboBox cbStages;
		private System.Windows.Forms.RichTextBox tbInstructions;
		private System.Windows.Forms.TabPage tabStageDebug;
		private System.Windows.Forms.ComboBox cbDebugStages;
		private System.Windows.Forms.RichTextBox rbDebugResult;
		private System.Windows.Forms.TabPage tabMethodCounters;
		private System.Windows.Forms.RichTextBox rbMethodCounters;
		private System.Windows.Forms.TabPage tabGlobalCounters;
		private System.Windows.Forms.RichTextBox rbGlobalCounters;
		private System.Windows.Forms.TabPage tabDebug;
		private System.Windows.Forms.RichTextBox rbLog;
		private System.Windows.Forms.TabPage tabErrors;
		private System.Windows.Forms.RichTextBox rbErrors;
		private System.Windows.Forms.TabPage tabExceptions;
		private System.Windows.Forms.RichTextBox rbException;
		private System.Windows.Forms.ToolStripMenuItem cbEnableValueNumbering;
		private System.Windows.Forms.ToolStripMenuItem cbEnableMethodScanner;
		private System.Windows.Forms.TextBox tbFilter;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabPage tabEvents;
		private System.Windows.Forms.RichTextBox tbEvents;
		private System.Windows.Forms.RichTextBox tbDebug;
	}
}
