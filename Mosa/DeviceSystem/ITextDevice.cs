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
		Black,
		/// <summary>
		/// Blue
		/// </summary>
		Blue,
		/// <summary>
		/// Green
		/// </summary>
		Green,
		/// <summary>
		/// Cyan
		/// </summary>
		Cyan,
		/// <summary>
		/// Red
		/// </summary>
		Red,
		/// <summary>
		/// Magenta
		/// </summary>
		Magenta,
		/// <summary>
		/// Brown
		/// </summary>
		Brown,
		/// <summary>
		/// White
		/// </summary>
		White,
		/// <summary>
		/// DarkGray
		/// </summary>
		DarkGray,
		/// <summary>
		/// LightBlue
		/// </summary>
		LightBlue,
		/// <summary>
		/// LightGreen
		/// </summary>
		LightGreen,
		/// <summary>
		/// LightCyan
		/// </summary>
		LightCyan,
		/// <summary>
		/// LightRed
		/// </summary>
		LightRed,
		/// <summary>
		/// LightMagenta
		/// </summary>
		LightMagenta,
		/// <summary>
		/// Yellow
		/// </summary>
		Yellow,
		/// <summary>
		/// BrightWhite
		/// </summary>
		BrightWhite
	}

}
