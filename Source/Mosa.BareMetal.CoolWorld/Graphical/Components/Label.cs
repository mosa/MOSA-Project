// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem.Fonts;

namespace Mosa.BareMetal.CoolWorld.Graphical.Components;

public class Label
{
	public uint X, Y;

	public string Text;

	public ISimpleFont Font;

	public Color ForeColor;

	public Label(string text, ISimpleFont font, uint x, uint y, Color foreColor)
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
