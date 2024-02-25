// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Drawing;
using Mosa.DeviceSystem.Fonts;
using Mosa.DeviceSystem.Graphics;

namespace Mosa.DeviceSystem.TextDevice;

/// <summary>
/// An implementation of the <see cref="ITextDevice"/> interface using an <see cref="IGraphicsDevice"/> and an <see cref="ISimpleFont"/>.
/// </summary>
public class GraphicalTextDevice : ITextDevice
{
	public uint Width { get; }

	public uint Height { get; }

	public ISimpleFont Font { get; set; }

	public IGraphicsDevice GraphicsDevice { get; }

	public GraphicalTextDevice(ISimpleFont font, IGraphicsDevice graphicsDevice)
	{
		Width = graphicsDevice.FrameBuffer.Width;
		Height = graphicsDevice.FrameBuffer.Height;
		Font = font;
		GraphicsDevice = graphicsDevice;
	}

	public void Clear(Color background)
	{
		GraphicsDevice.FrameBuffer.ClearScreen((uint)background.ToArgb());
		GraphicsDevice.Update(0, 0, Width, Height);
	}

	public void WriteChar(uint x, uint y, char c, Color foreground)
	{
		var charWidth = Font.CalculateWidth(c);
		var charHeight = Font.CalculateHeight(c);
		var charX = x * charWidth;
		var charY = y * charHeight;

		Font.DrawChar(GraphicsDevice.FrameBuffer, (uint)foreground.ToArgb(), charX, charY, c);
		GraphicsDevice.Update(charX, charY, charWidth, charHeight);
	}

	public void ScrollUp()
	{
		throw new NotImplementedException();
	}

	public void SetCursor(uint x, uint y)
	{ }
}
