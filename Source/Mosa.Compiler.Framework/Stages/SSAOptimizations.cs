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
		private int simplifyExtendedMoveWithConstantCount = 0;
		private int simplifySubtractionCount = 0;
		private int strengthReductionMultiplicationCount = 0;
		private int strengthReductionDivisionCount = 0;
		private int strengthReductionIntegerAdditionAndSubstractionCount = 0;
		private int strengthReductionLogicalOperatorsCount = 0;
		private int constantFoldingIntegerOperationsCount = 0;
		private int simpleConstantPropagationCount = 0;
		private int simpleForwardCopyPropagationCount = 0;
		private int simpleBackwardCopyPropagation = 0;
		private int constantFoldingIntegerCompareCount = 0;
		private int strengthReductionIntegerCompareBranchCount = 0;
		private int deadCodeEliminationCount = 0;
		private int simplifyIntegerCompareCount = 0;
		private int reduceTruncationAndExpansion = 0;
		private int constantFoldingAdditionAndSubstraction = 0;
		private int constantFoldingMultiplicationCount = 0;
		private int constantFoldingDivisionCount = 0;
		private int blockRemovedCount = 0;
		private int foldIntegerCompareBranch = 0;
		private int reduceZeroExtendedMove = 0;
		private int foldIntegerCompare = 0;
		private int simplifyExtendedMove = 0;
		private int foldLoadStoreOffsets = 0;

		private Stack<int> worklist = new Stack<int>();

		private SectionTrace trace;

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count == 0)
				return;

			// Unable to optimize SSA w/ exceptions or finally handlers present
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			trace = CreateTrace();

			PromoteLocalVariable();

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

					ProcessWorkList();
				}
			}

			while (PromoteLocalVariable())
			{
				ProcessWorkList();
			}

			UpdateCounter("SSAOptimizations.IRInstructionRemoved", instructionsRemovedCount);
			UpdateCounter("SSAOptimizations.SimplifyExtendedMoveWithConstant", simplifyExtendedMoveWithConstantCount);
			UpdateCounter("SSAOptimizations.SimplifySubtraction", simplifySubtractionCount);
			UpdateCounter("SSAOptimizations.StrengthReductionMultiplication", strengthReductionMultiplicationCount);
			UpdateCounter("SSAOptimizations.StrengthReductionDivision", strengthReductionDivisionCount);
			UpdateCounter("SSAOptimizations.StrengthReductionIntegerAdditionAndSubstraction", strengthReductionIntegerAdditionAndSubstractionCount);
			UpdateCounter("SSAOptimizations.StrengthReductionLogicalOperators", strengthReductionLogicalOperatorsCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingIntegerOperations", constantFoldingIntegerOperationsCount);
			UpdateCounter("SSAOptimizations.SimpleConstantPropagation", simpleConstantPropagationCount);
			UpdateCounter("SSAOptimizations.SimpleForwardCopyPropagation", simpleForwardCopyPropagationCount);
			UpdateCounter("SSAOptimizations.SimpleBackwardCopyPropagation", simpleBackwardCopyPropagation);
			UpdateCounter("SSAOptimizations.ConstantFoldingIntegerCompare", constantFoldingIntegerCompareCount);
			UpdateCounter("SSAOptimizations.StrengthReductionIntegerCompareBranch", strengthReductionIntegerCompareBranchCount);
			UpdateCounter("SSAOptimizations.DeadCodeElimination", deadCodeEliminationCount);
			UpdateCounter("SSAOptimizations.SimplifyIntegerCompareCount", simplifyIntegerCompareCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingAdditionAndSubstraction", constantFoldingAdditionAndSubstraction);
			UpdateCounter("SSAOptimizations.ConstantFoldingMultiplicationCount", constantFoldingMultiplicationCount);
			UpdateCounter("SSAOptimizations.ConstantFoldingDivisionCount", constantFoldingDivisionCount);
			UpdateCounter("SSAOptimizations.ReduceTruncationAndExpansion", reduceTruncationAndExpansion);
			UpdateCounter("SSAOptimizations.FoldIntegerCompareBranch", foldIntegerCompareBranch);
			UpdateCounter("SSAOptimizations.FoldIntegerCompare", foldIntegerCompare);
			UpdateCounter("SSAOptimizations.SimplifyExtendedMove", simplifyExtendedMove);
			UpdateCounter("SSAOptimizations.ReduceZeroExtendedMove", reduceZeroExtendedMove);
			UpdateCounter("SSAOptimizations.FoldLoadStoreOffsets", foldLoadStoreOffsets);
			UpdateCounter("SSAOptimizations.BlockRemoved", blockRemovedCount);

			worklist = null;
		}

		private void ProcessWorkList()
		{
			while (worklist.Count != 0)
			{
				int index = worklist.Pop();
				Context ctx2 = new Context(InstructionSet, index);
				Do(ctx2);
			}
		}

		private void Do(Context context)
		{
			if (context.IsEmpty)
				return;

			SimpleConstantPropagation(context);
			SimpleForwardCopyPropagation(context);
			SimpleBackwardCopyPropagation(context);
			DeadCodeElimination(context);
			DeadCodeElimination2(context);
			SimplifySubtraction(context);
			ConstantMove(context);
			StrengthReductionMultiplication(context);
			StrengthReductionDivision(context);
			StrengthReductionIntegerAdditionAndSubstraction(context);
			StrengthReductionLogicalOperators(context);
			StrengthReductionShiftOperators(context);
			ConstantFoldingIntegerOperations(context);
			ReduceZeroExtendedMove(context);
			ConstantFoldingAdditionAndSubstraction(context);
			ConstantFoldingMultiplication(context);
			ConstantFoldingDivision(context);
			SimplifyIntegerCompare(context);
			ConstantFoldingIntegerCompare(context);
			FoldIntegerCompare(context);
			FoldIntegerCompareBranch(context);
			SimplifyExtendedMoveWithConstant(context);
			StrengthReductionIntegerCompareBranch(context);
			ReduceTruncationAndExpansion(context);
			SimplifyExtendedMove(context);
			FoldLoadStoreOffsets(context);
		}

		/// <summary>
		/// Adds to work list.
		/// </summary>
		/// <param name="index">The index.</param>
		private void AddToWorkList(int index)
		{
			// work list stays small, so the check is inexpensive
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

		private bool ContainsAddressOf(Operand local)
		{
			foreach (int index in local.Uses)
			{
				Context ctx = new Context(InstructionSet, index);

				if (ctx.Instruction == IRInstruction.AddressOf)
					return true;
			}

			return false;
		}

		private bool PromoteLocalVariable()
		{
			bool promoted = false;

			trace = CreateTrace();

			foreach (var local in MethodCompiler.LocalVariables)
			{
				if (local.IsVirtualRegister)
					continue;

				if (local.Definitions.Count != 1)
					continue;

				if (!local.IsReferenceType && !local.IsInteger && !local.IsR && !local.IsChar && !local.IsBoolean && !local.IsPointer && !local.IsI && !local.IsU)
					continue;

				if (ContainsAddressOf(local))
					continue;

				promoted = true;
				PromoteTempVariable(local);
			}

			return promoted;
		}

		private void PromoteTempVariable(Operand local)
		{
			var stacktype = local.Type.GetStackType();

			var v = MethodCompiler.CreateVirtualRegister(stacktype);

			if (trace.Active) trace.Log("*** PromoteTempVariable");
			if (trace.Active) trace.Log("Replacing: " + local.ToString() + " with " + v.ToString());

			foreach (int index in local.Uses.ToArray())
			{
				Context ctx = new Context(InstructionSet, index);

				AddOperandUsageToWorkList(ctx);

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (local == operand)
					{
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetOperand(i, v);
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}

			foreach (int index in local.Definitions.ToArray())
			{
				Context ctx = new Context(InstructionSet, index);
				AddOperandUsageToWorkList(ctx);

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetResult(i);

					if (local == operand)
					{
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetResult(i, v);
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}
		}

		private void DeadCodeElimination2(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.ResultCount != 1)
				return;

			if (context.Result.IsVirtualRegister)
				return;

			if (!context.Result.IsStackLocal)
				return;

			if (context.Result.Uses.Count != 0)
				return;

			if (context.Instruction == IRInstruction.Call || context.Instruction == IRInstruction.IntrinsicMethodCall)
				return;

			if (trace.Active) trace.Log("*** DeadCodeElimination-2");
			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			deadCodeEliminationCount++;
			return;
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

			if (context.Instruction == IRInstruction.Call || context.Instruction == IRInstruction.IntrinsicMethodCall)
				return;

			if (context.Instruction == IRInstruction.Move && context.Operand1.IsVirtualRegister && context.Operand1 == context.Result)
			{
				if (trace.Active) trace.Log("*** DeadCodeElimination");
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
				deadCodeEliminationCount++;
				return;
			}

			if (context.Result.Uses.Count != 0)
				return;

			if (trace.Active) trace.Log("*** DeadCodeElimination");
			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			deadCodeEliminationCount++;
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

			Operand destination = context.Result;
			Operand source = context.Operand1;

			// for each statement T that uses operand, substituted c in statement T
			foreach (int index in destination.Uses.ToArray())
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
						ctx.SetOperand(i, source);
						simpleConstantPropagationCount++;
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}

				if (propogated)
					AddToWorkList(index);
			}
		}

		private bool CanCopyPropagation(Operand source, Operand destination)
		{
			if (source.IsPointer && destination.IsPointer)
				return true;

			if (source.IsReferenceType && destination.IsReferenceType)
				return true;

			if (source.Type.IsArray & destination.Type.IsArray & source.Type.ElementType == destination.Type.ElementType)
				return true;

			if (source.Type.IsUI1 & destination.Type.IsUI1)
				return true;

			if (source.Type.IsUI2 & destination.Type.IsUI2)
				return true;

			if (source.Type.IsUI4 & destination.Type.IsUI4)
				return true;

			if (source.Type.IsUI8 & destination.Type.IsUI8)
				return true;

			if (NativePointerSize == 4 && (destination.IsI || destination.IsU || destination.IsPointer) && (source.IsI4 || source.IsU4))
				return true;

			if (NativePointerSize == 4 && (source.IsI || source.IsU || source.IsPointer) && (destination.IsI4 || destination.IsU4))
				return true;

			if (NativePointerSize == 8 && (destination.IsI || destination.IsU || destination.IsPointer) && (source.IsI8 || source.IsU8))
				return true;

			if (NativePointerSize == 8 && (source.IsI || source.IsU || source.IsPointer) && (destination.IsI8 || destination.IsU8))
				return true;

			if (source.IsR4 && destination.IsR4)
				return true;

			if (source.IsR8 && destination.IsR8)
				return true;

			if (source.IsI && destination.IsI)
				return true;

			if (source.IsValueType || destination.IsValueType)
				return false;

			if (source.Type == destination.Type)
				return true;

			return false;
		}

		/// <summary>
		/// Simple copy propagation.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimpleForwardCopyPropagation(Context context)
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
			// Also if the operand sign is different, we cannot do it as it requires a signed/unsigned extended move, not a normal move
			if (!CanCopyPropagation(context.Result, context.Operand1))
				return;

			Operand destination = context.Result;
			Operand source = context.Operand1;

			if (ContainsAddressOf(destination))
				return;

			// for each statement T that uses operand, substituted c in statement T
			AddOperandUsageToWorkList(context);

			foreach (int index in destination.Uses.ToArray())
			{
				Context ctx = new Context(InstructionSet, index);

				for (int i = 0; i < ctx.OperandCount; i++)
				{
					Operand operand = ctx.GetOperand(i);

					if (destination == operand)
					{
						if (trace.Active) trace.Log("*** SimpleForwardCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
						ctx.SetOperand(i, source);
						simpleForwardCopyPropagationCount++;
						if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
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
				  context.Instruction == IRInstruction.ShiftLeft || context.Instruction == IRInstruction.ShiftRight))
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

			if (!(context.Instruction == IRInstruction.AddSigned || context.Instruction == IRInstruction.AddUnsigned
				|| context.Instruction == IRInstruction.SubSigned || context.Instruction == IRInstruction.SubUnsigned))
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
		private void SimplifyExtendedMoveWithConstant(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.ZeroExtendedMove || context.Instruction == IRInstruction.SignExtendedMove))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand1.IsConstant)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;

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
			if (trace.Active) trace.Log("*** SimplifyExtendedMoveWithConstant");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, result, newOperand);
			simplifyExtendedMoveWithConstantCount++;
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

			if (!context.Operand2.IsConstant)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (context.Instruction == IRInstruction.LogicalOr)
			{
				if (op2.IsConstantZero)
				{
					AddOperandUsageToWorkList(context);
					if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
					context.SetInstruction(IRInstruction.Move, result, op1);
					strengthReductionLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
					return;
				}
			}
			else if (context.Instruction == IRInstruction.LogicalAnd)
			{
				if (op2.IsConstantZero)
				{
					AddOperandUsageToWorkList(context);
					if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
					context.SetInstruction(IRInstruction.Move, result, Operand.CreateConstant(context.Result.Type, 0));
					strengthReductionLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
					return;
				}

				if ((result.IsI4 || result.IsU4) && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
				{
					AddOperandUsageToWorkList(context);
					if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
					context.SetInstruction(IRInstruction.Move, result, op1);
					strengthReductionLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
					return;
				}

				if ((result.IsI8 || result.IsU8) && op2.ConstantUnsignedInteger == 0xFFFFFFFFFFFFFFFF)
				{
					AddOperandUsageToWorkList(context);
					if (trace.Active) trace.Log("*** StrengthReductionLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
					context.SetInstruction(IRInstruction.Move, result, op1);
					strengthReductionLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
					return;
				}
			}
			// TODO: Add more strength reductions especially for AND w/ 0xFF, 0xFFFF, 0xFFFFFFFF, etc when source or destination are same or smaller
		}

		/// <summary>
		/// Strength reduction shift operators.
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionShiftOperators(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.ShiftLeft || context.Instruction == IRInstruction.ShiftRight || context.Instruction == IRInstruction.ArithmeticShiftRight))
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			if (!context.Operand2.IsConstant)
				return;

			Operand result = context.Result;
			Operand op1 = context.Operand1;
			Operand op2 = context.Operand2;

			if (op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(context);
				if (trace.Active) trace.Log("*** StrengthReductionShiftOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
				context.SetInstruction(IRInstruction.Move, result, op1);
				//strengthReductionLogicalOperatorsCount++;
				if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
				return;
			}
		}

		/// <summary>
		/// Strength reduction integer compare branch.
		/// </summary>
		/// <param name="context">The context.</param>
		private void StrengthReductionIntegerCompareBranch(Context context)
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
				if (trace.Active) trace.Log("*** StrengthReductionIntegerCompareBranch-1");
				if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
				AddOperandUsageToWorkList(context);
				context.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
				strengthReductionIntegerCompareBranchCount++;
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

			if (trace.Active) trace.Log("*** StrengthReductionIntegerCompareBranch-2");

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
			strengthReductionIntegerCompareBranchCount++;

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
				|| context.Instruction == IRInstruction.MulSigned || context.Instruction == IRInstruction.MulUnsigned
				|| context.Instruction == IRInstruction.LogicalAnd || context.Instruction == IRInstruction.LogicalOr
				|| context.Instruction == IRInstruction.LogicalXor))
				return;

			if (context.Operand2.IsConstant)
				return;

			if (!context.Operand1.IsConstant)
				return;

			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** ConstantMove");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			var op1 = context.Operand1;
			context.Operand1 = context.Operand2;
			context.Operand2 = op1;
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

			Debug.Assert(ctx.Result.Definitions.Count == 1);

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

			Debug.Assert(ctx.Result.Definitions.Count == 1);

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

			Debug.Assert(ctx.Result.Definitions.Count == 1);

			ulong r = context.Operand2.ConstantUnsignedInteger * ctx.Operand2.ConstantUnsignedInteger;

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

		private void ReduceZeroExtendedMove(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.ZeroExtendedMove)
				return;

			if (!context.Operand1.IsVirtualRegister)
				return;

			if (!context.Result.IsVirtualRegister)
				return;

			//if (!CanCopyPropagation(context.Result, context.Operand1))
			//	return;

			if (trace.Active) trace.Log("*** ReduceZeroExtendedMove");
			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			reduceZeroExtendedMove++;
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

			if (context.Result.Uses.Count != 1)
				return;

			Debug.Assert(context.Result.Definitions.Count == 1);

			if (context.Operand1.Uses.Count != 1 || context.Operand1.Definitions.Count != 1)
				return;

			Context ctx = new Context(InstructionSet, context.Operand1.Definitions[0]);

			if (ctx.Instruction != IRInstruction.Move)
				return;

			if (ctx.Result.Uses.Count != 1)
				return;

			Debug.Assert(ctx.Result.Definitions.Count == 1);

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

		private void FoldIntegerCompareBranch(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.IntegerCompareBranch)
				return;

			if (!(context.ConditionCode == ConditionCode.NotEqual || context.ConditionCode == ConditionCode.Equal))
				return;

			if (!((context.Operand1.IsVirtualRegister && context.Operand2.IsConstant && context.Operand2.IsConstantZero) ||
				(context.Operand2.IsVirtualRegister && context.Operand1.IsConstant && context.Operand1.IsConstantZero)))
				return;

			var operand = (context.Operand2.IsConstant && context.Operand2.IsConstantZero) ? context.Operand1 : context.Operand2;

			if (operand.Uses.Count != 1)
				return;

			Debug.Assert(operand.Definitions.Count == 1);

			Context ctx = new Context(InstructionSet, operand.Definitions[0]);

			if (ctx.Instruction != IRInstruction.IntegerCompare)
				return;

			AddOperandUsageToWorkList(ctx);
			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** FoldIntegerCompareBranch");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.ConditionCode = context.ConditionCode == ConditionCode.NotEqual ? ctx.ConditionCode : ctx.ConditionCode.GetOpposite();
			context.Operand1 = ctx.Operand1;
			context.Operand2 = ctx.Operand2;
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Nop);
			foldIntegerCompareBranch++;
			instructionsRemovedCount++;
		}

		private void FoldIntegerCompare(Context context)
		{
			if (context.IsEmpty)
				return;

			if (context.Instruction != IRInstruction.IntegerCompare)
				return;

			if (!(context.ConditionCode == ConditionCode.NotEqual || context.ConditionCode == ConditionCode.Equal))
				return;

			if (!((context.Operand1.IsVirtualRegister && context.Operand2.IsConstant && context.Operand2.IsConstantZero) ||
				(context.Operand2.IsVirtualRegister && context.Operand1.IsConstant && context.Operand1.IsConstantZero)))
				return;

			var operand = (context.Operand2.IsConstant && context.Operand2.IsConstantZero) ? context.Operand1 : context.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (operand.Definitions.Count != 1)
				return;

			Debug.Assert(operand.Definitions.Count == 1);

			Context ctx = new Context(InstructionSet, operand.Definitions[0]);

			if (ctx.Instruction != IRInstruction.IntegerCompare)
				return;

			AddOperandUsageToWorkList(ctx);
			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** FoldIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.ConditionCode = context.ConditionCode == ConditionCode.NotEqual ? ctx.ConditionCode : ctx.ConditionCode.GetOpposite();
			context.Operand1 = ctx.Operand1;
			context.Operand2 = ctx.Operand2;
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Nop);
			foldIntegerCompare++;
			instructionsRemovedCount++;
		}

		/// <summary>
		/// Simplifies sign/zero extended move
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimplifyExtendedMove(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.ZeroExtendedMove || context.Instruction == IRInstruction.SignExtendedMove))
				return;

			if (!context.Result.IsVirtualRegister || !context.Operand1.IsVirtualRegister)
				return;

			if (!((NativePointerSize == 4 && context.Result.IsI && (context.Operand1.IsI4 || context.Operand1.IsU4)) ||
				(NativePointerSize == 4 && context.Operand1.IsI && (context.Result.IsI4 || context.Result.IsU4)) ||
				(NativePointerSize == 8 && context.Result.IsI && (context.Operand1.IsI8 || context.Operand1.IsU8)) ||
				(NativePointerSize == 8 && context.Operand1.IsI && (context.Result.IsI8 || context.Result.IsU8))))
				return;

			AddOperandUsageToWorkList(context);
			if (trace.Active) trace.Log("*** SimplifyExtendedMove");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.SetInstruction(IRInstruction.Move, context.Result, context.Operand1);
			simplifyExtendedMove++;
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
		}

		/// <summary>
		/// Simple backward copy propagation.
		/// </summary>
		/// <param name="context">The context.</param>
		private void SimpleBackwardCopyPropagation(Context context)
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

			if (context.Operand1.Uses.Count != 1)
				return;

			// If the pointer or reference types are different, we can not copy propagation because type information would be lost.
			// Also if the operand sign is different, we cannot do it as it requires a signed/unsigned extended move, not a normal move
			if (!CanCopyPropagation(context.Result, context.Operand1))
				return;

			Operand destination = context.Result;
			Operand source = context.Operand1;

			//if (trace.IsLogging) trace.Log("REVIEWING:\t" + context.ToString());

			AddOperandUsageToWorkList(context);

			Context ctx = new Context(InstructionSet, source.Definitions[0]);

			for (int i = 0; i < ctx.OperandCount; i++)
			{
				Operand operand = ctx.GetOperand(i);

				if (destination == operand)
				{
					if (trace.Active) trace.Log("*** SimpleBackwardCopyPropagation");
					if (trace.Active) trace.Log("BEFORE:\t" + ctx.ToString());
					ctx.SetOperand(i, source);
					simpleBackwardCopyPropagation++;
					if (trace.Active) trace.Log("AFTER: \t" + ctx.ToString());
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + context.ToString());
			AddOperandUsageToWorkList(context);
			context.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
		}

		private void FoldLoadStoreOffsets(Context context)
		{
			if (context.IsEmpty)
				return;

			if (!(context.Instruction == IRInstruction.Load || context.Instruction == IRInstruction.Store
				|| context.Instruction == IRInstruction.LoadSignExtended || context.Instruction == IRInstruction.LoadZeroExtended))
				return;

			if (!context.Operand2.IsConstant)
				return;

			if (!context.Operand1.IsVirtualRegister)
				return;

			if (context.Operand1.Uses.Count != 1)
				return;

			Context ctx = new Context(InstructionSet, context.Operand1.Definitions[0]);

			if (!(ctx.Instruction == IRInstruction.AddSigned || ctx.Instruction == IRInstruction.SubSigned ||
				ctx.Instruction == IRInstruction.AddUnsigned || ctx.Instruction == IRInstruction.SubUnsigned))
				return;

			if (!ctx.Operand2.IsConstant)
				return;

			Operand constant;

			if (ctx.Instruction == IRInstruction.AddUnsigned || ctx.Instruction == IRInstruction.AddSigned)
			{
				constant = Operand.CreateConstant(context.Operand2.Type, ctx.Operand2.ConstantSignedInteger + context.Operand2.ConstantSignedInteger);
			}
			else
			{
				constant = Operand.CreateConstant(context.Operand2.Type, context.Operand2.ConstantSignedInteger - ctx.Operand2.ConstantSignedInteger);
			}

			AddOperandUsageToWorkList(context);
			AddOperandUsageToWorkList(ctx);
			if (trace.Active) trace.Log("*** FoldLoadStoreOffsets");
			if (trace.Active) trace.Log("BEFORE:\t" + context.ToString());
			context.Operand1 = ctx.Operand1;
			context.Operand2 = constant;
			if (trace.Active) trace.Log("AFTER: \t" + context.ToString());
			if (trace.Active) trace.Log("REMOVED:\t" + ctx.ToString());
			ctx.SetInstruction(IRInstruction.Nop);
			foldLoadStoreOffsets++;
			instructionsRemovedCount++;
		}
	}
}