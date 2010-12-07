namespace Mosa.Tools.StageVisualizer
{
	partial class frmMain
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
			System.Windows.Forms.Label methodLabel;
			System.Windows.Forms.Label labelLabel;
			System.Windows.Forms.Label stageLabel;
			System.Windows.Forms.GroupBox formatOptionsGroupBox;
			this.cbRemoveNextPrev = new System.Windows.Forms.CheckBox();
			this.cbSpace = new System.Windows.Forms.CheckBox();
			this.cbStage = new System.Windows.Forms.CheckBox();
			this.cbLabel = new System.Windows.Forms.CheckBox();
			this.cbLabels = new System.Windows.Forms.ComboBox();
			this.refreshButton = new System.Windows.Forms.Button();
			this.cbMethods = new System.Windows.Forms.ComboBox();
			this.loadButton = new System.Windows.Forms.Button();
			this.cbStages = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Result = new System.Windows.Forms.TabPage();
			this.tbResult = new System.Windows.Forms.RichTextBox();
			this.Source = new System.Windows.Forms.TabPage();
			this.tbSource = new System.Windows.Forms.RichTextBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.panel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.saveButton = new System.Windows.Forms.Button();
			this.resetButton = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			methodLabel = new System.Windows.Forms.Label();
			labelLabel = new System.Windows.Forms.Label();
			stageLabel = new System.Windows.Forms.Label();
			formatOptionsGroupBox = new System.Windows.Forms.GroupBox();
			formatOptionsGroupBox.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.Result.SuspendLayout();
			this.Source.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// methodLabel
			// 
			methodLabel.AutoSize = true;
			methodLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			methodLabel.Location = new System.Drawing.Point(10, 110);
			methodLabel.Margin = new System.Windows.Forms.Padding(4);
			methodLabel.Name = "methodLabel";
			methodLabel.Size = new System.Drawing.Size(59, 17);
			methodLabel.TabIndex = 4;
			methodLabel.Text = "Method:";
			// 
			// labelLabel
			// 
			labelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			labelLabel.Location = new System.Drawing.Point(10, 228);
			labelLabel.Margin = new System.Windows.Forms.Padding(4);
			labelLabel.Name = "labelLabel";
			labelLabel.Size = new System.Drawing.Size(158, 20);
			labelLabel.TabIndex = 17;
			labelLabel.Text = "Label:";
			// 
			// stageLabel
			// 
			stageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			stageLabel.Location = new System.Drawing.Point(10, 167);
			stageLabel.Margin = new System.Windows.Forms.Padding(4);
			stageLabel.Name = "stageLabel";
			stageLabel.Size = new System.Drawing.Size(158, 20);
			stageLabel.TabIndex = 18;
			stageLabel.Text = "Stage:";
			// 
			// formatOptionsGroupBox
			// 
			formatOptionsGroupBox.Controls.Add(this.cbRemoveNextPrev);
			formatOptionsGroupBox.Controls.Add(this.cbSpace);
			formatOptionsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			formatOptionsGroupBox.Location = new System.Drawing.Point(10, 289);
			formatOptionsGroupBox.Margin = new System.Windows.Forms.Padding(4);
			formatOptionsGroupBox.Name = "formatOptionsGroupBox";
			formatOptionsGroupBox.Padding = new System.Windows.Forms.Padding(4);
			formatOptionsGroupBox.Size = new System.Drawing.Size(248, 191);
			formatOptionsGroupBox.TabIndex = 19;
			formatOptionsGroupBox.TabStop = false;
			formatOptionsGroupBox.Text = "Formatting Options";
			// 
			// cbRemoveNextPrev
			// 
			this.cbRemoveNextPrev.AutoSize = true;
			this.cbRemoveNextPrev.Checked = true;
			this.cbRemoveNextPrev.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRemoveNextPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbRemoveNextPrev.Location = new System.Drawing.Point(8, 59);
			this.cbRemoveNextPrev.Margin = new System.Windows.Forms.Padding(4);
			this.cbRemoveNextPrev.Name = "cbRemoveNextPrev";
			this.cbRemoveNextPrev.Size = new System.Drawing.Size(172, 21);
			this.cbRemoveNextPrev.TabIndex = 12;
			this.cbRemoveNextPrev.Text = "Remove next/prev info.";
			this.cbRemoveNextPrev.UseVisualStyleBackColor = true;
			this.cbRemoveNextPrev.CheckStateChanged += new System.EventHandler(this.refreshButton_Click);
			// 
			// cbSpace
			// 
			this.cbSpace.AutoSize = true;
			this.cbSpace.Checked = true;
			this.cbSpace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSpace.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbSpace.Location = new System.Drawing.Point(8, 27);
			this.cbSpace.Margin = new System.Windows.Forms.Padding(4);
			this.cbSpace.Name = "cbSpace";
			this.cbSpace.Size = new System.Drawing.Size(164, 21);
			this.cbSpace.TabIndex = 13;
			this.cbSpace.Text = "Add space after block";
			this.cbSpace.UseVisualStyleBackColor = true;
			this.cbSpace.CheckedChanged += new System.EventHandler(this.refreshButton_Click);
			// 
			// cbStage
			// 
			this.cbStage.AutoSize = true;
			this.cbStage.Checked = true;
			this.cbStage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbStage.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbStage.Location = new System.Drawing.Point(176, 167);
			this.cbStage.Margin = new System.Windows.Forms.Padding(4);
			this.cbStage.Name = "cbStage";
			this.cbStage.Size = new System.Drawing.Size(73, 21);
			this.cbStage.TabIndex = 11;
			this.cbStage.Text = "Display";
			this.cbStage.UseVisualStyleBackColor = true;
			this.cbStage.CheckedChanged += new System.EventHandler(this.refreshButton_Click);
			// 
			// cbLabel
			// 
			this.cbLabel.AutoSize = true;
			this.cbLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbLabel.Location = new System.Drawing.Point(176, 228);
			this.cbLabel.Margin = new System.Windows.Forms.Padding(4);
			this.cbLabel.Name = "cbLabel";
			this.cbLabel.Size = new System.Drawing.Size(73, 21);
			this.cbLabel.TabIndex = 8;
			this.cbLabel.Text = "Display";
			this.cbLabel.UseVisualStyleBackColor = true;
			this.cbLabel.CheckedChanged += new System.EventHandler(this.refreshButton_Click);
			// 
			// cbLabels
			// 
			this.cbLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLabels.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbLabels.FormattingEnabled = true;
			this.cbLabels.Location = new System.Drawing.Point(10, 257);
			this.cbLabels.Margin = new System.Windows.Forms.Padding(4);
			this.cbLabels.MaxDropDownItems = 20;
			this.cbLabels.Name = "cbLabels";
			this.cbLabels.Size = new System.Drawing.Size(248, 24);
			this.cbLabels.TabIndex = 6;
			this.cbLabels.SelectionChangeCommitted += new System.EventHandler(this.cbLabels_SelectionChangeCommitted);
			// 
			// refreshButton
			// 
			this.refreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.refreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.refreshButton.Location = new System.Drawing.Point(10, 60);
			this.refreshButton.Margin = new System.Windows.Forms.Padding(4);
			this.refreshButton.Name = "refreshButton";
			this.refreshButton.Size = new System.Drawing.Size(120, 42);
			this.refreshButton.TabIndex = 5;
			this.refreshButton.Text = "Refresh";
			this.refreshButton.UseVisualStyleBackColor = true;
			this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
			// 
			// cbMethods
			// 
			this.cbMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMethods.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbMethods.FormattingEnabled = true;
			this.cbMethods.Location = new System.Drawing.Point(10, 135);
			this.cbMethods.Margin = new System.Windows.Forms.Padding(4);
			this.cbMethods.MaxDropDownItems = 20;
			this.cbMethods.Name = "cbMethods";
			this.cbMethods.Size = new System.Drawing.Size(248, 24);
			this.cbMethods.TabIndex = 3;
			this.cbMethods.SelectionChangeCommitted += new System.EventHandler(this.cbMethods_SelectionChangeCommitted);
			// 
			// loadButton
			// 
			this.loadButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.loadButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.loadButton.Location = new System.Drawing.Point(10, 10);
			this.loadButton.Margin = new System.Windows.Forms.Padding(4);
			this.loadButton.Name = "loadButton";
			this.loadButton.Size = new System.Drawing.Size(120, 42);
			this.loadButton.TabIndex = 1;
			this.loadButton.Text = "Load...";
			this.loadButton.UseVisualStyleBackColor = true;
			this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
			// 
			// cbStages
			// 
			this.cbStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStages.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbStages.FormattingEnabled = true;
			this.cbStages.Location = new System.Drawing.Point(10, 196);
			this.cbStages.Margin = new System.Windows.Forms.Padding(4);
			this.cbStages.MaxDropDownItems = 20;
			this.cbStages.Name = "cbStages";
			this.cbStages.Size = new System.Drawing.Size(248, 24);
			this.cbStages.TabIndex = 0;
			this.cbStages.SelectionChangeCommitted += new System.EventHandler(this.cbStages_SelectionChangeCommitted);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.Result);
			this.tabControl1.Controls.Add(this.Source);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabControl1.Location = new System.Drawing.Point(268, 0);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(5);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(746, 578);
			this.tabControl1.TabIndex = 2;
			// 
			// Result
			// 
			this.Result.Controls.Add(this.tbResult);
			this.Result.Location = new System.Drawing.Point(4, 25);
			this.Result.Margin = new System.Windows.Forms.Padding(5);
			this.Result.Name = "Result";
			this.Result.Padding = new System.Windows.Forms.Padding(5);
			this.Result.Size = new System.Drawing.Size(738, 549);
			this.Result.TabIndex = 0;
			this.Result.Text = "Result";
			this.Result.UseVisualStyleBackColor = true;
			// 
			// tbResult
			// 
			this.tbResult.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbResult.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbResult.Location = new System.Drawing.Point(5, 5);
			this.tbResult.Margin = new System.Windows.Forms.Padding(5);
			this.tbResult.Name = "tbResult";
			this.tbResult.Size = new System.Drawing.Size(728, 539);
			this.tbResult.TabIndex = 2;
			this.tbResult.Text = "";
			this.tbResult.WordWrap = false;
			// 
			// Source
			// 
			this.Source.Controls.Add(this.tbSource);
			this.Source.Location = new System.Drawing.Point(4, 25);
			this.Source.Margin = new System.Windows.Forms.Padding(5);
			this.Source.Name = "Source";
			this.Source.Padding = new System.Windows.Forms.Padding(5);
			this.Source.Size = new System.Drawing.Size(738, 549);
			this.Source.TabIndex = 1;
			this.Source.Text = "Source";
			this.Source.UseVisualStyleBackColor = true;
			// 
			// tbSource
			// 
			this.tbSource.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbSource.Location = new System.Drawing.Point(5, 5);
			this.tbSource.Margin = new System.Windows.Forms.Padding(5);
			this.tbSource.Name = "tbSource";
			this.tbSource.Size = new System.Drawing.Size(728, 539);
			this.tbSource.TabIndex = 1;
			this.tbSource.Text = "";
			this.tbSource.WordWrap = false;
			this.tbSource.TextChanged += new System.EventHandler(this.tbSource_TextChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 578);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(3, 0, 24, 0);
			this.statusStrip1.Size = new System.Drawing.Size(1014, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lbStatus
			// 
			this.lbStatus.Name = "lbStatus";
			this.lbStatus.Size = new System.Drawing.Size(145, 17);
			this.lbStatus.Text = "MOSA Compiler Visualizer";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.loadButton);
			this.panel1.Controls.Add(this.saveButton);
			this.panel1.Controls.Add(this.refreshButton);
			this.panel1.Controls.Add(this.resetButton);
			this.panel1.Controls.Add(methodLabel);
			this.panel1.Controls.Add(this.cbMethods);
			this.panel1.Controls.Add(stageLabel);
			this.panel1.Controls.Add(this.cbStage);
			this.panel1.Controls.Add(this.cbStages);
			this.panel1.Controls.Add(labelLabel);
			this.panel1.Controls.Add(this.cbLabel);
			this.panel1.Controls.Add(this.cbLabels);
			this.panel1.Controls.Add(formatOptionsGroupBox);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Margin = new System.Windows.Forms.Padding(0);
			this.panel1.Name = "panel1";
			this.panel1.Padding = new System.Windows.Forms.Padding(6);
			this.panel1.Size = new System.Drawing.Size(268, 578);
			this.panel1.TabIndex = 1;
			// 
			// saveButton
			// 
			this.saveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.saveButton.Location = new System.Drawing.Point(138, 10);
			this.saveButton.Margin = new System.Windows.Forms.Padding(4);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(120, 42);
			this.saveButton.TabIndex = 14;
			this.saveButton.Text = "Save...";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// resetButton
			// 
			this.resetButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.resetButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.resetButton.Location = new System.Drawing.Point(138, 60);
			this.resetButton.Margin = new System.Windows.Forms.Padding(4);
			this.resetButton.Name = "resetButton";
			this.resetButton.Size = new System.Drawing.Size(120, 42);
			this.resetButton.TabIndex = 15;
			this.resetButton.Text = "Reset";
			this.resetButton.UseVisualStyleBackColor = true;
			// 
			// frmMain
			// 
			this.AcceptButton = this.refreshButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1014, 600);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.statusStrip1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margin = new System.Windows.Forms.Padding(5);
			this.Name = "frmMain";
			this.Text = "MOSA Compiler Visualizer";
			formatOptionsGroupBox.ResumeLayout(false);
			formatOptionsGroupBox.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.Result.ResumeLayout(false);
			this.Source.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.ComboBox cbMethods;
		private System.Windows.Forms.Button loadButton;
		private System.Windows.Forms.ComboBox cbStages;
        private System.Windows.Forms.Button refreshButton;
		private System.Windows.Forms.CheckBox cbLabel;
		private System.Windows.Forms.ComboBox cbLabels;
		private System.Windows.Forms.CheckBox cbStage;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage Result;
		private System.Windows.Forms.TabPage Source;
		private System.Windows.Forms.RichTextBox tbSource;
		private System.Windows.Forms.RichTextBox tbResult;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel lbStatus;
		private System.Windows.Forms.CheckBox cbRemoveNextPrev;
		private System.Windows.Forms.CheckBox cbSpace;
        private System.Windows.Forms.FlowLayoutPanel panel1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button resetButton;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
	}
}

