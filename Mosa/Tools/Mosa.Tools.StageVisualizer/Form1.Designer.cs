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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.panel1 = new System.Windows.Forms.Panel();
			this.cbStage = new System.Windows.Forms.CheckBox();
			this.cbBlock = new System.Windows.Forms.CheckBox();
			this.cbBlocks = new System.Windows.Forms.ComboBox();
			this.cbLabel = new System.Windows.Forms.CheckBox();
			this.cbLabels = new System.Windows.Forms.ComboBox();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cbMethods = new System.Windows.Forms.ComboBox();
			this.btnLoad = new System.Windows.Forms.Button();
			this.cbStages = new System.Windows.Forms.ComboBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.Result = new System.Windows.Forms.TabPage();
			this.tbResult = new System.Windows.Forms.RichTextBox();
			this.Source = new System.Windows.Forms.TabPage();
			this.tbSource = new System.Windows.Forms.RichTextBox();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lbStatus = new System.Windows.Forms.ToolStripStatusLabel();
			this.cbRemoveNextPrev = new System.Windows.Forms.CheckBox();
			this.cbSpace = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.Result.SuspendLayout();
			this.Source.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.cbSpace);
			this.panel1.Controls.Add(this.cbRemoveNextPrev);
			this.panel1.Controls.Add(this.cbStage);
			this.panel1.Controls.Add(this.cbBlock);
			this.panel1.Controls.Add(this.cbBlocks);
			this.panel1.Controls.Add(this.cbLabel);
			this.panel1.Controls.Add(this.cbLabels);
			this.panel1.Controls.Add(this.btnUpdate);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.cbMethods);
			this.panel1.Controls.Add(this.btnLoad);
			this.panel1.Controls.Add(this.cbStages);
			this.panel1.Location = new System.Drawing.Point(1, 2);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(260, 439);
			this.panel1.TabIndex = 1;
			// 
			// cbStage
			// 
			this.cbStage.AutoSize = true;
			this.cbStage.Checked = true;
			this.cbStage.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbStage.Location = new System.Drawing.Point(11, 118);
			this.cbStage.Name = "cbStage";
			this.cbStage.Size = new System.Drawing.Size(57, 17);
			this.cbStage.TabIndex = 11;
			this.cbStage.Text = "Stage:";
			this.cbStage.UseVisualStyleBackColor = true;
			this.cbStage.CheckedChanged += new System.EventHandler(this.btnUpdate_Click);
			// 
			// cbBlock
			// 
			this.cbBlock.AutoSize = true;
			this.cbBlock.Location = new System.Drawing.Point(10, 368);
			this.cbBlock.Name = "cbBlock";
			this.cbBlock.Size = new System.Drawing.Size(56, 17);
			this.cbBlock.TabIndex = 10;
			this.cbBlock.Text = "Block:";
			this.cbBlock.UseVisualStyleBackColor = true;
			this.cbBlock.Visible = false;
			this.cbBlock.CheckedChanged += new System.EventHandler(this.btnUpdate_Click);
			// 
			// cbBlocks
			// 
			this.cbBlocks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBlocks.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbBlocks.FormattingEnabled = true;
			this.cbBlocks.Location = new System.Drawing.Point(4, 388);
			this.cbBlocks.MaxDropDownItems = 20;
			this.cbBlocks.Name = "cbBlocks";
			this.cbBlocks.Size = new System.Drawing.Size(245, 23);
			this.cbBlocks.TabIndex = 9;
			this.cbBlocks.Visible = false;
			// 
			// cbLabel
			// 
			this.cbLabel.AutoSize = true;
			this.cbLabel.Location = new System.Drawing.Point(11, 174);
			this.cbLabel.Name = "cbLabel";
			this.cbLabel.Size = new System.Drawing.Size(55, 17);
			this.cbLabel.TabIndex = 8;
			this.cbLabel.Text = "Label:";
			this.cbLabel.UseVisualStyleBackColor = true;
			this.cbLabel.CheckedChanged += new System.EventHandler(this.btnUpdate_Click);
			// 
			// cbLabels
			// 
			this.cbLabels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLabels.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbLabels.FormattingEnabled = true;
			this.cbLabels.Location = new System.Drawing.Point(6, 197);
			this.cbLabels.MaxDropDownItems = 20;
			this.cbLabels.Name = "cbLabels";
			this.cbLabels.Size = new System.Drawing.Size(245, 23);
			this.cbLabels.TabIndex = 6;
			this.cbLabels.SelectionChangeCommitted += new System.EventHandler(this.cbLabels_SelectionChangeCommitted);
			// 
			// btnUpdate
			// 
			this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
			this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnUpdate.Location = new System.Drawing.Point(17, 304);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(213, 41);
			this.btnUpdate.TabIndex = 5;
			this.btnUpdate.Text = "Update";
			this.btnUpdate.UseVisualStyleBackColor = true;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(11, 68);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(46, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Method:";
			// 
			// cbMethods
			// 
			this.cbMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbMethods.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbMethods.FormattingEnabled = true;
			this.cbMethods.Location = new System.Drawing.Point(5, 84);
			this.cbMethods.MaxDropDownItems = 20;
			this.cbMethods.Name = "cbMethods";
			this.cbMethods.Size = new System.Drawing.Size(246, 23);
			this.cbMethods.TabIndex = 3;
			this.cbMethods.SelectionChangeCommitted += new System.EventHandler(this.cbMethods_SelectionChangeCommitted);
			// 
			// btnLoad
			// 
			this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
			this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLoad.Location = new System.Drawing.Point(17, 11);
			this.btnLoad.Name = "btnLoad";
			this.btnLoad.Size = new System.Drawing.Size(213, 41);
			this.btnLoad.TabIndex = 1;
			this.btnLoad.Text = "Load Output";
			this.btnLoad.UseVisualStyleBackColor = true;
			this.btnLoad.Click += new System.EventHandler(this.button1_Click);
			// 
			// cbStages
			// 
			this.cbStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbStages.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbStages.FormattingEnabled = true;
			this.cbStages.Location = new System.Drawing.Point(5, 138);
			this.cbStages.MaxDropDownItems = 20;
			this.cbStages.Name = "cbStages";
			this.cbStages.Size = new System.Drawing.Size(245, 23);
			this.cbStages.TabIndex = 0;
			this.cbStages.SelectionChangeCommitted += new System.EventHandler(this.cbStages_SelectionChangeCommitted);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.Result);
			this.tabControl1.Controls.Add(this.Source);
			this.tabControl1.Location = new System.Drawing.Point(257, 2);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(525, 439);
			this.tabControl1.TabIndex = 2;
			// 
			// Result
			// 
			this.Result.Controls.Add(this.tbResult);
			this.Result.Location = new System.Drawing.Point(4, 22);
			this.Result.Name = "Result";
			this.Result.Padding = new System.Windows.Forms.Padding(3);
			this.Result.Size = new System.Drawing.Size(517, 413);
			this.Result.TabIndex = 0;
			this.Result.Text = "Result";
			this.Result.UseVisualStyleBackColor = true;
			// 
			// tbResult
			// 
			this.tbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbResult.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbResult.Location = new System.Drawing.Point(0, 0);
			this.tbResult.Name = "tbResult";
			this.tbResult.Size = new System.Drawing.Size(516, 395);
			this.tbResult.TabIndex = 2;
			this.tbResult.Text = "";
			this.tbResult.WordWrap = false;
			// 
			// Source
			// 
			this.Source.Controls.Add(this.tbSource);
			this.Source.Location = new System.Drawing.Point(4, 22);
			this.Source.Name = "Source";
			this.Source.Padding = new System.Windows.Forms.Padding(3);
			this.Source.Size = new System.Drawing.Size(517, 413);
			this.Source.TabIndex = 1;
			this.Source.Text = "Source";
			this.Source.UseVisualStyleBackColor = true;
			// 
			// tbSource
			// 
			this.tbSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.tbSource.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbSource.Location = new System.Drawing.Point(0, 0);
			this.tbSource.Name = "tbSource";
			this.tbSource.Size = new System.Drawing.Size(521, 413);
			this.tbSource.TabIndex = 1;
			this.tbSource.Text = "";
			this.tbSource.WordWrap = false;
			this.tbSource.TextChanged += new System.EventHandler(this.tbSource_TextChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbStatus});
			this.statusStrip1.Location = new System.Drawing.Point(0, 422);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(782, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lbStatus
			// 
			this.lbStatus.Name = "lbStatus";
			this.lbStatus.Size = new System.Drawing.Size(118, 17);
			this.lbStatus.Text = "toolStripStatusLabel1";
			// 
			// cbRemoveNextPrev
			// 
			this.cbRemoveNextPrev.AutoSize = true;
			this.cbRemoveNextPrev.Checked = true;
			this.cbRemoveNextPrev.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbRemoveNextPrev.Location = new System.Drawing.Point(11, 238);
			this.cbRemoveNextPrev.Name = "cbRemoveNextPrev";
			this.cbRemoveNextPrev.Size = new System.Drawing.Size(169, 17);
			this.cbRemoveNextPrev.TabIndex = 12;
			this.cbRemoveNextPrev.Text = "Remove next/prev information";
			this.cbRemoveNextPrev.UseVisualStyleBackColor = true;
			this.cbRemoveNextPrev.CheckStateChanged += new System.EventHandler(this.btnUpdate_Click);
			// 
			// cbSpace
			// 
			this.cbSpace.AutoSize = true;
			this.cbSpace.Checked = true;
			this.cbSpace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbSpace.Location = new System.Drawing.Point(11, 261);
			this.cbSpace.Name = "cbSpace";
			this.cbSpace.Size = new System.Drawing.Size(130, 17);
			this.cbSpace.TabIndex = 13;
			this.cbSpace.Text = "Add space after block";
			this.cbSpace.UseVisualStyleBackColor = true;
			this.cbSpace.CheckedChanged += new System.EventHandler(this.btnUpdate_Click);
			// 
			// frmMain
			// 
			this.AcceptButton = this.btnUpdate;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(782, 444);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.panel1);
			this.Name = "frmMain";
			this.Text = "MOSA Compiler Visualizer";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.Result.ResumeLayout(false);
			this.Source.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox cbMethods;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.ComboBox cbStages;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox cbLabel;
        private System.Windows.Forms.ComboBox cbLabels;
        private System.Windows.Forms.CheckBox cbStage;
        private System.Windows.Forms.CheckBox cbBlock;
        private System.Windows.Forms.ComboBox cbBlocks;
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
    }
}

