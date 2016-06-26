// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.UnitTest.Collection
{
	public static class StringTests
	{
		public static string valueA = "Foo";
		public static string valueB = "Something";

		public static int CheckLength()
		{
			return valueA.Length;
		}

		public static char CheckFirstCharacter()
		{
			return valueA[0];
		}

		public static char CheckLastCharacter()
		{
			return valueA[valueA.Length - 1];
		}

		public static char LastCharacterMustMatch()
		{
			char ch = '\0';

			for (int index = 0; index < valueB.Length; index++)
			{
				ch = valueB[index];
			}

			return ch;
		}

		public static bool ConcatTest1()
		{
			string part1 = "abc";
			string part2 = "def";
			string ab = "abcdef";
			string combined = string.Concat(part1, part2);

			return string.Equals(combined, ab);
		}

		public static bool ConcatTest2()
		{
			string part1 = "abc";
			string part2 = "def";
			string part3 = "ghi";
			string abc = "abcdefghi";
			string combined = string.Concat(part1, part2, part3);

			return string.Equals(combined, abc);
		}

		public static bool Equal1()
		{
			string a = "abc";
			string b = "abc";

			return string.Equals(a, b);
		}

		public static bool NotEqual1()
		{
			string a = "abc";
			string b = "abd";

			return string.Equals(a, b);
		}
	}
}
