/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceSystem
{

	/// <summary>
	/// Implements a text screen for a <see cref="ITextDevice"/>.
	/// </summary>
	public class TextScreen : ITextScreen
	{
		/// <summary>
		/// 
		/// </summary>
		protected ITextDevice textDevice;
		/// <summary>
		/// 
		/// </summary>
		protected ushort cursorX;
		/// <summary>
		/// 
		/// </summary>
		protected ushort cursorY;
		/// <summary>
		/// 
		/// </summary>
		protected TextColor foreground;
		/// <summary>
		/// 
		/// </summary>
		protected TextColor background;
		/// <summary>
		/// 
		/// </summary>
		protected ushort width;
		/// <summary>
		/// 
		/// </summary>
		protected ushort height;

		/// <summary>
		/// Initializes a new instance of the <see cref="TextScreen"/> class.
		/// </summary>
		/// <param name="textDevice">The text device.</param>
		public TextScreen(ITextDevice textDevice)
		{
			this.textDevice = textDevice;
			width = textDevice.Width;
			height = textDevice.Height;
			foreground = TextColor.Black;
			background = TextColor.White;
			ClearScreen();
		}

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		protected void SetCursor()
		{
			textDevice.SetCursor(cursorX, cursorY);
		}

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		/// <param name="cursorX">The cursor X.</param>
		/// <param name="cursorY">The cursor Y.</param>
		public void SetCursor(ushort cursorX, ushort cursorY)
		{
			this.cursorX = cursorX;
			this.cursorY = cursorY;
			SetCursor();
		}

		/// <summary>
		/// Clears the screen.
		/// </summary>
		public void ClearScreen()
		{
			cursorX = 0;
			cursorY = 0;
			textDevice.ClearScreen();
			SetCursor();
		}

		/// <summary>
		/// Writes the specified character.
		/// </summary>
		/// <param name="character">The character.</param>
		protected void InternalWrite(char character)
		{
			// TODO: handle character = newline!

			textDevice.WriteChar(cursorX, cursorY, character, foreground, background);
			cursorX++;

			if (cursorX == width) {
				cursorY++;

				if (cursorY == height) {
					textDevice.ScrollUp();
					cursorY--;
				}

				cursorX = 0;
			}
			SetCursor();
		}

		/// <summary>
		/// Writes the specified text to the screen.
		/// </summary>
		/// <param name="text">The text.</param>
		public void Write(string text)
		{
			foreach (char c in text) 
				InternalWrite(c);
			
			SetCursor();
		}

		/// <summary>
		/// Writes the specified character.
		/// </summary>
		/// <param name="character"></param>
		public void Write(char character)
		{
			InternalWrite(character);
			SetCursor();
		}

		/// <summary>
		/// Writes an empty line to the screen.
		/// </summary>
		public void WriteLine()
		{
			cursorY++;

			if (cursorY == height) {
				textDevice.ScrollUp();
				cursorY--;
			}

			cursorX = 0;
			SetCursor();
		}

		/// <summary>
		/// Writes the line to the screen.
		/// </summary>
		/// <param name="text">The text.</param>
		public void WriteLine(string text)
		{
			Write(text);
			WriteLine();
		}
	}
}
