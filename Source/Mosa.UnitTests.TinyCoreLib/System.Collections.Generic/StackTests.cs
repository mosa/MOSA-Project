// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.UnitTests.TinyCoreLib;

public static class StackTests
{
	// == Constructor tests

	[MosaUnitTest]
	public static int Test_Stack_Constructor_Default()
	{
		var stack = new Stack<int>();
		return stack.Count;
	}

	[MosaUnitTest]
	public static int Test_Stack_Constructor_WithCapacity()
	{
		var stack = new Stack<int>(10);
		return stack.Count;
	}

	[MosaUnitTest(Series = "I4I4I4I4I4")]
	public static int Test_Stack_Constructor_FromCollection(int val1, int val2, int val3, int val4, int val5)
	{
		var source = new int[] { val1, val2, val3, val4, val5 };
		var stack = new Stack<int>(source);
		return stack.Count;
	}

	// == Push tests

	[MosaUnitTest(Series = "I4")]
	public static int Test_Stack_Push_SingleItem(int value)
	{
		var stack = new Stack<int>();
		stack.Push(value);
		return stack.Count;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Stack_Push_MultipleItems(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		return stack.Count;
	}

	// == Pop tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Stack_Pop_SingleItem(int value)
	{
		var stack = new Stack<int>();
		stack.Push(value);
		int result = stack.Pop();
		return result == value && stack.Count == 0;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Stack_Pop_MultipleItems(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		stack.Pop();
		stack.Pop();
		return stack.Count;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_Pop_LIFO(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		return stack.Pop() == val3 && stack.Pop() == val2 && stack.Pop() == val1;
	}

	// == Peek tests

	[MosaUnitTest(Series = "I4")]
	public static int Test_Stack_Peek_ValidStack(int value)
	{
		var stack = new Stack<int>();
		stack.Push(value);
		int result = stack.Peek();
		return result;
	}

	[MosaUnitTest(Series = "I4")]
	public static int Test_Stack_Peek_DoesNotRemove(int value)
	{
		var stack = new Stack<int>();
		stack.Push(value);
		stack.Peek();
		return stack.Count;
	}

	// == Contains tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_Contains_ExistingItem(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		return stack.Contains(val2);
	}

	[MosaUnitTest]
	public static bool Test_Stack_Contains_NonExistingItem()
	{
		var stack = new Stack<int>();
		stack.Push(1);
		stack.Push(2);
		return !stack.Contains(99);
	}

	// == Clear tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Stack_Clear_NonEmptyStack(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		stack.Clear();
		return stack.Count;
	}

	// == ToArray tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_ToArray_ValidStack(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		var array = stack.ToArray();
		return array.Length == 3 && array[0] == val3 && array[1] == val2 && array[2] == val1;
	}

	// == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_CopyTo_ValidArray(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		var array = new int[4];
		stack.CopyTo(array, 1);
		return array[0] == 0 && array[1] == val3 && array[2] == val2 && array[3] == val1;
	}

	// == TryPop tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Stack_TryPop_NonEmptyStack(int value)
	{
		var stack = new Stack<int>();
		stack.Push(value);
		bool success = stack.TryPop(out int result);
		return success && result == value && stack.Count == 0;
	}

	[MosaUnitTest]
	public static bool Test_Stack_TryPop_EmptyStack()
	{
		var stack = new Stack<int>();
		bool success = stack.TryPop(out int result);
		return !success && result == 0;
	}

	// == TryPeek tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Stack_TryPeek_NonEmptyStack(int value)
	{
		var stack = new Stack<int>();
		stack.Push(value);
		bool success = stack.TryPeek(out int result);
		return success && result == value && stack.Count == 1;
	}

	[MosaUnitTest]
	public static bool Test_Stack_TryPeek_EmptyStack()
	{
		var stack = new Stack<int>();
		bool success = stack.TryPeek(out int result);
		return !success && result == 0;
	}

	// == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_Enumerator_Iteration(int val1, int val2, int val3)
	{
		var stack = new Stack<int>();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		var items = new List<int>();
		foreach (var item in stack)
		{
			items.Add(item);
		}
		return items.Count == 3 && items[0] == val3 && items[1] == val2 && items[2] == val1;
	}
}
