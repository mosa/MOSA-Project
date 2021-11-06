// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class Image
	{
		public ConstrainedPointer Data { get; protected set; }

		//public byte[] Pixel { get; protected set; }

		public int Bpp { get; protected set; }
		public int Width { get; protected set; }
		public int Height { get; protected set; }

		public Image(int width, int height)
		{
			Width = width;
			Height = height;
			Bpp = 4;

			//Pixel = new byte[width * Height * Bpp];

			Data = HAL.AllocateMemory((uint)(width * height * Bpp), 0);
		}

		public Image()
		{
		}
	}
}
