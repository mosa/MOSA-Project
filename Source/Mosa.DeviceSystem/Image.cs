// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	public class Image
	{
		public Pointer RawData;

		public int Bpp;
		public int Width;
		public int Height;

		public uint Size;

		public Image(int width, int height, int depth)
		{
			Width = width;
			Height = height;
			Bpp = depth / 8;

			Size = (uint)(width * height * Bpp);
			RawData = GC.AllocateObject(Size);
		}

		public Image() { }

		public Image ScaleImage(int NewWidth, int NewHeight)
		{
			Pointer temp = GC.AllocateObject((uint)(NewWidth * NewHeight * Bpp));
			
			int w1 = Width, h1 = Height;
			int x_ratio = ((w1 << 16) / NewWidth) + 1, y_ratio = ((h1 << 16) / NewHeight) + 1, x2, y2;

			for (int i = 0; i < NewHeight; i++)
				for (int j = 0; j < NewWidth; j++)
				{
					x2 = ((j * x_ratio) >> 16);
					y2 = ((i * y_ratio) >> 16);
					temp.Store32((uint)((i * NewWidth) + j), RawData.Load32((uint)((y2 * w1) + x2)));
				}

			Internal.MemoryClear(RawData, Size);

			return new Image()
			{
				Width = NewWidth,
				Height = NewHeight,
				Bpp = Bpp,
				RawData = temp
			};
		}
	}
}
