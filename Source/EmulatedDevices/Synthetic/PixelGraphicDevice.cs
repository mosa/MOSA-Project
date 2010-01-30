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
using Mosa.DeviceSystem;
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedDevices.Synthetic
{

	/// <summary>
	/// Represents an synthetic pixel graphics Device
	/// </summary>
	public class PixelGraphicDevice : Device, IPixelGraphicsDevice
	{
		private ushort width;
		private ushort height;
		private DisplayForm displayform;
		private System.Drawing.Imaging.BitmapData bitmapData;

		/// <summary>
		/// Initializes a new instance of the <see cref="PixelGraphicDevice"/> class.
		/// </summary>
		public PixelGraphicDevice(DisplayForm displayform)
		{
			base.name = "EmulatedPixelGraphicDevice";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Online;

			this.displayform = displayform;
			width = (ushort)displayform.bitmap.Width;
			height = (ushort)displayform.bitmap.Height;
		}

		/// <summary>
		/// Writes the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixel(Color color, ushort x, ushort y)
		{
			lock (displayform.bitmap) {
				displayform.bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue));
			}
			displayform.Changed = true;
		}

		/// <summary>
		/// Writes the pixel fast.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixelFast(Color color, ushort x, ushort y)
		{
			unsafe {
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
			System.Threading.Monitor.Enter(displayform.bitmap);
			bitmapData = displayform.bitmap.LockBits(new System.Drawing.Rectangle(0, 0, Width, Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
		}

		/// <summary>
		/// Unlocks this instance.
		/// </summary>
		public void Unlock()
		{
			System.Threading.Monitor.Exit(displayform.bitmap);
			displayform.bitmap.UnlockBits(bitmapData);
		}

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public Color ReadPixel(ushort x, ushort y)
		{
			System.Drawing.Color color;
			lock (displayform.bitmap) {
				color = displayform.bitmap.GetPixel(x, y);
			}

			return new Mosa.DeviceSystem.Color(color.R, color.G, color.B);
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
			displayform.Invalidate();
		}
	}
}
