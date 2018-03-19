namespace Mosa.Tool.GDBDebugger
{
    partial class DebugVMImageWindow
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
            this.tbImageFile = new System.Windows.Forms.TextBox();
            this.btnImageBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbDebugInfoFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbImageFile
            // 
            this.tbImageFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImageFile.BackColor = System.Drawing.Color.White;
            this.tbImageFile.Location = new System.Drawing.Point(93, 15);
            this.tbImageFile.Name = "tbImageFile";
            this.tbImageFile.ReadOnly = true;
            this.tbImageFile.Size = new System.Drawing.Size(313, 20);
            this.tbImageFile.TabIndex = 1;
            this.tbImageFile.TextChanged += new System.EventHandler(this.CheckDebugButton);
            // 
            // btnImageBrowse
            // 
            this.btnImageBrowse.Location = new System.Drawing.Point(12, 12);
            this.btnImageBrowse.Name = "btnImageBrowse";
            this.btnImageBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnImageBrowse.TabIndex = 2;
            this.btnImageBrowse.Text = "Image:";
            this.btnImageBrowse.UseVisualStyleBackColor = true;
            this.btnImageBrowse.Click += new System.EventHandler(this.btnImageBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(196, 75);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 26);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebug.Enabled = false;
            this.btnDebug.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDebug.Location = new System.Drawing.Point(305, 75);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(101, 26);
            this.btnDebug.TabIndex = 6;
            this.btnDebug.Text = "Launch";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Debug Info:";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tbDebugInfoFile
            // 
            this.tbDebugInfoFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDebugInfoFile.BackColor = System.Drawing.Color.White;
            this.tbDebugInfoFile.Location = new System.Drawing.Point(93, 43);
            this.tbDebugInfoFile.Name = "tbDebugInfoFile";
            this.tbDebugInfoFile.ReadOnly = true;
            this.tbDebugInfoFile.Size = new System.Drawing.Size(313, 20);
            this.tbDebugInfoFile.TabIndex = 9;
            this.tbDebugInfoFile.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // DebugVMImageWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 113);
            this.Controls.Add(this.tbDebugInfoFile);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.btnImageBrowse);
            this.Controls.Add(this.tbImageFile);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DebugVMImageWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VM Image";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbImageFile;
        private System.Windows.Forms.Button btnImageBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDebug;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox tbDebugInfoFile;
	}
}
