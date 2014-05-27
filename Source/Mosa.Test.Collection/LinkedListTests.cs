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
	}
}