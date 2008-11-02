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
	/// Implementation of BitMap with 32 Bits Per Pixel
	/// </summary>
	public class BitMap32bpp : BitMap, IBitMap
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="BitMap32bpp"/> class.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="bytesPerLine">The bytes per line.</param>
		public BitMap32bpp(IMemory memory, uint width, uint height, uint offset, uint bytesPerLine)
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
			return (uint)(offset + (y * bytesPerLine) + (x << 2));
		}

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public override uint GetPixel(uint x, uint y)
		{
			return memory.Read32(GetOffset(x, y), 4);
		}

		/// <summary>
		/// Sets the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public override void SetPixel(uint color, uint x, uint y)
		{
			memory.Write32(GetOffset(x, y), color, 4);
		}
	}

}
