// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger.Views
{
	partial class SourceDataView
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
			this.label1 = new System.Windows.Forms.Label();
			this.lbSourceFilename = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbMethodID = new System.Windows.Forms.Label();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.dataGridView2 = new System.Windows.Forms.DataGridView();
			this.lbOffset = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(560, 20);
			this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 29);
			this.label1.TabIndex = 1;
			this.label1.Text = "Source:";
			// 
			// lbSourceFilename
			// 
			this.lbSourceFilename.AutoSize = true;
			this.lbSourceFilename.Location = new System.Drawing.Point(702, 20);
			this.lbSourceFilename.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.lbSourceFilename.Name = "lbSourceFilename";
			this.lbSourceFilename.Size = new System.Drawing.Size(171, 29);
			this.lbSourceFilename.TabIndex = 2;
			this.lbSourceFilename.Text = "<Source File>";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(8, 20);
			this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(137, 29);
			this.label2.TabIndex = 6;
			this.label2.Text = "MethodID:";
			// 
			// lbMethodID
			// 
			this.lbMethodID.AutoSize = true;
			this.lbMethodID.Location = new System.Drawing.Point(178, 20);
			this.lbMethodID.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.lbMethodID.Name = "lbMethodID";
			this.lbMethodID.Size = new System.Drawing.Size(67, 29);
			this.lbMethodID.TabIndex = 7;
			this.lbMethodID.Text = "<ID>";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.Location = new System.Drawing.Point(0, 76);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.dataGridView2);
			this.splitContainer1.Size = new System.Drawing.Size(1120, 756);
			this.splitContainer1.SplitterDistance = 376;
			this.splitContainer1.SplitterWidth = 9;
			this.splitContainer1.TabIndex = 8;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(8, -7);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersWidth = 92;
			this.dataGridView1.Size = new System.Drawing.Size(1108, 389);
			this.dataGridView1.TabIndex = 5;
			// 
			// dataGridView2
			// 
			this.dataGridView2.AllowUserToAddRows = false;
			this.dataGridView2.AllowUserToDeleteRows = false;
			this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView2.Location = new System.Drawing.Point(8, -2);
			this.dataGridView2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.dataGridView2.Name = "dataGridView2";
			this.dataGridView2.ReadOnly = true;
			this.dataGridView2.RowHeadersWidth = 92;
			this.dataGridView2.Size = new System.Drawing.Size(1108, 376);
			this.dataGridView2.TabIndex = 6;
			// 
			// lbOffset
			// 
			this.lbOffset.AutoSize = true;
			this.lbOffset.Location = new System.Drawing.Point(395, 20);
			this.lbOffset.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.lbOffset.Name = "lbOffset";
			this.lbOffset.Size = new System.Drawing.Size(105, 29);
			this.lbOffset.TabIndex = 10;
			this.lbOffset.Text = "<offset>";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(268, 20);
			this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(94, 29);
			this.label4.TabIndex = 9;
			this.label4.Text = "Offset:";
			// 
			// SourceDataView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(15F, 29F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1120, 830);
			this.Controls.Add(this.lbOffset);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.lbMethodID);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbSourceFilename);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
			this.Name = "SourceDataView";
			this.TabText = "Source Data";
			this.Text = "Source Data";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lbSourceFilename;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbMethodID;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridView dataGridView2;
		private System.Windows.Forms.Label lbOffset;
		private System.Windows.Forms.Label label4;
	}
}
