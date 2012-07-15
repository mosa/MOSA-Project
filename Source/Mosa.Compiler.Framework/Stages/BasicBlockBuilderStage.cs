/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.IR;

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

		#endregion // Data members

		#region IMethodCompilerStage members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			if (methodCompiler.Compiler.PlugSystem.GetPlugMethod(methodCompiler.Method) != null)
				return;

			if (!methodCompiler.Method.HasCode)
				return;

			// Create the prologue block
			Context context = new Context(instructionSet);
			// Add a jump instruction to the first block from the prologue
			context.AppendInstruction(IRInstruction.Jmp);
			context.SetBranch(0);
			context.Label = BasicBlock.PrologueLabel;
			prologue = basicBlocks.CreateBlock(BasicBlock.PrologueLabel, context.Index);
			basicBlocks.AddHeaderBlock(prologue);

			SplitIntoBlocks(0);

			// Create the epilogue block
			context = new Context(instructionSet);
			// Add null instruction, necessary to generate a block index
			context.AppendInstruction(null);
			context.Label = BasicBlock.EpilogueLabel;
			epilogue = basicBlocks.CreateBlock(BasicBlock.EpilogueLabel, context.Index);

			// Link all the blocks together
			BuildBlockLinks(prologue);

			foreach (ExceptionHandlingClause exceptionClause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				if (exceptionClause.HandlerOffset != 0)
				{
					BasicBlock basicBlock = basicBlocks.GetByLabel(exceptionClause.HandlerOffset);
					BuildBlockLinks(basicBlock);
					basicBlocks.AddHeaderBlock(basicBlock);
				}
				if (exceptionClause.FilterOffset != 0)
				{
					BasicBlock basicBlock = basicBlocks.GetByLabel(exceptionClause.FilterOffset);
					BuildBlockLinks(basicBlock);
					basicBlocks.AddHeaderBlock(basicBlock);
				}
			}
		}

		#endregion // IMethodCompilerStage members

		/// <summary>
		/// Finds all targets.
		/// </summary>
		/// <param name="index">The index.</param>
		private void SplitIntoBlocks(int index)
		{
			Dictionary<int, int> targets = new Dictionary<int, int>();

			targets.Add(index, -1);

			// Find out all targets labels
			for (Context ctx = new Context(instructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				switch (ctx.Instruction.FlowControl)
				{
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Break: goto case FlowControl.Branch;
					case FlowControl.Return: continue;
					case FlowControl.Throw: continue;
					case FlowControl.Branch:
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
			foreach (ExceptionHandlingClause exceptionClause in methodCompiler.ExceptionClauseHeader.Clauses)
			{
				if (!targets.ContainsKey(exceptionClause.HandlerOffset))
					targets.Add(exceptionClause.HandlerOffset, -1);

				if (!targets.ContainsKey(exceptionClause.TryOffset))
					targets.Add(exceptionClause.TryOffset, -1);

				if (!targets.ContainsKey(exceptionClause.FilterOffset))
					targets.Add(exceptionClause.FilterOffset, -1);
			}

			bool slice = false;

			for (Context ctx = new Context(instructionSet, index); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				FlowControl flow;

				if (targets.ContainsKey(ctx.Label))
				{
					basicBlocks.CreateBlock(ctx.Label, ctx.Index);

					if (!ctx.IsFirstInstruction)
					{
						Context prev = ctx.Previous;
						flow = prev.Instruction.FlowControl;
						if (flow == FlowControl.Next || flow == FlowControl.Call || flow == FlowControl.ConditionalBranch || flow == FlowControl.Switch)
						{
							// This jump joins fall-through blocks, by giving them a proper end.
							prev.AppendInstruction(CIL.CILInstruction.Get(CIL.OpCode.Br));
							prev.SetBranch(ctx.Label);

							prev.SliceAfter();
						}
					}

					targets.Remove(ctx.Label);
				}

				if (slice)
					ctx.SliceBefore();

				flow = ctx.Instruction.FlowControl;

				slice = (flow == FlowControl.Return || flow == FlowControl.Branch || flow == FlowControl.ConditionalBranch || flow == FlowControl.Break || flow == FlowControl.Throw || flow == FlowControl.Leave || flow == FlowControl.EndFinally);
			}

			Debug.Assert(targets.Count <= 1);

			//if (basicBlocks.FindBlock(0) == null)
			//	basicBlocks.CreateBlock(0, index);
		}

		/// <summary>
		/// Builds the block links.
		/// </summary>
		/// <param name="block">The current block.</param>
		private void BuildBlockLinks(BasicBlock block)
		{
			for (Context ctx = CreateContext(block); !ctx.EndOfInstruction; ctx.GotoNext())
			{
				switch (ctx.Instruction.FlowControl)
				{
					case FlowControl.Next: continue;
					case FlowControl.Call: continue;
					case FlowControl.Return:
						if (!block.NextBlocks.Contains(epilogue))
							LinkBlocks(block, epilogue);
						return;
					case FlowControl.Break: goto case FlowControl.Branch;
					case FlowControl.Throw: continue;
					case FlowControl.Switch: goto case FlowControl.ConditionalBranch;
					case FlowControl.Branch:
						FindAndLinkBlock(block, ctx.BranchTargets[0]);
						return;
					case FlowControl.ConditionalBranch:
						foreach (int target in ctx.BranchTargets)
							FindAndLinkBlock(block, target);

						int nextIndex = ctx.Index + 1;
						if (nextIndex < this.instructionSet.Used)
							FindAndLinkBlock(block, this.instructionSet.Data[nextIndex].Label);

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
			BasicBlock next = basicBlocks.GetByLabel(target);
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
