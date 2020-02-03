namespace Mosa.Tool.Debugger.Views
{
	partial class StatusView : DebugDockContent
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
			this.tbInstruction = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.tbIP = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.tbMethod = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tbInstruction
			// 
			this.tbInstruction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbInstruction.Location = new System.Drawing.Point(118, 8);
			this.tbInstruction.Name = "tbInstruction";
			this.tbInstruction.ReadOnly = true;
			this.tbInstruction.Size = new System.Drawing.Size(1416, 36);
			this.tbInstruction.TabIndex = 27;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(3, 35);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(104, 29);
			this.label8.TabIndex = 26;
			this.label8.Text = "Method:";
			// 
			// tbIP
			// 
			this.tbIP.Location = new System.Drawing.Point(25, 8);
			this.tbIP.Name = "tbIP";
			this.tbIP.ReadOnly = true;
			this.tbIP.Size = new System.Drawing.Size(87, 36);
			this.tbIP.TabIndex = 30;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 11);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 29);
			this.label3.TabIndex = 33;
			this.label3.Text = "IP:";
			// 
			// tbMethod
			// 
			this.tbMethod.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tbMethod.Location = new System.Drawing.Point(55, 32);
			this.tbMethod.Name = "tbMethod";
			this.tbMethod.ReadOnly = true;
			this.tbMethod.Size = new System.Drawing.Size(1479, 36);
			this.tbMethod.TabIndex = 34;
			// 
			// StatusView
			// 
			this.ClientSize = new System.Drawing.Size(1544, 161);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.tbMethod);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.tbIP);
			this.Controls.Add(this.tbInstruction);
			this.Controls.Add(this.label8);
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(100, 50);
			this.Name = "StatusView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Status";
			this.Text = "Status";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.TextBox tbInstruction;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox tbIP;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbMethod;
	}
}