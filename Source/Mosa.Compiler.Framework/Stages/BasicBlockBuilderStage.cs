/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using Mosa.Compiler.Framework.IR;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// This compilation stage is used by method compilers after the
	/// IL decoding stage to build basic Blocks out of the instruction list.
	/// </summary>
	public sealed class BasicBlockBuilderStage : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		#region Data members

		/// <summary>
		///
		/// </summary>
		private BasicBlock epilogue;

		/// <summary>
		///
		/// </summary>
		private BasicBlock prologue;

		#endregion Data members

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Execute()
		{
			// No basic block building if this is a linker generated method
			if (MethodCompiler.Method.IsLinkerGenerated)
				return;

			if (MethodCompiler.Compiler.PlugSystem.GetPlugMethod(MethodCompiler.Method) != null)
				return;

			if (MethodCompiler.Method.Code.Count == 0)
				return;

			// Create the prologue block
			Context context = CreateNewBlockWithContext(BasicBlock.PrologueLabel);

			// Add a jump instruction to the first block from the prologue
			context.AppendInstruction(IRInstruction.Jmp);
			context.SetBranch(0);
			prologue = context.BasicBlock;
			BasicBlocks.AddHeaderBlock(prologue);

			SplitIntoBlocks(0);

			// Create the epilogue block
			context = CreateNewBlockWithContext(BasicBlock.EpilogueLabel);
			epilogue = context.BasicBlock;

			// Link all the blocks together
			BuildBlockLinks(prologue);

			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				if (clause.HandlerOffset != 0)
				{
					BasicBlock basicBlock = BasicBlocks.GetByLabel(clause.HandlerOffset);
					BuildBlockLinks(basicBlock);
					BasicBlocks.AddHeaderBlock(basicBlock);
				}
				if (clause.FilterOffset != null)
				{
					BasicBlock basicBlock = BasicBlocks.GetByLabel(clause.FilterOffset.Value);
					BuildBlockLinks(basicBlock);
					BasicBlocks.AddHeaderBlock(basicBlock);
				}
			}
		}

		#endregion IMethodCompilerStage members

		/// <summary>
		/// Finds all targets.
		/// </summary>
		/// <param name="index">The index.</param>
		private void SplitIntoBlocks(int index)
		{
			Dictionary<int, int> targets = new Dictionary<int, int>();

			targets.Add(index, -1);

			// Find out all targets labels
			for (Context ctx = new Context(InstructionSet, index); ctx.Index >= 0; ctx.GotoNext())
			{
				switch (ctx.Instruction.FlowControl)
				{
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Break: goto case FlowControl.UnconditionalBranch;
					case FlowControl.Return: continue;
					case FlowControl.Throw: continue;
					case FlowControl.UnconditionalBranch:

						// Unconditional branch
						Debug.Assert(ctx.BranchTargets.Length == 1);
						if (!targets.ContainsKey(ctx.BranchTargets[0]))
							targets.Add(ctx.BranchTargets[0], -1);
						continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.ConditionalBranch:

						// Conditional branch with multiple targets
						foreach (int target in ctx.BranchTargets)
							if (!targets.ContainsKey(target))
								targets.Add(target, -1);
						int next = ctx.Next.Label;
						if (!targets.ContainsKey(next))
							targets.Add(next, -1);
						continue;
					case FlowControl.EndFinally: continue;
					case FlowControl.Leave:
						Debug.Assert(ctx.BranchTargets.Length == 1);
						if (!targets.ContainsKey(ctx.BranchTargets[0]))
							targets.Add(ctx.BranchTargets[0], -1);
						continue;
					default:
						Debug.Assert(false);
						break;
				}
			}

			// Add Exception Class targets
			foreach (var clause in MethodCompiler.Method.ExceptionBlocks)
			{
				if (!targets.ContainsKey(clause.HandlerOffset))
					targets.Add(clause.HandlerOffset, -1);

				if (!targets.ContainsKey(clause.TryOffset))
					targets.Add(clause.TryOffset, -1);

				if (clause.FilterOffset != null && !targets.ContainsKey(clause.FilterOffset.Value))
					targets.Add(clause.FilterOffset.Value, -1);
			}

			BasicBlock currentBlock = null;
			Context previous = null;

			for (Context ctx = new Context(InstructionSet, index); ctx.Index >= 0; ctx.GotoNext())
			{
				if (targets.ContainsKey(ctx.Label))
				{
					if (currentBlock != null)
					{
						previous = ctx.Previous;

						var flow = previous.Instruction.FlowControl;

						if (flow == FlowControl.Next || flow == FlowControl.Call || flow == FlowControl.ConditionalBranch || flow == FlowControl.Switch)
						{
							// This jump joins fall-through blocks, by giving them a proper end.
							previous.AppendInstruction(IRInstruction.Jmp);
							previous.SetBranch(ctx.Label);
						}

						// Close current block
						previous.AppendInstruction(IRInstruction.BlockEnd);
						currentBlock.EndIndex = previous.Index;
					}

					Context prev = ctx.InsertBefore();
					prev.SetInstruction(IRInstruction.BlockStart);
					currentBlock = BasicBlocks.CreateBlock(ctx.Label, prev.Index);

					targets.Remove(ctx.Label);
				}

				previous = ctx.Clone();
			}

			// Close current block
			previous.AppendInstruction(IRInstruction.BlockEnd);
			currentBlock.EndIndex = previous.Index;

			Debug.Assert(targets.Count <= 1);
		}

		/// <summary>
		/// Builds the block links.
		/// </summary>
		/// <param name="block">The current block.</param>
		private void BuildBlockLinks(BasicBlock block)
		{
			for (Context ctx = CreateContext(block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
			{
				switch (ctx.Instruction.FlowControl)
				{
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Return:
						if (!block.NextBlocks.Contains(epilogue))
							LinkBlocks(block, epilogue);
						return;

					case FlowControl.Break: goto case FlowControl.UnconditionalBranch;
					case FlowControl.Throw: continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.UnconditionalBranch:
						FindAndLinkBlock(block, ctx.BranchTargets[0]);
						return;

					case FlowControl.ConditionalBranch:
						foreach (int target in ctx.BranchTargets)
							FindAndLinkBlock(block, target);

						int nextIndex = ctx.Index + 1;
						if (nextIndex < this.InstructionSet.Used)
							FindAndLinkBlock(block, InstructionSet.Data[nextIndex].Label);

						continue;
					case FlowControl.EndFinally: return;
					case FlowControl.Leave:
						FindAndLinkBlock(block, ctx.BranchTargets[0]);
						return;

					default:
						Debug.Assert(false);
						break;
				}
			}
		}

		private void FindAndLinkBlock(BasicBlock block, int target)
		{
			BasicBlock next = BasicBlocks.GetByLabel(target);
			if (!block.NextBlocks.Contains(next))
			{
				LinkBlocks(block, next);
				BuildBlockLinks(next);
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
	}
}