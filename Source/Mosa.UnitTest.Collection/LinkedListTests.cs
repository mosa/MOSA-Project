// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTest.Collection
{
	public static class LinkedListTests
	{
		public static bool Create()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			return IntList != null;
		}

		public static bool EmptySize()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			return IntList.Count == 0;
		}

		public static bool Size1()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.AddLast(101);

			return IntList.Count == 1;
		}

		public static bool Size2()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.AddLast(101);
			IntList.AddLast(202);

			return IntList.Count == 2;
		}

		public static bool First1()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.AddLast(101);

			return IntList.First.Value == 101;
		}

		public static bool First2()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.AddLast(101);
			IntList.AddLast(202);

			return IntList.First.Value == 101;
		}

		public static bool Last1()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.AddLast(101);

			return IntList.Last.Value == 101;
		}

		public static bool Last2()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.AddLast(101);
			IntList.AddLast(202);

			return IntList.Last.Value == 202;
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

		public static bool PopulateList()
		{
			var list = Populate();

			return list.Count == 9;
		}

		public static int Foreach()
		{
			var list = Populate();
			int sum = 0;

			foreach (var item in list)
			{
				sum = sum + item;
			}

			return sum;
		}

		public static int ForeachNested()
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

			return sum;
		}

		public static int ForeachNested2()
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

			return sum + nestedSum;
		}

		public static int ForeachBreak()
		{
			LinkedList<Holder> holderList = new LinkedList<Holder>();
			for (int i = 1; i < 10; i++)
			{
				var p = new Holder(10 * i, 20 * i, 30 * i);
				holderList.AddLast(p);
			}

			var found = FindHolder(holderList);
			return (found.value1 + found.value2 + found.value3);
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
			public int value1;
			public int value2;
			public int value3;

			public Holder(int v1, int v2, int v3)
			{
				value1 = v1;
				value2 = v2;
				value3 = v3;
			}
		}
	}
}
