namespace Mosa.Tool.Simulator
{
	partial class ControlView : SimulatorDockContent
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
			this.button1 = new System.Windows.Forms.Button();
			this.tbSteps = new System.Windows.Forms.TextBox();
			this.cbRecord = new System.Windows.Forms.CheckBox();
			this.button2 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(95, 5);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Step";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tbSteps
			// 
			this.tbSteps.Location = new System.Drawing.Point(257, 7);
			this.tbSteps.Name = "tbSteps";
			this.tbSteps.Size = new System.Drawing.Size(75, 20);
			this.tbSteps.TabIndex = 20;
			this.tbSteps.Text = "1000";
			// 
			// cbRecord
			// 
			this.cbRecord.AutoSize = true;
			this.cbRecord.Location = new System.Drawing.Point(338, 9);
			this.cbRecord.Name = "cbRecord";
			this.cbRecord.Size = new System.Drawing.Size(61, 17);
			this.cbRecord.TabIndex = 21;
			this.cbRecord.Text = "Record";
			this.cbRecord.UseVisualStyleBackColor = true;
			this.cbRecord.CheckedChanged += new System.EventHandler(this.cbRecord_CheckedChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(176, 5);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 22;
			this.button2.Text = "Step N";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(14, 5);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 23;
			this.button3.Text = "Restart";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// ControlView
			// 
			this.ClientSize = new System.Drawing.Size(415, 40);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.button3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.cbRecord);
			this.Controls.Add(this.tbSteps);
			this.Controls.Add(this.button1);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(0, 75);
			this.Name = "ControlView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Control Panel";
			this.Text = "Control Panel";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox tbSteps;
		private System.Windows.Forms.CheckBox cbRecord;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;



	}
}