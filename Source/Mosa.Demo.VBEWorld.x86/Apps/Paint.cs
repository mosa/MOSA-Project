// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.VBEWorld.x86.Components;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Apps
{
	public class Paint : Window
	{
		public PaintArea PaintArea;

		public Paint(int x, int y, int width, int height, Color inactiveTitlebarColor, Color activeTitlebarColor, Color bodyColor)
			: base("Paint", x, y, width, height, inactiveTitlebarColor, activeTitlebarColor, bodyColor)
		{
			PaintArea = new PaintArea(x + 2, y + 2 + TitlebarHeight, width - 2, height - 2, bodyColor);
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

			if (Opened)
			{
				PaintArea.X = X;
				PaintArea.Y = Y + TitlebarHeight;
				PaintArea.Update();
			}
		}
	}
}
