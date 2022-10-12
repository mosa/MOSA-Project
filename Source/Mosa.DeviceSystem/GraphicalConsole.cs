// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Drawing;

namespace Mosa.DeviceSystem
{
	public class Character
	{
		public char Char;
		public int X, Y;

		public Character(char c, int x, int y)
		{
			Char = c;
			X = x;
			Y = y;
		}
	}

	//https://github.com/nifanfa/MOSA-Core/blob/master/src/Mosa/Mosa.External.x86/VBEConsole.cs
	// TODO: Scrolling support
	public class GraphicalConsole
	{
		#region Definitions

		public int BaseX { get; set; }

		public int BaseY { get; set; }

		public int X { get; private set; }

		public int Y { get; private set; }

		public int Width { get; }

		public int Height { get; }

		public ISimpleFont Font { get; set; }

		public Keyboard Keyboard { get; }

		public FrameBuffer32 FrameBuffer { get; }

		public Color ForeColor { get; }

		public List<Character> Characters { get; }

		private int LastCharWidth;

		private string Line;

		#endregion

		public GraphicalConsole(int baseX, int baseY, int width, int height, ISimpleFont font, Keyboard keyboard, FrameBuffer32 frameBuffer, Color foreColor)
		{
			BaseX = baseX;
			BaseY = baseY;
			X = BaseX;
			Y = BaseY;
			Width = width;
			Height = height;
			Font = font;
			Keyboard = keyboard;
			FrameBuffer = frameBuffer;
			ForeColor = foreColor;
			Characters = new List<Character>(Font.Width * Width * Font.Height * Height);
			Line = string.Empty;
		}

		public void SetPosition(int x, int y)
		{
			X = x * LastCharWidth;
			Y = y * Font.Height;
		}

		public void Next(int width = 0)
		{
			X += width == 0 ? Font.Width : width;

			if (X >= Width)
				NewLine();
		}

		public void Previous()
		{
			if (X == 0 && Y == 0)
				return;

			if (X == 0)
			{
				Y -= Font.Height;
				X = Width - LastCharWidth;
			} else X -= LastCharWidth;
		}

		public void NewLine()
		{
			X = 0;
			Y += Font.Height;
		}

		public void Draw()
		{
			foreach (var c in Characters)
				Font.DrawString(FrameBuffer, (uint)ForeColor.ToArgb(), (uint)(BaseX + c.X), (uint)(BaseY + c.Y), c.Char.ToString());
		}

		public void Write(char c)
		{
			switch (c)
			{
				case '\n':
					NewLine();
					break;

				case '\r':
					X = 0;
					break;

				case '\t':
					X += 4;
					break;

				default:
					Characters.Add(new Character(c, X, Y));
					LastCharWidth = Font.CalculateWidth(c);
					Next(LastCharWidth);
					break;
			}
		}

		public void Write(string s)
		{
			for (var i = 0; i < s.Length; i++)
				Write(s[i]);
		}

		public void WriteLine(char c)
		{
			Write(c);
			NewLine();
		}

		public void WriteLine(string s)
		{
			Write(s);
			NewLine();
		}

		public string ReadLine()
		{
			var code = ReadKey();

			if (code != null)
				switch ((byte)code.Character)
				{
					// Enter key
					case 10:
						NewLine();
						var l = Line;
						Line = string.Empty;
						return l;

					// Backspace key
					case 8:
						if (Line.Length > 0)
						{
							Previous();
							Characters.RemoveAt(Characters.Count - 1);
							Line = Line[0..^1];
						}
						break;

					// Any other key
					default:
						Line += code.Character;
						Write(code.Character);
						break;
				}

			return string.Empty;
		}

		public Key ReadKey()
		{
			return Keyboard.GetKeyPressed();
		}

		public void ToTop()
		{
			// Avoids unnecessary operations from SetPosition()
			X = 0;
			Y = 0;
		}

		public void Clear()
		{
			Characters.Clear();
			ToTop();
		}
	}
}
