// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
	/// <summary>
	/// Tree capable of adding arbitrary intervals and performing search queries on them
	/// </summary>
	public sealed class IntervalTree<T>
	{
		private enum NodeColor
		{
			RED,
			BLACK
		}

		private enum NodeDirection
		{
			LEFT,
			RIGHT,
			NONE
		}

		/// <summary>
		/// Node of interval Tree
		/// </summary>
		/// <typeparam name="N">type of interval bounds</typeparam>
		private class IntervalNode<N>
		{
			public IntervalNode<N> Left { get; set; }
			public IntervalNode<N> Right { get; set; }
			public IntervalNode<N> Parent { get; set; }

			/// <summary>
			/// Maximum "end" value of interval in node subtree
			/// </summary>
			public int MaxEnd { get; set; }

			public Interval Interval;

			/// <summary>
			/// The interval this node holds
			/// </summary>
			public N Value;

			/// <summary>
			/// Color of the node used for R-B implementation
			/// </summary>
			public NodeColor Color { get; set; }

			public IntervalNode()
			{
				//Parent = Left = Right = Sentinel;
				Color = NodeColor.BLACK;
			}

			public IntervalNode(Interval interval, N value) : this()
			{
				MaxEnd = interval.End;
				Interval = interval;
				Value = value;
			}

			//public int CompareTo(IntervalNode<T> other)
			//{
			//	return Interval.CompareTo(other.Interval);
			//}
		}

		private IntervalNode<T> Sentinel = new IntervalNode<T>();

		private IntervalNode<T> Root { get; set; }

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

		public bool Contains(Interval interval)
		{
			return SearchSubtree(Root, interval);
		}

		public bool Contains(int val)
		{
			return SearchSubtree(Root, val);
		}

		/// <summary>
		/// Search interval tree for a given point
		/// </summary>
		/// <param name="val">value to be searched for</param>
		/// <returns>list of intervals which contain the value</returns>
		public List<T> Search(int val)
		{
			var result = new List<T>();
			SearchSubtree(Root, val, result);
			return result;
		}

		/// <summary>
		/// Search interval tree for intervals overlapping with given
		/// </summary>
		/// <param name="interval"></param>
		/// <returns></returns>
		public List<T> Search(Interval interval)
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
		public T SearchFirstOverlapping(Interval interval)
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

		public T SearchFirstOverlapping(int val)
		{
			var node = Root;

			while (node != Sentinel && !node.Interval.Overlaps(val))
			{
				if (node.Left != Sentinel && node.Left.MaxEnd.CompareTo(val) > 0)
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

		private void SearchSubtree(IntervalNode<T> node, Interval interval, List<T> result)
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

		private void SearchSubtree(IntervalNode<T> node, int val, List<T> result)
		{
			if (node == Sentinel)
			{
				return;
			}

			if (node.Left != Sentinel)
			{
				SearchSubtree(node.Left, val, result);
			}

			if (node.Interval.Contains(val))
			{
				result.Add(node.Value);
			}

			// Interval start is greater than largest endpoint in this subtree
			if (node.Right != Sentinel && val.CompareTo(node.MaxEnd) < 0)
			{
				SearchSubtree(node.Right, val, result);
			}
		}

		private bool SearchSubtree(IntervalNode<T> node, Interval interval)
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

		private bool SearchSubtree(IntervalNode<T> node, int val)
		{
			if (node == Sentinel)
			{
				return false;
			}

			if (node.Left != Sentinel)
			{
				if (SearchSubtree(node.Left, val))
					return true;
			}

			if (node.Interval.Contains(val))
			{
				return true;
			}

			// Interval start is greater than largest endpoint in this subtree
			if (node.Right != Sentinel && val.CompareTo(node.MaxEnd) < 0)
			{
				return SearchSubtree(node.Right, val);
			}

			return false;
		}

		private IntervalNode<T> FindInterval(IntervalNode<T> tree, Interval interval)
		{
			while (tree != Sentinel)
			{
				if (tree.Interval.CompareTo(interval) > 0)
				{
					tree = tree.Left;
					continue;
				}

				if (tree.Interval.CompareTo(interval) < 0)
				{
					tree = tree.Right;
					continue;
				}

				if (tree.Interval.CompareTo(interval) == 0)
				{
					return tree;
				}
			}

			return Sentinel;
		}

		#endregion Tree searching

		/// <summary>
		/// Insert new interval to interval tree
		/// </summary>
		/// <param name="interval">interval to add</param>
		public void Add(Interval interval, T value)
		{
			Debug.Assert(!Contains(interval));

			var node = new IntervalNode<T>(interval, value)
			{
				Parent = Sentinel,
				Left = Sentinel,
				Right = Sentinel
			};

			if (Root == Sentinel)
			{
				node.Color = NodeColor.BLACK;
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
		private void InsertInterval(IntervalNode<T> node, IntervalNode<T> currentNode)
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
					node.Color = NodeColor.RED;

					currentNode.Left = node;
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
					node.Color = NodeColor.RED;

					currentNode.Right = node;
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

			Root.Color = NodeColor.BLACK;
		}

		/// <summary>
		/// The direction of the parent, from the child point-of-view
		/// </summary>
		private NodeDirection GetParentDirection(IntervalNode<T> node)
		{
			if (node.Parent == Sentinel)
			{
				return NodeDirection.NONE;
			}

			return node.Parent.Left == node ? NodeDirection.RIGHT : NodeDirection.LEFT;
		}

		private IntervalNode<T> GetSuccessor(IntervalNode<T> node)
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

		private IntervalNode<T> GetGrandParent(IntervalNode<T> node)
		{
			if (node.Parent != Sentinel)
			{
				return node.Parent.Parent;
			}
			return Sentinel;
		}

		private IntervalNode<T> GetUncle(IntervalNode<T> node)
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
		private void RenewConstraintsAfterInsert(IntervalNode<T> node)
		{
			if (node.Parent == Sentinel)
			{
				return;
			}

			if (node.Parent.Color == NodeColor.BLACK)
			{
				return;
			}

			var uncle = GetUncle(node);

			if (uncle != Sentinel && uncle.Color == NodeColor.RED)
			{
				node.Parent.Color = uncle.Color = NodeColor.BLACK;

				var grandparent = GetGrandParent(node);
				if (grandparent != Sentinel && grandparent.Parent != Sentinel)
				{
					grandparent.Color = NodeColor.RED;
					RenewConstraintsAfterInsert(grandparent);
				}
			}
			else
			{
				var parentDirection = GetParentDirection(node);
				var parentParentDirection = GetParentDirection(node.Parent);

				if (parentDirection == NodeDirection.LEFT && parentParentDirection == NodeDirection.RIGHT)
				{
					RotateLeft(node.Parent);
					node = node.Left;
				}
				else if (parentDirection == NodeDirection.RIGHT && parentParentDirection == NodeDirection.LEFT)
				{
					RotateRight(node.Parent);
					node = node.Right;
				}

				node.Parent.Color = NodeColor.BLACK;

				var grandparent = GetGrandParent(node);

				if (grandparent == Sentinel)
				{
					return;
				}
				grandparent.Color = NodeColor.RED;

				if (GetParentDirection(node) == NodeDirection.RIGHT)
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
		public void Remove(Interval interval)
		{
			RemoveNode(FindInterval(Root, interval));
		}

		private void RemoveNode(IntervalNode<T> node)
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
				if (GetParentDirection(node) == NodeDirection.RIGHT)
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

			if (node.Color == NodeColor.BLACK)
			{
				RenewConstraintsAfterDelete(temp);
			}
		}

		/// <summary>
		/// Ensures constraints still apply after node deletion
		/// - made with the help of algorithm from Cormen et Al. Introduction to Algorithms 2nd ed.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RenewConstraintsAfterDelete(IntervalNode<T> node)
		{
			// Need to bubble up and fix
			while (node != Root && node.Color == NodeColor.BLACK)
			{
				if (GetParentDirection(node) == NodeDirection.RIGHT)
				{
					var aux = node.Parent.Right;
					if (aux.Color == NodeColor.RED)
					{
						aux.Color = NodeColor.BLACK;
						node.Parent.Color = NodeColor.RED;
						RotateLeft(node.Parent);
						aux = node.Parent.Right;
					}

					if (aux.Left.Color == NodeColor.BLACK && aux.Right.Color == NodeColor.BLACK)
					{
						aux.Color = NodeColor.RED;
						node = node.Parent;
					}
					else
					{
						if (aux.Right.Color == NodeColor.BLACK)
						{
							aux.Left.Color = NodeColor.BLACK;
							aux.Color = NodeColor.RED;
							RotateRight(aux);
							aux = node.Parent.Right;
						}

						aux.Color = node.Parent.Color;
						node.Parent.Color = NodeColor.BLACK;
						aux.Right.Color = NodeColor.BLACK;
						RotateLeft(node.Parent);
						node = Root;
					}
				}
				else
				{
					var aux = node.Parent.Left;
					if (aux.Color == NodeColor.RED)
					{
						aux.Color = NodeColor.BLACK;
						node.Parent.Color = NodeColor.RED;
						RotateRight(node.Parent);
						aux = node.Parent.Left;
					}

					if (aux.Left.Color == NodeColor.BLACK && aux.Right.Color == NodeColor.BLACK)
					{
						aux.Color = NodeColor.RED;
						node = node.Parent;
					}
					else
					{
						if (aux.Left.Color == NodeColor.BLACK)
						{
							aux.Right.Color = NodeColor.BLACK;
							aux.Color = NodeColor.RED;
							RotateLeft(aux);
							aux = node.Parent.Left;
						}

						aux.Color = node.Parent.Color;
						node.Parent.Color = NodeColor.BLACK;
						aux.Left.Color = NodeColor.BLACK;
						RotateRight(node.Parent);
						node = Root;
					}
				}
			}

			node.Color = NodeColor.BLACK;
		}

		/// <summary>
		/// General right rotation
		/// </summary>
		/// <param name="node">Top of rotated subtree</param>
		private void RotateRight(IntervalNode<T> node)
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

			if (dir == NodeDirection.LEFT)
			{
				parent.Right = pivot;
			}
			else if (dir == NodeDirection.RIGHT)
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
		private void RotateLeft(IntervalNode<T> node)
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

			if (dir == NodeDirection.LEFT)
			{
				parent.Right = pivot;
			}
			else if (dir == NodeDirection.RIGHT)
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
		private void RecalculateMaxEnd(IntervalNode<T> node)
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

		private IEnumerable<IntervalNode<T>> InOrderWalk(IntervalNode<T> node)
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
