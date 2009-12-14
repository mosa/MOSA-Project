using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Mosa.DeviceSystem;

namespace Mosa.EmulatedDevices.Synthetic
{
	/// <summary>
	/// 
	/// </summary>
	public partial class DisplayForm : Form
	{
		/// <summary>
		/// 
		/// </summary>
		public Bitmap bitmap;
		/// <summary>
		/// 
		/// </summary>
		public Graphics graphic;

		/// <summary>
		/// 
		/// </summary>
		public volatile bool Changed = false;

		/// <summary>
		/// 
		/// </summary>
		private volatile bool Resized = false;
		/// <summary>
		/// 
		/// </summary>
		public Keyboard.KeyPressed onKeyPressed;

		private Timer timer;

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayForm"/> class.
		/// </summary>
		public DisplayForm(int width, int height)
		{
			InitializeComponent();
			SetSize(width, height);
			DoubleBuffered = true;
		}

		/// <summary>
		/// Sets the size.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public void SetSize(int width, int height)
		{
			bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			graphic = Graphics.FromImage(bitmap);
			Resized = true;
		}

		/// <summary>
		/// Starts the timer.
		/// </summary>
		public void StartTimer()
		{
			timer = new Timer();
			timer.Interval = 250;
			timer.Tick += new EventHandler(Timer_Tick);
			timer.Start();
		}

		private void Timer_Tick(object sender, EventArgs eArgs)
		{
			if (sender == timer)
				if (Changed)
					this.Refresh();
		}

		private void DisplayForm_Paint(object sender, PaintEventArgs e)
		{
			lock (bitmap) {
				e.Graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);
				if (Resized) {
					Width = bitmap.Width + 12;
					Height = bitmap.Height + 10;
					Resized = true;
				}
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

		private void DisplayForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (onKeyPressed != null)
				onKeyPressed(new Key(e.KeyChar));
		}

		private void DisplayForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this == Setup.PrimaryDisplayForm)
				Environment.Exit(0);
		}

	}
}
