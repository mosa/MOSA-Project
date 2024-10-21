// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests.Korlib;

public static class ListTests
{
	[MosaUnitTest]
	public static bool Create()
	{
		var list = new List<int>();

		return list != null;
	}

	[MosaUnitTest]
	public static bool EmptySize()
	{
		var list = new List<int>();

		return list.Count == 0;
	}

	[MosaUnitTest]
	public static bool Add1()
	{
		var list = new List<int>();

		list.Add(101);

		return list[0] == 101 && list.Count == 1;
	}

	[MosaUnitTest]
	public static bool Add2()
	{
		var list = new List<int>();

		list.Add(101);
		list.Add(202);

		return list[0] == 101 && list[1] == 202 && list.Count == 2;
	}

	[MosaUnitTest]
	public static bool Add4()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		return list[0] == 0 && list[1] == 1 && list[2] == 2 && list[3] == 3 && list[4] == 4 && list[5] == 5;
	}

	[MosaUnitTest]
	public static int Check0()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		return list[0];
	}

	[MosaUnitTest]
	public static int Check1()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		return list[1];
	}

	[MosaUnitTest]
	public static int Check2()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		return list[2];
	}

	[MosaUnitTest]
	public static int Check4()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		return list[4];
	}

	[MosaUnitTest]
	public static bool Index1()
	{
		var list = new List<int>();

		list.Add(101);

		return list[0] == 101;
	}

	[MosaUnitTest]
	public static bool Index2()
	{
		var list = new List<int>();

		list.Add(101);
		list.Add(202);

		return list[1] == 202;
	}

	[MosaUnitTest]
	public static bool IndexOf1()
	{
		var list = new List<int>();

		list.Add(101);
		list.Add(202);

		return list.IndexOf(101) == 1;
	}

	[MosaUnitTest]
	public static bool IndexOf2()
	{
		var list = new List<int>();

		list.Add(101);
		list.Add(202);

		return list.IndexOf(202) == 2;
	}

	[MosaUnitTest]
	public static bool Remove1()
	{
		var list = new List<int>();

		list.Add(101);
		list.Add(202);
		list.Remove(101);

		return list.IndexOf(202) == 0;
	}

	[MosaUnitTest]
	public static bool Remove2()
	{
		var list = new List<int>();

		list.Add(101);
		list.Add(202);
		list.Remove(202);

		return list.IndexOf(101) == 0;
	}

	[MosaUnitTest]
	public static bool RemoveAt0()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.RemoveAt(0);

		return list[0] == 1 && list[1] == 2 && list[2] == 3 && list[3] == 4;
	}

	[MosaUnitTest]
	public static bool RemoveAt1()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.RemoveAt(1);

		return list[0] == 0 && list[1] == 2 && list[2] == 3 && list[3] == 3;
	}

	[MosaUnitTest]
	public static bool RemoveAt3()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.RemoveAt(4);

		return list[0] == 0 && list[1] == 1 && list[2] == 2 && list[3] == 4;
	}

	[MosaUnitTest]
	public static bool RemoveAt4()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.RemoveAt(4);

		return list[0] == 0 && list[1] == 1 && list[2] == 2 && list[3] == 3;
	}

	[MosaUnitTest]
	public static bool Insert0()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.Insert(0, 99);

		return list[0] == 99 && list[1] == 0 && list[2] == 1 && list[3] == 2 && list[4] == 3 && list[5] == 4;
	}

	[MosaUnitTest]
	public static bool Insert1()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.Insert(1, 99);

		return list[0] == 0 && list[1] == 99 && list[2] == 1 && list[3] == 2 && list[4] == 3 && list[5] == 4;
	}

	[MosaUnitTest]
	public static bool Insert2()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.Insert(2, 99);

		return list[0] == 0 && list[1] == 1 && list[2] == 99 && list[3] == 2 && list[4] == 3 && list[5] == 4;
	}

	[MosaUnitTest]
	public static bool Insert3()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.Insert(3, 99);

		return list[0] == 0 && list[1] == 1 && list[2] == 2 && list[3] == 99 && list[4] == 3 && list[5] == 4;
	}

	[MosaUnitTest]
	public static bool Insert4()
	{
		var list = new List<int>();

		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(4);

		list.Insert(4, 99);

		return list[0] == 0 && list[1] == 1 && list[2] == 2 && list[3] == 3 && list[4] == 99 && list[5] == 4;
	}

	private static List<int> Populate(int count)
	{
		var list = new List<int>();

		for (var i = 0; i < count; i++)
		{
			list.Add(101 * i);
		}

		return list;
	}

	[MosaUnitTest]
	public static bool PopulateList()
	{
		var list = Populate(6);

		return list.Count == 6;
	}

	private static int SumAll(List<int> list)
	{
		var sum = 0;

		foreach (var item in list)
		{
			sum = sum + item;
		}

		return sum;
	}

	[MosaUnitTest]
	public static bool CheckPopulate()
	{
		var size = 50;

		var list = Populate(size);

		for (var i = 0; i < size; i++)
		{
			if (list[i] != 101 * i)
				return false;
		}

		return true;
	}

	[MosaUnitTest]
	public static int Foreach()
	{
		var list = Populate(10);

		return SumAll(list);
	}

	[MosaUnitTest]
	public static int ForeachNested()
	{
		var list = Populate(9);
		var sum = 0;

		foreach (var item in list)
		{
			sum = sum + item;

			foreach (var nested in list)
			{
				sum = sum + nested;
			}
		}

		return sum;
	}

	[MosaUnitTest]
	public static int ForeachBreak()
	{
		var holderList = new List<Holder2>();

		for (var i = 1; i < 10; i++)
		{
			var p = new Holder2(10 * i, 20 * i, 30 * i);
			holderList.Add(p);
		}

		var found = FindHolder(holderList);
		return found.value1 + found.value2 + found.value3;
	}

	private static Holder2 FindHolder(List<Holder2> holderList)
	{
		foreach (var p in holderList)
		{
			if (p.value1 == 90 && p.value2 == 180 && p.value3 == 270)
				return p;
		}
		return new Holder2(0, 0, 0);
	}

	private struct Holder2
	{
		public readonly int value1;
		public readonly int value2;
		public readonly int value3;

		public Holder2(int v1, int v2, int v3)
		{
			value1 = v1;
			value2 = v2;
			value3 = v3;
		}
	}
}
