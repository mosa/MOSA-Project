using Mosa.Platforms.x86;
using Mosa.Kernel;
using Mosa.Kernel.X86;
using System;

namespace Mosa.HelloWorld.Tests
{
	public static class StringTest
	{
		public static void Test()
		{
			Screen.SetCursor(23, 0);
			Screen.Write("TEST: ");

			string part1 = "abc";
			string part2 = "def";
			string part3 = "ghi";

			string ab = "abcdef";
			string abc = "abcdefghi";

			string combine1 = string.Concat(part1, part2);
			string combine2 = string.Concat(part1, part2, part3);
			
			if (!String.Equals(combine1, ab))
			{
				Screen.Write("#1 ");
				Screen.Write(combine1);
				Screen.Write("|");
				Screen.Write(ab);
				Screen.Write("| ");
			}

			if (!String.Equals(combine2, abc))
				Screen.Write("#2 ");

			Screen.Write("Done");
		}
	}
}
