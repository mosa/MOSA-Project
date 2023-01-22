// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.UnitTests;

namespace Mosa.UnitTests.Basic
{
	public interface IAA { }

	public interface IBB { }

	public interface ICC { }

	public class AA : IAA { }

	public class BB : AA, IBB { }

	public class CC { }

	public class DD : BB { }

	public static class IsInstTests
	{
		[MosaUnitTest]
		public static bool IsInstAAToAA()
		{
			object o = new AA();

			return (o is AA);
		}

		[MosaUnitTest]
		public static bool IsInstBBToAA()
		{
			object o = new BB();

			return (o is AA);
		}

		[MosaUnitTest]
		public static bool IsInstCCToAA()
		{
			object o = new CC();

			return (o is AA);
		}

		[MosaUnitTest]
		public static bool IsInstCCToBB()
		{
			object o = new CC();

			return (o is BB);
		}

		[MosaUnitTest]
		public static bool IsInstDDToAA()
		{
			object o = new DD();

			return (o is AA);
		}

		[MosaUnitTest]
		public static bool IsInstDDToBB()
		{
			object o = new DD();

			return (o is BB);
		}

		[MosaUnitTest]
		public static bool IsInstDDToCC()
		{
			object o = new DD();

			return (o is CC);
		}

		[MosaUnitTest]
		public static bool IsInstAAtoIAA()
		{
			object o = new AA();

			return (o is IAA);
		}

		[MosaUnitTest]
		public static bool IsInstBBToIAA()
		{
			object o = new BB();

			return (o is IAA);
		}

		[MosaUnitTest]
		public static bool IsInstCCToIAA()
		{
			object o = new CC();

			return (o is IAA);
		}

		[MosaUnitTest]
		public static bool IsInstCCToIBB()
		{
			object o = new CC();

			return (o is IBB);
		}

		[MosaUnitTest(Series = "I4")]
		public static bool IsInstI4ToI4(int i)
		{
			object o = i;
			return (o is int);
		}

		[MosaUnitTest]
		public static bool IsInstU4ToU4()
		{
			object o = (uint)1;
			return (o is uint);
		}

		[MosaUnitTest]
		public static bool IsInstI4ToU4()
		{
			object o = 1;
			return (o is uint);
		}

		[MosaUnitTest]
		public static bool IsInstU4ToI4()
		{
			object o = (uint)1;
			return (o is int);
		}

		[MosaUnitTest]
		public static bool IsInstI8ToI8()
		{
			object o = (long)1;
			return (o is long);
		}

		[MosaUnitTest]
		public static bool IsInstU8ToU8()
		{
			object o = (ulong)1;
			return (o is ulong);
		}

		[MosaUnitTest]
		public static bool IsInstI1ToI1()
		{
			object o = (sbyte)1;
			return (o is sbyte);
		}

		[MosaUnitTest]
		public static bool IsInstI2ToI2()
		{
			object o = (short)1;
			return (o is short);
		}

		[MosaUnitTest]
		public static bool IsInstU1ToU1()
		{
			object o = (byte)1;
			return (o is byte);
		}

		[MosaUnitTest]
		public static bool IsInstU2ToU2()
		{
			object o = (ushort)1;
			return (o is ushort);
		}

		[MosaUnitTest]
		public static bool IsInstCToC()
		{
			object o = 'A';
			return (o is char);
		}

		[MosaUnitTest(Series = "B")]
		public static bool IsInstBToB(bool b)
		{
			object o = b;
			return (o is bool);
		}
	}
}
