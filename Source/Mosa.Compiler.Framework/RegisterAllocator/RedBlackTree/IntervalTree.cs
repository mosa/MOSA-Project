// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;

/// <summary>
/// Tree capable of adding arbitrary intervals and performing search queries on them
/// </summary>
public sealed partial class IntervalTree<T> where T : class
{
	private readonly Node<T> Sentinel = new Node<T>(new Interval(-1, -1), null);

	private Node<T> Root;

	public IntervalTree()
	{
		Sentinel.Left = Sentinel;
		Sentinel.Right = Sentinel;
		Sentinel.Parent = Sentinel;

		Root = Sentinel;
		Root.Left = Sentinel;
		Root.Right = Sentinel;
		Root.Parent = Sentinel;
	}

	#region Tree searching

	public bool Contains(int start, int end)
	{
		return SearchSubtree(Root, new Interval(start, end));
	}

	private bool Contains(Interval interval)
	{
		return SearchSubtree(Root, interval);
	}

	public bool Contains(int at)
	{
		return SearchSubtree(Root, at);
	}

	/// <summary>
	/// Search interval tree for a given point
	/// </summary>
	/// <param name="at">value to be searched for</param>
	/// <returns>list of intervals which contain the value</returns>
	public List<T> Search(int at)
	{
		var result = new List<T>();
		SearchSubtree(Root, at, result);
		return result;
	}

	/// <summary>
	/// Search interval tree for a given point, filling a caller-provided list to avoid allocation
	/// </summary>
	public void Search(int at, List<T> result)
	{
		SearchSubtree(Root, at, result);
	}

	/// <summary>
	/// Search interval tree for intervals overlapping with given
	/// </summary>
	public List<T> Search(int start, int end)
	{
		var result = new List<T>();
		SearchSubtree(Root, new Interval(start, end), result);
		return result;
	}

	/// <summary>
	/// Search interval tree for intervals overlapping with given, filling a caller-provided list to avoid allocation
	/// </summary>
	public void Search(int start, int end, List<T> result)
	{
		SearchSubtree(Root, new Interval(start, end), result);
	}

	/// <summary>
	/// Search interval tree for intervals overlapping with given
	/// </summary>
	private List<T> Search(Interval interval)
	{
		var result = new List<T>();
		SearchSubtree(Root, interval, result);
		return result;
	}

	/// <summary>
	/// Searches for the first overlapping interval
	/// </summary>
	public T SearchFirstOverlapping(int start, int end)
	{
		if (TrySearchFirstOverlapping(start, end, out var value))
		{
			return value;
		}

		throw new KeyNotFoundException("No overlapping interval found.");
	}

	public bool TrySearchFirstOverlapping(int start, int end, out T value)
	{
		return TrySearchFirstOverlapping(new Interval(start, end), out value);
	}

	private bool TrySearchFirstOverlapping(Interval interval, out T value)
	{
		var node = Root;

		while (node != Sentinel && !node.Interval.Overlaps(interval))
		{
			if (node.Left != Sentinel && node.Left.MaxEnd > interval.Start)
			{
				node = node.Left;
			}
			else
			{
				node = node.Right;
			}
		}

		if (node == Sentinel)
		{
			value = null;
			return false;
		}

		value = node.Value;
		return true;
	}

	public T SearchFirstOverlapping(int at)
	{
		if (TrySearchFirstOverlapping(at, out var value))
		{
			return value;
		}

		throw new KeyNotFoundException("No overlapping interval found.");
	}

	public bool TrySearchFirstOverlapping(int at, out T value)
	{
		var node = Root;

		while (node != Sentinel && !node.Interval.Overlaps(at))
		{
			if (node.Left != Sentinel && node.Left.MaxEnd > at)
			{
				node = node.Left;
			}
			else
			{
				node = node.Right;
			}
		}

		if (node == Sentinel)
		{
			value = null;
			return false;
		}

		value = node.Value;
		return true;
	}

	private void SearchSubtree(Node<T> node, Interval interval, List<T> result)
	{
		if (node == Sentinel)
		{
			return;
		}

		if (node.Left != Sentinel && node.Left.MaxEnd >= interval.Start)
		{
			SearchSubtree(node.Left, interval, result);
		}

		if (interval.Overlaps(node.Interval))
		{
			result.Add(node.Value);
		}

		if (node.Right != Sentinel && node.Right.MaxEnd >= interval.Start)
		{
			SearchSubtree(node.Right, interval, result);
		}
	}

	private void SearchSubtree(Node<T> node, int at, List<T> result)
	{
		if (node == Sentinel)
		{
			return;
		}

		if (node.Left != Sentinel && node.Left.MaxEnd >= at)
		{
			SearchSubtree(node.Left, at, result);
		}

		if (node.Interval.Contains(at))
		{
			result.Add(node.Value);
		}

		if (node.Right != Sentinel && node.Right.MaxEnd >= at)
		{
			SearchSubtree(node.Right, at, result);
		}
	}

	/// <summary>
	/// Returns true if any interval in the subtree overlaps the given interval.
	/// The right-side tail recursion is replaced with a loop to reduce stack depth.
	/// </summary>
	private bool SearchSubtree(Node<T> node, Interval interval)
	{
		while (node != Sentinel)
		{
			if (node.Left != Sentinel && node.Left.MaxEnd >= interval.Start && SearchSubtree(node.Left, interval))
				return true;

			if (interval.Overlaps(node.Interval))
				return true;

			if (node.Right == Sentinel || node.Right.MaxEnd < interval.Start)
				return false;

			node = node.Right;
		}

		return false;
	}

	/// <summary>
	/// Returns true if any interval in the subtree contains the given point.
	/// The right-side tail recursion is replaced with a loop to reduce stack depth.
	/// </summary>
	private bool SearchSubtree(Node<T> node, int at)
	{
		while (node != Sentinel)
		{
			if (node.Left != Sentinel && node.Left.MaxEnd >= at && SearchSubtree(node.Left, at))
				return true;

			if (node.Interval.Contains(at))
				return true;

			if (node.Right == Sentinel || node.Right.MaxEnd < at)
				return false;

			node = node.Right;
		}

		return false;
	}

	private Node<T> FindInterval(Node<T> tree, Interval interval)
	{
		while (tree != Sentinel)
		{
			var val = tree.Interval.CompareTo(interval);

			if (val == 0)
			{
				return tree;
			}

			if (val > 0)
			{
				tree = tree.Left;
			}
			else
			{
				tree = tree.Right;
			}
		}

		return Sentinel;
	}

	#endregion Tree searching

	/// <summary>
	/// Insert new interval to interval tree
	/// </summary>
	/// <param name="interval">interval to add</param>
	public void Add(int start, int end, T value)
	{
		Add(new Interval(start, end), value);
	}

	/// <summary>
	/// Insert new interval to interval tree
	/// </summary>
	/// <param name="interval">interval to add</param>
	private void Add(Interval interval, T value)
	{
		//Debug.Assert(!Contains(interval));

		var node = new Node<T>(interval, value)
		{
			Parent = Sentinel,
			Left = Sentinel,
			Right = Sentinel
		};

		if (Root == Sentinel)
		{
			node.Color = Color.BLACK;
			Root = node;
		}
		else
		{
			InsertInterval(node, Root);
		}
	}

	#region Tree insertion internals

	/// <summary>
	/// Recursively descends to the correct spot for interval insertion in the tree
	/// When a free spot is found for the node, it is attached and tree state is validated
	/// </summary>
	/// <param name="node">interval to be added</param>
	/// <param name="currentNode">subtree accessed in recursion</param>
	private void InsertInterval(Node<T> node, Node<T> currentNode)
	{
		while (true)
		{
			var compare = node.Interval.CompareTo(currentNode.Interval);

			if (compare < 0)
			{
				if (currentNode.Left == Sentinel)
				{
					currentNode.Left = node;
					node.Parent = currentNode;
					break;
				}

				currentNode = currentNode.Left;
				continue;
			}

			if (compare > 0)
			{
				if (currentNode.Right == Sentinel)
				{
					currentNode.Right = node;
					node.Parent = currentNode;
					break;
				}

				currentNode = currentNode.Right;
				continue;
			}

			return;
		}

		node.Left = Sentinel;
		node.Right = Sentinel;
		node.Color = Color.RED;

		RecalculateMaxEndUpwards(node.Parent);

		RenewConstraintsAfterInsert(node);

		Root.Color = Color.BLACK;
	}

	/// <summary>
	/// The direction of the parent, from the child point-of-view
	/// </summary>
	private Direction GetParentDirection(Node<T> node)
	{
		if (node.Parent == Sentinel)
		{
			return Direction.NONE;
		}

		return node.Parent.Left == node ? Direction.RIGHT : Direction.LEFT;
	}

	private Node<T> GetSuccessor(Node<T> node)
	{
		if (node.Right == Sentinel)
		{
			return Sentinel;
		}

		node = node.Right;
		while (node.Left != Sentinel)
		{
			node = node.Left;
		}

		return node;
	}

	private Node<T> GetGrandParent(Node<T> node)
	{
		if (node.Parent != Sentinel)
		{
			return node.Parent.Parent;
		}
		return Sentinel;
	}

	private Node<T> GetUncle(Node<T> node)
	{
		var grandparent = GetGrandParent(node);

		if (grandparent == Sentinel)
		{
			return Sentinel;
		}

		if (node.Parent == grandparent.Left)
		{
			return grandparent.Right;
		}

		return grandparent.Left;
	}

	/// <summary>
	/// Validates and applies RB-tree constraints to node.
	/// The tail-recursive call is replaced with a loop to avoid O(log n) stack frames.
	/// </summary>
	private void RenewConstraintsAfterInsert(Node<T> node)
	{
		while (true)
		{
			if (node.Parent == Sentinel)
				return;

			if (node.Parent.Color == Color.BLACK)
				return;

			var uncle = GetUncle(node);

			if (uncle != Sentinel && uncle.Color == Color.RED)
			{
				node.Parent.Color = uncle.Color = Color.BLACK;

				var grandparent = GetGrandParent(node);

				if (grandparent == Sentinel || grandparent.Parent == Sentinel)
					return;

				grandparent.Color = Color.RED;
				node = grandparent;
				// continue loop instead of recursive call
			}
			else
			{
				var parentDirection = GetParentDirection(node);
				var parentParentDirection = GetParentDirection(node.Parent);

				if (parentDirection == Direction.LEFT && parentParentDirection == Direction.RIGHT)
				{
					RotateLeft(node.Parent);
					node = node.Left;
				}
				else if (parentDirection == Direction.RIGHT && parentParentDirection == Direction.LEFT)
				{
					RotateRight(node.Parent);
					node = node.Right;
				}

				node.Parent.Color = Color.BLACK;

				var grandparent = GetGrandParent(node);

				if (grandparent == Sentinel)
					return;

				grandparent.Color = Color.RED;

				if (GetParentDirection(node) == Direction.RIGHT)
				{
					RotateRight(grandparent);
				}
				else
				{
					RotateLeft(grandparent);
				}

				return;
			}
		}
	}

	#endregion Tree insertion internals

	/// <summary>
	/// Removes interval from tree (if present in tree)
	/// </summary>
	public void Remove(int start, int end)
	{
		RemoveNode(FindInterval(Root, new Interval(start, end)));
	}

	/// <summary>
	/// Replaces interval with new value
	/// </summary>
	public void Replace(int start, int end, T value)
	{
		var node = FindInterval(Root, new Interval(start, end));
		node.Value = value;
	}

	private void RemoveNode(Node<T> node)
	{
		if (node == Sentinel)
		{
			return;
		}

		var temp = node;

		if (node.Right != Sentinel && node.Left != Sentinel)
		{
			// Trick when deleting node with both children, switch it with closest in order node
			// swap values and delete the bottom node converting it to other cases

			temp = GetSuccessor(node);
			node.Interval = temp.Interval;
			node.Value = temp.Value;

			RecalculateMaxEndUpwards(node);
		}

		node = temp;
		temp = node.Left != Sentinel ? node.Left : node.Right;

		// we will replace node with temp and delete node
		temp.Parent = node.Parent;

		if (node.Parent == Sentinel)
		{
			Root = temp; // Set new root
		}
		else
		{
			// Reattach node to parent
			if (GetParentDirection(node) == Direction.RIGHT)
			{
				node.Parent.Left = temp;
			}
			else
			{
				node.Parent.Right = temp;
			}

			RecalculateMaxEndUpwards(node.Parent);
		}

		if (node.Color == Color.BLACK)
		{
			RenewConstraintsAfterDelete(temp);
		}
	}

	/// <summary>
	/// Ensures constraints still apply after node deletion
	/// - made with the help of algorithm from Cormen et Al. Introduction to Algorithms 2nd ed.
	/// </summary>
	private void RenewConstraintsAfterDelete(Node<T> node)
	{
		// Need to bubble up and fix
		while (node != Root && node.Color == Color.BLACK)
		{
			if (GetParentDirection(node) == Direction.RIGHT)
			{
				var aux = node.Parent.Right;
				if (aux.Color == Color.RED)
				{
					aux.Color = Color.BLACK;
					node.Parent.Color = Color.RED;
					RotateLeft(node.Parent);
					aux = node.Parent.Right;
				}

				if (aux.Left.Color == Color.BLACK && aux.Right.Color == Color.BLACK)
				{
					aux.Color = Color.RED;
					node = node.Parent;
				}
				else
				{
					if (aux.Right.Color == Color.BLACK)
					{
						aux.Left.Color = Color.BLACK;
						aux.Color = Color.RED;
						RotateRight(aux);
						aux = node.Parent.Right;
					}

					aux.Color = node.Parent.Color;
					node.Parent.Color = Color.BLACK;
					aux.Right.Color = Color.BLACK;
					RotateLeft(node.Parent);
					node = Root;
				}
			}
			else
			{
				var aux = node.Parent.Left;
				if (aux.Color == Color.RED)
				{
					aux.Color = Color.BLACK;
					node.Parent.Color = Color.RED;
					RotateRight(node.Parent);
					aux = node.Parent.Left;
				}

				if (aux.Left.Color == Color.BLACK && aux.Right.Color == Color.BLACK)
				{
					aux.Color = Color.RED;
					node = node.Parent;
				}
				else
				{
					if (aux.Left.Color == Color.BLACK)
					{
						aux.Right.Color = Color.BLACK;
						aux.Color = Color.RED;
						RotateLeft(aux);
						aux = node.Parent.Left;
					}

					aux.Color = node.Parent.Color;
					node.Parent.Color = Color.BLACK;
					aux.Left.Color = Color.BLACK;
					RotateRight(node.Parent);
					node = Root;
				}
			}
		}

		node.Color = Color.BLACK;
	}

	/// <summary>
	/// General right rotation
	/// </summary>
	/// <param name="node">Top of rotated subtree</param>
	private void RotateRight(Node<T> node)
	{
		var pivot = node.Left;
		var dir = GetParentDirection(node);
		var parent = node.Parent;
		var tempTree = pivot.Right;

		pivot.Right = node;
		node.Parent = pivot;
		node.Left = tempTree;

		if (tempTree != Sentinel)
		{
			tempTree.Parent = node;
		}

		if (dir == Direction.LEFT)
		{
			parent.Right = pivot;
		}
		else if (dir == Direction.RIGHT)
		{
			parent.Left = pivot;
		}
		else
		{
			Root = pivot;
		}

		pivot.Parent = parent;

		RecalculateMaxEndLocal(node);
		RecalculateMaxEndLocal(pivot);
		RecalculateMaxEndUpwards(parent);
	}

	/// <summary>
	/// General left rotation
	/// </summary>
	/// <param name="node">top of rotated subtree</param>
	private void RotateLeft(Node<T> node)
	{
		var pivot = node.Right;
		var dir = GetParentDirection(node);
		var parent = node.Parent;
		var tempTree = pivot.Left;

		pivot.Left = node;
		node.Parent = pivot;
		node.Right = tempTree;

		if (tempTree != Sentinel)
		{
			tempTree.Parent = node;
		}

		if (dir == Direction.LEFT)
		{
			parent.Right = pivot;
		}
		else if (dir == Direction.RIGHT)
		{
			parent.Left = pivot;
		}
		else
		{
			Root = pivot;
		}

		pivot.Parent = parent;

		RecalculateMaxEndLocal(node);
		RecalculateMaxEndLocal(pivot);
		RecalculateMaxEndUpwards(parent);
	}

	/// <summary>
	/// Refreshes the MaxEnd value after node manipulation — local only
	/// </summary>
	private void RecalculateMaxEndLocal(Node<T> node)
	{
		var max = node.Interval.End;

		if (node.Right != Sentinel && node.Right.MaxEnd > max)
		{
			max = node.Right.MaxEnd;
		}

		if (node.Left != Sentinel && node.Left.MaxEnd > max)
		{
			max = node.Left.MaxEnd;
		}

		node.MaxEnd = max;
	}

	/// <summary>
	/// Walks up the tree recalculating MaxEnd, stopping early when no change is detected.
	/// RecalculateMaxEndLocal is inlined here to eliminate the per-iteration method call.
	/// </summary>
	private void RecalculateMaxEndUpwards(Node<T> node)
	{
		while (node != Sentinel)
		{
			var max = node.Interval.End;

			if (node.Right != Sentinel && node.Right.MaxEnd > max)
				max = node.Right.MaxEnd;

			if (node.Left != Sentinel && node.Left.MaxEnd > max)
				max = node.Left.MaxEnd;

			if (node.MaxEnd == max)
				break;

			node.MaxEnd = max;
			node = node.Parent;
		}
	}

	#region Enumerators

	private IEnumerable<Node<T>> InOrderWalk(Node<T> node)
	{
		if (node.Left != Sentinel)
		{
			foreach (var val in InOrderWalk(node.Left))
			{
				yield return val;
			}
		}

		if (node != Sentinel)
		{
			yield return node;
		}

		if (node.Right != Sentinel)
		{
			foreach (var val in InOrderWalk(node.Right))
			{
				yield return val;
			}
		}
	}

	public IEnumerator<T> GetEnumerator()
	{
		foreach (var val in InOrderWalk(Root))
		{
			yield return val.Value;
		}
	}

	#endregion Enumerators

	public override string ToString()
	{
		var sb = new StringBuilder();

		foreach (var node in InOrderWalk(Root))
		{
			sb.Append('[').Append(node.Interval.Start).Append('-').Append(node.Interval.End).Append("],");
		}

		sb.Length--;

		return sb.ToString();
	}
}
