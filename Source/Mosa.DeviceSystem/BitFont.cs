// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// BitFont (dynamic fonts)
	/// </summary>
	public class BitFont : IFont
	{
		private string charset;
		private byte[] buffer;
		private int size;
		private string name;

		/// <summary>Gets the charset.</summary>
		/// <value>The charset.</value>
		public string Charset { get { return charset; } set { charset = value; } }

		/// <summary>Gets the name.</summary>
		/// <value>The name.</value>
		public string Name { get { return name; } set { name = value; } }

		/// <summary>Gets the size.</summary>
		/// <value>The size.</value>
		public int Size { get { return size; } set { size = value; } }

		/// <summary>Gets the buffer.</summary>
		/// <value>The buffer.</value>
		public byte[] Buffer { get { return buffer; } set { buffer = value; } }

		/// <summary>Draws the string.</summary>
		public void DrawString(IFrameBuffer frameBuffer, uint color, uint x, uint y, string text)
		{
			int size8 = size / 8;
			string[] lines = text.Split('\n');

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
								if ((buffer[sizePerFont + (h * size8) + aw] & (0x80 >> ww)) != 0)
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
