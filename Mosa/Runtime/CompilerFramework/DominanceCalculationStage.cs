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
	public sealed class DominanceCalculationStage : BaseStage, IMethodCompilerStage, IDominanceProvider
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

		#region IMethodCompilerStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		public string Name
		{
			get { return @"Dominance Calculation"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			CalculateDominance();
			CalculateDominanceFrontier();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pipeline"></param>
		public void SetPipelinePosition(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.RunAfter<IR.CilTransformationStage>(this);
		}

		/// <summary>
		/// Calculates the immediate dominance of all Blocks in the block provider.
		/// </summary>
		private void CalculateDominance()
		{
			// Changed flag
			bool changed = true;
			// Blocks in reverse post order topology
			BasicBlock[] revPostOrder = ReversePostorder(BasicBlocks);

			// Allocate a dominance array
			_doms = new BasicBlock[BasicBlocks.Count];
			_doms[0] = BasicBlocks[0];

			// Calculate the dominance
			while (changed) {
				changed = false;
				foreach (BasicBlock b in revPostOrder) {
					if (null != b) {
						BasicBlock idom = b.PreviousBlocks[0];
						//Debug.Assert(-1 !=  Array.IndexOf(_doms, idom));

						for (int idx = 1; idx < b.PreviousBlocks.Count; idx++) {
							BasicBlock p = b.PreviousBlocks[idx];
							if (null != _doms[p.Index])
								idom = Intersect(p, idom);
						}

						if (!ReferenceEquals(_doms[b.Index], idom)) {
							_doms[b.Index] = idom;
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
			List<BasicBlock>[] domFrontiers = new List<BasicBlock>[BasicBlocks.Count];
			foreach (BasicBlock b in BasicBlocks) {
				if (b.PreviousBlocks.Count > 1) {
					foreach (BasicBlock p in b.PreviousBlocks) {
						BasicBlock runner = p;
						while (runner != null && !ReferenceEquals(runner, _doms[b.Index])) {
							List<BasicBlock> runnerFrontier = domFrontiers[runner.Index];
							if (null == runnerFrontier)
								runnerFrontier = domFrontiers[runner.Index] = new List<BasicBlock>();

							if (!domFrontier.Contains(b))
								domFrontier.Add(b);
							runnerFrontier.Add(b);
							runner = _doms[runner.Index];
						}
					}
				}
			}

			Debug.WriteLine(@"Computed dominance frontiers");
			int idx = 0;
			_domFrontierOfBlock = new BasicBlock[BasicBlocks.Count][];
			foreach (List<BasicBlock> frontier in domFrontiers) {
				if (null != frontier)
					_domFrontierOfBlock[idx] = frontier.ToArray();
				idx++;
			}

			_domFrontier = domFrontier.ToArray();
		}

		#endregion // IMethodCompilerStage Members

		#region IDominanceProvider Members

		BasicBlock IDominanceProvider.GetImmediateDominator(BasicBlock block)
		{
			if (null == block)
				throw new ArgumentNullException(@"block");

			Debug.Assert(block.Index < _doms.Length, @"Invalid block index.");
			if (block.Index >= _doms.Length)
				throw new ArgumentException(@"Invalid block index.", @"block");

			return _doms[block.Index];
		}

		BasicBlock[] IDominanceProvider.GetDominators(BasicBlock block)
		{
			if (null == block)
				throw new ArgumentNullException(@"block");
			Debug.Assert(block.Index < _doms.Length, @"Invalid block index.");
			if (block.Index >= _doms.Length)
				throw new ArgumentException(@"Invalid block index.", @"block");

			// Return value
			BasicBlock[] result;
			// Counter
			int count, idx = block.Index;

			// Count the dominators first
			for (count = 1; 0 != idx; count++)
				idx = _doms[idx].Index;

			// Allocate a dominator array
			result = new BasicBlock[count + 1];
			result[0] = block;
			for (idx = block.Index, count = 1; 0 != idx; idx = _doms[idx].Index)
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
			if (null == block)
				throw new ArgumentNullException(@"block");

			return _domFrontierOfBlock[block.Index];
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

			while (f2 != null && f1 != null && f1.Index != f2.Index) {
				while (f1 != null && f1.Index > f2.Index)
					f1 = _doms[f1.Index];

				while (f2 != null && f1 != null && f2.Index > f1.Index)
					f2 = _doms[f2.Index];
			}

			return f1;
		}

		private BasicBlock[] ReversePostorder(List<BasicBlock> blocks)
		{
			BasicBlock[] result = new BasicBlock[blocks.Count - 1];
			int idx = 0;
			Queue<BasicBlock> workList = new Queue<BasicBlock>(blocks.Count);

			// Add next Blocks
			foreach (BasicBlock next in NextBlocks(blocks[0]))
				workList.Enqueue(next);

			while (0 != workList.Count) {
				BasicBlock current = workList.Dequeue();
				if (-1 == Array.IndexOf(result, current)) {
					result[idx++] = current;
					foreach (BasicBlock next in NextBlocks(current))
						workList.Enqueue(next);
				}
			}

			return result;
		}

		private IEnumerable<BasicBlock> NextBlocks(BasicBlock basicBlock)
		{
			List<BasicBlock> blocks = new List<BasicBlock>();

			for (Context ctx = new Context(InstructionSet, basicBlock); !ctx.EndOfInstruction; ctx.GotoNext()) {
				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Branch:
						foreach (int target in ctx.Branch.Targets) 
							blocks.Add(FindBlock(target));
						break;
					case FlowControl.ConditionalBranch:
						goto case FlowControl.Branch;
				}
			}

			return blocks;
		}


		#endregion // Internals
	}
}
