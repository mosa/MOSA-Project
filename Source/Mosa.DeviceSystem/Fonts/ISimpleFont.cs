// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Devices.Graphics;

namespace Mosa.DeviceSystem.Fonts;

/// <summary>
/// An interface used for implementing simple fonts. Those can be fixed, like <see cref="ASC16Font"/>, or dynamic, like the
/// <see cref="SimpleBitFont"/> format. It provides access to the name of the font, its fixed width/height/size (if any),
/// and the charset it supports. It then allows drawing characters using the DrawChar() and DrawString() functions.
/// To reliably calculate the total width of a character or string, or the height of a character, respectively use the
/// CalculateWidth() and CalculateHeight() functions.
/// </summary>
public interface ISimpleFont
{
	string Name { get; }

	uint Width { get; }

	uint Height { get; }

	uint Size { get; }

	string Charset { get; }

	void DrawChar(FrameBuffer32 frameBuffer, uint color, uint x, uint y, char c);

	void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text);

	uint CalculateWidth(char c);

	uint CalculateWidth(string s);

	uint CalculateHeight(char c);
}
