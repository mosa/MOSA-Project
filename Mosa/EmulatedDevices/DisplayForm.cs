using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Mosa.EmulatedDevices
{
	/// <summary>
	/// 
	/// </summary>
	public partial class DisplayForm : Form
	{
		/// <summary>
		/// 
		/// </summary>
		public volatile bool Changed = false;

		private Bitmap bitmap;
		private Timer timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayForm"/> class.
		/// </summary>
		public DisplayForm(Bitmap bitmap)
		{
			this.bitmap = bitmap;
			timer = new Timer();
			InitializeComponent();
			timer.Interval = 1000;
			timer.Start();
			//timer.Tick += new EventHandler(Timer_Tick);
            this.DoubleBuffered = true;
        }

		private void Timer_Tick(object sender, EventArgs eArgs)
		{
			if (sender == timer)
				if (Changed) {
					this.Refresh();
				}

		}

		private void DisplayForm_Paint(object sender, PaintEventArgs e)
		{
			lock (bitmap) {
				e.Graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
				Changed = false;
			}
		}

		/// <summary>
		/// Paints the background of the control.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
      protected override void OnPaintBackground(PaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

	  /// <summary>
	  /// Handles the Load event of the DisplayForm control.
	  /// </summary>
	  /// <param name="sender">The source of the event.</param>
	  /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		private void DisplayForm_Load(object sender, EventArgs e)
		{

		}
	}
}
