// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem.Fonts;
using Mosa.DeviceSystem.Graphics;

namespace Mosa.DeviceSystem.TextDevice;

/// <summary>
/// An interface used for interacting with a text device. It can clear the underlying buffer, write characters, set the cursor position,
/// scroll up, and retrieve the width and height.
/// See <see cref="GraphicalTextDevice"/> for an implementation that uses an <see cref="IGraphicsDevice"/> and an <see cref="ISimpleFont"/>
/// underneath.
/// </summary>
public interface ITextDevice
{
	uint Width { get; }

	uint Height { get; }

	void WriteChar(uint x, uint y, char c, Color foreground);

	void SetCursor(uint x, uint y);

	void Clear(Color background);

	void ScrollUp();
}
