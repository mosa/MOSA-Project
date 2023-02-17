// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.Generics;

public static class LinkedListTests
{
	[MosaUnitTest]
	public static bool Create()
	{
		var IntList = new LinkedList<int>();

		return IntList != null;
	}

	[MosaUnitTest]
	public static bool EmptySize()
	{
		var IntList = new LinkedList<int>();

		return IntList.Count == 0;
	}

	[MosaUnitTest]
	public static bool Size1()
	{
		var IntList = new LinkedList<int>();

		IntList.AddLast(101);

		return IntList.Count == 1;
	}

	[MosaUnitTest]
	public static bool Size2()
	{
		var IntList = new LinkedList<int>();

		IntList.AddLast(101);
		IntList.AddLast(202);

		return IntList.Count == 2;
	}

	[MosaUnitTest]
	public static bool First1()
	{
		var IntList = new LinkedList<int>();

		IntList.AddLast(101);

		return IntList.First.Value == 101;
	}

	[MosaUnitTest]
	public static bool First2()
	{
		var IntList = new LinkedList<int>();

		IntList.AddLast(101);
		IntList.AddLast(202);

		return IntList.First.Value == 101;
	}

	[MosaUnitTest]
	public static bool Last1()
	{
		var IntList = new LinkedList<int>();

		IntList.AddLast(101);

		return IntList.Last.Value == 101;
	}

	[MosaUnitTest]
	public static bool Last2()
	{
		var IntList = new LinkedList<int>();

		IntList.AddLast(101);
		IntList.AddLast(202);

		return IntList.Last.Value == 202;
	}

	private static LinkedList<int> Populate()
	{
		var IntList = new LinkedList<int>();

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

	[MosaUnitTest]
	public static bool PopulateList()
	{
		var list = Populate();

		return list.Count == 9;
	}

	[MosaUnitTest]
	public static int Foreach()
	{
		var list = Populate();
		int sum = 0;

		foreach (var item in list)
		{
			sum += item;
		}

		return sum;
	}

	[MosaUnitTest]
	public static int ForeachNested()
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

		return sum;
	}

	[MosaUnitTest]
	public static int ForeachNested2()
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

		return sum + nestedSum;
	}

	[MosaUnitTest]
	public static int ForeachBreak()
	{
		var holderList = new LinkedList<Holder>();
		for (int i = 1; i < 10; i++)
		{
			var p = new Holder(10 * i, 20 * i, 30 * i);
			holderList.AddLast(p);
		}

		var found = FindHolder(holderList);
		return found.value1 + found.value2 + found.value3;
	}

	private static Holder FindHolder(LinkedList<Holder> holderList)
	{
		foreach (var p in holderList)
		{
			if (p.value1 == 90 && p.value2 == 180 && p.value3 == 270)
				return p;
		}
		return new Holder(0, 0, 0);
	}

	private struct Holder
	{
		public readonly int value1;
		public readonly int value2;
		public readonly int value3;

		public Holder(int v1, int v2, int v3)
		{
			value1 = v1;
			value2 = v2;
			value3 = v3;
		}
	}
}
