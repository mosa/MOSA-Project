namespace Mosa.Tool.Simulator
{
	partial class HistoryView : SimulatorDockContent
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
			this.lbHistory = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// lbHistory
			// 
			this.lbHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lbHistory.FormattingEnabled = true;
			this.lbHistory.Location = new System.Drawing.Point(0, 0);
			this.lbHistory.Name = "lbHistory";
			this.lbHistory.ScrollAlwaysVisible = true;
			this.lbHistory.Size = new System.Drawing.Size(216, 160);
			this.lbHistory.TabIndex = 0;
			this.lbHistory.DoubleClick += new System.EventHandler(this.lbHistory_DoubleClick);
			// 
			// HistoryView
			// 
			this.ClientSize = new System.Drawing.Size(219, 161);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.lbHistory);
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "HistoryView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "History";
			this.Text = "History";
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ListBox lbHistory;




	}
}