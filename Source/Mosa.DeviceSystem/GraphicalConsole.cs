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

			Characters = new List<Character>();

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
			Characters.Add(new Character(c, X, Y));
			LastCharWidth = Font.CalculateWidth(c);
			Next(LastCharWidth);
		}

		public void Write(string s)
		{
			for (var i = 0; i < s.Length; i++)
				switch (s[i])
				{
					case '\n':
						NewLine();
						break;

					case '\r':
						X = 0;
						break;

					default:
						Write(s[i]);
						break;
				}
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

		// TODO: Use actual KeyTypes
		public string ReadLine()
		{
			var code = ReadKey(false);

			if (code != null)
				switch (code.KeyType)
				{
					case KeyType.RegularKey:
						if ((byte)code.Character == 10) // Enter key
						{
							NewLine();

							var l = Line;
							Line = string.Empty;
							return l;
						}
						else if ((byte)code.Character == 8) // Backspace key
						{
							if (Line.Length > 0)
							{
								Previous();
								Characters.RemoveAt(Characters.Count - 1);
								Line = Line.Substring(0, Line.Length - 1);
							}
						}
						else
						{
							Line += code.Character;
							Write(code.Character);
						}

						break;

					default:
						break;
				}

			return string.Empty;
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
