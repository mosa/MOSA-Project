namespace Mosa.Tool.Debugger
{
	partial class ConnectionProperties : DebuggerDockContent
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
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.lbServerName = new System.Windows.Forms.Label();
			this.tbPort = new System.Windows.Forms.TextBox();
			this.tbServerName = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.btnDisconnect = new System.Windows.Forms.Button();
			this.btnConnect = new System.Windows.Forms.Button();
			this.tbNamedPipe = new System.Windows.Forms.TextBox();
			this.lbPipeName = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Items.AddRange(new object[] {
            "TCP Client Socket",
            "TCP Server Socket",
            "Named Pipe"});
			this.comboBox1.Location = new System.Drawing.Point(12, 36);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(191, 21);
			this.comboBox1.TabIndex = 19;
			this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(160, 70);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(29, 13);
			this.label7.TabIndex = 17;
			this.label7.Text = "Port:";
			// 
			// lbServerName
			// 
			this.lbServerName.AutoSize = true;
			this.lbServerName.Location = new System.Drawing.Point(12, 70);
			this.lbServerName.Name = "lbServerName";
			this.lbServerName.Size = new System.Drawing.Size(41, 13);
			this.lbServerName.TabIndex = 16;
			this.lbServerName.Text = "Server:";
			// 
			// tbPort
			// 
			this.tbPort.Location = new System.Drawing.Point(164, 86);
			this.tbPort.Name = "tbPort";
			this.tbPort.Size = new System.Drawing.Size(39, 20);
			this.tbPort.TabIndex = 15;
			this.tbPort.Text = "9999";
			// 
			// tbServerName
			// 
			this.tbServerName.Location = new System.Drawing.Point(15, 86);
			this.tbServerName.Name = "tbServerName";
			this.tbServerName.Size = new System.Drawing.Size(146, 20);
			this.tbServerName.TabIndex = 14;
			this.tbServerName.Text = "localhost";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 20);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(91, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "Connection Type:";
			// 
			// btnDisconnect
			// 
			this.btnDisconnect.Location = new System.Drawing.Point(128, 122);
			this.btnDisconnect.Name = "btnDisconnect";
			this.btnDisconnect.Size = new System.Drawing.Size(75, 24);
			this.btnDisconnect.TabIndex = 9;
			this.btnDisconnect.Text = "Disconnect";
			this.btnDisconnect.UseVisualStyleBackColor = true;
			this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
			// 
			// btnConnect
			// 
			this.btnConnect.Location = new System.Drawing.Point(12, 122);
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(75, 24);
			this.btnConnect.TabIndex = 6;
			this.btnConnect.Text = "Connect";
			this.btnConnect.UseVisualStyleBackColor = true;
			this.btnConnect.Click += new System.EventHandler(this.button1_Click);
			// 
			// tbNamedPipe
			// 
			this.tbNamedPipe.Location = new System.Drawing.Point(15, 86);
			this.tbNamedPipe.Name = "tbNamedPipe";
			this.tbNamedPipe.Size = new System.Drawing.Size(146, 20);
			this.tbNamedPipe.TabIndex = 20;
			this.tbNamedPipe.Text = "MOSA";
			// 
			// lbPipeName
			// 
			this.lbPipeName.AutoSize = true;
			this.lbPipeName.Location = new System.Drawing.Point(12, 70);
			this.lbPipeName.Name = "lbPipeName";
			this.lbPipeName.Size = new System.Drawing.Size(62, 13);
			this.lbPipeName.TabIndex = 21;
			this.lbPipeName.Text = "Pipe Name:";
			// 
			// ConnectionProperties
			// 
			this.ClientSize = new System.Drawing.Size(219, 162);
			this.Controls.Add(this.tbNamedPipe);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lbServerName);
			this.Controls.Add(this.tbPort);
			this.Controls.Add(this.tbServerName);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.btnDisconnect);
			this.Controls.Add(this.btnConnect);
			this.Controls.Add(this.lbPipeName);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(235, 200);
			this.Name = "ConnectionProperties";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Connection Properties";
			this.Text = "Connection Properties";
			this.Load += new System.EventHandler(this.ConnectionProperties_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button btnConnect;
		private System.Windows.Forms.Button btnDisconnect;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lbServerName;
		private System.Windows.Forms.TextBox tbPort;
		private System.Windows.Forms.TextBox tbServerName;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox tbNamedPipe;
		private System.Windows.Forms.Label lbPipeName;
    }
}