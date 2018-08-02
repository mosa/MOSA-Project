// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	/// The Loop Aware Block Ordering reorders blocks to optimize loops and reduce the distance of jumps and branches.
	/// </summary>
	public sealed class LoopAwareBlockOrder : BaseBlockOrder
	{
		#region Data Members

		private BasicBlocks basicBlocks;
		private BitArray active;
		private BitArray visited;
		private BitArray loopHeader;
		private List<BasicBlock> loopEnds;
		private int loopCount;
		private int[] forwardBranchesCount;
		private int[] loopBlockIndex;
		private BitArray loopMap;
		private int blockCount;

		private int[] loopDepth;
		private int[] loopIndex;

		private HashSet<BasicBlock> orderSet;

		#endregion Data Members

		#region Priority class

		/// <summary>
		/// Priority
		/// </summary>
		private class Priority : IComparable<Priority>
		{
			public int Depth;
			public int Order;

			/// <summary>
			/// Initializes a new instance of the <see cref="Priority" /> class.
			/// </summary>
			/// <param name="depth">The depth.</param>
			/// <param name="order">The order.</param>
			public Priority(int depth, int order)
			{
				Depth = depth;
				Order = order;
			}

			/// <summary>
			/// Compares the current object with another object of the same type.
			/// </summary>
			/// <param name="other">An object to compare with this object.</param>
			/// <returns>
			/// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
			/// Value
			/// Meaning
			/// Less than zero
			/// This object is less than the <paramref name="other"/> parameter.
			/// Zero
			/// This object is equal to <paramref name="other"/>.
			/// Greater than zero
			/// This object is greater than <paramref name="other"/>.
			/// </returns>
			public int CompareTo(Priority other)
			{
				if (Depth > other.Depth)
					return 1;
				if (Depth < other.Depth)
					return -1;
				if (Order < other.Order)
					return 1;
				if (Order > other.Order)
					return -1;
				return 0;
			}
		}

		#endregion Priority class

		/// <summary>
		/// Initializes a new instance of the <see cref="LoopAwareBlockOrder"/> class.
		/// </summary>
		public LoopAwareBlockOrder()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LoopAwareBlockOrder"/> class.
		/// </summary>
		/// <param name="basicBlocks">The basic blocks.</param>
		public LoopAwareBlockOrder(BasicBlocks basicBlocks)
		{
			Analyze(basicBlocks);
		}

		public override int GetLoopDepth(BasicBlock block)
		{
			return loopDepth[block.Sequence];
		}

		public override int GetLoopIndex(BasicBlock block)
		{
			return loopIndex[block.Sequence];
		}

		public override void Analyze(BasicBlocks basicBlocks)
		{
			this.basicBlocks = basicBlocks;

			blockCount = basicBlocks.Count;
			loopEnds = new List<BasicBlock>();
			loopCount = 0;
			loopHeader = new BitArray(blockCount, false);
			forwardBranchesCount = new int[blockCount];
			loopBlockIndex = new int[blockCount];
			loopDepth = new int[blockCount];
			loopIndex = new int[blockCount];
			NewBlockOrder = new List<BasicBlock>(blockCount);
			orderSet = new HashSet<BasicBlock>();

			foreach (var head in basicBlocks.HeadBlocks)
			{
				Start(head);
			}

			loopHeader = null;
			loopEnds = null;
			loopHeader = null;
			loopBlockIndex = null;
			forwardBranchesCount = null;
			orderSet = null;
		}

		#region Members

		/// <summary>
		/// Determines the loop depths.
		/// </summary>
		/// <param name="start">The start.</param>
		private void Start(BasicBlock start)
		{
			StartCountEdges(start);
			loopMap = new BitArray(loopCount * blockCount, false);
			MarkLoops();
			AssignLoopDepth(start);
			ComputeOrder(start);
		}

		private void StartCountEdges(BasicBlock start)
		{
			active = new BitArray(blockCount, false);
			visited = new BitArray(blockCount, false);

			CountEdges(start, null);
		}

		private void CountEdges(BasicBlock current, BasicBlock parent)
		{
			int blockId = current.Sequence;

			if (active.Get(blockId))
			{
				Debug.Assert(visited.Get(blockId));
				Debug.Assert(parent != null);

				loopHeader.Set(blockId, true);
				loopEnds.Add(parent);

				return;
			}

			forwardBranchesCount[blockId]++;

			if (visited.Get(blockId))
				return;

			visited.Set(blockId, true);
			active.Set(blockId, true);

			foreach (var next in current.NextBlocks)
			{
				CountEdges(next, current);
			}

			Debug.Assert(active.Get(blockId));
			active.Set(blockId, false);

			if (loopHeader.Get(blockId))
			{
				loopBlockIndex[blockId] = loopCount;
				loopCount++;
			}
		}

		#region Helpers

		private void SetLoopMap(int l, BasicBlock b)
		{
			loopMap.Set((l * blockCount) + b.Sequence, true);
		}

		private bool GetLoopMap(int l, BasicBlock b)
		{
			return loopMap.Get((l * blockCount) + b.Sequence);
		}

		#endregion Helpers

		private void MarkLoops()
		{
			if (loopCount == 0)
				return;

			var worklist = new Stack<BasicBlock>();

			foreach (var loopEnd in loopEnds)
			{
				var loopStart = loopEnd.NextBlocks[0]; // assuming the first one?
				int loopStartIndex = loopBlockIndex[loopStart.Sequence];

				worklist.Push(loopEnd);

				SetLoopMap(loopStartIndex, loopEnd);

				while (worklist.Count != 0)
				{
					var current = worklist.Pop();

					if (current == loopStart)
						continue;

					foreach (var previous in current.PreviousBlocks)
					{
						if (!GetLoopMap(loopStartIndex, previous))
						{
							worklist.Push(previous);
							SetLoopMap(loopStartIndex, previous);
						}
					}
				}
			}
		}

		private void AssignLoopDepth(BasicBlock start)
		{
			visited = new BitArray(blockCount, false);

			var worklist = new Stack<BasicBlock>();

			worklist.Push(start);

			while (worklist.Count != 0)
			{
				var current = worklist.Pop();

				if (!visited.Get(current.Sequence))
				{
					visited.Set(current.Sequence, true);

					int currentLoopDepth = 0;
					int minLoopIndex = -1;
					for (int i = loopCount - 1; i >= 0; i--)
					{
						if (GetLoopMap(i, current))
						{
							currentLoopDepth++;
							minLoopIndex = i;
						}
					}

					loopDepth[current.Sequence] = currentLoopDepth;
					loopIndex[current.Sequence] = minLoopIndex;

					foreach (var next in current.NextBlocks)
					{
						worklist.Push(next);
					}
				}
			}
		}

		private void ComputeOrder(BasicBlock start)
		{
			// Create sorted worklist
			var workList = new SortedList<Priority, BasicBlock>
			{
				// Start worklist with first block
				[new Priority(0, 0)] = start
			};

			while (workList.Count != 0)
			{
				var block = workList.Values[workList.Count - 1];
				workList.RemoveAt(workList.Count - 1);

				if (orderSet.Contains(block))
					continue;

				orderSet.Add(block);
				NewBlockOrder.Add(block);

				foreach (var successor in block.NextBlocks)
				{
					forwardBranchesCount[successor.Sequence]--;

					if (forwardBranchesCount[successor.Sequence] == 0)
					{
						workList.Add(new Priority(loopDepth[successor.Sequence], successor.Sequence), successor);
					}
				}
			}
		}

		#endregion Members
	}
}
