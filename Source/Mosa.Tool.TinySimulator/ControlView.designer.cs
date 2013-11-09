namespace Mosa.Tool.TinySimulator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlView));
			this.cbBreakAfterReturn = new System.Windows.Forms.CheckBox();
			this.cbBreakAfterCall = new System.Windows.Forms.CheckBox();
			this.cbBreakAfterJump = new System.Windows.Forms.CheckBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnRestart = new System.Windows.Forms.Button();
			this.btnStepN = new System.Windows.Forms.Button();
			this.cbRecord = new System.Windows.Forms.CheckBox();
			this.tbSteps = new System.Windows.Forms.TextBox();
			this.btnStep = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.cbBreakAfterBranch = new System.Windows.Forms.CheckBox();
			this.btnStepOver = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// cbBreakAfterReturn
			// 
			this.cbBreakAfterReturn.AutoSize = true;
			this.cbBreakAfterReturn.Location = new System.Drawing.Point(179, 35);
			this.cbBreakAfterReturn.Name = "cbBreakAfterReturn";
			this.cbBreakAfterReturn.Size = new System.Drawing.Size(58, 17);
			this.cbBreakAfterReturn.TabIndex = 28;
			this.cbBreakAfterReturn.Text = "Return";
			this.cbBreakAfterReturn.UseVisualStyleBackColor = true;
			this.cbBreakAfterReturn.CheckedChanged += new System.EventHandler(this.cbBreakOnReturn_CheckedChanged);
			// 
			// cbBreakAfterCall
			// 
			this.cbBreakAfterCall.AutoSize = true;
			this.cbBreakAfterCall.Location = new System.Drawing.Point(130, 34);
			this.cbBreakAfterCall.Name = "cbBreakAfterCall";
			this.cbBreakAfterCall.Size = new System.Drawing.Size(43, 17);
			this.cbBreakAfterCall.TabIndex = 27;
			this.cbBreakAfterCall.Text = "Call";
			this.cbBreakAfterCall.UseVisualStyleBackColor = true;
			this.cbBreakAfterCall.CheckedChanged += new System.EventHandler(this.cbBreakOnCall_CheckedChanged);
			// 
			// cbBreakAfterJump
			// 
			this.cbBreakAfterJump.AutoSize = true;
			this.cbBreakAfterJump.Location = new System.Drawing.Point(73, 34);
			this.cbBreakAfterJump.Name = "cbBreakAfterJump";
			this.cbBreakAfterJump.Size = new System.Drawing.Size(51, 17);
			this.cbBreakAfterJump.TabIndex = 26;
			this.cbBreakAfterJump.Text = "Jump";
			this.cbBreakAfterJump.UseVisualStyleBackColor = true;
			this.cbBreakAfterJump.CheckedChanged += new System.EventHandler(this.cbBreakOnJump_CheckedChanged);
			// 
			// btnStop
			// 
			this.btnStop.Image = ((System.Drawing.Image)(resources.GetObject("btnStop.Image")));
			this.btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnStop.Location = new System.Drawing.Point(167, 5);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(80, 23);
			this.btnStop.TabIndex = 25;
			this.btnStop.Text = "Stop";
			this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStart
			// 
			this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
			this.btnStart.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnStart.Location = new System.Drawing.Point(86, 5);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(80, 23);
			this.btnStart.TabIndex = 24;
			this.btnStart.Text = "Start";
			this.btnStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnRestart
			// 
			this.btnRestart.Image = ((System.Drawing.Image)(resources.GetObject("btnRestart.Image")));
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
			this.btnStepN.Image = ((System.Drawing.Image)(resources.GetObject("btnStepN.Image")));
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
			// cbRecord
			// 
			this.cbRecord.AutoSize = true;
			this.cbRecord.Location = new System.Drawing.Point(343, 34);
			this.cbRecord.Name = "cbRecord";
			this.cbRecord.Size = new System.Drawing.Size(61, 17);
			this.cbRecord.TabIndex = 21;
			this.cbRecord.Text = "Record";
			this.cbRecord.UseVisualStyleBackColor = true;
			this.cbRecord.CheckedChanged += new System.EventHandler(this.cbRecord_CheckedChanged);
			// 
			// tbSteps
			// 
			this.tbSteps.Location = new System.Drawing.Point(410, 32);
			this.tbSteps.Name = "tbSteps";
			this.tbSteps.Size = new System.Drawing.Size(80, 20);
			this.tbSteps.TabIndex = 20;
			this.tbSteps.Text = "1000";
			// 
			// btnStep
			// 
			this.btnStep.Image = ((System.Drawing.Image)(resources.GetObject("btnStep.Image")));
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
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(4, 35);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(63, 13);
			this.label1.TabIndex = 29;
			this.label1.Text = "Break After:";
			// 
			// cbBreakAfterBranch
			// 
			this.cbBreakAfterBranch.AutoSize = true;
			this.cbBreakAfterBranch.Location = new System.Drawing.Point(243, 35);
			this.cbBreakAfterBranch.Name = "cbBreakAfterBranch";
			this.cbBreakAfterBranch.Size = new System.Drawing.Size(60, 17);
			this.cbBreakAfterBranch.TabIndex = 30;
			this.cbBreakAfterBranch.Text = "Branch";
			this.cbBreakAfterBranch.UseVisualStyleBackColor = true;
			this.cbBreakAfterBranch.CheckedChanged += new System.EventHandler(this.cbBreakAfterBranch_CheckedChanged);
			// 
			// btnStepOver
			// 
			this.btnStepOver.Image = ((System.Drawing.Image)(resources.GetObject("btnStepOver.Image")));
			this.btnStepOver.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnStepOver.Location = new System.Drawing.Point(248, 5);
			this.btnStepOver.Name = "btnStepOver";
			this.btnStepOver.Size = new System.Drawing.Size(80, 23);
			this.btnStepOver.TabIndex = 31;
			this.btnStepOver.Text = "Step Over";
			this.btnStepOver.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnStepOver.UseVisualStyleBackColor = true;
			this.btnStepOver.Click += new System.EventHandler(this.btnStepOver_Click);
			// 
			// ControlView
			// 
			this.ClientSize = new System.Drawing.Size(502, 58);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.btnStepOver);
			this.Controls.Add(this.cbBreakAfterBranch);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.cbBreakAfterReturn);
			this.Controls.Add(this.cbBreakAfterCall);
			this.Controls.Add(this.cbBreakAfterJump);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnRestart);
			this.Controls.Add(this.btnStepN);
			this.Controls.Add(this.cbRecord);
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
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Control Panel";
			this.Text = "Control Panel";
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		#endregion

		private System.Windows.Forms.Button btnStep;
		private System.Windows.Forms.TextBox tbSteps;
		private System.Windows.Forms.CheckBox cbRecord;
		private System.Windows.Forms.Button btnStepN;
		private System.Windows.Forms.Button btnRestart;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.CheckBox cbBreakAfterJump;
		private System.Windows.Forms.CheckBox cbBreakAfterCall;
		private System.Windows.Forms.CheckBox cbBreakAfterReturn;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox cbBreakAfterBranch;
		private System.Windows.Forms.Button btnStepOver;



	}
}