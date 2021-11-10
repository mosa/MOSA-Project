// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Image
	{
		private int[] pixels;

		public int[] Pixels => pixels;

		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public Image(int width, int height)
		{
			Width = width;
			Height = height;

			pixels = new int[Width * Height];
		}

		public int GetColor(int x, int y)
		{
			return pixels[y * Width + x];
		}

		public void SetColor(int x, int y, int color)
		{
			pixels[y * Width + x] = color;
		}

		public void Clear(int color = 0)
		{
			for (int i = 0; i < pixels.Length; i++)
				pixels[i] = color;
		}
	}
}
