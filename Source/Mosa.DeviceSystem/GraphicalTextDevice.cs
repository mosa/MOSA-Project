// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Drawing;

namespace Mosa.DeviceSystem;

public class GraphicalTextDevice : ITextDevice
{
	#region Definitions

	public uint Width { get; }

	public uint Height { get; }

	public ISimpleFont Font { get; set; }

	public IGraphicsDevice GraphicsDevice { get; }

	#endregion

	public GraphicalTextDevice(ISimpleFont font, IGraphicsDevice graphicsDevice)
	{
		Width = graphicsDevice.FrameBuffer.Width;
		Height = graphicsDevice.FrameBuffer.Height;
		Font = font;
		GraphicsDevice = graphicsDevice;
	}

	public void ClearScreen(Color background)
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

	public void SetCursor(uint x, uint y) { }
}
