namespace Mosa.Tool.Debugger
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.btnConnect = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnViewMemory = new System.Windows.Forms.ToolStripButton();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.odfVMImage = new System.Windows.Forms.OpenFileDialog();
			this.toolStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dockPanel
			// 
			this.dockPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.dockPanel.DockBackColor = System.Drawing.SystemColors.AppWorkspace;
			this.dockPanel.DockBottomPortion = 150D;
			this.dockPanel.DockLeftPortion = 240D;
			this.dockPanel.DockRightPortion = 240D;
			this.dockPanel.DockTopPortion = 150D;
			this.dockPanel.DocumentStyle = WeifenLuo.WinFormsUI.Docking.DocumentStyle.DockingWindow;
			this.dockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
			this.dockPanel.Location = new System.Drawing.Point(0, 28);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.RightToLeftLayout = true;
			this.dockPanel.Size = new System.Drawing.Size(799, 150);
			this.dockPanel.TabIndex = 0;
			// 
			// toolStrip1
			// 
			this.toolStrip1.AllowMerge = false;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.toolStripSeparator,
            this.toolStripButton2,
            this.toolStripSeparator1,
            this.btnViewMemory});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.toolStrip1.Size = new System.Drawing.Size(799, 25);
			this.toolStrip1.TabIndex = 28;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// btnConnect
			// 
			this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
			this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnConnect.Name = "btnConnect";
			this.btnConnect.Size = new System.Drawing.Size(93, 22);
			this.btnConnect.Text = "VM Connect";
			this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(103, 22);
			this.toolStripButton2.Text = "App Locations";
			this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnViewMemory
			// 
			this.btnViewMemory.Image = ((System.Drawing.Image)(resources.GetObject("btnViewMemory.Image")));
			this.btnViewMemory.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnViewMemory.Name = "btnViewMemory";
			this.btnViewMemory.Size = new System.Drawing.Size(100, 22);
			this.btnViewMemory.Text = "View Memory";
			this.btnViewMemory.Click += new System.EventHandler(this.btnViewMemory_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.ImageScalingSize = new System.Drawing.Size(36, 36);
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripSeparator2});
			this.statusStrip1.Location = new System.Drawing.Point(0, 180);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(799, 23);
			this.statusStrip1.TabIndex = 29;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(42, 18);
			this.toolStripStatusLabel1.Text = "Ready!";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
			// 
			// odfVMImage
			// 
			this.odfVMImage.DefaultExt = "img";
			this.odfVMImage.Filter = "Image File|*.img|All Files|*.*";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(799, 203);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.dockPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Text = "MOSA Debugger v1.2";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripButton btnViewMemory;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.OpenFileDialog odfVMImage;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
	}
}
