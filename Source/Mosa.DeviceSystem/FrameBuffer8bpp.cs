// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Implementation of FrameBuffer with 8 Bits Per Pixel
	/// </summary>
	public sealed class FrameBuffer8bpp : FrameBuffer
	{
		/// <summary>Initializes a new instance of the <see cref="FrameBuffer8bpp"/> class.</summary>
		/// <param name="buffer">The memory.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="depth">The depth.</param>
		public FrameBuffer8bpp(ConstrainedPointer buffer, uint width, uint height, uint offset, uint bytesPerLine, bool doubleBuffering = true)
		{
			this.Buffer = buffer;
			this.Width = width;
			this.Height = height;
			this.offset = offset;
			this.bytesPerPixel = 1;     // = 8 bits per pixel
			this.bytesPerLine = bytesPerLine;
		}

		/// <summary>Gets the pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public override uint GetPixel(uint x, uint y)
		{
			if (x < 0 || y < 0 || x >= Width || y >= Height)
				return 0;

			return Buffer.Read8(GetOffset(x, y));
		}

		/// <summary>Sets the pixel.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public override void SetPixel(uint color, uint x, uint y)
		{
			if (x < 0 || y < 0 || x >= Width || y >= Height)
				return;

			Buffer.Write8(GetOffset(x, y), (byte)color);
		}
	}
}
