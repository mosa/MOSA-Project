// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Bitmap
	{
		public static Image CreateImage(byte[] data)
		{
			var stream = new ByteStream(data);

			var width = stream.Read32(0x12);
			var height = stream.Read32(0x16);
			var bpp = stream.Read8(0x1C) / 8;

			var image = new Image(width, height);

			var dataSectionOffset = stream.Read32(0xA);

			int[] temp = new int[width];
			int w = 0, h = height - 1;

			for (int i = 0; i < (uint)(width * height * bpp); i += bpp)
			{
				if (w == width)
				{
					for (int k = 0; k < temp.Length; k++)
					{
						image.SetColor(k, h, temp[k]);
					}
					w = 0;
					h--;
				}

				switch (bpp)
				{
					case 3: temp[w] = (int)(0xFF000000 | stream.Read24(dataSectionOffset + i)); break; // 24-bit
					case 4: temp[w] = stream.Read32(dataSectionOffset + i); break; // 32-bit
				}

				w++;
			}

			return image;
		}
	}
}
