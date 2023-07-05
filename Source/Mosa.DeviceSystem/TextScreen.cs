// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;

namespace Mosa.DeviceSystem;

/// <summary>
/// Implements a text screen for a <see cref="ITextDevice"/>.
/// </summary>
public class TextScreen : ITextScreen
{
	private readonly ITextDevice textDevice;
	private readonly IKeyboard keyboard;

	private readonly char[] buffer = new char[2048];

	private Color background, foreground;

	private uint cursorX, cursorY;

	public TextScreen(ITextDevice textDevice, IKeyboard keyboard)
	{
		this.textDevice = textDevice;
		this.keyboard = keyboard;

		SetColor(Color.White, Color.Black);
		ClearScreen();
	}

	public void SetCursor(uint x, uint y)
	{
		cursorX = x;
		cursorY = y;
		textDevice.SetCursor(cursorX, cursorY);
	}

	public void ClearScreen()
	{
		SetCursor(0, 0);
		textDevice.ClearScreen(background);
	}

	public void Write(string text)
	{
		foreach (var c in text)
			Write(c);
	}

	public void Write(char character)
	{
		InternalWrite(character);
		textDevice.SetCursor(cursorX, cursorY);
	}

	public void WriteLine()
	{
		Write('\n');
	}

	public void WriteLine(string text)
	{
		Write(text);
		WriteLine();
	}

	public void SetColor(Color foreground, Color background)
	{
		this.foreground = foreground;
		this.background = background;
	}

	public string ReadLine()
	{
		var length = 0;

		for (; ; )
		{
			HAL.Yield();

			var key = keyboard.GetKeyPressed();
			if (key == null)
				continue;

			switch (key.Character)
			{
				// Enter key
				case '\n':
					NewLine();
					textDevice.SetCursor(cursorX, cursorY);
					return new string(buffer, 0, length);

				// Backspace key
				case '\b':
					if (length > 0)
					{
						Previous();
						InternalWrite(' ', false);
						textDevice.SetCursor(cursorX, cursorY);
						length--;
					}
					break;

				// Any other key
				default:
					buffer[length++] = key.Character;
					Write(key.Character);
					break;
			}
		}
	}

	private void InternalWrite(char character, bool increaseX = true)
	{
		if (cursorX == textDevice.Width || character == '\n')
		{
			NewLine();
			textDevice.SetCursor(cursorX, cursorY);
			return;
		}

		switch (character)
		{
			case '\r':
				{
					cursorX = 0;
					break;
				}
			case '\t':
				{
					cursorX += 4;
					break;
				}
			default:
				{
					textDevice.WriteChar(cursorX, cursorY, character, foreground);
					if (increaseX)
						cursorX++;
					break;
				}
		}

		textDevice.SetCursor(cursorX, cursorY);
	}

	private void NewLine()
	{
		cursorY++;
		cursorX = 0;

		if (cursorY == textDevice.Height)
		{
			textDevice.ScrollUp();
			cursorY--;
		}
	}

	private void Previous()
	{
		if (cursorX == 0 && cursorY == 0)
			return;

		if (cursorX == 0)
		{
			cursorY--;
			cursorX = textDevice.Width;
		}
		else cursorX--;
	}
}
