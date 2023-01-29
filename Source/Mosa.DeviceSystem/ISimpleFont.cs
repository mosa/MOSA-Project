// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
///
/// </summary>
public interface ISimpleFont
{
	/// <summary>
	/// Gets the name.
	/// </summary>
	string Name { get; }

	/// <summary>
	/// Gets the size.
	/// </summary>
	uint Size { get; }

	/// <summary>
	/// Gets the width.
	/// </summary>
	uint Width { get; }

	/// <summary>
	/// Gets the height.
	/// </summary>
	uint Height { get; }

	/// <summary>
	/// Draws the character.
	/// </summary>
	void DrawChar(FrameBuffer32 frameBuffer, uint color, uint x, uint y, char c);

	/// <summary>
	/// Draws the string.
	/// </summary>
	void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text);

	/// <summary>
	/// Calculates the width of a character.
	/// </summary>
	uint CalculateWidth(char c);

	/// <summary>
	/// Calculates the width of a string.
	/// </summary>
	uint CalculateWidth(string s);

	/// <summary>
	/// Calculates the height of a character.
	/// </summary>
	uint CalculateHeight(char c);
}
