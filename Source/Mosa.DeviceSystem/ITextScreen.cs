// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;

namespace Mosa.DeviceSystem;

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
	void SetCursor(uint cursorX, uint cursorY);

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

	/// <summary>
	/// Sets the background and foreground color.
	/// </summary>
	/// <param name="foreground">The foreground color.</param>
	/// <param name="background">The background color.</param>
	void SetColor(Color foreground, Color background);

	/// <summary>
	/// Reads a line.
	/// </summary>
	/// <returns>The read line.</returns>
	string ReadLine();
}
