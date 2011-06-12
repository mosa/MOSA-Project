/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Performs dominance calculations on basic Blocks built by a previous compilation stage.
	/// </summary>
	/// <remarks>
	/// The stage exposes the IDominanceProvider interface for other compilation stages to allow
	/// them to use the calculated dominance properties.
	/// <para/>
	/// The implementation is based on "A Simple, Fast Dominance Algorithm" by Keith D. Cooper, 
	/// Timothy J. Harvey, and Ken Kennedy, Rice University in Houston, Texas, USA.
	/// </remarks>
	public sealed class DominanceCalculationStage : BaseMethodCompilerStage, IMethodCompilerStage, IDominanceProvider, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// Holds the dominance information of a block.
		/// </summary>
		private BasicBlock[] _doms;

		/// <summary>
		/// Holds the dominance frontier Blocks.
		/// </summary>
		private BasicBlock[] _domFrontier;

		/// <summary>
		/// Holds the dominance frontier of individual Blocks.
		/// </summary>
		private BasicBlock[][] _domFrontierOfBlock;

		#endregion // Data members

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name { get { return @"DominanceCalculationStage"; } }

		#endregion // IPipelineStage Members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			CalculateDominance();
			CalculateDominanceFrontier();
		}

		/// <summary>
		/// Calculates the immediate dominance of all Blocks in the block provider.
		/// </summary>
		private void CalculateDominance()
		{
			// Changed flag
			bool changed = true;

			// Blocks in reverse post order topology
			BasicBlock[] revPostOrder = ReversePostorder(basicBlocks);

			// Allocate a dominance array
			_doms = new BasicBlock[basicBlocks.Count];
			_doms[0] = basicBlocks[0];

			// Calculate the dominance
			while (changed)
			{
				changed = false;
				foreach (BasicBlock b in revPostOrder)
				{
					if (b != null)
					{
						BasicBlock idom = b.PreviousBlocks[0];
						//Debug.Assert(-1 !=  Array.IndexOf(_doms, idom));

						for (int idx = 1; idx < b.PreviousBlocks.Count; idx++)
						{
							BasicBlock p = b.PreviousBlocks[idx];
							if (null != _doms[p.Sequence])
								idom = Intersect(p, idom);
						}

						if (!ReferenceEquals(_doms[b.Sequence], idom))
						{
							_doms[b.Sequence] = idom;
							changed = true;
						}
					}
				}
			}
		}

		/// <summary>
		/// Calculates the dominance frontier of all Blocks in the block list.
		/// </summary>
		private void CalculateDominanceFrontier()
		{
			List<BasicBlock> domFrontier = new List<BasicBlock>();
			List<BasicBlock>[] domFrontiers = new List<BasicBlock>[basicBlocks.Count];

			foreach (BasicBlock b in basicBlocks)
			{
				if (b.PreviousBlocks.Count > 1)
				{
					foreach (BasicBlock p in b.PreviousBlocks)
					{
						BasicBlock runner = p;
						while (runner != null && !ReferenceEquals(runner, _doms[b.Sequence]))
						{
							List<BasicBlock> runnerFrontier = domFrontiers[runner.Sequence];
							if (runnerFrontier == null)
								runnerFrontier = domFrontiers[runner.Sequence] = new List<BasicBlock>();

							if (!domFrontier.Contains(b))
								domFrontier.Add(b);
							runnerFrontier.Add(b);
							runner = _doms[runner.Sequence];
						}
					}
				}
			}

			int idx = 0;
			_domFrontierOfBlock = new BasicBlock[basicBlocks.Count][];
			foreach (List<BasicBlock> frontier in domFrontiers)
			{
				if (frontier != null)
					_domFrontierOfBlock[idx] = frontier.ToArray();
				idx++;
			}

			_domFrontier = domFrontier.ToArray();
		}

		#endregion // IMethodCompilerStage Members

		#region IDominanceProvider Members

		BasicBlock IDominanceProvider.GetImmediateDominator(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			Debug.Assert(block.Sequence < _doms.Length, @"Invalid block index.");

			if (block.Sequence >= _doms.Length)
				throw new ArgumentException(@"Invalid block index.", @"block");

			return _doms[block.Sequence];
		}

		BasicBlock[] IDominanceProvider.GetDominators(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			Debug.Assert(block.Sequence < _doms.Length, @"Invalid block index.");

			if (block.Sequence >= _doms.Length)
				throw new ArgumentException(@"Invalid block index.", @"block");

			// Return value
			BasicBlock[] result;
			// Counter
			int count, idx = block.Sequence;

			// Count the dominators first
			for (count = 1; 0 != idx; count++)
				idx = _doms[idx].Sequence;

			// Allocate a dominator array
			result = new BasicBlock[count + 1];
			result[0] = block;
			for (idx = block.Sequence, count = 1; 0 != idx; idx = _doms[idx].Sequence)
				result[count++] = _doms[idx];
			result[count] = _doms[0];

			return result;
		}

		BasicBlock[] IDominanceProvider.GetDominanceFrontier()
		{
			return _domFrontier;
		}

		BasicBlock[] IDominanceProvider.GetDominanceFrontierOfBlock(BasicBlock block)
		{
			if (block == null)
				throw new ArgumentNullException(@"block");

			return _domFrontierOfBlock[block.Sequence];
		}

		#endregion // IDominanceProvider Members

		#region Internals

		/// <summary>
		/// Retrieves the highest common immediate dominator of the two given Blocks.
		/// </summary>
		/// <param name="b1">The first basic block.</param>
		/// <param name="b2">The second basic block.</param>
		/// <returns>The highest common dominator.</returns>
		private BasicBlock Intersect(BasicBlock b1, BasicBlock b2)
		{
			BasicBlock f1 = b1, f2 = b2;

			while (f2 != null && f1 != null && f1.Sequence != f2.Sequence)
			{
				while (f1 != null && f1.Sequence > f2.Sequence)
					f1 = _doms[f1.Sequence];

				while (f2 != null && f1 != null && f2.Sequence > f1.Sequence)
					f2 = _doms[f2.Sequence];
			}

			return f1;
		}

		private BasicBlock[] ReversePostorder(IList<BasicBlock> blocks)
		{
			BasicBlock[] result = new BasicBlock[blocks.Count - 1];
			int idx = 0;
			Queue<BasicBlock> workList = new Queue<BasicBlock>(blocks.Count);

			// Add next blocks
			foreach (BasicBlock next in blocks[0].NextBlocks)
				workList.Enqueue(next);

			while (workList.Count != 0)
			{
				BasicBlock current = workList.Dequeue();
				if (Array.IndexOf(result, current) == -1)
				{
					result[idx++] = current;
					foreach (BasicBlock next in current.NextBlocks)
						workList.Enqueue(next);
				}
			}

			return result;
		}

		#endregion // Internals
	}
}
