/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;

namespace Mosa.TestWorld.x86.Tests
{
	public class Generic2Test : KernelTest
	{
		public Generic2Test()
			: base("Generic-N")
		{
			testMethods.Add(GenericTest1);
			testMethods.Add(GenericTest2);
			testMethods.Add(GenericTest3);
			testMethods.Add(GenericTest4);
			testMethods.Add(GenericTest5);
			testMethods.Add(GenericTest6);
			testMethods.Add(GenericTest7);
		}

		public static bool GenericTest1()
		{
			var node = new LinkedListNode<uint>(10);
			return node.Value == 10;
		}

		public static bool GenericTest2()
		{
			var list = new LinkedList<uint>();
			var node1 = new LinkedListNode<uint>(10);
			var node2 = new LinkedListNode<uint>(20);
			var node3 = new LinkedListNode<uint>(30);

			list.AddLast(node1);
			list.AddLast(node2);
			list.AddLast(node3);

			return node1.Next.Next.Value == 30;
		}

		public static bool GenericTest3()
		{
			var list = new LinkedList<int>();

			list.Add(10);
			list.Add(20);

			return list.First.Value == 10;
		}

		public static bool GenericTest4()
		{
			var list = new LinkedList<int>();

			list.Add(10);
			list.Add(20);
			list.Add(30);

			return list.First.Value == 10 && list.Last.Value == 30 && list.Find(20).Value == 20;
		}

		public static bool GenericTest5()
		{
			var list = new LinkedList<int>();

			list.Add(30);
			list.Add(10);
			list.Add(30);
			list.Add(20);
			list.Add(30);

			return list.FindLast(30) == list.Last;
		}

		public static bool GenericTest6()
		{
			var list = new LinkedList<int>();

			list.Add(30);
			list.Add(10);
			list.Add(30);
			list.Add(20);
			list.Add(30);

			return list.FindLast(30) != list.First;
		}

		public static bool GenericTest7()
		{
			LinkedList<IntClass> list = new LinkedList<IntClass>();

			IntClass value1 = new IntClass(9);
			IntClass value2 = new IntClass(2);

			list.Add(value1);
			list.Add(value2);

			IntClass first = list.First.Value;

			return first.value == 9;
		}
	}

	public class IntClass
	{
		public int value;

		public IntClass(int value)
		{
			this.value = value;
		}
	}
}