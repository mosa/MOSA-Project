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
	/// Implementation of FrameBuffer with 24 Bits Per Pixel
	/// </summary>
	public class FrameBuffer24bpp : FrameBuffer, IFrameBuffer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FrameBuffer24bpp"/> class.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="bytesPerLine">The bytes per line.</param>
		public FrameBuffer24bpp(IMemory memory, uint width, uint height, uint offset, uint bytesPerLine)
		{
			this.memory = memory;
			this.width = width;
			this.height = height;
			this.offset = offset;
			this.bytesPerLine = bytesPerLine;
		}

		/// <summary>
		/// Gets the offset.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		protected override uint GetOffset(uint x, uint y)
		{
			return (uint)(offset + (y * bytesPerLine) + (x * 3));
		}

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public override uint GetPixel(uint x, uint y)
		{
			return memory.Read24(GetOffset(x, y));
		}

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public override void SetPixel(uint color, uint x, uint y)
		{
			memory.Write24(GetOffset(x, y), (ushort)color);
		}
	}
}