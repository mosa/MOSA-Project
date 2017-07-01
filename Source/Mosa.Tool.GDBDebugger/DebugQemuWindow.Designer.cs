namespace Mosa.Tool.GDBDebugger
{
    partial class DebugQemuWindow
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
            this.tbImage = new System.Windows.Forms.TextBox();
            this.btnImageBrowse = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnQEMU = new System.Windows.Forms.Button();
            this.tbQEMU = new System.Windows.Forms.TextBox();
            this.btnBIOSDirectory = new System.Windows.Forms.Button();
            this.tbBIOSDirectory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbImage
            // 
            this.tbImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImage.BackColor = System.Drawing.Color.White;
            this.tbImage.Location = new System.Drawing.Point(101, 64);
            this.tbImage.Name = "tbImage";
            this.tbImage.ReadOnly = true;
            this.tbImage.Size = new System.Drawing.Size(282, 20);
            this.tbImage.TabIndex = 1;
            this.tbImage.TextChanged += new System.EventHandler(this.CheckDebugButton);
            // 
            // btnImageBrowse
            // 
            this.btnImageBrowse.Location = new System.Drawing.Point(20, 62);
            this.btnImageBrowse.Name = "btnImageBrowse";
            this.btnImageBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnImageBrowse.TabIndex = 2;
            this.btnImageBrowse.Text = "Image:";
            this.btnImageBrowse.UseVisualStyleBackColor = true;
            this.btnImageBrowse.Click += new System.EventHandler(this.btnImageBrowse_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(105, 104);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 26);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDebug
            // 
            this.btnDebug.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnDebug.Enabled = false;
            this.btnDebug.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDebug.Location = new System.Drawing.Point(212, 104);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(101, 26);
            this.btnDebug.TabIndex = 6;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnQEMU
            // 
            this.btnQEMU.Location = new System.Drawing.Point(20, 10);
            this.btnQEMU.Name = "btnQEMU";
            this.btnQEMU.Size = new System.Drawing.Size(75, 23);
            this.btnQEMU.TabIndex = 9;
            this.btnQEMU.Text = "QEMU:";
            this.btnQEMU.UseVisualStyleBackColor = true;
            // 
            // tbQEMU
            // 
            this.tbQEMU.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbQEMU.BackColor = System.Drawing.Color.White;
            this.tbQEMU.Location = new System.Drawing.Point(101, 12);
            this.tbQEMU.Name = "tbQEMU";
            this.tbQEMU.ReadOnly = true;
            this.tbQEMU.Size = new System.Drawing.Size(282, 20);
            this.tbQEMU.TabIndex = 8;
            this.tbQEMU.TextChanged += new System.EventHandler(this.CheckDebugButton);
            // 
            // btnBIOSDirectory
            // 
            this.btnBIOSDirectory.Location = new System.Drawing.Point(20, 36);
            this.btnBIOSDirectory.Name = "btnBIOSDirectory";
            this.btnBIOSDirectory.Size = new System.Drawing.Size(75, 23);
            this.btnBIOSDirectory.TabIndex = 11;
            this.btnBIOSDirectory.Text = "BIOS:";
            this.btnBIOSDirectory.UseVisualStyleBackColor = true;
            // 
            // tbBIOSDirectory
            // 
            this.tbBIOSDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbBIOSDirectory.BackColor = System.Drawing.Color.White;
            this.tbBIOSDirectory.Location = new System.Drawing.Point(101, 38);
            this.tbBIOSDirectory.Name = "tbBIOSDirectory";
            this.tbBIOSDirectory.ReadOnly = true;
            this.tbBIOSDirectory.Size = new System.Drawing.Size(282, 20);
            this.tbBIOSDirectory.TabIndex = 10;
            this.tbBIOSDirectory.TextChanged += new System.EventHandler(this.CheckDebugButton);
            // 
            // DebugQemuWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 140);
            this.Controls.Add(this.btnBIOSDirectory);
            this.Controls.Add(this.tbBIOSDirectory);
            this.Controls.Add(this.btnQEMU);
            this.Controls.Add(this.tbQEMU);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.btnImageBrowse);
            this.Controls.Add(this.tbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DebugQemuWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Debug QEMU";
            this.Load += new System.EventHandler(this.DebugQemuWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbImage;
        private System.Windows.Forms.Button btnImageBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnQEMU;
        private System.Windows.Forms.TextBox tbQEMU;
        private System.Windows.Forms.Button btnBIOSDirectory;
        private System.Windows.Forms.TextBox tbBIOSDirectory;
    }
}