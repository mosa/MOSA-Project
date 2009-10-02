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

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlockBuilderStage"/> class.
		/// </summary>
		public BasicBlockBuilderStage()
		{
			_loopHeads = new SortedDictionary<int, BasicBlock>();
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

			FindLoopHeads(0);

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

			// Link prologue block to the first leader
			LinkBlocks(prologue, _loopHeads[0]);

			CreateBlocks(_loopHeads, epilogue);
		}

		/// <summary>
		/// Finds the loop heads.
		/// </summary>
		/// <param name="index">The index.</param>
		private void FindLoopHeads(int index)
		{
			// Add the start of the instruction set
			AddLoopHead(0);

			for (Context ctx = new Context(InstructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext()) {
				// Does this instruction end a block?
				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Break: goto case FlowControl.Next;
					case FlowControl.Call: goto case FlowControl.Next;
					case FlowControl.Next: break;

					case FlowControl.Return:
						if (!ctx.IsLastInstruction)
							AddLoopHead(ctx.Next.Offset);
						break;

					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.Branch: goto case FlowControl.ConditionalBranch;

					case FlowControl.ConditionalBranch:
						// Conditional branch with multiple targets
						foreach (int target in ctx.Branch.Targets)
							AddLoopHead(target);
						goto case FlowControl.Throw;

					case FlowControl.Throw:
						// End the block, start a new one on the next statement
						if (!ctx.IsLastInstruction)
							AddLoopHead(ctx.Next.Offset);
						break;

					default:
						Debug.Assert(false);
						break;
				}
			}
		}

		/// <summary>
		/// Adds the leader.
		/// </summary>
		/// <param name="index">The index.</param>
		public void AddLoopHead(int index)
		{
			if (!_loopHeads.ContainsKey(index))
				_loopHeads.Add(index, new BasicBlock(index));
		}

		/// <summary>
		/// Inserts the instructions into blocks.
		/// </summary>
		/// <param name="leaders">The leaders.</param>
		/// <param name="epilogue">The epilogue.</param>
		private void CreateBlocks(IDictionary<int, BasicBlock> leaders, BasicBlock epilogue)
		{
			KeyValuePair<int, BasicBlock> current = new KeyValuePair<int, BasicBlock>(-1, null);
			int blockIndex = 0;

			foreach (KeyValuePair<int, BasicBlock> next in leaders) {
				if (current.Key != -1) {

					// Insert block into list of basic Blocks
					BasicBlocks.Add(current.Value);

					// Set the block index
					current.Value.Index = ++blockIndex;	// ???

					Context ctx = new Context(InstructionSet, current.Key);
					ctx.BasicBlock = current.Value;

					// Set the block index on all the instructions
					while ((ctx.Index != next.Key) && !ctx.EndOfInstruction)
						ctx.GotoNext();

					ctx.GotoPrevious(); // FIXME PG - might be buggy if on the last instruction in set

					InsertFlowControl(ctx, current.Value, next.Key, epilogue);
				}

				current = next;
			}
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
					Context inserted = ctx.InsertAfter();
					inserted.SetInstruction(CIL.Instruction.Get(CIL.OpCode.Br_s));
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
			// Chain the Blocks together
			callee.PreviousBlocks.Add(caller);
			caller.NextBlocks.Add(callee);
		}

		#endregion // IMethodCompilerStage members

	}
}
