// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Bitmap
	{
		public static Image CreateImage(byte[] data)
		{
			if (data[0] != (byte)'B' && data[1] != (byte)'M')
				return null;

			var bpp = (uint)(data[0x1C] / 8);

			if (bpp != 3 && bpp != 4)
				return null;

			var stream = new ByteStream(data);

			var width = stream.Read32(0x12);
			var height = stream.Read32(0x16);
			var dataSectionOffset = stream.Read32(0xA);

			var image = new Image(width, height, bpp);

			uint x = 0;
			uint y = height - 1;

			for (var i = dataSectionOffset; i < dataSectionOffset + image.Pixels.Size; i += bpp)
			{
				image.SetColor(x, y, stream.Read32((int)i));

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
