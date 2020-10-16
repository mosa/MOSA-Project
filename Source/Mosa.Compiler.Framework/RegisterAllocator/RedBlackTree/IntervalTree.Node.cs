// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
	public sealed partial class IntervalTree<T>
	{
		/// <summary>
		/// Node of interval Tree
		/// </summary>
		/// <typeparam name="N">type of interval bounds</typeparam>
		private class Node<N> where N : class
		{
			public Node<N> Left;
			public Node<N> Right;
			public Node<N> Parent;

			/// <summary>
			/// Maximum "end" value of interval in node subtree
			/// </summary>
			public int MaxEnd;

			/// <summary>
			/// The interval
			/// </summary>
			public Interval Interval;

			/// <summary>
			/// The interval this node holds
			/// </summary>
			public N Value;

			/// <summary>
			/// Color of the node used for R-B implementation
			/// </summary>
			public Color Color;

			public Node()
			{
				Color = Color.BLACK;
			}

			public Node(Interval interval, N value)
			{
				Color = Color.BLACK;
				MaxEnd = interval.End;
				Interval = interval;
				Value = value;
			}

			//public int CompareTo(IntervalNode<T> other)
			//{
			//	return Interval.CompareTo(other.Interval);
			//}

			//public bool IsSentinel { get { return Value == null; } }
		}
	}
}
