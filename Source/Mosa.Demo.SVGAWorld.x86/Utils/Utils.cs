// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;
using System.Collections.Generic;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86
{
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
			var stream = new MemoryStream(data);

			var name = string.Empty;
			var charset = string.Empty;

			// Name
			for (;;)
			{
				var ch = stream.ReadByte();
				if (ch == byte.MaxValue)
					break;

				name += (char)ch;
			}

			// Charset
			for (;;)
			{
				var ch = stream.ReadByte();
				if (ch == byte.MaxValue)
					break;

				charset += (char)ch;
			}

			var width = stream.ReadByte();
			var height = stream.ReadByte();
			var size = stream.ReadByte();

			_ = stream.ReadByte(); // Skip end byte

			var fontData = stream.ReadEnd();

			return new SimpleBitFont(name, width, height, size, charset, fontData);
		}
	}
}
