namespace Mosa.Tool.Debugger.Views
{
	partial class ControlView : DebugDockContent
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
			this.tbSteps = new System.Windows.Forms.TextBox();
			this.btnStepOut = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnContinue = new System.Windows.Forms.Button();
			this.btnRestart = new System.Windows.Forms.Button();
			this.btnStepN = new System.Windows.Forms.Button();
			this.btnStep = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// tbSteps
			// 
			this.tbSteps.Location = new System.Drawing.Point(496, 7);
			this.tbSteps.Name = "tbSteps";
			this.tbSteps.Size = new System.Drawing.Size(55, 20);
			this.tbSteps.TabIndex = 20;
			this.tbSteps.Text = "1000";
			// 
			// btnStepOut
			// 
			this.btnStepOut.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_eject;
			this.btnStepOut.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnStepOut.Location = new System.Drawing.Point(248, 5);
			this.btnStepOut.Name = "btnStepOut";
			this.btnStepOut.Size = new System.Drawing.Size(80, 23);
			this.btnStepOut.TabIndex = 26;
			this.btnStepOut.Text = "Step Out";
			this.btnStepOut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStepOut.UseVisualStyleBackColor = true;
			this.btnStepOut.Click += new System.EventHandler(this.btnStepOut_Click);
			// 
			// btnPause
			// 
			this.btnPause.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_pause;
			this.btnPause.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnPause.Location = new System.Drawing.Point(167, 5);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(80, 23);
			this.btnPause.TabIndex = 25;
			this.btnPause.Text = "Pause";
			this.btnPause.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnContinue
			// 
			this.btnContinue.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_play;
			this.btnContinue.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnContinue.Location = new System.Drawing.Point(86, 5);
			this.btnContinue.Name = "btnContinue";
			this.btnContinue.Size = new System.Drawing.Size(80, 23);
			this.btnContinue.TabIndex = 24;
			this.btnContinue.Text = "Continue";
			this.btnContinue.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnContinue.UseVisualStyleBackColor = true;
			this.btnContinue.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnRestart
			// 
			this.btnRestart.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_power;
			this.btnRestart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnRestart.Location = new System.Drawing.Point(5, 5);
			this.btnRestart.Name = "btnRestart";
			this.btnRestart.Size = new System.Drawing.Size(80, 23);
			this.btnRestart.TabIndex = 23;
			this.btnRestart.Text = "Restart";
			this.btnRestart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnRestart.UseVisualStyleBackColor = true;
			this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
			// 
			// btnStepN
			// 
			this.btnStepN.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_fastforward;
			this.btnStepN.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnStepN.Location = new System.Drawing.Point(410, 5);
			this.btnStepN.Name = "btnStepN";
			this.btnStepN.Size = new System.Drawing.Size(80, 23);
			this.btnStepN.TabIndex = 22;
			this.btnStepN.Text = "Step N";
			this.btnStepN.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStepN.UseVisualStyleBackColor = true;
			this.btnStepN.Click += new System.EventHandler(this.btnStepN_Click);
			// 
			// btnStep
			// 
			this.btnStep.Image = global::Mosa.Tool.Debugger.Properties.Resources.control_end;
			this.btnStep.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnStep.Location = new System.Drawing.Point(329, 5);
			this.btnStep.Name = "btnStep";
			this.btnStep.Size = new System.Drawing.Size(80, 23);
			this.btnStep.TabIndex = 0;
			this.btnStep.Text = "Step";
			this.btnStep.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStep.UseVisualStyleBackColor = true;
			this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
			// 
			// ControlView
			// 
			this.ClientSize = new System.Drawing.Size(780, 114);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.btnStepOut);
			this.Controls.Add(this.btnPause);
			this.Controls.Add(this.btnContinue);
			this.Controls.Add(this.btnRestart);
			this.Controls.Add(this.btnStepN);
			this.Controls.Add(this.tbSteps);
			this.Controls.Add(this.btnStep);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(0, 75);
			this.Name = "ControlView";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Control Panel";
			this.Text = "Control Panel";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button btnStep;
		private System.Windows.Forms.TextBox tbSteps;
		private System.Windows.Forms.Button btnStepN;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnStepOut;
	}
}