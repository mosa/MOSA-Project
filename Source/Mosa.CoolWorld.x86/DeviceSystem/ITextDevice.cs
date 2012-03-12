/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Interface to a text device
	/// </summary>
	public interface ITextDevice
	{
		/// <summary>
		/// Writes the char.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="c">The c.</param>
		/// <param name="foreground">The foreground.</param>
		/// <param name="background">The background.</param>
		void WriteChar(ushort x, ushort y, char c, TextColor foreground, TextColor background);

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		void SetCursor(ushort x, ushort y);

		/// <summary>
		/// Clears the screen.
		/// </summary>
		void ClearScreen();

		/// <summary>
		/// Scrolls up.
		/// </summary>
		void ScrollUp();

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		byte Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		byte Height { get; }
	}

	/// <summary>
	/// Text Colors
	/// </summary>
	public enum TextColor : byte
	{
		/// <summary>
		/// Black
		/// </summary>
		Black = 0,
		/// <summary>
		/// Blue
		/// </summary>
		Blue = 1,
		/// <summary>
		/// Green
		/// </summary>
		Green = 2,
		/// <summary>
		/// Cyan
		/// </summary>
		Cyan = 3,
		/// <summary>
		/// Red
		/// </summary>
		Red = 4,
		/// <summary>
		/// Magenta
		/// </summary>
		Magenta = 5,
		/// <summary>
		/// Brown
		/// </summary>
		Brown = 6,
		/// <summary>
		/// White
		/// </summary>
		White = 7,
		/// <summary>
		/// DarkGray
		/// </summary>
		DarkGray = 8,
		/// <summary>
		/// LightBlue
		/// </summary>
		LightBlue = 9,
		/// <summary>
		/// LightGreen
		/// </summary>
		LightGreen = 10,
		/// <summary>
		/// LightCyan
		/// </summary>
		LightCyan = 11,
		/// <summary>
		/// LightRed
		/// </summary>
		LightRed = 12,
		/// <summary>
		/// LightMagenta
		/// </summary>
		LightMagenta = 13,
		/// <summary>
		/// Yellow
		/// </summary>
		Yellow = 14,
		/// <summary>
		/// BrightWhite
		/// </summary>
		BrightWhite = 15
	}

}
