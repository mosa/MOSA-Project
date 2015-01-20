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
	public abstract class FrameBuffer : IFrameBuffer
	{
		/// <summary>
		///
		/// </summary>
		protected uint width;

		/// <summary>
		///
		/// </summary>
		protected uint height;

		/// <summary>
		///
		/// </summary>
		protected IMemory memory;

		/// <summary>
		///
		/// </summary>
		protected uint offset;

		/// <summary>
		///
		/// </summary>
		protected uint depth;

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public uint Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public uint Height { get { return height; } }

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		protected abstract uint GetOffset(uint x, uint y);

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public abstract uint GetPixel(uint x, uint y);

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="color"></param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public abstract void SetPixel(uint color, uint x, uint y);
	}
}