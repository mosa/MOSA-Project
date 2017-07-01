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
            this.SuspendLayout();
            // 
            // tbImage
            // 
            this.tbImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbImage.BackColor = System.Drawing.Color.White;
            this.tbImage.Location = new System.Drawing.Point(99, 18);
            this.tbImage.Name = "tbImage";
            this.tbImage.ReadOnly = true;
            this.tbImage.Size = new System.Drawing.Size(282, 20);
            this.tbImage.TabIndex = 1;
            this.tbImage.TextChanged += new System.EventHandler(this.CheckDebugButton);
            // 
            // btnImageBrowse
            // 
            this.btnImageBrowse.Location = new System.Drawing.Point(18, 16);
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
            this.btnCancel.Location = new System.Drawing.Point(105, 78);
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
            this.btnDebug.Location = new System.Drawing.Point(212, 78);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(101, 26);
            this.btnDebug.TabIndex = 6;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // DebugQemuWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 114);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDebug);
            this.Controls.Add(this.btnImageBrowse);
            this.Controls.Add(this.tbImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DebugQemuWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Debug QEMU";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbImage;
        private System.Windows.Forms.Button btnImageBrowse;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDebug;
    }
}