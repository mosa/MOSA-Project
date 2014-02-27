/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.HelloWorld.x86.Tests
{
	public class Generic2Test : KernelTest
	{
		public Generic2Test()
			: base("Gen-N")
		{
			testMethods.Add(GenericTest1);
			testMethods.Add(GenericTest2);
			testMethods.Add(GenericTest3);
			testMethods.Add(GenericTest4);
			testMethods.Add(GenericTest5);
			testMethods.Add(GenericTest6);
		}

		public static bool GenericTest1()
		{
			var node = new LinkedList<uint>.LinkedListNode<uint>(10, null, null);
			return node.value == 10;
		}

		public static bool GenericTest2()
		{
			var node1 = new LinkedList<uint>.LinkedListNode<uint>(10, null, null);
			var node2 = new LinkedList<uint>.LinkedListNode<uint>(20, node1, null);
			var node3 = new LinkedList<uint>.LinkedListNode<uint>(30, node2, null);

			node1.next = node2;
			node2.next = node3;

			return node1.next.next.value == 30;
		}

		public static bool GenericTest3()
		{
			var list = new LinkedList<int>();

			list.Add(10);
			list.Add(20);

			return list.First.value == 10;
		}

		public static bool GenericTest4()
		{
			var list = new LinkedList<int>();

			list.Add(10);
			list.Add(20);
			list.Add(30);

			return list.First.value == 10 && list.Last.value == 30 && list.Find(20).value == 20;
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
	}
}