// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Image
	{
		public ConstrainedPointer RawData;

		public int Bpp;
		public int Width;
		public int Height;

		public Image(int width, int height)
		{
			Width = width;
			Height = height;
			Bpp = 4;

			RawData = HAL.AllocateMemory((uint)(width * height * Bpp), 0);
		}

		public Image()
		{
		}

		// TODO: Fix
		public Image ScaleImage(int NewWidth, int NewHeight)
		{
			var temp = HAL.AllocateMemory((uint)(NewWidth * NewHeight * Bpp), 0);

			int w1 = Width, h1 = Height;
			int x_ratio = ((w1 << 16) / NewWidth) + 1, y_ratio = ((h1 << 16) / NewHeight) + 1;
			int x2, y2;

			for (int i = 0; i < NewHeight; i++)
				for (int j = 0; j < NewWidth; j++)
				{
					x2 = ((j * x_ratio) >> 16);
					y2 = ((i * y_ratio) >> 16);
					temp.Write32(((uint)((i * NewWidth) + j)) * (uint)Bpp, RawData.Read32(((uint)((y2 * w1) + x2)) * (uint)Bpp));
				}

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
