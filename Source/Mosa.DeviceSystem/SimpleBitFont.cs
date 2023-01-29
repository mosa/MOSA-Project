// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// BitFont (dynamic fonts)
/// </summary>
public class SimpleBitFont : ISimpleFont
{
	public string Name { get; }

	public uint Width { get; }

	public uint Height { get; }

	public uint Size { get; }

	private uint _maxX;

	private readonly string Charset;

	private readonly byte[] Buffer;

	public SimpleBitFont(string name, uint width, uint height, uint size, string charset, byte[] data)
	{
		Name = name;

		Width = width;
		Height = height;
		Size = size;

		Charset = charset;

		Buffer = data;

		_maxX = 0;
	}

	public void DrawChar(FrameBuffer32 frameBuffer, uint color, uint x, uint y, char c)
	{
		_maxX = 0;

		var index = Charset.IndexOf(c);
		var size8 = Size / 8;
		var sizePerFont = Size * size8 * index;

		if (index < 0)
			return;

		for (var h = 0; h < Size; h++)
		for (var aw = 0; aw < size8; aw++)
		for (var ww = 0; ww < 8; ww++)
			if ((Buffer[sizePerFont + h * size8 + aw] & (0x80 >> ww)) != 0)
			{
				var max = aw * 8 + ww;

				frameBuffer.SetPixel(color, (uint)(x + max), (uint)(y + h));

				if (max > _maxX)
					_maxX = (uint)max;
			}
	}

	public void DrawString(FrameBuffer32 frameBuffer, uint color, uint x, uint y, string text)
	{
		uint usedX = 0;

		for (var i = 0; i < text.Length; i++)
		{
			DrawChar(frameBuffer, color, usedX + x, y, text[i]);
			usedX += _maxX + 2;
		}
	}

	public uint CalculateWidth(char c)
	{
		var size8 = Size / 8;
		var index = Charset.IndexOf(c);

		if (index < 0)
			return Width;

		uint maxX = 0;
		var sizePerFont = Size * size8 * index;

		for (var h = 0; h < Size; h++)
		for (var aw = 0; aw < size8; aw++)
		for (var ww = 0; ww < 8; ww++)
			if ((Buffer[sizePerFont + h * size8 + aw] & (0x80 >> ww)) != 0)
			{
				var max = aw * 8 + ww;

				if (max > maxX)
					maxX = (uint)max;
			}

		return maxX;
	}

	public uint CalculateWidth(string s)
	{
		uint r = 0;
		for (var i = 0; i < s.Length; i++)
			r += CalculateWidth(s[i]) + 2;

		return r;
	}

	public uint CalculateHeight(char c)
	{
		return Height;
	}
}
