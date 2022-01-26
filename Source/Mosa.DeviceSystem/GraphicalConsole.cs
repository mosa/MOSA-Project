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

	//https://github.com/nifanfa/MOSA-Core/blob/master/Mosa/Mosa.External.x86/VBEConsole.cs
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

		public Color BackColor { get; }

		public Color ForeColor { get; }

		public List<Character> Characters { get; }

		private int LastCharWidth;

		#endregion

		public GraphicalConsole(int baseX, int baseY, int width, int height, ISimpleFont font, Keyboard keyboard, FrameBuffer32 frameBuffer, Color backColor, Color foreColor)
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

			BackColor = backColor;
			ForeColor = foreColor;

			Characters = new List<Character>();
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
			Characters.Add(new Character(c, X, Y));

			LastCharWidth = Font.CalculateWidth(c);
			Next(LastCharWidth);
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
			var line = string.Empty;
			Key code;

			for (;;)
			{
				code = ReadKey();

				if (code.Character == (char)0x1C) // Enter
					break;

				if (code.KeyType == KeyType.Delete && line.Length > 0)
				{
					Previous();
					Characters.RemoveAt(Characters.Count - 1);
					line = line.Substring(0, line.Length - 1);

					continue;
				}

				line += code;
				Write(code.Character);
			}

			NewLine();
			return line;
		}

		public Key ReadKey(bool waitForKey = true)
		{
			if (!waitForKey)
				return Keyboard.GetKeyPressed();

			Key key;

			for (;;)
			{
				HAL.Pause();

				var code = Keyboard.GetKeyPressed();
				if (code == null)
					continue;

				key = code;
				break;
			}

			return key;
		}

		public void ToTop()
		{
			SetPosition(0, 0);
		}

		public void Clear()
		{
			Characters.Clear();
			ToTop();
		}
	}
}
