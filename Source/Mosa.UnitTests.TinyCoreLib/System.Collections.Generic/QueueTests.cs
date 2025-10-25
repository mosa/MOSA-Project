// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests.TinyCoreLib;

public static class QueueTests
{
	// == Constructor tests

	[MosaUnitTest]
	public static int Test_Queue_Constructor_Default()
	{
		var queue = new Queue<int>();
		return queue.Count;
	}

	[MosaUnitTest]
	public static int Test_Queue_Constructor_WithCapacity()
	{
		var queue = new Queue<int>(10);
		return queue.Count;
	}

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_Queue_Constructor_FromCollection(int val1, int val2, int val3, int val4, int val5)
	{
		var source = new int[] { val1, val2, val3, val4, val5 };
		var queue = new Queue<int>(source);
		return queue.Count;
	}

	// == Enqueue tests

	[MosaUnitTest(Series = "I4")]
	public static int Test_Queue_Enqueue_SingleItem(int value)
	{
		var queue = new Queue<int>();
		queue.Enqueue(value);
		return queue.Count;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Queue_Enqueue_MultipleItems(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		return queue.Count;
	}

	// == Dequeue tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Queue_Dequeue_SingleItem(int value)
	{
		var queue = new Queue<int>();
		queue.Enqueue(value);
		int result = queue.Dequeue();
		return result == value && queue.Count == 0;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Queue_Dequeue_MultipleItems(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		queue.Dequeue();
		queue.Dequeue();
		return queue.Count;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Queue_Dequeue_FIFO(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		return queue.Dequeue() == val1 && queue.Dequeue() == val2 && queue.Dequeue() == val3;
	}

	// == Peek tests

	[MosaUnitTest(Series = "I4")]
	public static int Test_Queue_Peek_ValidQueue(int value)
	{
		var queue = new Queue<int>();
		queue.Enqueue(value);
		int result = queue.Peek();
		return result;
	}

	[MosaUnitTest(Series = "I4")]
	public static int Test_Queue_Peek_DoesNotRemove(int value)
	{
		var queue = new Queue<int>();
		queue.Enqueue(value);
		queue.Peek();
		return queue.Count;
	}

	// == Contains tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Queue_Contains_ExistingItem(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		return queue.Contains(val2);
	}

	[MosaUnitTest]
	public static bool Test_Queue_Contains_NonExistingItem()
	{
		var queue = new Queue<int>();
		queue.Enqueue(1);
		queue.Enqueue(2);
		return !queue.Contains(99);
	}

	// == Clear tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Queue_Clear_NonEmptyQueue(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		queue.Clear();
		return queue.Count;
	}

	// == ToArray tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Queue_ToArray_ValidQueue(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		var array = queue.ToArray();
		return array.Length == 3 && array[0] == val1 && array[1] == val2 && array[2] == val3;
	}

	// == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4I4")]
	public static bool Test_Queue_CopyTo_ValidArray(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		var array = new int[4];
		queue.CopyTo(array, 1);
		return array[0] == 0 && array[1] == val1 && array[2] == val2 && array[3] == val3;
	}

	// == TryDequeue tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Queue_TryDequeue_NonEmptyQueue(int value)
	{
		var queue = new Queue<int>();
		queue.Enqueue(value);
		bool success = queue.TryDequeue(out int result);
		return success && result == value && queue.Count == 0;
	}

	[MosaUnitTest]
	public static bool Test_Queue_TryDequeue_EmptyQueue()
	{
		var queue = new Queue<int>();
		bool success = queue.TryDequeue(out int result);
		return !success && result == 0;
	}

	// == TryPeek tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Queue_TryPeek_NonEmptyQueue(int value)
	{
		var queue = new Queue<int>();
		queue.Enqueue(value);
		bool success = queue.TryPeek(out int result);
		return success && result == value && queue.Count == 1;
	}

	[MosaUnitTest]
	public static bool Test_Queue_TryPeek_EmptyQueue()
	{
		var queue = new Queue<int>();
		bool success = queue.TryPeek(out int result);
		return !success && result == 0;
	}

	// == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Queue_Enumerator_Iteration(int val1, int val2, int val3)
	{
		var queue = new Queue<int>();
		queue.Enqueue(val1);
		queue.Enqueue(val2);
		queue.Enqueue(val3);
		var items = new List<int>();
		foreach (var item in queue)
		{
			items.Add(item);
		}
		return items.Count == 3 && items[0] == val1 && items[1] == val2 && items[2] == val3;
	}
}
