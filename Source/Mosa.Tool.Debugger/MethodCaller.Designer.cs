// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Tool.Debugger
{
	partial class MethodCaller
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
			this.tbMethodAddress = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tbParameter1 = new System.Windows.Forms.TextBox();
			this.tbParameter2 = new System.Windows.Forms.TextBox();
			this.tbParameter3 = new System.Windows.Forms.TextBox();
			this.cbParameter1 = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.cbParameter2 = new System.Windows.Forms.ComboBox();
			this.cbParameter3 = new System.Windows.Forms.ComboBox();
			this.cbResultType = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// tbMethodAddress
			// 
			this.tbMethodAddress.Location = new System.Drawing.Point(15, 26);
			this.tbMethodAddress.Name = "tbMethodAddress";
			this.tbMethodAddress.Size = new System.Drawing.Size(100, 20);
			this.tbMethodAddress.TabIndex = 0;
			this.tbMethodAddress.Text = "0x00000000";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Method Address:";
			// 
			// tbParameter1
			// 
			this.tbParameter1.Location = new System.Drawing.Point(92, 66);
			this.tbParameter1.Name = "tbParameter1";
			this.tbParameter1.Size = new System.Drawing.Size(82, 20);
			this.tbParameter1.TabIndex = 2;
			this.tbParameter1.Text = "0x00000000";
			// 
			// tbParameter2
			// 
			this.tbParameter2.Location = new System.Drawing.Point(92, 92);
			this.tbParameter2.Name = "tbParameter2";
			this.tbParameter2.Size = new System.Drawing.Size(82, 20);
			this.tbParameter2.TabIndex = 5;
			this.tbParameter2.Text = "0x00000000";
			// 
			// tbParameter3
			// 
			this.tbParameter3.Location = new System.Drawing.Point(92, 120);
			this.tbParameter3.Name = "tbParameter3";
			this.tbParameter3.Size = new System.Drawing.Size(82, 20);
			this.tbParameter3.TabIndex = 7;
			this.tbParameter3.Text = "0x00000000";
			// 
			// cbParameter1
			// 
			this.cbParameter1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbParameter1.FormattingEnabled = true;
			this.cbParameter1.Items.AddRange(new object[] {
            "None",
            "Integer",
            "Long",
            "Double"});
			this.cbParameter1.Location = new System.Drawing.Point(12, 65);
			this.cbParameter1.Name = "cbParameter1";
			this.cbParameter1.Size = new System.Drawing.Size(74, 21);
			this.cbParameter1.TabIndex = 9;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Parameters:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 198);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(159, 23);
			this.button1.TabIndex = 16;
			this.button1.Text = "Execute Unit Test";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// cbParameter2
			// 
			this.cbParameter2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbParameter2.FormattingEnabled = true;
			this.cbParameter2.Items.AddRange(new object[] {
            "None",
            "Integer",
            "Long",
            "Double"});
			this.cbParameter2.Location = new System.Drawing.Point(12, 92);
			this.cbParameter2.Name = "cbParameter2";
			this.cbParameter2.Size = new System.Drawing.Size(74, 21);
			this.cbParameter2.TabIndex = 17;
			// 
			// cbParameter3
			// 
			this.cbParameter3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbParameter3.FormattingEnabled = true;
			this.cbParameter3.Items.AddRange(new object[] {
            "None",
            "Integer",
            "Long",
            "Double"});
			this.cbParameter3.Location = new System.Drawing.Point(12, 119);
			this.cbParameter3.Name = "cbParameter3";
			this.cbParameter3.Size = new System.Drawing.Size(74, 21);
			this.cbParameter3.TabIndex = 18;
			// 
			// cbResultType
			// 
			this.cbResultType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbResultType.FormattingEnabled = true;
			this.cbResultType.Items.AddRange(new object[] {
            "None",
            "Integer",
            "Long",
            "Double"});
			this.cbResultType.Location = new System.Drawing.Point(12, 171);
			this.cbResultType.Name = "cbResultType";
			this.cbResultType.Size = new System.Drawing.Size(159, 21);
			this.cbResultType.TabIndex = 19;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 155);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(67, 13);
			this.label3.TabIndex = 20;
			this.label3.Text = "Result Type:";
			// 
			// MethodCaller
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(187, 230);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.cbResultType);
			this.Controls.Add(this.cbParameter3);
			this.Controls.Add(this.cbParameter2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbParameter1);
			this.Controls.Add(this.tbParameter3);
			this.Controls.Add(this.tbParameter2);
			this.Controls.Add(this.tbParameter1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbMethodAddress);
			this.Name = "MethodCaller";
			this.Text = "Method Caller";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox tbMethodAddress;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbParameter1;
		private System.Windows.Forms.TextBox tbParameter2;
		private System.Windows.Forms.TextBox tbParameter3;
		private System.Windows.Forms.ComboBox cbParameter1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.ComboBox cbParameter2;
		private System.Windows.Forms.ComboBox cbParameter3;
		private System.Windows.Forms.ComboBox cbResultType;
		private System.Windows.Forms.Label label3;
	}
}