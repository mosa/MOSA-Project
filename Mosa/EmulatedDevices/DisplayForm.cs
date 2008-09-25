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
		/// Initializes a new instance of the <see cref="DisplayForm"/> class.
		/// </summary>
		public DisplayForm()
		{
			InitializeComponent();
		}

		private void DisplayForm_Paint(object sender, PaintEventArgs e)
		{

		}

		/// <summary>
		/// Writes the pixel.
		/// </summary>
		public void WritePixel()
		{
			System.Drawing.Graphics objGraphics;
			objGraphics = this.CreateGraphics();
		}

		private void DisplayForm_Load(object sender, EventArgs e)
		{

		}
	}
}
