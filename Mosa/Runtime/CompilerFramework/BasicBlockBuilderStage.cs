/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
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
		private SortedDictionary<int, BasicBlock> _heads;
		/// <summary>
		/// 
		/// </summary>
		private List<int> _slice;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, int> _targets;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _epilogue;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _prologue;

		#endregion // Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="BasicBlockBuilderStage"/> class.
		/// </summary>
		public BasicBlockBuilderStage()
		{
			_heads = new SortedDictionary<int, BasicBlock>();
			_slice = new List<int>();
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

			// Create the prologue block
			Context ctx = new Context(InstructionSet, -1);
			// Add a jump instruction to the first block from the prologue
			//ctx.InsertInstructionAfter(IR.Instruction.JmpInstruction);
			ctx.InsertInstructionAfter(CIL.Instruction.Get(CIL.OpCode.Br));
			ctx.SetBranch(0);
			ctx.Label = -1;
			_prologue = new BasicBlock(-1, ctx.Index);

			// Create the epilogue block
			ctx = new Context(InstructionSet, -1);
			// Add null instruction, necessary to generate a block index
			ctx.InsertInstructionAfter(null);
			ctx.Ignore = true;
			ctx.Label = Int32.MaxValue;
			_epilogue = new BasicBlock(Int32.MaxValue, ctx.Index);

			// Add epilogue block to leaders (helps with loop below)
			_heads.Add(_epilogue.Label, _epilogue);

			compiler.BasicBlocks = new List<BasicBlock>(_heads.Count + 2);
			BasicBlocks = compiler.BasicBlocks;
			BasicBlocks.Add(_prologue);

			FindTargets(0);

			// Split the blocks
			SplitIntoBlocks();

			// Link all the blocks together
			BuildBlockLinks(_prologue);

			BasicBlocks.Add(_epilogue);

			// help out the gargage collector
			_heads = null;
			_slice = null;
			_targets = null;
		}

		/// <summary>
		/// Finds all targets.
		/// </summary>
		/// <param name="index">The index.</param>
		private void FindTargets(int index)
		{
			_targets.Add(index, -1);

			// Find out all targets labels
			for (Context ctx = new Context(InstructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext()) {

				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Break:
						goto case FlowControl.Branch;
					case FlowControl.Return:
						_slice.Add(ctx.Index);
						continue;
					case FlowControl.Throw:
						goto case FlowControl.Branch;
					case FlowControl.Branch:
						// Unconditional branch 
						Debug.Assert(ctx.Branch.Targets.Length == 1);
						_slice.Add(ctx.Index);
						if (!_targets.ContainsKey(ctx.Branch.Targets[0]))
							_targets.Add(ctx.Branch.Targets[0], -1);
						continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.ConditionalBranch:
						// Conditional branch with multiple targets
						_slice.Add(ctx.Index);
						foreach (int target in ctx.Branch.Targets)
							if (!_targets.ContainsKey(target))
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

			Debug.Assert(_targets.Count <= 1);

			if (FindBlock(0) == null)
				BasicBlocks.Add(new BasicBlock(0, index));

		}

		/// <summary>
		/// Splits the instruction set into blocks.
		/// </summary>
		private void SplitIntoBlocks()
		{
			foreach (int index in _slice)
				InstructionSet.SliceAfter(index);
		}

		/// <summary>
		/// Builds the block links.
		/// </summary>
		/// <param name="block">The current block.</param>
		private void BuildBlockLinks(BasicBlock block)
		{
			for (Context ctx = new Context(InstructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext()) {

				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Return:
						LinkBlocks(block, _epilogue);
						return;
					case FlowControl.Break: goto case FlowControl.Branch;
					case FlowControl.Throw: goto case FlowControl.Branch;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.Branch: {
							BasicBlock next = FindBlock(ctx.Branch.Targets[0]);
							if (!block.NextBlocks.Contains(next)) {
								LinkBlocks(block, next);
								BuildBlockLinks(next);
							}
							return;
						}
					case FlowControl.ConditionalBranch:
						foreach (int target in ctx.Branch.Targets) {
							BasicBlock next = FindBlock(target);
							if (!block.NextBlocks.Contains(next)) {
								LinkBlocks(block, next);
								BuildBlockLinks(next);
							}
						}
						return;
					default:
						Debug.Assert(false);
						break;
				}
			}
		}

		/// <summary>
		/// Adds the stage to the pipeline.
		/// </summary>
		/// <param name="pipeline">The pipeline to add to.</param>
		public void AddToPipeline(CompilerPipeline<IMethodCompilerStage> pipeline)
		{
			pipeline.InsertBefore<IR.CilTransformationStage>(this);
		}

		/// <summary>
		/// Links the Blocks.
		/// </summary>
		/// <param name="caller">The caller.</param>
		/// <param name="callee">The callee.</param>
		private void LinkBlocks(BasicBlock caller, BasicBlock callee)
		{
			// Chain the blocks together
			caller.NextBlocks.Add(callee);
			callee.PreviousBlocks.Add(caller);
		}

		#endregion // IMethodCompilerStage members

	}
}
