namespace Mosa.Tool.TinySimulator
{
	partial class DisassemblyView : SimulatorDockContent
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
			this.tbLastInstruction = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbNextInstruction = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbLastInstruction
			// 
			this.tbLastInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLastInstruction.Location = new System.Drawing.Point(3, 18);
			this.tbLastInstruction.Name = "tbLastInstruction";
			this.tbLastInstruction.ReadOnly = true;
			this.tbLastInstruction.Size = new System.Drawing.Size(234, 20);
			this.tbLastInstruction.TabIndex = 27;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 2);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(82, 13);
			this.label1.TabIndex = 28;
			this.label1.Text = "Last Instruction:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 41);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(84, 13);
			this.label2.TabIndex = 30;
			this.label2.Text = "Next Instruction:";
			// 
			// tbNextInstruction
			// 
			this.tbNextInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbNextInstruction.Location = new System.Drawing.Point(3, 57);
			this.tbNextInstruction.Name = "tbNextInstruction";
			this.tbNextInstruction.ReadOnly = true;
			this.tbNextInstruction.Size = new System.Drawing.Size(234, 20);
			this.tbNextInstruction.TabIndex = 29;
			// 
			// DisassemblyView
			// 
			this.ClientSize = new System.Drawing.Size(240, 100);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.tbNextInstruction);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbLastInstruction);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(100, 50);
			this.Name = "DisassemblyView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Status";
			this.Text = "Disassembly View";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.TextBox tbLastInstruction;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbNextInstruction;
	}
}