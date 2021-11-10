// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.DeviceSystem
{
	public class Bitmap
	{
		public static Image CreateImage(byte[] data)
		{
			var bpp = data[0x1C] / 8;

			if (!(bpp == 3 || bpp == 4))
				return null;

			var stream = new ByteStream(data);

			var sig = string.Empty + Encoding.ASCII.GetChar(stream.Read8(0)) + Encoding.ASCII.GetChar(stream.Read8(1));
			var width = stream.Read32(0x12);
			var height = stream.Read32(0x16);

			if (sig != "BM")
				return null;

			var image = new Image(width, height);
			var dataSectionOffset = stream.Read32(0xA);

			int x = 0;
			int y = height - 1;

			for (var i = dataSectionOffset; i < dataSectionOffset + (width * height * bpp); i += bpp)
			{
				var color = (bpp == 4) ? stream.Read32(i) : (int)(stream.Read24(i) | 0xFF000000); // 32 & 24 bit

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
