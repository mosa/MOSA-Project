// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.BareMetal.CoolWorld.Graphical.Components;

namespace Mosa.BareMetal.CoolWorld.Graphical.Apps;

public class Paint : Window
{
	public readonly PaintArea PaintArea;

	public Paint(uint x, uint y, uint width, uint height, Color inactiveTitlebarColor, Color activeTitlebarColor, Color bodyColor)
		: base("Paint", x, y, width, height, inactiveTitlebarColor, activeTitlebarColor, bodyColor)
	{
		PaintArea = new PaintArea(x, y + TitlebarHeight, width, height, bodyColor);
	}

	public override void Draw()
	{
		base.Draw();

		if (Opened)
			PaintArea.Draw();
	}

	public override void Update()
	{
		base.Update();

		if (!Opened || WindowManager.ActiveWindow != this)
			return;

		PaintArea.X = X;
		PaintArea.Y = Y + TitlebarHeight;
		PaintArea.Update();
	}
}
