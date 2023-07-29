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
			components = new System.ComponentModel.Container();
			var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			statusStrip1 = new System.Windows.Forms.StatusStrip();
			toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			nowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableAllOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			cbDisableAllOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			cbEnableSSA = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableBasicOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableValueNumbering = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableSparseConditionalConstantPropagation = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableDevirtualization = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableInline = new System.Windows.Forms.ToolStripMenuItem();
			cbInlineExplicit = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableLongExpansion = new System.Windows.Forms.ToolStripMenuItem();
			cbLoopInvariantCodeMotion = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableBitTracker = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableTwoPassOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			cbPlatformOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableBinaryCodeGeneration = new System.Windows.Forms.ToolStripMenuItem();
			displayOptionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			showOperandTypes = new System.Windows.Forms.ToolStripMenuItem();
			padInstructions = new System.Windows.Forms.ToolStripMenuItem();
			showSizes = new System.Windows.Forms.ToolStripMenuItem();
			removeIRNop = new System.Windows.Forms.ToolStripMenuItem();
			advanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableMultithreading = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableMethodScanner = new System.Windows.Forms.ToolStripMenuItem();
			cbEnableDebugDiagnostic = new System.Windows.Forms.ToolStripMenuItem();
			cbDumpAllMethodStages = new System.Windows.Forms.ToolStripMenuItem();
			openFileDialog = new System.Windows.Forms.OpenFileDialog();
			treeView = new System.Windows.Forms.TreeView();
			splitContainer1 = new System.Windows.Forms.SplitContainer();
			label2 = new System.Windows.Forms.Label();
			tbFilter = new System.Windows.Forms.TextBox();
			tabControl = new System.Windows.Forms.TabControl();
			tabStages = new System.Windows.Forms.TabPage();
			btnSaveB = new System.Windows.Forms.Button();
			btnSaveA = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			cbInstructionLabels = new System.Windows.Forms.ComboBox();
			cbInstructionStages = new System.Windows.Forms.ComboBox();
			stageLabel = new System.Windows.Forms.Label();
			tbInstructions = new System.Windows.Forms.RichTextBox();
			tabStageDebug = new System.Windows.Forms.TabPage();
			cbGraphviz = new System.Windows.Forms.CheckBox();
			cbDebugStages = new System.Windows.Forms.ComboBox();
			label3 = new System.Windows.Forms.Label();
			tbDebugResult = new System.Windows.Forms.RichTextBox();
			panel1 = new System.Windows.Forms.Panel();
			tabTransforms = new System.Windows.Forms.TabPage();
			splitContainer2 = new System.Windows.Forms.SplitContainer();
			lbSteps = new System.Windows.Forms.Label();
			btnLast = new System.Windows.Forms.Button();
			btnNext = new System.Windows.Forms.Button();
			btnPrevious = new System.Windows.Forms.Button();
			btnFirst = new System.Windows.Forms.Button();
			tbTransforms = new System.Windows.Forms.RichTextBox();
			cbSetBlock = new System.Windows.Forms.CheckBox();
			dataGridView1 = new System.Windows.Forms.DataGridView();
			label7 = new System.Windows.Forms.Label();
			cbTransformLabels = new System.Windows.Forms.ComboBox();
			cbTransformStages = new System.Windows.Forms.ComboBox();
			label8 = new System.Windows.Forms.Label();
			tabMethodCounters = new System.Windows.Forms.TabPage();
			tabControl1 = new System.Windows.Forms.TabControl();
			tabPage1 = new System.Windows.Forms.TabPage();
			label5 = new System.Windows.Forms.Label();
			tbCounterFilter = new System.Windows.Forms.TextBox();
			gridMethodCounters = new System.Windows.Forms.DataGridView();
			tabPage2 = new System.Windows.Forms.TabPage();
			tbMethodCounters = new System.Windows.Forms.RichTextBox();
			tabLogs = new System.Windows.Forms.TabPage();
			cbCompilerSections = new System.Windows.Forms.ComboBox();
			label4 = new System.Windows.Forms.Label();
			tbCompilerLogs = new System.Windows.Forms.RichTextBox();
			tabCompilerCounters = new System.Windows.Forms.TabPage();
			tabControl2 = new System.Windows.Forms.TabControl();
			tabPage4 = new System.Windows.Forms.TabPage();
			label6 = new System.Windows.Forms.Label();
			tbCompilerCounterFilter = new System.Windows.Forms.TextBox();
			gridCompilerCounters = new System.Windows.Forms.DataGridView();
			tabPage5 = new System.Windows.Forms.TabPage();
			tbCompilerCounters = new System.Windows.Forms.RichTextBox();
			toolStrip1 = new System.Windows.Forms.ToolStrip();
			cbPlatform = new System.Windows.Forms.ToolStripComboBox();
			toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			tsbOpen = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			tsbRefresh = new System.Windows.Forms.ToolStripButton();
			toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			tsbCompile = new System.Windows.Forms.ToolStripButton();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			timer1 = new System.Windows.Forms.Timer(components);
			saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			statusStrip1.SuspendLayout();
			menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
			splitContainer1.Panel1.SuspendLayout();
			splitContainer1.Panel2.SuspendLayout();
			splitContainer1.SuspendLayout();
			tabControl.SuspendLayout();
			tabStages.SuspendLayout();
			tabStageDebug.SuspendLayout();
			tabTransforms.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
			splitContainer2.Panel1.SuspendLayout();
			splitContainer2.Panel2.SuspendLayout();
			splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
			tabMethodCounters.SuspendLayout();
			tabControl1.SuspendLayout();
			tabPage1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)gridMethodCounters).BeginInit();
			tabPage2.SuspendLayout();
			tabLogs.SuspendLayout();
			tabCompilerCounters.SuspendLayout();
			tabControl2.SuspendLayout();
			tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)gridCompilerCounters).BeginInit();
			tabPage5.SuspendLayout();
			toolStrip1.SuspendLayout();
			SuspendLayout();
			// 
			// statusStrip1
			// 
			statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1, toolStripProgressBar1, toolStripStatusLabel });
			statusStrip1.Location = new System.Drawing.Point(0, 450);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
			statusStrip1.Size = new System.Drawing.Size(940, 23);
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new System.Drawing.Size(0, 18);
			// 
			// toolStripProgressBar1
			// 
			toolStripProgressBar1.Name = "toolStripProgressBar1";
			toolStripProgressBar1.Size = new System.Drawing.Size(233, 17);
			// 
			// toolStripStatusLabel
			// 
			toolStripStatusLabel.Name = "toolStripStatusLabel";
			toolStripStatusLabel.Size = new System.Drawing.Size(0, 18);
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem, compileToolStripMenuItem, optionsToolStripMenuItem, displayOptionsToolStripMenuItem, advanceToolStripMenuItem });
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
			menuStrip1.Size = new System.Drawing.Size(940, 24);
			menuStrip1.TabIndex = 3;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { openToolStripMenuItem, toolStripMenuItem1, quitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			openToolStripMenuItem.Text = "&Open";
			openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
			// 
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new System.Drawing.Size(100, 6);
			// 
			// quitToolStripMenuItem
			// 
			quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			quitToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			quitToolStripMenuItem.Text = "&Quit";
			quitToolStripMenuItem.Click += QuitToolStripMenuItem_Click;
			// 
			// compileToolStripMenuItem
			// 
			compileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { nowToolStripMenuItem });
			compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			compileToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
			compileToolStripMenuItem.Text = "Compile";
			// 
			// nowToolStripMenuItem
			// 
			nowToolStripMenuItem.Name = "nowToolStripMenuItem";
			nowToolStripMenuItem.Size = new System.Drawing.Size(99, 22);
			nowToolStripMenuItem.Text = "Now";
			nowToolStripMenuItem.Click += NowToolStripMenuItem_Click;
			// 
			// optionsToolStripMenuItem
			// 
			optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { cbEnableAllOptimizations, cbDisableAllOptimizations, toolStripSeparator4, cbEnableSSA, cbEnableBasicOptimizations, cbEnableValueNumbering, cbEnableSparseConditionalConstantPropagation, cbEnableDevirtualization, cbEnableInline, cbInlineExplicit, cbEnableLongExpansion, cbLoopInvariantCodeMotion, cbEnableBitTracker, cbEnableTwoPassOptimizations, cbPlatformOptimizations, cbEnableBinaryCodeGeneration });
			optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			optionsToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
			optionsToolStripMenuItem.Text = "Optimizations";
			// 
			// cbEnableAllOptimizations
			// 
			cbEnableAllOptimizations.Name = "cbEnableAllOptimizations";
			cbEnableAllOptimizations.Size = new System.Drawing.Size(293, 22);
			cbEnableAllOptimizations.Text = "Enable All";
			cbEnableAllOptimizations.Click += cbEnableAllOptimizations_Click;
			// 
			// cbDisableAllOptimizations
			// 
			cbDisableAllOptimizations.Name = "cbDisableAllOptimizations";
			cbDisableAllOptimizations.Size = new System.Drawing.Size(293, 22);
			cbDisableAllOptimizations.Text = "Disable All";
			cbDisableAllOptimizations.Click += cbDisableAllOptimizations_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new System.Drawing.Size(290, 6);
			// 
			// cbEnableSSA
			// 
			cbEnableSSA.Checked = true;
			cbEnableSSA.CheckOnClick = true;
			cbEnableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableSSA.Name = "cbEnableSSA";
			cbEnableSSA.Size = new System.Drawing.Size(293, 22);
			cbEnableSSA.Text = "Enable SSA";
			// 
			// cbEnableBasicOptimizations
			// 
			cbEnableBasicOptimizations.Checked = true;
			cbEnableBasicOptimizations.CheckOnClick = true;
			cbEnableBasicOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableBasicOptimizations.Name = "cbEnableBasicOptimizations";
			cbEnableBasicOptimizations.Size = new System.Drawing.Size(293, 22);
			cbEnableBasicOptimizations.Text = "Enable Basic Optimizations";
			// 
			// cbEnableValueNumbering
			// 
			cbEnableValueNumbering.Checked = true;
			cbEnableValueNumbering.CheckOnClick = true;
			cbEnableValueNumbering.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableValueNumbering.Name = "cbEnableValueNumbering";
			cbEnableValueNumbering.Size = new System.Drawing.Size(293, 22);
			cbEnableValueNumbering.Text = "Enable Value Numbering";
			// 
			// cbEnableSparseConditionalConstantPropagation
			// 
			cbEnableSparseConditionalConstantPropagation.Checked = true;
			cbEnableSparseConditionalConstantPropagation.CheckOnClick = true;
			cbEnableSparseConditionalConstantPropagation.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableSparseConditionalConstantPropagation.Name = "cbEnableSparseConditionalConstantPropagation";
			cbEnableSparseConditionalConstantPropagation.Size = new System.Drawing.Size(293, 22);
			cbEnableSparseConditionalConstantPropagation.Text = "Enable Conditional Constant Propagation";
			// 
			// cbEnableDevirtualization
			// 
			cbEnableDevirtualization.Checked = true;
			cbEnableDevirtualization.CheckOnClick = true;
			cbEnableDevirtualization.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableDevirtualization.Name = "cbEnableDevirtualization";
			cbEnableDevirtualization.Size = new System.Drawing.Size(293, 22);
			cbEnableDevirtualization.Text = "Enable Devirtualization";
			// 
			// cbEnableInline
			// 
			cbEnableInline.Checked = true;
			cbEnableInline.CheckOnClick = true;
			cbEnableInline.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableInline.Name = "cbEnableInline";
			cbEnableInline.Size = new System.Drawing.Size(293, 22);
			cbEnableInline.Text = "Enable Inlined Methods";
			// 
			// cbInlineExplicit
			// 
			cbInlineExplicit.Checked = true;
			cbInlineExplicit.CheckOnClick = true;
			cbInlineExplicit.CheckState = System.Windows.Forms.CheckState.Checked;
			cbInlineExplicit.Name = "cbInlineExplicit";
			cbInlineExplicit.Size = new System.Drawing.Size(293, 22);
			cbInlineExplicit.Text = "Enable Inlined Explicit Methods";
			// 
			// cbEnableLongExpansion
			// 
			cbEnableLongExpansion.Checked = true;
			cbEnableLongExpansion.CheckOnClick = true;
			cbEnableLongExpansion.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableLongExpansion.Name = "cbEnableLongExpansion";
			cbEnableLongExpansion.Size = new System.Drawing.Size(293, 22);
			cbEnableLongExpansion.Text = "Enable Long Expansion";
			// 
			// cbLoopInvariantCodeMotion
			// 
			cbLoopInvariantCodeMotion.Checked = true;
			cbLoopInvariantCodeMotion.CheckOnClick = true;
			cbLoopInvariantCodeMotion.CheckState = System.Windows.Forms.CheckState.Checked;
			cbLoopInvariantCodeMotion.Name = "cbLoopInvariantCodeMotion";
			cbLoopInvariantCodeMotion.Size = new System.Drawing.Size(293, 22);
			cbLoopInvariantCodeMotion.Text = "Enable Loop Invariant Code Motion";
			// 
			// cbEnableBitTracker
			// 
			cbEnableBitTracker.Checked = true;
			cbEnableBitTracker.CheckOnClick = true;
			cbEnableBitTracker.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableBitTracker.Name = "cbEnableBitTracker";
			cbEnableBitTracker.Size = new System.Drawing.Size(293, 22);
			cbEnableBitTracker.Text = "Enable Bit Tracker";
			// 
			// cbEnableTwoPassOptimizations
			// 
			cbEnableTwoPassOptimizations.Checked = true;
			cbEnableTwoPassOptimizations.CheckOnClick = true;
			cbEnableTwoPassOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableTwoPassOptimizations.Name = "cbEnableTwoPassOptimizations";
			cbEnableTwoPassOptimizations.Size = new System.Drawing.Size(293, 22);
			cbEnableTwoPassOptimizations.Text = "Enable Two Optimization Passes";
			// 
			// cbPlatformOptimizations
			// 
			cbPlatformOptimizations.Checked = true;
			cbPlatformOptimizations.CheckOnClick = true;
			cbPlatformOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			cbPlatformOptimizations.Name = "cbPlatformOptimizations";
			cbPlatformOptimizations.Size = new System.Drawing.Size(293, 22);
			cbPlatformOptimizations.Text = "Enable Platform Optimizations";
			// 
			// cbEnableBinaryCodeGeneration
			// 
			cbEnableBinaryCodeGeneration.Checked = true;
			cbEnableBinaryCodeGeneration.CheckOnClick = true;
			cbEnableBinaryCodeGeneration.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableBinaryCodeGeneration.Name = "cbEnableBinaryCodeGeneration";
			cbEnableBinaryCodeGeneration.Size = new System.Drawing.Size(293, 22);
			cbEnableBinaryCodeGeneration.Text = "Enable Binary Code Generation";
			// 
			// displayOptionsToolStripMenuItem
			// 
			displayOptionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showOperandTypes, padInstructions, showSizes, removeIRNop });
			displayOptionsToolStripMenuItem.Name = "displayOptionsToolStripMenuItem";
			displayOptionsToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
			displayOptionsToolStripMenuItem.Text = "Display";
			// 
			// showOperandTypes
			// 
			showOperandTypes.CheckOnClick = true;
			showOperandTypes.Name = "showOperandTypes";
			showOperandTypes.Size = new System.Drawing.Size(184, 22);
			showOperandTypes.Text = "Show Operand Types";
			showOperandTypes.CheckStateChanged += DisplayCheckStateChanged;
			// 
			// padInstructions
			// 
			padInstructions.Checked = true;
			padInstructions.CheckOnClick = true;
			padInstructions.CheckState = System.Windows.Forms.CheckState.Checked;
			padInstructions.Name = "padInstructions";
			padInstructions.Size = new System.Drawing.Size(184, 22);
			padInstructions.Text = "Pad Instructions";
			padInstructions.CheckStateChanged += DisplayCheckStateChanged;
			// 
			// showSizes
			// 
			showSizes.Checked = true;
			showSizes.CheckOnClick = true;
			showSizes.CheckState = System.Windows.Forms.CheckState.Checked;
			showSizes.Name = "showSizes";
			showSizes.Size = new System.Drawing.Size(184, 22);
			showSizes.Text = "Show Sizes";
			showSizes.CheckStateChanged += DisplayCheckStateChanged;
			showSizes.Click += showSizesToolStripMenuItem_Click;
			// 
			// removeIRNop
			// 
			removeIRNop.CheckOnClick = true;
			removeIRNop.Name = "removeIRNop";
			removeIRNop.Size = new System.Drawing.Size(184, 22);
			removeIRNop.Text = "Remove IR.Nop";
			removeIRNop.CheckStateChanged += DisplayCheckStateChanged;
			// 
			// advanceToolStripMenuItem
			// 
			advanceToolStripMenuItem.CheckOnClick = true;
			advanceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { cbEnableMultithreading, cbEnableMethodScanner, cbEnableDebugDiagnostic, cbDumpAllMethodStages });
			advanceToolStripMenuItem.Name = "advanceToolStripMenuItem";
			advanceToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
			advanceToolStripMenuItem.Text = "Advance";
			// 
			// cbEnableMultithreading
			// 
			cbEnableMultithreading.Checked = true;
			cbEnableMultithreading.CheckOnClick = true;
			cbEnableMultithreading.CheckState = System.Windows.Forms.CheckState.Checked;
			cbEnableMultithreading.Name = "cbEnableMultithreading";
			cbEnableMultithreading.Size = new System.Drawing.Size(206, 22);
			cbEnableMultithreading.Text = "Enable Multithreading";
			// 
			// cbEnableMethodScanner
			// 
			cbEnableMethodScanner.CheckOnClick = true;
			cbEnableMethodScanner.Name = "cbEnableMethodScanner";
			cbEnableMethodScanner.Size = new System.Drawing.Size(206, 22);
			cbEnableMethodScanner.Text = "Enable Method Scanner";
			// 
			// cbEnableDebugDiagnostic
			// 
			cbEnableDebugDiagnostic.CheckOnClick = true;
			cbEnableDebugDiagnostic.Name = "cbEnableDebugDiagnostic";
			cbEnableDebugDiagnostic.Size = new System.Drawing.Size(206, 22);
			cbEnableDebugDiagnostic.Text = "Enable Debug Diagnostic";
			// 
			// cbDumpAllMethodStages
			// 
			cbDumpAllMethodStages.Name = "cbDumpAllMethodStages";
			cbDumpAllMethodStages.Size = new System.Drawing.Size(206, 22);
			cbDumpAllMethodStages.Text = "Dump All Method Stages";
			cbDumpAllMethodStages.Click += DumpAllMethodStagesToolStripMenuItem_Click;
			// 
			// openFileDialog
			// 
			openFileDialog.DefaultExt = "exe";
			openFileDialog.Filter = "Executable|*.exe|Library|*.dll|All Files|*.*";
			openFileDialog.FilterIndex = 2;
			// 
			// treeView
			// 
			treeView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			treeView.Location = new System.Drawing.Point(4, 31);
			treeView.Margin = new System.Windows.Forms.Padding(0);
			treeView.Name = "treeView";
			treeView.Size = new System.Drawing.Size(239, 365);
			treeView.TabIndex = 3;
			treeView.BeforeSelect += treeView_BeforeSelect;
			treeView.AfterSelect += TreeView_AfterSelect;
			treeView.NodeMouseDoubleClick += treeView_NodeMouseDoubleClick;
			// 
			// splitContainer1
			// 
			splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			splitContainer1.Location = new System.Drawing.Point(0, 54);
			splitContainer1.Margin = new System.Windows.Forms.Padding(0);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
			splitContainer1.Panel1.Controls.Add(label2);
			splitContainer1.Panel1.Controls.Add(tbFilter);
			splitContainer1.Panel1.Controls.Add(treeView);
			splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(tabControl);
			splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			splitContainer1.Size = new System.Drawing.Size(940, 396);
			splitContainer1.SplitterDistance = 244;
			splitContainer1.TabIndex = 26;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(4, 9);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(36, 15);
			label2.TabIndex = 5;
			label2.Text = "Filter:";
			// 
			// tbFilter
			// 
			tbFilter.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbFilter.Location = new System.Drawing.Point(45, 5);
			tbFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbFilter.Name = "tbFilter";
			tbFilter.Size = new System.Drawing.Size(192, 23);
			tbFilter.TabIndex = 4;
			tbFilter.TextChanged += tbFilter_TextChanged;
			// 
			// tabControl
			// 
			tabControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tabControl.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			tabControl.Controls.Add(tabStages);
			tabControl.Controls.Add(tabStageDebug);
			tabControl.Controls.Add(tabTransforms);
			tabControl.Controls.Add(tabMethodCounters);
			tabControl.Controls.Add(tabLogs);
			tabControl.Controls.Add(tabCompilerCounters);
			tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tabControl.Location = new System.Drawing.Point(1, 3);
			tabControl.Margin = new System.Windows.Forms.Padding(0);
			tabControl.Name = "tabControl";
			tabControl.Padding = new System.Drawing.Point(0, 0);
			tabControl.SelectedIndex = 0;
			tabControl.Size = new System.Drawing.Size(691, 393);
			tabControl.TabIndex = 38;
			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
			// 
			// tabStages
			// 
			tabStages.BackColor = System.Drawing.Color.Gainsboro;
			tabStages.Controls.Add(btnSaveB);
			tabStages.Controls.Add(btnSaveA);
			tabStages.Controls.Add(label1);
			tabStages.Controls.Add(cbInstructionLabels);
			tabStages.Controls.Add(cbInstructionStages);
			tabStages.Controls.Add(stageLabel);
			tabStages.Controls.Add(tbInstructions);
			tabStages.Location = new System.Drawing.Point(4, 28);
			tabStages.Margin = new System.Windows.Forms.Padding(0);
			tabStages.Name = "tabStages";
			tabStages.Size = new System.Drawing.Size(683, 361);
			tabStages.TabIndex = 0;
			tabStages.Text = "Instructions";
			// 
			// btnSaveB
			// 
			btnSaveB.Location = new System.Drawing.Point(613, 6);
			btnSaveB.Name = "btnSaveB";
			btnSaveB.Size = new System.Drawing.Size(64, 23);
			btnSaveB.TabIndex = 43;
			btnSaveB.Text = "Save B";
			btnSaveB.UseVisualStyleBackColor = true;
			btnSaveB.Click += btnSaveB_Click;
			// 
			// btnSaveA
			// 
			btnSaveA.Location = new System.Drawing.Point(543, 6);
			btnSaveA.Name = "btnSaveA";
			btnSaveA.Size = new System.Drawing.Size(64, 23);
			btnSaveA.TabIndex = 42;
			btnSaveA.Text = "Save A";
			btnSaveA.UseVisualStyleBackColor = true;
			btnSaveA.Click += btnSaveA_Click;
			// 
			// label1
			// 
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label1.Location = new System.Drawing.Point(352, 9);
			label1.Margin = new System.Windows.Forms.Padding(5);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(56, 23);
			label1.TabIndex = 41;
			label1.Text = "Block:";
			// 
			// cbInstructionLabels
			// 
			cbInstructionLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbInstructionLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			cbInstructionLabels.FormattingEnabled = true;
			cbInstructionLabels.Location = new System.Drawing.Point(413, 8);
			cbInstructionLabels.Margin = new System.Windows.Forms.Padding(5);
			cbInstructionLabels.MaxDropDownItems = 20;
			cbInstructionLabels.Name = "cbInstructionLabels";
			cbInstructionLabels.Size = new System.Drawing.Size(122, 21);
			cbInstructionLabels.TabIndex = 40;
			cbInstructionLabels.SelectedIndexChanged += cbInstructionLabels_SelectedIndexChanged;
			// 
			// cbInstructionStages
			// 
			cbInstructionStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbInstructionStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			cbInstructionStages.FormattingEnabled = true;
			cbInstructionStages.ItemHeight = 13;
			cbInstructionStages.Location = new System.Drawing.Point(64, 8);
			cbInstructionStages.Margin = new System.Windows.Forms.Padding(5);
			cbInstructionStages.MaxDropDownItems = 40;
			cbInstructionStages.Name = "cbInstructionStages";
			cbInstructionStages.Size = new System.Drawing.Size(282, 21);
			cbInstructionStages.TabIndex = 38;
			cbInstructionStages.SelectedIndexChanged += cbInstructionStages_SelectedIndexChanged;
			// 
			// stageLabel
			// 
			stageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			stageLabel.Location = new System.Drawing.Point(5, 9);
			stageLabel.Margin = new System.Windows.Forms.Padding(5);
			stageLabel.Name = "stageLabel";
			stageLabel.Size = new System.Drawing.Size(58, 23);
			stageLabel.TabIndex = 39;
			stageLabel.Text = "Stage:";
			// 
			// tbInstructions
			// 
			tbInstructions.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbInstructions.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tbInstructions.Location = new System.Drawing.Point(0, 37);
			tbInstructions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbInstructions.Name = "tbInstructions";
			tbInstructions.Size = new System.Drawing.Size(677, 324);
			tbInstructions.TabIndex = 31;
			tbInstructions.Text = "";
			tbInstructions.WordWrap = false;
			// 
			// tabStageDebug
			// 
			tabStageDebug.BackColor = System.Drawing.Color.Gainsboro;
			tabStageDebug.Controls.Add(cbGraphviz);
			tabStageDebug.Controls.Add(cbDebugStages);
			tabStageDebug.Controls.Add(label3);
			tabStageDebug.Controls.Add(tbDebugResult);
			tabStageDebug.Controls.Add(panel1);
			tabStageDebug.Location = new System.Drawing.Point(4, 28);
			tabStageDebug.Margin = new System.Windows.Forms.Padding(0);
			tabStageDebug.Name = "tabStageDebug";
			tabStageDebug.Size = new System.Drawing.Size(683, 361);
			tabStageDebug.TabIndex = 1;
			tabStageDebug.Text = "Debug";
			// 
			// cbGraphviz
			// 
			cbGraphviz.AutoSize = true;
			cbGraphviz.Location = new System.Drawing.Point(523, 10);
			cbGraphviz.Name = "cbGraphviz";
			cbGraphviz.Size = new System.Drawing.Size(73, 21);
			cbGraphviz.TabIndex = 44;
			cbGraphviz.Text = "Display";
			cbGraphviz.UseVisualStyleBackColor = true;
			cbGraphviz.CheckedChanged += cbGraphviz_CheckedChanged;
			// 
			// cbDebugStages
			// 
			cbDebugStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbDebugStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			cbDebugStages.FormattingEnabled = true;
			cbDebugStages.Location = new System.Drawing.Point(64, 8);
			cbDebugStages.Margin = new System.Windows.Forms.Padding(5);
			cbDebugStages.MaxDropDownItems = 20;
			cbDebugStages.Name = "cbDebugStages";
			cbDebugStages.Size = new System.Drawing.Size(451, 21);
			cbDebugStages.TabIndex = 40;
			cbDebugStages.SelectedIndexChanged += cbDebugStages_SelectedIndexChanged;
			// 
			// label3
			// 
			label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label3.Location = new System.Drawing.Point(5, 9);
			label3.Margin = new System.Windows.Forms.Padding(5);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(58, 23);
			label3.TabIndex = 41;
			label3.Text = "Stage:";
			// 
			// tbDebugResult
			// 
			tbDebugResult.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbDebugResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tbDebugResult.Location = new System.Drawing.Point(0, 37);
			tbDebugResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbDebugResult.Name = "tbDebugResult";
			tbDebugResult.Size = new System.Drawing.Size(679, 331);
			tbDebugResult.TabIndex = 32;
			tbDebugResult.Text = "";
			tbDebugResult.WordWrap = false;
			// 
			// panel1
			// 
			panel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			panel1.AutoScroll = true;
			panel1.Location = new System.Drawing.Point(5, 40);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(674, 321);
			panel1.TabIndex = 45;
			// 
			// tabTransforms
			// 
			tabTransforms.BackColor = System.Drawing.Color.Gainsboro;
			tabTransforms.Controls.Add(splitContainer2);
			tabTransforms.Controls.Add(label7);
			tabTransforms.Controls.Add(cbTransformLabels);
			tabTransforms.Controls.Add(cbTransformStages);
			tabTransforms.Controls.Add(label8);
			tabTransforms.Location = new System.Drawing.Point(4, 28);
			tabTransforms.Name = "tabTransforms";
			tabTransforms.Padding = new System.Windows.Forms.Padding(3);
			tabTransforms.Size = new System.Drawing.Size(683, 361);
			tabTransforms.TabIndex = 9;
			tabTransforms.Text = "Transforms";
			// 
			// splitContainer2
			// 
			splitContainer2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			splitContainer2.Location = new System.Drawing.Point(0, 34);
			splitContainer2.Margin = new System.Windows.Forms.Padding(0);
			splitContainer2.Name = "splitContainer2";
			// 
			// splitContainer2.Panel1
			// 
			splitContainer2.Panel1.Controls.Add(lbSteps);
			splitContainer2.Panel1.Controls.Add(btnLast);
			splitContainer2.Panel1.Controls.Add(btnNext);
			splitContainer2.Panel1.Controls.Add(btnPrevious);
			splitContainer2.Panel1.Controls.Add(btnFirst);
			splitContainer2.Panel1.Controls.Add(tbTransforms);
			// 
			// splitContainer2.Panel2
			// 
			splitContainer2.Panel2.Controls.Add(cbSetBlock);
			splitContainer2.Panel2.Controls.Add(dataGridView1);
			splitContainer2.Size = new System.Drawing.Size(683, 327);
			splitContainer2.SplitterDistance = 424;
			splitContainer2.TabIndex = 48;
			// 
			// lbSteps
			// 
			lbSteps.AutoSize = true;
			lbSteps.Location = new System.Drawing.Point(348, 7);
			lbSteps.Name = "lbSteps";
			lbSteps.Size = new System.Drawing.Size(52, 17);
			lbSteps.TabIndex = 42;
			lbSteps.Text = "## / ##";
			// 
			// btnLast
			// 
			btnLast.Location = new System.Drawing.Point(250, 4);
			btnLast.Name = "btnLast";
			btnLast.Size = new System.Drawing.Size(75, 23);
			btnLast.TabIndex = 41;
			btnLast.Text = "Last";
			btnLast.UseVisualStyleBackColor = true;
			// 
			// btnNext
			// 
			btnNext.Location = new System.Drawing.Point(169, 4);
			btnNext.Name = "btnNext";
			btnNext.Size = new System.Drawing.Size(75, 23);
			btnNext.TabIndex = 40;
			btnNext.Text = "Next";
			btnNext.UseVisualStyleBackColor = true;
			// 
			// btnPrevious
			// 
			btnPrevious.Location = new System.Drawing.Point(88, 4);
			btnPrevious.Name = "btnPrevious";
			btnPrevious.Size = new System.Drawing.Size(75, 23);
			btnPrevious.TabIndex = 39;
			btnPrevious.Text = "Previous";
			btnPrevious.UseVisualStyleBackColor = true;
			// 
			// btnFirst
			// 
			btnFirst.Location = new System.Drawing.Point(7, 4);
			btnFirst.Name = "btnFirst";
			btnFirst.Size = new System.Drawing.Size(75, 23);
			btnFirst.TabIndex = 38;
			btnFirst.Text = "First";
			btnFirst.UseVisualStyleBackColor = true;
			// 
			// tbTransforms
			// 
			tbTransforms.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbTransforms.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tbTransforms.Location = new System.Drawing.Point(0, 34);
			tbTransforms.Margin = new System.Windows.Forms.Padding(0);
			tbTransforms.Name = "tbTransforms";
			tbTransforms.Size = new System.Drawing.Size(416, 287);
			tbTransforms.TabIndex = 33;
			tbTransforms.Text = "";
			tbTransforms.WordWrap = false;
			// 
			// cbSetBlock
			// 
			cbSetBlock.AutoSize = true;
			cbSetBlock.Location = new System.Drawing.Point(5, 6);
			cbSetBlock.Name = "cbSetBlock";
			cbSetBlock.Size = new System.Drawing.Size(86, 21);
			cbSetBlock.TabIndex = 50;
			cbSetBlock.Text = "Set Block";
			cbSetBlock.UseVisualStyleBackColor = true;
			cbSetBlock.CheckedChanged += cbSetBlock_CheckedChanged;
			// 
			// dataGridView1
			// 
			dataGridView1.AllowUserToAddRows = false;
			dataGridView1.AllowUserToDeleteRows = false;
			dataGridView1.AllowUserToResizeRows = false;
			dataGridView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Location = new System.Drawing.Point(0, 34);
			dataGridView1.Margin = new System.Windows.Forms.Padding(0);
			dataGridView1.MultiSelect = false;
			dataGridView1.Name = "dataGridView1";
			dataGridView1.ReadOnly = true;
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			dataGridView1.RowTemplate.Height = 18;
			dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.Size = new System.Drawing.Size(253, 290);
			dataGridView1.TabIndex = 48;
			dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
			// 
			// label7
			// 
			label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label7.Location = new System.Drawing.Point(352, 9);
			label7.Margin = new System.Windows.Forms.Padding(5);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(56, 23);
			label7.TabIndex = 45;
			label7.Text = "Block:";
			// 
			// cbTransformLabels
			// 
			cbTransformLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbTransformLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			cbTransformLabels.FormattingEnabled = true;
			cbTransformLabels.Location = new System.Drawing.Point(413, 8);
			cbTransformLabels.Margin = new System.Windows.Forms.Padding(5);
			cbTransformLabels.MaxDropDownItems = 20;
			cbTransformLabels.Name = "cbTransformLabels";
			cbTransformLabels.Size = new System.Drawing.Size(122, 21);
			cbTransformLabels.TabIndex = 44;
			cbTransformLabels.SelectedIndexChanged += cbTransformLabels_SelectedIndexChanged;
			// 
			// cbTransformStages
			// 
			cbTransformStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbTransformStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			cbTransformStages.FormattingEnabled = true;
			cbTransformStages.ItemHeight = 13;
			cbTransformStages.Location = new System.Drawing.Point(64, 8);
			cbTransformStages.Margin = new System.Windows.Forms.Padding(5);
			cbTransformStages.MaxDropDownItems = 40;
			cbTransformStages.Name = "cbTransformStages";
			cbTransformStages.Size = new System.Drawing.Size(282, 21);
			cbTransformStages.TabIndex = 42;
			cbTransformStages.SelectedIndexChanged += cbTransformStages_SelectedIndexChanged;
			// 
			// label8
			// 
			label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label8.Location = new System.Drawing.Point(5, 9);
			label8.Margin = new System.Windows.Forms.Padding(5);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(58, 23);
			label8.TabIndex = 43;
			label8.Text = "Stage:";
			// 
			// tabMethodCounters
			// 
			tabMethodCounters.BackColor = System.Drawing.Color.Gainsboro;
			tabMethodCounters.Controls.Add(tabControl1);
			tabMethodCounters.Location = new System.Drawing.Point(4, 28);
			tabMethodCounters.Margin = new System.Windows.Forms.Padding(0);
			tabMethodCounters.Name = "tabMethodCounters";
			tabMethodCounters.Size = new System.Drawing.Size(683, 361);
			tabMethodCounters.TabIndex = 6;
			tabMethodCounters.Text = "Counters";
			// 
			// tabControl1
			// 
			tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Location = new System.Drawing.Point(0, 0);
			tabControl1.Margin = new System.Windows.Forms.Padding(0);
			tabControl1.Multiline = true;
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new System.Drawing.Size(687, 368);
			tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			tabPage1.BackColor = System.Drawing.SystemColors.Control;
			tabPage1.Controls.Add(label5);
			tabPage1.Controls.Add(tbCounterFilter);
			tabPage1.Controls.Add(gridMethodCounters);
			tabPage1.Location = new System.Drawing.Point(4, 28);
			tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage1.Size = new System.Drawing.Size(679, 336);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Grid";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(495, 13);
			label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(43, 17);
			label5.TabIndex = 9;
			label5.Text = "Filter:";
			// 
			// tbCounterFilter
			// 
			tbCounterFilter.Location = new System.Drawing.Point(495, 31);
			tbCounterFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbCounterFilter.Name = "tbCounterFilter";
			tbCounterFilter.Size = new System.Drawing.Size(178, 23);
			tbCounterFilter.TabIndex = 8;
			tbCounterFilter.TextChanged += tbMethodCounterFilter_TextChanged;
			// 
			// gridMethodCounters
			// 
			gridMethodCounters.AllowUserToAddRows = false;
			gridMethodCounters.AllowUserToDeleteRows = false;
			gridMethodCounters.AllowUserToOrderColumns = true;
			gridMethodCounters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			gridMethodCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			gridMethodCounters.Location = new System.Drawing.Point(3, 0);
			gridMethodCounters.Name = "gridMethodCounters";
			gridMethodCounters.ReadOnly = true;
			gridMethodCounters.RowHeadersVisible = false;
			gridMethodCounters.RowHeadersWidth = 51;
			gridMethodCounters.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			gridMethodCounters.RowTemplate.Height = 20;
			gridMethodCounters.ShowCellErrors = false;
			gridMethodCounters.ShowCellToolTips = false;
			gridMethodCounters.ShowEditingIcon = false;
			gridMethodCounters.ShowRowErrors = false;
			gridMethodCounters.Size = new System.Drawing.Size(481, 334);
			gridMethodCounters.TabIndex = 7;
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(tbMethodCounters);
			tabPage2.Location = new System.Drawing.Point(4, 27);
			tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage2.Size = new System.Drawing.Size(679, 337);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Text";
			tabPage2.UseVisualStyleBackColor = true;
			// 
			// tbMethodCounters
			// 
			tbMethodCounters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbMethodCounters.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tbMethodCounters.Location = new System.Drawing.Point(0, 3);
			tbMethodCounters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbMethodCounters.Name = "tbMethodCounters";
			tbMethodCounters.Size = new System.Drawing.Size(676, 334);
			tbMethodCounters.TabIndex = 10;
			tbMethodCounters.Text = "";
			tbMethodCounters.WordWrap = false;
			// 
			// tabLogs
			// 
			tabLogs.BackColor = System.Drawing.Color.Gainsboro;
			tabLogs.Controls.Add(cbCompilerSections);
			tabLogs.Controls.Add(label4);
			tabLogs.Controls.Add(tbCompilerLogs);
			tabLogs.Location = new System.Drawing.Point(4, 28);
			tabLogs.Margin = new System.Windows.Forms.Padding(0);
			tabLogs.Name = "tabLogs";
			tabLogs.Size = new System.Drawing.Size(683, 361);
			tabLogs.TabIndex = 7;
			tabLogs.Text = "Compiler Logs";
			// 
			// cbCompilerSections
			// 
			cbCompilerSections.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbCompilerSections.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			cbCompilerSections.FormattingEnabled = true;
			cbCompilerSections.Location = new System.Drawing.Point(77, 8);
			cbCompilerSections.Margin = new System.Windows.Forms.Padding(5);
			cbCompilerSections.MaxDropDownItems = 20;
			cbCompilerSections.Name = "cbCompilerSections";
			cbCompilerSections.Size = new System.Drawing.Size(285, 21);
			cbCompilerSections.TabIndex = 44;
			cbCompilerSections.SelectedIndexChanged += cbCompilerSections_SelectedIndexChanged;
			// 
			// label4
			// 
			label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			label4.Location = new System.Drawing.Point(5, 9);
			label4.Margin = new System.Windows.Forms.Padding(5);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(70, 23);
			label4.TabIndex = 45;
			label4.Text = "Section:";
			// 
			// tbCompilerLogs
			// 
			tbCompilerLogs.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbCompilerLogs.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tbCompilerLogs.Location = new System.Drawing.Point(0, 37);
			tbCompilerLogs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbCompilerLogs.Name = "tbCompilerLogs";
			tbCompilerLogs.Size = new System.Drawing.Size(683, 324);
			tbCompilerLogs.TabIndex = 3;
			tbCompilerLogs.Text = "";
			tbCompilerLogs.WordWrap = false;
			// 
			// tabCompilerCounters
			// 
			tabCompilerCounters.Controls.Add(tabControl2);
			tabCompilerCounters.Location = new System.Drawing.Point(4, 28);
			tabCompilerCounters.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabCompilerCounters.Name = "tabCompilerCounters";
			tabCompilerCounters.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabCompilerCounters.Size = new System.Drawing.Size(683, 361);
			tabCompilerCounters.TabIndex = 8;
			tabCompilerCounters.Text = "Compiler Counters";
			tabCompilerCounters.UseVisualStyleBackColor = true;
			// 
			// tabControl2
			// 
			tabControl2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
			tabControl2.Controls.Add(tabPage4);
			tabControl2.Controls.Add(tabPage5);
			tabControl2.Location = new System.Drawing.Point(0, 0);
			tabControl2.Margin = new System.Windows.Forms.Padding(0);
			tabControl2.Multiline = true;
			tabControl2.Name = "tabControl2";
			tabControl2.SelectedIndex = 0;
			tabControl2.Size = new System.Drawing.Size(687, 365);
			tabControl2.TabIndex = 8;
			// 
			// tabPage4
			// 
			tabPage4.BackColor = System.Drawing.SystemColors.Control;
			tabPage4.Controls.Add(label6);
			tabPage4.Controls.Add(tbCompilerCounterFilter);
			tabPage4.Controls.Add(gridCompilerCounters);
			tabPage4.Location = new System.Drawing.Point(4, 28);
			tabPage4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage4.Name = "tabPage4";
			tabPage4.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage4.Size = new System.Drawing.Size(679, 333);
			tabPage4.TabIndex = 0;
			tabPage4.Text = "Grid";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(498, 15);
			label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(43, 17);
			label6.TabIndex = 9;
			label6.Text = "Filter:";
			// 
			// tbCompilerCounterFilter
			// 
			tbCompilerCounterFilter.Location = new System.Drawing.Point(495, 31);
			tbCompilerCounterFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbCompilerCounterFilter.Name = "tbCompilerCounterFilter";
			tbCompilerCounterFilter.Size = new System.Drawing.Size(178, 23);
			tbCompilerCounterFilter.TabIndex = 8;
			tbCompilerCounterFilter.TextChanged += tbCompilerCounterFilter_TextChanged;
			// 
			// gridCompilerCounters
			// 
			gridCompilerCounters.AllowUserToAddRows = false;
			gridCompilerCounters.AllowUserToDeleteRows = false;
			gridCompilerCounters.AllowUserToOrderColumns = true;
			gridCompilerCounters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			gridCompilerCounters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			gridCompilerCounters.Location = new System.Drawing.Point(3, 0);
			gridCompilerCounters.Name = "gridCompilerCounters";
			gridCompilerCounters.ReadOnly = true;
			gridCompilerCounters.RowHeadersVisible = false;
			gridCompilerCounters.RowHeadersWidth = 51;
			gridCompilerCounters.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			gridCompilerCounters.RowTemplate.Height = 20;
			gridCompilerCounters.ShowCellErrors = false;
			gridCompilerCounters.ShowCellToolTips = false;
			gridCompilerCounters.ShowEditingIcon = false;
			gridCompilerCounters.ShowRowErrors = false;
			gridCompilerCounters.Size = new System.Drawing.Size(481, 332);
			gridCompilerCounters.TabIndex = 7;
			// 
			// tabPage5
			// 
			tabPage5.Controls.Add(tbCompilerCounters);
			tabPage5.Location = new System.Drawing.Point(4, 27);
			tabPage5.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage5.Name = "tabPage5";
			tabPage5.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			tabPage5.Size = new System.Drawing.Size(679, 334);
			tabPage5.TabIndex = 1;
			tabPage5.Text = "Text";
			tabPage5.UseVisualStyleBackColor = true;
			// 
			// tbCompilerCounters
			// 
			tbCompilerCounters.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tbCompilerCounters.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			tbCompilerCounters.Location = new System.Drawing.Point(0, 0);
			tbCompilerCounters.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			tbCompilerCounters.Name = "tbCompilerCounters";
			tbCompilerCounters.Size = new System.Drawing.Size(676, 330);
			tbCompilerCounters.TabIndex = 10;
			tbCompilerCounters.Text = "";
			tbCompilerCounters.WordWrap = false;
			// 
			// toolStrip1
			// 
			toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { cbPlatform, toolStripSeparator3, tsbOpen, toolStripSeparator2, tsbRefresh, toolStripSeparator1, tsbCompile });
			toolStrip1.Location = new System.Drawing.Point(0, 24);
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new System.Drawing.Size(940, 27);
			toolStrip1.TabIndex = 27;
			toolStrip1.Text = "toolStrip1";
			// 
			// cbPlatform
			// 
			cbPlatform.BackColor = System.Drawing.SystemColors.Window;
			cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbPlatform.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			cbPlatform.Items.AddRange(new object[] { "x86", "x64", "ARMv8A32" });
			cbPlatform.Name = "cbPlatform";
			cbPlatform.Size = new System.Drawing.Size(104, 27);
			cbPlatform.SelectedIndexChanged += cbPlatform_SelectedIndexChanged;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
			// 
			// tsbOpen
			// 
			tsbOpen.Image = (System.Drawing.Image)resources.GetObject("tsbOpen.Image");
			tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbOpen.Name = "tsbOpen";
			tsbOpen.Size = new System.Drawing.Size(60, 24);
			tsbOpen.Text = "Open";
			tsbOpen.Click += ToolStripButton1_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
			// 
			// tsbRefresh
			// 
			tsbRefresh.Image = (System.Drawing.Image)resources.GetObject("tsbRefresh.Image");
			tsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbRefresh.Name = "tsbRefresh";
			tsbRefresh.Size = new System.Drawing.Size(70, 24);
			tsbRefresh.Text = "Refresh";
			tsbRefresh.Click += tsbRefresh_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
			// 
			// tsbCompile
			// 
			tsbCompile.Image = (System.Drawing.Image)resources.GetObject("tsbCompile.Image");
			tsbCompile.ImageTransparentColor = System.Drawing.Color.Magenta;
			tsbCompile.Name = "tsbCompile";
			tsbCompile.Size = new System.Drawing.Size(76, 24);
			tsbCompile.Text = "Compile";
			tsbCompile.Click += ToolStripButton4_Click;
			// 
			// folderBrowserDialog1
			// 
			folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// timer1
			// 
			timer1.Enabled = true;
			timer1.Tick += timer1_Tick;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(940, 473);
			Controls.Add(toolStrip1);
			Controls.Add(statusStrip1);
			Controls.Add(menuStrip1);
			Controls.Add(splitContainer1);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			Name = "MainForm";
			Text = "MOSA Explorer";
			Load += Main_Load;
			statusStrip1.ResumeLayout(false);
			statusStrip1.PerformLayout();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			splitContainer1.Panel1.ResumeLayout(false);
			splitContainer1.Panel1.PerformLayout();
			splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
			splitContainer1.ResumeLayout(false);
			tabControl.ResumeLayout(false);
			tabStages.ResumeLayout(false);
			tabStageDebug.ResumeLayout(false);
			tabStageDebug.PerformLayout();
			tabTransforms.ResumeLayout(false);
			splitContainer2.Panel1.ResumeLayout(false);
			splitContainer2.Panel1.PerformLayout();
			splitContainer2.Panel2.ResumeLayout(false);
			splitContainer2.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
			splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
			tabMethodCounters.ResumeLayout(false);
			tabControl1.ResumeLayout(false);
			tabPage1.ResumeLayout(false);
			tabPage1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)gridMethodCounters).EndInit();
			tabPage2.ResumeLayout(false);
			tabLogs.ResumeLayout(false);
			tabCompilerCounters.ResumeLayout(false);
			tabControl2.ResumeLayout(false);
			tabPage4.ResumeLayout(false);
			tabPage4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)gridCompilerCounters).EndInit();
			tabPage5.ResumeLayout(false);
			toolStrip1.ResumeLayout(false);
			toolStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
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
		private System.Windows.Forms.ToolStripButton tsbOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton tsbCompile;
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
		private System.Windows.Forms.ToolStripMenuItem cbDumpAllMethodStages;
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
		private System.Windows.Forms.ToolStripMenuItem cbEnableMultithreading;
		private System.Windows.Forms.ToolStripMenuItem cbLoopInvariantCodeMotion;
		private System.Windows.Forms.ToolStripMenuItem cbPlatformOptimizations;
		private System.Windows.Forms.ToolStripMenuItem cbInlineExplicit;
		private System.Windows.Forms.ToolStripMenuItem cbEnableAllOptimizations;
		private System.Windows.Forms.ToolStripMenuItem cbDisableAllOptimizations;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem cbEnableDevirtualization;
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
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ComboBox cbTransformLabels;
		private System.Windows.Forms.ComboBox cbTransformStages;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RichTextBox tbCompilerLogs;
		private System.Windows.Forms.Button btnSaveB;
		private System.Windows.Forms.Button btnSaveA;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.ToolStripMenuItem removeIRNop;
		private System.Windows.Forms.ToolStripMenuItem cbEnableDebugDiagnostic;
		private System.Windows.Forms.ToolStripButton tsbRefresh;
		private System.Windows.Forms.CheckBox cbGraphviz;
		private System.Windows.Forms.RichTextBox tbDebugResult;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.RichTextBox tbTransforms;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.Label lbSteps;
		private System.Windows.Forms.Button btnLast;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.CheckBox cbSetBlock;
	}
}
