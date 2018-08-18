// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Optimization Stage
	/// </summary>
	public sealed class IROptimizationStage : BaseMethodCompilerStage
	{
		private int arithmeticSimplificationAdditionAndSubstractionCount = 0;
		private int arithmeticSimplificationDivisionCount = 0;
		private int arithmeticSimplificationModulusCount = 0;
		private int arithmeticSimplificationMultiplicationCount = 0;
		private int arithmeticSimplificationShiftCount = 0;
		private int blockRemovedCount = 0;
		private int combineIntegerCompareBranchCount = 0;
		private int constantFoldingIntegerCompareCount = 0;
		private int constantFoldingIntegerCount = 0;
		private int constantFoldingPhiCount = 0;
		private int deadCodeEliminationCount = 0;
		private int deadCodeEliminationPhiCount = 0;
		private int expressionFoldingAdditionAndSubstractionCount = 0;
		private int expressionFoldingDivisionCount = 0;
		private int expressionFoldingLogicalAndCount = 0;
		private int expressionFoldingLogicalOrCount = 0;
		private int expressionFoldingMultiplicationCount = 0;
		private int foldIfThenElseCount = 0;
		private int foldIntegerCompareBranchCount = 0;
		private int foldIntegerCompareCount = 0;
		private int foldLoadStoreOffsetsCount = 0;
		private int propagateCompoundMoveCount = 0;
		private int propagateMoveCount = 0;
		private int instructionsRemovedCount = 0;
		private int longConstantFoldingCount = 0;
		private int longConstantReductionCount = 0;
		private int longPropagateCount = 0;
		private int removeUselessIntegerCompareBranchCount = 0;
		private int propagateConstantCount = 0;
		private int simplifyBranchComparisonCount = 0;
		private int simplifyExtendedMoveCount = 0;
		private int simplifyGeneralCount = 0;
		private int simplifyGetHighCount = 0;
		private int simplifyGetLowCount = 0;
		private int simplifyIntegerCompareCount = 0;
		private int simplifyParamLoadCount = 0;
		private int simplifyPhiCount = 0;
		private int strengthReductionIntegerCount = 0;

		private Stack<InstructionNode> worklist;

		private TraceLog trace;

		private delegate void Transformation(InstructionNode node);

		private List<Transformation> transformations;

		protected override void Initialize()
		{
			base.Initialize();

			worklist = new Stack<InstructionNode>();

			transformations = CreateTransformationList();
		}

		private List<Transformation> CreateTransformationList()
		{
			return new List<Transformation>()
			{
				PropagateConstant,
				PropagateMove,
				PropagateCompoundMove,
				DeadCodeElimination,
				ConstantFoldingInteger,
				StrengthReductionInteger,
				ArithmeticSimplificationMultiplication,
				ArithmeticSimplificationDivision,
				ArithmeticSimplificationRemUnsignedModulus,
				CombineIntegerCompareBranch,
				FoldIntegerCompare,
				RemoveUselessIntegerCompareBranch,
				ConstantFoldIntegerCompareBranch,
				SimplifyExtendedMoves,
				SimplifyIntegerCompare2,
				SimplifyIntegerCompare,
				SimplifyAddCarryOut,
				SimplifyAddCarryOut2,
				SimplifyAddWithCarry,
				SimplifySubCarryOut,
				SimplifyCompareBranch,
				SimplifyGetLow64,
				SimplifyGetHigh64,
				SimplifyGetLow64b,
				SimplifyGetHigh64b,
				SimplifyPhi,
				SimplifyPhi2,
				SimplifyParamLoadCount,
				GetHigh64Constant,
				GetLow64Constant,
				GetHigh64Propagation,
				GetLow64Propagation,
				FoldGetHigh64Constant,
				FoldGetLow64Constant,
				FoldGetLow64PointerConstant,
				FoldTo64Constant,
				FoldIfThenElse,
				FoldLoadStoreOffsets,
				ConstantFoldingPhi,
				DeadCodeEliminationPhi,
				ExpressionFoldingAdditionAndSubstraction,
				ExpressionFoldingMultiplication,
				ExpressionFoldingDivision,
				ExpressionFoldingLogicalOr,
				ExpressionFoldingLogicalAnd,
			};
		}

		protected override void Setup()
		{
			base.Setup();

			instructionsRemovedCount = 0;
			blockRemovedCount = 0;
			constantFoldingIntegerCount = 0;
			strengthReductionIntegerCount = 0;
			arithmeticSimplificationMultiplicationCount = 0;
			arithmeticSimplificationDivisionCount = 0;
			arithmeticSimplificationAdditionAndSubstractionCount = 0;
			arithmeticSimplificationShiftCount = 0;
			arithmeticSimplificationModulusCount = 0;
			expressionFoldingAdditionAndSubstractionCount = 0;
			expressionFoldingMultiplicationCount = 0;
			expressionFoldingDivisionCount = 0;
			expressionFoldingLogicalOrCount = 0;
			expressionFoldingLogicalAndCount = 0;
			propagateConstantCount = 0;
			propagateMoveCount = 0;
			propagateCompoundMoveCount = 0;
			deadCodeEliminationCount = 0;
			constantFoldingIntegerCompareCount = 0;
			constantFoldingPhiCount = 0;
			foldIntegerCompareBranchCount = 0;
			foldIntegerCompareCount = 0;
			simplifyExtendedMoveCount = 0;
			foldLoadStoreOffsetsCount = 0;
			simplifyPhiCount = 0;
			deadCodeEliminationPhiCount = 0;
			combineIntegerCompareBranchCount = 0;
			removeUselessIntegerCompareBranchCount = 0;
			longConstantReductionCount = 0;
			longPropagateCount = 0;
			longConstantFoldingCount = 0;
			simplifyIntegerCompareCount = 0;
			simplifyGeneralCount = 0;
			simplifyParamLoadCount = 0;
			simplifyBranchComparisonCount = 0;
			simplifyGetLowCount = 0;
			simplifyGetHighCount = 0;
		}

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (!HasCode)
				return;

			trace = CreateTraceLog();

			Optimize();
		}

		protected override void Finish()
		{
			base.Finish();

			UpdateCounter("IROptimizations.IRInstructionRemoved", instructionsRemovedCount);
			UpdateCounter("IROptimizations.BlockRemoved", blockRemovedCount);
			UpdateCounter("IROptimizations.PropagateConstant", propagateConstantCount);
			UpdateCounter("IROptimizations.PropagateMoveCount", propagateMoveCount);
			UpdateCounter("IROptimizations.ConstantFoldingInteger", constantFoldingIntegerCount);
			UpdateCounter("IROptimizations.StrengthReductionInteger", strengthReductionIntegerCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationMultiplication", arithmeticSimplificationMultiplicationCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationDivision", arithmeticSimplificationDivisionCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationAdditionAndSubstraction", arithmeticSimplificationAdditionAndSubstractionCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationShift", arithmeticSimplificationShiftCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationModulus", arithmeticSimplificationModulusCount);
			UpdateCounter("IROptimizations.ExpressionFoldingAdditionAndSubstraction", expressionFoldingAdditionAndSubstractionCount);
			UpdateCounter("IROptimizations.ExpressionFoldingMultiplication", expressionFoldingMultiplicationCount);
			UpdateCounter("IROptimizations.ExpressionFoldingDivision", expressionFoldingDivisionCount);
			UpdateCounter("IROptimizations.ExpressionFoldingLogicalOr", expressionFoldingLogicalOrCount);
			UpdateCounter("IROptimizations.ExpressionFoldingLogicalAnd", expressionFoldingLogicalAndCount);
			UpdateCounter("IROptimizations.ConstantFoldingIntegerCompare", constantFoldingIntegerCompareCount);
			UpdateCounter("IROptimizations.ConstantFoldingPhi", constantFoldingPhiCount);
			UpdateCounter("IROptimizations.FoldIntegerCompareBranch", foldIntegerCompareBranchCount);
			UpdateCounter("IROptimizations.FoldIntegerCompare", foldIntegerCompareCount);
			UpdateCounter("IROptimizations.FoldLoadStoreOffsets", foldLoadStoreOffsetsCount);
			UpdateCounter("IROptimizations.DeadCodeElimination", deadCodeEliminationCount);
			UpdateCounter("IROptimizations.DeadCodeEliminationPhi", deadCodeEliminationPhiCount);
			UpdateCounter("IROptimizations.CombineIntegerCompareBranch", combineIntegerCompareBranchCount);
			UpdateCounter("IROptimizations.SimplifyExtendedMove", simplifyExtendedMoveCount);
			UpdateCounter("IROptimizations.SimplifyPhi", simplifyPhiCount);
			UpdateCounter("IROptimizations.SimplifyIntegerCompare", simplifyIntegerCompareCount);
			UpdateCounter("IROptimizations.SimplifyGeneral", simplifyGeneralCount);
			UpdateCounter("IROptimizations.SimplifyParamLoad", simplifyParamLoadCount);
			UpdateCounter("IROptimizations.SimplifyBranchComparison", simplifyBranchComparisonCount);
			UpdateCounter("IROptimizations.SimplifyGetLow", simplifyGetLowCount);
			UpdateCounter("IROptimizations.SimplifyGetHigh", simplifyGetHighCount);
			UpdateCounter("IROptimizations.RemoveUselessIntegerCompareBranch", removeUselessIntegerCompareBranchCount);
			UpdateCounter("IROptimizations.LongConstantReduction", longConstantReductionCount);
			UpdateCounter("IROptimizations.longPropagateCount", longPropagateCount);
			UpdateCounter("IROptimizations.LongConstantFolding", longConstantFoldingCount);
			UpdateCounter("IROptimizations.PropagateCompoundMoveCount", propagateCompoundMoveCount);

			worklist.Clear();
			trace = null;
		}

		private void Optimize()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmptyOrNop)
						continue;

					if (node.ResultCount == 0 && node.OperandCount == 0)
						continue;

					Do(node);

					ProcessWorkList();
				}
			}
		}

		private void ProcessWorkList()
		{
			while (worklist.Count != 0)
			{
				var node = worklist.Pop();
				Do(node);
			}
		}

		private void Do(InstructionNode node)
		{
			foreach (var method in transformations)
			{
				if (node.IsEmptyOrNop)
					return;

				if (node.ResultCount == 0 && node.OperandCount == 0)
					continue;

				method.Invoke(node);
			}
		}

		private void AddToWorkList(InstructionNode node)
		{
			if (node.IsEmptyOrNop)
				return;

			// work list stays small, so the check is inexpensive
			if (worklist.Contains(node))
				return;

			worklist.Push(node);
		}

		/// <summary>
		/// Adds the operand usage and definitions to work list.
		/// </summary>
		/// <param name="operand">The operand.</param>
		private void AddOperandUsageToWorkList(Operand operand)
		{
			if (!operand.IsVirtualRegister)
				return;

			foreach (var index in operand.Uses)
			{
				AddToWorkList(index);
			}

			foreach (var index in operand.Definitions)
			{
				AddToWorkList(index);
			}
		}

		/// <summary>
		/// Adds the all the operands usage and definitions to work list.
		/// </summary>
		/// <param name="node">The node.</param>
		private void AddOperandUsageToWorkList(InstructionNode node)
		{
			if (node.Result != null)
			{
				AddOperandUsageToWorkList(node.Result);
			}
			if (node.Result2 != null)
			{
				AddOperandUsageToWorkList(node.Result2);
			}
			foreach (var operand in node.Operands)
			{
				AddOperandUsageToWorkList(operand);
			}
		}

		private static BaseInstruction GetMoveInteger(Operand operand)
		{
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.MoveInt64 : IRInstruction.MoveInt32;
		}

		private static bool ValidateSSAForm(Operand operand)
		{
			return operand.Definitions.Count == 1;
		}

		private void DeadCodeElimination(InstructionNode node)
		{
			if (node.ResultCount == 0 || node.ResultCount > 2)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (node.ResultCount == 2 && !node.Result2.IsVirtualRegister)
				return;

			if (node.ResultCount == 2 && !ValidateSSAForm(node.Result2))
				return;

			if (node.Instruction == IRInstruction.CallDynamic
				|| node.Instruction == IRInstruction.CallInterface
				|| node.Instruction == IRInstruction.CallDirect
				|| node.Instruction == IRInstruction.CallStatic
				|| node.Instruction == IRInstruction.CallVirtual
				|| node.Instruction == IRInstruction.NewObject
				|| node.Instruction == IRInstruction.SetReturn32
				|| node.Instruction == IRInstruction.SetReturn64
				|| node.Instruction == IRInstruction.SetReturnR4
				|| node.Instruction == IRInstruction.SetReturnR8
				|| node.Instruction == IRInstruction.SetReturnCompound
				|| node.Instruction == IRInstruction.IntrinsicMethodCall)
				return;

			if ((node.Instruction == IRInstruction.MoveInt32 || node.Instruction == IRInstruction.MoveInt64)
				&& node.Operand1.IsVirtualRegister
				&& node.Operand1 == node.Result)
			{
				if (trace.Active) trace.Log("*** DeadCodeElimination");
				if (trace.Active) trace.Log("REMOVED:\t" + node);
				AddOperandUsageToWorkList(node);
				node.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
				deadCodeEliminationCount++;
				return;
			}

			if (node.Result.Uses.Count != 0)
				return;

			if (node.ResultCount == 2 && node.Result2.Uses.Count != 0)
				return;

			if (trace.Active) trace.Log("*** DeadCodeElimination");
			if (trace.Active) trace.Log("REMOVED:\t" + node);
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			deadCodeEliminationCount++;
		}

		private void PropagateConstant(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MoveInt32 || node.Instruction == IRInstruction.MoveInt64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			Operand destination = node.Result;
			Operand source = node.Operand1;

			// for each statement T that uses operand, substituted c in statement T
			foreach (var useNode in destination.Uses.ToArray())
			{
				if (useNode.Instruction == IRInstruction.AddressOf)
					continue;

				bool propogated = false;

				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (operand == destination)
					{
						propogated = true;

						if (trace.Active) trace.Log("*** PropagateConstant");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);

						AddOperandUsageToWorkList(operand);
						AddOperandUsageToWorkList(useNode.GetOperand(i));

						useNode.SetOperand(i, source);
						propagateConstantCount++;
						if (trace.Active) trace.Log("AFTER: \t" + useNode);
					}
				}

				if (propogated)
				{
					AddToWorkList(useNode);
					AddToWorkList(node);
				}
			}
		}

		private void PropagateMove(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MoveInt32
				|| node.Instruction == IRInstruction.MoveInt64
				|| node.Instruction == IRInstruction.MoveFloatR4
				|| node.Instruction == IRInstruction.MoveFloatR8))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			if (node.Operand1.IsResolvedConstant)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			var destination = node.Result;
			var source = node.Operand1;

			Debug.Assert(destination != source);

			// for each statement T that uses operand, substituted c in statement T
			AddOperandUsageToWorkList(node);

			foreach (var useNode in destination.Uses.ToArray())
			{
				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (destination == operand)
					{
						if (trace.Active) trace.Log("*** PropagateMove");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);
						useNode.SetOperand(i, source);
						propagateMoveCount++;
						if (trace.Active) trace.Log("AFTER: \t" + useNode);
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
		}

		private void PropagateCompoundMove(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.MoveCompound)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var destination = node.Result;
			var source = node.Operand1;

			Debug.Assert(destination != source);

			// for each statement T that uses operand, substituted c in statement T
			AddOperandUsageToWorkList(node);

			foreach (var useNode in destination.Uses.ToArray())
			{
				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (destination == operand)
					{
						if (trace.Active) trace.Log("*** PropagateCompoundMove");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);
						useNode.SetOperand(i, source);
						propagateCompoundMoveCount++;
						if (trace.Active) trace.Log("AFTER: \t" + useNode);
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
		}

		/// <summary>
		/// Strength reduction for multiplication when one of the constants is zero or one
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationMultiplication(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MulSigned32
				|| node.Instruction == IRInstruction.MulUnsigned32
				|| node.Instruction == IRInstruction.MulSigned64
				|| node.Instruction == IRInstruction.MulUnsigned64))
				return;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op2.IsResolvedConstant)
				return;

			if (IsPowerOfTwo(op2.ConstantUnsignedLongInteger))
			{
				int shift = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

				if (shift < 32 || (shift < 64 && result.Is64BitInteger))
				{
					AddOperandUsageToWorkList(node);
					if (trace.Active) trace.Log("*** ArithmeticSimplificationMultiplication");
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(Select(result, IRInstruction.ShiftLeft32, IRInstruction.ShiftLeft64), result, op1, CreateConstant((int)shift));
					arithmeticSimplificationMultiplicationCount++;
					if (trace.Active) trace.Log("AFTER: \t" + node);
					return;
				}
			}
		}

		private void ArithmeticSimplificationDivision(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.DivSigned32
				|| node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.DivSigned64
				|| node.Instruction == IRInstruction.DivUnsigned64))
				return;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (op2.IsConstantZero || op2.IsVirtualRegister)
				return;

			if ((node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivUnsigned64) && IsPowerOfTwo(op2.ConstantUnsignedLongInteger))
			{
				int shift = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

				if (shift < 32 || (shift < 64 && result.Is64BitInteger))
				{
					AddOperandUsageToWorkList(node);
					if (trace.Active) trace.Log("*** ArithmeticSimplificationDivision");
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(Select(result, IRInstruction.ShiftRight32, IRInstruction.ShiftLeft64), result, op1, CreateConstant((int)shift));
					arithmeticSimplificationDivisionCount++;
					if (trace.Active) trace.Log("AFTER: \t" + node);
					return;
				}
			}
		}

		private void RemoveUselessIntegerCompareBranch(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64))
				return;

			if (node.Block.NextBlocks.Count != 1)
				return;

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			removeUselessIntegerCompareBranchCount++;
		}

		private void ConstantFoldIntegerCompareBranch(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64))
				return;

			Debug.Assert(node.OperandCount == 2);

			var op1 = node.Operand1;
			var op2 = node.Operand2;
			var result = node.Result;

			if (!op1.IsResolvedConstant || !op2.IsResolvedConstant)
				return;

			InstructionNode nextNode = node.Next;

			if (nextNode.Instruction != IRInstruction.Jmp)
				return;

			if (node.BranchTargets[0] == nextNode.BranchTargets[0])
			{
				if (trace.Active) trace.Log("*** FoldIntegerCompareBranch-Useless");
				if (trace.Active) trace.Log("REMOVED:\t" + node);
				AddOperandUsageToWorkList(node);
				node.SetInstruction(IRInstruction.Nop);
				instructionsRemovedCount++;
				foldIntegerCompareBranchCount++;
				return;
			}

			bool compareResult = true;

			switch (node.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ConstantUnsignedLongInteger == op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsignedLongInteger != op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.LessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;

				case ConditionCode.UnsignedGreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.UnsignedGreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.UnsignedLessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.UnsignedLessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;

				// TODO: Add more
				default: return;
			}

			BasicBlock notTaken;
			InstructionNode notUsed;

			if (trace.Active) trace.Log("*** FoldIntegerCompareBranch");

			if (compareResult)
			{
				notTaken = nextNode.BranchTargets[0];
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(IRInstruction.Jmp, node.BranchTargets[0]);
				if (trace.Active) trace.Log("AFTER:\t" + node);

				notUsed = nextNode;
			}
			else
			{
				notTaken = node.BranchTargets[0];
				notUsed = node;
			}

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			AddOperandUsageToWorkList(notUsed);
			notUsed.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			foldIntegerCompareBranchCount++;

			// if target block no longer has any predecessors (or the only predecessor is itself), remove all instructions from it.
			CheckAndClearEmptyBlock(notTaken);
		}

		private void CheckAndClearEmptyBlock(BasicBlock block)
		{
			if (block.PreviousBlocks.Count != 0 || BasicBlocks.HeadBlocks.Contains(block))
				return;

			if (trace.Active) trace.Log("*** RemoveBlock: " + block);

			blockRemovedCount++;

			var nextBlocks = block.NextBlocks.ToArray();

			EmptyBlockOfAllInstructions(block);

			RemoveBlockFromPhiInstructions(block, nextBlocks);

			Debug.Assert(block.NextBlocks.Count == 0);
			Debug.Assert(block.PreviousBlocks.Count == 0);

			foreach (var next in nextBlocks)
			{
				CheckAndClearEmptyBlock(next);
			}
		}

		private void ExpressionFoldingAdditionAndSubstraction(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Add32
				|| node.Instruction == IRInstruction.Add64
				|| node.Instruction == IRInstruction.Sub32
				|| node.Instruction == IRInstruction.Sub64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.Add32
				|| node2.Instruction == IRInstruction.Add64
				|| node2.Instruction == IRInstruction.Sub32
				|| node2.Instruction == IRInstruction.Sub64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			bool add = true;

			if ((node.Instruction == IRInstruction.Add32 || node.Instruction == IRInstruction.Add64)
				&& (node2.Instruction == IRInstruction.Sub32 || node2.Instruction == IRInstruction.Sub64))
			{
				add = false;
			}
			else if ((node.Instruction == IRInstruction.Sub32 || node.Instruction == IRInstruction.Sub64)
				&& (node2.Instruction == IRInstruction.Add32 || node2.Instruction == IRInstruction.Add64))
			{
				add = false;
			}

			ulong r = add ? node.Operand2.ConstantUnsignedLongInteger + node2.Operand2.ConstantUnsignedLongInteger :
				node.Operand2.ConstantUnsignedLongInteger - node2.Operand2.ConstantUnsignedLongInteger;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			if (trace.Active) trace.Log("*** ExpressionFoldingAdditionAndSubstraction");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			expressionFoldingAdditionAndSubstractionCount++;
		}

		private void ExpressionFoldingLogicalOr(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LogicalOr32 || node.Instruction == IRInstruction.LogicalOr64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.LogicalOr32 || node2.Instruction == IRInstruction.LogicalOr64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = node.Operand2.ConstantUnsignedLongInteger | node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ExpressionFoldingLogicalOr");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			expressionFoldingLogicalOrCount++;
		}

		private void ExpressionFoldingLogicalAnd(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LogicalAnd32 || node.Instruction == IRInstruction.LogicalAnd64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.LogicalAnd32 || node2.Instruction == IRInstruction.LogicalAnd64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = node.Operand2.ConstantUnsignedLongInteger & node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ExpressionFoldingLogicalAnd");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			expressionFoldingLogicalAndCount++;
		}

		private void ExpressionFoldingMultiplication(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MulSigned32
				|| node.Instruction == IRInstruction.MulUnsigned32
				|| node.Instruction == IRInstruction.MulSigned64
				|| node.Instruction == IRInstruction.MulUnsigned64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.MulSigned32
				|| node2.Instruction == IRInstruction.MulUnsigned32
				|| node2.Instruction == IRInstruction.MulSigned64
				|| node2.Instruction == IRInstruction.MulUnsigned64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = node.Operand2.ConstantUnsignedLongInteger * node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ExpressionFoldingMultiplication");
			AddOperandUsageToWorkList(node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			expressionFoldingMultiplicationCount++;
		}

		private void ExpressionFoldingDivision(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.DivSigned32
				|| node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.DivSigned64
				|| node.Instruction == IRInstruction.DivUnsigned64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.DivSigned32
				|| node2.Instruction == IRInstruction.DivUnsigned32
				|| node2.Instruction == IRInstruction.DivSigned64
				|| node2.Instruction == IRInstruction.DivUnsigned64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = (node2.Instruction == IRInstruction.DivSigned32 || node2.Instruction == IRInstruction.DivSigned64) ?
				(ulong)(node.Operand2.ConstantSignedLongInteger / node2.Operand2.ConstantSignedLongInteger) :
				node.Operand2.ConstantUnsignedLongInteger / node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ConstantFoldingDivision");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			expressionFoldingDivisionCount++;
		}

		private void CombineIntegerCompareBranch(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64))
				return;

			if (!(node.ConditionCode == ConditionCode.NotEqual || node.ConditionCode == ConditionCode.Equal))
				return;

			if (!((node.Operand1.IsVirtualRegister && node.Operand2.IsConstantZero)
				|| (node.Operand2.IsVirtualRegister && node.Operand1.IsConstantZero)))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (!ValidateSSAForm(operand))
				return;

			var node2 = operand.Definitions[0];

			if (!(node2.Instruction == IRInstruction.CompareInt32x32
				|| node2.Instruction == IRInstruction.CompareInt64x32
				|| node2.Instruction == IRInstruction.CompareInt64x64))
				return;

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** CombineIntegerCompareBranch");
			if (trace.Active) trace.Log("BEFORE:\t" + node);

			node.ConditionCode = node.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();
			node.Operand1 = node2.Operand1;
			node.Operand2 = node2.Operand2;
			node.Instruction = Select(node2.Operand1, IRInstruction.CompareIntBranch32, IRInstruction.CompareIntBranch64);

			if (trace.Active) trace.Log("AFTER: \t" + node);
			if (trace.Active) trace.Log("REMOVED:\t" + node2);
			node2.SetInstruction(IRInstruction.Nop);
			combineIntegerCompareBranchCount++;
			instructionsRemovedCount++;
		}

		private void FoldIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInt32x32
				|| node.Instruction == IRInstruction.CompareInt64x32
				|| node.Instruction == IRInstruction.CompareInt64x64))
				return;

			if (!(node.ConditionCode == ConditionCode.NotEqual || node.ConditionCode == ConditionCode.Equal))
				return;

			if (!((node.Operand1.IsVirtualRegister && node.Operand2.IsConstantZero)
				|| (node.Operand2.IsVirtualRegister && node.Operand1.IsConstantZero)))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (!ValidateSSAForm(operand))
				return;

			var node2 = operand.Definitions[0];

			if (!(node2.Instruction == IRInstruction.CompareInt32x32
				|| node2.Instruction == IRInstruction.CompareInt64x32
				|| node2.Instruction == IRInstruction.CompareInt64x64))
				return;

			var compareInteger = node2.Instruction;
			var conditionCode = node.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** FoldIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(compareInteger, conditionCode, node.Result, node2.Operand1, node2.Operand2);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			if (trace.Active) trace.Log("REMOVED:\t" + node2);
			node2.SetInstruction(IRInstruction.Nop);
			foldIntegerCompareCount++;
			instructionsRemovedCount++;
		}

		private void SimplifyExtendedMoves(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.SignExtend8x32
				|| node.Instruction == IRInstruction.SignExtend16x32
				|| node.Instruction == IRInstruction.SignExtend8x64
				|| node.Instruction == IRInstruction.SignExtend16x64
				|| node.Instruction == IRInstruction.SignExtend32x64
				|| node.Instruction == IRInstruction.ZeroExtend8x32
				|| node.Instruction == IRInstruction.ZeroExtend16x32
				|| node.Instruction == IRInstruction.ZeroExtend8x64
				|| node.Instruction == IRInstruction.ZeroExtend16x64
				|| node.Instruction == IRInstruction.ZeroExtend32x64))
				return;

			if (!node.Result.IsVirtualRegister || !node.Operand1.IsVirtualRegister)
				return;

			if (!((NativePointerSize == 4 && node.Result.IsInt && (node.Operand1.IsInt || node.Operand1.IsU || node.Operand1.IsI))
				|| (NativePointerSize == 4 && node.Operand1.IsInt && (node.Result.IsInt || node.Result.IsU || node.Result.IsI))
				|| (NativePointerSize == 8 && node.Result.IsLong && (node.Operand1.IsLong || node.Operand1.IsU || node.Operand1.IsI))
				|| (NativePointerSize == 8 && node.Operand1.IsLong && (node.Result.IsLong || node.Result.IsU || node.Result.IsI))))
				return;

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** SimplifyExtendedMove");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(GetMoveInteger(node.Result), node.Result, node.Operand1);
			simplifyExtendedMoveCount++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
		}

		private void FoldLoadStoreOffsets(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LoadInt32
				|| node.Instruction == IRInstruction.LoadInt64
				|| node.Instruction == IRInstruction.StoreInt8
				|| node.Instruction == IRInstruction.StoreInt16
				|| node.Instruction == IRInstruction.StoreInt32
				|| node.Instruction == IRInstruction.StoreInt64
				|| node.Instruction == IRInstruction.StoreFloatR4
				|| node.Instruction == IRInstruction.StoreFloatR8
				|| node.Instruction == IRInstruction.LoadSignExtend8x32
				|| node.Instruction == IRInstruction.LoadSignExtend16x32
				|| node.Instruction == IRInstruction.LoadSignExtend8x64
				|| node.Instruction == IRInstruction.LoadSignExtend16x64
				|| node.Instruction == IRInstruction.LoadSignExtend32x64
				|| node.Instruction == IRInstruction.LoadZeroExtend8x32
				|| node.Instruction == IRInstruction.LoadZeroExtend16x32
				|| node.Instruction == IRInstruction.LoadZeroExtend8x64
				|| node.Instruction == IRInstruction.LoadZeroExtend16x64
				|| node.Instruction == IRInstruction.LoadZeroExtend32x64))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (node.Operand1.Uses.Count != 1)
				return;

			var node2 = node.Operand1.Definitions[0];

			if (!(node2.Instruction == IRInstruction.Add32
				|| node2.Instruction == IRInstruction.Add64
				|| node2.Instruction == IRInstruction.Sub32
				|| node2.Instruction == IRInstruction.Sub64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Operand constant;

			if (node2.Instruction == IRInstruction.Add32
				|| node2.Instruction == IRInstruction.Add64)
			{
				constant = Operand.CreateConstant(node.Operand2.Type, node2.Operand2.ConstantSignedLongInteger + node.Operand2.ConstantSignedLongInteger);
			}
			else
			{
				constant = Operand.CreateConstant(node.Operand2.Type, node.Operand2.ConstantSignedLongInteger - node2.Operand2.ConstantSignedLongInteger);
			}

			if (trace.Active) trace.Log("*** FoldLoadStoreOffsets");
			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand1 = node2.Operand1;
			node.Operand2 = constant;
			if (trace.Active) trace.Log("AFTER: \t" + node);
			if (trace.Active) trace.Log("REMOVED:\t" + node2);
			node2.SetInstruction(IRInstruction.Nop);
			foldLoadStoreOffsetsCount++;
			instructionsRemovedCount++;
		}

		private void ConstantFoldingPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Result.IsInteger)
				return;

			Operand operand1 = node.Operand1;
			Operand result = node.Result;

			foreach (var operand in node.Operands)
			{
				if (!operand.IsResolvedConstant)
					return;

				if (operand.ConstantUnsignedLongInteger != operand1.ConstantUnsignedLongInteger)
					return;
			}

			if (trace.Active) trace.Log("*** FoldConstantPhiInstruction");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);
			node.SetInstruction(GetMoveInteger(result), result, operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantFoldingPhiCount++;
		}

		private void SimplifyPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (node.OperandCount != 1)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (trace.Active) trace.Log("*** SimplifyPhiInstruction");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			if (node.Result != node.Operand1)
			{
				node.SetInstruction(GetMoveInteger(node.Result), node.Result, node.Operand1);
			}
			else
			{
				node.SetInstruction(IRInstruction.Nop);
			}

			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyPhiCount++;
		}

		private void SimplifyPhi2(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			var operand = node.Operand1;

			foreach (var op in node.Operands)
			{
				if (op != operand)
					return;
			}

			if (trace.Active) trace.Log("*** SimplifyPhiInstruction");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(GetMoveInteger(node.Result), node.Result, node.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyPhiCount++;
		}

		// Doesn't work --- since non-lower byte is ignored
		private void SimplifyIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInt32x32
				|| node.Instruction == IRInstruction.CompareInt64x32
				|| node.Instruction == IRInstruction.CompareInt64x64))
				return;

			if (node.ConditionCode != ConditionCode.NotEqual)
				return;

			Debug.Assert(node.Result.IsVirtualRegister);

			if (!ValidateSSAForm(node.Result))
				return;

			if (!((node.Operand1.IsVirtualRegister && node.Operand2.IsConstantZero)
				|| (node.Operand2.IsVirtualRegister && node.Operand1.IsConstantZero)))
				return;

			// and the use of the result is just another comparison
			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.CompareInt32x32
				|| node2.Instruction == IRInstruction.CompareInt64x32
				|| node2.Instruction == IRInstruction.CompareInt64x64))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (!ValidateSSAForm(operand))
				return;

			if (operand.Is64BitInteger != node.Result.Is64BitInteger)
				return;

			if (trace.Active) trace.Log("*** SimplifyIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(GetMoveInteger(node.Result), node.Result, operand);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyIntegerCompareCount++;
		}

		private void SimplifyAddCarryOut(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddCarryOut32)
				return;

			if (node.Result2.Uses.Count != 0)
				return;

			if (trace.Active) trace.Log("*** SimplifyAddCarryOut");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(IRInstruction.Add32, node.Result, node.Operand1, node.Operand2);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyGeneralCount++;
		}

		private void SimplifyAddCarryOut2(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddCarryOut32)
				return;

			var result = node.Result;
			var result2 = node.Result2;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;

			if (operand1.IsConstantZero || operand2.IsConstantZero)
			{
				if (trace.Active) trace.Log("*** SimplifyAddCarryOut2");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				AddOperandUsageToWorkList(node);

				var context = new Context(node);
				context.SetInstruction(IRInstruction.MoveInt32, result, operand1.IsConstantZero ? operand1 : operand2);
				context.AppendInstruction(IRInstruction.MoveInt32, result2, ConstantZero);

				if (trace.Active) trace.Log("AFTER: \t" + context);
				if (trace.Active) trace.Log("AFTER: \t" + context.Previous);
				simplifyGeneralCount++;
			}
		}

		private void SimplifyAddWithCarry(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddWithCarry32)
				return;

			var result = node.Result;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;
			var operand3 = node.Operand3;

			if (operand3.IsResolvedConstant && operand3.IsConstantZero)
			{
				if (trace.Active) trace.Log("*** SimplifyAddWithCarry");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				AddOperandUsageToWorkList(node);

				node.SetInstruction(IRInstruction.Add32, result, operand1, operand2);
				if (trace.Active) trace.Log("AFTER: \t" + node);
				simplifyGeneralCount++;
				return;
			}

			if (operand3.IsResolvedConstant && !operand3.IsConstantZero)
			{
				var v1 = AllocateVirtualRegister(result.Type);

				var context = new Context(node);

				if (trace.Active) trace.Log("*** SimplifyAddWithCarry");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				AddOperandUsageToWorkList(node);

				context.SetInstruction(IRInstruction.Add32, v1, operand1, operand2);
				context.AppendInstruction(IRInstruction.Add32, result, v1, CreateConstant(1));
				if (trace.Active) trace.Log("AFTER: \t" + context.Previous);
				if (trace.Active) trace.Log("AFTER: \t" + context);
				simplifyGeneralCount++;
				return;
			}
		}

		private void SimplifySubCarryOut(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.SubCarryOut32)
				return;

			if (node.Result2.Uses.Count != 0)
				return;

			if (trace.Active) trace.Log("*** SimplifySubCarryOut");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(IRInstruction.Sub32, node.Result, node.Operand1, node.Operand2);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyGeneralCount++;
		}

		private void SimplifyIntegerCompare2(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInt32x32
				|| node.Instruction == IRInstruction.CompareInt64x32
				|| node.Instruction == IRInstruction.CompareInt64x64))
				return;

			if (node.ConditionCode != ConditionCode.UnsignedGreaterThan)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsConstantZero)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (trace.Active) trace.Log("*** SimplifyIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(Select(node.Result, !node.Operand1.Is64BitInteger ? (BaseInstruction)IRInstruction.CompareInt32x32 : IRInstruction.CompareInt64x32, IRInstruction.CompareInt64x64), ConditionCode.NotEqual, node.Result, node.Operand1, node.Operand2);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyIntegerCompareCount++;
		}

		private void DeadCodeEliminationPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			var result = node.Result;

			foreach (var use in result.Uses)
			{
				if (use != node)
					return;
			}

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** DeadCodeEliminationPhi");
			if (trace.Active) trace.Log("REMOVED:\t" + node);
			node.SetInstruction(IRInstruction.Nop);
			deadCodeEliminationPhiCount++;
		}

		private void ArithmeticSimplificationRemUnsignedModulus(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.RemUnsigned32 || node.Instruction == IRInstruction.RemUnsigned64))
				return;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op2.IsResolvedConstant)
				return;

			if (op2.ConstantUnsignedLongInteger == 0)
				return;

			if (!IsPowerOfTwo(op2.ConstantUnsignedLongInteger))
				return;

			int power = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

			var mask = (1 << power) - 1;

			var constant = CreateConstant(mask);

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ArithmeticSimplificationRemUnsignedModulus");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(Select(result, IRInstruction.LogicalAnd32, IRInstruction.LogicalAnd64), result, op1, constant);
			arithmeticSimplificationModulusCount++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
			return;
		}

		private static bool IsPowerOfTwo(ulong n)
		{
			return (n & (n - 1)) == 0;
		}

		private static int GetPowerOfTwo(ulong n)
		{
			int bits = 0;
			while (n > 0)
			{
				bits++;
				n >>= 1;
			}

			return bits - 1;
		}

		private void GetLow64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			AddOperandUsageToWorkList(node);

			var low = CreateConstant((uint)(node.Operand1.ConstantUnsignedLongInteger & 0xFFFFFFFF));

			if (trace.Active) trace.Log("*** GetLow64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, low);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longConstantReductionCount++;
		}

		private void GetHigh64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetHigh64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			AddOperandUsageToWorkList(node);

			var high = CreateConstant((uint)(node.Operand1.ConstantUnsignedLongInteger >> 32));

			if (trace.Active) trace.Log("*** GetHigh64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, high);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longConstantReductionCount++;
		}

		private void GetLow64Propagation(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.To64)
				return;

			Debug.Assert(node2.Result == node.Operand1);

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			if (trace.Active) trace.Log("*** GetLow64Propagation");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, node2.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longPropagateCount++;
		}

		private void GetHigh64Propagation(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetHigh64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.To64)
				return;

			Debug.Assert(node2.Result == node.Operand1);

			//if (!node2.Result.IsVirtualRegister)
			//	return;

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			if (trace.Active) trace.Log("*** GetHigh64Propagation");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, node2.Operand2);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longPropagateCount++;
		}

		private void FoldGetHigh64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetHigh64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			var constant = CreateConstant((uint)(node.Operand1.ConstantUnsignedLongInteger >> 32));

			AddOperandUsageToWorkList(node);

			if (trace.Active) trace.Log("*** FoldGetHigh64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, constant);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longConstantFoldingCount++;
		}

		private void FoldGetLow64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			var constant = CreateConstant((uint)(node.Operand1.ConstantUnsignedLongInteger) & 0xFFFFFFFF);

			AddOperandUsageToWorkList(node);

			if (trace.Active) trace.Log("*** FoldGetLow64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, constant);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longConstantFoldingCount++;
		}

		private void FoldGetLow64PointerConstant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (node.Operand1.IsResolvedConstant)
				return;

			if (!(node.Operand1.IsPointer || node.Operand1.IsCPURegister))
				return;

			AddOperandUsageToWorkList(node);

			if (trace.Active) trace.Log("*** FoldGetLow64PointerConstant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, node.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longConstantFoldingCount++;
		}

		private void FoldTo64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.To64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			var constant = CreateConstant(node.Operand2.ConstantUnsignedLongInteger << 32 | node.Operand1.ConstantUnsignedLongInteger);

			AddOperandUsageToWorkList(node);

			if (trace.Active) trace.Log("*** FoldTo64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt64, node.Result, constant);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			longConstantFoldingCount++;
		}

		private void SimplifyParamLoadCount(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LoadInt32
				|| node.Instruction == IRInstruction.LoadInt64
				|| node.Instruction == IRInstruction.LoadSignExtend8x32
				|| node.Instruction == IRInstruction.LoadSignExtend16x32
				|| node.Instruction == IRInstruction.LoadSignExtend8x64
				|| node.Instruction == IRInstruction.LoadSignExtend16x64
				|| node.Instruction == IRInstruction.LoadSignExtend32x64
				|| node.Instruction == IRInstruction.LoadZeroExtend8x32
				|| node.Instruction == IRInstruction.LoadZeroExtend16x32
				|| node.Instruction == IRInstruction.LoadZeroExtend8x64
				|| node.Instruction == IRInstruction.LoadZeroExtend16x64
				|| node.Instruction == IRInstruction.LoadZeroExtend32x64
				|| node.Instruction == IRInstruction.LoadFloatR4
				|| node.Instruction == IRInstruction.LoadFloatR8))
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!node.Operand2.IsConstantZero)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.AddressOf)
				return;

			if (!node2.Operand1.IsParameter)
				return;

			BaseInstruction instruction = null;

			if (node.Instruction == IRInstruction.LoadInt32)
				instruction = IRInstruction.LoadParamInt32;
			else if (node.Instruction == IRInstruction.LoadInt64)
				instruction = IRInstruction.LoadParamInt64;
			else if (node.Instruction == IRInstruction.LoadFloatR4)
				instruction = IRInstruction.LoadParamFloatR4;
			else if (node.Instruction == IRInstruction.LoadFloatR8)
				instruction = IRInstruction.LoadParamFloatR8;
			else if (node.Instruction == IRInstruction.LoadSignExtend8x32)
				instruction = IRInstruction.LoadParamSignExtend8x32;
			else if (node.Instruction == IRInstruction.LoadSignExtend16x32)
				instruction = IRInstruction.LoadParamSignExtend16x32;
			else if (node.Instruction == IRInstruction.LoadSignExtend8x64)
				instruction = IRInstruction.LoadParamSignExtend8x64;
			else if (node.Instruction == IRInstruction.LoadSignExtend16x64)
				instruction = IRInstruction.LoadParamSignExtend16x64;
			else if (node.Instruction == IRInstruction.LoadSignExtend32x64)
				instruction = IRInstruction.LoadParamSignExtend32x64;
			else if (node.Instruction == IRInstruction.LoadZeroExtend8x32)
				instruction = IRInstruction.LoadParamZeroExtend8x32;
			else if (node.Instruction == IRInstruction.LoadZeroExtend16x32)
				instruction = IRInstruction.LoadParamZeroExtend16x32;
			else if (node.Instruction == IRInstruction.LoadZeroExtend8x64)
				instruction = IRInstruction.LoadParamZeroExtend8x64;
			else if (node.Instruction == IRInstruction.LoadZeroExtend16x64)
				instruction = IRInstruction.LoadParamZeroExtend16x64;
			else if (node.Instruction == IRInstruction.LoadZeroExtend32x64)
				instruction = IRInstruction.LoadParamZeroExtend32x64;

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** SimplifyParamLoadCount");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(instruction, node.Result, node2.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyParamLoadCount++;
		}

		private void FoldIfThenElse(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.IfThenElse64 || node.Instruction == IRInstruction.IfThenElse32))
				return;

			bool simplify = false;
			bool result = false;

			if (node.Operand2.IsVirtualRegister && node.Operand3.IsVirtualRegister && node.Operand2 == node.Operand3)
			{
				// always true
				simplify = true;
				result = true;
			}
			else if (node.Operand1.IsResolvedConstant)
			{
				simplify = true;
				result = node.Operand1.ConstantUnsignedLongInteger != 0;
			}
			else if (node.Operand2.IsResolvedConstant && node.Operand3.IsResolvedConstant)
			{
				simplify = true;
				result = node.Operand2.ConstantUnsignedLongInteger == node.Operand3.ConstantUnsignedLongInteger;
			}

			if (simplify)
			{
				var move = (node.Instruction == IRInstruction.IfThenElse32) ? (BaseInstruction)IRInstruction.MoveInt32 : IRInstruction.MoveInt64;
				var operand = result ? node.Operand2 : node.Operand3;

				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** FoldIfThenElse");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(move, node.Result, operand);
				if (trace.Active) trace.Log("AFTER: \t" + node);
				foldIfThenElseCount++;
			}
		}

		private void SimplifyCompareBranch(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.CompareIntBranch64)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			if (!ValidateSSAForm(node.Operand2))
				return;

			if (node.Operand1.Definitions[0].Instruction != IRInstruction.SignExtend32x64)
				return;

			if (node.Operand2.Definitions[0].Instruction != IRInstruction.SignExtend32x64)
				return;

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node.Operand1.Definitions[0]);
			AddOperandUsageToWorkList(node.Operand2.Definitions[0]);

			if (trace.Active) trace.Log("*** SimplifyCompareBranch");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.CompareIntBranch32, node.ConditionCode, null, node.Operand1.Definitions[0].Operand1, node.Operand2.Definitions[0].Operand1, node.BranchTargets[0]);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyBranchComparisonCount++;
		}

		private void SimplifyGetLow64(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.ShiftRight64)
				return;

			Debug.Assert(node2.Result == node.Operand1);

			if (node2.Operand2.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			if (node2.Operand2.ConstantSignedInteger != 32)
				return;

			if (!ValidateSSAForm(node2.Operand1))
				return;

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			if (trace.Active) trace.Log("*** SimplifyGetLow64");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.GetHigh64, node.Result, node2.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyGetLowCount++;
		}

		private void SimplifyGetHigh64(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetHigh64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.ShiftLeft64)
				return;

			Debug.Assert(node2.Result == node.Operand1);

			if (node2.Operand2.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			if (!ValidateSSAForm(node2.Operand1))
				return;

			if (node2.Operand2.ConstantSignedInteger != 32)
				return;

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			if (trace.Active) trace.Log("*** SimplifyGetHigh64");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.GetLow64, node.Result, node2.Operand1);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyGetHighCount++;
		}

		private void SimplifyGetLow64b(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.ShiftLeft64)
				return;

			Debug.Assert(node2.Result == node.Operand1);

			if (node2.Operand2.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			if (!ValidateSSAForm(node2.Operand1))
				return;

			if (node2.Operand2.ConstantSignedInteger < 32)
				return;

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			if (trace.Active) trace.Log("*** SimplifyGetLow64b");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, ConstantZero);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyGetHighCount++;
		}

		private void SimplifyGetHigh64b(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetHigh64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var node2 = node.Operand1.Definitions[0];

			if (node2.Instruction != IRInstruction.ShiftRight64)
				return;

			Debug.Assert(node2.Result == node.Operand1);

			if (node2.Operand2.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			if (!ValidateSSAForm(node2.Operand1))
				return;

			if (node2.Operand2.ConstantSignedInteger < 32)
				return;

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			if (trace.Active) trace.Log("*** SimplifyGetHigh64b");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, ConstantZero);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyGetHighCount++;
		}

		private void ConstantFoldingInteger(InstructionNode node)
		{
			var operand = ValueNumberingStage.ConstantFoldingInteger(node);

			if (operand == null)
				return;

			AddOperandUsageToWorkList(node);

			if (trace.Active) trace.Log("*** ConstantFoldingInteger");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(Select(IRInstruction.MoveInt32, IRInstruction.MoveInt64), node.Result, operand);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantFoldingIntegerCount++;
		}

		private void StrengthReductionInteger(InstructionNode node)
		{
			var operand = ValueNumberingStage.StrengthReductionInteger(node);

			if (operand == null)
				return;

			AddOperandUsageToWorkList(node);

			if (trace.Active) trace.Log("*** StrengthReductionInteger");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(Select(IRInstruction.MoveInt32, IRInstruction.MoveInt64), node.Result, operand);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			strengthReductionIntegerCount++;
		}
	}
}
