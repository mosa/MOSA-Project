// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Devices.Graphics;

namespace Mosa.DeviceSystem.Fonts;

/// <summary>
/// An implementation of the <see cref="ISimpleFont"/> interface using the dynamic BitFont format.
/// </summary>
public class SimpleBitFont : ISimpleFont
{
	public string Name { get; }

	public uint Width { get; }

	public uint Height { get; }

	public uint Size { get; }

	public string Charset { get; }

	private uint maxX;

	private readonly byte[] buffer;

	public SimpleBitFont(string name, uint width, uint height, uint size, string charset, byte[] data)
	{
		Name = name;
		Width = width;
		Height = height;
		Size = size;
		Charset = charset;

		maxX = 0;

		buffer = data;
	}

	public void DrawChar(FrameBuffer32 frameBuffer, uint color, uint x, uint y, char c)
	{
		maxX = 0;

		var index = Charset.IndexOf(c);
		var size8 = Size / 8;
		var sizePerFont = Size * size8 * index;

		if (index < 0)
			return;

		for (var h = 0; h < Size; h++)
			for (var aw = 0; aw < size8; aw++)
				for (var ww = 0; ww < 8; ww++)
					if ((buffer[sizePerFont + h * size8 + aw] & (0x80 >> ww)) != 0)
					{
						var max = aw * 8 + ww;

						frameBuffer.SetPixel(color, (uint)(x + max), (uint)(y + h));

						if (max > maxX)
							maxX = (uint)max;
					}
	}

	public void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text)
	{
		uint usedX = 0;

		for (var i = 0; i < text.Length; i++)
		{
			DrawChar(frameBuffer, color, usedX + x, y, text[i]);
			usedX += maxX + 2;
		}
	}

	public uint CalculateWidth(char c)
	{
		var size8 = Size / 8;
		var index = Charset.IndexOf(c);

		if (index < 0)
			return Width;

		var maximumX = 0U;
		var sizePerFont = Size * size8 * index;

		for (var h = 0; h < Size; h++)
			for (var aw = 0; aw < size8; aw++)
				for (var ww = 0; ww < 8; ww++)
					if ((buffer[sizePerFont + h * size8 + aw] & (0x80 >> ww)) != 0)
					{
						var max = aw * 8 + ww;

						if (max > maximumX)
							maximumX = (uint)max;
					}

		return maximumX;
	}

	public uint CalculateWidth(string s)
	{
		var r = 0U;

		for (var i = 0; i < s.Length; i++)
			r += CalculateWidth(s[i]) + 2;

		return r;
	}

	public uint CalculateHeight(char c) => Height;
}
