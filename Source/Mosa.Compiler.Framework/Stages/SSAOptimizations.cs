/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.InternalTrace;
using Mosa.Compiler.MosaTypeSystem;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	///
	/// </summary>
	public sealed class SSAOptimizations : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{
		private int instructionsRemoved = 0;

		private Stack<int> worklist = new Stack<int>();

		private CompilerTrace trace;

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
				for (Context ctx = new Context(instructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
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

		#endregion IMethodCompilerStage Members

		private void Do(Context context)
		{
			if (context.IsEmpty)
				return;

			trace = CreateTrace();

			//if (trace.IsLogging) trace.Log("@REVIEW:\t" + context.ToString());

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
		private void AddToWorkList(int index)
		{
			// work list never gets very large, so the check is inexpensive
			if (!worklist.Contains(index))
				worklist.Push(index);
		}

		/// <summary>
		/// Adds the operand usage and definitions to work list.
		/// </summary>
		/// <param name="operand">The operand.</param>
		private void AddOperandUsageToWorkList(Operand operand)
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
		private void AddOperandUsageToWorkList(Context context)
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
		/// Removes the useless move and dead code
		/// </summary>
		/// <param name="context">The context.</param>
		private void DeadCodeElimination(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.ResultCount != 1)
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (context.Instruction is IR.Move && context.Operand1.IsVirtualRegister && context.Operand1 == context.Result)
			{
				if (trace.Active) trace.Log("*** DeadCodeElimination");
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemoved++;

				//context.Remove();
				return;
			}

			if (context.Result.Uses.Count != 0 || context.Instruction is IR.Call || context.Instruction is IR.IntrinsicMethodCall)
				return;

			if (trace.Active) trace.Log("*** DeadCodeElimination");
			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
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

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand1.IsConstant)
				return;

			Debug.Assert(context.Result.Definitions.Count == 1);

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

						if (trace.Active) trace.Log("*** SimpleConstantPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						AddOperandUsageToWorkList(operand);
						ctx.SetOperand(i, sourceOperand);
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}

				if (propogated)
					AddToWorkList(index);
			}
		}

		private bool CanCopyPropagation(Operand result, Operand destination)
		{
			if (!result.IsValueType && !destination.IsValueType)
				return true;

			if (result.IsInteger && destination.IsInteger &&
				result.IsSigned != result.IsUnsigned)
				return false;

			if (!result.IsPointer && !destination.IsPointer)
				return true;

			if (result.Type != destination.Type)
				return false;

			return result.Type.ElementType == destination.Type.ElementType;
		}

		/// <summary>
		/// Simple copy propagation.
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

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand1.IsVirtualRegister)
				return;

			Debug.Assert(context.Result.Definitions.Count == 1);

			// If the pointer or reference types are different, we can not copy propagation because type information would be lost.
			// Also if the operand sign is different, we cannot do it as it required a signed/unsigned extended move, not a normal move
			if (!CanCopyPropagation(context.Result, context.Operand1))
				return;

			Operand destinationOperand = context.Result;
			Operand sourceOperand = context.Operand1;

			//if (trace.IsLogging) trace.Log("REVIEWING:\t" + context.ToString());

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

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (destinationOperand == operand)
					{
						if (trace.Active) trace.Log("*** SimpleCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetOperand(i, sourceOperand);
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}

			Debug.Assert(destinationOperand.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
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

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			// Divide by zero!
			if ((context.Instruction is IR.DivSigned || context.Instruction is IR.DivUnsigned) && op2.IsConstant && op2.IsConstantZero)
				return;

			Operand constant = null;

			if (context.Instruction is IR.AddSigned || context.Instruction is IR.AddUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger + op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction is IR.SubSigned || context.Instruction is IR.SubUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger - op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction is IR.LogicalAnd)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger & op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction is IR.LogicalOr)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger | op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction is IR.LogicalXor)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger ^ op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction is IR.MulSigned || context.Instruction is IR.MulUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger * op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction is IR.DivSigned || context.Instruction is IR.DivUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger / op2.ConstantUnsignedInteger);
			}

			if (constant != null)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** ConstantFoldingIntegerOperations");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, context.Result, constant);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
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

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			if (!op1.IsValueType || !op2.IsValueType)
				return;

			bool compareResult = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ConstantUnsignedInteger == op2.ConstantUnsignedInteger); break;
				case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsignedInteger != op2.ConstantUnsignedInteger); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsignedInteger >= op2.ConstantUnsignedInteger); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsignedInteger > op2.ConstantUnsignedInteger); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsignedInteger <= op2.ConstantUnsignedInteger); break;
				case ConditionCode.LessThan: compareResult = (op1.ConstantUnsignedInteger < op2.ConstantUnsignedInteger); break;

				// TODO: Add more
				default: return;
			}

			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** ConstantFoldingIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(result.Type, (int)(compareResult ? 1 : 0)));
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
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

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && !op2.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionIntegerAdditionAndSubstraction");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op2);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && !op1.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionIntegerAdditionAndSubstraction");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op1);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
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

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op1.IsConstant && op1.IsConstantOne)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op2);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && op2.IsConstantOne)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op1);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
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

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionDivision");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && op2.IsConstantZero)
			{
				// TODO: Divide by zero
				return;
			}

			if (op2.IsConstant && op2.IsConstantOne)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionDivision");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op1);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}
		}

		/// <summary>
		/// Simplifies extended moves with a constant
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimplifyExtendedMove(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.ZeroExtendedMove || context.Instruction is IR.SignExtendedMove))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;

			if (!op1.IsConstant)
				return;

			Operand newOperand;

			if (context.Instruction is IR.ZeroExtendedMove && result.IsUnsigned && op1.IsSigned)
			{
				var newConstant = Unsign(op1.Type, op1.ConstantSignedInteger);
				newOperand = Operand.CreateConstant(context.Result.Type, newConstant);
			}
			else
			{
				newOperand = Operand.CreateConstant(context.Result.Type, op1.ConstantUnsignedInteger);
			}

			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** SimplifyExtendedMove");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, result, newOperand);
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
		}

		static private ulong Unsign(MosaType type, long value)
		{
			if (type.IsI1) return (ulong)((sbyte)value);
			else if (type.IsI2) return (ulong)((short)value);
			else if (type.IsI4) return (ulong)((int)value);
			else if (type.IsI8) return (ulong)((long)value);
			else return (ulong)value;
		}

		/// <summary>
		/// Simplifies subtraction where both operands are the same
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimplifySubtraction(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction is IR.SubSigned || context.Instruction is IR.SubUnsigned))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op1 != op2)
				return;

			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** SimplifySubtraction");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
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

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (context.Instruction is IR.LogicalOr && op1.IsConstant && !op2.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op2.ConstantSignedInteger));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			}

			if (context.Instruction is IR.LogicalOr && op2.IsConstant && !op1.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op1.ConstantSignedInteger));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			}

			if (context.Instruction is IR.LogicalAnd && op1.IsConstant && !op1.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			}

			if (context.Instruction is IR.LogicalAnd && op2.IsConstant && !op1.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
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

			//if (!context.Result.IsVirtualRegister)
			//	return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			if (!(context.Next.Instruction is IR.Jmp))
				return;

			if (context.BranchTargets[0] == context.Next.BranchTargets[0])
			{
				if (trace.Active) trace.Log("*** FoldIntegerCompareBranch-1");
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemoved++;
				return;
			}

			bool compareResult = true;

			switch (context.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ConstantUnsignedInteger == op2.ConstantUnsignedInteger); break;
				case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsignedInteger != op2.ConstantUnsignedInteger); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsignedInteger >= op2.ConstantUnsignedInteger); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsignedInteger > op2.ConstantUnsignedInteger); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsignedInteger <= op2.ConstantUnsignedInteger); break;
				case ConditionCode.LessThan: compareResult = (op1.ConstantUnsignedInteger < op2.ConstantUnsignedInteger); break;

				// TODO: Add more
				default: return;
			}

			Debug.Assert(context.Next.Instruction is IR.Jmp);

			BasicBlock target;

			if (trace.Active) trace.Log("*** FoldIntegerCompareBranch-2");

			if (compareResult)
			{
				target = basicBlocks.GetByLabel(context.Next.BranchTargets[0]);

				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());

				// change to JMP
				context.SetInstruction(IRInstruction.Jmp, basicBlocks.GetByLabel(context.BranchTargets[0]));
				if (trace.Active) trace.Log("AFTER:\t" + context.ToString());

				// goto next instruction and prepare to remove it
				context.GotoNext();
			}
			else
			{
				target = basicBlocks.GetByLabel(context.BranchTargets[0]);
			}

			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemoved++;

			// Goto the beginning of the block, to get to the first index of the block
			Context first = context.Clone();
			first.GotoFirst(); // FIXME: use block start index

			// Find block based on first index
			BasicBlock currentBlock = null;
			foreach (var block in basicBlocks)
			{
				if (block.StartIndex == first.Index)
				{
					currentBlock = block;
					break;
				}
			}

			currentBlock.NextBlocks.Remove(target);
			target.PreviousBlocks.Remove(currentBlock);

			// TODO: if target block no longer has any predecessors (or the only predecessor is itself), remove all instructions from it.
		}
	}
}
