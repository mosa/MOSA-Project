// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem;

namespace Mosa.Demo.SVGAWorld.x86
{
	internal static class MosaLogo
	{
		//Size in tiles
		private const int Width = 23;

		private const int Height = 7;

		public static void Draw(int tileSize)
		{
			var positionX = Display.Width / 2 - Width * tileSize / 2;
			var positionY = Display.Height / 2 - Height * tileSize / 2;

			// Can't store these as a static fields, they seem to break something
			int[] logo = { 0x39E391, 0x44145B, 0x7CE455, 0x450451, 0x450451, 0x451451, 0x44E391 };
			int[] colors = { 0x00FF0000, 0x0000FF00, 0x000000FF, 0x00FFFF00 }; // Colors for each pixel

			for (var ty = 0; ty < Height; ty++)
			{
				var data = logo[ty];

				for (var tx = 0; tx < Width; tx++)
				{
					var mask = 1 << tx;

					if ((data & mask) == mask)
					{
						//Each pixel is approximately 5 tiles in width
						Display.DrawRectangle(positionX + tileSize * tx, positionY + tileSize * ty, tileSize,
							tileSize, Color.FromArgb(colors[tx / 6]), true);
					}
				}
			}
		}
	}
}
