/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
 */

using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mosa.TestWorld.x86.Tests
{
	public class OtherTest : KernelTest
	{
		public OtherTest()
			: base("Other")
		{
			testMethods.AddLast(OtherTest1);
			testMethods.AddLast(OtherTest2);
			testMethods.AddLast(OtherTest3);
			testMethods.AddLast(OtherTest4);
			testMethods.AddLast(OtherTest5);
			testMethods.AddLast(ForeachNestedTest);
			testMethods.AddLast(ForeachNested2Test);
			testMethods.AddLast(StructNewObjTest);
			testMethods.AddLast(StructNotBoxed);
			testMethods.AddLast(ForeachBreak);
			testMethods.AddLast(ConditionalBug);
			testMethods.AddLast(PointerBug);
		}

		private static uint StaticValue = 0x200000;

		public static bool OtherTest1()
		{
			uint x = StaticValue;

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
				sum = sum + item;
				foreach (var nestedItem in nestedList)
				{
					nestedSum = nestedSum + nestedItem;
					nestedSum = nestedSum / ++nestedCount;
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
				sum = sum + item;

				foreach (var nested in list)
				{
					sum = sum + nested;
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

			return (t1.A == 5 && t1.B == 10);
		}

		public static bool StructNotBoxed()
		{
			NotBoxedStruct s = new NotBoxedStruct();
			s.ToString();
			s.ToString();
			s.ToString();
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
			return (found.A == 90 && found.B == 180);
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
			uint address = 0x1000;
			Mosa.Platform.Internal.x86.Native.Set8(address, 81);
			var num = Mosa.Platform.Internal.x86.Native.Get8(address);

			if (num >= 32 && num < 128)
				return true;
			else
				return false;
		}

		public static bool PointerBug()
		{
			return PointerBugClass.Test();
		}

		unsafe public static class PointerBugClass
		{
			private static uint pageDirectoryAddress = 0x1000;
			private static PageDirectoryEntry* pageDirectoryEntries;

			public static bool Test()
			{
				pageDirectoryEntries = (PageDirectoryEntry*)pageDirectoryAddress;
				return GetItem(1);
			}

			internal static bool GetItem(uint index)
			{
				//increpmenting a pointer
				var addr1 = (uint)(pageDirectoryEntries + index);

				//increpmenting a UInt32
				var addr2 = (uint)(pageDirectoryAddress + (index * 4));  //struct PageDirectoryEntry as a size of 4 bytes

				return addr1 == addr2;
			}

			[StructLayout(LayoutKind.Explicit)]
			unsafe public struct PageDirectoryEntry
			{
				[FieldOffset(0)]
				private uint data;
			}
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
}