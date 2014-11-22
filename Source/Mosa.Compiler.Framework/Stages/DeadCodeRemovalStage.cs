/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public class DeadCodeRemovalStage : BaseMethodCompilerStage
	{
		protected SectionTrace trace;

		protected int instructionsRemovedCount = 0;

		protected Stack<Context> instructionWorkList = new Stack<Context>();

		protected HashSet<int> markInstructions = new HashSet<int>();

		protected bool changed = false;

		protected override void Setup()
		{
			trace = CreateTrace();
		}

		protected override void Run()
		{
			// this optimization stage can not optimize methods with protected regions
			if (HasProtectedRegions)
				return;

			Initialize();
			Mark();

			DumpTrace();

			Sweep();

			UpdateCounter("DeadCodeRemoval.IRInstructionRemoved", instructionsRemovedCount);

			markInstructions = null;
			instructionWorkList = null;
		}

		protected bool IsInstructionCritical(Context context)
		{
			var instruction = context.Instruction;

			if (instruction == IRInstruction.Call ||
				instruction == IRInstruction.Store ||
				instruction == IRInstruction.CompoundStore ||
				instruction == IRInstruction.Return ||
				instruction == IRInstruction.Throw ||
				instruction == IRInstruction.IntrinsicMethodCall ||
				instruction == IRInstruction.InternalReturn ||
				instruction == IRInstruction.Prologue ||
				instruction == IRInstruction.Epilogue ||
				// included because these instructions may cause an exception
				instruction == IRInstruction.DivFloat ||
				instruction == IRInstruction.DivSigned ||
				instruction == IRInstruction.DivUnsigned ||
				instruction == IRInstruction.RemFloat ||
				instruction == IRInstruction.RemSigned ||
				instruction == IRInstruction.RemUnsigned)
				return true;

			if (!(instruction is BaseIRInstruction))
				return true;

			if (context.ResultCount >= 1 && context.Result.IsMemoryAddress)
				return true;

			if (instruction == IRInstruction.Jmp && context.BranchTargets == null)
				return true;

			return false;
		}

		protected void Initialize()
		{
			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (!IsInstructionCritical(ctx))
						continue;

					markInstructions.Add(ctx.Index);
					instructionWorkList.Push(ctx.Clone());
				}
			}
		}

		// copied from SparseConditionalConstantPropagation
		private BasicBlock GetBlock(Context context, bool backwards)
		{
			var block = context.BasicBlock;

			if (block == null)
			{
				Context clone = context.Clone();

				if (backwards)
					clone.GotoFirst();
				else
					clone.GotoLast();

				block = BasicBlocks.GetByLabel(clone.Label);
			}

			return block;
		}

		protected void Mark()
		{
			var markTrace = CreateTrace("Mark");

			while (instructionWorkList.Count != 0)
			{
				var ctx = instructionWorkList.Pop();

				if (markTrace.Active) markTrace.Log("Visiting: " + ctx.ToString());

				var visitor = new OperandVisitor(ctx);

				foreach (var op in visitor.Input)
				{
					if (!op.IsVirtualRegister)
						continue;

					foreach (var def in op.Definitions)
					{
						if (markInstructions.Contains(def))
							continue;

						var ctx2 = new Context(InstructionSet, def);

						if (markTrace.Active) markTrace.Log("Marking: " + ctx2.ToString());

						markInstructions.Add(def);
						instructionWorkList.Push(ctx2.Clone());
					}
				}

				var block = GetBlock(ctx, true);

				foreach (var prev in block.PreviousBlocks)
				{
					var ctx3 = new Context(InstructionSet, prev, prev.EndIndex);

					while (ctx3.IsEmpty || ctx3.IsBlockEndInstruction || ctx3.BranchTargets != null)
					{
						if (ctx3.IsEmpty || ctx3.IsBlockEndInstruction)
						{
							ctx3.GotoPrevious();
							continue;
						}

						if (ctx3.BranchTargets == null)
							break;

						foreach (var target in ctx3.BranchTargets)
						{
							if (target != block.Label)
								continue;

							if (markInstructions.Contains(ctx3.Index))
								continue;

							if (markTrace.Active) markTrace.Log("Marking: " + ctx3.ToString());

							markInstructions.Add(ctx3.Index);
							instructionWorkList.Push(ctx3.Clone());

							break;
						}

						ctx3.GotoPrevious();
					}
				}
			}
		}

		protected void Sweep()
		{
			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty || ctx.IsBlockEndInstruction || ctx.IsBlockStartInstruction)
						continue;

					if (ctx.Instruction == IRInstruction.Nop)
						continue;

					if (markInstructions.Contains(ctx.Index))
						continue;

					if (ctx.BranchTargets == null)
					{
						if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());
						ctx.SetInstruction(IRInstruction.Nop);
						instructionsRemovedCount++;
					}
					else
					{
						return;
					}
				}
			}
		}

		private void DumpTrace()
		{
			if (!trace.Active)
				return;

			var instructionTrace = CreateTrace("Instructions");

			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					bool isalive = markInstructions.Contains(ctx.Index);

					instructionTrace.Log((isalive ? "*" : "-") + ctx.ToString());
				}
			}
		}
	}
}