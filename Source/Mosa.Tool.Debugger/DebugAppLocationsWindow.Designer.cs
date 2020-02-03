namespace Mosa.Tool.Debugger
{
    partial class DebugAppLocationsWindow
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
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnQEMU = new System.Windows.Forms.Button();
            this.tbQEMU = new System.Windows.Forms.TextBox();
            this.btnBIOSDirectory = new System.Windows.Forms.Button();
            this.tbBIOSDirectory = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnDebug
            // 
            this.btnDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDebug.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDebug.Location = new System.Drawing.Point(305, 75);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(101, 26);
            this.btnDebug.TabIndex = 6;
            this.btnDebug.Text = "Close";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnQEMU
            // 
            this.btnQEMU.Location = new System.Drawing.Point(12, 12);
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
            this.tbQEMU.Location = new System.Drawing.Point(93, 15);
            this.tbQEMU.Name = "tbQEMU";
            this.tbQEMU.ReadOnly = true;
            this.tbQEMU.Size = new System.Drawing.Size(313, 20);
            this.tbQEMU.TabIndex = 8;
            // 
            // btnBIOSDirectory
            // 
            this.btnBIOSDirectory.Location = new System.Drawing.Point(12, 41);
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
            this.tbBIOSDirectory.Location = new System.Drawing.Point(93, 43);
            this.tbBIOSDirectory.Name = "tbBIOSDirectory";
            this.tbBIOSDirectory.ReadOnly = true;
            this.tbBIOSDirectory.Size = new System.Drawing.Size(313, 20);
            this.tbBIOSDirectory.TabIndex = 10;
            // 
            // DebugAppLocationsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 113);
            this.Controls.Add(this.btnBIOSDirectory);
            this.Controls.Add(this.tbBIOSDirectory);
            this.Controls.Add(this.btnQEMU);
            this.Controls.Add(this.tbQEMU);
            this.Controls.Add(this.btnDebug);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DebugAppLocationsWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "App Locations";
            this.Load += new System.EventHandler(this.DebugQemuWindow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.Button btnQEMU;
        private System.Windows.Forms.TextBox tbQEMU;
        private System.Windows.Forms.Button btnBIOSDirectory;
        private System.Windows.Forms.TextBox tbBIOSDirectory;
    }
}
