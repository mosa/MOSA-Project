// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Demo.SVGAWorld.x86.Components;
using System.Drawing;
using Mosa.DeviceSystem;

namespace Mosa.Demo.SVGAWorld.x86.Apps
{
	public class Notepad : Window
	{
		private GraphicalConsole Console;

		private string Line;

		public Notepad(int x, int y, int width, int height, Color inactiveTitlebarColor, Color activeTitlebarColor, Color bodyColor)
			: base("Notepad", x, y, width, height, inactiveTitlebarColor, activeTitlebarColor, bodyColor)
		{
			Line = string.Empty;

			Console = new GraphicalConsole(x, y + TitlebarHeight, width, height, Display.DefaultFont, Boot.Keyboard, Display.BackFrame, Color.Black);
			Console.Clear();
		}

		public override void Draw()
		{
			base.Draw();

			if (!Opened)
				return;

			Console.Draw();
		}

		public override void Update()
		{
			base.Update();

			if (!Opened || WindowManager.ActiveWindow != this)
				return;

			Console.Font = Display.DefaultFont;
			Console.BaseX = X;
			Console.BaseY = Y + TitlebarHeight;

			var code = Console.ReadKey(false);
			if (code == null)
				return;

			if (code.KeyType == KeyType.Home)
			{
				Console.NewLine();
				return;
			}

			if (code.KeyType == KeyType.Delete && Line.Length > 0)
			{
				Console.Previous();
				Console.Characters.RemoveAt(Console.Characters.Count - 1);
				Line = Line.Substring(0, Line.Length - 1);

				return;
			}

			Line += code;
			Console.Write(code.Character);
		}
	}
}
