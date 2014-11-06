/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Test.Collection
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
			LinkedList<Holder> IntList = new LinkedList<Holder>();

			for (int i = 1; i < 10; i++)
			{
				Holder h = new Holder();
				h.value1 = 101 * i;
				h.value2 = 101 * i;
				h.value3 = 101 * i;
				IntList.AddLast(h);
			}

			int sum = 0;

			foreach (var item in IntList)
			{
				sum += item.value1;
				sum = sum + item.value2;
				sum = sum + item.value3;
				if (sum > 5000)
					break;
			}

			return sum;
		}

		private struct Holder
		{
			public int value1;
			public int value2;
			public int value3;
		}
	}
}