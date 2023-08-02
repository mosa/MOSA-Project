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

	[MosaUnitTest]
	public static bool SubstringTest1()
	{
		return "abc".Substring(1).Equals("bc");
	}

	[MosaUnitTest]
	public static bool SubstringTest2()
	{
		return "abcdef".Substring(3).Equals("def");
	}

	[MosaUnitTest]
	public static bool SubstringTest3() {
		return "abcdef".Substring(0, 3).Equals("abc");
	}

	[MosaUnitTest]
	public static bool SubstringTest4()
	{
		return "abcdef".Substring(2, 1).Equals("c");
	}

	[MosaUnitTest]
	public static bool InsertTest1()
	{
		return "aaa".Insert(0, "bbb").Equals("bbbaaa");
	}

	[MosaUnitTest]
	public static bool InsertTest2()
	{
		return "aaa".Insert(2, "bbb").Equals("aabbba");
	}

	[MosaUnitTest]
	public static bool RemoveTest1()
	{
		return "aaabbb".Remove(0, 3).Equals("bbb");
	}

	[MosaUnitTest]
	public static bool RemoveTest2()
	{
		return "aabbba".Remove(2, 3).Equals("aaa");
	}

	[MosaUnitTest]
	public static bool IsNullOrWhiteSpaceTest1()
	{
		return string.IsNullOrWhiteSpace("");
	}

	[MosaUnitTest]
	public static bool IsNullOrWhiteSpaceTest2()
	{
		return string.IsNullOrWhiteSpace(" ");
	}

	[MosaUnitTest]
	public static bool IsNullOrWhiteSpaceTest3()
	{
		// ReSharper disable once ConditionIsAlwaysTrueOrFalse
		return string.IsNullOrWhiteSpace(null);
	}

	[MosaUnitTest]
	public static bool IsNullOrWhiteSpaceTest4()
	{
		return !string.IsNullOrWhiteSpace("test");
	}
}
