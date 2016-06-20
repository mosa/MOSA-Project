// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTest.Collection
{
	public static class ListTests
	{
		public static bool Create()
		{
			var list = new List<int>();

			return list != null;
		}

		public static bool EmptySize()
		{
			var list = new List<int>();

			return list.Count == 0;
		}

		public static bool Add1()
		{
			var list = new List<int>();

			list.Add(101);

			return list.Count == 1;
		}

		public static bool Add2()
		{
			var list = new List<int>();

			list.Add(101);
			list.Add(202);

			return list.Count == 2;
		}

		public static bool Index1()
		{
			var list = new List<int>();

			list.Add(101);

			return list[0] == 101;
		}

		public static bool Index2()
		{
			var list = new List<int>();

			list.Add(101);
			list.Add(202);

			return list[1] == 202;
		}

		public static bool IndexOf1()
		{
			var list = new List<int>();

			list.Add(101);
			list.Add(202);

			return list.IndexOf(101) == 1;
		}

		public static bool IndexOf2()
		{
			var list = new List<int>();

			list.Add(101);
			list.Add(202);

			return list.IndexOf(202) == 2;
		}

		public static bool Remove1()
		{
			var list = new List<int>();

			list.Add(101);
			list.Add(202);
			list.Remove(101);

			return list.IndexOf(202) == 0;
		}

		public static bool Remove2()
		{
			var list = new List<int>();

			list.Add(101);
			list.Add(202);
			list.Remove(202);

			return list.IndexOf(101) == 0;
		}

		private static List<int> Populate(int count)
		{
			var list = new List<int>();

			for (int i = 1; i < count; i++)
			{
				list.Add(101 * i);
			}

			return list;
		}

		public static bool PopulateList()
		{
			var list = Populate(9);

			return list.Count == 9;
		}

		private static int SumAll(List<int> list)
		{
			int sum = 0;

			foreach (var item in list)
			{
				sum = sum + item;
			}

			return sum;
		}

		public static int Foreach()
		{
			var list = Populate(10);

			return SumAll(list);
		}

		public static int ForeachNested()
		{
			var list = Populate(3);
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

		public static int ForeachBreak()
		{
			List<Holder2> holderList = new List<Holder2>();
			for (int i = 1; i < 10; i++)
			{
				var p = new Holder2(10 * i, 20 * i, 30 * i);
				holderList.Add(p);
			}

			var found = FindHolder(holderList);
			return (found.value1 + found.value2 + found.value3);
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
			public int value1;
			public int value2;
			public int value3;

			public Holder2(int v1, int v2, int v3)
			{
				value1 = v1;
				value2 = v2;
				value3 = v3;
			}
		}
	}
}
