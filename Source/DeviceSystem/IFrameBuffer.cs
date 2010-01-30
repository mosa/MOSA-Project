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
	/// 
	/// </summary>
	public interface IFrameBuffer
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		uint Width { get; }
		
		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		uint Height { get; }

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		uint GetPixel(uint x, uint y);

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		void SetPixel(uint color, uint x, uint y);

		//TODO: Add more methods
	}

}
