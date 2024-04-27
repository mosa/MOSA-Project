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
			statusStrip1 = new StatusStrip();
			toolStripStatusLabel1 = new ToolStripStatusLabel();
			toolStripProgressBar1 = new ToolStripProgressBar();
			toolStripStatusLabel = new ToolStripStatusLabel();
			menuStrip1 = new MenuStrip();
			fileToolStripMenuItem = new ToolStripMenuItem();
			openToolStripMenuItem = new ToolStripMenuItem();
			toolStripMenuItem1 = new ToolStripSeparator();
			quitToolStripMenuItem = new ToolStripMenuItem();
			compileToolStripMenuItem = new ToolStripMenuItem();
			nowToolStripMenuItem = new ToolStripMenuItem();
			optionsToolStripMenuItem = new ToolStripMenuItem();
			cbEnableAllOptimizations = new ToolStripMenuItem();
			cbDisableAllOptimizations = new ToolStripMenuItem();
			toolStripSeparator4 = new ToolStripSeparator();
			cbEnableSSA = new ToolStripMenuItem();
			cbEnableBasicOptimizations = new ToolStripMenuItem();
			cbEnableValueNumbering = new ToolStripMenuItem();
			cbEnableSparseConditionalConstantPropagation = new ToolStripMenuItem();
			cbEnableDevirtualization = new ToolStripMenuItem();
			cbEnableInline = new ToolStripMenuItem();
			cbInlineExplicit = new ToolStripMenuItem();
			cbEnableLongExpansion = new ToolStripMenuItem();
			cbLoopInvariantCodeMotion = new ToolStripMenuItem();
			cbEnableBitTracker = new ToolStripMenuItem();
			cbEnableLoopRangeTracker = new ToolStripMenuItem();
			cbEnableTwoPassOptimizations = new ToolStripMenuItem();
			cbPlatformOptimizations = new ToolStripMenuItem();
			cbEnableBinaryCodeGeneration = new ToolStripMenuItem();
			cbEnableCodeSizeReduction = new ToolStripMenuItem();
			displayOptionsToolStripMenuItem = new ToolStripMenuItem();
			showOperandTypes = new ToolStripMenuItem();
			showSizes = new ToolStripMenuItem();
			removeIRNop = new ToolStripMenuItem();
			advanceToolStripMenuItem = new ToolStripMenuItem();
			cbEnableMultithreading = new ToolStripMenuItem();
			cbEnableMethodScanner = new ToolStripMenuItem();
			cbEnableDebugDiagnostic = new ToolStripMenuItem();
			cbDumpAllMethodStages = new ToolStripMenuItem();
			openFileDialog = new OpenFileDialog();
			treeView = new TreeView();
			splitContainer1 = new SplitContainer();
			label2 = new Label();
			tbFilter = new TextBox();
			tabControl = new TabControl();
			tabStages = new TabPage();
			btnSaveB = new Button();
			btnSaveA = new Button();
			label1 = new Label();
			cbInstructionLabels = new ComboBox();
			cbInstructionStages = new ComboBox();
			stageLabel = new Label();
			tbInstructions = new RichTextBox();
			tabStageDebug = new TabPage();
			cbGraphviz = new CheckBox();
			cbDebugStages = new ComboBox();
			label3 = new Label();
			tbDebugResult = new RichTextBox();
			panel1 = new Panel();
			tabTransforms = new TabPage();
			button1 = new Button();
			button2 = new Button();
			splitContainer2 = new SplitContainer();
			lbSteps = new Label();
			btnLast = new Button();
			btnNext = new Button();
			btnPrevious = new Button();
			btnFirst = new Button();
			tbTransforms = new RichTextBox();
			cbSetBlock = new CheckBox();
			dataGridView1 = new DataGridView();
			label7 = new Label();
			cbTransformLabels = new ComboBox();
			cbTransformStages = new ComboBox();
			label8 = new Label();
			tabMethodCounters = new TabPage();
			tabControl1 = new TabControl();
			tabPage1 = new TabPage();
			label5 = new Label();
			tbCounterFilter = new TextBox();
			gridMethodCounters = new DataGridView();
			tabPage2 = new TabPage();
			tbMethodCounters = new RichTextBox();
			tabLogs = new TabPage();
			cbCompilerSections = new ComboBox();
			label4 = new Label();
			tbCompilerLogs = new RichTextBox();
			tabCompilerCounters = new TabPage();
			tabControl2 = new TabControl();
			tabPage4 = new TabPage();
			label6 = new Label();
			tbCompilerCounterFilter = new TextBox();
			gridCompilerCounters = new DataGridView();
			tabPage5 = new TabPage();
			tbCompilerCounters = new RichTextBox();
			toolStrip1 = new ToolStrip();
			cbPlatform = new ToolStripComboBox();
			toolStripSeparator3 = new ToolStripSeparator();
			tsbOpen = new ToolStripButton();
			toolStripSeparator2 = new ToolStripSeparator();
			tsbRefresh = new ToolStripButton();
			toolStripSeparator1 = new ToolStripSeparator();
			tsbCompile = new ToolStripButton();
			folderBrowserDialog1 = new FolderBrowserDialog();
			timer1 = new System.Windows.Forms.Timer(components);
			saveFileDialog1 = new SaveFileDialog();
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
			statusStrip1.ImageScalingSize = new Size(20, 20);
			statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripProgressBar1, toolStripStatusLabel });
			statusStrip1.Location = new Point(0, 450);
			statusStrip1.Name = "statusStrip1";
			statusStrip1.Padding = new Padding(1, 0, 16, 0);
			statusStrip1.Size = new Size(940, 23);
			statusStrip1.TabIndex = 0;
			statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			toolStripStatusLabel1.Size = new Size(0, 18);
			// 
			// toolStripProgressBar1
			// 
			toolStripProgressBar1.Name = "toolStripProgressBar1";
			toolStripProgressBar1.Size = new Size(233, 17);
			// 
			// toolStripStatusLabel
			// 
			toolStripStatusLabel.Name = "toolStripStatusLabel";
			toolStripStatusLabel.Size = new Size(0, 18);
			// 
			// menuStrip1
			// 
			menuStrip1.ImageScalingSize = new Size(20, 20);
			menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, compileToolStripMenuItem, optionsToolStripMenuItem, displayOptionsToolStripMenuItem, advanceToolStripMenuItem });
			menuStrip1.Location = new Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Padding = new Padding(7, 2, 0, 2);
			menuStrip1.Size = new Size(940, 24);
			menuStrip1.TabIndex = 3;
			menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, toolStripMenuItem1, quitToolStripMenuItem });
			fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			fileToolStripMenuItem.Size = new Size(37, 20);
			fileToolStripMenuItem.Text = "&File";
			// 
			// openToolStripMenuItem
			// 
			openToolStripMenuItem.Name = "openToolStripMenuItem";
			openToolStripMenuItem.Size = new Size(103, 22);
			openToolStripMenuItem.Text = "&Open";
			openToolStripMenuItem.Click += OpenToolStripMenuItem_Click;
			// 
			// toolStripMenuItem1
			// 
			toolStripMenuItem1.Name = "toolStripMenuItem1";
			toolStripMenuItem1.Size = new Size(100, 6);
			// 
			// quitToolStripMenuItem
			// 
			quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			quitToolStripMenuItem.Size = new Size(103, 22);
			quitToolStripMenuItem.Text = "&Quit";
			quitToolStripMenuItem.Click += QuitToolStripMenuItem_Click;
			// 
			// compileToolStripMenuItem
			// 
			compileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nowToolStripMenuItem });
			compileToolStripMenuItem.Name = "compileToolStripMenuItem";
			compileToolStripMenuItem.Size = new Size(64, 20);
			compileToolStripMenuItem.Text = "Compile";
			// 
			// nowToolStripMenuItem
			// 
			nowToolStripMenuItem.Name = "nowToolStripMenuItem";
			nowToolStripMenuItem.Size = new Size(99, 22);
			nowToolStripMenuItem.Text = "Now";
			nowToolStripMenuItem.Click += NowToolStripMenuItem_Click;
			// 
			// optionsToolStripMenuItem
			// 
			optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cbEnableAllOptimizations, cbDisableAllOptimizations, toolStripSeparator4, cbEnableSSA, cbEnableBasicOptimizations, cbEnableValueNumbering, cbEnableSparseConditionalConstantPropagation, cbEnableDevirtualization, cbEnableInline, cbInlineExplicit, cbEnableLongExpansion, cbLoopInvariantCodeMotion, cbEnableBitTracker, cbEnableLoopRangeTracker, cbEnableTwoPassOptimizations, cbPlatformOptimizations, cbEnableBinaryCodeGeneration, cbEnableCodeSizeReduction });
			optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			optionsToolStripMenuItem.Size = new Size(93, 20);
			optionsToolStripMenuItem.Text = "Optimizations";
			// 
			// cbEnableAllOptimizations
			// 
			cbEnableAllOptimizations.Name = "cbEnableAllOptimizations";
			cbEnableAllOptimizations.Size = new Size(293, 22);
			cbEnableAllOptimizations.Text = "Enable All";
			cbEnableAllOptimizations.Click += cbEnableAllOptimizations_Click;
			// 
			// cbDisableAllOptimizations
			// 
			cbDisableAllOptimizations.Name = "cbDisableAllOptimizations";
			cbDisableAllOptimizations.Size = new Size(293, 22);
			cbDisableAllOptimizations.Text = "Disable All";
			cbDisableAllOptimizations.Click += cbDisableAllOptimizations_Click;
			// 
			// toolStripSeparator4
			// 
			toolStripSeparator4.Name = "toolStripSeparator4";
			toolStripSeparator4.Size = new Size(290, 6);
			// 
			// cbEnableSSA
			// 
			cbEnableSSA.Checked = true;
			cbEnableSSA.CheckOnClick = true;
			cbEnableSSA.CheckState = CheckState.Checked;
			cbEnableSSA.Name = "cbEnableSSA";
			cbEnableSSA.Size = new Size(293, 22);
			cbEnableSSA.Text = "Enable SSA";
			// 
			// cbEnableBasicOptimizations
			// 
			cbEnableBasicOptimizations.Checked = true;
			cbEnableBasicOptimizations.CheckOnClick = true;
			cbEnableBasicOptimizations.CheckState = CheckState.Checked;
			cbEnableBasicOptimizations.Name = "cbEnableBasicOptimizations";
			cbEnableBasicOptimizations.Size = new Size(293, 22);
			cbEnableBasicOptimizations.Text = "Enable Basic Optimizations";
			// 
			// cbEnableValueNumbering
			// 
			cbEnableValueNumbering.Checked = true;
			cbEnableValueNumbering.CheckOnClick = true;
			cbEnableValueNumbering.CheckState = CheckState.Checked;
			cbEnableValueNumbering.Name = "cbEnableValueNumbering";
			cbEnableValueNumbering.Size = new Size(293, 22);
			cbEnableValueNumbering.Text = "Enable Value Numbering";
			// 
			// cbEnableSparseConditionalConstantPropagation
			// 
			cbEnableSparseConditionalConstantPropagation.Checked = true;
			cbEnableSparseConditionalConstantPropagation.CheckOnClick = true;
			cbEnableSparseConditionalConstantPropagation.CheckState = CheckState.Checked;
			cbEnableSparseConditionalConstantPropagation.Name = "cbEnableSparseConditionalConstantPropagation";
			cbEnableSparseConditionalConstantPropagation.Size = new Size(293, 22);
			cbEnableSparseConditionalConstantPropagation.Text = "Enable Conditional Constant Propagation";
			// 
			// cbEnableDevirtualization
			// 
			cbEnableDevirtualization.Checked = true;
			cbEnableDevirtualization.CheckOnClick = true;
			cbEnableDevirtualization.CheckState = CheckState.Checked;
			cbEnableDevirtualization.Name = "cbEnableDevirtualization";
			cbEnableDevirtualization.Size = new Size(293, 22);
			cbEnableDevirtualization.Text = "Enable Devirtualization";
			// 
			// cbEnableInline
			// 
			cbEnableInline.Checked = true;
			cbEnableInline.CheckOnClick = true;
			cbEnableInline.CheckState = CheckState.Checked;
			cbEnableInline.Name = "cbEnableInline";
			cbEnableInline.Size = new Size(293, 22);
			cbEnableInline.Text = "Enable Inlined Methods";
			// 
			// cbInlineExplicit
			// 
			cbInlineExplicit.Checked = true;
			cbInlineExplicit.CheckOnClick = true;
			cbInlineExplicit.CheckState = CheckState.Checked;
			cbInlineExplicit.Name = "cbInlineExplicit";
			cbInlineExplicit.Size = new Size(293, 22);
			cbInlineExplicit.Text = "Enable Inlined Explicit Methods";
			// 
			// cbEnableLongExpansion
			// 
			cbEnableLongExpansion.Checked = true;
			cbEnableLongExpansion.CheckOnClick = true;
			cbEnableLongExpansion.CheckState = CheckState.Checked;
			cbEnableLongExpansion.Name = "cbEnableLongExpansion";
			cbEnableLongExpansion.Size = new Size(293, 22);
			cbEnableLongExpansion.Text = "Enable Long Expansion";
			// 
			// cbLoopInvariantCodeMotion
			// 
			cbLoopInvariantCodeMotion.Checked = true;
			cbLoopInvariantCodeMotion.CheckOnClick = true;
			cbLoopInvariantCodeMotion.CheckState = CheckState.Checked;
			cbLoopInvariantCodeMotion.Name = "cbLoopInvariantCodeMotion";
			cbLoopInvariantCodeMotion.Size = new Size(293, 22);
			cbLoopInvariantCodeMotion.Text = "Enable Loop Invariant Code Motion";
			// 
			// cbEnableBitTracker
			// 
			cbEnableBitTracker.Checked = true;
			cbEnableBitTracker.CheckOnClick = true;
			cbEnableBitTracker.CheckState = CheckState.Checked;
			cbEnableBitTracker.Name = "cbEnableBitTracker";
			cbEnableBitTracker.Size = new Size(293, 22);
			cbEnableBitTracker.Text = "Enable Bit Tracker";
			// 
			// cbEnableLoopRangeTracker
			// 
			cbEnableLoopRangeTracker.Checked = true;
			cbEnableLoopRangeTracker.CheckOnClick = true;
			cbEnableLoopRangeTracker.CheckState = CheckState.Checked;
			cbEnableLoopRangeTracker.Name = "cbEnableLoopRangeTracker";
			cbEnableLoopRangeTracker.Size = new Size(293, 22);
			cbEnableLoopRangeTracker.Text = "Enable Loop Range Tracker";
			// 
			// cbEnableTwoPassOptimizations
			// 
			cbEnableTwoPassOptimizations.Checked = true;
			cbEnableTwoPassOptimizations.CheckOnClick = true;
			cbEnableTwoPassOptimizations.CheckState = CheckState.Checked;
			cbEnableTwoPassOptimizations.Name = "cbEnableTwoPassOptimizations";
			cbEnableTwoPassOptimizations.Size = new Size(293, 22);
			cbEnableTwoPassOptimizations.Text = "Enable Two Optimization Passes";
			// 
			// cbPlatformOptimizations
			// 
			cbPlatformOptimizations.Checked = true;
			cbPlatformOptimizations.CheckOnClick = true;
			cbPlatformOptimizations.CheckState = CheckState.Checked;
			cbPlatformOptimizations.Name = "cbPlatformOptimizations";
			cbPlatformOptimizations.Size = new Size(293, 22);
			cbPlatformOptimizations.Text = "Enable Platform Optimizations";
			// 
			// cbEnableBinaryCodeGeneration
			// 
			cbEnableBinaryCodeGeneration.Checked = true;
			cbEnableBinaryCodeGeneration.CheckOnClick = true;
			cbEnableBinaryCodeGeneration.CheckState = CheckState.Checked;
			cbEnableBinaryCodeGeneration.Name = "cbEnableBinaryCodeGeneration";
			cbEnableBinaryCodeGeneration.Size = new Size(293, 22);
			cbEnableBinaryCodeGeneration.Text = "Enable Binary Code Generation";
			// 
			// cbEnableCodeSizeReduction
			// 
			cbEnableCodeSizeReduction.CheckOnClick = true;
			cbEnableCodeSizeReduction.Name = "cbEnableCodeSizeReduction";
			cbEnableCodeSizeReduction.Size = new Size(293, 22);
			cbEnableCodeSizeReduction.Text = "Enable Code Size Reduction";
			// 
			// displayOptionsToolStripMenuItem
			// 
			displayOptionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { showOperandTypes, showSizes, removeIRNop });
			displayOptionsToolStripMenuItem.Name = "displayOptionsToolStripMenuItem";
			displayOptionsToolStripMenuItem.Size = new Size(57, 20);
			displayOptionsToolStripMenuItem.Text = "Display";
			// 
			// showOperandTypes
			// 
			showOperandTypes.CheckOnClick = true;
			showOperandTypes.Name = "showOperandTypes";
			showOperandTypes.Size = new Size(184, 22);
			showOperandTypes.Text = "Show Operand Types";
			showOperandTypes.CheckStateChanged += DisplayCheckStateChanged;
			// 
			// showSizes
			// 
			showSizes.Checked = true;
			showSizes.CheckOnClick = true;
			showSizes.CheckState = CheckState.Checked;
			showSizes.Name = "showSizes";
			showSizes.Size = new Size(184, 22);
			showSizes.Text = "Show Sizes";
			showSizes.CheckStateChanged += DisplayCheckStateChanged;
			showSizes.Click += showSizesToolStripMenuItem_Click;
			// 
			// removeIRNop
			// 
			removeIRNop.CheckOnClick = true;
			removeIRNop.Name = "removeIRNop";
			removeIRNop.Size = new Size(184, 22);
			removeIRNop.Text = "Remove IR.Nop";
			removeIRNop.CheckStateChanged += DisplayCheckStateChanged;
			// 
			// advanceToolStripMenuItem
			// 
			advanceToolStripMenuItem.CheckOnClick = true;
			advanceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { cbEnableMultithreading, cbEnableMethodScanner, cbEnableDebugDiagnostic, cbDumpAllMethodStages });
			advanceToolStripMenuItem.Name = "advanceToolStripMenuItem";
			advanceToolStripMenuItem.Size = new Size(65, 20);
			advanceToolStripMenuItem.Text = "Advance";
			// 
			// cbEnableMultithreading
			// 
			cbEnableMultithreading.Checked = true;
			cbEnableMultithreading.CheckOnClick = true;
			cbEnableMultithreading.CheckState = CheckState.Checked;
			cbEnableMultithreading.Name = "cbEnableMultithreading";
			cbEnableMultithreading.Size = new Size(206, 22);
			cbEnableMultithreading.Text = "Enable Multithreading";
			// 
			// cbEnableMethodScanner
			// 
			cbEnableMethodScanner.CheckOnClick = true;
			cbEnableMethodScanner.Name = "cbEnableMethodScanner";
			cbEnableMethodScanner.Size = new Size(206, 22);
			cbEnableMethodScanner.Text = "Enable Method Scanner";
			// 
			// cbEnableDebugDiagnostic
			// 
			cbEnableDebugDiagnostic.CheckOnClick = true;
			cbEnableDebugDiagnostic.Name = "cbEnableDebugDiagnostic";
			cbEnableDebugDiagnostic.Size = new Size(206, 22);
			cbEnableDebugDiagnostic.Text = "Enable Debug Diagnostic";
			// 
			// cbDumpAllMethodStages
			// 
			cbDumpAllMethodStages.Name = "cbDumpAllMethodStages";
			cbDumpAllMethodStages.Size = new Size(206, 22);
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
			treeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			treeView.Location = new Point(4, 31);
			treeView.Margin = new Padding(0);
			treeView.Name = "treeView";
			treeView.Size = new Size(239, 365);
			treeView.TabIndex = 3;
			treeView.BeforeSelect += treeView_BeforeSelect;
			treeView.AfterSelect += TreeView_AfterSelect;
			treeView.NodeMouseDoubleClick += treeView_NodeMouseDoubleClick;
			// 
			// splitContainer1
			// 
			splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			splitContainer1.Location = new Point(0, 54);
			splitContainer1.Margin = new Padding(0);
			splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			splitContainer1.Panel1.BackColor = SystemColors.Control;
			splitContainer1.Panel1.Controls.Add(label2);
			splitContainer1.Panel1.Controls.Add(tbFilter);
			splitContainer1.Panel1.Controls.Add(treeView);
			splitContainer1.Panel1.RightToLeft = RightToLeft.No;
			// 
			// splitContainer1.Panel2
			// 
			splitContainer1.Panel2.Controls.Add(tabControl);
			splitContainer1.Panel2.RightToLeft = RightToLeft.No;
			splitContainer1.RightToLeft = RightToLeft.No;
			splitContainer1.Size = new Size(940, 396);
			splitContainer1.SplitterDistance = 244;
			splitContainer1.TabIndex = 26;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(4, 9);
			label2.Margin = new Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new Size(36, 15);
			label2.TabIndex = 5;
			label2.Text = "Filter:";
			// 
			// tbFilter
			// 
			tbFilter.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			tbFilter.Location = new Point(45, 5);
			tbFilter.Margin = new Padding(4, 3, 4, 3);
			tbFilter.Name = "tbFilter";
			tbFilter.Size = new Size(192, 23);
			tbFilter.TabIndex = 4;
			tbFilter.TextChanged += tbFilter_TextChanged;
			// 
			// tabControl
			// 
			tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tabControl.Appearance = TabAppearance.Buttons;
			tabControl.Controls.Add(tabStages);
			tabControl.Controls.Add(tabStageDebug);
			tabControl.Controls.Add(tabTransforms);
			tabControl.Controls.Add(tabMethodCounters);
			tabControl.Controls.Add(tabLogs);
			tabControl.Controls.Add(tabCompilerCounters);
			tabControl.Font = new Font("Microsoft Sans Serif", 10F);
			tabControl.Location = new Point(1, 3);
			tabControl.Margin = new Padding(0);
			tabControl.Name = "tabControl";
			tabControl.Padding = new Point(0, 0);
			tabControl.SelectedIndex = 0;
			tabControl.Size = new Size(691, 393);
			tabControl.TabIndex = 38;
			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
			// 
			// tabStages
			// 
			tabStages.BackColor = Color.Gainsboro;
			tabStages.Controls.Add(btnSaveB);
			tabStages.Controls.Add(btnSaveA);
			tabStages.Controls.Add(label1);
			tabStages.Controls.Add(cbInstructionLabels);
			tabStages.Controls.Add(cbInstructionStages);
			tabStages.Controls.Add(stageLabel);
			tabStages.Controls.Add(tbInstructions);
			tabStages.Location = new Point(4, 28);
			tabStages.Margin = new Padding(0);
			tabStages.Name = "tabStages";
			tabStages.Size = new Size(683, 361);
			tabStages.TabIndex = 0;
			tabStages.Text = "Instructions";
			// 
			// btnSaveB
			// 
			btnSaveB.Location = new Point(613, 6);
			btnSaveB.Name = "btnSaveB";
			btnSaveB.Size = new Size(64, 23);
			btnSaveB.TabIndex = 43;
			btnSaveB.Text = "Save B";
			btnSaveB.UseVisualStyleBackColor = true;
			btnSaveB.Click += btnSaveB_Click;
			// 
			// btnSaveA
			// 
			btnSaveA.Location = new Point(543, 6);
			btnSaveA.Name = "btnSaveA";
			btnSaveA.Size = new Size(64, 23);
			btnSaveA.TabIndex = 42;
			btnSaveA.Text = "Save A";
			btnSaveA.UseVisualStyleBackColor = true;
			btnSaveA.Click += btnSaveA_Click;
			// 
			// label1
			// 
			label1.Font = new Font("Microsoft Sans Serif", 10F);
			label1.Location = new Point(352, 9);
			label1.Margin = new Padding(5);
			label1.Name = "label1";
			label1.Size = new Size(56, 23);
			label1.TabIndex = 41;
			label1.Text = "Block:";
			// 
			// cbInstructionLabels
			// 
			cbInstructionLabels.DropDownStyle = ComboBoxStyle.DropDownList;
			cbInstructionLabels.Font = new Font("Microsoft Sans Serif", 8.25F);
			cbInstructionLabels.FormattingEnabled = true;
			cbInstructionLabels.Location = new Point(413, 8);
			cbInstructionLabels.Margin = new Padding(5);
			cbInstructionLabels.MaxDropDownItems = 20;
			cbInstructionLabels.Name = "cbInstructionLabels";
			cbInstructionLabels.Size = new Size(122, 21);
			cbInstructionLabels.TabIndex = 40;
			cbInstructionLabels.SelectedIndexChanged += cbInstructionLabels_SelectedIndexChanged;
			// 
			// cbInstructionStages
			// 
			cbInstructionStages.DropDownStyle = ComboBoxStyle.DropDownList;
			cbInstructionStages.Font = new Font("Microsoft Sans Serif", 8.25F);
			cbInstructionStages.FormattingEnabled = true;
			cbInstructionStages.ItemHeight = 13;
			cbInstructionStages.Location = new Point(64, 8);
			cbInstructionStages.Margin = new Padding(5);
			cbInstructionStages.MaxDropDownItems = 40;
			cbInstructionStages.Name = "cbInstructionStages";
			cbInstructionStages.Size = new Size(282, 21);
			cbInstructionStages.TabIndex = 38;
			cbInstructionStages.SelectedIndexChanged += cbInstructionStages_SelectedIndexChanged;
			// 
			// stageLabel
			// 
			stageLabel.Font = new Font("Microsoft Sans Serif", 10F);
			stageLabel.Location = new Point(5, 9);
			stageLabel.Margin = new Padding(5);
			stageLabel.Name = "stageLabel";
			stageLabel.Size = new Size(58, 23);
			stageLabel.TabIndex = 39;
			stageLabel.Text = "Stage:";
			// 
			// tbInstructions
			// 
			tbInstructions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tbInstructions.Font = new Font("Lucida Console", 8F);
			tbInstructions.Location = new Point(0, 37);
			tbInstructions.Margin = new Padding(4, 3, 4, 3);
			tbInstructions.Name = "tbInstructions";
			tbInstructions.Size = new Size(677, 324);
			tbInstructions.TabIndex = 31;
			tbInstructions.Text = "";
			tbInstructions.WordWrap = false;
			// 
			// tabStageDebug
			// 
			tabStageDebug.BackColor = Color.Gainsboro;
			tabStageDebug.Controls.Add(cbGraphviz);
			tabStageDebug.Controls.Add(cbDebugStages);
			tabStageDebug.Controls.Add(label3);
			tabStageDebug.Controls.Add(tbDebugResult);
			tabStageDebug.Controls.Add(panel1);
			tabStageDebug.Location = new Point(4, 28);
			tabStageDebug.Margin = new Padding(0);
			tabStageDebug.Name = "tabStageDebug";
			tabStageDebug.Size = new Size(683, 361);
			tabStageDebug.TabIndex = 1;
			tabStageDebug.Text = "Debug";
			// 
			// cbGraphviz
			// 
			cbGraphviz.AutoSize = true;
			cbGraphviz.Location = new Point(523, 10);
			cbGraphviz.Name = "cbGraphviz";
			cbGraphviz.Size = new Size(73, 21);
			cbGraphviz.TabIndex = 44;
			cbGraphviz.Text = "Display";
			cbGraphviz.UseVisualStyleBackColor = true;
			cbGraphviz.CheckedChanged += cbGraphviz_CheckedChanged;
			// 
			// cbDebugStages
			// 
			cbDebugStages.DropDownStyle = ComboBoxStyle.DropDownList;
			cbDebugStages.Font = new Font("Microsoft Sans Serif", 8.25F);
			cbDebugStages.FormattingEnabled = true;
			cbDebugStages.Location = new Point(64, 8);
			cbDebugStages.Margin = new Padding(5);
			cbDebugStages.MaxDropDownItems = 20;
			cbDebugStages.Name = "cbDebugStages";
			cbDebugStages.Size = new Size(451, 21);
			cbDebugStages.TabIndex = 40;
			cbDebugStages.SelectedIndexChanged += cbDebugStages_SelectedIndexChanged;
			// 
			// label3
			// 
			label3.Font = new Font("Microsoft Sans Serif", 10F);
			label3.Location = new Point(5, 9);
			label3.Margin = new Padding(5);
			label3.Name = "label3";
			label3.Size = new Size(58, 23);
			label3.TabIndex = 41;
			label3.Text = "Stage:";
			// 
			// tbDebugResult
			// 
			tbDebugResult.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tbDebugResult.Font = new Font("Lucida Console", 8F);
			tbDebugResult.Location = new Point(0, 37);
			tbDebugResult.Margin = new Padding(4, 3, 4, 3);
			tbDebugResult.Name = "tbDebugResult";
			tbDebugResult.Size = new Size(679, 331);
			tbDebugResult.TabIndex = 32;
			tbDebugResult.Text = "";
			tbDebugResult.WordWrap = false;
			// 
			// panel1
			// 
			panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			panel1.AutoScroll = true;
			panel1.Location = new Point(5, 40);
			panel1.Name = "panel1";
			panel1.Size = new Size(674, 321);
			panel1.TabIndex = 45;
			// 
			// tabTransforms
			// 
			tabTransforms.BackColor = Color.Gainsboro;
			tabTransforms.Controls.Add(button1);
			tabTransforms.Controls.Add(button2);
			tabTransforms.Controls.Add(splitContainer2);
			tabTransforms.Controls.Add(label7);
			tabTransforms.Controls.Add(cbTransformLabels);
			tabTransforms.Controls.Add(cbTransformStages);
			tabTransforms.Controls.Add(label8);
			tabTransforms.Location = new Point(4, 28);
			tabTransforms.Name = "tabTransforms";
			tabTransforms.Padding = new Padding(3);
			tabTransforms.Size = new Size(683, 361);
			tabTransforms.TabIndex = 9;
			tabTransforms.Text = "Transforms";
			// 
			// button1
			// 
			button1.Location = new Point(623, 6);
			button1.Name = "button1";
			button1.Size = new Size(64, 23);
			button1.TabIndex = 50;
			button1.Text = "Save 2";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new Point(553, 6);
			button2.Name = "button2";
			button2.Size = new Size(64, 23);
			button2.TabIndex = 49;
			button2.Text = "Save 1";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// splitContainer2
			// 
			splitContainer2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			splitContainer2.Location = new Point(0, 34);
			splitContainer2.Margin = new Padding(0);
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
			splitContainer2.Size = new Size(683, 327);
			splitContainer2.SplitterDistance = 424;
			splitContainer2.TabIndex = 48;
			// 
			// lbSteps
			// 
			lbSteps.AutoSize = true;
			lbSteps.Location = new Point(348, 7);
			lbSteps.Name = "lbSteps";
			lbSteps.Size = new Size(52, 17);
			lbSteps.TabIndex = 42;
			lbSteps.Text = "## / ##";
			// 
			// btnLast
			// 
			btnLast.Location = new Point(250, 4);
			btnLast.Name = "btnLast";
			btnLast.Size = new Size(75, 23);
			btnLast.TabIndex = 41;
			btnLast.Text = "Last";
			btnLast.UseVisualStyleBackColor = true;
			// 
			// btnNext
			// 
			btnNext.Location = new Point(169, 4);
			btnNext.Name = "btnNext";
			btnNext.Size = new Size(75, 23);
			btnNext.TabIndex = 40;
			btnNext.Text = "Next";
			btnNext.UseVisualStyleBackColor = true;
			// 
			// btnPrevious
			// 
			btnPrevious.Location = new Point(88, 4);
			btnPrevious.Name = "btnPrevious";
			btnPrevious.Size = new Size(75, 23);
			btnPrevious.TabIndex = 39;
			btnPrevious.Text = "Previous";
			btnPrevious.UseVisualStyleBackColor = true;
			// 
			// btnFirst
			// 
			btnFirst.Location = new Point(7, 4);
			btnFirst.Name = "btnFirst";
			btnFirst.Size = new Size(75, 23);
			btnFirst.TabIndex = 38;
			btnFirst.Text = "First";
			btnFirst.UseVisualStyleBackColor = true;
			// 
			// tbTransforms
			// 
			tbTransforms.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tbTransforms.Font = new Font("Lucida Console", 8F);
			tbTransforms.Location = new Point(0, 34);
			tbTransforms.Margin = new Padding(0);
			tbTransforms.Name = "tbTransforms";
			tbTransforms.Size = new Size(416, 287);
			tbTransforms.TabIndex = 33;
			tbTransforms.Text = "";
			tbTransforms.WordWrap = false;
			// 
			// cbSetBlock
			// 
			cbSetBlock.AutoSize = true;
			cbSetBlock.Location = new Point(5, 6);
			cbSetBlock.Name = "cbSetBlock";
			cbSetBlock.Size = new Size(86, 21);
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
			dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dataGridView1.Location = new Point(0, 34);
			dataGridView1.Margin = new Padding(0);
			dataGridView1.MultiSelect = false;
			dataGridView1.Name = "dataGridView1";
			dataGridView1.ReadOnly = true;
			dataGridView1.RowHeadersVisible = false;
			dataGridView1.RowTemplate.DefaultCellStyle.Font = new Font("Consolas", 8F);
			dataGridView1.RowTemplate.Height = 18;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			dataGridView1.Size = new Size(253, 290);
			dataGridView1.TabIndex = 48;
			dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
			// 
			// label7
			// 
			label7.Font = new Font("Microsoft Sans Serif", 10F);
			label7.Location = new Point(352, 9);
			label7.Margin = new Padding(5);
			label7.Name = "label7";
			label7.Size = new Size(56, 23);
			label7.TabIndex = 45;
			label7.Text = "Block:";
			// 
			// cbTransformLabels
			// 
			cbTransformLabels.DropDownStyle = ComboBoxStyle.DropDownList;
			cbTransformLabels.Font = new Font("Microsoft Sans Serif", 8.25F);
			cbTransformLabels.FormattingEnabled = true;
			cbTransformLabels.Location = new Point(413, 8);
			cbTransformLabels.Margin = new Padding(5);
			cbTransformLabels.MaxDropDownItems = 20;
			cbTransformLabels.Name = "cbTransformLabels";
			cbTransformLabels.Size = new Size(122, 21);
			cbTransformLabels.TabIndex = 44;
			cbTransformLabels.SelectedIndexChanged += cbTransformLabels_SelectedIndexChanged;
			// 
			// cbTransformStages
			// 
			cbTransformStages.DropDownStyle = ComboBoxStyle.DropDownList;
			cbTransformStages.Font = new Font("Microsoft Sans Serif", 8.25F);
			cbTransformStages.FormattingEnabled = true;
			cbTransformStages.ItemHeight = 13;
			cbTransformStages.Location = new Point(64, 8);
			cbTransformStages.Margin = new Padding(5);
			cbTransformStages.MaxDropDownItems = 40;
			cbTransformStages.Name = "cbTransformStages";
			cbTransformStages.Size = new Size(282, 21);
			cbTransformStages.TabIndex = 42;
			cbTransformStages.SelectedIndexChanged += cbTransformStages_SelectedIndexChanged;
			// 
			// label8
			// 
			label8.Font = new Font("Microsoft Sans Serif", 10F);
			label8.Location = new Point(5, 9);
			label8.Margin = new Padding(5);
			label8.Name = "label8";
			label8.Size = new Size(58, 23);
			label8.TabIndex = 43;
			label8.Text = "Stage:";
			// 
			// tabMethodCounters
			// 
			tabMethodCounters.BackColor = Color.Gainsboro;
			tabMethodCounters.Controls.Add(tabControl1);
			tabMethodCounters.Location = new Point(4, 28);
			tabMethodCounters.Margin = new Padding(0);
			tabMethodCounters.Name = "tabMethodCounters";
			tabMethodCounters.Size = new Size(683, 361);
			tabMethodCounters.TabIndex = 6;
			tabMethodCounters.Text = "Counters";
			// 
			// tabControl1
			// 
			tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tabControl1.Appearance = TabAppearance.Buttons;
			tabControl1.Controls.Add(tabPage1);
			tabControl1.Controls.Add(tabPage2);
			tabControl1.Location = new Point(0, 0);
			tabControl1.Margin = new Padding(0);
			tabControl1.Multiline = true;
			tabControl1.Name = "tabControl1";
			tabControl1.SelectedIndex = 0;
			tabControl1.Size = new Size(687, 368);
			tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			tabPage1.BackColor = SystemColors.Control;
			tabPage1.Controls.Add(label5);
			tabPage1.Controls.Add(tbCounterFilter);
			tabPage1.Controls.Add(gridMethodCounters);
			tabPage1.Location = new Point(4, 28);
			tabPage1.Margin = new Padding(3, 2, 3, 2);
			tabPage1.Name = "tabPage1";
			tabPage1.Padding = new Padding(3, 2, 3, 2);
			tabPage1.Size = new Size(679, 336);
			tabPage1.TabIndex = 0;
			tabPage1.Text = "Grid";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(495, 13);
			label5.Margin = new Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new Size(43, 17);
			label5.TabIndex = 9;
			label5.Text = "Filter:";
			// 
			// tbCounterFilter
			// 
			tbCounterFilter.Location = new Point(495, 31);
			tbCounterFilter.Margin = new Padding(4, 3, 4, 3);
			tbCounterFilter.Name = "tbCounterFilter";
			tbCounterFilter.Size = new Size(178, 23);
			tbCounterFilter.TabIndex = 8;
			tbCounterFilter.TextChanged += tbMethodCounterFilter_TextChanged;
			// 
			// gridMethodCounters
			// 
			gridMethodCounters.AllowUserToAddRows = false;
			gridMethodCounters.AllowUserToDeleteRows = false;
			gridMethodCounters.AllowUserToOrderColumns = true;
			gridMethodCounters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			gridMethodCounters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			gridMethodCounters.Location = new Point(3, 0);
			gridMethodCounters.Name = "gridMethodCounters";
			gridMethodCounters.ReadOnly = true;
			gridMethodCounters.RowHeadersVisible = false;
			gridMethodCounters.RowHeadersWidth = 51;
			gridMethodCounters.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8F);
			gridMethodCounters.RowTemplate.Height = 20;
			gridMethodCounters.ShowCellErrors = false;
			gridMethodCounters.ShowCellToolTips = false;
			gridMethodCounters.ShowEditingIcon = false;
			gridMethodCounters.ShowRowErrors = false;
			gridMethodCounters.Size = new Size(481, 334);
			gridMethodCounters.TabIndex = 7;
			// 
			// tabPage2
			// 
			tabPage2.Controls.Add(tbMethodCounters);
			tabPage2.Location = new Point(4, 27);
			tabPage2.Margin = new Padding(3, 2, 3, 2);
			tabPage2.Name = "tabPage2";
			tabPage2.Padding = new Padding(3, 2, 3, 2);
			tabPage2.Size = new Size(679, 337);
			tabPage2.TabIndex = 1;
			tabPage2.Text = "Text";
			tabPage2.UseVisualStyleBackColor = true;
			// 
			// tbMethodCounters
			// 
			tbMethodCounters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tbMethodCounters.Font = new Font("Lucida Console", 8F);
			tbMethodCounters.Location = new Point(0, 3);
			tbMethodCounters.Margin = new Padding(4, 3, 4, 3);
			tbMethodCounters.Name = "tbMethodCounters";
			tbMethodCounters.Size = new Size(676, 334);
			tbMethodCounters.TabIndex = 10;
			tbMethodCounters.Text = "";
			tbMethodCounters.WordWrap = false;
			// 
			// tabLogs
			// 
			tabLogs.BackColor = Color.Gainsboro;
			tabLogs.Controls.Add(cbCompilerSections);
			tabLogs.Controls.Add(label4);
			tabLogs.Controls.Add(tbCompilerLogs);
			tabLogs.Location = new Point(4, 28);
			tabLogs.Margin = new Padding(0);
			tabLogs.Name = "tabLogs";
			tabLogs.Size = new Size(683, 361);
			tabLogs.TabIndex = 7;
			tabLogs.Text = "Compiler Logs";
			// 
			// cbCompilerSections
			// 
			cbCompilerSections.DropDownStyle = ComboBoxStyle.DropDownList;
			cbCompilerSections.Font = new Font("Microsoft Sans Serif", 8.25F);
			cbCompilerSections.FormattingEnabled = true;
			cbCompilerSections.Location = new Point(77, 8);
			cbCompilerSections.Margin = new Padding(5);
			cbCompilerSections.MaxDropDownItems = 20;
			cbCompilerSections.Name = "cbCompilerSections";
			cbCompilerSections.Size = new Size(285, 21);
			cbCompilerSections.TabIndex = 44;
			cbCompilerSections.SelectedIndexChanged += cbCompilerSections_SelectedIndexChanged;
			// 
			// label4
			// 
			label4.Font = new Font("Microsoft Sans Serif", 10F);
			label4.Location = new Point(5, 9);
			label4.Margin = new Padding(5);
			label4.Name = "label4";
			label4.Size = new Size(70, 23);
			label4.TabIndex = 45;
			label4.Text = "Section:";
			// 
			// tbCompilerLogs
			// 
			tbCompilerLogs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tbCompilerLogs.Font = new Font("Lucida Console", 8F);
			tbCompilerLogs.Location = new Point(0, 37);
			tbCompilerLogs.Margin = new Padding(4, 3, 4, 3);
			tbCompilerLogs.Name = "tbCompilerLogs";
			tbCompilerLogs.Size = new Size(683, 324);
			tbCompilerLogs.TabIndex = 3;
			tbCompilerLogs.Text = "";
			tbCompilerLogs.WordWrap = false;
			// 
			// tabCompilerCounters
			// 
			tabCompilerCounters.Controls.Add(tabControl2);
			tabCompilerCounters.Location = new Point(4, 28);
			tabCompilerCounters.Margin = new Padding(3, 2, 3, 2);
			tabCompilerCounters.Name = "tabCompilerCounters";
			tabCompilerCounters.Padding = new Padding(3, 2, 3, 2);
			tabCompilerCounters.Size = new Size(683, 361);
			tabCompilerCounters.TabIndex = 8;
			tabCompilerCounters.Text = "Compiler Counters";
			tabCompilerCounters.UseVisualStyleBackColor = true;
			// 
			// tabControl2
			// 
			tabControl2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tabControl2.Appearance = TabAppearance.Buttons;
			tabControl2.Controls.Add(tabPage4);
			tabControl2.Controls.Add(tabPage5);
			tabControl2.Location = new Point(0, 0);
			tabControl2.Margin = new Padding(0);
			tabControl2.Multiline = true;
			tabControl2.Name = "tabControl2";
			tabControl2.SelectedIndex = 0;
			tabControl2.Size = new Size(687, 365);
			tabControl2.TabIndex = 8;
			// 
			// tabPage4
			// 
			tabPage4.BackColor = SystemColors.Control;
			tabPage4.Controls.Add(label6);
			tabPage4.Controls.Add(tbCompilerCounterFilter);
			tabPage4.Controls.Add(gridCompilerCounters);
			tabPage4.Location = new Point(4, 28);
			tabPage4.Margin = new Padding(3, 2, 3, 2);
			tabPage4.Name = "tabPage4";
			tabPage4.Padding = new Padding(3, 2, 3, 2);
			tabPage4.Size = new Size(679, 333);
			tabPage4.TabIndex = 0;
			tabPage4.Text = "Grid";
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(498, 15);
			label6.Margin = new Padding(4, 0, 4, 0);
			label6.Name = "label6";
			label6.Size = new Size(43, 17);
			label6.TabIndex = 9;
			label6.Text = "Filter:";
			// 
			// tbCompilerCounterFilter
			// 
			tbCompilerCounterFilter.Location = new Point(495, 31);
			tbCompilerCounterFilter.Margin = new Padding(4, 3, 4, 3);
			tbCompilerCounterFilter.Name = "tbCompilerCounterFilter";
			tbCompilerCounterFilter.Size = new Size(178, 23);
			tbCompilerCounterFilter.TabIndex = 8;
			tbCompilerCounterFilter.TextChanged += tbCompilerCounterFilter_TextChanged;
			// 
			// gridCompilerCounters
			// 
			gridCompilerCounters.AllowUserToAddRows = false;
			gridCompilerCounters.AllowUserToDeleteRows = false;
			gridCompilerCounters.AllowUserToOrderColumns = true;
			gridCompilerCounters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
			gridCompilerCounters.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			gridCompilerCounters.Location = new Point(3, 0);
			gridCompilerCounters.Name = "gridCompilerCounters";
			gridCompilerCounters.ReadOnly = true;
			gridCompilerCounters.RowHeadersVisible = false;
			gridCompilerCounters.RowHeadersWidth = 51;
			gridCompilerCounters.RowTemplate.DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8F);
			gridCompilerCounters.RowTemplate.Height = 20;
			gridCompilerCounters.ShowCellErrors = false;
			gridCompilerCounters.ShowCellToolTips = false;
			gridCompilerCounters.ShowEditingIcon = false;
			gridCompilerCounters.ShowRowErrors = false;
			gridCompilerCounters.Size = new Size(481, 332);
			gridCompilerCounters.TabIndex = 7;
			// 
			// tabPage5
			// 
			tabPage5.Controls.Add(tbCompilerCounters);
			tabPage5.Location = new Point(4, 27);
			tabPage5.Margin = new Padding(3, 2, 3, 2);
			tabPage5.Name = "tabPage5";
			tabPage5.Padding = new Padding(3, 2, 3, 2);
			tabPage5.Size = new Size(679, 334);
			tabPage5.TabIndex = 1;
			tabPage5.Text = "Text";
			tabPage5.UseVisualStyleBackColor = true;
			// 
			// tbCompilerCounters
			// 
			tbCompilerCounters.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			tbCompilerCounters.Font = new Font("Lucida Console", 8F);
			tbCompilerCounters.Location = new Point(0, 0);
			tbCompilerCounters.Margin = new Padding(4, 3, 4, 3);
			tbCompilerCounters.Name = "tbCompilerCounters";
			tbCompilerCounters.Size = new Size(676, 330);
			tbCompilerCounters.TabIndex = 10;
			tbCompilerCounters.Text = "";
			tbCompilerCounters.WordWrap = false;
			// 
			// toolStrip1
			// 
			toolStrip1.ImageScalingSize = new Size(20, 20);
			toolStrip1.Items.AddRange(new ToolStripItem[] { cbPlatform, toolStripSeparator3, tsbOpen, toolStripSeparator2, tsbRefresh, toolStripSeparator1, tsbCompile });
			toolStrip1.Location = new Point(0, 24);
			toolStrip1.Name = "toolStrip1";
			toolStrip1.Size = new Size(940, 27);
			toolStrip1.TabIndex = 27;
			toolStrip1.Text = "toolStrip1";
			// 
			// cbPlatform
			// 
			cbPlatform.BackColor = SystemColors.Window;
			cbPlatform.DropDownStyle = ComboBoxStyle.DropDownList;
			cbPlatform.FlatStyle = FlatStyle.Standard;
			cbPlatform.Items.AddRange(new object[] { "x86", "x64", "ARM32" });
			cbPlatform.Name = "cbPlatform";
			cbPlatform.Size = new Size(104, 27);
			cbPlatform.SelectedIndexChanged += cbPlatform_SelectedIndexChanged;
			// 
			// toolStripSeparator3
			// 
			toolStripSeparator3.Name = "toolStripSeparator3";
			toolStripSeparator3.Size = new Size(6, 27);
			// 
			// tsbOpen
			// 
			tsbOpen.Image = (Image)resources.GetObject("tsbOpen.Image");
			tsbOpen.ImageTransparentColor = Color.Magenta;
			tsbOpen.Name = "tsbOpen";
			tsbOpen.Size = new Size(60, 24);
			tsbOpen.Text = "Open";
			tsbOpen.Click += ToolStripButton1_Click;
			// 
			// toolStripSeparator2
			// 
			toolStripSeparator2.Name = "toolStripSeparator2";
			toolStripSeparator2.Size = new Size(6, 27);
			// 
			// tsbRefresh
			// 
			tsbRefresh.Image = (Image)resources.GetObject("tsbRefresh.Image");
			tsbRefresh.ImageTransparentColor = Color.Magenta;
			tsbRefresh.Name = "tsbRefresh";
			tsbRefresh.Size = new Size(70, 24);
			tsbRefresh.Text = "Refresh";
			tsbRefresh.Click += tsbRefresh_Click;
			// 
			// toolStripSeparator1
			// 
			toolStripSeparator1.Name = "toolStripSeparator1";
			toolStripSeparator1.Size = new Size(6, 27);
			// 
			// tsbCompile
			// 
			tsbCompile.Image = (Image)resources.GetObject("tsbCompile.Image");
			tsbCompile.ImageTransparentColor = Color.Magenta;
			tsbCompile.Name = "tsbCompile";
			tsbCompile.Size = new Size(76, 24);
			tsbCompile.Text = "Compile";
			tsbCompile.Click += ToolStripButton4_Click;
			// 
			// folderBrowserDialog1
			// 
			folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
			// 
			// timer1
			// 
			timer1.Enabled = true;
			timer1.Tick += timer1_Tick;
			// 
			// MainForm
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(940, 473);
			Controls.Add(toolStrip1);
			Controls.Add(statusStrip1);
			Controls.Add(menuStrip1);
			Controls.Add(splitContainer1);
			Icon = (Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = menuStrip1;
			Margin = new Padding(4, 3, 4, 3);
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

		private StatusStrip statusStrip1;
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem openToolStripMenuItem;
		private ToolStripSeparator toolStripMenuItem1;
		private ToolStripMenuItem quitToolStripMenuItem;
		private OpenFileDialog openFileDialog;
		private ToolStripMenuItem optionsToolStripMenuItem;
		private TreeView treeView;
		private SplitContainer splitContainer1;
		private ToolStripMenuItem compileToolStripMenuItem;
		private ToolStripMenuItem nowToolStripMenuItem;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private ToolStrip toolStrip1;
		private ToolStripButton tsbOpen;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripButton tsbCompile;
		private ToolStripMenuItem cbEnableSSA;
		private ToolStripMenuItem cbEnableBinaryCodeGeneration;
		private ToolStripSeparator toolStripSeparator2;
		private ToolStripMenuItem cbEnableBasicOptimizations;
		private ToolStripProgressBar toolStripProgressBar1;
		private ToolStripStatusLabel toolStripStatusLabel;
		private ToolStripMenuItem cbEnableSparseConditionalConstantPropagation;
		private ToolStripMenuItem cbEnableInline;
		private FolderBrowserDialog folderBrowserDialog1;
		private ToolStripMenuItem advanceToolStripMenuItem;
		private ToolStripMenuItem cbDumpAllMethodStages;
		private ToolStripMenuItem cbEnableLongExpansion;
		private ToolStripMenuItem cbEnableTwoPassOptimizations;
		private ToolStripMenuItem displayOptionsToolStripMenuItem;
		private ToolStripMenuItem showOperandTypes;
		private ToolStripMenuItem showSizes;
		private ToolStripMenuItem cbEnableValueNumbering;
		private ToolStripMenuItem cbEnableMethodScanner;
		private TextBox tbFilter;
		private Label label2;
		private System.Windows.Forms.Timer timer1;
		private ToolStripComboBox cbPlatform;
		private ToolStripSeparator toolStripSeparator3;
		private ToolStripMenuItem cbEnableBitTracker;
		private ToolStripMenuItem cbEnableMultithreading;
		private ToolStripMenuItem cbLoopInvariantCodeMotion;
		private ToolStripMenuItem cbPlatformOptimizations;
		private ToolStripMenuItem cbInlineExplicit;
		private ToolStripMenuItem cbEnableAllOptimizations;
		private ToolStripMenuItem cbDisableAllOptimizations;
		private ToolStripSeparator toolStripSeparator4;
		private ToolStripMenuItem cbEnableDevirtualization;
		private TabControl tabControl;
		private TabPage tabStages;
		private Label label1;
		private ComboBox cbInstructionLabels;
		private ComboBox cbInstructionStages;
		private Label stageLabel;
		private RichTextBox tbInstructions;
		private TabPage tabStageDebug;
		private ComboBox cbDebugStages;
		private Label label3;
		private TabPage tabMethodCounters;
		private TabControl tabControl1;
		private TabPage tabPage1;
		private DataGridView gridMethodCounters;
		private TabPage tabPage2;
		private TabPage tabLogs;
		private ComboBox cbCompilerSections;
		private Label label4;
		private RichTextBox tbMethodCounters;
		private Label label5;
		private TextBox tbCounterFilter;
		private TabPage tabCompilerCounters;
		private TabControl tabControl2;
		private TabPage tabPage4;
		private Label label6;
		private TextBox tbCompilerCounterFilter;
		private DataGridView gridCompilerCounters;
		private TabPage tabPage5;
		private RichTextBox tbCompilerCounters;
		private TabPage tabTransforms;
		private Label label7;
		private ComboBox cbTransformLabels;
		private ComboBox cbTransformStages;
		private Label label8;
		private RichTextBox tbCompilerLogs;
		private Button btnSaveB;
		private Button btnSaveA;
		private SaveFileDialog saveFileDialog1;
		private ToolStripMenuItem removeIRNop;
		private ToolStripMenuItem cbEnableDebugDiagnostic;
		private ToolStripButton tsbRefresh;
		private CheckBox cbGraphviz;
		private RichTextBox tbDebugResult;
		private Panel panel1;
		private SplitContainer splitContainer2;
		private RichTextBox tbTransforms;
		private DataGridView dataGridView1;
		private Label lbSteps;
		private Button btnLast;
		private Button btnNext;
		private Button btnPrevious;
		private Button btnFirst;
		private CheckBox cbSetBlock;
		private Button button1;
		private Button button2;
		private ToolStripMenuItem cbEnableCodeSizeReduction;
		private ToolStripMenuItem cbEnableLoopRangeTracker;
	}
}
