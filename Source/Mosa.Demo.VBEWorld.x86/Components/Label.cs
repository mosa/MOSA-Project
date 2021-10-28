// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	public class Label
	{
		public int X, Y;

		public string Text, FontName;

		public Color ForeColor;

		public Label(string text, string fontName, int x, int y, Color foreColor)
		{
			Text = text;
			FontName = fontName;

			X = x;
			Y = y;

			ForeColor = foreColor;
		}

		public void Draw()
		{
			if (Text != null && Text != string.Empty)
				Display.DrawString(X, Y, Text, FontName, ForeColor);
		}
	}
}
