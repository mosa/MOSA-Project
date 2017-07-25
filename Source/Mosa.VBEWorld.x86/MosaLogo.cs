using Mosa.DeviceSystem;

namespace Mosa.VBEWorld.x86
{
	static class MosaLogo
	{
		//Size in tiles
		private static uint _width = 23;
		private static uint _height = 7;

		public static void Draw(IFrameBuffer frameBuffer, uint tileSize)
		{
			uint positionX = (frameBuffer.Width / 2) - ((_width * tileSize) / 2);
			uint positionY = (frameBuffer.Height / 2) - ((_height * tileSize) / 2);

			//Can't store these as a static fields, they seem to break something
			uint[] logo = new uint[] { 0x39E391, 0x44145B, 0x7CE455, 0x450451, 0x450451, 0x451451, 0x44E391 };
			uint[] colors = new uint[] { 0x00FF0000, 0x0000FF00, 0x000000FF, 0x00FFFF00 }; //Colors for each pixel

			for (int ty = 0; ty < _height; ty++)
			{
				uint data = logo[ty];

				for (int tx = 0; tx < _width; tx++)
				{
					int mask = 1 << tx;

					if ((data & mask) == mask)
					{
						frameBuffer.FillRectangle(colors[tx / 6], (uint)(positionX + tileSize * tx), (uint)(positionY + tileSize * ty), tileSize, tileSize); //Each pixel is aprox 5 tiles in width
					}
				}
			}
		}
	}
}