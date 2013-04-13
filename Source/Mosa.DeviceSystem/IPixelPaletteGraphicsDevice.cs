/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a pixel graphics device using an indexed palette
	/// </summary>
	public interface IPixelPaletteGraphicsDevice
	{
		/// <summary>
		/// Writes the pixel.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		void WritePixel(byte colorIndex, ushort x, ushort y);

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		byte ReadPixel(ushort x, ushort y);

		/// <summary>
		/// Clears device with the specified color index.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		void Clear(byte colorIndex);

		/// <summary>
		/// Sets the palette.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <param name="color">The color.</param>
		void SetPalette(byte colorIndex, Color color);

		/// <summary>
		/// Gets the palette.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <returns></returns>
		Color GetPalette(byte colorIndex);

		/// <summary>
		/// Gets the size of the palette.
		/// </summary>
		/// <value>The size of the palette.</value>
		ushort PaletteSize { get; }

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns></returns>
		ushort Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns></returns>
		ushort Height { get; }
	}
}