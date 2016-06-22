// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	//public static class GenericMixedTest
	//{
	//	class Tuple<X, Y>
	//	{
	//		public X I1;
	//		public Y I2;

	//		// HACK: Workaround of value type not working
	//		string ret(X x, Y y)
	//		{
	//			return "[" + x.ToString() + ", " + y.ToString() + "]";
	//		}

	//		public override string ToString()
	//		{
	//			return ret(I1, I2);
	//		}
	//	}

	//	class TupleArray<X, Y> : Tuple<X, Y[]>
	//	{
	//		// HACK: Workaround of value type not working
	//		string ret2(Y y)
	//		{
	//			return y.ToString();
	//		}

	//		string ret(X x, Y[] y)
	//		{
	//			string result = "[" + x.ToString();
	//			for (int i = 0; i < I2.Length; i++)
	//			{
	//				result += ", " + ret2(I2[i]);
	//			}
	//			return result + "]";
	//		}

	//		public override string ToString()
	//		{
	//			return ret(I1, I2);
	//		}
	//	}

	//	class List<X>
	//	{
	//		object[] list = new object[10];

	//		public void Put<Y>(int index, X i1, Y i2)
	//		{
	//			list[index] = new Tuple<X, Y>() { I1 = i1, I2 = i2 };
	//		}

	//		public void PutArray<Y>(int index, X i1, Y[] i2)
	//		{
	//			list[index] = new TupleArray<X, Y>() { I1 = i1, I2 = i2 };
	//		}

	//		public X GetOne<Y>(int index)
	//		{
	//			return ((Tuple<X, Y>)list[index]).I1;
	//		}

	//		public Y GetTwo<Y>(int index)
	//		{
	//			return ((Tuple<X, Y>)list[index]).I2;
	//		}

	//		public string ToString(int index)
	//		{
	//			return list[index].ToString();
	//		}
	//	}

	//	public static uint GenericMixed()
	//	{
	//		List<int> list = new List<int>();
	//		list.Put<string>(0, 1, "MOSA Generics");
	//		list.Put<byte>(2, 3, 0xff);
	//		list.Put<object>(4, 0x12345678, new object());
	//		uint[] x = new uint[4];
	//		x[0] = 0x12345678;
	//		x[3] = 0x87654321;
	//		list.PutArray<uint>(8, 11, x);

	//		uint testResult = 0;

	//		if (list.GetOne<string>(0) == 1)
	//			testResult |= 0x1;
	//		if (list.GetTwo<string>(0) == "MOSA Generics")
	//			testResult |= 0x2;

	//		if (list.GetOne<byte>(2) == 3)
	//			testResult |= 0x4;
	//		if (list.GetTwo<byte>(2) == 0xff)
	//			testResult |= 0x8;

	//		if (list.GetOne<object>(4) == 0x12345678)
	//			testResult |= 0x10;
	//		if (list.GetTwo<object>(4).GetType().FullName == "System.Object")
	//			testResult |= 0x20;

	//		uint[] q = list.GetTwo<uint[]>(8);
	//		if (list.GetOne<object>(8) == 11)
	//			testResult |= 0x40;
	//		if (q.GetType().FullName == "System.UInt32[]")
	//			testResult |= 0x80;
	//		if (q.Length == 4)
	//			testResult |= 0x100;
	//		if (q[0] == 0x12345678)
	//			testResult |= 0x200;
	//		if (q[3] == 0x87654321)
	//			testResult |= 0x400;

	//		if (list.ToString(0) == "[1, MOSA Generics]")
	//			testResult |= 0x800;
	//		if (list.ToString(2) == "[3, 255]")
	//			testResult |= 0x1000;
	//		if (list.ToString(4) == "[305419896, System.Object]")
	//			testResult |= 0x2000;
	//		if (list.ToString(8) == "[11, 305419896, 0, 0, 2271560481]")
	//			testResult |= 0x4000;

	//		return testResult;
	//	}
	//}
}
