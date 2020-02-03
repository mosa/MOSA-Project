// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.Views
{
	partial class SourceView
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
				components.Dispose();

			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.rtbSource = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.lbSourceFilename = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// rtbSource
			// 
			this.rtbSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.rtbSource.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rtbSource.Location = new System.Drawing.Point(-2, 67);
			this.rtbSource.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.rtbSource.Name = "rtbSource";
			this.rtbSource.ReadOnly = true;
			this.rtbSource.Size = new System.Drawing.Size(1324, 432);
			this.rtbSource.TabIndex = 0;
			this.rtbSource.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(20, 20);
			this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 29);
			this.label1.TabIndex = 1;
			this.label1.Text = "Source:";
			// 
			// lbSourceFilename
			// 
			this.lbSourceFilename.AutoSize = true;
			this.lbSourceFilename.Location = new System.Drawing.Point(145, 20);
			this.lbSourceFilename.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.lbSourceFilename.Name = "lbSourceFilename";
			this.lbSourceFilename.Size = new System.Drawing.Size(171, 29);
			this.lbSourceFilename.TabIndex = 2;
			this.lbSourceFilename.Text = "<Source File>";
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 599);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 35, 0);
			this.statusStrip1.Size = new System.Drawing.Size(1332, 48);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(65, 37);
			this.toolStripStatusLabel1.Text = "N/A";
			// 
			// SourceView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1332, 647);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.lbSourceFilename);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.rtbSource);
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.Name = "SourceView";
			this.TabText = "Source";
			this.Text = "Source";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.RichTextBox rtbSource;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbSourceFilename;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
	}
}
