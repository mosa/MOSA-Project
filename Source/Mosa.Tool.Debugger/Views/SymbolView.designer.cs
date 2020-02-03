namespace Mosa.Tool.Debugger.Views
{
	partial class SymbolView : DebugDockContent
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.cbKind = new System.Windows.Forms.ToolStripComboBox();
			this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
			this.cbLength = new System.Windows.Forms.ToolStripComboBox();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 56);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowHeadersWidth = 92;
			this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8F);
			this.dataGridView1.RowTemplate.Height = 18;
			this.dataGridView1.Size = new System.Drawing.Size(890, 204);
			this.dataGridView1.TabIndex = 2;
			this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
			// 
			// toolStrip1
			// 
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.toolStripTextBox1,
            this.toolStripLabel3,
            this.cbKind,
            this.toolStripLabel4,
            this.cbLength});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(890, 56);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(82, 50);
			this.toolStripLabel1.Text = "Filter:";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 56);
			// 
			// toolStripLabel2
			// 
			this.toolStripLabel2.Name = "toolStripLabel2";
			this.toolStripLabel2.Size = new System.Drawing.Size(200, 50);
			this.toolStripLabel2.Text = "Symbols Name:";
			// 
			// toolStripTextBox1
			// 
			this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.Window;
			this.toolStripTextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.toolStripTextBox1.Margin = new System.Windows.Forms.Padding(1, 3, 1, 2);
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new System.Drawing.Size(200, 51);
			this.toolStripTextBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// toolStripLabel3
			// 
			this.toolStripLabel3.Name = "toolStripLabel3";
			this.toolStripLabel3.Size = new System.Drawing.Size(171, 50);
			this.toolStripLabel3.Text = "Section Kind:";
			// 
			// cbKind
			// 
			this.cbKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbKind.Items.AddRange(new object[] {
            "<Any>",
            "Text",
            "BSS",
            "ROData"});
			this.cbKind.Name = "cbKind";
			this.cbKind.Size = new System.Drawing.Size(75, 56);
			this.cbKind.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox1_SelectedIndexChanged);
			// 
			// toolStripLabel4
			// 
			this.toolStripLabel4.Name = "toolStripLabel4";
			this.toolStripLabel4.Size = new System.Drawing.Size(105, 37);
			this.toolStripLabel4.Text = "Length:";
			// 
			// cbLength
			// 
			this.cbLength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLength.Items.AddRange(new object[] {
            "<Any>",
            "1-Byte",
            "2-Word",
            "4-Integer",
            "8-Long"});
			this.cbLength.Name = "cbLength";
			this.cbLength.Size = new System.Drawing.Size(75, 45);
			this.cbLength.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBox2_SelectedIndexChanged);
			// 
			// SymbolView
			// 
			this.ClientSize = new System.Drawing.Size(890, 260);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.toolStrip1);
			this.Name = "SymbolView";
			this.TabText = "Symbols";
			this.Text = "Symbols";
			this.Load += new System.EventHandler(this.SymbolView_Load);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripLabel toolStripLabel2;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
		private System.Windows.Forms.ToolStripComboBox cbKind;
		private System.Windows.Forms.ToolStripLabel toolStripLabel3;
		private System.Windows.Forms.ToolStripLabel toolStripLabel4;
		private System.Windows.Forms.ToolStripComboBox cbLength;


	}
}