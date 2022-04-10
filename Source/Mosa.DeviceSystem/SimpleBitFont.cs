// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// BitFont (dynamic fonts)
	/// </summary>
	public class SimpleBitFont : ISimpleFont
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>
		/// The name.
		/// </value>
		public string Name { get; }

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public int Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public int Height { get; }

		/// <summary>
		/// Gets the size.
		/// </summary>
		/// <value>The size.</value>
		public int Size { get; }

		private readonly string Charset;

		private readonly byte[] Buffer;

		public SimpleBitFont(string name, int width, int height, int size, string charset, byte[] data)
		{
			Name = name;

			Width = width;
			Height = height;
			Size = size;

			Charset = charset;

			Buffer = data;
		}

		/// <summary>
		/// Draws the string.
		/// </summary>
		public void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text)
		{
			var size8 = Size / 8;
			var usedX = 0;

			for (var i = 0; i < text.Length; i++)
			{
				var maxX = 0;
				var xx = usedX + x;
				var c = text[i];
				var index = Charset.IndexOf(c);
				var sizePerFont = Size * size8 * index;

				if (index < 0)
				{
					if (c == '\n')
					{
						usedX = 0;
						y += (uint)Size;
					}
					else if (c == '\r') usedX = 0;
					else usedX += Width;

					continue;
				}

				for (var h = 0; h < Size; h++)
					for (var aw = 0; aw < size8; aw++)
						for (var ww = 0; ww < 8; ww++)
							if ((Buffer[sizePerFont + h * size8 + aw] & (0x80 >> ww)) != 0)
							{
								var max = aw * 8 + ww;

								frameBuffer.SetPixel(color, (uint)(xx + max), (uint)(y + h));

								if (max > maxX)
									maxX = max;
							}

				usedX += maxX + 2;
			}
		}

		/// <summary>
		/// Calculates the width of a character.
		/// </summary>
		public int CalculateWidth(char c)
		{
			var size8 = Size / 8;
			var index = Charset.IndexOf(c);

			if (index < 0)
				return Width;

			int maxX = 0, sizePerFont = Size * size8 * index;

			for (var h = 0; h < Size; h++)
				for (var aw = 0; aw < size8; aw++)
					for (var ww = 0; ww < 8; ww++)
						if ((Buffer[sizePerFont + h * size8 + aw] & (0x80 >> ww)) != 0)
						{
							var max = aw * 8 + ww;

							if (max > maxX)
								maxX = max;
						}

			return maxX;
		}

		/// <summary>
		/// Calculates the width of a string.
		/// </summary>
		public int CalculateWidth(string s)
		{
			var r = 0;
			for (var i = 0; i < s.Length; i++)
				r += CalculateWidth(s[i]) + 2;

			return r;
		}
	}
}
