// Copyright (c) MOSA Project. Licensed under the New BSD License.


namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
	public sealed partial class IntervalTree<T>
	{
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
	}
}
