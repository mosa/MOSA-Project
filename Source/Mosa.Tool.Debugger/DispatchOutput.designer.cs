namespace Mosa.Tool.Debugger
{
    partial class DispatchOutput
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
			this.components = new System.ComponentModel.Container();
			this.imageList = new System.Windows.Forms.ImageList(this.components);
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// imageList
			// 
			this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imageList.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// listBox1
			// 
			this.listBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(0, 2);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(263, 348);
			this.listBox1.TabIndex = 3;
			// 
			// DispatchOutput
			// 
			this.ClientSize = new System.Drawing.Size(263, 352);
			this.Controls.Add(this.listBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.HideOnClose = true;
			this.Location = new System.Drawing.Point(0, 20);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(278, 0);
			this.Name = "DispatchOutput";
			this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide;
			this.ShowInTaskbar = false;
			this.TabText = "Connection Output";
			this.Text = "Connection Output";
			this.ResumeLayout(false);

		}
		#endregion

		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.ListBox listBox1;
    }
}