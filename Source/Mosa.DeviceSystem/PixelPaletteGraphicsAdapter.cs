// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Translates IPixelPaletteGraphicsDevice to PixelPaletteGraphicsAdapter
	/// </summary>
	public class PixelPaletteGraphicsAdapter : IPixelGraphicsDevice
	{
		/// <summary>
		///
		/// </summary>
		protected IPixelPaletteGraphicsDevice pixelPaletteGraphicsDevice;

		/// <summary>
		///
		/// </summary>
		protected ColorPalette colorPalette;

		/// <summary>
		/// Initializes a new instance of the <see cref="PixelPaletteGraphicsAdapter"/> class.
		/// </summary>
		/// <param name="pixelPaletteGraphicsDevice">The pixel palette graphics device.</param>
		public PixelPaletteGraphicsAdapter(IPixelPaletteGraphicsDevice pixelPaletteGraphicsDevice)
		{
			this.pixelPaletteGraphicsDevice = pixelPaletteGraphicsDevice;

			if (pixelPaletteGraphicsDevice.PaletteSize == 256)
				this.colorPalette = ColorPalette.CreateNetscape256ColorPalette();
			else
				this.colorPalette = ColorPalette.CreateStandard16ColorPalette();
		}

		/// <summary>
		/// Writes the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixel(Color color, ushort x, ushort y)
		{
			pixelPaletteGraphicsDevice.WritePixel(colorPalette.FindClosestMatch(color), x, y);
		}

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public Color ReadPixel(ushort x, ushort y)
		{
			return colorPalette.GetColor(pixelPaletteGraphicsDevice.ReadPixel(x, y));
		}

		/// <summary>
		/// Clears the specified color.
		/// </summary>
		/// <param name="color">The color.</param>
		public void Clear(Color color)
		{
			pixelPaletteGraphicsDevice.Clear(colorPalette.FindClosestMatch(color));
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns></returns>
		public ushort Width { get { return pixelPaletteGraphicsDevice.Width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns></returns>
		public ushort Height { get { return pixelPaletteGraphicsDevice.Height; } }
	}
}
