// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Text;

namespace Mosa.DeviceSystem
{
	public struct BitmapHeader
	{
		public string Type;
		public uint Size;
		public uint DataSectionOffset;
		public uint Width;
		public uint Height;
		public uint Bpp;
	}

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

			BitmapHeader bitmapHeader = new BitmapHeader();

			bitmapHeader.Type = string.Empty + Encoding.ASCII.GetChar(ptr.Load8(0)) + Encoding.ASCII.GetChar(ptr.Load8(1));
			bitmapHeader.Size = ptr.Load32(2);
			bitmapHeader.DataSectionOffset = ptr.Load32(0xA);
			bitmapHeader.Width = ptr.Load32(0x12);
			bitmapHeader.Height = ptr.Load32(0x16);
			bitmapHeader.Bpp = ptr.Load8(0x1C);

			if (bitmapHeader.Type != "BM")
				throw new Exception("This is not a bitmap");

			if (bitmapHeader.Bpp != 24 && bitmapHeader.Bpp != 32)
				throw new Exception(bitmapHeader.Bpp + " bits bitmap is not supported");

			Width = (int)bitmapHeader.Width;
			Height = (int)bitmapHeader.Height;
			Bpp = (int)bitmapHeader.Bpp;
			RawData = GC.AllocateObject((uint)(Width * Height * Bpp));

			int[] temp = new int[Width];
			uint w = 0, h = (uint)Height - 1;
			uint depth = (uint)(Bpp / 8);

			for (uint i = 0; i < Width * Height * depth; i += depth)
			{
				if (w == Width)
				{
					for (uint k = 0; k < temp.Length; k++)
						RawData.Store32(((uint)Width * h + k) * 4, temp[k]);

					w = 0;
					h--;
				}

				switch (Bpp)
				{
					case 24:
						temp[w] = (int)(0xFF000000 | (int)ptr.Load24(bitmapHeader.DataSectionOffset + i));
						break;

					case 32:
						temp[w] = (int)ptr.Load32(bitmapHeader.DataSectionOffset + i);
						break;
				}

				w++;
			}

			return;
		}
	}
}
