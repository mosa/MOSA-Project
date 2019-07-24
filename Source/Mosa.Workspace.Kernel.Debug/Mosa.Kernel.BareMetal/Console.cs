// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal
{
	public static class Console
	{
		public const byte Escape = 0x1b;
		public const byte Newline = 0x0A;
		public const byte Formfeed = 0x0C;
		public const byte Backspace = 0x08;

		public static void Write(byte c)
		{
			Platform.ConsoleWrite(c);
		}

		public static void Write(char c)
		{
			Write((byte)c);
		}

		public static void Write(string s)
		{
			foreach (var c in s)
			{
				Write((byte)c);
			}
		}

		public static void WriteLine(string s)
		{
			Write(s);
			Write(Newline);
		}

		public static void WriteLine()
		{
			Write(Newline);
		}

		public static void ClearScreen()
		{
			Write(Escape);
			Write("[2J]");
		}

		public static void SetForground(ConsoleColor color)
		{
			Write(Escape);
			Write("[3");
			Write((byte)((byte)(color) % 10));
		}

		public static void SetBackground(ConsoleColor color)
		{
			Write(Escape);
			Write("[4");
			Write((byte)((byte)(color) % 10));
		}
	}
}
