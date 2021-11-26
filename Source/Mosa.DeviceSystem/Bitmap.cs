// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Bitmap
	{
		public static Image CreateImage(byte[] data)
		{
			if (data[0] != (byte)'B' || data[1] != (byte)'M')
				return null;

			var bpp = (uint)(data[0x1C] / 8);

			if (bpp != 3 && bpp != 4)
				return null;

			var stream = new ByteStream(data);

			var width = stream.Read32(0x12);
			vardataSectionOffset height = stream.Read32(0x16);
			var dataSectionOffset = stream.Read32(0xA);

			var image = new Image(width, height, bpp);

			uint x = 0;
			uint y = height - 1;

			var offset = dataSectionOffset + (width * height * 4);
			for (var i = dataSectionOffset; i < offset; i += 4)
			{
				uint color;

				// This should handle padding for non 32-bit bitmaps.
				// If i == offset, the series of bytes would look like this:
				// 48 56 32 0 // That zero byte is the start of empty pad data
				// If we remove, say, 1 to i (for 24-bit bitmaps), the series of bytes would now look like this:
				// 48 56 32 // Notice how the zero byte is now gone
				// However, we would also have to read depending if the series of bytes is now
				// 3-byte, 2-byte of 1-byte aligned (Read24(), Read16() and Read8() respectively)
				if (bpp < 4 && i == offset)
				{
					i -= 4 - bpp;

					// TODO: Other color depths
					color = (i == 1) ? (uint)(stream.Read24(i) | 0xFF000000) : 0;
				}
				else color = (bpp == 4) ? stream.Read32(i) : (uint)(stream.Read24(i) | 0xFF000000); // 32 & 24 bit

				image.SetColor(x, y, color);

				x++;

				if (x == width)
				{
					x = 0;
					y--;
				}
			}

			return image;
		}
	}
}
