// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.UnitTests.FlowControl;

public static class WhileTests
{
	[MosaUnitTest(0, 20)]
	[MosaUnitTest(-20, 0)]
	[MosaUnitTest(-100, 100)]
	public static int WhileIncI4(int start, int limit)
	{
		var count = 0;

		while (start < limit)
		{
			++count;
			++start;
		}

		return count;
	}

	[MosaUnitTest(20, 0)]
	[MosaUnitTest(0, -20)]
	[MosaUnitTest(100, -100)]
	public static int WhileDecI4(int start, int limit)
	{
		var count = 0;

		while (start > limit)
		{
			++count;
			--start;
		}

		return count;
	}

	[MosaUnitTest]
	public static bool WhileFalse()
	{
		var called = false;

		while (false)
		{
#pragma warning disable CS0162 // Unreachable code detected
			called = true;
#pragma warning restore CS0162 // Unreachable code detected
		}

		return called;
	}

	[MosaUnitTest]
	public static bool WhileContinueBreak()
	{
		const int limit = 20;
		var count = 0;

		while (true)
		{
			++count;

			if (count == limit)
			{
				break;
			}
			else
			{
				continue;
			}
		}

		return count == 20;
	}

	[MosaUnitTest]
	public static bool WhileContinueBreak2()
	{
		var start = 0;
		const int limit = 20;
		var count = 0;

		while (true)
		{
			++count;
			++start;

			if (start == limit)
			{
				break;
			}
			else
			{
				continue;
			}
		}

		return start == limit && count == 20;
	}

	[MosaUnitTest]
	public static int WhileContinueBreak2B()
	{
		var start = 0;
		var limit = 20;
		var count = 0;

		while (true)
		{
			++count;
			++start;

			if (start == limit)
			{
				break;
			}
			else
			{
				continue;
			}
		}

		return count;
	}

	[MosaUnitTest((byte)254, (byte)1)]
	[MosaUnitTest(byte.MaxValue, byte.MinValue)]
	public static int WhileOverflowIncI1(byte start, byte limit)
	{
		var count = 0;

		while (start != limit)
		{
			++start;
			++count;
		}

		return count;
	}

	[MosaUnitTest((byte)1, (byte)254)]
	[MosaUnitTest(byte.MinValue, byte.MaxValue)]
	public static int WhileOverflowDecI1(byte start, byte limit)
	{
		var count = 0;

		while (start != limit)
		{
			--start;
			++count;
		}

		return count;
	}

	[MosaUnitTest(2, 3, 0, 20)]
	[MosaUnitTest(0, 1, 100, 200)]
	[MosaUnitTest(1, 0, -100, 100)]
	[MosaUnitTest(int.MaxValue, int.MinValue, -2, 3)]
	public static int WhileNestedEqualsI4(int a, int b, int start, int limit)
	{
		var count = 0;
		var start2 = start;
		var status = a;

		while (status == a)
		{
			start2 = 1;

			while (start2 < 5)
			{
				++start2;
			}

			++start;

			if (start == limit)
			{
				status = b;
			}
		}

		return count;
	}

	// === SCCP Bug Regression Tests ===
	// These tests specifically target the bug where SCCP incorrectly eliminated null checks
	// in while loops traversing linked objects, causing infinite loops

	private class LinkedNode
	{
		public int Value;
		public LinkedNode Next;

		public LinkedNode(int value)
		{
			Value = value;
		}
	}

	[MosaUnitTest]
	public static int WhileObjectTraversalNullCheck()
	{
		// Create a simple linked list: 1 -> 2 -> 3 -> null
		var head = new LinkedNode(1);
		head.Next = new LinkedNode(2);
		head.Next.Next = new LinkedNode(3);

		var sum = 0;
		var current = head;

		// This while loop with null check was being incorrectly optimized by SCCP
		while (current != null)
		{
			sum += current.Value;
			current = current.Next;
		}

		return sum; // Should be 6
	}

	[MosaUnitTest]
	public static int WhileObjectTraversalCount()
	{
		// Create a longer chain to ensure the loop iterates multiple times
		var head = new LinkedNode(10);
		var current = head;

		for (int i = 1; i < 5; i++)
		{
			current.Next = new LinkedNode(10 + i);
			current = current.Next;
		}

		// Traverse and count
		current = head;
		int count = 0;

		while (current != null)
		{
			count++;
			current = current.Next;
		}

		return count; // Should be 5
	}

	[MosaUnitTest]
	public static bool WhileObjectTraversalLongChain()
	{
		// Create a chain of 10 nodes
		var head = new LinkedNode(0);
		var current = head;

		for (int i = 1; i < 10; i++)
		{
			current.Next = new LinkedNode(i);
			current = current.Next;
		}

		// Traverse to end
		current = head;
		int lastValue = -1;

		while (current != null)
		{
			lastValue = current.Value;
			current = current.Next;
		}

		return lastValue == 9; // Last node should have value 9
	}

	[MosaUnitTest]
	public static int WhileObjectTraversalSum()
	{
		// Create list: 10 -> 20 -> 30 -> 40
		var head = new LinkedNode(10);
		head.Next = new LinkedNode(20);
		head.Next.Next = new LinkedNode(30);
		head.Next.Next.Next = new LinkedNode(40);

		int sum = 0;
		var current = head;

		// This is the exact pattern from GCTests::SumLinkedList that was failing
		while (current != null)
		{
			sum += current.Value;
			current = current.Next;
		}

		return sum; // Should be 100
	}

	[MosaUnitTest]
	public static bool WhileObjectTraversalWithModification()
	{
		// Create a chain and modify values during traversal
		var head = new LinkedNode(1);
		head.Next = new LinkedNode(2);
		head.Next.Next = new LinkedNode(3);

		var current = head;
		int product = 1;

		while (current != null)
		{
			product *= current.Value;
			current.Value *= 10; // Modify during traversal
			current = current.Next;
		}

		// Verify both product (1*2*3 = 6) and modifications occurred
		return product == 6 && head.Value == 10 && head.Next.Value == 20;
	}

	[MosaUnitTest]
	public static int WhileObjectNullCheckAtStart()
	{
		// Test when list is initially null
		LinkedNode head = null;
		int count = 0;

		while (head != null)
		{
			count++;
			head = head.Next;
		}

		return count; // Should be 0
	}

	[MosaUnitTest]
	public static int WhileObjectSingleNode()
	{
		// Test with single-node list
		var head = new LinkedNode(42);
		var current = head;
		int sum = 0;

		while (current != null)
		{
			sum += current.Value;
			current = current.Next;
		}

		return sum; // Should be 42
	}
}
