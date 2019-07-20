// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal
{
	public static class Console
	{
		public static void Write(char c)
		{
			Platform.ConsoleWrite(c);
		}

		public static void Write(string s)
		{
			foreach (var c in s)
			{
				Write(c);
			}
		}

		public static void WriteLine(string s)
		{
			Write(s);
			Write('\n');
		}
	}
}
