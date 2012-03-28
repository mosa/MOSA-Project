/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Mosa.Compiler.Framework
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

		/// <summary>
		/// 
		/// </summary>
		private List<BasicBlock>[] _children;

		#endregion // Data members

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (AreExceptions)
				return;

			var entryBlock = this.FindBlock(-1);
			var exitBlock = this.FindBlock(int.MaxValue);

			entryBlock.NextBlocks.Add(exitBlock);
			exitBlock.PreviousBlocks.Add(entryBlock);

			CalculateDominance();
			CalculateChildren();
			CalculateDominanceFrontier();

			entryBlock.NextBlocks.Remove(exitBlock);
			exitBlock.PreviousBlocks.Remove(entryBlock);
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
			_children = new List<BasicBlock>[basicBlocks.Count];
			_doms[0] = this.FindBlock(-1);

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

		private void CalculateChildren()
		{
			foreach (var x in this.basicBlocks)
			{
				var immediateDominator = (this as IDominanceProvider).GetImmediateDominator(x);
				if (immediateDominator == null)
					continue;

				if (this._children[immediateDominator.Sequence] == null)
					this._children[immediateDominator.Sequence] = new List<BasicBlock>();

				if (!this._children[immediateDominator.Sequence].Contains(x) && x != immediateDominator)
					this._children[immediateDominator.Sequence].Add(x);
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
							if (!runnerFrontier.Contains(b))
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
					_domFrontierOfBlock[idx++] = frontier.ToArray();
				else
					_domFrontierOfBlock[idx++] = new BasicBlock[0];
			}

			_domFrontier = domFrontier.ToArray();

			/*_domFrontierOfBlock = new BasicBlock[basicBlocks.Count][];
			foreach (var block in this.basicBlocks)
			{
				var result = new List<BasicBlock>();
				var dominatorChildren = this._children[block.Sequence];
				if (dominatorChildren != null)
				foreach (var child in dominatorChildren)
				{
					if (this._children[child.Sequence] != null)
						result.AddRange(this._children[child.Sequence]);
				}
				_domFrontierOfBlock[block.Sequence] = result.ToArray();
			}
			_domFrontier = null;*/
		}

		/// <summary>
		/// Iterateds the dominance frontier.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		List<BasicBlock> IDominanceProvider.IteratedDominanceFrontier(List<BasicBlock> s)
		{
			var result = new List<BasicBlock>();
			var workList = new Queue<BasicBlock>();

			foreach (var block in s)
			{
				//result.Add(block);
				workList.Enqueue(block);
			}

			while (workList.Count > 0)
			{
				var n = workList.Dequeue();
				var dominanceFrontier = (this as IDominanceProvider).GetDominanceFrontierOfBlock(n);
				if (dominanceFrontier == null)
					continue;

				foreach (var c in dominanceFrontier)
				{
					if (!result.Contains(c))
					{
						result.Add(c);
						workList.Enqueue(c);
					}
				}
			}
			return result;
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


		public BasicBlock[] GetChildren(BasicBlock block)
		{
			if (this._children[block.Sequence] == null)
				return new BasicBlock[0];
			return this._children[block.Sequence].ToArray();
		}
	}
}
