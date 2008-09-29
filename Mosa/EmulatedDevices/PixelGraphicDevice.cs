/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Threading;
using System.Windows.Forms;
using Mosa.ClassLib;
using Mosa.DeviceDrivers;
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedDevices
{

	/// <summary>
	/// Represents an emulated vga  device
	/// </summary>
	public class PixelGraphicDevice : Device, IPixelGraphicsDevice
	{
		private ushort width;
		private ushort height;
		private System.Drawing.Bitmap bitmap;
		private DisplayForm dislayForm;
        private System.Drawing.Imaging.BitmapData bitmapData;

		/// <summary>
		/// Initializes a new instance of the <see cref="PixelGraphicDevice"/> class.
		/// </summary>
		public PixelGraphicDevice(ushort width, ushort height)
		{
			base.name = "PixelGraphicDevice";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Online;

			this.width = width;
			this.height = height;

			bitmap = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

			Thread thread = new Thread(new ThreadStart(CreateForm));
			thread.Start();
		}

		/// <summary>
		/// Creates the form.
		/// </summary>
		private void CreateForm()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			dislayForm = new DisplayForm(bitmap);
            dislayForm.Width = Width;
            dislayForm.Height = Height;
			Application.Run(dislayForm);
		}

		/// <summary>
		/// Writes the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixel(Color color, ushort x, ushort y)
		{
			lock (bitmap) {
				bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue));
			}
			dislayForm.Changed = true;
		}

        /// <summary>
        /// Writes the pixel fast.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void WritePixelFast(Color color, ushort x, ushort y)
        {
            unsafe
            {
                byte* row = (byte*)bitmapData.Scan0 + (y * bitmapData.Stride);
                row[x * 3 + 2] = color.Red;
                row[x * 3 + 1] = color.Green;
                row[x * 3 + 0] = color.Blue;
            }
        }

        /// <summary>
        /// Locks this instance.
        /// </summary>
        public void Lock()
        {
            bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
        }

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        public void Unlock()
        {
            bitmap.UnlockBits(bitmapData);
            //dislayForm.Changed = true;
        }

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public Color ReadPixel(ushort x, ushort y)
		{
			System.Drawing.Color color = bitmap.GetPixel(x, y);

			return new Mosa.DeviceDrivers.Color(color.R, color.G, color.B);
		}

		/// <summary>
		/// Clears the specified color.
		/// </summary>
		/// <param name="color">The color.</param>
		public void Clear(Color color)
		{
			// TODO
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns></returns>
		public ushort Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns></returns>
		public ushort Height { get { return height; } }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            dislayForm.Invalidate();
        }
	}
}
