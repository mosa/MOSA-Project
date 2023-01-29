// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;

namespace Mosa.DeviceSystem;

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
	void WriteChar(uint x, uint y, char c, Color foreground);

	/// <summary>
	/// Sets the cursor.
	/// </summary>
	/// <param name="x">The x.</param>
	/// <param name="y">The y.</param>
	void SetCursor(uint x, uint y);

	/// <summary>
	/// Clears the screen.
	/// </summary>
	void ClearScreen(Color background);

	/// <summary>
	/// Scrolls up.
	/// </summary>
	void ScrollUp();

	/// <summary>
	/// Gets the width.
	/// </summary>
	/// <value>The width.</value>
	uint Width { get; }

	/// <summary>
	/// Gets the height.
	/// </summary>
	/// <value>The height.</value>
	uint Height { get; }
}
