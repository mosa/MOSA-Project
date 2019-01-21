// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mosa.TestWorld.x86.Tests
{
	public class OtherTest : KernelTest
	{
		public OtherTest()
			: base("Other")
		{
			testMethods.Add(BoxTestEqualsI4);
			testMethods.Add(OtherTest1);
			testMethods.Add(OtherTest2);
			testMethods.Add(OtherTest3);
			testMethods.Add(OtherTest4);
			testMethods.Add(OtherTest5);
			testMethods.Add(ForeachNestedTest);
			testMethods.Add(ForeachNested2Test);
			testMethods.Add(StructNewObjTest);
			testMethods.Add(StructNotBoxed);
			testMethods.Add(ForeachBreak);
			testMethods.Add(ConditionalBug);
			testMethods.Add(PointerBug);
			testMethods.Add(AddressOfThisBug);
			testMethods.Add(RemR4);
			testMethods.Add(RemR8);
			testMethods.Add(NullableTest1);
			testMethods.Add(NullableTest2);
			testMethods.Add(NullableTest3);
			testMethods.Add(NullableTest4);
			testMethods.Add(NullableTest5);
			testMethods.Add(ForeachU1Test);
			testMethods.Add(MethodScanner);
		}

		private const uint StaticValue = 0x200000;

		public static bool OtherTest1()
		{
			const uint x = StaticValue;

			return x == 0x200000;
		}

		public static bool OtherTest2()
		{
			return StaticValue == 0x200000;
		}

		public static bool OtherTest3()
		{
			return 3.Equals(3);
		}

		public static bool OtherTest4()
		{
			return 3.Equals((object)3);
		}

		public static bool OtherTest5()
		{
			return ((object)3).Equals(3);
		}

		public static bool ForeachNestedTest()
		{
			var list = Populate();
			var nestedList = Populate();
			int sum = 0;
			int nestedSum = 0;
			int nestedCount = 0;

			foreach (var item in list)
			{
				sum += item;
				foreach (var nestedItem in nestedList)
				{
					nestedSum += nestedItem;
					nestedSum /= ++nestedCount;
				}
			}

			return (sum + nestedSum) == 4556;
		}

		public static bool ForeachNested2Test()
		{
			var list = Populate2();
			int sum = 0;

			foreach (var item in list)
			{
				sum += item;

				foreach (var nested in list)
				{
					sum += nested;
				}
			}

			return sum == 1200;
		}

		private static LinkedList<int> Populate()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			for (int i = 1; i < 10; i++)
			{
				IntList.AddLast(101 * i);
			}

			return IntList;
		}

		private static LinkedList<int> Populate2()
		{
			var IntList = new LinkedList<int>();

			IntList.AddLast(100);
			IntList.AddLast(300);

			return IntList;
		}

		public static bool StructNewObjTest()
		{
			Pair t1 = default(Pair);
			try { t1 = new Pair(5, 10); }
			catch { }

			return t1.A == 5 && t1.B == 10;
		}

		public static bool StructNotBoxed()
		{
			NotBoxedStruct s = new NotBoxedStruct();

			s.ToString();
			s.ToString();
			s.ToString();
			return s.I == 3;
		}

		public static bool TestField()
		{
			TestClassAA s = new TestClassAA();

			return s.I == 3;
		}

		public static bool ForeachBreak()
		{
			LinkedList<Pair> PairList = new LinkedList<Pair>();
			for (int i = 1; i < 10; i++)
			{
				Pair p = new Pair(10 * i, 20 * i);
				PairList.AddLast(p);
			}

			Pair found = FindPair(PairList);
			return found.A == 90 && found.B == 180;
		}

		private static Pair FindPair(LinkedList<Pair> PairList)
		{
			foreach (Pair p in PairList)
			{
				if (p.A == 90 && p.B == 180)
					return p;
			}
			return new Pair(0, 0);
		}

		public static bool ConditionalBug()
		{
			var address = new IntPtr(0x3000);
			Intrinsic.Store8(address, 81);
			var num = Intrinsic.Load8(address);

			if (num >= 32 && num < 128)
				return true;
			else
				return false;
		}

		public static bool PointerBug()
		{
			return PointerBugClass.Test();
		}

		public static bool AddressOfThisBug()
		{
			return PointerBugClass.Test2();
		}

		private const float f1 = 1.232145E+10f;
		private const float f2 = 2f;

		public static bool RemR4()
		{
			return (f1 % f2) == 0f;
		}

		private const double d1 = 1.232145E+10d;
		private const double d2 = 15d;

		public static bool RemR8()
		{
			return (d1 % d2) == 0d;
		}

		public static bool NullableTest1()
		{
			bool? v1 = true;

			return v1 == true;
		}

		public static bool NullableTest2()
		{
			bool? v1 = null;

			return v1 == null;
		}

		public static bool NullableTest3()
		{
			bool? v1 = null;

			return !(v1 == true);
		}

		public static bool NullableTest4()
		{
			bool? v1 = null;
			bool? v2 = true;

			return !(v1 == v2);
		}

		public static bool NullableTest5()
		{
			int? v1 = null;
			int? v2 = 32;

			long? v3 = v1 ?? v2;

			return v3 == 32;
		}

		public static bool NullableTest6()
		{
			bool? v1 = null;
			bool? v2 = true;

			return !Nullable.Equals(v1, v2);
		}

		public static byte ForeachU1()
		{
			byte[] a = new byte[5];
			for (int i = 0; i < 5; i++)
				a[i] = (byte)i;

			byte total = 0;

			foreach (byte v in a)
				total += v;

			return total;
		}

		public static bool ForeachU1Test()
		{
			return ForeachU1() == 10;
		}

		unsafe public static class PointerBugClass
		{
			private const uint pageDirectoryAddress = 0x1000;
			private static PageDirectoryEntry* pageDirectoryEntries;

			public static bool Test()
			{
				pageDirectoryEntries = (PageDirectoryEntry*)pageDirectoryAddress;
				return GetItem(1);
			}

			public static bool Test2()
			{
				return (uint)pageDirectoryEntries == pageDirectoryEntries->AddressOfThis;
			}

			internal static bool GetItem(uint index)
			{
				//incrementing a pointer
				var addr1 = (uint)(pageDirectoryEntries + index);

				//incrementing a UInt32
				var addr2 = pageDirectoryAddress + (index * 4);  //struct PageDirectoryEntry as a size of 4 bytes

				return addr1 == addr2;
			}

			[StructLayout(LayoutKind.Explicit)]
			unsafe public struct PageDirectoryEntry
			{
				[FieldOffset(0)]
				private readonly uint data;

				public uint AddressOfThis
				{
					get
					{
						fixed (PageDirectoryEntry* ptr = &this)
							return (uint)ptr;
					}
				}
			}
		}

		public static bool BoxTestEqualsI4()
		{
			return UnitTests.BoxingTests.EqualsI4(10);
		}

		public static bool MethodScanner()
		{
			var shape = new VeryOddBox();

			var i = shape.GetArea();

			return true;
		}
	}

	public struct TestStruct
	{
		public byte One;
	}

	public struct NotBoxedStruct
	{
		public int I;

		public override string ToString()
		{
			I++;
			return "";
		}
	}

	public struct Pair
	{
		public int A;
		public int B;

		public Pair(int a, int b)
		{
			A = a;
			B = b;
		}
	}

	public struct TestClassAA
	{
		public int I;
	}

	public abstract class Shape
	{
		public abstract int GetArea();

		public int GetPerimeter()
		{
			return 0;
		}
	}

	public class Box : Shape
	{
		public override int GetArea()
		{
			return 10;
		}
	}

	public class Circle : Shape
	{
		public override int GetArea()
		{
			return 10;
		}
	}

	public class Rectangle : Box
	{
		public override int GetArea()
		{
			return 20;
		}
	}

	public class OddBox : Rectangle
	{
		public override int GetArea()
		{
			return 30;
		}
	}

	public class VeryOddBox : OddBox
	{
	}
}
