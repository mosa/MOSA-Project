/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{

	public class Screen
	{
		protected ITextDevice textDevice;
		protected ushort cursorX;
		protected ushort cursorY;
		protected TextColor foreground;
		protected TextColor background;
		protected ushort width;
		protected ushort height;

		public Screen(ITextDevice textDevice)
		{
			this.textDevice = textDevice;
			width = textDevice.GetWidth();
			height = textDevice.GetHeight();
			foreground = TextColor.Black;
			background = TextColor.White;
			Clear();
		}

		protected void SetCursor()
		{
			textDevice.SetCursor(cursorX, cursorY);
		}

		public void Clear()
		{
			cursorX = 0;
			cursorY = 0;
			textDevice.Clear();
			SetCursor();
		}

		public void Write(string text)
		{
			foreach (char c in text) {
				textDevice.WriteChar(cursorX, cursorY, c, foreground, background);
				cursorX++;

				if (cursorX == width) {
					cursorY++;

					if (cursorY == height) {
						textDevice.ScrollUp();
						cursorY--;
					}

					cursorX = 0;
				}
			}
			SetCursor();
		}

		public void WriteLine()
		{
			cursorY++;

			if (cursorY == height) {
				textDevice.ScrollUp();
				cursorY--;
			}

			cursorX = 0;
		}

		public void WriteLine(string text)
		{
			Write(text);
			WriteLine();
		}
	}
}
