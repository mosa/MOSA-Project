// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	// TODO: Add support for other color depths (8-bit, 16-bit and 24-bit)
	public class Image
	{
		public ConstrainedPointer Pixels { get; protected set; }

		public uint Width { get; protected set; }
		public uint Height { get; protected set; }
		public uint BytesPerPixel { get; protected set; }

		public Image(uint width, uint height, uint bytesPerPixel)
		{
			Width = width;
			Height = height;
			BytesPerPixel = bytesPerPixel;

			// Allocates virtual memory
			Pixels = HAL.AllocateMemory(Width * Height * BytesPerPixel, 0);
		}

		private uint GetOffset(uint x, uint y)
		{
			return (y * Width + x) * BytesPerPixel;
		}

		public uint GetColor(uint x, uint y)
		{
			return Pixels.Read32(GetOffset(x, y));
		}

		public void SetColor(uint x, uint y, uint color)
		{
			Pixels.Write32(GetOffset(x, y), color);
		}

		public void Clear(uint color = 0)
		{
			Internal.MemorySet(Pixels.Address, color, Pixels.Size);
		}
	}
}
