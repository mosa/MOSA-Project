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
			this.label2 = new System.Windows.Forms.Label();
			this.lbGPRs = new System.Windows.Forms.ListBox();
			this.button1 = new System.Windows.Forms.Button();
			this.lbFlags = new System.Windows.Forms.ListBox();
			this.label5 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.cbRecord = new System.Windows.Forms.CheckBox();
			this.tbLabel = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.tbSteps = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tpGPRegisters = new System.Windows.Forms.TabPage();
			this.tbXMMRegisters = new System.Windows.Forms.TabPage();
			this.lbXMMRegisters = new System.Windows.Forms.ListBox();
			this.tabControl2 = new System.Windows.Forms.TabControl();
			this.tbFrame = new System.Windows.Forms.TabPage();
			this.tbStack = new System.Windows.Forms.TabPage();
			this.lbCurrentFrame = new System.Windows.Forms.ListBox();
			this.lbStack = new System.Windows.Forms.ListBox();
			this.tabControl1.SuspendLayout();
			this.tpGPRegisters.SuspendLayout();
			this.tbXMMRegisters.SuspendLayout();
			this.tabControl2.SuspendLayout();
			this.tbFrame.SuspendLayout();
			this.tbStack.SuspendLayout();
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
			this.lbInstructionHistory.Location = new System.Drawing.Point(3, 257);
			this.lbInstructionHistory.Name = "lbInstructionHistory";
			this.lbInstructionHistory.Size = new System.Drawing.Size(743, 136);
			this.lbInstructionHistory.TabIndex = 0;
			this.lbInstructionHistory.SelectedIndexChanged += new System.EventHandler(this.lbInstructionHistory_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(0, 241);
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
			this.lbGPRs.Location = new System.Drawing.Point(0, 0);
			this.lbGPRs.Name = "lbGPRs";
			this.lbGPRs.Size = new System.Drawing.Size(155, 136);
			this.lbGPRs.TabIndex = 4;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(595, 200);
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
			this.lbFlags.Location = new System.Drawing.Point(511, 58);
			this.lbFlags.Name = "lbFlags";
			this.lbFlags.Size = new System.Drawing.Size(155, 136);
			this.lbFlags.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(508, 42);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(72, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Flag Register:";
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button2.Location = new System.Drawing.Point(3, 407);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(133, 23);
			this.button2.TabIndex = 13;
			this.button2.Text = "Execute X Instructions:";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// cbRecord
			// 
			this.cbRecord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cbRecord.AutoSize = true;
			this.cbRecord.Location = new System.Drawing.Point(490, 204);
			this.cbRecord.Name = "cbRecord";
			this.cbRecord.Size = new System.Drawing.Size(61, 17);
			this.cbRecord.TabIndex = 15;
			this.cbRecord.Text = "Record";
			this.cbRecord.UseVisualStyleBackColor = true;
			// 
			// tbLabel
			// 
			this.tbLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tbLabel.Location = new System.Drawing.Point(367, 408);
			this.tbLabel.Name = "tbLabel";
			this.tbLabel.Size = new System.Drawing.Size(156, 20);
			this.tbLabel.TabIndex = 17;
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button3.Location = new System.Drawing.Point(268, 407);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(93, 23);
			this.button3.TabIndex = 18;
			this.button3.Text = "Execute Until:";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// tbSteps
			// 
			this.tbSteps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tbSteps.Location = new System.Drawing.Point(142, 408);
			this.tbSteps.Name = "tbSteps";
			this.tbSteps.Size = new System.Drawing.Size(102, 20);
			this.tbSteps.TabIndex = 19;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(0, 12);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(31, 13);
			this.label7.TabIndex = 20;
			this.label7.Text = "Tick:";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(34, 9);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(122, 20);
			this.textBox1.TabIndex = 21;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(162, 12);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(82, 13);
			this.label8.TabIndex = 22;
			this.label8.Text = "Last Instruction:";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(250, 9);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(234, 20);
			this.textBox2.TabIndex = 23;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tpGPRegisters);
			this.tabControl1.Controls.Add(this.tbXMMRegisters);
			this.tabControl1.Location = new System.Drawing.Point(3, 35);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(172, 171);
			this.tabControl1.TabIndex = 24;
			// 
			// tpGPRegisters
			// 
			this.tpGPRegisters.Controls.Add(this.lbGPRs);
			this.tpGPRegisters.Location = new System.Drawing.Point(4, 22);
			this.tpGPRegisters.Name = "tpGPRegisters";
			this.tpGPRegisters.Padding = new System.Windows.Forms.Padding(3);
			this.tpGPRegisters.Size = new System.Drawing.Size(164, 145);
			this.tpGPRegisters.TabIndex = 0;
			this.tpGPRegisters.Text = "GP Registers";
			this.tpGPRegisters.UseVisualStyleBackColor = true;
			// 
			// tbXMMRegisters
			// 
			this.tbXMMRegisters.Controls.Add(this.lbXMMRegisters);
			this.tbXMMRegisters.Location = new System.Drawing.Point(4, 22);
			this.tbXMMRegisters.Name = "tbXMMRegisters";
			this.tbXMMRegisters.Padding = new System.Windows.Forms.Padding(3);
			this.tbXMMRegisters.Size = new System.Drawing.Size(164, 145);
			this.tbXMMRegisters.TabIndex = 1;
			this.tbXMMRegisters.Text = "XMM Registers";
			this.tbXMMRegisters.UseVisualStyleBackColor = true;
			// 
			// lbXMMRegisters
			// 
			this.lbXMMRegisters.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbXMMRegisters.FormattingEnabled = true;
			this.lbXMMRegisters.ItemHeight = 11;
			this.lbXMMRegisters.Location = new System.Drawing.Point(-4, 0);
			this.lbXMMRegisters.Name = "lbXMMRegisters";
			this.lbXMMRegisters.Size = new System.Drawing.Size(155, 136);
			this.lbXMMRegisters.TabIndex = 7;
			// 
			// tabControl2
			// 
			this.tabControl2.Controls.Add(this.tbFrame);
			this.tabControl2.Controls.Add(this.tbStack);
			this.tabControl2.Location = new System.Drawing.Point(192, 35);
			this.tabControl2.Name = "tabControl2";
			this.tabControl2.SelectedIndex = 0;
			this.tabControl2.Size = new System.Drawing.Size(172, 171);
			this.tabControl2.TabIndex = 25;
			// 
			// tbFrame
			// 
			this.tbFrame.Controls.Add(this.lbCurrentFrame);
			this.tbFrame.Location = new System.Drawing.Point(4, 22);
			this.tbFrame.Name = "tbFrame";
			this.tbFrame.Padding = new System.Windows.Forms.Padding(3);
			this.tbFrame.Size = new System.Drawing.Size(164, 145);
			this.tbFrame.TabIndex = 0;
			this.tbFrame.Text = "Frame";
			this.tbFrame.UseVisualStyleBackColor = true;
			// 
			// tbStack
			// 
			this.tbStack.Controls.Add(this.lbStack);
			this.tbStack.Location = new System.Drawing.Point(4, 22);
			this.tbStack.Name = "tbStack";
			this.tbStack.Padding = new System.Windows.Forms.Padding(3);
			this.tbStack.Size = new System.Drawing.Size(164, 145);
			this.tbStack.TabIndex = 1;
			this.tbStack.Text = "Stack";
			this.tbStack.UseVisualStyleBackColor = true;
			// 
			// lbCurrentFrame
			// 
			this.lbCurrentFrame.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbCurrentFrame.FormattingEnabled = true;
			this.lbCurrentFrame.ItemHeight = 11;
			this.lbCurrentFrame.Location = new System.Drawing.Point(3, 3);
			this.lbCurrentFrame.Name = "lbCurrentFrame";
			this.lbCurrentFrame.Size = new System.Drawing.Size(125, 136);
			this.lbCurrentFrame.TabIndex = 2;
			// 
			// lbStack
			// 
			this.lbStack.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbStack.FormattingEnabled = true;
			this.lbStack.ItemHeight = 11;
			this.lbStack.Location = new System.Drawing.Point(3, 3);
			this.lbStack.Name = "lbStack";
			this.lbStack.Size = new System.Drawing.Size(125, 136);
			this.lbStack.TabIndex = 12;
			// 
			// SimProcessorX86Form
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 436);
			this.Controls.Add(this.tabControl2);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.tbSteps);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.tbLabel);
			this.Controls.Add(this.cbRecord);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbFlags);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.lbInstructionHistory);
			this.Name = "SimProcessorX86Form";
			this.Text = "X86 Processor Simulation";
			this.Load += new System.EventHandler(this.SimProcessorX86Form_Load);
			this.tabControl1.ResumeLayout(false);
			this.tpGPRegisters.ResumeLayout(false);
			this.tbXMMRegisters.ResumeLayout(false);
			this.tabControl2.ResumeLayout(false);
			this.tbFrame.ResumeLayout(false);
			this.tbStack.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox lbInstructionHistory;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ListBox lbGPRs;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ListBox lbFlags;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.CheckBox cbRecord;
		private System.Windows.Forms.TextBox tbLabel;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox tbSteps;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tpGPRegisters;
		private System.Windows.Forms.TabPage tbXMMRegisters;
		private System.Windows.Forms.ListBox lbXMMRegisters;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tbFrame;
		private System.Windows.Forms.ListBox lbCurrentFrame;
		private System.Windows.Forms.TabPage tbStack;
		private System.Windows.Forms.ListBox lbStack;
	}
}