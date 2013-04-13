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
	/// Interface to a pixel graphics device
	/// </summary>
	public interface IPixelGraphicsDevice
	{
		/// <summary>
		/// Writes the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		void WritePixel(Color color, ushort x, ushort y);

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		Color ReadPixel(ushort x, ushort y);

		/// <summary>
		/// Clears the specified color.
		/// </summary>
		/// <param name="color">The color.</param>
		void Clear(Color color);

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