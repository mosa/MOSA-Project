/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Operands;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SSAOptimizations : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		private Queue<int> worklist = new Queue<int>();
		private BitArray worklistbitmap;

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Unable to optimize SSA w/ exceptions present
			if (basicBlocks.Count != 1)
				return; 

			worklistbitmap = new BitArray(instructionSet.Size);

			// initialize worklist
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.Ignore)
						continue;
					if (ctx.ResultCount == 0 && ctx.OperandCount == 0)
						continue;

					AddToWorkList(ctx.Index);
				}
			}

			while (worklist.Count != 0)
			{
				int index = worklist.Dequeue();
				Context ctx = new Context(instructionSet, index);

				RemoveUselessMove(ctx);
				SimpleConstantPropagation(ctx);
			}
		}

		void AddToWorkList(int index)
		{
			if (!worklistbitmap.Get(index))
			{
				worklist.Enqueue(index);
				worklistbitmap.Set(index, true);
			}
		}

		void RemoveUselessMove(Context context)
		{
			if (context.Ignore)
				return;

			if (!(context.Instruction is IR.Move))
				return;

			if (context.Operand1 != context.Result)
				return;

			context.Remove();
		}

		void SimpleConstantPropagation(Context context)
		{
			if (context.Ignore)
				return;

			if (!(context.Instruction is IR.Move))
				return;

			ConstantOperand constantOperand = context.Operand1 as ConstantOperand;

			if (constantOperand == null)
				return;

			// propagate constant

			// for each statement T that uses operand, substituted c in statement T
			foreach (int index in context.Result.Uses.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Instruction is IR.AddressOf)
					continue;

				if (!(ctx.Instruction is IR.Move))
					continue;

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (operand == context.Result)
					{
						ctx.SetOperand(i, constantOperand);
					}

				}

				AddToWorkList(index);
			}

			// if constant has no uses, delete S

			if (context.Result.Uses.Count == 0)
			{
				//context.SetInstruction(IRInstruction.Nop);
				context.Remove();
			}
		}

		#endregion // IMethodCompilerStage Members
	}
}
