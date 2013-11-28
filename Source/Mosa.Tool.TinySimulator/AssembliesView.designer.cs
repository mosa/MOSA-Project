namespace Mosa.Tool.TinySimulator
{
	partial class AssembliesView : SimulatorDockContent
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
			this.treeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Margin = new System.Windows.Forms.Padding(0);
			this.treeView.Name = "treeView";
			this.treeView.Size = new System.Drawing.Size(219, 160);
			this.treeView.TabIndex = 4;
			// 
			// AssembliesView
			// 
			this.ClientSize = new System.Drawing.Size(219, 162);
			this.CloseButton = false;
			this.CloseButtonVisible = false;
			this.Controls.Add(this.treeView);
			this.HideOnClose = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(100, 100);
			this.Name = "AssembliesView";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
			this.TabText = "Assemblies";
			this.Text = "Assemblies";
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.TreeView treeView;

	}
}