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
	public sealed class SSAOptimizations : BaseMethodCompilerStage
	{
		private int instructionsRemovedCount = 0;
		private int simplifyExtendedMoveCount = 0;
		private int simplifySubtractionCount = 0;
		private int strengthReductionMultiplicationCount = 0;
		private int strengthReductionDivisionCount = 0;
		private int strengthReductionIntegerAdditionAndSubstractionCount = 0;
		private int strengthReductionLogicalOperatorsCount = 0;
		private int constantFoldingIntegerOperationsCount = 0;
		private int simpleConstantPropagationCount = 0;
		private int simpleCopyPropagationCount = 0;
		private int constantFoldingIntegerCompareCount = 0;
		private int foldIntegerCompareBranchCount = 0;
		private int deadCodeEliminationCount = 0;
		private int simplifyIntegerCompareCount = 0;
		private int reduceTruncationAndExpansion = 0;
		private int constantFoldingAdditionAndSubstraction = 0;
		private int constantFoldingMultiplicationCount = 0;
		private int constantFoldingDivisionCount = 0;
		private int blockRemovedCount = 0;

		private Stack<int> worklist = new Stack<int>();

		private CompilerTrace trace;

		protected override void Run()
		{
			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			// initialize worklist
			foreach (BasicBlock block in BasicBlocks)
			{
				for (Context ctx = new Context(InstructionSet, block); !ctx.IsBlockEndInstruction; ctx.GotoNext())
				{
					if (ctx.IsEmpty)
						continue;

					if (ctx.ResultCount == 0 && ctx.OperandCount == 0)
						continue;

					Do(ctx);

					while (worklist.Count != 0)
					{
						int index = worklist.Pop();
						Context ctx2 = new Context(InstructionSet, index);
						Do(ctx2);
					}
				}
			}

			UpdateCounter("SSAOptimizations.IRInstructionRemoved", instructionsRemovedCount);

			UpdateCounter("SSAOptimizations.SimplifyExtendedMove", simplifyExtendedMoveCount);
			UpdateCounter("SSAOptimizations.SimplifySubtraction", simplifySubtractionCount);
			UpdateCounter("SSAOptimizations.StrengthReductionMultiplication", strengthReductionMultiplicationCount);
			UpdateCounter("SSAOptimizations.StrengthReductionDivision", strengthReductionDivisionCount);
			UpdateCounter("SSAOptimizations.StrengthReductionIntegerAdditionAndSubstraction", strengthReductionIntegerAdditionAndSubstractionCount);
			UpdateCounter("SSAOptimizations.StrengthReductionLogicalOperators", strengthReductionLogicalOperatorsCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingIntegerOperations", constantFoldingIntegerOperationsCount);
			UpdateCounter("SSAOptimizations.SimpleConstantPropagation", simpleConstantPropagationCount);
			UpdateCounter("SSAOptimizations.SimpleCopyPropagation", simpleCopyPropagationCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingIntegerCompare", constantFoldingIntegerCompareCount);
			UpdateCounter("SSAOptimizations.FoldIntegerCompareBranch", foldIntegerCompareBranchCount);
			UpdateCounter("SSAOptimizations.DeadCodeElimination", deadCodeEliminationCount);
			UpdateCounter("SSAOptimizations.SimplifyIntegerCompareCount", simplifyIntegerCompareCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingAdditionAndSubstraction", constantFoldingAdditionAndSubstraction);
			UpdateCounter("SSAOptimizations.ConstantFoldingMultiplicationCount", constantFoldingMultiplicationCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingDivisionCount", constantFoldingDivisionCount);
			UpdateCounter("SSAOptimizations.ReduceTruncationAndExpansion", reduceTruncationAndExpansion);
			UpdateCounter("SSAOptimizations.BlockRemoved", blockRemovedCount);

			worklist = null;
		}

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
			ConstantMove(context);
			ConstantFoldingAdditionAndSubstraction(context);
			ConstantFoldingMultiplication(context);
			ConstantFoldingDivision(context);
			SimplifyIntegerCompare(context);
			DeadCodeElimination(context);
			ConstantFoldingIntegerCompare(context);
			FoldIntegerCompareBranch(context);
			ReduceTruncationAndExpansion(context);
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

			if (context.Instruction == IRInstruction.Move && context.Operand1.IsVirtualRegister && context.Operand1 == context.Result)
			{
				if (trace.Active) trace.Log("*** DeadCodeElimination");
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
				deadCodeEliminationCount++;
				//context.Remove();
				return;
			}

			if (context.Result.Uses.Count != 0 || context.Instruction == IRInstruction.Call || context.Instruction == IRInstruction.IntrinsicMethodCall)
				return;

			if (trace.Active) trace.Log("*** DeadCodeElimination");
			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			deadCodeEliminationCount++;
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

			if (context.Instruction != IRInstruction.Move)
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
				Context ctx = new Context(InstructionSet, index);

				if (ctx.Instruction == IRInstruction.AddressOf)
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
						simpleConstantPropagationCount++;
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}

				if (propogated)
					AddToWorkList(index);
			}
		}

		private bool CanCopyPropagation(Operand result, Operand destination)
		{
			if (result.IsPointer && destination.IsPointer)
				return true;

			if (result.IsReferenceType && destination.IsReferenceType)
				return true;

			if (result.Type.IsArray & destination.Type.IsArray & result.Type.ElementType == destination.Type.ElementType)
				return true;

			if (result.Type.IsUI1 & destination.Type.IsUI1)
				return true;

			if (result.Type.IsUI2 & destination.Type.IsUI2)
				return true;

			if (result.Type.IsUI4 & destination.Type.IsUI4)
				return true;

			if (result.Type.IsUI8 & destination.Type.IsUI8)
				return true;

			if (result.IsValueType || destination.IsValueType)
				return false;

			if (result.Type == destination.Type)
				return true;

			return false;
		}

		/// <summary>
		/// Simple copy propagation.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimpleCopyPropagation(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.Move)
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
				Context ctx = new Context(InstructionSet, index);

				if (ctx.Instruction == IRInstruction.AddressOf)
					return;
			}

			AddOperandUsageToWorkList(context);

			foreach (int index in destinationOperand.Uses.ToArray())
			{
				Context ctx = new Context(InstructionSet, index);

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (destinationOperand == operand)
					{
						if (trace.Active) trace.Log("*** SimpleCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetOperand(i, sourceOperand);
						simpleCopyPropagationCount++;
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}

			Debug.Assert(destinationOperand.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;

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

			if (!(context.Instruction == IRInstruction.AddSigned || context.Instruction == IRInstruction.AddUnsigned ||
				  context.Instruction == IRInstruction.SubSigned || context.Instruction == IRInstruction.SubUnsigned ||
				  context.Instruction == IRInstruction.LogicalAnd || context.Instruction == IRInstruction.LogicalOr ||
				  context.Instruction == IRInstruction.LogicalXor ||
				  context.Instruction == IRInstruction.MulSigned || context.Instruction == IRInstruction.MulUnsigned ||
				  context.Instruction == IRInstruction.DivSigned || context.Instruction == IRInstruction.DivUnsigned ||
				  context.Instruction == IRInstruction.ArithmeticShiftRight ||
				  context.Instruction == IRInstruction.ShiftLeft || context.Instruction == IRInstruction.ShiftRight
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
			if ((context.Instruction == IRInstruction.DivSigned || context.Instruction == IRInstruction.DivUnsigned) && op2.IsConstant && op2.IsConstantZero)
				return;

			Operand constant = null;

			if (context.Instruction == IRInstruction.AddSigned || context.Instruction == IRInstruction.AddUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger + op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.SubSigned || context.Instruction == IRInstruction.SubUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger - op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.LogicalAnd)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger & op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.LogicalOr)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger | op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.LogicalXor)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger ^ op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.MulSigned || context.Instruction == IRInstruction.MulUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger * op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.DivSigned || context.Instruction == IRInstruction.DivUnsigned)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger / op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.ArithmeticShiftRight)
			{
				constant = Operand.CreateConstant(result.Type, ((long)op1.ConstantUnsignedInteger) >> (int)op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.ShiftRight)
			{
				constant = Operand.CreateConstant(result.Type, ((ulong)op1.ConstantUnsignedInteger) >> (int)op2.ConstantUnsignedInteger);
			}
			else if (context.Instruction == IRInstruction.ShiftLeft)
			{
				constant = Operand.CreateConstant(result.Type, op1.ConstantUnsignedInteger << (int)op2.ConstantUnsignedInteger);
			}

			if (constant == null)
				return;

			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** ConstantFoldingIntegerOperations");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, context.Result, constant);
			constantFoldingIntegerOperationsCount++;
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
		}

		/// <summary>
		/// Folds the integer compare on constants
		/// </summary>
		/// <param name="context">The context.</param>
		private void ConstantFoldingIntegerCompare(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.IntegerCompare)
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
			constantFoldingIntegerCompareCount++;
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

			if (!(context.Instruction == IRInstruction.AddSigned
				|| context.Instruction == IRInstruction.AddUnsigned
				|| context.Instruction == IRInstruction.SubSigned
				|| context.Instruction == IRInstruction.SubUnsigned))
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
				strengthReductionIntegerAdditionAndSubstractionCount++;
				return;
			}

			if (op2.IsConstant && !op1.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionIntegerAdditionAndSubstraction");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op1);
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				strengthReductionIntegerAdditionAndSubstractionCount++;
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

			if (!(context.Instruction == IRInstruction.MulSigned || context.Instruction == IRInstruction.MulUnsigned || context.Instruction == IRInstruction.MulFloat))
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
				strengthReductionMultiplicationCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				strengthReductionMultiplicationCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op1.IsConstant && op1.IsConstantOne)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op2);
				strengthReductionMultiplicationCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}

			if (op2.IsConstant && op2.IsConstantOne)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op1);
				strengthReductionMultiplicationCount++;
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

			if (!(context.Instruction == IRInstruction.DivSigned || context.Instruction == IRInstruction.DivUnsigned))
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
				strengthReductionDivisionCount++;
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
				strengthReductionDivisionCount++;
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

			if (!(context.Instruction == IRInstruction.ZeroExtendedMove || context.Instruction == IRInstruction.SignExtendedMove))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;

			if (!op1.IsConstant)
				return;

			Operand newOperand;

			if (context.Instruction == IRInstruction.ZeroExtendedMove && result.IsUnsigned && op1.IsSigned)
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
			simplifyExtendedMoveCount++;
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

			if (!(context.Instruction == IRInstruction.SubSigned || context.Instruction == IRInstruction.SubUnsigned))
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
			simplifySubtractionCount++;
		}

		/// <summary>
		/// Strength reduction for logical operators
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionLogicalOperators(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.LogicalAnd || context.Instruction == IRInstruction.LogicalOr))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (context.Instruction == IRInstruction.LogicalOr && op1.IsConstant && !op2.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op2.ConstantSignedInteger));
				strengthReductionLogicalOperatorsCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			}

			if (context.Instruction == IRInstruction.LogicalOr && op2.IsConstant && !op1.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, op1.ConstantSignedInteger));
				strengthReductionLogicalOperatorsCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			}

			if (context.Instruction == IRInstruction.LogicalAnd && op1.IsConstant && !op1.IsConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				strengthReductionLogicalOperatorsCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			}

			if (context.Instruction == IRInstruction.LogicalAnd && op2.IsConstant && !op1.IsConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
				strengthReductionLogicalOperatorsCount++;
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

			if (context.Instruction != IRInstruction.IntegerCompareBranch)
				return;

			if (context.OperandCount != 2)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (!op1.IsConstant || !op2.IsConstant)
				return;

			Debug.Assert(context.Next.Instruction == IRInstruction.Jmp);

			if (context.BranchTargets[0] == context.Next.BranchTargets[0])
			{
				if (trace.Active) trace.Log("*** FoldIntegerCompareBranch-1");
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
				foldIntegerCompareBranchCount++;
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

			BasicBlock target;

			if (trace.Active) trace.Log("*** FoldIntegerCompareBranch-2");

			if (compareResult)
			{
				target = BasicBlocks.GetByLabel(context.Next.BranchTargets[0]);

				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());

				// change to JMP
				context.SetInstruction(IRInstruction.Jmp, BasicBlocks.GetByLabel(context.BranchTargets[0]));
				if (trace.Active) trace.Log("AFTER:\t" + context.ToString());

				// goto next instruction and prepare to remove it
				context.GotoNext();
			}
			else
			{
				target = BasicBlocks.GetByLabel(context.BranchTargets[0]);
			}

			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			foldIntegerCompareBranchCount++;

			// Goto the beginning of the block, to get to the first index of the block
			Context first = context.Clone();
			first.GotoFirst(); // FIXME: use block start index

			// Find block based on first index
			BasicBlock currentBlock = null;
			foreach (var block in BasicBlocks)
			{
				if (block.StartIndex == first.Index)
				{
					currentBlock = block;
					break;
				}
			}

			currentBlock.NextBlocks.Remove(target);
			target.PreviousBlocks.Remove(currentBlock);

			// if target block no longer has any predecessors (or the only predecessor is itself), remove all instructions from it.
			CheckAndClearEmptyBlock(target);
		}

		private void CheckAndClearEmptyBlock(BasicBlock block)
		{
			if (block.PreviousBlocks.Count != 0 || BasicBlocks.HeadBlocks.Contains(block))
				return;

			if (trace.Active) trace.Log("*** RemoveBlock");
			if (trace.Active) trace.Log("    Block: " + block.ToString());

			blockRemovedCount++;

			var nextBlocks = new List<BasicBlock>(block.NextBlocks.Count);

			foreach (var next in block.NextBlocks)
			{
				next.PreviousBlocks.Remove(block);
				nextBlocks.Add(next);
			}

			block.NextBlocks.Clear();

			for (Context context = new Context(InstructionSet, block); !context.IsBlockEndInstruction; context.GotoNext())
			{
				if (context.IsEmpty)
					continue;

				if (context.IsBlockStartInstruction)
					continue;

				if (context.Instruction == IRInstruction.Nop)
					continue;

				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
			}

			foreach (var next in nextBlocks)
			{
				CheckAndClearEmptyBlock(next);
			}
		}

		/// <summary>
		/// Simplifies the integer compare.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimplifyIntegerCompare(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.IntegerCompare)
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (context.Result.Uses.Count != 1)
				return;

			if (context.ConditionCode != ConditionCode.Equal)
				return;

			Context ctx = new Context(InstructionSet, context.Result.Uses[0]);

			if (ctx.Instruction != IRInstruction.IntegerCompare)
				return;

			if (ctx.ConditionCode != ConditionCode.Equal)
				return;

			if (!ctx.Operand2.IsConstant)
				return;

			if (!ctx.Operand2.IsConstantZero)
				return;

			Debug.Assert(ctx.Result.Definitions.Count == 1);

			if (trace.Active) trace.Log("*** SimplifyIntegerCompare");
			AddOperandUsageToWorkList(ctx);
			if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Move, ctx.Result, context.Result);
			simplifyIntegerCompareCount++;
			if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
		}

		private void ConstantMove(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.AddSigned || context.Instruction == IRInstruction.AddUnsigned
				|| context.Instruction == IRInstruction.MulSigned || context.Instruction == IRInstruction.MulUnsigned))
				return;

			if (!context.Operand1.IsConstant || context.Operand2.IsConstant)
				return;

			if (trace.Active) trace.Log("*** ConstantMove");
			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			var tmp = context.Operand1;
			context.Operand1 = context.Operand2;
			context.Operand2 = tmp;
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
		}

		private void ConstantFoldingAdditionAndSubstraction(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.AddSigned || context.Instruction == IRInstruction.AddUnsigned
				|| context.Instruction == IRInstruction.SubSigned || context.Instruction == IRInstruction.SubUnsigned))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand2.IsConstant)
				return;

			if (context.Result.Uses.Count != 1)
				return;

			Context ctx = new Context(InstructionSet, context.Result.Uses[0]);

			if (!(ctx.Instruction == IRInstruction.AddSigned || ctx.Instruction == IRInstruction.AddUnsigned
				|| ctx.Instruction == IRInstruction.SubSigned || ctx.Instruction == IRInstruction.SubUnsigned))
				return;

			if (!ctx.Result.IsVirtualRegister)
				return;

			if (!ctx.Operand2.IsConstant)
				return;

			if (ctx.Result.Definitions.Count != 1)
				return;

			bool add = true;

			if ((context.Instruction == IRInstruction.AddSigned || context.Instruction == IRInstruction.AddUnsigned) &&
				(ctx.Instruction == IRInstruction.SubSigned || ctx.Instruction == IRInstruction.SubUnsigned))
			{
				add = false;
			}
			else if ((context.Instruction == IRInstruction.SubSigned || context.Instruction == IRInstruction.SubUnsigned) &&
				(ctx.Instruction == IRInstruction.AddSigned || ctx.Instruction == IRInstruction.AddUnsigned))
			{
				add = false;
			}

			ulong r = add ? context.Operand2.ConstantUnsignedInteger + ctx.Operand2.ConstantUnsignedInteger :
				context.Operand2.ConstantUnsignedInteger - ctx.Operand2.ConstantUnsignedInteger;

			Debug.Assert(ctx.Result.Definitions.Count == 1);

			if (trace.Active) trace.Log("*** ConstantFoldingAdditionAndSubstraction");
			AddOperandUsageToWorkList(ctx);
			if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Move, ctx.Result, context.Result);
			if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.Operand2 = Operand.CreateConstant(context.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			constantFoldingAdditionAndSubstraction++;
		}

		private void ConstantFoldingMultiplication(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.MulSigned || context.Instruction == IRInstruction.MulUnsigned))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand2.IsConstant)
				return;

			if (context.Result.Uses.Count != 1)
				return;

			Context ctx = new Context(InstructionSet, context.Result.Uses[0]);

			if (!(ctx.Instruction == IRInstruction.MulSigned || ctx.Instruction == IRInstruction.MulUnsigned))
				return;

			if (!ctx.Result.IsVirtualRegister)
				return;

			if (!ctx.Operand2.IsConstant)
				return;

			if (ctx.Result.Definitions.Count != 1)
				return;

			ulong r = context.Operand2.ConstantUnsignedInteger * ctx.Operand2.ConstantUnsignedInteger;

			if (trace.Active) trace.Log("*** ConstantFoldingMultiplication");
			AddOperandUsageToWorkList(ctx);
			if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Move, ctx.Result, context.Result);
			if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.Operand2 = Operand.CreateConstant(context.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			constantFoldingMultiplicationCount++;
		}

		private void ConstantFoldingDivision(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.DivSigned || context.Instruction == IRInstruction.DivUnsigned))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand2.IsConstant)
				return;

			if (context.Result.Uses.Count != 1)
				return;

			Context ctx = new Context(InstructionSet, context.Result.Uses[0]);

			if (!(ctx.Instruction == IRInstruction.DivSigned || ctx.Instruction == IRInstruction.DivUnsigned))
				return;

			if (!ctx.Result.IsVirtualRegister)
				return;

			if (!ctx.Operand2.IsConstant)
				return;

			if (ctx.Result.Definitions.Count != 1)
				return;

			ulong r = context.Operand2.ConstantUnsignedInteger * ctx.Operand2.ConstantUnsignedInteger;

			Debug.Assert(ctx.Result.Definitions.Count == 1);

			if (trace.Active) trace.Log("*** ConstantFoldingDivision");
			AddOperandUsageToWorkList(ctx);
			if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Move, ctx.Result, context.Result);
			if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.Operand2 = Operand.CreateConstant(context.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			constantFoldingDivisionCount++;
		}

		private void ReduceTruncationAndExpansion(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.ZeroExtendedMove)
				return;

			if (!context.Operand1.IsVirtualRegister)
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (context.Result.Uses.Count != 1 || context.Result.Definitions.Count != 1)
				return;

			if (context.Operand1.Uses.Count != 1 || context.Operand1.Definitions.Count != 1)
				return;

			Context ctx = new Context(InstructionSet, context.Operand1.Definitions[0]);

			if (ctx.Instruction != IRInstruction.Move)
				return;

			if (ctx.Result.Uses.Count != 1 || ctx.Result.Definitions.Count != 1)
				return;

			if (ctx.Operand1.Type != context.Result.Type)
				return;

			if (trace.Active) trace.Log("*** ReduceTruncationAndExpansion");
			AddOperandUsageToWorkList(ctx);
			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
			ctx.Result = context.Result;
			if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
			if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());
			context.SetInstruction(IRInstruction.Nop);
			reduceTruncationAndExpansion++;
			instructionsRemovedCount++;
		}
	}
}