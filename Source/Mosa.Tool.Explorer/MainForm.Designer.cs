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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.stageLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
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
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableAllOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			this.cbDisableAllOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.cbEnableSSA = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableBasicOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableValueNumbering = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableSparseConditionalConstantPropagation = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableDevirtualization = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableInline = new System.Windows.Forms.ToolStripMenuItem();
			this.cbInlineExplicitOnly = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableLongExpansion = new System.Windows.Forms.ToolStripMenuItem();
			this.cbLoopInvariantCodeMotion = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableBitTracker = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableTwoPassOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			this.cbPlatformOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableBinaryCodeGeneration = new System.Windows.Forms.ToolStripMenuItem();
			this.displayOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showOperandTypes = new System.Windows.Forms.ToolStripMenuItem();
			this.padInstructions = new System.Windows.Forms.ToolStripMenuItem();
			this.showSizes = new System.Windows.Forms.ToolStripMenuItem();
			this.advanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CBEnableMultithreading = new System.Windows.Forms.ToolStripMenuItem();
			this.cbEnableMethodScanner = new System.Windows.Forms.ToolStripMenuItem();
			this.dumpAllMethodStagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeView = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label2 = new System.Windows.Forms.Label();
			this.tbFilter = new System.Windows.Forms.TextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabStages = new System.Windows.Forms.TabPage();
			this.cbLabels = new System.Windows.Forms.ComboBox();
			this.cbStages = new System.Windows.Forms.ComboBox();
			this.tbInstructions = new System.Windows.Forms.RichTextBox();
			this.tabStageDebug = new System.Windows.Forms.TabPage();
			this.cbDebugStages = new System.Windows.Forms.ComboBox();
			this.tbDebugResult = new System.Windows.Forms.RichTextBox();
			this.tabMethodCounters = new System.Windows.Forms.TabPage();
			this.tbMethodCounters = new System.Windows.Forms.RichTextBox();
			this.tabLogs = new System.Windows.Forms.TabPage();
			this.cbSectionLogs = new System.Windows.Forms.ComboBox();
			this.tbLogs = new System.Windows.Forms.RichTextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.cbPlatform = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabStages.SuspendLayout();
			this.tabStageDebug.SuspendLayout();
			this.tabMethodCounters.SuspendLayout();
			this.tabLogs.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label4.Location = new System.Drawing.Point(5, 9);
			this.label4.Margin = new System.Windows.Forms.Padding(5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(70, 23);
			this.label4.TabIndex = 45;
			this.label4.Text = "Section:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(5, 9);
			this.label3.Margin = new System.Windows.Forms.Padding(5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 23);
			this.label3.TabIndex = 41;
			this.label3.Text = "Stage:";
			// 
			// stageLabel
			// 
			this.stageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.stageLabel.Location = new System.Drawing.Point(5, 9);
			this.stageLabel.Margin = new System.Windows.Forms.Padding(5);
			this.stageLabel.Name = "stageLabel";
			this.stageLabel.Size = new System.Drawing.Size(58, 23);
			this.stageLabel.TabIndex = 39;
			this.stageLabel.Text = "Stage:";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(352, 9);
			this.label1.Margin = new System.Windows.Forms.Padding(5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 23);
			this.label1.TabIndex = 41;
			this.label1.Text = "Label:";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 545);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusStrip1.Size = new System.Drawing.Size(954, 24);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 19);
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(233, 18);
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 19);
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
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(954, 24);
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
            this.nowToolStripMenuItem});
			this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			this.compileToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
			this.compileToolStripMenuItem.Text = "Compile";
			// 
			// nowToolStripMenuItem
			// 
			this.nowToolStripMenuItem.Name = "nowToolStripMenuItem";
			this.nowToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			this.nowToolStripMenuItem.Text = "Now";
			this.nowToolStripMenuItem.Click += new System.EventHandler(this.NowToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbEnableAllOptimizations,
            this.cbDisableAllOptimizations,
            this.toolStripSeparator4,
            this.cbEnableSSA,
            this.cbEnableBasicOptimizations,
            this.cbEnableValueNumbering,
            this.cbEnableSparseConditionalConstantPropagation,
            this.cbEnableDevirtualization,
            this.cbEnableInline,
            this.cbInlineExplicitOnly,
            this.cbEnableLongExpansion,
            this.cbLoopInvariantCodeMotion,
            this.cbEnableBitTracker,
            this.cbEnableTwoPassOptimizations,
            this.cbPlatformOptimizations,
            this.cbEnableBinaryCodeGeneration});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
			this.optionsToolStripMenuItem.Text = "Optimizations";
			// 
			// cbEnableAllOptimizations
			// 
			this.cbEnableAllOptimizations.Name = "cbEnableAllOptimizations";
			this.cbEnableAllOptimizations.Size = new System.Drawing.Size(293, 22);
			this.cbEnableAllOptimizations.Text = "Enable All";
			this.cbEnableAllOptimizations.Click += new System.EventHandler(this.cbEnableAllOptimizations_Click);
			// 
			// cbDisableAllOptimizations
			// 
			this.cbDisableAllOptimizations.Name = "cbDisableAllOptimizations";
			this.cbDisableAllOptimizations.Size = new System.Drawing.Size(293, 22);
			this.cbDisableAllOptimizations.Text = "Disable All";
			this.cbDisableAllOptimizations.Click += new System.EventHandler(this.cbDisableAllOptimizations_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(290, 6);
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
			// cbEnableBasicOptimizations
			// 
			this.cbEnableBasicOptimizations.Checked = true;
			this.cbEnableBasicOptimizations.CheckOnClick = true;
			this.cbEnableBasicOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableBasicOptimizations.Name = "cbEnableBasicOptimizations";
			this.cbEnableBasicOptimizations.Size = new System.Drawing.Size(293, 22);
			this.cbEnableBasicOptimizations.Text = "Enable Optimizations";
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
			// cbEnableDevirtualization
			// 
			this.cbEnableDevirtualization.Checked = true;
			this.cbEnableDevirtualization.CheckOnClick = true;
			this.cbEnableDevirtualization.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableDevirtualization.Name = "cbEnableDevirtualization";
			this.cbEnableDevirtualization.Size = new System.Drawing.Size(293, 22);
			this.cbEnableDevirtualization.Text = "Enable Devirtualization";
			// 
			// cbEnableInline
			// 
			this.cbEnableInline.Checked = true;
			this.cbEnableInline.CheckOnClick = true;
			this.cbEnableInline.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableInline.Name = "cbEnableInline";
			this.cbEnableInline.Size = new System.Drawing.Size(293, 22);
			this.cbEnableInline.Text = "Enable Inlined Methods";
			// 
			// cbInlineExplicitOnly
			// 
			this.cbInlineExplicitOnly.CheckOnClick = true;
			this.cbInlineExplicitOnly.Name = "cbInlineExplicitOnly";
			this.cbInlineExplicitOnly.Size = new System.Drawing.Size(293, 22);
			this.cbInlineExplicitOnly.Text = "Inline Only Explicit";
			// 
			// cbEnableLongExpansion
			// 
			this.cbEnableLongExpansion.Checked = true;
			this.cbEnableLongExpansion.CheckOnClick = true;
			this.cbEnableLongExpansion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableLongExpansion.Name = "cbEnableLongExpansion";
			this.cbEnableLongExpansion.Size = new System.Drawing.Size(293, 22);
			this.cbEnableLongExpansion.Text = "Enable Long Expansion";
			// 
			// cbLoopInvariantCodeMotion
			// 
			this.cbLoopInvariantCodeMotion.Checked = true;
			this.cbLoopInvariantCodeMotion.CheckOnClick = true;
			this.cbLoopInvariantCodeMotion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbLoopInvariantCodeMotion.Name = "cbLoopInvariantCodeMotion";
			this.cbLoopInvariantCodeMotion.Size = new System.Drawing.Size(293, 22);
			this.cbLoopInvariantCodeMotion.Text = "Enable Loop Invariant Code Motion";
			// 
			// cbEnableBitTracker
			// 
			this.cbEnableBitTracker.Checked = true;
			this.cbEnableBitTracker.CheckOnClick = true;
			this.cbEnableBitTracker.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableBitTracker.Name = "cbEnableBitTracker";
			this.cbEnableBitTracker.Size = new System.Drawing.Size(293, 22);
			this.cbEnableBitTracker.Text = "Enable Bit Tracker";
			// 
			// cbEnableTwoPassOptimizations
			// 
			this.cbEnableTwoPassOptimizations.Checked = true;
			this.cbEnableTwoPassOptimizations.CheckOnClick = true;
			this.cbEnableTwoPassOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbEnableTwoPassOptimizations.Name = "cbEnableTwoPassOptimizations";
			this.cbEnableTwoPassOptimizations.Size = new System.Drawing.Size(293, 22);
			this.cbEnableTwoPassOptimizations.Text = "Enable Two Optimization Passes";
			// 
			// cbPlatformOptimizations
			// 
			this.cbPlatformOptimizations.Checked = true;
			this.cbPlatformOptimizations.CheckOnClick = true;
			this.cbPlatformOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbPlatformOptimizations.Name = "cbPlatformOptimizations";
			this.cbPlatformOptimizations.Size = new System.Drawing.Size(293, 22);
			this.cbPlatformOptimizations.Text = "Enable Platform Optimizations";
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
			this.showOperandTypes.Size = new System.Drawing.Size(184, 22);
			this.showOperandTypes.Text = "Show Operand Types";
			this.showOperandTypes.CheckStateChanged += new System.EventHandler(this.showOperandTypes_CheckStateChanged);
			// 
			// padInstructions
			// 
			this.padInstructions.Checked = true;
			this.padInstructions.CheckOnClick = true;
			this.padInstructions.CheckState = System.Windows.Forms.CheckState.Checked;
			this.padInstructions.Name = "padInstructions";
			this.padInstructions.Size = new System.Drawing.Size(184, 22);
			this.padInstructions.Text = "Pad Instructions";
			this.padInstructions.CheckStateChanged += new System.EventHandler(this.padInstructions_CheckStateChanged);
			// 
			// showSizes
			// 
			this.showSizes.Checked = true;
			this.showSizes.CheckOnClick = true;
			this.showSizes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showSizes.Name = "showSizes";
			this.showSizes.Size = new System.Drawing.Size(184, 22);
			this.showSizes.Text = "Show Sizes";
			this.showSizes.Click += new System.EventHandler(this.showSizesToolStripMenuItem_Click);
			// 
			// advanceToolStripMenuItem
			// 
			this.advanceToolStripMenuItem.CheckOnClick = true;
			this.advanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CBEnableMultithreading,
            this.cbEnableMethodScanner,
            this.dumpAllMethodStagesToolStripMenuItem});
			this.advanceToolStripMenuItem.Name = "advanceToolStripMenuItem";
			this.advanceToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
			this.advanceToolStripMenuItem.Text = "Advance";
			// 
			// CBEnableMultithreading
			// 
			this.CBEnableMultithreading.Checked = true;
			this.CBEnableMultithreading.CheckOnClick = true;
			this.CBEnableMultithreading.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CBEnableMultithreading.Name = "CBEnableMultithreading";
			this.CBEnableMultithreading.Size = new System.Drawing.Size(206, 22);
			this.CBEnableMultithreading.Text = "Enable Multithreading";
			// 
			// cbEnableMethodScanner
			// 
			this.cbEnableMethodScanner.CheckOnClick = true;
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
			this.openFileDialog.FilterIndex = 2;
			// 
			// treeView
			// 
			this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView.Location = new System.Drawing.Point(4, 31);
			this.treeView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(244, 456);
			this.treeView.TabIndex = 3;
			this.treeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeSelect);
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
			this.treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseDoubleClick);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 54);
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
			this.splitContainer1.Panel2.Controls.Add(this.tabControl);
			this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer1.Size = new System.Drawing.Size(950, 490);
			this.splitContainer1.SplitterDistance = 249;
			this.splitContainer1.SplitterWidth = 2;
			this.splitContainer1.TabIndex = 26;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(4, 8);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 15);
			this.label2.TabIndex = 5;
			this.label2.Text = "Filter:";
			// 
			// tbFilter
			// 
			this.tbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbFilter.Location = new System.Drawing.Point(48, 5);
			this.tbFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbFilter.Name = "tbFilter";
			this.tbFilter.Size = new System.Drawing.Size(200, 23);
			this.tbFilter.TabIndex = 4;
			this.tbFilter.TextChanged += new System.EventHandler(this.tbFilter_TextChanged);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabStages);
			this.tabControl.Controls.Add(this.tabStageDebug);
			this.tabControl.Controls.Add(this.tabMethodCounters);
			this.tabControl.Controls.Add(this.tabLogs);
			this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tabControl.Location = new System.Drawing.Point(0, 0);
			this.tabControl.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl.Name = "tabControl";
			this.tabControl.Padding = new System.Drawing.Point(0, 0);
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(716, 490);
			this.tabControl.TabIndex = 38;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabStages
			// 
			this.tabStages.BackColor = System.Drawing.Color.Gainsboro;
			this.tabStages.Controls.Add(this.label1);
			this.tabStages.Controls.Add(this.cbLabels);
			this.tabStages.Controls.Add(this.cbStages);
			this.tabStages.Controls.Add(this.stageLabel);
			this.tabStages.Controls.Add(this.tbInstructions);
			this.tabStages.Location = new System.Drawing.Point(4, 25);
			this.tabStages.Margin = new System.Windows.Forms.Padding(0);
			this.tabStages.Name = "tabStages";
			this.tabStages.Size = new System.Drawing.Size(708, 461);
			this.tabStages.TabIndex = 0;
			this.tabStages.Text = "Instructions";
			// 
			// cbLabels
			// 
			this.cbLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbLabels.FormattingEnabled = true;
			this.cbLabels.Location = new System.Drawing.Point(413, 8);
			this.cbLabels.Margin = new System.Windows.Forms.Padding(5);
			this.cbLabels.MaxDropDownItems = 20;
			this.cbLabels.Name = "cbLabels";
			this.cbLabels.Size = new System.Drawing.Size(122, 21);
			this.cbLabels.TabIndex = 40;
			this.cbLabels.SelectedIndexChanged += new System.EventHandler(this.CbLabels_SelectedIndexChanged);
			// 
			// cbStages
			// 
			this.cbStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbStages.FormattingEnabled = true;
			this.cbStages.ItemHeight = 13;
			this.cbStages.Location = new System.Drawing.Point(64, 8);
			this.cbStages.Margin = new System.Windows.Forms.Padding(5);
			this.cbStages.MaxDropDownItems = 40;
			this.cbStages.Name = "cbStages";
			this.cbStages.Size = new System.Drawing.Size(282, 21);
			this.cbStages.TabIndex = 38;
			this.cbStages.SelectedIndexChanged += new System.EventHandler(this.CbStages_SelectedIndexChanged);
			// 
			// tbInstructions
			// 
			this.tbInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbInstructions.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbInstructions.Location = new System.Drawing.Point(0, 37);
			this.tbInstructions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbInstructions.Name = "tbInstructions";
			this.tbInstructions.Size = new System.Drawing.Size(691, 421);
			this.tbInstructions.TabIndex = 31;
			this.tbInstructions.Text = "";
			this.tbInstructions.WordWrap = false;
			// 
			// tabStageDebug
			// 
			this.tabStageDebug.BackColor = System.Drawing.Color.Gainsboro;
			this.tabStageDebug.Controls.Add(this.cbDebugStages);
			this.tabStageDebug.Controls.Add(this.label3);
			this.tabStageDebug.Controls.Add(this.tbDebugResult);
			this.tabStageDebug.Location = new System.Drawing.Point(4, 25);
			this.tabStageDebug.Margin = new System.Windows.Forms.Padding(0);
			this.tabStageDebug.Name = "tabStageDebug";
			this.tabStageDebug.Size = new System.Drawing.Size(708, 461);
			this.tabStageDebug.TabIndex = 1;
			this.tabStageDebug.Text = "Debug";
			// 
			// cbDebugStages
			// 
			this.cbDebugStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDebugStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbDebugStages.FormattingEnabled = true;
			this.cbDebugStages.Location = new System.Drawing.Point(64, 8);
			this.cbDebugStages.Margin = new System.Windows.Forms.Padding(5);
			this.cbDebugStages.MaxDropDownItems = 20;
			this.cbDebugStages.Name = "cbDebugStages";
			this.cbDebugStages.Size = new System.Drawing.Size(451, 21);
			this.cbDebugStages.TabIndex = 40;
			this.cbDebugStages.SelectedIndexChanged += new System.EventHandler(this.CbDebugStages_SelectedIndexChanged);
			// 
			// tbDebugResult
			// 
			this.tbDebugResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDebugResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbDebugResult.Location = new System.Drawing.Point(0, 37);
			this.tbDebugResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbDebugResult.Name = "tbDebugResult";
			this.tbDebugResult.Size = new System.Drawing.Size(691, 412);
			this.tbDebugResult.TabIndex = 32;
			this.tbDebugResult.Text = "";
			this.tbDebugResult.WordWrap = false;
			// 
			// tabMethodCounters
			// 
			this.tabMethodCounters.BackColor = System.Drawing.Color.Gainsboro;
			this.tabMethodCounters.Controls.Add(this.tbMethodCounters);
			this.tabMethodCounters.Location = new System.Drawing.Point(4, 25);
			this.tabMethodCounters.Margin = new System.Windows.Forms.Padding(0);
			this.tabMethodCounters.Name = "tabMethodCounters";
			this.tabMethodCounters.Size = new System.Drawing.Size(708, 461);
			this.tabMethodCounters.TabIndex = 6;
			this.tabMethodCounters.Text = "Counters";
			// 
			// tbMethodCounters
			// 
			this.tbMethodCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMethodCounters.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbMethodCounters.Location = new System.Drawing.Point(0, 0);
			this.tbMethodCounters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbMethodCounters.Name = "tbMethodCounters";
			this.tbMethodCounters.Size = new System.Drawing.Size(691, 449);
			this.tbMethodCounters.TabIndex = 3;
			this.tbMethodCounters.Text = "";
			this.tbMethodCounters.WordWrap = false;
			// 
			// tabLogs
			// 
			this.tabLogs.BackColor = System.Drawing.Color.Gainsboro;
			this.tabLogs.Controls.Add(this.cbSectionLogs);
			this.tabLogs.Controls.Add(this.label4);
			this.tabLogs.Controls.Add(this.tbLogs);
			this.tabLogs.Location = new System.Drawing.Point(4, 25);
			this.tabLogs.Margin = new System.Windows.Forms.Padding(0);
			this.tabLogs.Name = "tabLogs";
			this.tabLogs.Size = new System.Drawing.Size(708, 461);
			this.tabLogs.TabIndex = 7;
			this.tabLogs.Text = "Logs";
			// 
			// cbSectionLogs
			// 
			this.cbSectionLogs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSectionLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbSectionLogs.FormattingEnabled = true;
			this.cbSectionLogs.Location = new System.Drawing.Point(77, 8);
			this.cbSectionLogs.Margin = new System.Windows.Forms.Padding(5);
			this.cbSectionLogs.MaxDropDownItems = 20;
			this.cbSectionLogs.Name = "cbSectionLogs";
			this.cbSectionLogs.Size = new System.Drawing.Size(285, 21);
			this.cbSectionLogs.TabIndex = 44;
			this.cbSectionLogs.SelectedIndexChanged += new System.EventHandler(this.cbSections_SelectedIndexChanged);
			// 
			// tbLogs
			// 
			this.tbLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLogs.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbLogs.Location = new System.Drawing.Point(0, 37);
			this.tbLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbLogs.Name = "tbLogs";
			this.tbLogs.Size = new System.Drawing.Size(691, 412);
			this.tbLogs.TabIndex = 3;
			this.tbLogs.Text = "";
			this.tbLogs.WordWrap = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbPlatform,
            this.toolStripSeparator3,
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripButton4,
            this.toolStripSeparator1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(954, 25);
			this.toolStrip1.TabIndex = 27;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// cbPlatform
			// 
			this.cbPlatform.BackColor = System.Drawing.SystemColors.Window;
			this.cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPlatform.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cbPlatform.Items.AddRange(new object[] {
            "x86",
            "x64",
            "ARMv8A32"});
			this.cbPlatform.Name = "cbPlatform";
			this.cbPlatform.Size = new System.Drawing.Size(104, 25);
			this.cbPlatform.SelectedIndexChanged += new System.EventHandler(this.cbPlatform_SelectedIndexChanged);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
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
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(954, 569);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
			this.tabControl.ResumeLayout(false);
			this.tabStages.ResumeLayout(false);
			this.tabStageDebug.ResumeLayout(false);
			this.tabMethodCounters.ResumeLayout(false);
			this.tabLogs.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ToolStripMenuItem cbEnableSSA;
		private System.Windows.Forms.ToolStripMenuItem cbEnableBinaryCodeGeneration;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem cbEnableBasicOptimizations;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private System.Windows.Forms.ToolStripMenuItem cbEnableSparseConditionalConstantPropagation;
		private System.Windows.Forms.ToolStripMenuItem cbEnableInline;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ToolStripMenuItem advanceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dumpAllMethodStagesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem cbEnableLongExpansion;
		private System.Windows.Forms.ToolStripMenuItem cbEnableTwoPassOptimizations;
		private System.Windows.Forms.ToolStripMenuItem displayOptionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showOperandTypes;
		private System.Windows.Forms.ToolStripMenuItem showSizes;
		private System.Windows.Forms.ToolStripMenuItem padInstructions;
		private System.Windows.Forms.ToolStripMenuItem cbEnableValueNumbering;
		private System.Windows.Forms.ToolStripMenuItem cbEnableMethodScanner;
		private System.Windows.Forms.TextBox tbFilter;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabStages;
		private System.Windows.Forms.ComboBox cbLabels;
		private System.Windows.Forms.ComboBox cbStages;
		private System.Windows.Forms.RichTextBox tbInstructions;
		private System.Windows.Forms.TabPage tabStageDebug;
		private System.Windows.Forms.ComboBox cbDebugStages;
		private System.Windows.Forms.RichTextBox tbDebugResult;
		private System.Windows.Forms.TabPage tabMethodCounters;
		private System.Windows.Forms.RichTextBox tbMethodCounters;
		private System.Windows.Forms.TabPage tabLogs;
		private System.Windows.Forms.ComboBox cbSectionLogs;
		private System.Windows.Forms.RichTextBox tbLogs;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripComboBox cbPlatform;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cbEnableBitTracker;
		private System.Windows.Forms.ToolStripMenuItem CBEnableMultithreading;
		private System.Windows.Forms.ToolStripMenuItem cbLoopInvariantCodeMotion;
		private System.Windows.Forms.ToolStripMenuItem cbPlatformOptimizations;
		private System.Windows.Forms.ToolStripMenuItem cbInlineExplicitOnly;
		private System.Windows.Forms.ToolStripMenuItem cbEnableAllOptimizations;
		private System.Windows.Forms.ToolStripMenuItem cbDisableAllOptimizations;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem cbEnableDevirtualization;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label stageLabel;
		private System.Windows.Forms.Label label1;
	}
}
