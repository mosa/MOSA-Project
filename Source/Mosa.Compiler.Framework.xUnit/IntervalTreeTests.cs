// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree;
using System.Diagnostics;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit
{
	public class IntervalTreeTests
	{
		[Fact]
		public void Insert()
		{
			var tree = new IntervalTree<object>();

			for (int i = 0; i < 100; i++)
			{
				tree.Add(i * 2, i * 2 + 1, i);

				for (int n = 0; n <= i; n++)
				{
					Debug.Assert(tree.Contains(n * 2));
					Debug.Assert(tree.Contains(n * 2, n * 2 + 1));
				}
			}
		}

		[Fact]
		public void Delete()
		{
			var tree = new IntervalTree<object>();

			for (int i = 0; i < 100; i++)
			{
				tree.Add(i * 2, i * 2 + 1, i);
			}

			for (int i = 0; i < 100; i += 2)
			{
				tree.Remove(i * 2, i * 2 + 1);
				Debug.Assert(!tree.Contains(i * 2));
			}
		}

		[Fact]
		public void MixedAddDelete()
		{
			var tree = new IntervalTree<object>();

			int i = 0;

			tree.Add(1, 2, ++i);
			Debug.Assert(tree.Contains(1));
			tree.Remove(1, 2);
			Debug.Assert(!tree.Contains(1));
			Debug.Assert(!tree.Contains(2));
			tree.Add(1, 2, ++i);
			Debug.Assert(tree.Contains(1));
			tree.Remove(1, 2);
			Debug.Assert(!tree.Contains(1));
			tree.Add(1, 2, ++i);
			Debug.Assert(tree.Contains(1));
			tree.Add(3, 4, ++i);
			Debug.Assert(tree.Contains(3));
			tree.Remove(1, 2);
			Debug.Assert(!tree.Contains(1));
			Debug.Assert(!tree.Contains(2));
			tree.Remove(3, 4);
			Debug.Assert(!tree.Contains(3));
		}
	}
}
