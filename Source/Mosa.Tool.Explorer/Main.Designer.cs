namespace Mosa.Tool.Explorer
{
	partial class Main
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
			System.Windows.Forms.Label label2;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label stageLabel;
			System.Windows.Forms.Label label3;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
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
			this.showSizes = new System.Windows.Forms.ToolStripMenuItem();
			this.displayShortName = new System.Windows.Forms.ToolStripMenuItem();
			this.includeTestKorlibToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableSSA = new System.Windows.Forms.ToolStripMenuItem();
			this.enableOptimizations = new System.Windows.Forms.ToolStripMenuItem();
			this.enableSparseConditionalConstantPropagation = new System.Windows.Forms.ToolStripMenuItem();
			this.enableBinaryCodeGeneration = new System.Windows.Forms.ToolStripMenuItem();
			this.enableVariablePromotion = new System.Windows.Forms.ToolStripMenuItem();
			this.enableInlinedMethods = new System.Windows.Forms.ToolStripMenuItem();
			this.advanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dumpAllMethodStagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeView = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tbStages = new System.Windows.Forms.TabPage();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.cbLabels = new System.Windows.Forms.ComboBox();
			this.cbStages = new System.Windows.Forms.ComboBox();
			this.tbResult = new System.Windows.Forms.RichTextBox();
			this.tbDebug = new System.Windows.Forms.TabPage();
			this.cbDebugStages = new System.Windows.Forms.ComboBox();
			this.rbOtherResult = new System.Windows.Forms.RichTextBox();
			this.tbMethodCounters = new System.Windows.Forms.TabPage();
			this.rbMethodCounters = new System.Windows.Forms.RichTextBox();
			this.tbGlobalCounters = new System.Windows.Forms.TabPage();
			this.rbGlobalCounters = new System.Windows.Forms.RichTextBox();
			this.tbLogs = new System.Windows.Forms.TabPage();
			this.rbLog = new System.Windows.Forms.RichTextBox();
			this.tbErrors = new System.Windows.Forms.TabPage();
			this.rbErrors = new System.Windows.Forms.RichTextBox();
			this.tbExceptions = new System.Windows.Forms.TabPage();
			this.rbException = new System.Windows.Forms.RichTextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.cbPlatform = new System.Windows.Forms.ComboBox();
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			label2 = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			stageLabel = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tbStages.SuspendLayout();
			this.tbDebug.SuspendLayout();
			this.tbMethodCounters.SuspendLayout();
			this.tbGlobalCounters.SuspendLayout();
			this.tbLogs.SuspendLayout();
			this.tbErrors.SuspendLayout();
			this.tbExceptions.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label2
			// 
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(467, 8);
			label2.Margin = new System.Windows.Forms.Padding(4);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(63, 20);
			label2.TabIndex = 43;
			label2.Text = "IL Label:";
			label2.Visible = false;
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
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1,
            this.toolStripStatusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 477);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(975, 22);
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
            this.advanceToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(975, 24);
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
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
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
			this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
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
			this.nowToolStripMenuItem.Click += new System.EventHandler(this.nowToolStripMenuItem_Click);
			// 
			// snippetToolStripMenuItem
			// 
			this.snippetToolStripMenuItem.Name = "snippetToolStripMenuItem";
			this.snippetToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
			this.snippetToolStripMenuItem.Text = "Snippet";
			this.snippetToolStripMenuItem.Click += new System.EventHandler(this.snippetToolStripMenuItem_Click);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSizes,
            this.displayShortName,
            this.includeTestKorlibToolStripMenuItem,
            this.enableSSA,
            this.enableOptimizations,
            this.enableSparseConditionalConstantPropagation,
            this.enableBinaryCodeGeneration,
            this.enableVariablePromotion,
            this.enableInlinedMethods});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// showSizes
			// 
			this.showSizes.Checked = true;
			this.showSizes.CheckOnClick = true;
			this.showSizes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showSizes.Name = "showSizes";
			this.showSizes.Size = new System.Drawing.Size(293, 22);
			this.showSizes.Text = "Show Sizes";
			this.showSizes.Click += new System.EventHandler(this.showSizes_Click);
			// 
			// displayShortName
			// 
			this.displayShortName.Checked = true;
			this.displayShortName.CheckOnClick = true;
			this.displayShortName.CheckState = System.Windows.Forms.CheckState.Checked;
			this.displayShortName.Name = "displayShortName";
			this.displayShortName.Size = new System.Drawing.Size(293, 22);
			this.displayShortName.Text = "Display Short Name";
			// 
			// includeTestKorlibToolStripMenuItem
			// 
			this.includeTestKorlibToolStripMenuItem.CheckOnClick = true;
			this.includeTestKorlibToolStripMenuItem.Name = "includeTestKorlibToolStripMenuItem";
			this.includeTestKorlibToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
			this.includeTestKorlibToolStripMenuItem.Text = "Include Test Components";
			// 
			// enableSSA
			// 
			this.enableSSA.Checked = true;
			this.enableSSA.CheckOnClick = true;
			this.enableSSA.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableSSA.Name = "enableSSA";
			this.enableSSA.Size = new System.Drawing.Size(293, 22);
			this.enableSSA.Text = "Enable SSA";
			// 
			// enableOptimizations
			// 
			this.enableOptimizations.Checked = true;
			this.enableOptimizations.CheckOnClick = true;
			this.enableOptimizations.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableOptimizations.Name = "enableOptimizations";
			this.enableOptimizations.Size = new System.Drawing.Size(293, 22);
			this.enableOptimizations.Text = "Enable Optimizations";
			// 
			// enableSparseConditionalConstantPropagation
			// 
			this.enableSparseConditionalConstantPropagation.Checked = true;
			this.enableSparseConditionalConstantPropagation.CheckOnClick = true;
			this.enableSparseConditionalConstantPropagation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableSparseConditionalConstantPropagation.Name = "enableSparseConditionalConstantPropagation";
			this.enableSparseConditionalConstantPropagation.Size = new System.Drawing.Size(293, 22);
			this.enableSparseConditionalConstantPropagation.Text = "Enable Conditional Constant Propagation";
			// 
			// enableBinaryCodeGeneration
			// 
			this.enableBinaryCodeGeneration.Checked = true;
			this.enableBinaryCodeGeneration.CheckOnClick = true;
			this.enableBinaryCodeGeneration.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableBinaryCodeGeneration.Name = "enableBinaryCodeGeneration";
			this.enableBinaryCodeGeneration.Size = new System.Drawing.Size(293, 22);
			this.enableBinaryCodeGeneration.Text = "Enable Binary Code Generation";
			// 
			// enableVariablePromotion
			// 
			this.enableVariablePromotion.Checked = true;
			this.enableVariablePromotion.CheckOnClick = true;
			this.enableVariablePromotion.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableVariablePromotion.Name = "enableVariablePromotion";
			this.enableVariablePromotion.Size = new System.Drawing.Size(293, 22);
			this.enableVariablePromotion.Text = "Enable Variable Promotion";
			// 
			// enableInlinedMethods
			// 
			this.enableInlinedMethods.Checked = true;
			this.enableInlinedMethods.CheckOnClick = true;
			this.enableInlinedMethods.CheckState = System.Windows.Forms.CheckState.Checked;
			this.enableInlinedMethods.Name = "enableInlinedMethods";
			this.enableInlinedMethods.Size = new System.Drawing.Size(293, 22);
			this.enableInlinedMethods.Text = "Enable Inlined Methods";
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
			this.dumpAllMethodStagesToolStripMenuItem.Click += new System.EventHandler(this.dumpAllMethodStagesToolStripMenuItem_Click);
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
			this.treeView.Size = new System.Drawing.Size(262, 425);
			this.treeView.TabIndex = 3;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
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
			this.splitContainer1.Size = new System.Drawing.Size(971, 422);
			this.splitContainer1.SplitterDistance = 259;
			this.splitContainer1.SplitterWidth = 6;
			this.splitContainer1.TabIndex = 26;
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
			this.tabControl1.Size = new System.Drawing.Size(691, 425);
			this.tabControl1.TabIndex = 38;
			// 
			// tbStages
			// 
			this.tbStages.BackColor = System.Drawing.Color.Gainsboro;
			this.tbStages.Controls.Add(label2);
			this.tbStages.Controls.Add(this.textBox1);
			this.tbStages.Controls.Add(label1);
			this.tbStages.Controls.Add(this.cbLabels);
			this.tbStages.Controls.Add(this.cbStages);
			this.tbStages.Controls.Add(stageLabel);
			this.tbStages.Controls.Add(this.tbResult);
			this.tbStages.Location = new System.Drawing.Point(4, 25);
			this.tbStages.Margin = new System.Windows.Forms.Padding(0);
			this.tbStages.Name = "tbStages";
			this.tbStages.Size = new System.Drawing.Size(683, 396);
			this.tbStages.TabIndex = 0;
			this.tbStages.Text = "Instructions";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(537, 5);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(76, 23);
			this.textBox1.TabIndex = 42;
			this.textBox1.Visible = false;
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
			this.cbLabels.SelectedIndexChanged += new System.EventHandler(this.cbLabels_SelectedIndexChanged);
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
			this.cbStages.SelectedIndexChanged += new System.EventHandler(this.cbStages_SelectedIndexChanged);
			// 
			// tbResult
			// 
			this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbResult.Location = new System.Drawing.Point(0, 32);
			this.tbResult.Name = "tbResult";
			this.tbResult.Size = new System.Drawing.Size(693, 368);
			this.tbResult.TabIndex = 31;
			this.tbResult.Text = "";
			this.tbResult.WordWrap = false;
			// 
			// tbDebug
			// 
			this.tbDebug.BackColor = System.Drawing.Color.Gainsboro;
			this.tbDebug.Controls.Add(this.cbDebugStages);
			this.tbDebug.Controls.Add(label3);
			this.tbDebug.Controls.Add(this.rbOtherResult);
			this.tbDebug.Location = new System.Drawing.Point(4, 25);
			this.tbDebug.Margin = new System.Windows.Forms.Padding(0);
			this.tbDebug.Name = "tbDebug";
			this.tbDebug.Size = new System.Drawing.Size(683, 396);
			this.tbDebug.TabIndex = 1;
			this.tbDebug.Text = "Debug";
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
			this.cbDebugStages.SelectedIndexChanged += new System.EventHandler(this.cbDebugStages_SelectedIndexChanged);
			// 
			// rbOtherResult
			// 
			this.rbOtherResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rbOtherResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbOtherResult.Location = new System.Drawing.Point(0, 32);
			this.rbOtherResult.Name = "rbOtherResult";
			this.rbOtherResult.Size = new System.Drawing.Size(697, 365);
			this.rbOtherResult.TabIndex = 32;
			this.rbOtherResult.Text = "";
			this.rbOtherResult.WordWrap = false;
			// 
			// tbMethodCounters
			// 
			this.tbMethodCounters.Controls.Add(this.rbMethodCounters);
			this.tbMethodCounters.Location = new System.Drawing.Point(4, 25);
			this.tbMethodCounters.Name = "tbMethodCounters";
			this.tbMethodCounters.Size = new System.Drawing.Size(683, 396);
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
			this.rbMethodCounters.Size = new System.Drawing.Size(687, 394);
			this.rbMethodCounters.TabIndex = 2;
			this.rbMethodCounters.Text = "";
			this.rbMethodCounters.WordWrap = false;
			// 
			// tbGlobalCounters
			// 
			this.tbGlobalCounters.BackColor = System.Drawing.Color.Gainsboro;
			this.tbGlobalCounters.Controls.Add(this.rbGlobalCounters);
			this.tbGlobalCounters.Location = new System.Drawing.Point(4, 25);
			this.tbGlobalCounters.Name = "tbGlobalCounters";
			this.tbGlobalCounters.Padding = new System.Windows.Forms.Padding(3);
			this.tbGlobalCounters.Size = new System.Drawing.Size(683, 396);
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
			this.rbGlobalCounters.Size = new System.Drawing.Size(687, 394);
			this.rbGlobalCounters.TabIndex = 1;
			this.rbGlobalCounters.Text = "";
			this.rbGlobalCounters.WordWrap = false;
			// 
			// tbLogs
			// 
			this.tbLogs.BackColor = System.Drawing.Color.Gainsboro;
			this.tbLogs.Controls.Add(this.rbLog);
			this.tbLogs.Location = new System.Drawing.Point(4, 25);
			this.tbLogs.Name = "tbLogs";
			this.tbLogs.Padding = new System.Windows.Forms.Padding(3);
			this.tbLogs.Size = new System.Drawing.Size(683, 396);
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
			this.rbLog.Size = new System.Drawing.Size(687, 394);
			this.rbLog.TabIndex = 1;
			this.rbLog.Text = "";
			this.rbLog.WordWrap = false;
			// 
			// tbErrors
			// 
			this.tbErrors.BackColor = System.Drawing.Color.Gainsboro;
			this.tbErrors.Controls.Add(this.rbErrors);
			this.tbErrors.Location = new System.Drawing.Point(4, 25);
			this.tbErrors.Name = "tbErrors";
			this.tbErrors.Padding = new System.Windows.Forms.Padding(3);
			this.tbErrors.Size = new System.Drawing.Size(683, 396);
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
			this.rbErrors.Size = new System.Drawing.Size(687, 394);
			this.rbErrors.TabIndex = 0;
			this.rbErrors.Text = "";
			this.rbErrors.WordWrap = false;
			// 
			// tbExceptions
			// 
			this.tbExceptions.Controls.Add(this.rbException);
			this.tbExceptions.Location = new System.Drawing.Point(4, 25);
			this.tbExceptions.Name = "tbExceptions";
			this.tbExceptions.Padding = new System.Windows.Forms.Padding(3);
			this.tbExceptions.Size = new System.Drawing.Size(683, 396);
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
			this.rbException.Size = new System.Drawing.Size(697, 394);
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
			this.toolStrip1.Size = new System.Drawing.Size(975, 25);
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
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(67, 22);
			this.toolStripButton2.Text = "Snippet";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
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
			this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
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
			// 
			// folderBrowserDialog1
			// 
			this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(975, 499);
			this.Controls.Add(this.cbPlatform);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.splitContainer1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Main";
			this.Text = "MOSA Explorer v1.5.3";
			this.Load += new System.EventHandler(this.Main_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tbStages.ResumeLayout(false);
			this.tbStages.PerformLayout();
			this.tbDebug.ResumeLayout(false);
			this.tbMethodCounters.ResumeLayout(false);
			this.tbGlobalCounters.ResumeLayout(false);
			this.tbLogs.ResumeLayout(false);
			this.tbErrors.ResumeLayout(false);
			this.tbExceptions.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripMenuItem showSizes;
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
		private System.Windows.Forms.ToolStripMenuItem includeTestKorlibToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ComboBox cbPlatform;
		private System.Windows.Forms.ToolStripMenuItem enableSSA;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tbStages;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ComboBox cbLabels;
		private System.Windows.Forms.ComboBox cbStages;
		private System.Windows.Forms.RichTextBox tbResult;
		private System.Windows.Forms.TabPage tbDebug;
		private System.Windows.Forms.ComboBox cbDebugStages;
		private System.Windows.Forms.RichTextBox rbOtherResult;
		private System.Windows.Forms.ToolStripMenuItem enableBinaryCodeGeneration;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem enableOptimizations;
		private System.Windows.Forms.ToolStripMenuItem displayShortName;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
		private System.Windows.Forms.TabPage tbErrors;
		private System.Windows.Forms.RichTextBox rbErrors;
		private System.Windows.Forms.TabPage tbLogs;
		private System.Windows.Forms.RichTextBox rbLog;
		private System.Windows.Forms.TabPage tbGlobalCounters;
		private System.Windows.Forms.RichTextBox rbGlobalCounters;
		private System.Windows.Forms.ToolStripMenuItem enableSparseConditionalConstantPropagation;
		private System.Windows.Forms.TabPage tbExceptions;
		private System.Windows.Forms.RichTextBox rbException;
		private System.Windows.Forms.ToolStripMenuItem enableInlinedMethods;
		private System.Windows.Forms.ToolStripMenuItem enableVariablePromotion;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.ToolStripMenuItem advanceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dumpAllMethodStagesToolStripMenuItem;
		private System.Windows.Forms.TabPage tbMethodCounters;
		private System.Windows.Forms.RichTextBox rbMethodCounters;
	}
}
