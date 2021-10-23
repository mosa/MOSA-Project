// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Text;

namespace Mosa.DeviceSystem
{
	public struct BitmapHeader
	{
		public char[] Signature;
		public uint Size;
		public uint DataSectionOffset;
		public uint Width;
		public uint Height;
		public uint Depth;
	}

	public class Bitmap : Image
	{
		public static char[] Signature = new char[2] { 'B', 'M' };

		public BitmapHeader Header;

		public Bitmap(byte[] data)
		{
			Pointer ptr = Intrinsic.GetObjectAddress(data);   // HACK - FIX ME! Unsafe. GetObjectAddress method is only for low level internal use.

			Header = new BitmapHeader();
			Header.Signature = new char[2] { Encoding.ASCII.GetChar(ptr.Load8(0)), Encoding.ASCII.GetChar(ptr.Load8(1)) };
			Header.Size = ptr.Load32(2);
			Header.DataSectionOffset = ptr.Load32(0xA);
			Header.Width = ptr.Load32(0x12);
			Header.Height = ptr.Load32(0x16);
			Header.Depth = ptr.Load8(0x1C);

			if (Header.Signature != Signature)
				throw new Exception("Given image is not a bitmap (signature recognization failed)");

			Width = (int)Header.Width;
			Height = (int)Header.Height;
			Bpp = (int)Header.Depth / 8;

			Size = (uint)(Width * Height * Bpp);
			RawData = GC.AllocateObject(Size);              // HACK - FIX ME! This is not an object. Use new byte[] instead

			uint len = (uint)Width;
			Pointer temp = GC.AllocateObject(len);          // HACK - FIX ME! This is not an object. Use new byte[] instead
			uint w = 0, h = (uint)Height - 1;

			for (uint i = 0; i < Size; i += (uint)Bpp)
			{
				if (w == Width)
				{
					switch (Bpp)
					{
						case 1: // 8 bit
							for (uint k = 0; k < len; k++)
								RawData.Store8((uint)Width * h + k, temp.Load8(k));
							break;

						case 2: // 16 bit
							for (uint k = 0; k < len; k++)
								RawData.Store16((uint)Width * h + k, temp.Load16(k));
							break;

						case 3: // 24 bit
							for (uint k = 0; k < len; k++)
								RawData.Store24((uint)Width * h + k, temp.Load24(k));
							break;

						case 4: // 32 bit
							for (uint k = 0; k < len; k++)
								RawData.Store32((uint)Width * h + k, temp.Load32(k));
							break;
					}

					w = 0;
					h--;
				}

				uint offset = Header.DataSectionOffset + i;
				switch (Bpp)
				{
					case 1: // 8 bit
						temp.Store8(w, ptr.Load8(offset));
						break;

					case 2: // 16 bit
						temp.Store16(w, ptr.Load16(offset));
						break;

					case 3: // 24 bit
						temp.Store24(w, ptr.Load24(offset));
						break;

					case 4: // 32 bit
						temp.Store32(w, ptr.Load32(offset));
						break;
				}

				w++;
			}

			Internal.MemoryClear(temp, len);    // HACK - FIX ME! MemoryClear is for low level internal use only.

			return;
		}
	}
}
