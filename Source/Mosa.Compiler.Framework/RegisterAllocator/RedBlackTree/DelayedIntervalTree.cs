// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;

/// <summary>
/// Tree capable of adding arbitrary intervals and performing search queries on them.
/// Delays Add/Remove operations to reduce tree manipulations while maintaining correctness.
/// </summary>
public sealed class DelayedIntervalTree<T> where T : class
{
	private readonly IntervalTree<T> tree = new IntervalTree<T>();

	private bool delayedDelete;
	private bool delayedAdd;

	private int delayedDeleteStart;
	private int delayedDeleteEnd;

	private int delayedAddStart;
	private int delayedAddEnd;
	private T delayedAddValue;

	private void FlushDelete()
	{
		if (!delayedDelete)
			return;

		tree.Remove(delayedDeleteStart, delayedDeleteEnd);
		delayedDelete = false;
	}

	private void FlushAdd()
	{
		if (!delayedAdd)
			return;

		tree.Add(delayedAddStart, delayedAddEnd, delayedAddValue);
		delayedAdd = false;
	}

	private static bool Contains(int start, int end, int at)
	{
		return at >= start && at <= end;
	}

	private static bool Overlaps(int aStart, int aEnd, int bStart, int bEnd)
	{
		return aStart <= bEnd && aEnd >= bStart;
	}

	/// <summary>
	/// Check if interval range exists in tree (accounting for pending operations)
	/// </summary>
	public bool Contains(int start, int end)
	{
		// Short-circuit: if pending delete covers this range, it doesn't exist
		if (delayedDelete && start >= delayedDeleteStart && end <= delayedDeleteEnd)
			return false;

		// Short-circuit: if pending add overlaps this range, include it
		if (delayedAdd && Overlaps(delayedAddStart, delayedAddEnd, start, end))
			return true;

		FlushDelete();

		return tree.Contains(start, end);
	}

	/// <summary>
	/// Check if point is contained in any interval (accounting for pending operations)
	/// </summary>
	public bool Contains(int at)
	{
		// Short-circuit: check pending add first
		if (delayedAdd && Contains(delayedAddStart, delayedAddEnd, at))
			return true;

		// Short-circuit: check pending delete
		if (delayedDelete && Contains(delayedDeleteStart, delayedDeleteEnd, at))
			return false;

		return tree.Contains(at);
	}

	public void Add(int start, int end, T value)
	{
		// If pending delete overlaps, flush add and replace
		if (delayedDelete && Overlaps(delayedDeleteStart, delayedDeleteEnd, start, end))
		{
			FlushAdd();
			tree.Replace(start, end, value);
			delayedDelete = false;
			return;
		}

		FlushAdd();

		delayedAdd = true;
		delayedAddStart = start;
		delayedAddEnd = end;
		delayedAddValue = value;
	}

	public void Remove(int start, int end)
	{
		// If pending add overlaps, cancel it
		if (delayedAdd && (Contains(delayedAddStart, delayedAddEnd, start) || Contains(delayedAddStart, delayedAddEnd, end)))
		{
			delayedAdd = false;
			return;
		}

		FlushDelete();

		delayedDelete = true;
		delayedDeleteStart = start;
		delayedDeleteEnd = end;
	}

	/// <summary>
	/// Replace the value of an interval (accounting for pending operations)
	/// </summary>
	public void Replace(int start, int end, T value)
	{
		// Check if it's the pending add
		if (delayedAdd && start == delayedAddStart && end == delayedAddEnd)
		{
			delayedAddValue = value;
			return;
		}

		FlushDelete();
		FlushAdd();
		tree.Replace(start, end, value);
	}

	public T SearchFirstOverlapping(int start, int end)
	{
		if (TrySearchFirstOverlapping(start, end, out var value))
			return value;

		throw new KeyNotFoundException("No overlapping interval found.");
	}

	public bool TrySearchFirstOverlapping(int start, int end, out T value)
	{
		// If the pending add overlaps the query range, return it immediately without touching the tree.
		if (delayedAdd && Overlaps(delayedAddStart, delayedAddEnd, start, end))
		{
			value = delayedAddValue;
			return true;
		}

		FlushDelete();
		// delayedAdd (if any) does not overlap [start, end], so flushing it won't affect this search.
		return tree.TrySearchFirstOverlapping(start, end, out value);
	}

	public T SearchFirstOverlapping(int at)
	{
		if (TrySearchFirstOverlapping(at, out var value))
			return value;

		throw new KeyNotFoundException("No overlapping interval found.");
	}

	public bool TrySearchFirstOverlapping(int at, out T value)
	{
		// If the pending add covers the query point, return it immediately without touching the tree.
		if (delayedAdd && Contains(delayedAddStart, delayedAddEnd, at))
		{
			value = delayedAddValue;
			return true;
		}

		FlushDelete();
		// delayedAdd (if any) does not cover 'at', so flushing it won't affect this search.
		return tree.TrySearchFirstOverlapping(at, out value);
	}

	public List<T> Search(int at)
	{
		FlushDelete();
		var result = tree.Search(at);

		// Append the pending add to the result if it covers 'at', avoiding a tree insertion.
		if (delayedAdd && Contains(delayedAddStart, delayedAddEnd, at))
			result.Add(delayedAddValue);

		return result;
	}

	/// <summary>
	/// Fills a caller-provided list to avoid per-call allocation.
	/// </summary>
	public void Search(int at, List<T> result)
	{
		FlushDelete();
		tree.Search(at, result);

		if (delayedAdd && Contains(delayedAddStart, delayedAddEnd, at))
			result.Add(delayedAddValue);
	}

	public List<T> Search(int start, int end)
	{
		FlushDelete();
		var result = tree.Search(start, end);

		// Append the pending add to the result if it overlaps [start, end], avoiding a tree insertion.
		if (delayedAdd && Overlaps(delayedAddStart, delayedAddEnd, start, end))
			result.Add(delayedAddValue);

		return result;
	}

	/// <summary>
	/// Fills a caller-provided list to avoid per-call allocation.
	/// </summary>
	public void Search(int start, int end, List<T> result)
	{
		FlushDelete();
		tree.Search(start, end, result);

		if (delayedAdd && Overlaps(delayedAddStart, delayedAddEnd, start, end))
			result.Add(delayedAddValue);
	}

	public IEnumerator<T> GetEnumerator()
	{
		FlushDelete();
		FlushAdd();
		return tree.GetEnumerator();
	}

	public override string ToString()
	{
		FlushDelete();
		FlushAdd();

		var enumerator = tree.GetEnumerator();
		if (!enumerator.MoveNext())
			return string.Empty;
		return tree.ToString();
	}
}
