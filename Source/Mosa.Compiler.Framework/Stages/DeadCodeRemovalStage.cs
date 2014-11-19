/*
 * (c) 2011 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using System.Collections.Generic;
using System.Diagnostics;

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
			Initialize();
			Mark();

			DumpTrace();
			UpdateCounter("DeadCodeRemoval.IRInstructionRemoved", instructionsRemovedCount);


			markInstructions = null;
			instructionWorkList = null;
		}

		protected bool IsInstructionCritical(BaseInstruction instruction)
		{
			return (
				instruction == IRInstruction.Call ||
				instruction == IRInstruction.Store ||
				instruction == IRInstruction.Return ||
				instruction == IRInstruction.Throw ||
				instruction == IRInstruction.IntrinsicMethodCall ||
				instruction == IRInstruction.InternalReturn ||
				// included because instruction may cause an exception
				instruction == IRInstruction.DivFloat ||
				instruction == IRInstruction.DivSigned ||
				instruction == IRInstruction.DivUnsigned ||
				instruction == IRInstruction.RemFloat ||
				instruction == IRInstruction.RemSigned ||
				instruction == IRInstruction.RemUnsigned
			);
		}

		protected void Initialize()
		{
			foreach (var block in BasicBlocks)
			{
				for (var ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (!IsInstructionCritical(ctx.Instruction))
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
			while (instructionWorkList.Count != 0)
			{
				var ctx = instructionWorkList.Pop();

				if (trace.Active) trace.Log("Visiting: " + ctx.ToString());

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

						if (trace.Active) trace.Log("Marking: " + ctx2.ToString());

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
							
							if (trace.Active) trace.Log("Marking: " + ctx3.ToString());

							markInstructions.Add(ctx3.Index);
							instructionWorkList.Push(ctx3.Clone());

							break;
						}

						ctx3.GotoPrevious();
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