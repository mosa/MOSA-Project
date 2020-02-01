// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
	/// <summary>
	/// Tree capable of adding arbitrary intervals and performing search queries on them
	/// </summary>
	public sealed partial class IntervalTree<T> where T : class
	{
		private Node<T> Sentinel = new Node<T>(new Interval(-1, -1), null);

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
		/// Search interval tree for intervals overlapping with given
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public List<T> Search(int start, int end)
		{
			var result = new List<T>();
			SearchSubtree(Root, new Interval(start, end), result);
			return result;
		}

		/// <summary>
		/// Search interval tree for intervals overlapping with given
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		private List<T> Search(Interval interval)
		{
			var result = new List<T>();
			SearchSubtree(Root, interval, result);
			return result;
		}

		/// <summary>
		/// Searches for the first overlapping interval
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public T SearchFirstOverlapping(int start, int end)
		{
			return SearchFirstOverlapping(new Interval(start, end));
		}

		private T SearchFirstOverlapping(Interval interval)
		{
			var node = Root;

			while (node != Sentinel && !node.Interval.Overlaps(interval))
			{
				if (node.Left != Sentinel && node.Left.MaxEnd.CompareTo(interval.Start) > 0)
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
				throw new KeyNotFoundException("No overlapping interval found.");
			}

			return node.Value;
		}

		public T SearchFirstOverlapping(int at)
		{
			var node = Root;

			while (node != Sentinel && !node.Interval.Overlaps(at))
			{
				if (node.Left != Sentinel && node.Left.MaxEnd.CompareTo(at) > 0)
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
				throw new KeyNotFoundException("No overlapping interval found.");
			}

			return node.Value;
		}

		private void SearchSubtree(Node<T> node, Interval interval, List<T> result)
		{
			if (node == Sentinel)
			{
				return;
			}

			if (node.Left != Sentinel)
			{
				SearchSubtree(node.Left, interval, result);
			}

			if (interval.Overlaps(node.Interval))
			{
				result.Add(node.Value);
			}

			// Interval start is greater than largest endpoint in this subtree
			if (node.Right != Sentinel && interval.Start.CompareTo(node.MaxEnd) < 0)
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

			if (node.Left != Sentinel)
			{
				SearchSubtree(node.Left, at, result);
			}

			if (node.Interval.Contains(at))
			{
				result.Add(node.Value);
			}

			// Interval start is greater than largest endpoint in this subtree
			if (node.Right != Sentinel && at.CompareTo(node.MaxEnd) < 0)
			{
				SearchSubtree(node.Right, at, result);
			}
		}

		private bool SearchSubtree(Node<T> node, Interval interval)
		{
			if (node == Sentinel)
			{
				return false;
			}

			if (node.Left != Sentinel)
			{
				if (SearchSubtree(node.Left, interval))
					return true;
			}

			if (interval.Overlaps(node.Interval))
			{
				return true;
			}

			// Interval start is greater than largest endpoint in this subtree
			if (node.Right != Sentinel && interval.Start.CompareTo(node.MaxEnd) < 0)
			{
				return SearchSubtree(node.Right, interval);
			}

			return false;
		}

		private bool SearchSubtree(Node<T> node, int at)
		{
			if (node == Sentinel)
			{
				return false;
			}

			if (node.Left != Sentinel)
			{
				if (SearchSubtree(node.Left, at))
					return true;
			}

			if (node.Interval.Contains(at))
			{
				return true;
			}

			// Interval start is greater than largest endpoint in this subtree
			if (node.Right != Sentinel && at.CompareTo(node.MaxEnd) < 0)
			{
				return SearchSubtree(node.Right, at);
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
					continue;
				}

				if (val < 0)
				{
					tree = tree.Right;
					continue;
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
			var addedNode = Sentinel;

			var compare = node.Interval.CompareTo(currentNode.Interval);

			if (compare < 0)
			{
				if (currentNode.Left == Sentinel)
				{
					node.Parent = Sentinel;
					node.Left = Sentinel;
					node.Right = Sentinel;
					node.Color = Color.RED;
					addedNode = node;

					currentNode.Left = addedNode;
					addedNode.Parent = currentNode;
				}
				else
				{
					InsertInterval(node, currentNode.Left);
					return;
				}
			}
			else if (compare > 0)
			{
				if (currentNode.Right == Sentinel)
				{
					node.Parent = Sentinel;
					node.Left = Sentinel;
					node.Right = Sentinel;
					node.Color = Color.RED;
					addedNode = node;

					currentNode.Right = addedNode;
					addedNode.Parent = currentNode;
				}
				else
				{
					InsertInterval(node, currentNode.Right);
					return;
				}
			}
			else
			{
				return;
			}

			RecalculateMaxEnd(addedNode.Parent);

			RenewConstraintsAfterInsert(addedNode);

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
		/// Validates and applies RB-tree constraints to node
		/// </summary>
		/// <param name="node">node to be validated and fixed</param>
		private void RenewConstraintsAfterInsert(Node<T> node)
		{
			if (node.Parent == Sentinel)
			{
				return;
			}

			if (node.Parent.Color == Color.BLACK)
			{
				return;
			}

			var uncle = GetUncle(node);

			if (uncle != Sentinel && uncle.Color == Color.RED)
			{
				node.Parent.Color = uncle.Color = Color.BLACK;

				var grandparent = GetGrandParent(node);
				if (grandparent != Sentinel && grandparent.Parent != Sentinel)
				{
					grandparent.Color = Color.RED;
					RenewConstraintsAfterInsert(grandparent);
				}
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
				{
					return;
				}
				grandparent.Color = Color.RED;

				if (GetParentDirection(node) == Direction.RIGHT)
				{
					RotateRight(grandparent);
				}
				else
				{
					RotateLeft(grandparent);
				}
			}
		}

		#endregion Tree insertion internals

		/// <summary>
		/// Removes interval from tree (if present in tree)
		/// </summary>
		/// <param name="?"></param>
		public void Remove(int start, int end)
		{
			RemoveNode(FindInterval(Root, new Interval(start, end)));
		}

		/// <summary>
		/// Replaces interval with new value
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		/// <param name="value">The value.</param>
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

				RecalculateMaxEnd(node);
				while (node.Parent != Sentinel)
				{
					node = node.Parent;
					RecalculateMaxEnd(node);
				}
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

				var maxAux = node.Parent;
				RecalculateMaxEnd(maxAux);

				while (maxAux.Parent != Sentinel)
				{
					maxAux = maxAux.Parent;
					RecalculateMaxEnd(maxAux);
				}
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
		/// <param name="node">The node.</param>
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

			RecalculateMaxEnd(pivot);
			RecalculateMaxEnd(node);
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

			RecalculateMaxEnd(pivot);
			RecalculateMaxEnd(node);
		}

		/// <summary>
		/// Refreshes the MaxEnd value after node manipulation
		/// This is a local operation only
		/// </summary>
		private void RecalculateMaxEnd(Node<T> node)
		{
			int max = node.Interval.End;

			if (node.Right != Sentinel)
			{
				if (node.Right.MaxEnd.CompareTo(max) > 0)
				{
					max = node.Right.MaxEnd;
				}
			}

			if (node.Left != Sentinel)
			{
				if (node.Left.MaxEnd.CompareTo(max) > 0)
				{
					max = node.Left.MaxEnd;
				}
			}

			node.MaxEnd = max;

			if (node.Parent != Sentinel)
			{
				RecalculateMaxEnd(node.Parent);
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
				sb.Append("[").Append(node.Interval.Start).Append("-").Append(node.Interval.End).Append("],");
			}

			sb.Length--;

			return sb.ToString();
		}
	}
}
