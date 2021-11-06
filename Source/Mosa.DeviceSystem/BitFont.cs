// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// BitFont (dynamic fonts)
	/// </summary>
	public class BitFont : IFont
	{
		private string charset;
		private byte[] data;
		private int size;

		public BitFont(string name, string charset, byte[] data, int size)
		{
			Name = name;
			Size = size;
			this.charset = charset;
			this.data = data;
		}

		/// <summary>Gets the name.</summary>
		/// <value>The name.</value>
		public string Name { get; private set; }

		/// <summary>Gets the size.</summary>
		/// <value>The size.</value>
		public int Size { get; private set; }

		/// <summary>Draws the string.</summary>
		public void DrawString(IFrameBuffer frameBuffer, uint color, uint x, uint y, string text)
		{
			int size8 = size / 8;
			var lines = text.Split('\n');           // FUTURE: Avoid splitting the string

			for (int l = 0; l < lines.Length; l++)
			{
				int usedX = 0;
				for (int i = 0; i < lines[l].Length; i++)
				{
					char c = lines[l][i];
					int index = charset.IndexOf(c);

					if (index < 0)
					{
						usedX += size / 2;
						continue;
					}

					int maxX = 0, sizePerFont = size * size8 * index, X = usedX + (int)x, Y = (int)y + size * l;

					for (int h = 0; h < size; h++)
						for (int aw = 0; aw < size8; aw++)
							for (int ww = 0; ww < 8; ww++)
								if ((data[sizePerFont + (h * size8) + aw] & (0x80 >> ww)) != 0)
								{
									int max = (aw * 8) + ww;

									int xx = X + max;
									int yy = Y + h;

									frameBuffer.SetPixel(color, (uint)xx, (uint)yy);

									if (max > maxX)
										maxX = max;
								}

					usedX += maxX + 2;
				}
			}
		}
	}
}
