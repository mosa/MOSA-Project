// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests.TinyCoreLib;

public static class LinkedListTests
{
	// == Constructor tests

	[MosaUnitTest]
	public static bool Test_Constructor_Default()
	{
		var list = new LinkedList<int>();
		return list.Count == 0 && list.First == null && list.Last == null;
	}

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static bool Test_Constructor_FromCollection(int val1, int val2, int val3, int val4, int val5)
	{
		var source = new int[] { val1, val2, val3, val4, val5 };
		var list = new LinkedList<int>(source);
		return list.Count == 5 && list.First!.Value == val1 && list.Last!.Value == val5;
	}

	// == AddFirst tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_AddFirst_SingleItem(int value)
	{
		var list = new LinkedList<int>();
		list.AddFirst(value);
		return list.Count == 1 && ReferenceEquals(list.First, list.Last) && list.First!.Value == value && list.Last!.Value == value;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_AddFirst_MultipleItems(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddFirst(val1);
		list.AddFirst(val2);
		list.AddFirst(val3);
		return list.Count == 3 && list.First!.Value == val3 && list.Last!.Value == val1;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_AddFirst_Node(int value)
	{
		var list = new LinkedList<int>();
		var node = new LinkedListNode<int>(value);
		list.AddFirst(node);
		return list.Count == 1 && list.First!.Value == value && ReferenceEquals(node, list.First);
	}

	// == AddLast tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_AddLast_SingleItem(int value)
	{
		var list = new LinkedList<int>();
		list.AddLast(value);
		return list.Count == 1 && list.First!.Value == value && list.Last!.Value == value;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_AddLast_MultipleItems(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		return list.Count == 3 && list.First!.Value == val1 && list.Last!.Value == val3;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_AddLast_Node(int value)
	{
		var list = new LinkedList<int>();
		var node = new LinkedListNode<int>(value);
		list.AddLast(node);
		return list.Count == 1 && list.Last!.Value == value && ReferenceEquals(node, list.Last);
	}

	// == AddBefore tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_AddBefore_ValidNode(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		var node = list.AddLast(val3);
		list.AddBefore(node, val2);
		return list.Count == 3 && list.First!.Value == val1 && list.First.Next!.Value == val2 && list.Last!.Value == val3;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static bool Test_AddBefore_FirstNode(int val1, int val2)
	{
		var list = new LinkedList<int>();
		var node = list.AddFirst(val2);
		list.AddBefore(node, val1);
		return list.Count == 2 && list.First!.Value == val1 && list.Last!.Value == val2;
	}

	// == AddAfter tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_AddAfter_ValidNode(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		var node = list.AddLast(val1);
		list.AddLast(val3);
		list.AddAfter(node, val2);
		return list.Count == 3 && list.First!.Value == val1 && list.First.Next!.Value == val2 && list.Last!.Value == val3;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_AddAfter_LastNode(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddFirst(val1);
		var node = list.AddLast(val2);
		list.AddAfter(node, val3);
		return list.Count == 3 && list.First!.Value == val1 && list.First.Next!.Value == val2 && list.Last!.Value == val3;
	}

	// == Remove tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Remove_ExistingValue(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		bool removed = list.Remove(val2);
		return removed && list.Count == 2 && list.First!.Value == val1 && list.Last!.Value == val3;
	}

	[MosaUnitTest]
	public static bool Test_Remove_NonExistingValue()
	{
		var list = new LinkedList<int>();
		list.AddLast(1);
		list.AddLast(2);
		bool removed = list.Remove(99);
		return !removed && list.Count == 2;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Remove_Node(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		var node = list.AddLast(val2);
		list.AddLast(val3);
		list.Remove(node);
		return list.Count == 2 && list.First!.Value == val1 && list.Last!.Value == val3;
	}

	// == RemoveFirst tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RemoveFirst_SingleItem(int value)
	{
		var list = new LinkedList<int>();
		list.AddFirst(value);
		list.RemoveFirst();
		return list.Count == 0 && list.First == null && list.Last == null;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_RemoveFirst_MultipleItems(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		list.RemoveFirst();
		return list.Count == 2 && list.First!.Value == val2 && list.Last!.Value == val3;
	}

	// == RemoveLast tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_RemoveLast_SingleItem(int value)
	{
		var list = new LinkedList<int>();
		list.AddLast(value);
		list.RemoveLast();
		return list.Count == 0 && list.First == null && list.Last == null;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_RemoveLast_MultipleItems(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		list.RemoveLast();
		return list.Count == 2 && list.First!.Value == val1 && list.Last!.Value == val2;
	}

	// == Find tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Find_ExistingValue(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		var node = list.Find(val2);
		return node != null && node!.Value == val2;
	}

	[MosaUnitTest]
	public static bool Test_Find_NonExistingValue()
	{
		var list = new LinkedList<int>();
		list.AddLast(1);
		list.AddLast(2);
		var node = list.Find(99);
		return node == null;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Find_FirstOccurrence(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val2);
		list.AddLast(val3);
		var node = list.Find(val2);
		return node != null && node!.Value == val2 && node.Next!.Value == val2;
	}

	// == FindLast tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_FindLast_ExistingValue(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		var node = list.FindLast(val2);
		return node != null && node!.Value == val2;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_FindLast_LastOccurrence(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val2);
		list.AddLast(val3);
		var node = list.FindLast(val2);
		return node != null && node!.Value == val2 && node.Next!.Value == val3;
	}

	// == Contains tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Contains_ExistingValue(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		return list.Contains(val2);
	}

	[MosaUnitTest]
	public static bool Test_Contains_NonExistingValue()
	{
		var list = new LinkedList<int>();
		list.AddLast(1);
		list.AddLast(2);
		return !list.Contains(99);
	}

	// == Clear tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Clear_NonEmptyList(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		list.Clear();
		return list.Count == 0 && list.First == null && list.Last == null;
	}

	[MosaUnitTest]
	public static bool Test_Clear_EmptyList()
	{
		var list = new LinkedList<int>();
		list.Clear();
		return list.Count == 0 && list.First == null && list.Last == null;
	}

	// == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_CopyTo_ValidArray(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		var array = new int[4];
		list.CopyTo(array, 1);
		return array[0] == 0 && array[1] == val1 && array[2] == val2 && array[3] == val3;
	}

	// == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Enumerator_ForwardIteration(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		var items = new System.Collections.Generic.List<int>();
		foreach (var item in list)
		{
			items.Add(item);
		}
		return items.Count == 3 && items[0] == val1 && items[1] == val2 && items[2] == val3;
	}

	[MosaUnitTest]
	public static int Test_Enumerator_EmptyList()
	{
		var list = new LinkedList<int>();
		var count = 0;
		foreach (var item in list)
		{
			count++;
		}
		return count;
	}

	// == Node navigation tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Node_NextNavigation(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		var node = list.First;
		if (node!.Value != val1) return false;
		node = node.Next;
		if (node!.Value != val2) return false;
		node = node.Next;
		return node!.Value == val3 && node.Next == null;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Node_PreviousNavigation(int val1, int val2, int val3)
	{
		var list = new LinkedList<int>();
		list.AddLast(val1);
		list.AddLast(val2);
		list.AddLast(val3);
		var node = list.Last;
		if (node!.Value != val3) return false;
		node = node.Previous;
		if (node!.Value != val2) return false;
		node = node.Previous;
		return node!.Value == val1 && node.Previous == null;
	}
}
