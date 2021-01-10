// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.Views
{
	partial class MemoryView
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
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tbAddress = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.lbMemory = new System.Windows.Forms.TextBox();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.tbAddress,
            this.toolStripSeparator1,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(524, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(52, 20);
			this.toolStripLabel1.Text = "Address:";
			// 
			// tbAddress
			// 
			this.tbAddress.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tbAddress.MaxLength = 20;
			this.tbAddress.Name = "tbAddress";
			this.tbAddress.Size = new System.Drawing.Size(100, 23);
			this.tbAddress.Text = "0x200000";
			this.tbAddress.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbMemory_KeyDown);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.Image = global::Mosa.Tool.Debugger.Properties.Resources.page_refresh;
			this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Black;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(66, 20);
			this.toolStripButton1.Text = "Refresh";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// lbMemory
			// 
			this.lbMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMemory.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbMemory.Location = new System.Drawing.Point(0, 27);
			this.lbMemory.Multiline = true;
			this.lbMemory.Name = "lbMemory";
			this.lbMemory.Size = new System.Drawing.Size(524, 366);
			this.lbMemory.TabIndex = 1;
			// 
			// MemoryView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(524, 395);
			this.Controls.Add(this.lbMemory);
			this.Controls.Add(this.toolStrip1);
			this.Name = "MemoryView";
			this.TabText = "Memory";
			this.Text = "Memory";
			this.Load += new System.EventHandler(this.MemoryView_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox tbAddress;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.TextBox lbMemory;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
	}
}