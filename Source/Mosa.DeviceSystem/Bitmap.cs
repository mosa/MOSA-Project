// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	public class Bitmap : Image
	{
		public Bitmap(byte[] data)
		{
			Pointer ptr;

			unsafe
			{
				fixed (byte* p = data)
					ptr = (Pointer)p;
			}

			Width = (int)ptr.Load32(0x12);
			Height = (int)ptr.Load32(0x16);
			Bpp = ptr.Load8(0x1C) / 8;

			var dataSectionOffset = ptr.Load32(0xA);

			Data = HAL.AllocateMemory((uint)(Width * Height * Bpp), 0);

			int[] temp = new int[Width];
			uint w = 0, h = (uint)Height - 1;

			for (uint i = 0; i < (uint)(Width * Height * Bpp); i += (uint)Bpp)
			{
				if (w == Width)
				{
					for (uint k = 0; k < temp.Length; k++)
						Data.Write32(((uint)Width * h + k) * (uint)Bpp, (uint)temp[k]);

					w = 0;
					h--;
				}

				switch (Bpp)
				{
					case 3: temp[w] = (int)(0xFF000000 | (int)ptr.Load24(dataSectionOffset + i)); break; // 24-bit
					case 4: temp[w] = (int)ptr.Load32(dataSectionOffset + i); break; // 32-bit
				}

				w++;
			}

			return;
		}
	}
}
