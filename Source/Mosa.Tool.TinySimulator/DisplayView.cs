using Mosa.TinyCPUSimulator;
using Mosa.TinyCPUSimulator.Adaptor;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mosa.Tool.TinySimulator
{
	/// <summary>
	///
	/// </summary>
	public partial class DisplayView : SimulatorDockContent, ISimDisplay
	{
		/// <summary>
		///
		/// </summary>
		private Bitmap bitmap;

		/// <summary>
		///
		/// </summary>
		private volatile bool changed = false;

		/// <summary>
		///
		/// </summary>
		private volatile bool resized = false;

		/// <summary>
		///
		/// </summary>
		private Timer timer;

		bool ISimDisplay.Changed { get { return changed; } set { changed = value; } }

		bool ISimDisplay.Resized { get { return resized; } set { resized = value; } }

		void ISimDisplay.SetBitMap(object bitmap)
		{
			this.bitmap = bitmap as Bitmap;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DisplayView" /> class.
		/// </summary>
		public DisplayView(MainForm mainForm)
			: base(mainForm)
		{
			InitializeComponent();
			DoubleBuffered = true;
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
				if (changed)
					this.Refresh();
		}

		private void DisplayForm_Paint(object sender, PaintEventArgs e)
		{
			if (bitmap == null)
				return;

			lock (bitmap)
			{
				e.Graphics.DrawImage(bitmap, 0, 0, bitmap.Width, bitmap.Height);

				changed = false;
			}
		}

		public override void UpdateDock(BaseSimState simState)
		{
			this.Refresh();
		}
	}
}