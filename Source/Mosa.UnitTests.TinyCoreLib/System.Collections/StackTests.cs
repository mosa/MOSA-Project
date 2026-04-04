// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Collections.Generic;

namespace Mosa.UnitTests.TinyCoreLib;

public static class NonGenericStackTests
{
	// == Constructor tests

	[MosaUnitTest]
	public static int Test_Stack_Constructor_Default()
	{
		var stack = new Stack();
		return stack.Count;
	}

	[MosaUnitTest]
	public static int Test_Stack_Constructor_WithCapacity()
	{
		var stack = new Stack(10);
		return stack.Count;
	}

	[MosaUnitTest]
	public static int Test_Stack_Constructor_FromCollection()
	{
		var source = new ArrayList { 1, 2, 3, 4 };
		var stack = new Stack(source);
		return stack.Count;
	}

	// == Push tests

	[MosaUnitTest]
	public static int Test_Stack_Push_MultipleItems()
	{
		var stack = new Stack();
		stack.Push(42);
		stack.Push("Hello");
		stack.Push(3.14);
		return stack.Count;
	}

	[MosaUnitTest]
	public static int Test_Stack_Push_NullValue()
	{
		var stack = new Stack();
		stack.Push(null);
		return stack.Count;
	}

	// == Pop tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Stack_Pop_SingleItem(int value)
	{
		var stack = new Stack();
		stack.Push(value);

		var result = stack.Pop();
		return result is int i && i == value && stack.Count == 0;
	}

	[MosaUnitTest]
	public static int Test_Stack_Pop_Count()
	{
		var stack = new Stack();
		stack.Push(1);
		stack.Push(2);
		stack.Push(3);
		stack.Pop();
		stack.Pop();
		return stack.Count;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_Pop_Values(int val1, int val2, int val3)
	{
		var stack = new Stack();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		var assert1 = stack.Pop() is int i1 && i1 == val3;
		var assert2 = stack.Pop() is int i2 && i2 == val2;
		var assert3 = stack.Pop() is int i3 && i3 == val1;
		return assert1 && assert2 && assert3;
	}

	// == Peek tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Stack_Peek_ValidStack(int value)
	{
		var stack = new Stack();
		stack.Push(value);

		var result = stack.Peek();
		return result is int i && i == value && stack.Count == 1;
	}

	// == Contains tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_Contains_ExistingItem(int val1, int val2, int val3)
	{
		var stack = new Stack();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);
		return stack.Contains(val2);
	}

	[MosaUnitTest]
	public static bool Test_Stack_Contains_NonExistingItem()
	{
		var stack = new Stack();
		stack.Push(1);
		stack.Push(2);
		return !stack.Contains(99);
	}

	[MosaUnitTest(Series = "I4I4")]
	public static bool Test_Stack_Contains_NullValue(int val1, int val2)
	{
		var stack = new Stack();
		stack.Push(val1);
		stack.Push(null);
		stack.Push(val2);
		return stack.Contains(null);
	}

	// == Clear tests

	[MosaUnitTest]
	public static int Test_Stack_Clear_NonEmptyStack()
	{
		var stack = new Stack();
		stack.Push(1);
		stack.Push(2);
		stack.Push(3);
		stack.Clear();
		return stack.Count;
	}

	// == ToArray tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_ToArray_ValidStack(int val1, int val2, int val3)
	{
		var stack = new Stack();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);

		var array = stack.ToArray();
		var assert1 = array[0] is int i1 && i1 == val3;
		var assert2 = array[1] is int i2 && i2 == val2;
		var assert3 = array[2] is int i3 && i3 == val1;
		return array.Length == 3 && assert1 && assert2 && assert3;
	}

	[MosaUnitTest]
	public static int Test_Stack_ToArray_EmptyStack()
	{
		var stack = new Stack();
		var array = stack.ToArray();
		return array.Length;
	}

	// == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_CopyTo_ValidArray(int val1, int val2, int val3)
	{
		var stack = new Stack();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);

		var array = new object[4];
		stack.CopyTo(array, 1);

		var assert1 = array[1] is int i1 && i1 == val3;
		var assert2 = array[2] is int i2 && i2 == val2;
		var assert3 = array[3] is int i3 && i3 == val1;
		return assert1 && assert2 && assert3;
	}

	// == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Stack_Enumerator_Iteration(int val1, int val2, int val3)
	{
		var stack = new Stack();
		stack.Push(val1);
		stack.Push(val2);
		stack.Push(val3);

		var items = new List<object?>();
		foreach (var item in stack)
			items.Add(item);

		var assert1 = items[0] is int i1 && i1 == val3;
		var assert2 = items[1] is int i2 && i2 == val2;
		var assert3 = items[2] is int i3 && i3 == val1;
		return items.Count == 3 && assert1 && assert2 && assert3;
	}

	[MosaUnitTest]
	public static int Test_Stack_Enumerator_EmptyStack()
	{
		var stack = new Stack();
		var count = 0;

		foreach (var item in stack)
			count++;

		return count;
	}
}
