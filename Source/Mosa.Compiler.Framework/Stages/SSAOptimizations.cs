/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Collections.Generic;
using System.Diagnostics;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Metadata;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class SSAOptimizations : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		private int instructionsRemoved = 0;

		private Stack<int> worklist = new Stack<int>();

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

					while (worklist.Count != 0)
					{
						int index = worklist.Pop();
						Context ctx2 = new Context(instructionSet, index);
						Do(ctx2);
					}
				}
			}

			UpdateCounter("SSAOptimizations.IRInstructionRemoved", instructionsRemoved);
			worklist = null;
		}

		#endregion // IMethodCompilerStage Members

		private void Do(Context context)
		{
			if (context.IsEmpty)
				return;

			//if (IsLogging) Trace("@REVIEW:\t" + context.ToString());

			SimplifyExtendedMove(context);
			SimplifySubtraction(context);
			StrengthReductionMultiplication(context);
			StrengthReductionDivision(context);
			StrengthReductionIntegerAdditionAndSubstraction(context);
			StrengthReductionLogicalOperators(context);
			ConstantFoldingIntegerOperations(context);
			SimpleConstantPropagation(context);
			SimpleCopyPropagation(context);
			DeadCodeElimination(context);
			ConstantFoldingIntegerCompare(context);
			FoldIntegerCompareBranch(context);
			//CheckForMore(context);
		}

		/// <summary>
		/// Adds to work list.
		/// </summary>
		/// <param name="index">The index.</param>
		void AddToWorkList(int index)
		{
			// work list never gets very large, so the check is inexpensive
			if (!worklist.Contains(index))
				worklist.Push(index);
		}

		/// <summary>
		/// Adds the operand usage and definitions to work list.
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
		/// Adds the all the operands usage and definitions to work list.
		/// </summary>
		/// <param name="context">The context.</param>
		void AddOperandUsageToWorkList(Context context)
		{
			if (context.Result != null)
			{
				AddOperandUsageToWorkList(context.Result);
			}
			foreach (var operand in context.Operands)
			{
				AddOperandUsageToWorkList(operand);
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
		/// Removes the useless move and dead code
		/// </summary>
		/// <param name="context">The context.</param>
		private void DeadCodeElimination(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.ResultCount != 1)
				return;

			if (context.Instruction is IR.Move && context.Operand1 == context.Result)
			{
				if (IsLogging) Trace("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemoved++;
				//context.Remove();
				return;
			}

			if (context.Result.Uses.Count != 0 || context.Instruction is IR.Call || context.Instruction is IR.IntrinsicMethodCall)
				return;

			if (!context.Result.IsStackLocal)
				return;

			if (IsLogging) Trace("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemoved++;
			//context.Remove();
			return;
		}

		/// <summary>
		/// Simple constant propagation.
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

			Operand destinationOperand = context.Result;
			Operand sourceOperand = context.Operand1;

			// for each statement T that uses operand, substituted c in statement T
			foreach (int index in destinationOperand.Uses.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Instruction is IR.AddressOf || ctx.Instruction is IR.Phi)
					continue;

				bool propogated = false;

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (operand == context.Result)
					{
						propogated = true;

						if (IsLogging) Trace("BEFORE:\t" + ctx.ToString());
						AddOperandUsageToWorkList(operand);
						ctx.SetOperand(i, sourceOperand);
						if (IsLogging) Trace("AFTER: \t" + ctx.ToString());
					}
				}

				if (propogated)
					AddToWorkList(index);
			}

		}

		private bool CanCopyPropagation(Operand operand)
		{
			return !(operand.Type.Type == CilElementType.Ptr || operand.Type.Type == CilElementType.I4);
		}

		/// <summary>
		/// Simples the copy propagation.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimpleCopyPropagation(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.Move))
				return;

			if (context.Operand1.IsConstant)
				return;

			if (!context.Result.IsStackLocal)
				return;

			// FIXME: does not work on ptr or I4 types - probably because of signed extension, or I8/U8 - probably because 64-bit
			if (!(CanCopyPropagation(context.Result) && CanCopyPropagation(context.Operand1)))
				return;

			Operand destinationOperand = context.Result;
			Operand sourceOperand = context.Operand1;

			//if (IsLogging) Trace("REVIEWING:\t" + context.ToString());

			// for each statement T that uses operand, substituted c in statement T
			foreach (int index in destinationOperand.Uses.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Instruction is IR.AddressOf || ctx.Instruction is IR.Phi)
					return;
			}

			AddOperandUsageToWorkList(context);

			foreach (int index in destinationOperand.Uses.ToArray())
			{
				Context ctx = new Context(instructionSet, index);

				if (ctx.Instruction is IR.AddressOf || ctx.Instruction is IR.Phi)
					continue;

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (destinationOperand == operand)
					{
						if (IsLogging) Trace("BEFORE:\t" + ctx.ToString());
						ctx.SetOperand(i, sourceOperand);
						if (IsLogging) Trace("AFTER: \t" + ctx.ToString());
					}
				}

			}

			Debug.Assert(destinationOperand.Uses.Count == 0);

			if (IsLogging) Trace("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemoved++;
			//context.Remove();
		}

		/// <summary>
		/// Folds an integer operation on constants
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConstantFoldingIntegerOperations(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.AddSigned || context.Instruction is IR.AddUnsigned ||
				  context.Instruction is IR.SubSigned || context.Instruction is IR.SubUnsigned ||
				  context.Instruction is IR.LogicalAnd || context.Instruction is IR.LogicalOr ||
				  context.Instruction is IR.LogicalXor ||
				  context.Instruction is IR.MulSigned || context.Instruction is IR.MulUnsigned ||
				  context.Instruction is IR.DivSigned || context.Instruction is IR.DivUnsigned
				 ))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			// Divide by zero!
			if ((context.Instruction is IR.DivSigned || context.Instruction is IR.DivUnsigned) && op2.IsConstant && IsValueZero(op2))
				return;

			Operand constant = null;

			if (context.Instruction is IR.AddSigned || context.Instruction is IR.AddUnsigned)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger + op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}
			else if (context.Instruction is IR.SubSigned || context.Instruction is IR.SubUnsigned)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger - op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}
			else if (context.Instruction is IR.LogicalAnd)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger & op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}
			else if (context.Instruction is IR.LogicalOr)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger | op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}
			else if (context.Instruction is IR.LogicalXor)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger ^ op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}
			else if (context.Instruction is IR.MulSigned || context.Instruction is IR.MulUnsigned)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger * op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}
			else if (context.Instruction is IR.DivSigned || context.Instruction is IR.DivUnsigned)
			{
				switch (result.Type.Type)
				{
					case CilElementType.U1: constant = Operand.CreateConstant(result.Type, (byte)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.U2: constant = Operand.CreateConstant(result.Type, (ushort)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.U4: constant = Operand.CreateConstant(result.Type, (uint)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.U8: constant = Operand.CreateConstant(result.Type, (ulong)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.I1: constant = Operand.CreateConstant(result.Type, (sbyte)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.I2: constant = Operand.CreateConstant(result.Type, (short)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.I4: constant = Operand.CreateConstant(result.Type, (int)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;
					case CilElementType.I8: constant = Operand.CreateConstant(result.Type, (long)(op1.ValueAsLongInteger / op2.ValueAsLongInteger)); break;

					default: throw new CompilationException("Not an integer");
				}
			}

			if (constant != null)
			{
				AddOperandUsageToWorkList(context);

				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, context.Result, constant);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
			}

		}

		/// <summary>
		/// Folds the integer compare on constants
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConstantFoldingIntegerCompare(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.IntegerCompare))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			if (op1.Type.Type == CilElementType.Object || op2.Type.Type == CilElementType.Object)
				return;

			bool compareResult = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ValueAsLongInteger == op2.ValueAsLongInteger); break;
				case ConditionCode.NotEqual: compareResult = (op1.ValueAsLongInteger != op2.ValueAsLongInteger); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ValueAsLongInteger >= op2.ValueAsLongInteger); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ValueAsLongInteger > op2.ValueAsLongInteger); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ValueAsLongInteger <= op2.ValueAsLongInteger); break;
				case ConditionCode.LessThan: compareResult = (op1.ValueAsLongInteger < op2.ValueAsLongInteger); break;
				// TODO: Add more
				default: return;
			}

			AddOperandUsageToWorkList(context);
			if (IsLogging) Trace("BEFORE:\t" + context.ToString());
			context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(result.Type, (int)(compareResult ? 1 : 0)));
			if (IsLogging) Trace("AFTER: \t" + context.ToString());
		}

		/// <summary>
		/// Strength reduction for integer addition when one of the constants is zero
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionIntegerAdditionAndSubstraction(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.AddSigned || context.Instruction is IR.AddUnsigned || context.Instruction is IR.SubSigned || context.Instruction is IR.SubUnsigned))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && !op2.IsConstant && IsValueZero(op1))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op2);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && !op1.IsConstant && IsValueZero(op2))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op1);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

		}

		/// <summary>
		/// Strength reduction for multiplication when one of the constants is zero or one
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionMultiplication(Context context)
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
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && IsValueZero(op2))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op1.IsConstant && IsValueOne(op1))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op2);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && IsValueOne(op2))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op1);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}
		}

		/// <summary>
		/// Strength reduction for division when one of the constants is zero or one
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionDivision(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.DivSigned || context.Instruction is IR.DivUnsigned))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && IsValueZero(op1))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && IsValueZero(op2))
			{
				// TODO: Divide by zero
				return;
			}

			if (op2.IsConstant && IsValueOne(op2))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, op1);
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
				return;
			}
		}

		/// <summary>
		/// Simplifies an extended moves on a constant
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimplifyExtendedMove(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.ZeroExtendedMove || context.Instruction is IR.SignExtendedMove))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;

			if (!op1.IsConstant)
				return;

			AddOperandUsageToWorkList(context);
			if (IsLogging) Trace("BEFORE:\t" + context.ToString());
			context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op1.Value));
			if (IsLogging) Trace("AFTER: \t" + context.ToString());
		}

		/// <summary>
		/// Simplifies an subtraction with both operands are the same
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimplifySubtraction(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.SubSigned || context.Instruction is IR.SubUnsigned))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1 != op2)
				return;

			AddOperandUsageToWorkList(context);
			if (IsLogging) Trace("BEFORE:\t" + context.ToString());
			context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
		}

		/// <summary>
		/// Strength reduction for logical operators
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionLogicalOperators(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.LogicalAnd || context.Instruction is IR.LogicalOr))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (context.Instruction is IR.LogicalOr && op1.IsConstant && !op2.IsConstant && IsValueZero(op1))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op2.Value));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
			}

			if (context.Instruction is IR.LogicalOr && op2.IsConstant && !op1.IsConstant && IsValueZero(op2))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op1.Value));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
			}

			if (context.Instruction is IR.LogicalAnd && op1.IsConstant && !op1.IsConstant && IsValueZero(op1))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
			}

			if (context.Instruction is IR.LogicalAnd && op2.IsConstant && !op1.IsConstant && IsValueZero(op2))
			{
				AddOperandUsageToWorkList(context);
				if (IsLogging) Trace("BEFORE:\t" + context.ToString());
				context.SetInstruction(IR.IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (IsLogging) Trace("AFTER: \t" + context.ToString());
			}

			// TODO: Add more strength reductions especially for AND w/ 0xFF, 0xFFFF, 0xFFFFFFFF, etc when source or destination are same or smaller
		}


		/// <summary>
		/// Folds the integer compare branch.
		/// </summary>
		/// <param name="context">The context.</param>
		private void FoldIntegerCompareBranch(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.OperandCount != 2)
				return;

			if (!(context.Instruction is IR.IntegerCompareBranch))
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			if (!(context.Next.Instruction is IR.Jmp))
				return;

			if (context.BranchTargets[0] == context.Next.BranchTargets[0])
			{
				if (IsLogging) Trace("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemoved++;
				return;
			}

			bool compareResult = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ValueAsLongInteger == op2.ValueAsLongInteger); break;
				case ConditionCode.NotEqual: compareResult = (op1.ValueAsLongInteger != op2.ValueAsLongInteger); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ValueAsLongInteger >= op2.ValueAsLongInteger); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ValueAsLongInteger > op2.ValueAsLongInteger); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ValueAsLongInteger <= op2.ValueAsLongInteger); break;
				case ConditionCode.LessThan: compareResult = (op1.ValueAsLongInteger < op2.ValueAsLongInteger); break;
				// TODO: Add more
				default: return;
			}

			Debug.Assert(context.Next.Instruction is IR.Jmp);

			BasicBlock target;

			if (compareResult)
			{
				target = basicBlocks.GetByLabel(context.Next.BranchTargets[0]);

				if (IsLogging) Trace("BEFORE:\t" + context.ToString());

				// change to JMP
				context.SetInstruction(IRInstruction.Jmp, basicBlocks.GetByLabel(context.BranchTargets[0]));
				if (IsLogging) Trace("AFTER:\t" + context.ToString());

				// goto next instruction and prepare to remove it
				context.GotoNext();
			}
			else
			{
				target = basicBlocks.GetByLabel(context.BranchTargets[0]);
			}

			if (IsLogging) Trace("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemoved++;

			// Goto the beginning of the block, to get to the first index of the block
			Context first = context.Clone();
			first.GotoFirst();

			// Find block based on first index
			BasicBlock currentBlock = null;
			foreach (var block in basicBlocks)
			{
				if (block.Index == first.Index)
				{
					currentBlock = block;
					break;
				}
			}

			currentBlock.NextBlocks.Remove(target);
			target.PreviousBlocks.Remove(currentBlock);

			// TODO: if target block no longer has any predecessors (or the only predecessor is itself), remove all instructions from it.


		}
		/// <summary>
		/// Checks for more.
		/// </summary>
		/// <param name="context">The context.</param>
		private void CheckForMore(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.OperandCount != 2)
				return;

			if (context.Instruction is IR.Call || context.Instruction is IR.IntrinsicMethodCall)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && op2.IsConstant)
			{
				return;
			}

		}


		#region Helpers

		/// <summary>
		/// Determines whether the value is zero.
		/// </summary>
		/// <param name="operand">The constant operand.</param>
		/// <returns>
		///   <c>true</c> if the value is zero; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueZero(Operand operand)
		{
			Debug.Assert(operand.IsConstant);
			object value = operand.Value;

			if (value is int)
				return (int)value == 0;
			else if (value is short)
				return (short)value == 0;
			else if (value is byte)
				return (byte)value == 0;
			else if (value is long)
				return (long)value == 0;
			else if (value is uint)
				return (uint)value == 0;
			else if (value is ushort)
				return (ushort)value == 0;
			else if (value is sbyte)
				return (sbyte)value == 0;
			else if (value is ulong)
				return (ulong)value == 0;
			else if (value is double)
				return (double)value == 0;
			else if (value is float)
				return (float)value == 0;

			throw new CompilationException("unknown type");
		}

		/// <summary>
		/// Determines whether the value is one.
		/// </summary>
		/// <param name="operand">The constant operand.</param>
		/// <returns>
		///   <c>true</c> if the value is one; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsValueOne(Operand operand)
		{
			Debug.Assert(operand.IsConstant);
			object value = operand.Value;

			if (value is int)
				return (int)value == 1;
			else if (value is short)
				return (short)value == 1;
			else if (value is byte)
				return (byte)value == 1;
			else if (value is long)
				return (long)value == 1;
			else if (value is uint)
				return (uint)value == 1;
			else if (value is ushort)
				return (ushort)value == 1;
			else if (value is sbyte)
				return (sbyte)value == 1;
			else if (value is ulong)
				return (ulong)value == 1;
			else if (value is double)
				return (double)value == 1;
			else if (value is float)
				return (float)value == 1;

			throw new CompilationException("unknown type");
		}

		/// <summary>
		/// Determines whether [is same integer constant] [the specified op1].
		/// </summary>
		/// <param name="op1">The op1.</param>
		/// <param name="op2">The op2.</param>
		/// <returns>
		///   <c>true</c> if [is same integer constant] [the specified op1]; otherwise, <c>false</c>.
		/// </returns>
		private static bool IsSameIntegerConstant(Operand op1, Operand op2)
		{
			return (op1.ValueAsLongInteger == op2.ValueAsLongInteger);
		}

		#endregion //Helpers

	}
}
