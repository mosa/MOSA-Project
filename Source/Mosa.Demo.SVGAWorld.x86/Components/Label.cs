﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem;

namespace Mosa.Demo.SVGAWorld.x86.Components
{
	public class Label
	{
		public int X, Y;

		public string Text;

		public ISimpleFont Font;

		public Color ForeColor;

		public Label(string text, ISimpleFont font, int x, int y, Color foreColor)
		{
			Text = text;
			Font = font;

			X = x;
			Y = y;

			ForeColor = foreColor;
		}

		public void Draw()
		{
			if (!string.IsNullOrEmpty(Text))
				Display.DrawString(X, Y, Text, Font, ForeColor);
		}
	}
}
