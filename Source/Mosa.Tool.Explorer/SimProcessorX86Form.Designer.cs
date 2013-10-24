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
			this.lbStack = new System.Windows.Forms.ListBox();
			this.label6 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.nvSteps = new System.Windows.Forms.NumericUpDown();
			this.cbRecord = new System.Windows.Forms.CheckBox();
			this.tbLabel = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.nvSteps)).BeginInit();
			this.SuspendLayout();
			// 
			// lbInstructionHistory
			// 
			this.lbInstructionHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbInstructionHistory.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbInstructionHistory.FormattingEnabled = true;
			this.lbInstructionHistory.ItemHeight = 11;
			this.lbInstructionHistory.Location = new System.Drawing.Point(3, 180);
			this.lbInstructionHistory.Name = "lbInstructionHistory";
			this.lbInstructionHistory.Size = new System.Drawing.Size(744, 169);
			this.lbInstructionHistory.TabIndex = 0;
			this.lbInstructionHistory.SelectedIndexChanged += new System.EventHandler(this.lbInstructionHistory_SelectedIndexChanged);
			// 
			// lbCurrentFrame
			// 
			this.lbCurrentFrame.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbCurrentFrame.FormattingEnabled = true;
			this.lbCurrentFrame.ItemHeight = 11;
			this.lbCurrentFrame.Location = new System.Drawing.Point(490, 25);
			this.lbCurrentFrame.Name = "lbCurrentFrame";
			this.lbCurrentFrame.Size = new System.Drawing.Size(125, 136);
			this.lbCurrentFrame.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(487, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Current Frame:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(0, 164);
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
			this.lbGPRs.Size = new System.Drawing.Size(155, 136);
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
			this.lbXMMRegisters.Location = new System.Drawing.Point(329, 25);
			this.lbXMMRegisters.Name = "lbXMMRegisters";
			this.lbXMMRegisters.Size = new System.Drawing.Size(155, 136);
			this.lbXMMRegisters.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(326, 9);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(82, 13);
			this.label4.TabIndex = 7;
			this.label4.Text = "XMM Registers:";
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(596, 355);
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
			this.lbFlags.Location = new System.Drawing.Point(164, 25);
			this.lbFlags.Name = "lbFlags";
			this.lbFlags.Size = new System.Drawing.Size(155, 136);
			this.lbFlags.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(161, 9);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Flag Register:";
			// 
			// lbStack
			// 
			this.lbStack.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbStack.FormattingEnabled = true;
			this.lbStack.ItemHeight = 11;
			this.lbStack.Location = new System.Drawing.Point(621, 25);
			this.lbStack.Name = "lbStack";
			this.lbStack.Size = new System.Drawing.Size(125, 136);
			this.lbStack.TabIndex = 11;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(618, 9);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(75, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Current Stack:";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.Location = new System.Drawing.Point(3, 355);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(171, 23);
			this.button2.TabIndex = 13;
			this.button2.Text = "Execute Next X Instructions";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// nvSteps
			// 
			this.nvSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.nvSteps.Location = new System.Drawing.Point(180, 358);
			this.nvSteps.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nvSteps.Name = "nvSteps";
			this.nvSteps.Size = new System.Drawing.Size(65, 20);
			this.nvSteps.TabIndex = 14;
			this.nvSteps.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
			// 
			// cbRecord
			// 
			this.cbRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cbRecord.AutoSize = true;
			this.cbRecord.Location = new System.Drawing.Point(529, 359);
			this.cbRecord.Name = "cbRecord";
			this.cbRecord.Size = new System.Drawing.Size(61, 17);
			this.cbRecord.TabIndex = 15;
			this.cbRecord.Text = "Record";
			this.cbRecord.UseVisualStyleBackColor = true;
			// 
			// tbLabel
			// 
			this.tbLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.tbLabel.Location = new System.Drawing.Point(367, 356);
			this.tbLabel.Name = "tbLabel";
			this.tbLabel.Size = new System.Drawing.Size(156, 20);
			this.tbLabel.TabIndex = 17;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(268, 355);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(93, 23);
			this.button3.TabIndex = 18;
			this.button3.Text = "Execute Until:";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// SimProcessorX86Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(755, 386);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.tbLabel);
			this.Controls.Add(this.cbRecord);
			this.Controls.Add(this.nvSteps);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.lbStack);
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
			((System.ComponentModel.ISupportInitialize)(this.nvSteps)).EndInit();
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
		private System.Windows.Forms.ListBox lbStack;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.NumericUpDown nvSteps;
		private System.Windows.Forms.CheckBox cbRecord;
		private System.Windows.Forms.TextBox tbLabel;
		private System.Windows.Forms.Button button3;
	}
}