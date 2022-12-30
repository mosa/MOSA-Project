// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.Demo.SVGAWorld.x86.Components;

namespace Mosa.Demo.SVGAWorld.x86.Apps
{
	public class Paint : Window
	{
		public PaintArea PaintArea;

		public Paint(int x, int y, int width, int height, Color inactiveTitlebarColor, Color activeTitlebarColor, Color bodyColor)
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
}
