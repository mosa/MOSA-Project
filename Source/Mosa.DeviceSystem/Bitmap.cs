// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Bitmap
	{
		public static Image CreateImage(byte[] data)
		{
			if (data[0] != (byte)'B' || data[1] != (byte)'M')
				return null;

			var bytesPerPixel = (uint)(data[0x1C] / 8);

			if (bytesPerPixel != 3 && bytesPerPixel != 4)
				return null;

			var stream = new ByteStream(data);

			var width = stream.Read32(0x12);
			var height = stream.Read32(0x16);
			var dataSectionOffset = stream.Read32(0xA);

			var image = new Image(width, height, bytesPerPixel);

			// TODO: Support for other color depths
			var y = height - 1;
			do
			{
				for (uint x = 0; x < width - 1; x++)
				{
					dataSectionOffset += bytesPerPixel;

					var color = bytesPerPixel == 4 ? stream.Read32(dataSectionOffset) : stream.Read24(dataSectionOffset) | 0xFF000000;
					image.SetColor(x, y, color);
				}

				if (bytesPerPixel == 3 && width * bytesPerPixel % 4 > 0)
					dataSectionOffset += 4;

				y--;
			} while (y > 0);

			return image;
		}
	}
}
