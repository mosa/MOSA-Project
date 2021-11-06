// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	public class Bitmap : Image
	{
		public Bitmap(byte[] data)
		{
			var stream = new ByteStream(data);

			Width = stream.Read32(0x12);
			Height = stream.Read32(0x16);
			Bpp = stream.Read8(0x1C) / 8;

			var dataSectionOffset = stream.Read32(0xA);

			Data = HAL.AllocateMemory((uint)(Width * Height * Bpp), 0); // TEMP

			int[] temp = new int[Width];
			int w = 0, h = Height - 1;

			for (int i = 0; i < (uint)(Width * Height * Bpp); i += Bpp)
			{
				if (w == Width)
				{
					for (uint k = 0; k < temp.Length; k++)
					{
						Data.Write32((uint)((Width * h + k) * Bpp), (uint)temp[k]);
					}
					w = 0;
					h--;
				}

				switch (Bpp)
				{
					case 3: temp[w] = (int)(0xFF000000 | stream.Read32(dataSectionOffset + i)); break; // 24-bit
					case 4: temp[w] = stream.Read32(dataSectionOffset + i); break; // 32-bit
				}

				w++;
			}

			return;
		}
	}
}
