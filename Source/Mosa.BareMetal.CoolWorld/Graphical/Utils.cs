// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Drawing;
using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem.Fonts;
using Mosa.DeviceSystem.Misc;

namespace Mosa.BareMetal.CoolWorld.Graphical;

public static class Utils
{
	public static StandardMouse Mouse;

	public static List<ISimpleFont> Fonts;

	public static Color BackColor;

	public static Color Invert(Color color)
	{
		return Color.FromArgb(color.A, (byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B));
	}

	public static SimpleBitFont Load(byte[] data)
	{
		var block = new DataBlock(data);
		var position = 0U;

		var name = string.Empty;
		var charset = string.Empty;

		// Name
		for (; ; )
		{
			var ch = block.GetChar(position++);
			if (ch == byte.MaxValue)
				break;

			name += ch;
		}

		// Charset
		for (; ; )
		{
			var ch = block.GetChar(position++);
			if (ch == byte.MaxValue)
				break;

			charset += ch;
		}

		var width = block.GetByte(position++);
		var height = block.GetByte(position++);
		var size = block.GetByte(position++);

		position++; // Skip end byte

		var fontData = block.GetBytes(position, block.Length - position);

		return new SimpleBitFont(name, width, height, size, charset, fontData);
	}
}
