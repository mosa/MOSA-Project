// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Basic;

public static class ArrayLayoutTests
{
	[MosaUnitTest]
	public static bool B_4()
	{
		var arr = new bool[] { true, false, true, false };
		return arr[0] && !arr[1] && arr[2] && !arr[3];
	}

	[MosaUnitTest]
	public static bool C_1()
	{
		var arr = new char[] { 'W' };
		return arr[0] == 'W';
	}

	[MosaUnitTest]
	public static bool C_2()
	{
		var arr = new char[] { 'W', 'X' };
		return arr[0] == 'W' && arr[1] == 'X';
	}

	[MosaUnitTest]
	public static bool C_3()
	{
		var arr = new char[] { 'W', 'X', 'Y' };
		return arr[0] == 'W' && arr[1] == 'X' && arr[2] == 'Y';
	}

	[MosaUnitTest]
	public static bool C_4()
	{
		var arr = new char[] { 'W', 'X', 'Y', 'Z' };
		return arr[0] == 'W' && arr[1] == 'X' && arr[2] == 'Y' && arr[3] == 'Z';
	}

	[MosaUnitTest]
	public static bool C_4a()
	{
		var arr = new char[] { 'W', 'X', 'Y', 'Z' };
		return arr[0] == 'W';
	}

	[MosaUnitTest]
	public static bool C_4z()
	{
		var arr = new char[] { 'W', 'X', 'Y', 'Z', '1' };
		return arr[0] == 'W' && arr[1] == 'X' && arr[2] == 'Y' && arr[3] == 'Z';
	}

	[MosaUnitTest]
	public static bool C_5()
	{
		var arr = new char[] { 'W', 'X', 'Y', 'Z', '1' };
		return arr[0] == 'W' && arr[1] == 'X' && arr[2] == 'Y' && arr[3] == 'Z' && arr[3] == '1';
	}

	//[MosaUnitTest]
	//public static int C_4ab()
	//{
	//	var arr = new char[] { 'W', 'X', 'Y', 'Z' };
	//	return arr[0];
	//}

	[MosaUnitTest]
	public static int C_4b()
	{
		var arr = new char[] { 'a', 'b', 'c', 'd' };
		return (byte)arr[0] + (byte)arr[1] + (byte)arr[2] + (byte)arr[3];
	}

	[MosaUnitTest]
	public static bool U1_4()
	{
		var arr = new byte[] { 0x55, 0x80, 0xaa, 0xff };
		return arr[0] == 0x55 && arr[1] == 0x80 && arr[2] == 0xaa && arr[3] == 0xff;
	}

	[MosaUnitTest]
	public static bool U2_4()
	{
		var arr = new ushort[] { 0x5555, 0x8080, 0xaaaa, 0xffff };
		return arr[0] == 0x5555 && arr[1] == 0x8080 && arr[2] == 0xaaaa && arr[3] == 0xffff;
	}

	[MosaUnitTest]
	public static bool U4_4()
	{
		var arr = new uint[] { 0x55555555, 0x80808080, 0xaaaaaaaa, 0xffffffff };
		return arr[0] == 0x55555555 && arr[1] == 0x80808080 && arr[2] == 0xaaaaaaaa && arr[3] == 0xffffffff;
	}

	[MosaUnitTest]
	public static bool U8_4()
	{
		var arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
		return arr[0] == 0x5555555555555555 && arr[1] == 0x8080808080808080 && arr[2] == 0xaaaaaaaaaaaaaaaa && arr[3] == 0xffffffffffffffff;
	}

	private static readonly ulong[] arrayU8 = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };

	[MosaUnitTest(0)]
	[MosaUnitTest(1)]
	[MosaUnitTest(2)]
	[MosaUnitTest(3)]
	public static ulong U8(int i)
	{
		return arrayU8[i];
	}

	[MosaUnitTest]
	public static ulong U8_4a()
	{
		var arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
		return arr[0];
	}

	[MosaUnitTest]
	public static ulong U8_4b()
	{
		var arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
		return arr[1];
	}

	[MosaUnitTest]
	public static ulong U8_4c()
	{
		var arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
		return arr[2];
	}

	[MosaUnitTest]
	public static ulong U8_4d()
	{
		var arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
		return arr[3];
	}

	[MosaUnitTest]
	public static bool I8_4()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa, 0x0fffffffffffffff };
		return arr[0] == 0x5555555555555555 && arr[1] == 0x0080808080808080 && arr[2] == 0x0aaaaaaaaaaaaaaa && arr[3] == 0x0fffffffffffffff;
	}

	[MosaUnitTest]
	public static bool I8_4a()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa, 0x0fffffffffffffff };
		return arr[0] == 0x5555555555555555;
	}

	[MosaUnitTest]
	public static bool I8_4b()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa, 0x0fffffffffffffff };
		return arr[1] == 0x0080808080808080;
	}

	[MosaUnitTest]
	public static bool I8_4c()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa, 0x0fffffffffffffff };
		return arr[2] == 0x0aaaaaaaaaaaaaaa;
	}

	[MosaUnitTest]
	public static bool I8_4d()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa, 0x0fffffffffffffff };
		return arr[3] == 0x0fffffffffffffff;
	}

	[MosaUnitTest]
	public static bool I8_3a()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa };
		return arr[0] == 0x5555555555555555;
	}

	[MosaUnitTest]
	public static bool I8_3b()
	{
		var arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa };
		return arr[2] == 0x0aaaaaaaaaaaaaaa;
	}

	[MosaUnitTest]
	public static bool R4_1()
	{
		var arr = new float[] { 1.234f };
		return arr[0] == 1.234f;
	}

	[MosaUnitTest]
	public static bool R4_3()
	{
		var arr = new float[] { 1.234f, 5.678f, 9.012f };
		return arr[0] == 1.234f && arr[1] == 5.678f && arr[2] == 9.012f;
	}

	[MosaUnitTest]
	public static bool R4_4()
	{
		var arr = new float[] { 1.234f, 5.678f, 9.012f, 3.456f };
		return arr[0] == 1.234f && arr[1] == 5.678f && arr[2] == 9.012f && arr[3] == 3.456f;
	}

	[MosaUnitTest]
	public static bool R4_5()
	{
		var arr = new float[] { 1.234f, 5.678f, 9.012f, 3.456f, 8.888f };
		return arr[0] == 1.234f && arr[1] == 5.678f && arr[2] == 9.012f && arr[3] == 3.456f && arr[4] == 8.888f;
	}

	[MosaUnitTest]
	public static bool R8_4()
	{
		var arr = new double[] { 1.23456, 7.89012, 3.45678, 9.01234 };
		return arr[0] == 1.23456 && arr[1] == 7.89012 && arr[2] == 3.45678 && arr[3] == 9.01234;
	}
}
