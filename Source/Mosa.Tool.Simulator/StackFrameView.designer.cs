namespace Mosa.Tool.Simulator
{
	partial class StackFrameView : SimulatorDockContent
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
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(216, 160);
			this.listBox1.TabIndex = 0;
			// 
			// StackFrameView
			// 
			this.ClientSize = new System.Drawing.Size(219, 161);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.listBox1);
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(235, 200);
			this.Name = "StackFrameView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Stack Frame";
			this.Text = "Stack Frame";
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ListBox listBox1;




	}
}