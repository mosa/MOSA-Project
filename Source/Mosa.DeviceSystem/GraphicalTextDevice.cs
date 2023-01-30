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

	public FrameBuffer32 FrameBuffer { get; }

	#endregion

	public GraphicalTextDevice(uint width, uint height, ISimpleFont font, FrameBuffer32 frameBuffer)
	{
		Width = width;
		Height = height;
		Font = font;
		FrameBuffer = frameBuffer;
	}

	public void ClearScreen(Color background)
	{
		FrameBuffer.ClearScreen((uint)background.ToArgb());
	}

	public void WriteChar(uint x, uint y, char c, Color foreground)
	{
		Font.DrawChar(FrameBuffer, (uint)foreground.ToArgb(), x * Font.CalculateWidth(c), y * Font.CalculateHeight(c), c);
	}

	public void ScrollUp()
	{
		throw new NotImplementedException();
	}

	public void SetCursor(uint x, uint y) { }
}
