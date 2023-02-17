// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Primitive;

public static class StringTests
{
	public static string valueA = "Foo";
	public static string valueB = "Something";

	[MosaUnitTest]
	public static int CheckLength()
	{
		return valueA.Length;
	}

	[MosaUnitTest]
	public static char CheckFirstCharacter()
	{
		return valueA[0];
	}

	[MosaUnitTest]
	public static char CheckLastCharacter()
	{
		return valueA[valueA.Length - 1];
	}

	[MosaUnitTest]
	public static char LastCharacterMustMatch()
	{
		var ch = '\0';

		for (var index = 0; index < valueB.Length; index++)
		{
			ch = valueB[index];
		}

		return ch;
	}

	[MosaUnitTest]
	public static bool ConcatTest1()
	{
		var part1 = "abc";
		var part2 = "def";
		var ab = "abcdef";
		var combined = string.Concat(part1, part2);

		return string.Equals(combined, ab);
	}

	[MosaUnitTest]
	public static bool ConcatTest2()
	{
		var part1 = "abc";
		var part2 = "def";
		var part3 = "ghi";
		var abc = "abcdefghi";
		var combined = string.Concat(part1, part2, part3);

		return string.Equals(combined, abc);
	}

	[MosaUnitTest]
	public static bool Equal1()
	{
		var a = "abc";
		var b = "abc";

		return string.Equals(a, b);
	}

	[MosaUnitTest]
	public static bool NotEqual1()
	{
		var a = "abc";
		var b = "abd";

		return string.Equals(a, b);
	}
}
