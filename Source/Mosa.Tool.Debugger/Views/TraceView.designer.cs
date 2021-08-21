namespace Mosa.Tool.Debugger.Views
{
	partial class TraceView : DebugDockContent
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.btnStart = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnStop = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.btnClear = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.btnSave = new System.Windows.Forms.ToolStripButton();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator7,
            this.btnStart,
            this.toolStripSeparator1,
            this.btnStop,
            this.toolStripSeparator5,
            this.btnClear,
            this.toolStripSeparator3,
            this.btnSave});
			this.toolStrip1.Location = new System.Drawing.Point(0, 2);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 2, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(624, 25);
			this.toolStrip1.TabIndex = 6;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 23);
			// 
			// btnStart
			// 
			this.btnStart.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnStart.ImageTransparentColor = System.Drawing.Color.Black;
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(35, 20);
			this.btnStart.Text = "Start";
			this.btnStart.Visible = false;
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
			this.toolStripSeparator1.Visible = false;
			// 
			// btnStop
			// 
			this.btnStop.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_stop_blue;
			this.btnStop.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnStop.ImageTransparentColor = System.Drawing.Color.Black;
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(51, 20);
			this.btnStop.Text = "Stop";
			this.btnStop.Visible = false;
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
			this.toolStripSeparator5.Visible = false;
			// 
			// btnClear
			// 
			this.btnClear.Image = global::Mosa.Tool.Debugger.Properties.Resources.layer_remove;
			this.btnClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnClear.ImageTransparentColor = System.Drawing.Color.Black;
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(54, 20);
			this.btnClear.Text = "Clear";
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
			// 
			// btnSave
			// 
			this.btnSave.Image = global::Mosa.Tool.Debugger.Properties.Resources.layer_save;
			this.btnSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnSave.ImageTransparentColor = System.Drawing.Color.Black;
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(51, 20);
			this.btnSave.Text = "Save";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridView1.Location = new System.Drawing.Point(0, 27);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(0);
			this.dataGridView1.MultiSelect = false;
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowHeadersWidth = 92;
			this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.dataGridView1.RowTemplate.Height = 18;
			this.dataGridView1.Size = new System.Drawing.Size(624, 176);
			this.dataGridView1.TabIndex = 7;
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "debug";
			this.saveFileDialog.FileName = "Trace.txt";
			this.saveFileDialog.Filter = "Tab Delimited |*.txt";
			// 
			// TraceView
			// 
			this.ClientSize = new System.Drawing.Size(624, 205);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.toolStrip1);
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(100, 20);
			this.Name = "TraceView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottom;
			this.TabText = "Trace";
			this.Text = "Trace View";
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnClear;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton btnStart;
		private System.Windows.Forms.ToolStripButton btnSave;
		private System.Windows.Forms.ToolStripButton btnStop;
	}
}
