/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;

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
			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (basicBlocks.HeadBlocks.Count != 1)
				return;

			// The Mosa.Kernel.x86.VirtualPageAllocator.Reserve method can not be optimized correctly
			if (methodCompiler.Method.ToString().StartsWith("Mosa.Kernel.x86.VirtualPageAllocator.Reserve"))
				return;

			// The Mosa.Kernel.KernelMemory method can not be optimized correctly
			if (methodCompiler.Method.ToString().StartsWith("Mosa.Kernel.KernelMemory"))
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
				worklistbitmap.Set(index, false);
				Context ctx = new Context(instructionSet, index);

				SimpleConstantPropagation(ctx);
				RemoveUselessMove(ctx);
				//FoldAddSigned(ctx);
			}
		}

		/// <summary>
		/// Adds to work list.
		/// </summary>
		/// <param name="index">The index.</param>
		void AddToWorkList(int index)
		{
			if (!worklistbitmap.Get(index))
			{
				worklist.Enqueue(index);
				worklistbitmap.Set(index, true);
				if (IsLogging) Trace("QUEUED:\t" + String.Format("L_{0:X4}", index));
			}
		}

		/// <summary>
		/// Adds the context result to work list.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddContextResultToWorkList(Context context)
		{
			foreach (int index in context.Result.Uses)
			{
				AddToWorkList(index);
			}
		}

		/// <summary>
		/// Adds the context result to work list.
		/// </summary>
		/// <param name="operand">The operand.</param>
		void AddOperandUsageToWorkList(Operand operand)
		{
			foreach (int index in operand.Uses)
			{
				AddToWorkList(index);
			}

			foreach (int index in operand.Definitions)
			{
				AddToWorkList(index);
			}
		}

		/// <summary>
		/// Gets the base operand.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		private Operand GetBaseOperand(Operand operand)
		{
			if (operand.IsSSA)
				return operand.SsaOperand;
			else
				return operand;
		}

		/// <summary>
		/// Removes the useless move.
		/// </summary>
		/// <param name="context">The context.</param>
		private void RemoveUselessMove(Context context)
		{
			if (context.Ignore)
				return;

			if (!(context.Instruction is IR.Move))
				return;

			if (context.Operand1 == context.Result)
			{
				if (IsLogging) Trace("REMOVED:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Nop);
				//context.Remove();
				return;
			}

			if (context.Result.Uses.Count == 0 && GetBaseOperand(context.Result).IsStackTemp)
			{
				if (IsLogging) Trace("REMOVED:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Nop);
				//context.Remove();
				return;
			}
		}

		/// <summary>
		/// Simples the constant propagation.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimpleConstantPropagation(Context context)
		{
			if (context.Ignore)
				return;

			if (!(context.Instruction is IR.Move))
				return;


			if (!context.Operand1.IsConstant)
				return;

			Operand constantOperand = context.Operand1;

			// propagate constant

			if (IsLogging) Trace("REVIEWING:\t" + context.ToString());

			// for each statement T that uses operand, substituted c in statement T
			foreach (int index in context.Result.Uses.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Instruction is IR.AddressOf)
					continue;

				if (ctx.Instruction is IR.Phi)
					continue;

				//if (!(ctx.Instruction is IR.Move))
				//    continue;

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (operand == context.Result)
					{
						if (IsLogging) Trace("UPDATING:\t" + ctx.ToString());
						if (IsLogging) Trace("REPLACED:\t" + operand.ToString() + " with " + constantOperand.ToString());

						ctx.SetOperand(i, constantOperand);

						if (IsLogging) Trace("RESULT:\t" + ctx.ToString());

						AddOperandUsageToWorkList(operand);
					}

				}

				AddToWorkList(index);
			}

			// if constant has no uses, delete S
			// and use is not a local or memory variable (only temp. stack-based operands can be removed)
			if (context.Result.Uses.Count == 0 && GetBaseOperand(context.Result).IsStackTemp)
			{
				if (IsLogging) Trace("REMOVED:\t" + context.ToString());

				context.SetInstruction(IRInstruction.Nop);
				//context.Remove();
			}
		}

		/// <summary>
		/// Folds the add signed.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FoldAddSigned(Context context)
		{
			if (context.Ignore)
				return;

			if (!(context.Instruction is IR.AddSigned))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			if (result.Type.Type != op1.Type.Type || op1.Type.Type != op2.Type.Type)
				return;

			Operand constant = null;

			switch (result.Type.Type)
			{
				case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.Value) + (byte)(op2.Value)); break;
				case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.Value) + (int)(op2.Value)); break;
				default: break;
			}

			if (constant != null)
			{
				AddContextResultToWorkList(context);

				context.SetInstruction(IRInstruction.Move, context.Result, constant);
			}

		}

		#endregion // IMethodCompilerStage Members
	}
}
