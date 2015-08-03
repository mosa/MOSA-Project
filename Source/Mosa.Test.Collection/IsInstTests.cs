// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Test.Collection
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
		public static bool IsInstAAToAA()
		{
			object o = new AA();

			return (o is AA);
		}

		public static bool IsInstBBToAA()
		{
			object o = new BB();

			return (o is AA);
		}

		public static bool IsInstCCToAA()
		{
			object o = new CC();

			return (o is AA);
		}

		public static bool IsInstCCToBB()
		{
			object o = new CC();

			return (o is BB);
		}

		public static bool IsInstDDToAA()
		{
			object o = new DD();

			return (o is AA);
		}

		public static bool IsInstDDToBB()
		{
			object o = new DD();

			return (o is BB);
		}

		public static bool IsInstDDToCC()
		{
			object o = new DD();

			return (o is CC);
		}

		public static bool IsInstAAtoIAA()
		{
			object o = new AA();

			return (o is IAA);
		}

		public static bool IsInstBBToIAA()
		{
			object o = new BB();

			return (o is IAA);
		}

		public static bool IsInstCCToIAA()
		{
			object o = new CC();

			return (o is IAA);
		}

		public static bool IsInstCCToIBB()
		{
			object o = new CC();

			return (o is IBB);
		}

		//public static bool IsInstI4ToI4()
		//{
		//	object o = (int)1;
		//	return (o is int);
		//}

		public static bool IsInstI4ToI4(int i)
		{
			object o = i;
			return (o is int);
		}

		public static bool IsInstU4ToI4()
		{
			object o = (uint)1;
			return (o is uint);
		}

		public static bool IsInstI8ToI8()
		{
			object o = (long)1;
			return (o is long);
		}

		public static bool IsInstU8ToU8()
		{
			object o = (ulong)1;
			return (o is ulong);
		}

		public static bool IsInstI4ToU4()
		{
			object o = (int)1;
			return (o is uint);
		}

		public static bool IsInstI1ToI1()
		{
			object o = (sbyte)1;
			return (o is sbyte);
		}

		public static bool IsInstI2ToI2()
		{
			object o = (short)1;
			return (o is short);
		}

		public static bool IsInstU1ToU1()
		{
			object o = (byte)1;
			return (o is byte);
		}

		public static bool IsInstU2ToU2()
		{
			object o = (ushort)1;
			return (o is ushort);
		}

		public static bool IsInstCToC()
		{
			object o = (char)'A';
			return (o is char);
		}

		public static bool IsInstBToB(bool b)
		{
			object o = (bool)b;
			return (o is bool);
		}
	}
}