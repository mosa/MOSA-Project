/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

using CIL = Mosa.Runtime.CompilerFramework.CIL;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// This compilation stage is used by method compilers after the
	/// IL decoding stage to build basic Blocks out of the instruction list.
	/// </summary>
	public sealed class BasicBlockBuilderStage : BaseStage, IMethodCompilerStage
	{
		#region Data members

		/// <summary>
		/// List of leaders
		/// </summary>
		private SortedDictionary<int, BasicBlock> _loopHeads;

		/// <summary>
		/// 
		/// </summary>
		private List<int> _sliceAfter;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, int> _targets;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlockBuilderStage"/> class.
		/// </summary>
		public BasicBlockBuilderStage()
		{
			_loopHeads = new SortedDictionary<int, BasicBlock>();
			_sliceAfter = new List<int>();
			_targets = new Dictionary<int, int>();
		}

		#endregion // Construction

		#region IMethodCompilerStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		public string Name
		{
			get { return @"Basic Block Builder"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		/// <param name="compiler">The compiler context to perform processing in.</param>
		public override void Run(IMethodCompiler compiler)
		{
			base.Run(compiler);

			compiler.BasicBlocks = new List<BasicBlock>((_loopHeads.Count + 2) * 2);
			BasicBlocks = compiler.BasicBlocks;

			// Create the prologue block
			Context ctx = new Context(InstructionSet, -1);
			// Add a jump instruction to the first block from the prologue
			ctx.InsertInstructionAfter(CIL.Instruction.Get(CIL.OpCode.Br));
			ctx.SetBranch(0);
			BasicBlock prologue = new BasicBlock(-1, ctx.Index);
			BasicBlocks.Add(prologue);

			// Create the epilogue block
			ctx = new Context(InstructionSet, -1);
			// Add null instruction, necessary to generate a block index
			ctx.InsertInstructionAfter(null);
			ctx.Ignore = true;
			BasicBlock epilogue = new BasicBlock(Int32.MaxValue, ctx.Index);
			BasicBlocks.Add(epilogue);

			// Add epilogue block to leaders (helps with loop below)
			_loopHeads.Add(epilogue.Label, epilogue);

			FindTargets(0);

			// Link prologue block to the first leader
			LinkBlocks(prologue, _loopHeads[0]);

			//			CreateBlocks(_loopHeads, epilogue);
		}

		/// <summary>
		/// Finds all targets.
		/// </summary>
		/// <param name="index">The index.</param>
		private void FindTargets(int index)
		{
			// Find out all targets labels
			for (Context ctx = new Context(InstructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext()) {

				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Break:
						_sliceAfter.Add(ctx.Index);
						continue;
					case FlowControl.Return:
						_sliceAfter.Add(ctx.Index);
						continue;
					case FlowControl.Throw:
						_sliceAfter.Add(ctx.Index);
						goto case FlowControl.Branch;
					case FlowControl.Branch:
						// Unconditional branch 
						Debug.Assert(ctx.Branch.Targets.Length == 1);
						_sliceAfter.Add(ctx.Index);
						_targets.Add(ctx.Branch.Targets[0], -1);
						continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.ConditionalBranch:
						// Conditional branch with multiple targets
						_sliceAfter.Add(ctx.Index);
						foreach (int target in ctx.Branch.Targets)
							_targets.Add(target, -1);
						continue;
					default:
						Debug.Assert(false);
						break;
				}
			}

			// Map target labels to indexes
			for (Context ctx = new Context(InstructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext())
				if (_targets.ContainsKey(ctx.Label)) {
					BasicBlocks.Add(new BasicBlock(ctx.Label, ctx.Index));
					_targets.Remove(ctx.Label);
				}

			Debug.Assert(_targets.Count == 0);

			if (FindBlock(0) == null)
				BasicBlocks.Add(new BasicBlock(0, index));

		}

		/// <summary>
		/// Splits the instruction set into blocks.
		/// </summary>
		private void SplitIntoBlocks()
		{
			foreach (int index in _sliceAfter)
				InstructionSet.SliceAfter(index);

			//			foreach (KeyValuePair<int, BasicBlock> next in _loopHeads)
			//				InstructionSet.SliceBefore(next.Key);
		}




		/// <summary>
		/// Inserts the flow control.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="current">The current.</param>
		/// <param name="nextBlock">The next block.</param>
		/// <param name="epilogue">The epilogue.</param>
		private void InsertFlowControl(Context ctx, BasicBlock current, int nextBlock, BasicBlock epilogue)
		{
			switch ((ctx.Instruction as CIL.BaseInstruction).FlowControl) {
				case FlowControl.Break: goto case FlowControl.Next;
				case FlowControl.Call: goto case FlowControl.Next;
				case FlowControl.Next:
					// Insert unconditional branch to next basic block
					Context inserted = ctx.Clone();
					inserted.InsertInstructionAfter(CIL.Instruction.Get(CIL.OpCode.Br_s));
					inserted.SetBranch(nextBlock);

					ctx.SliceAfter();
					LinkBlocks(current, _loopHeads[nextBlock]);
					break;

				case FlowControl.Return:
					// Insert unconditional branch to epilogue block
					LinkBlocks(current, epilogue);
					break;

				case FlowControl.Switch:
					// Switch may fall through
					goto case FlowControl.ConditionalBranch;

				case FlowControl.Branch: goto case FlowControl.ConditionalBranch;

				case FlowControl.ConditionalBranch:
					// Conditional branch with multiple targets
					foreach (int target in ctx.Branch.Targets)
						LinkBlocks(current, _loopHeads[target]);
					goto case FlowControl.Throw;

				case FlowControl.Throw:
					// End the block, start a new one on the next statement
					break;

				default:
					Debug.Assert(false);
					break;
			}
		}

		/// <summary>
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<CIL.CilToIrTransformationStage>(this);
		}

		/// <summary>
		/// Links the Blocks.
		/// </summary>
		/// <param name="caller">The caller.</param>
		/// <param name="callee">The callee.</param>
		private void LinkBlocks(BasicBlock caller, BasicBlock callee)
		{
			// Chain the blocks together
			callee.PreviousBlocks.Add(caller);
			caller.NextBlocks.Add(callee);
		}

		#endregion // IMethodCompilerStage members

	}
}
