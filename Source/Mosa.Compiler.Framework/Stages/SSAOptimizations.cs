/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Diagnostics;
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

		#region IMethodCompilerStage Members

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		void IMethodCompilerStage.Run()
		{
			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (basicBlocks.HeadBlocks.Count != 1)
				return;

			// initialize worklist
			foreach (BasicBlock block in basicBlocks)
			{
				for (Context ctx = new Context(instructionSet, block); !ctx.EndOfInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.ResultCount == 0 && ctx.OperandCount == 0)
						continue;

					Do(ctx);
				}
			}

			while (worklist.Count != 0)
			{
				int index = worklist.Dequeue();
				Context ctx = new Context(instructionSet, index);
				Do(ctx);
			}
		}

		private void Do(Context context)
		{
			SimpleConstantPropagation(context);
			RemoveUselessMove(context);
			FoldAddInteger(context);
			MultiplicationStrengthReduction(context);
		}

		/// <summary>
		/// Adds to work list.
		/// </summary>
		/// <param name="index">The index.</param>
		void AddToWorkList(int index)
		{
			worklist.Enqueue(index);
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
			if (context.IsEmpty)
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
			if (context.IsEmpty)
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
						if (IsLogging) Trace("BEFORE:\t" + ctx.ToString());
						//if (IsLogging) Trace("REPLACED:\t" + operand.ToString() + " with " + constantOperand.ToString());

						ctx.SetOperand(i, constantOperand);

						if (IsLogging) Trace("AFTER: \t" + ctx.ToString());

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
		/// Folds an integer add on constants
		/// </summary>
		/// <param name="context">The context.</param>
		private void FoldAddInteger(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.AddSigned || context.Instruction is IR.AddUnsigned))
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
				case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(LoadInteger(op1) + LoadInteger(op2))); break;
				case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(LoadInteger(op1) + LoadInteger(op2))); break;
				case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(LoadInteger(op1) + LoadInteger(op2))); break;
				case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(LoadInteger(op1) + LoadInteger(op2))); break;

				case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(LoadInteger(op1) + LoadInteger(op2))); break;
				case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(LoadInteger(op1) + LoadInteger(op2))); break;
				case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(LoadInteger(op1) + LoadInteger(op2))); break;
				case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(LoadInteger(op1) + LoadInteger(op2))); break;

				default: throw new CompilationException("Not an integer");
			}

			if (constant != null)
			{
				AddContextResultToWorkList(context);

				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, context.Result, constant);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
			}

		}

		/// <summary>
		/// Strength reduction for multiplication when one of the constants is zero or one
		/// </summary>
		/// <param name="context">The context.</param>
		private void MultiplicationStrengthReduction(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.MulSigned || context.Instruction is IR.MulUnsigned || context.Instruction is IR.MulFloat))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && IsValueZero(op1))
			{
				AddContextResultToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && IsValueZero(op2))
			{
				AddContextResultToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op1.IsConstant && IsValueOne(op1))
			{
				AddContextResultToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op2);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && IsValueOne(op2))
			{
				AddContextResultToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op1);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}
		}

		#region Helpers

		/// <summary>
		/// Loads the integer.
		/// </summary>
		/// <param name="operand">The operand.</param>
		/// <returns></returns>
		private long LoadInteger(Operand operand)
		{
			object value = operand.Value;

			if (value is int)
				return (long)(int)value;
			else if (value is short)
				return (long)(short)value;
			else if (value is sbyte)
				return (long)(sbyte)value;
			else if (value is long)
				return (long)value;
			else if (value is int)
				return (long)(int)value;
			else if (value is short)
				return (long)(short)value;
			else if (value is sbyte)
				return (long)(sbyte)value;
			else if (value is long)
				return (long)value;

			throw new CompilationException("Not an integer");
		}

		/// <summary>
		/// Determines whether the value is zero.
		/// </summary>
		/// 
		/// <param name="operand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is zero; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueZero(Operand operand)
		{
			Debug.Assert(operand.IsConstant);
			object value = operand.Value;

			if (value is int)
				return (int)value == 0;
			else if (value is short)
				return (short)value == 0;
			else if (value is sbyte)
				return (sbyte)value == 0;
			else if (value is long)
				return (long)value == 0;
			else if (value is int)
				return (int)value == 0;
			else if (value is short)
				return (short)value == 0;
			else if (value is sbyte)
				return (sbyte)value == 0;
			else if (value is long)
				return (long)value == 0;
			else if (value is double)
				return (double)value == 0;
			else if (value is float)
				return (float)value == 0;

			throw new CompilationException("unknown type");
		}

		/// <summary>
		/// Determines whether the value is one.
		/// </summary>
		/// 
		/// <param name="operand">The constant operand.</param>
		/// <returns>
		/// 	<c>true</c> if the value is one; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueOne(Operand operand)
		{
			Debug.Assert(operand.IsConstant);
			object value = operand.Value;

			if (value is int)
				return (int)value == 1;
			else if (value is short)
				return (short)value == 1;
			else if (value is sbyte)
				return (sbyte)value == 1;
			else if (value is long)
				return (long)value == 1;
			else if (value is int)
				return (int)value == 1;
			else if (value is short)
				return (short)value == 1;
			else if (value is sbyte)
				return (sbyte)value == 1;
			else if (value is long)
				return (long)value == 1;
			else if (value is double)
				return (double)value == 1;
			else if (value is float)
				return (float)value == 1;

			throw new CompilationException("unknown type");
		}

		#endregion //Helpers

		#endregion // IMethodCompilerStage Members
	}
}
