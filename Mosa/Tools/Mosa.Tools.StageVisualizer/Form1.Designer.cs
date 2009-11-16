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
            this.Source = new System.Windows.Forms.TabPage();
            this.tbSource = new System.Windows.Forms.RichTextBox();
            this.Result = new System.Windows.Forms.TabPage();
            this.tbResult = new System.Windows.Forms.RichTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.Source.SuspendLayout();
            this.Result.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
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
            this.panel1.Size = new System.Drawing.Size(221, 439);
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
            this.cbBlock.Location = new System.Drawing.Point(11, 212);
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
            this.cbBlocks.FormattingEnabled = true;
            this.cbBlocks.Location = new System.Drawing.Point(11, 232);
            this.cbBlocks.MaxDropDownItems = 20;
            this.cbBlocks.Name = "cbBlocks";
            this.cbBlocks.Size = new System.Drawing.Size(202, 21);
            this.cbBlocks.TabIndex = 9;
            this.cbBlocks.Visible = false;
            // 
            // cbLabel
            // 
            this.cbLabel.AutoSize = true;
            this.cbLabel.Location = new System.Drawing.Point(11, 165);
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
            this.cbLabels.FormattingEnabled = true;
            this.cbLabels.Location = new System.Drawing.Point(11, 185);
            this.cbLabels.MaxDropDownItems = 20;
            this.cbLabels.Name = "cbLabels";
            this.cbLabels.Size = new System.Drawing.Size(202, 21);
            this.cbLabels.TabIndex = 6;
            this.cbLabels.SelectionChangeCommitted += new System.EventHandler(this.cbLabels_SelectionChangeCommitted);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUpdate.Location = new System.Drawing.Point(17, 270);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(190, 41);
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
            this.cbMethods.FormattingEnabled = true;
            this.cbMethods.Location = new System.Drawing.Point(11, 84);
            this.cbMethods.MaxDropDownItems = 20;
            this.cbMethods.Name = "cbMethods";
            this.cbMethods.Size = new System.Drawing.Size(202, 21);
            this.cbMethods.TabIndex = 3;
            this.cbMethods.SelectionChangeCommitted += new System.EventHandler(this.cbMethods_SelectionChangeCommitted);
            // 
            // btnLoad
            // 
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoad.Location = new System.Drawing.Point(17, 11);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(190, 41);
            this.btnLoad.TabIndex = 1;
            this.btnLoad.Text = "Load Output";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbStages
            // 
            this.cbStages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbStages.FormattingEnabled = true;
            this.cbStages.Location = new System.Drawing.Point(11, 138);
            this.cbStages.MaxDropDownItems = 20;
            this.cbStages.Name = "cbStages";
            this.cbStages.Size = new System.Drawing.Size(202, 21);
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
            this.tabControl1.Location = new System.Drawing.Point(228, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(554, 439);
            this.tabControl1.TabIndex = 2;
            // 
            // Source
            // 
            this.Source.Controls.Add(this.tbSource);
            this.Source.Location = new System.Drawing.Point(4, 22);
            this.Source.Name = "Source";
            this.Source.Padding = new System.Windows.Forms.Padding(3);
            this.Source.Size = new System.Drawing.Size(546, 413);
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
            this.tbSource.Size = new System.Drawing.Size(543, 413);
            this.tbSource.TabIndex = 1;
            this.tbSource.Text = "";
            this.tbSource.WordWrap = false;
            // 
            // Result
            // 
            this.Result.Controls.Add(this.tbResult);
            this.Result.Location = new System.Drawing.Point(4, 22);
            this.Result.Name = "Result";
            this.Result.Padding = new System.Windows.Forms.Padding(3);
            this.Result.Size = new System.Drawing.Size(546, 413);
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
            this.tbResult.Location = new System.Drawing.Point(2, 0);
            this.tbResult.Name = "tbResult";
            this.tbResult.Size = new System.Drawing.Size(543, 413);
            this.tbResult.TabIndex = 2;
            this.tbResult.Text = "";
            this.tbResult.WordWrap = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 444);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "frmMain";
            this.Text = "MOSA Compiler Visualizer";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.Source.ResumeLayout(false);
            this.Result.ResumeLayout(false);
            this.ResumeLayout(false);

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
    }
}

