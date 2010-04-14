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
	public sealed class BasicBlockBuilderStage : BaseStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _epilogue;
		/// <summary>
		/// 
		/// </summary>
		private BasicBlock _prologue;

		#endregion // Data members

		#region IMethodCompilerStage members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value></value>
		string IPipelineStage.Name { get { return @"BasicBlockBuilderStage"; } }

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
			// Create the prologue block
			Context ctx = new Context(InstructionSet, -1);
			// Add a jump instruction to the first block from the prologue
			//ctx.InsertInstructionAfter(IR.Instruction.JmpInstruction);
			ctx.AppendInstruction(CIL.Instruction.Get(CIL.OpCode.Br));
			ctx.SetBranch(0);
			ctx.Label = -1;
			_prologue = CreateBlock(-1, ctx.Index);

			SplitIntoBlocks(0);

			// Create the epilogue block
			ctx = new Context(InstructionSet, -1);
			// Add null instruction, necessary to generate a block index
			ctx.AppendInstruction(null);
			ctx.Ignore = true;
			ctx.Label = Int32.MaxValue;
			_epilogue = CreateBlock(Int32.MaxValue, ctx.Index);

			// Link all the blocks together
			BuildBlockLinks(_prologue);
		}

		/// <summary>
		/// Finds all targets.
		/// </summary>
		/// <param name="index">The index.</param>
		private void SplitIntoBlocks(int index)
		{
			Dictionary<int, int> targets = new Dictionary<int, int>();

			targets.Add(index, -1);

			// Find out all targets labels
			for (Context ctx = new Context(InstructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext()) {
				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Break:
						goto case FlowControl.Branch;
					case FlowControl.Return:
						continue;
					case FlowControl.Throw:
						goto case FlowControl.Branch;
					case FlowControl.Branch:
						// Unconditional branch 
						Debug.Assert(ctx.Branch.Targets.Length == 1);
						if (!targets.ContainsKey(ctx.Branch.Targets[0]))
							targets.Add(ctx.Branch.Targets[0], -1);
						continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.ConditionalBranch:
						// Conditional branch with multiple targets
						foreach (int target in ctx.Branch.Targets)
							if (!targets.ContainsKey(target))
								targets.Add(target, -1);
						int next = ctx.Next.Label;
						if (!targets.ContainsKey(next))
							targets.Add(next, -1);
						continue;
					default:
						Debug.Assert(false);
						break;
				}
			}

			bool slice = false;

			for (Context ctx = new Context(InstructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext()) {
				FlowControl flow;

				if (targets.ContainsKey(ctx.Label)) {
					CreateBlock(ctx.Label, ctx.Index);

					if (!ctx.IsFirstInstruction) {
						Context prev = ctx.Previous;
						flow = prev.Instruction.FlowControl;
						if (flow == FlowControl.Next || flow == FlowControl.Call || flow == FlowControl.ConditionalBranch || flow == FlowControl.Switch) {
                            // This jump joins fall-through blocks, by giving them a proper end.
							prev.AppendInstruction(CIL.Instruction.Get(CIL.OpCode.Br));
							prev.SetBranch(ctx.Label);

							prev.SliceAfter();
						}
					}

					targets.Remove(ctx.Label);
				}

				if (slice)
					ctx.SliceBefore();

				flow = ctx.Instruction.FlowControl;

				slice = (flow == FlowControl.Return || flow == FlowControl.Branch || flow == FlowControl.ConditionalBranch || flow == FlowControl.Break || flow == FlowControl.Throw);
			}

			Debug.Assert(targets.Count <= 1);

			if (FindBlock(0) == null)
				CreateBlock(0, index);
		}

		/// <summary>
		/// Builds the block links.
		/// </summary>
		/// <param name="block">The current block.</param>
		private void BuildBlockLinks(BasicBlock block)
		{
			for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext()) {
				switch (ctx.Instruction.FlowControl) {
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Return:
						if (!block.NextBlocks.Contains(_epilogue))
							LinkBlocks(block, _epilogue);
						return;
					case FlowControl.Break: goto case FlowControl.Branch;
					case FlowControl.Throw: goto case FlowControl.Branch;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.Branch: {
                            FindAndLinkBlock(block, ctx.Branch.Targets[0]);
							return;
						}
					case FlowControl.ConditionalBranch:
						foreach (int target in ctx.Branch.Targets) {
							FindAndLinkBlock(block, target);
						}

                        // Conditional blocks are at least two way branches. The old way of adding jumps didn't properly
                        // resolve operands under certain circumstances. This does.
				        int nextIndex = ctx.Index + 1;
                        if (nextIndex < this.InstructionSet.Used)
                        {
                            FindAndLinkBlock(block, this.InstructionSet.Data[nextIndex].Label);
                        }
						continue;
					default:
						Debug.Assert(false);
						break;
				}
			}
		}

	    private void FindAndLinkBlock(BasicBlock block, int target)
	    {
	        BasicBlock next = this.FindBlock(target);
	        if (!block.NextBlocks.Contains(next)) {
	            this.LinkBlocks(block, next);
	            this.BuildBlockLinks(next);
	        }
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
