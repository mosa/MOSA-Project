// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Implementation of FrameBuffer with 16 Bits Per Pixel
	/// </summary>
	public sealed class FrameBuffer16bpp : FrameBuffer
	{
		/// <summary>Initializes a new instance of the <see cref="FrameBuffer16bpp"/> class.</summary>
		/// <param name="buffer">The memory.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="depth">The depth.</param>
		public FrameBuffer16bpp(ConstrainedPointer buffer, uint width, uint height, uint offset, uint bytesPerLine, bool doubleBuffering = true)
		{
			this.firstBuffer = buffer;
			this.width = width;
			this.height = height;
			this.offset = offset;
			this.bytesPerPixel = 2;     // = 16 bits per pixel
			this.bytesPerLine = bytesPerLine;
			this.doubleBuffering = doubleBuffering;

			if (doubleBuffering)
				secondBuffer = new ConstrainedPointer(GC.AllocateObject(buffer.Size), buffer.Size);
		}

		/// <summary>Gets the offset.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		protected override uint GetOffset(uint x, uint y)
		{
			return offset + y * bytesPerLine + x * 2;
		}

		/// <summary>Sets the pixel.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public override void SetPixel(uint color, uint x, uint y)
		{
			if (x < 0 || y < 0 || x >= width || y >= height)
				return;

			if (doubleBuffering)
				secondBuffer.Write16(GetOffset(x, y), (ushort)color);
			else
				firstBuffer.Write16(GetOffset(x, y), (ushort)color);
		}

		/// <summary>Gets the pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public override uint GetPixel(uint x, uint y)
		{
			if (x < 0 || y < 0 || x >= width || y >= height)
				return 0;

			return doubleBuffering ? secondBuffer.Read16(GetOffset(x, y)) : firstBuffer.Read16(GetOffset(x, y));
		}

		/// <summary>Fills a rectangle with color.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">X of the top left of the rectangle.</param>
		/// <param name="y">Y of the top left of the rectangle.</param>
		/// <param name="w">Width of the rectangle.</param>
		/// <param name="h">Width of the rectangle.</param>
		public override void FillRectangle(uint color, uint x, uint y, uint w, uint h)
		{
			w = Math.Clamp(w, 0, width - x);
            h = Math.Clamp(h, 0, height - y);

            if (x < 0 || y < 0 || x >= width || y >= height)
                return;
			
			uint startAddress = GetOffset(x, y);

			if (doubleBuffering)
			{
				for (uint offsetY = 0; offsetY < h; offsetY++)
				{
					for (uint offsetX = 0; offsetX < w; offsetX++)
					{
						secondBuffer.Write16(startAddress + (offsetX * 2), (ushort)color);
					}

					startAddress = GetOffset(x, y + offsetY);
				}
			}
			else
			{
				for (uint offsetY = 0; offsetY < h; offsetY++)
				{
					for (uint offsetX = 0; offsetX < w; offsetX++)
					{
						firstBuffer.Write16(startAddress + (offsetX * 2), (ushort)color);
					}

					startAddress = GetOffset(x, y + offsetY);
				}
			}
		}
	}
}
