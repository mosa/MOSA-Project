// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class DelayedIntervalTreeTests
{
	[Fact]
	public void Insert()
	{
		var tree = new DelayedIntervalTree<object>();

		for (int i = 0; i < 100; i++)
		{
			tree.Add(i * 2, i * 2 + 1, i);

			for (int n = 0; n <= i; n++)
			{
				Assert.True(tree.Contains(n * 2));
				Assert.True(tree.Contains(n * 2, n * 2 + 1));
			}
		}
	}

	[Fact]
	public void Delete()
	{
		var tree = new DelayedIntervalTree<object>();

		for (int i = 0; i < 100; i++)
		{
			tree.Add(i * 2, i * 2 + 1, i);
		}

		for (int i = 0; i < 100; i += 2)
		{
			tree.Remove(i * 2, i * 2 + 1);
			Assert.False(tree.Contains(i * 2));
		}
	}

	[Fact]
	public void MixedAddDelete()
	{
		var tree = new DelayedIntervalTree<object>();

		int i = 0;

		tree.Add(1, 2, ++i);
		Assert.True(tree.Contains(1));
		tree.Remove(1, 2);
		Assert.False(tree.Contains(1));
		Assert.False(tree.Contains(2));
		tree.Add(1, 2, ++i);
		Assert.True(tree.Contains(1));
		tree.Remove(1, 2);
		Assert.False(tree.Contains(1));
		tree.Add(1, 2, ++i);
		Assert.True(tree.Contains(1));
		tree.Add(3, 4, ++i);
		Assert.True(tree.Contains(3));
		tree.Remove(1, 2);
		Assert.False(tree.Contains(1));
		Assert.False(tree.Contains(2));
		tree.Remove(3, 4);
		Assert.False(tree.Contains(3));
	}

	[Fact]
	public void CheckIntervals()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 2, null);

		Assert.False(tree.Contains(0));
		Assert.True(tree.Contains(1));
		Assert.True(tree.Contains(2));

		Assert.True(tree.Contains(2, 3));
		Assert.False(tree.Contains(3, 4));
	}

	[Fact]
	public void CacheDelayedAddShortCircuit()
	{
		// Verify that pending add is checked before flushing and accessing tree
		var tree = new DelayedIntervalTree<object>();

		tree.Add(10, 20, "pending");

		// Should find pending add without touching tree
		Assert.True(tree.Contains(10));
		Assert.True(tree.Contains(15));
		Assert.True(tree.Contains(20));
		Assert.False(tree.Contains(21));
		Assert.False(tree.Contains(9));

		// Range check should also work with pending add
		Assert.True(tree.Contains(10, 20));
		Assert.True(tree.Contains(15, 18));
		Assert.False(tree.Contains(25, 30));
	}

	[Fact]
	public void CacheDelayedDeleteShortCircuit()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 5, "value1");
		tree.Add(10, 15, "value2");

		// Delay the delete
		tree.Remove(1, 5);

		// Pending delete should prevent Contains from returning true
		Assert.False(tree.Contains(1));
		Assert.False(tree.Contains(3));
		Assert.False(tree.Contains(5));

		// But other intervals should still be found
		Assert.True(tree.Contains(10));
		Assert.True(tree.Contains(12));
	}

	[Fact]
	public void CacheAddCancelsByDelete()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(5, 10, "pending");

		// Removing the pending add should cancel it
		tree.Remove(5, 10);

		// Should not find anything
		Assert.False(tree.Contains(5));
		Assert.False(tree.Contains(7));
		Assert.False(tree.Contains(10));
	}

	[Fact]
	public void CacheSearchFirstOverlappingWithPendingAdd()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(10, 20, "tree_value");

		// Add pending interval
		tree.Add(30, 40, "pending_value");

		// Search should find pending add without flushing tree add
		Assert.True(tree.TrySearchFirstOverlapping(30, 40, out var result));
		Assert.Equal("pending_value", result);

		// Search that only matches tree should also work
		Assert.True(tree.TrySearchFirstOverlapping(10, 20, out result));
		Assert.Equal("tree_value", result);
	}

	[Fact]
	public void CacheSearchFirstOverlappingByPoint()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 5, "value1");
		tree.Add(10, 20, "pending");

		// Search by point should find pending without flushing
		Assert.True(tree.TrySearchFirstOverlapping(15, out var result));
		Assert.Equal("pending", result);

		// Search point before pending
		Assert.True(tree.TrySearchFirstOverlapping(3, out result));
		Assert.Equal("value1", result);
	}

	[Fact]
	public void CacheSearchRangeWithPendingAdd()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 5, "value1");
		tree.Add(20, 30, "pending");

		// Search range that overlaps pending
		var results = tree.Search(25, 35);
		Assert.Single(results);
		Assert.Equal("pending", results[0]);

		// Search range that doesn't overlap pending
		results = tree.Search(1, 5);
		Assert.Single(results);
		Assert.Equal("value1", results[0]);

		// Search range that overlaps both
		results = tree.Search(3, 25);
		Assert.Equal(2, results.Count);
		Assert.Contains("value1", results);
		Assert.Contains("pending", results);
	}

	[Fact]
	public void CacheSearchPointWithPendingAdd()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 5, "value1");
		tree.Add(20, 30, "pending");

		// Search at point in pending
		var results = tree.Search(25);
		Assert.Single(results);
		Assert.Equal("pending", results[0]);

		// Search at point in tree value
		results = tree.Search(3);
		Assert.Single(results);
		Assert.Equal("value1", results[0]);
	}

	[Fact]
	public void CacheReplacePendingAdd()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(5, 10, "original");

		// Replace the pending add
		tree.Replace(5, 10, "replaced");

		Assert.True(tree.TrySearchFirstOverlapping(7, out var result));
		Assert.Equal("replaced", result);
	}

	[Fact]
	public void CacheReplaceTreeValue()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(5, 10, "original");
		tree.Add(20, 30, "pending");

		// Replace value in tree (flush both pending ops first)
		tree.Replace(5, 10, "replaced");

		// Pending should be flushed
		Assert.True(tree.Contains(25));

		// Original should be replaced
		Assert.True(tree.TrySearchFirstOverlapping(7, out var result));
		Assert.Equal("replaced", result);
	}

	[Fact]
	public void CacheMixedPendingOperations()
	{
		var tree = new DelayedIntervalTree<object>();

		// Add and then remove different intervals
		tree.Add(5, 10, "add1");
		tree.Add(20, 30, "add2");

		// Remove first one while second is pending
		tree.Remove(5, 10);

		// First should not be found
		Assert.False(tree.Contains(7));

		// Second should be found (it's pending)
		Assert.True(tree.Contains(25));

		// Now remove the pending second
		tree.Remove(20, 30);

		// Both should be gone
		Assert.False(tree.Contains(7));
		Assert.False(tree.Contains(25));
	}

	[Fact]
	public void CacheDeleteWithOverlapAddInteraction()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 5, "value1");

		// Add pending interval
		tree.Add(3, 7, "add_during_delete");

		// Remove that overlaps the pending add (should cancel it)
		tree.Remove(3, 7);

		// The pending add was cancelled, so value1 should still be found
		Assert.True(tree.Contains(1));
		Assert.True(tree.Contains(5));

		// Point 6 should not be found (value1 only goes to 5)
		Assert.False(tree.Contains(6));
	}

	[Fact]
	public void CacheSearchWithReplacedValue()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(10, 20, "value1");
		tree.Add(30, 40, "pending");

		// Replace tree value
		tree.Replace(10, 20, "replaced1");

		// Search should find replaced value
		Assert.True(tree.TrySearchFirstOverlapping(15, out var result));
		Assert.Equal("replaced1", result);
	}

	[Fact]
	public void EnumeratorFlushesAllPending()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 2, "add1");
		tree.Add(3, 4, "add2");

		// Before enumeration, pending operations not flushed
		tree.Remove(1, 2);

		// Enumerate should flush both operations
		var list = new List<object>();
		foreach (var item in tree)
		{
			list.Add(item);
		}

		// Should only have add2 (add1 was removed)
		Assert.Single(list);
		Assert.Equal("add2", list[0]);
	}

	[Fact]
	public void ToStringFlushesAllPending()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 2, "test");
		tree.Add(3, 4, "pending");

		// ToString should flush all pending operations
		var str = tree.ToString();

		// Should not be empty
		Assert.NotEmpty(str);

		// After ToString, tree should be in consistent state
		// Adding more should work correctly
		tree.Add(5, 6, "another");
		Assert.True(tree.Contains(5));
	}

	[Fact]
	public void EdgeCaseEmptyTree()
	{
		var tree = new DelayedIntervalTree<object>();

		Assert.False(tree.Contains(1));
		Assert.False(tree.Contains(1, 2));

		Assert.False(tree.TrySearchFirstOverlapping(1, out _));
		Assert.False(tree.TrySearchFirstOverlapping(1, 2, out _));

		var results = tree.Search(1);
		Assert.Empty(results);

		results = tree.Search(1, 2);
		Assert.Empty(results);
	}

	[Fact]
	public void EdgeCaseRemoveNonExistent()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 2, "value");

		// Remove non-existent should be safe
		tree.Remove(10, 20);

		// Original should still be there
		Assert.True(tree.Contains(1));
	}

	[Fact]
	public void SearchWithCallerProvidedList()
	{
		var tree = new DelayedIntervalTree<object>();

		tree.Add(1, 5, "value1");
		tree.Add(10, 20, "pending");

		var results = new List<object>();

		// Search by point with provided list
		tree.Search(3, results);
		Assert.Single(results);
		Assert.Equal("value1", results[0]);

		results.Clear();

		// Search by range with provided list
		tree.Search(10, 20, results);
		Assert.Single(results);
		Assert.Equal("pending", results[0]);

		results.Clear();

		// Search overlapping both
		tree.Search(3, 15, results);
		Assert.Equal(2, results.Count);
	}
}
