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
			this.cbInlineExplicit = new System.Windows.Forms.ToolStripMenuItem();
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
			this.cbCILDecoderStageV2Testing = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeView = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.label2 = new System.Windows.Forms.Label();
			this.tbFilter = new System.Windows.Forms.TextBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabStages = new System.Windows.Forms.TabPage();
			this.label1 = new System.Windows.Forms.Label();
			this.cbInstructionLabels = new System.Windows.Forms.ComboBox();
			this.cbInstructionStages = new System.Windows.Forms.ComboBox();
			this.stageLabel = new System.Windows.Forms.Label();
			this.tbInstructions = new System.Windows.Forms.RichTextBox();
			this.tabStageDebug = new System.Windows.Forms.TabPage();
			this.cbDebugStages = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbDebugResult = new System.Windows.Forms.RichTextBox();
			this.tabTransforms = new System.Windows.Forms.TabPage();
			this.label7 = new System.Windows.Forms.Label();
			this.cbTransformLabels = new System.Windows.Forms.ComboBox();
			this.cbTransformStages = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.lbSteps = new System.Windows.Forms.Label();
			this.btnLast = new System.Windows.Forms.Button();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnPrevious = new System.Windows.Forms.Button();
			this.btnFirst = new System.Windows.Forms.Button();
			this.tbTransforms = new System.Windows.Forms.RichTextBox();
			this.tabMethodCounters = new System.Windows.Forms.TabPage();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.tbCounterFilter = new System.Windows.Forms.TextBox();
			this.gridMethodCounters = new System.Windows.Forms.DataGridView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.tbMethodCounters = new System.Windows.Forms.RichTextBox();
			this.tabLogs = new System.Windows.Forms.TabPage();
			this.cbCompilerSections = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbCompilerLogs = new System.Windows.Forms.RichTextBox();
			this.tabCompilerCounters = new System.Windows.Forms.TabPage();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.label6 = new System.Windows.Forms.Label();
			this.tbCompilerCounterFilter = new System.Windows.Forms.TextBox();
			this.gridCompilerCounters = new System.Windows.Forms.DataGridView();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.tbCompilerCounters = new System.Windows.Forms.RichTextBox();
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
			this.tabTransforms.SuspendLayout();
			this.tabMethodCounters.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridMethodCounters)).BeginInit();
			this.tabPage2.SuspendLayout();
			this.tabLogs.SuspendLayout();
			this.tabCompilerCounters.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridCompilerCounters)).BeginInit();
			this.tabPage5.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 450);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			this.statusStrip1.Size = new System.Drawing.Size(940, 23);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 18);
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(233, 17);
			// 
			// toolStripStatusLabel
			// 
			this.toolStripStatusLabel.Name = "toolStripStatusLabel";
			this.toolStripStatusLabel.Size = new System.Drawing.Size(0, 18);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.displayOptionsToolStripMenuItem,
            this.advanceToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			this.menuStrip1.Size = new System.Drawing.Size(940, 24);
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
            this.cbInlineExplicit,
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
			this.cbEnableBasicOptimizations.Text = "Enable Basic Optimizations";
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
			// cbInlineExplicit
			// 
			this.cbInlineExplicit.Checked = true;
			this.cbInlineExplicit.CheckOnClick = true;
			this.cbInlineExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbInlineExplicit.Name = "cbInlineExplicit";
			this.cbInlineExplicit.Size = new System.Drawing.Size(293, 22);
			this.cbInlineExplicit.Text = "Enable Inlined Explicit Methods";
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
            this.dumpAllMethodStagesToolStripMenuItem,
            this.cbCILDecoderStageV2Testing});
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
			this.CBEnableMultithreading.Size = new System.Drawing.Size(234, 22);
			this.CBEnableMultithreading.Text = "Enable Multithreading";
			// 
			// cbEnableMethodScanner
			// 
			this.cbEnableMethodScanner.CheckOnClick = true;
			this.cbEnableMethodScanner.Name = "cbEnableMethodScanner";
			this.cbEnableMethodScanner.Size = new System.Drawing.Size(234, 22);
			this.cbEnableMethodScanner.Text = "Enable Method Scanner";
			// 
			// dumpAllMethodStagesToolStripMenuItem
			// 
			this.dumpAllMethodStagesToolStripMenuItem.Name = "dumpAllMethodStagesToolStripMenuItem";
			this.dumpAllMethodStagesToolStripMenuItem.Size = new System.Drawing.Size(234, 22);
			this.dumpAllMethodStagesToolStripMenuItem.Text = "Dump All Method Stages";
			this.dumpAllMethodStagesToolStripMenuItem.Click += new System.EventHandler(this.DumpAllMethodStagesToolStripMenuItem_Click);
			// 
			// cbCILDecoderStageV2Testing
			// 
			this.cbCILDecoderStageV2Testing.CheckOnClick = true;
			this.cbCILDecoderStageV2Testing.Name = "cbCILDecoderStageV2Testing";
			this.cbCILDecoderStageV2Testing.Size = new System.Drawing.Size(234, 22);
			this.cbCILDecoderStageV2Testing.Text = "CIL Decoder Stage V2 (Testing)";
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
			this.treeView.Size = new System.Drawing.Size(239, 365);
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
			this.splitContainer1.Size = new System.Drawing.Size(940, 396);
			this.splitContainer1.SplitterDistance = 244;
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
			this.tbFilter.Location = new System.Drawing.Point(45, 3);
			this.tbFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbFilter.Name = "tbFilter";
			this.tbFilter.Size = new System.Drawing.Size(192, 23);
			this.tbFilter.TabIndex = 4;
			this.tbFilter.TextChanged += new System.EventHandler(this.tbFilter_TextChanged);
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl.Controls.Add(this.tabStages);
			this.tabControl.Controls.Add(this.tabStageDebug);
			this.tabControl.Controls.Add(this.tabTransforms);
			this.tabControl.Controls.Add(this.tabMethodCounters);
			this.tabControl.Controls.Add(this.tabLogs);
			this.tabControl.Controls.Add(this.tabCompilerCounters);
			this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tabControl.Location = new System.Drawing.Point(1, 4);
			this.tabControl.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl.Name = "tabControl";
			this.tabControl.Padding = new System.Drawing.Point(0, 0);
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(691, 393);
			this.tabControl.TabIndex = 38;
			this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
			// 
			// tabStages
			// 
			this.tabStages.BackColor = System.Drawing.Color.Gainsboro;
			this.tabStages.Controls.Add(this.label1);
			this.tabStages.Controls.Add(this.cbInstructionLabels);
			this.tabStages.Controls.Add(this.cbInstructionStages);
			this.tabStages.Controls.Add(this.stageLabel);
			this.tabStages.Controls.Add(this.tbInstructions);
			this.tabStages.Location = new System.Drawing.Point(4, 28);
			this.tabStages.Margin = new System.Windows.Forms.Padding(0);
			this.tabStages.Name = "tabStages";
			this.tabStages.Size = new System.Drawing.Size(683, 361);
			this.tabStages.TabIndex = 0;
			this.tabStages.Text = "Instructions";
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
			// cbInstructionLabels
			// 
			this.cbInstructionLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbInstructionLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbInstructionLabels.FormattingEnabled = true;
			this.cbInstructionLabels.Location = new System.Drawing.Point(413, 8);
			this.cbInstructionLabels.Margin = new System.Windows.Forms.Padding(5);
			this.cbInstructionLabels.MaxDropDownItems = 20;
			this.cbInstructionLabels.Name = "cbInstructionLabels";
			this.cbInstructionLabels.Size = new System.Drawing.Size(122, 21);
			this.cbInstructionLabels.TabIndex = 40;
			this.cbInstructionLabels.SelectedIndexChanged += new System.EventHandler(this.cbInstructionLabels_SelectedIndexChanged);
			// 
			// cbInstructionStages
			// 
			this.cbInstructionStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbInstructionStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbInstructionStages.FormattingEnabled = true;
			this.cbInstructionStages.ItemHeight = 13;
			this.cbInstructionStages.Location = new System.Drawing.Point(64, 8);
			this.cbInstructionStages.Margin = new System.Windows.Forms.Padding(5);
			this.cbInstructionStages.MaxDropDownItems = 40;
			this.cbInstructionStages.Name = "cbInstructionStages";
			this.cbInstructionStages.Size = new System.Drawing.Size(282, 21);
			this.cbInstructionStages.TabIndex = 38;
			this.cbInstructionStages.SelectedIndexChanged += new System.EventHandler(this.cbInstructionStages_SelectedIndexChanged);
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
			// tbInstructions
			// 
			this.tbInstructions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbInstructions.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbInstructions.Location = new System.Drawing.Point(0, 37);
			this.tbInstructions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbInstructions.Name = "tbInstructions";
			this.tbInstructions.Size = new System.Drawing.Size(677, 324);
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
			this.tabStageDebug.Location = new System.Drawing.Point(4, 28);
			this.tabStageDebug.Margin = new System.Windows.Forms.Padding(0);
			this.tabStageDebug.Name = "tabStageDebug";
			this.tabStageDebug.Size = new System.Drawing.Size(683, 361);
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
			this.cbDebugStages.SelectedIndexChanged += new System.EventHandler(this.cbDebugStages_SelectedIndexChanged);
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
			// tbDebugResult
			// 
			this.tbDebugResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbDebugResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbDebugResult.Location = new System.Drawing.Point(0, 37);
			this.tbDebugResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbDebugResult.Name = "tbDebugResult";
			this.tbDebugResult.Size = new System.Drawing.Size(679, 331);
			this.tbDebugResult.TabIndex = 32;
			this.tbDebugResult.Text = "";
			this.tbDebugResult.WordWrap = false;
			// 
			// tabTransforms
			// 
			this.tabTransforms.BackColor = System.Drawing.Color.Gainsboro;
			this.tabTransforms.Controls.Add(this.label7);
			this.tabTransforms.Controls.Add(this.cbTransformLabels);
			this.tabTransforms.Controls.Add(this.cbTransformStages);
			this.tabTransforms.Controls.Add(this.label8);
			this.tabTransforms.Controls.Add(this.lbSteps);
			this.tabTransforms.Controls.Add(this.btnLast);
			this.tabTransforms.Controls.Add(this.btnNext);
			this.tabTransforms.Controls.Add(this.btnPrevious);
			this.tabTransforms.Controls.Add(this.btnFirst);
			this.tabTransforms.Controls.Add(this.tbTransforms);
			this.tabTransforms.Location = new System.Drawing.Point(4, 28);
			this.tabTransforms.Name = "tabTransforms";
			this.tabTransforms.Padding = new System.Windows.Forms.Padding(3);
			this.tabTransforms.Size = new System.Drawing.Size(683, 361);
			this.tabTransforms.TabIndex = 9;
			this.tabTransforms.Text = "Transforms";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label7.Location = new System.Drawing.Point(352, 9);
			this.label7.Margin = new System.Windows.Forms.Padding(5);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(56, 23);
			this.label7.TabIndex = 45;
			this.label7.Text = "Label:";
			// 
			// cbTransformLabels
			// 
			this.cbTransformLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTransformLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbTransformLabels.FormattingEnabled = true;
			this.cbTransformLabels.Location = new System.Drawing.Point(413, 8);
			this.cbTransformLabels.Margin = new System.Windows.Forms.Padding(5);
			this.cbTransformLabels.MaxDropDownItems = 20;
			this.cbTransformLabels.Name = "cbTransformLabels";
			this.cbTransformLabels.Size = new System.Drawing.Size(122, 21);
			this.cbTransformLabels.TabIndex = 44;
			this.cbTransformLabels.SelectedIndexChanged += new System.EventHandler(this.cbTransformLabels_SelectedIndexChanged);
			// 
			// cbTransformStages
			// 
			this.cbTransformStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbTransformStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbTransformStages.FormattingEnabled = true;
			this.cbTransformStages.ItemHeight = 13;
			this.cbTransformStages.Location = new System.Drawing.Point(64, 8);
			this.cbTransformStages.Margin = new System.Windows.Forms.Padding(5);
			this.cbTransformStages.MaxDropDownItems = 40;
			this.cbTransformStages.Name = "cbTransformStages";
			this.cbTransformStages.Size = new System.Drawing.Size(282, 21);
			this.cbTransformStages.TabIndex = 42;
			this.cbTransformStages.SelectedIndexChanged += new System.EventHandler(this.cbTransformStages_SelectedIndexChanged);
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label8.Location = new System.Drawing.Point(5, 9);
			this.label8.Margin = new System.Windows.Forms.Padding(5);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(58, 23);
			this.label8.TabIndex = 43;
			this.label8.Text = "Stage:";
			// 
			// lbSteps
			// 
			this.lbSteps.AutoSize = true;
			this.lbSteps.Location = new System.Drawing.Point(348, 39);
			this.lbSteps.Name = "lbSteps";
			this.lbSteps.Size = new System.Drawing.Size(52, 17);
			this.lbSteps.TabIndex = 37;
			this.lbSteps.Text = "## / ##";
			// 
			// btnLast
			// 
			this.btnLast.Location = new System.Drawing.Point(250, 36);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(75, 23);
			this.btnLast.TabIndex = 36;
			this.btnLast.Text = "Last";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(169, 36);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(75, 23);
			this.btnNext.TabIndex = 35;
			this.btnNext.Text = "Next";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnPrevious
			// 
			this.btnPrevious.Location = new System.Drawing.Point(88, 36);
			this.btnPrevious.Name = "btnPrevious";
			this.btnPrevious.Size = new System.Drawing.Size(75, 23);
			this.btnPrevious.TabIndex = 34;
			this.btnPrevious.Text = "Previous";
			this.btnPrevious.UseVisualStyleBackColor = true;
			this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
			// 
			// btnFirst
			// 
			this.btnFirst.Location = new System.Drawing.Point(7, 36);
			this.btnFirst.Name = "btnFirst";
			this.btnFirst.Size = new System.Drawing.Size(75, 23);
			this.btnFirst.TabIndex = 33;
			this.btnFirst.Text = "First";
			this.btnFirst.UseVisualStyleBackColor = true;
			this.btnFirst.Click += new System.EventHandler(this.btnFirst_Click);
			// 
			// tbTransforms
			// 
			this.tbTransforms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbTransforms.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbTransforms.Location = new System.Drawing.Point(0, 67);
			this.tbTransforms.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbTransforms.Name = "tbTransforms";
			this.tbTransforms.Size = new System.Drawing.Size(683, 298);
			this.tbTransforms.TabIndex = 32;
			this.tbTransforms.Text = "";
			this.tbTransforms.WordWrap = false;
			// 
			// tabMethodCounters
			// 
			this.tabMethodCounters.BackColor = System.Drawing.Color.Gainsboro;
			this.tabMethodCounters.Controls.Add(this.tabControl1);
			this.tabMethodCounters.Location = new System.Drawing.Point(4, 28);
			this.tabMethodCounters.Margin = new System.Windows.Forms.Padding(0);
			this.tabMethodCounters.Name = "tabMethodCounters";
			this.tabMethodCounters.Size = new System.Drawing.Size(683, 361);
			this.tabMethodCounters.TabIndex = 6;
			this.tabMethodCounters.Text = "Counters";
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(687, 368);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.tbCounterFilter);
			this.tabPage1.Controls.Add(this.gridMethodCounters);
			this.tabPage1.Location = new System.Drawing.Point(4, 28);
			this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage1.Size = new System.Drawing.Size(679, 336);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Grid";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(495, 13);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(43, 17);
			this.label5.TabIndex = 9;
			this.label5.Text = "Filter:";
			// 
			// tbCounterFilter
			// 
			this.tbCounterFilter.Location = new System.Drawing.Point(495, 31);
			this.tbCounterFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbCounterFilter.Name = "tbCounterFilter";
			this.tbCounterFilter.Size = new System.Drawing.Size(178, 23);
			this.tbCounterFilter.TabIndex = 8;
			this.tbCounterFilter.TextChanged += new System.EventHandler(this.tbMethodCounterFilter_TextChanged);
			// 
			// gridMethodCounters
			// 
			this.gridMethodCounters.AllowUserToAddRows = false;
			this.gridMethodCounters.AllowUserToDeleteRows = false;
			this.gridMethodCounters.AllowUserToOrderColumns = true;
			this.gridMethodCounters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridMethodCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridMethodCounters.Location = new System.Drawing.Point(3, 0);
			this.gridMethodCounters.Name = "gridMethodCounters";
			this.gridMethodCounters.ReadOnly = true;
			this.gridMethodCounters.RowHeadersVisible = false;
			this.gridMethodCounters.RowHeadersWidth = 51;
			this.gridMethodCounters.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.gridMethodCounters.RowTemplate.Height = 20;
			this.gridMethodCounters.ShowCellErrors = false;
			this.gridMethodCounters.ShowCellToolTips = false;
			this.gridMethodCounters.ShowEditingIcon = false;
			this.gridMethodCounters.ShowRowErrors = false;
			this.gridMethodCounters.Size = new System.Drawing.Size(481, 335);
			this.gridMethodCounters.TabIndex = 7;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.tbMethodCounters);
			this.tabPage2.Location = new System.Drawing.Point(4, 27);
			this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage2.Size = new System.Drawing.Size(679, 337);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Text";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// tbMethodCounters
			// 
			this.tbMethodCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMethodCounters.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbMethodCounters.Location = new System.Drawing.Point(0, 3);
			this.tbMethodCounters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbMethodCounters.Name = "tbMethodCounters";
			this.tbMethodCounters.Size = new System.Drawing.Size(676, 334);
			this.tbMethodCounters.TabIndex = 10;
			this.tbMethodCounters.Text = "";
			this.tbMethodCounters.WordWrap = false;
			// 
			// tabLogs
			// 
			this.tabLogs.BackColor = System.Drawing.Color.Gainsboro;
			this.tabLogs.Controls.Add(this.cbCompilerSections);
			this.tabLogs.Controls.Add(this.label4);
			this.tabLogs.Controls.Add(this.tbCompilerLogs);
			this.tabLogs.Location = new System.Drawing.Point(4, 28);
			this.tabLogs.Margin = new System.Windows.Forms.Padding(0);
			this.tabLogs.Name = "tabLogs";
			this.tabLogs.Size = new System.Drawing.Size(683, 361);
			this.tabLogs.TabIndex = 7;
			this.tabLogs.Text = "Compiler Logs";
			// 
			// cbCompilerSections
			// 
			this.cbCompilerSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbCompilerSections.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.cbCompilerSections.FormattingEnabled = true;
			this.cbCompilerSections.Location = new System.Drawing.Point(77, 8);
			this.cbCompilerSections.Margin = new System.Windows.Forms.Padding(5);
			this.cbCompilerSections.MaxDropDownItems = 20;
			this.cbCompilerSections.Name = "cbCompilerSections";
			this.cbCompilerSections.Size = new System.Drawing.Size(285, 21);
			this.cbCompilerSections.TabIndex = 44;
			this.cbCompilerSections.SelectedIndexChanged += new System.EventHandler(this.cbCompilerSections_SelectedIndexChanged);
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
			// tbCompilerLogs
			// 
			this.tbCompilerLogs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCompilerLogs.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbCompilerLogs.Location = new System.Drawing.Point(0, 37);
			this.tbCompilerLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbCompilerLogs.Name = "tbCompilerLogs";
			this.tbCompilerLogs.Size = new System.Drawing.Size(683, 324);
			this.tbCompilerLogs.TabIndex = 3;
			this.tbCompilerLogs.Text = "";
			this.tbCompilerLogs.WordWrap = false;
			// 
			// tabCompilerCounters
			// 
			this.tabCompilerCounters.Controls.Add(this.tabControl2);
			this.tabCompilerCounters.Location = new System.Drawing.Point(4, 28);
			this.tabCompilerCounters.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabCompilerCounters.Name = "tabCompilerCounters";
			this.tabCompilerCounters.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabCompilerCounters.Size = new System.Drawing.Size(683, 361);
			this.tabCompilerCounters.TabIndex = 8;
			this.tabCompilerCounters.Text = "Compiler Counters";
			this.tabCompilerCounters.UseVisualStyleBackColor = true;
			// 
			// tabControl2
			// 
			this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			this.tabControl2.Controls.Add(this.tabPage4);
			this.tabControl2.Controls.Add(this.tabPage5);
			this.tabControl2.Location = new System.Drawing.Point(0, 0);
			this.tabControl2.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl2.Multiline = true;
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(697, 365);
			this.tabControl2.TabIndex = 8;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.label6);
			this.tabPage4.Controls.Add(this.tbCompilerCounterFilter);
			this.tabPage4.Controls.Add(this.gridCompilerCounters);
			this.tabPage4.Location = new System.Drawing.Point(4, 28);
			this.tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage4.Size = new System.Drawing.Size(689, 333);
			this.tabPage4.TabIndex = 0;
			this.tabPage4.Text = "Grid";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(498, 15);
			this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(43, 17);
			this.label6.TabIndex = 9;
			this.label6.Text = "Filter:";
			// 
			// tbCompilerCounterFilter
			// 
			this.tbCompilerCounterFilter.Location = new System.Drawing.Point(495, 31);
			this.tbCompilerCounterFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbCompilerCounterFilter.Name = "tbCompilerCounterFilter";
			this.tbCompilerCounterFilter.Size = new System.Drawing.Size(178, 23);
			this.tbCompilerCounterFilter.TabIndex = 8;
			this.tbCompilerCounterFilter.TextChanged += new System.EventHandler(this.tbCompilerCounterFilter_TextChanged);
			// 
			// gridCompilerCounters
			// 
			this.gridCompilerCounters.AllowUserToAddRows = false;
			this.gridCompilerCounters.AllowUserToDeleteRows = false;
			this.gridCompilerCounters.AllowUserToOrderColumns = true;
			this.gridCompilerCounters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.gridCompilerCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.gridCompilerCounters.Location = new System.Drawing.Point(3, 0);
			this.gridCompilerCounters.Name = "gridCompilerCounters";
			this.gridCompilerCounters.ReadOnly = true;
			this.gridCompilerCounters.RowHeadersVisible = false;
			this.gridCompilerCounters.RowHeadersWidth = 51;
			this.gridCompilerCounters.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.gridCompilerCounters.RowTemplate.Height = 20;
			this.gridCompilerCounters.ShowCellErrors = false;
			this.gridCompilerCounters.ShowCellToolTips = false;
			this.gridCompilerCounters.ShowEditingIcon = false;
			this.gridCompilerCounters.ShowRowErrors = false;
			this.gridCompilerCounters.Size = new System.Drawing.Size(481, 333);
			this.gridCompilerCounters.TabIndex = 7;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.tbCompilerCounters);
			this.tabPage5.Location = new System.Drawing.Point(4, 27);
			this.tabPage5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPage5.Size = new System.Drawing.Size(689, 334);
			this.tabPage5.TabIndex = 1;
			this.tabPage5.Text = "Text";
			this.tabPage5.UseVisualStyleBackColor = true;
			// 
			// tbCompilerCounters
			// 
			this.tbCompilerCounters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbCompilerCounters.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.tbCompilerCounters.Location = new System.Drawing.Point(0, 0);
			this.tbCompilerCounters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tbCompilerCounters.Name = "tbCompilerCounters";
			this.tbCompilerCounters.Size = new System.Drawing.Size(686, 330);
			this.tbCompilerCounters.TabIndex = 10;
			this.tbCompilerCounters.Text = "";
			this.tbCompilerCounters.WordWrap = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbPlatform,
            this.toolStripSeparator3,
            this.toolStripButton1,
            this.toolStripSeparator2,
            this.toolStripButton4,
            this.toolStripSeparator1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(940, 27);
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
			this.cbPlatform.Size = new System.Drawing.Size(104, 27);
			this.cbPlatform.SelectedIndexChanged += new System.EventHandler(this.cbPlatform_SelectedIndexChanged);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(60, 24);
			this.toolStripButton1.Text = "Open";
			this.toolStripButton1.Click += new System.EventHandler(this.ToolStripButton1_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(76, 24);
			this.toolStripButton4.Text = "Compile";
			this.toolStripButton4.Click += new System.EventHandler(this.ToolStripButton4_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
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
			this.ClientSize = new System.Drawing.Size(940, 473);
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
			this.tabTransforms.ResumeLayout(false);
			this.tabTransforms.PerformLayout();
			this.tabMethodCounters.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridMethodCounters)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabLogs.ResumeLayout(false);
			this.tabCompilerCounters.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.gridCompilerCounters)).EndInit();
			this.tabPage5.ResumeLayout(false);
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
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ToolStripComboBox cbPlatform;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cbEnableBitTracker;
		private System.Windows.Forms.ToolStripMenuItem CBEnableMultithreading;
		private System.Windows.Forms.ToolStripMenuItem cbLoopInvariantCodeMotion;
		private System.Windows.Forms.ToolStripMenuItem cbPlatformOptimizations;
		private System.Windows.Forms.ToolStripMenuItem cbInlineExplicit;
		private System.Windows.Forms.ToolStripMenuItem cbEnableAllOptimizations;
		private System.Windows.Forms.ToolStripMenuItem cbDisableAllOptimizations;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem cbEnableDevirtualization;
		private System.Windows.Forms.ToolStripMenuItem cbCILDecoderStageV2Testing;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabStages;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox cbInstructionLabels;
		private System.Windows.Forms.ComboBox cbInstructionStages;
		private System.Windows.Forms.Label stageLabel;
		private System.Windows.Forms.RichTextBox tbInstructions;
		private System.Windows.Forms.TabPage tabStageDebug;
		private System.Windows.Forms.ComboBox cbDebugStages;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.RichTextBox tbDebugResult;
		private System.Windows.Forms.TabPage tabMethodCounters;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.DataGridView gridMethodCounters;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabLogs;
		private System.Windows.Forms.ComboBox cbCompilerSections;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.RichTextBox tbMethodCounters;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox tbCounterFilter;
		private System.Windows.Forms.TabPage tabCompilerCounters;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbCompilerCounterFilter;
		private System.Windows.Forms.DataGridView gridCompilerCounters;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.RichTextBox tbCompilerCounters;
		private System.Windows.Forms.TabPage tabTransforms;
		private System.Windows.Forms.RichTextBox tbTransforms;
		private System.Windows.Forms.Button btnLast;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.Label lbSteps;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbTransformLabels;
		private System.Windows.Forms.ComboBox cbTransformStages;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RichTextBox tbCompilerLogs;
	}
}
