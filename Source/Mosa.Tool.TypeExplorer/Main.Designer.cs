namespace Mosa.Tool.TypeExplorer
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
			System.Windows.Forms.Label stageLabel;
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.snippetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showTokenValues = new System.Windows.Forms.ToolStripMenuItem();
			this.showSizes = new System.Windows.Forms.ToolStripMenuItem();
			this.includeTestKorlibToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.enableSSAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.treeView = new System.Windows.Forms.TreeView();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.cbLabels = new System.Windows.Forms.ComboBox();
			this.cbStages = new System.Windows.Forms.ComboBox();
			this.tbResult = new System.Windows.Forms.RichTextBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.cbPlatform = new System.Windows.Forms.ComboBox();
			stageLabel = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.statusStrip1.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// stageLabel
			// 
			stageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			stageLabel.Location = new System.Drawing.Point(1, 4);
			stageLabel.Margin = new System.Windows.Forms.Padding(4);
			stageLabel.Name = "stageLabel";
			stageLabel.Size = new System.Drawing.Size(158, 20);
			stageLabel.TabIndex = 32;
			stageLabel.Text = "Stage:";
			// 
			// label1
			// 
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.Location = new System.Drawing.Point(260, 4);
			label1.Margin = new System.Windows.Forms.Padding(4);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(120, 20);
			label1.TabIndex = 35;
			label1.Text = "Block Label:";
			// 
			// label2
			// 
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(388, 4);
			label2.Margin = new System.Windows.Forms.Padding(4);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(100, 20);
			label2.TabIndex = 37;
			label2.Text = "IL Label:";
			label2.Visible = false;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 466);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(811, 22);
			this.statusStrip1.TabIndex = 0;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(59, 17);
			this.toolStripStatusLabel1.Text = "Loading...";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.compileToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(811, 24);
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
            this.showTokenValues,
            this.showSizes,
            this.includeTestKorlibToolStripMenuItem,
            this.enableSSAToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// showTokenValues
			// 
			this.showTokenValues.Checked = true;
			this.showTokenValues.CheckOnClick = true;
			this.showTokenValues.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showTokenValues.Name = "showTokenValues";
			this.showTokenValues.Size = new System.Drawing.Size(210, 22);
			this.showTokenValues.Text = "Show Token Values";
			this.showTokenValues.Click += new System.EventHandler(this.showTokenValues_Click);
			// 
			// showSizes
			// 
			this.showSizes.Checked = true;
			this.showSizes.CheckOnClick = true;
			this.showSizes.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showSizes.Name = "showSizes";
			this.showSizes.Size = new System.Drawing.Size(210, 22);
			this.showSizes.Text = "Show Sizes";
			this.showSizes.Click += new System.EventHandler(this.showSizes_Click);
			// 
			// includeTestKorlibToolStripMenuItem
			// 
			this.includeTestKorlibToolStripMenuItem.CheckOnClick = true;
			this.includeTestKorlibToolStripMenuItem.Name = "includeTestKorlibToolStripMenuItem";
			this.includeTestKorlibToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.includeTestKorlibToolStripMenuItem.Text = "Include Test Components";
			// 
			// enableSSAToolStripMenuItem
			// 
			this.enableSSAToolStripMenuItem.Name = "enableSSAToolStripMenuItem";
			this.enableSSAToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.enableSSAToolStripMenuItem.Text = "Enable SSA";
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
			this.treeView.Location = new System.Drawing.Point(3, 3);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(256, 405);
			this.treeView.TabIndex = 3;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 52);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView);
			this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(label2);
			this.splitContainer1.Panel2.Controls.Add(this.textBox1);
			this.splitContainer1.Panel2.Controls.Add(label1);
			this.splitContainer1.Panel2.Controls.Add(this.cbLabels);
			this.splitContainer1.Panel2.Controls.Add(this.cbStages);
			this.splitContainer1.Panel2.Controls.Add(stageLabel);
			this.splitContainer1.Panel2.Controls.Add(this.tbResult);
			this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer1.Size = new System.Drawing.Size(807, 411);
			this.splitContainer1.SplitterDistance = 261;
			this.splitContainer1.SplitterWidth = 5;
			this.splitContainer1.TabIndex = 26;
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(388, 25);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(100, 20);
			this.textBox1.TabIndex = 36;
			this.textBox1.Visible = false;
			// 
			// cbLabels
			// 
			this.cbLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbLabels.FormattingEnabled = true;
			this.cbLabels.Location = new System.Drawing.Point(259, 25);
			this.cbLabels.Margin = new System.Windows.Forms.Padding(4);
			this.cbLabels.MaxDropDownItems = 20;
			this.cbLabels.Name = "cbLabels";
			this.cbLabels.Size = new System.Drawing.Size(121, 21);
			this.cbLabels.TabIndex = 33;
			this.cbLabels.SelectedIndexChanged += new System.EventHandler(this.cbLabels_SelectedIndexChanged);
			// 
			// cbStages
			// 
			this.cbStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbStages.FormattingEnabled = true;
			this.cbStages.Location = new System.Drawing.Point(4, 25);
			this.cbStages.Margin = new System.Windows.Forms.Padding(4);
			this.cbStages.MaxDropDownItems = 20;
			this.cbStages.Name = "cbStages";
			this.cbStages.Size = new System.Drawing.Size(248, 21);
			this.cbStages.TabIndex = 31;
			this.cbStages.SelectedIndexChanged += new System.EventHandler(this.cbStages_SelectedIndexChanged);
			// 
			// tbResult
			// 
			this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbResult.Font = new System.Drawing.Font("Lucida Console", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbResult.Location = new System.Drawing.Point(0, 60);
			this.tbResult.Margin = new System.Windows.Forms.Padding(0);
			this.tbResult.Name = "tbResult";
			this.tbResult.Size = new System.Drawing.Size(541, 348);
			this.tbResult.TabIndex = 30;
			this.tbResult.Text = "";
			this.tbResult.WordWrap = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.toolStripButton4});
			this.toolStrip1.Location = new System.Drawing.Point(0, 24);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(811, 25);
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
			this.toolStripButton4.Size = new System.Drawing.Size(75, 22);
			this.toolStripButton4.Text = "Compile:";
			this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
			// 
			// cbPlatform
			// 
			this.cbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbPlatform.FormattingEnabled = true;
			this.cbPlatform.Items.AddRange(new object[] {
            "x86",
            "AVR32"});
			this.cbPlatform.Location = new System.Drawing.Point(213, 26);
			this.cbPlatform.Name = "cbPlatform";
			this.cbPlatform.Size = new System.Drawing.Size(58, 21);
			this.cbPlatform.TabIndex = 28;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(811, 488);
			this.Controls.Add(this.cbPlatform);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.splitContainer1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Main";
			this.Text = "MOSA Type Explorer";
			this.Load += new System.EventHandler(this.Main_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
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
		private System.Windows.Forms.ToolStripMenuItem showTokenValues;
		private System.Windows.Forms.ToolStripMenuItem showSizes;
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ComboBox cbLabels;
		private System.Windows.Forms.ComboBox cbStages;
		private System.Windows.Forms.RichTextBox tbResult;
		private System.Windows.Forms.ToolStripMenuItem compileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nowToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripMenuItem snippetToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.ToolStripMenuItem includeTestKorlibToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ComboBox cbPlatform;
		private System.Windows.Forms.ToolStripMenuItem enableSSAToolStripMenuItem;
	}
}