// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a text screen
	/// </summary>
	public interface ITextScreen
	{
		/// <summary>
		/// Clears the screen.
		/// </summary>
		void ClearScreen();

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		/// <param name="cursorX">The cursor X.</param>
		/// <param name="cursorY">The cursor Y.</param>
		void SetCursor(ushort cursorX, ushort cursorY);

		/// <summary>
		/// Writes the specified text to the screen.
		/// </summary>
		/// <param name="text">The text.</param>
		void Write(string text);

		/// <summary>
		/// Writes the specified character to the screen.
		/// </summary>
		/// <param name="character">The character.</param>
		void Write(char character);

		/// <summary>
		/// Writes an empty line to the screen.
		/// </summary>
		void WriteLine();

		/// <summary>
		/// Writes the line to the screen.
		/// </summary>
		/// <param name="text">The text.</param>
		void WriteLine(string text);
	}
}
