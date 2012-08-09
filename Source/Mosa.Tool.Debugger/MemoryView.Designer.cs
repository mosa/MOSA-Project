/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Tool.Debugger
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
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.cbSelect = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.tbMemory = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.lbMemory = new System.Windows.Forms.TextBox();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.cbSelect,
            this.toolStripSeparator2,
            this.toolStripLabel1,
            this.tbMemory,
            this.toolStripSeparator1,
            this.toolStripButton1});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(524, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(36, 22);
			this.toolStripLabel2.Text = "Goto:";
			// 
			// cbSelect
			// 
			this.cbSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelect.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.cbSelect.Items.AddRange(new object[] {
            "VGA Memory",
            "Multiboot Structure",
            "Page Directory (CR3)",
            "Physical Page Free List",
            "Virtual Page Bitmap"});
			this.cbSelect.Name = "cbSelect";
			this.cbSelect.Size = new System.Drawing.Size(160, 25);
			this.cbSelect.SelectedIndexChanged += new System.EventHandler(this.cbSelect_SelectedIndexChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(55, 22);
			this.toolStripLabel1.Text = "Memory:";
			// 
			// tbMemory
			// 
			this.tbMemory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.tbMemory.MaxLength = 25;
			this.tbMemory.Name = "tbMemory";
			this.tbMemory.Size = new System.Drawing.Size(120, 25);
			this.tbMemory.Text = "0x200000";
			this.tbMemory.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbMemory.TextChanged += new System.EventHandler(this.tbMemory_TextChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Black;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(50, 22);
			this.toolStripButton1.Text = "Refresh";
			this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
			// 
			// lbMemory
			// 
			this.lbMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbMemory.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbMemory.Location = new System.Drawing.Point(0, 28);
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
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "MemoryView";
			this.Text = "Memory View";
			this.Load += new System.EventHandler(this.MemoryView_Load);
			this.ResizeEnd += new System.EventHandler(this.MemoryForm_ResizeEnd);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripTextBox tbMemory;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.TextBox lbMemory;
		private System.Windows.Forms.ToolStripComboBox cbSelect;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
	}
}