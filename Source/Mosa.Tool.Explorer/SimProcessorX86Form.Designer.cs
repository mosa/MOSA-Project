namespace Mosa.Tool.Explorer
{
	partial class SimProcessorX86Form
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
			this.lbInstructionHistory = new System.Windows.Forms.ListBox();
			this.lbCurrentFrame = new System.Windows.Forms.ListBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbGPRs = new System.Windows.Forms.ListBox();
			this.label3 = new System.Windows.Forms.Label();
			this.lbXMMRegisters = new System.Windows.Forms.ListBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.lbFlags = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lbInstructionHistory
			// 
			this.lbInstructionHistory.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbInstructionHistory.FormattingEnabled = true;
			this.lbInstructionHistory.ItemHeight = 11;
			this.lbInstructionHistory.Location = new System.Drawing.Point(3, 211);
			this.lbInstructionHistory.Name = "lbInstructionHistory";
			this.lbInstructionHistory.Size = new System.Drawing.Size(651, 158);
			this.lbInstructionHistory.TabIndex = 0;
			this.lbInstructionHistory.SelectedIndexChanged += new System.EventHandler(this.lbInstructionHistory_SelectedIndexChanged);
			// 
			// lbCurrentFrame
			// 
			this.lbCurrentFrame.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbCurrentFrame.FormattingEnabled = true;
			this.lbCurrentFrame.ItemHeight = 11;
			this.lbCurrentFrame.Location = new System.Drawing.Point(534, 25);
			this.lbCurrentFrame.Name = "lbCurrentFrame";
			this.lbCurrentFrame.Size = new System.Drawing.Size(120, 158);
			this.lbCurrentFrame.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(531, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Current Frame:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(0, 195);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(94, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Instruction History:";
			// 
			// lbGPRs
			// 
			this.lbGPRs.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbGPRs.FormattingEnabled = true;
			this.lbGPRs.ItemHeight = 11;
			this.lbGPRs.Location = new System.Drawing.Point(3, 25);
			this.lbGPRs.Name = "lbGPRs";
			this.lbGPRs.Size = new System.Drawing.Size(159, 158);
			this.lbGPRs.TabIndex = 4;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(0, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "General Purpose Registers:";
			// 
			// lbXMMRegisters
			// 
			this.lbXMMRegisters.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbXMMRegisters.FormattingEnabled = true;
			this.lbXMMRegisters.ItemHeight = 11;
			this.lbXMMRegisters.Location = new System.Drawing.Point(178, 25);
			this.lbXMMRegisters.Name = "lbXMMRegisters";
			this.lbXMMRegisters.Size = new System.Drawing.Size(159, 158);
			this.lbXMMRegisters.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(175, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "XMM Registers:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(247, 377);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(151, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "Execute Next Instruction";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// lbFlags
			// 
			this.lbFlags.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbFlags.FormattingEnabled = true;
			this.lbFlags.ItemHeight = 11;
			this.lbFlags.Location = new System.Drawing.Point(353, 25);
			this.lbFlags.Name = "lbFlags";
			this.lbFlags.Size = new System.Drawing.Size(159, 158);
			this.lbFlags.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(350, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Flag Register:";
			// 
			// SimProcessorX86Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(719, 460);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbFlags);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lbXMMRegisters);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lbGPRs);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lbCurrentFrame);
			this.Controls.Add(this.lbInstructionHistory);
			this.Name = "SimProcessorX86Form";
			this.Text = "X86 Processor Simulation";
			this.Load += new System.EventHandler(this.SimProcessorX86Form_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbInstructionHistory;
		private System.Windows.Forms.ListBox lbCurrentFrame;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbGPRs;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ListBox lbXMMRegisters;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox lbFlags;
		private System.Windows.Forms.Label label5;
	}
}