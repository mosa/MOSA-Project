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

			IntList.Add(101);

			return IntList.Count == 1;
		}

		public static bool Size2()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.Add(101);
			IntList.Add(202);

			return IntList.Count == 2;
		}

		public static bool First1()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.Add(101);

			return IntList.First == 101;
		}

		public static bool First2()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.Add(101);
			IntList.Add(202);

			return IntList.First == 101;
		}

		public static bool Last1()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.Add(101);

			return IntList.Last == 101;
		}

		public static bool Last2()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			IntList.Add(101);
			IntList.Add(202);

			return IntList.Last == 202;
		}

		private static LinkedList<int> Populate()
		{
			LinkedList<int> IntList = new LinkedList<int>();

			for (int i = 1; i < 10; i++)
			{
				IntList.Add(101 * i);
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
	}
}