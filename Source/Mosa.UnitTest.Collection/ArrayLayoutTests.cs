// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class ArrayLayoutTests
	{
		[MosaUnitTest]
		public static bool B()
		{
			bool[] arr = new bool[] { true, false, true, false };
			return arr[0] && !arr[1] && arr[2] && !arr[3];
		}

		[MosaUnitTest]
		public static bool C1()
		{
			char[] arr = new char[] { 'W' };
			return arr[0] == 'W';
		}

		[MosaUnitTest]
		public static bool C()
		{
			char[] arr = new char[] { 'W', 'X', 'Y', 'Z' };
			return arr[0] == 'W' && arr[1] == 'X' && arr[2] == 'Y' && arr[3] == 'Z';
		}

		[MosaUnitTest]
		public static bool U1()
		{
			byte[] arr = new byte[] { 0x55, 0x80, 0xaa, 0xff };
			return arr[0] == 0x55 && arr[1] == 0x80 && arr[2] == 0xaa && arr[3] == 0xff;
		}

		[MosaUnitTest]
		public static bool U2()
		{
			ushort[] arr = new ushort[] { 0x5555, 0x8080, 0xaaaa, 0xffff };
			return arr[0] == 0x5555 && arr[1] == 0x8080 && arr[2] == 0xaaaa && arr[3] == 0xffff;
		}

		[MosaUnitTest]
		public static bool U4()
		{
			uint[] arr = new uint[] { 0x55555555, 0x80808080, 0xaaaaaaaa, 0xffffffff };
			return arr[0] == 0x55555555 && arr[1] == 0x80808080 && arr[2] == 0xaaaaaaaa && arr[3] == 0xffffffff;
		}

		[MosaUnitTest]
		public static bool U8()
		{
			ulong[] arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
			return arr[0] == 0x5555555555555555 && arr[1] == 0x8080808080808080 && arr[2] == 0xaaaaaaaaaaaaaaaa && arr[3] == 0xffffffffffffffff;
		}

		[MosaUnitTest]
		public static ulong U8a()
		{
			ulong[] arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
			return arr[0];
		}

		[MosaUnitTest]
		public static ulong U8b()
		{
			ulong[] arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
			return arr[1];
		}

		[MosaUnitTest]
		public static ulong U8c()
		{
			ulong[] arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
			return arr[2];
		}

		[MosaUnitTest]
		public static ulong U8d()
		{
			ulong[] arr = new ulong[] { 0x5555555555555555, 0x8080808080808080, 0xaaaaaaaaaaaaaaaa, 0xffffffffffffffff };
			return arr[3];
		}

		[MosaUnitTest]
		public static bool I8()
		{
			long[] arr = new long[] { 0x5555555555555555, 0x0080808080808080, 0x0aaaaaaaaaaaaaaa, 0x0fffffffffffffff };
			return arr[0] == 0x5555555555555555 && arr[1] == 0x0080808080808080 && arr[2] == 0x0aaaaaaaaaaaaaaa && arr[3] == 0x0fffffffffffffff;
		}

		[MosaUnitTest]
		public static bool R4()
		{
			float[] arr = new float[] { 1.234f, 5.678f, 9.012f, 3.456f };
			return arr[0] == 1.234f && arr[1] == 5.678f && arr[2] == 9.012f && arr[3] == 3.456f;
		}

		[MosaUnitTest]
		public static bool R8()
		{
			double[] arr = new double[] { 1.23456, 7.89012, 3.45678, 9.01234 };
			return arr[0] == 1.23456 && arr[1] == 7.89012 && arr[2] == 3.45678 && arr[3] == 9.01234;
		}
	}
}
