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
		private Counter InstructionsRemovedCount = new Counter("IROptimizationStage.IRInstructionRemoved");
		private Counter BlockRemovedCount = new Counter("IROptimizationStage.BlockRemoved");
		private Counter PropagateConstantCount = new Counter("IROptimizationStage.PropagateConstant");
		private Counter PropagateMoveCount = new Counter("IROptimizationStage.PropagateMoveCount");
		private Counter ConstantFoldingAndStrengthReductionCount = new Counter("IROptimizationStage.ConstantFoldingAndStrengthReduction");
		private Counter StrengthReductionAndSimplificationCount = new Counter("IROptimizationStage.StrengthReductionAndSimplification");
		private Counter InstructionSimplificationCount = new Counter("IROptimizationStage.InstructionSimplification");
		private Counter CombineAdditionAndSubstractionCount = new Counter("IROptimizationStage.CombineAdditionAndSubstraction");
		private Counter CombineMultiplicationCount = new Counter("IROptimizationStage.CombineMultiplication");
		private Counter CombineDivisionCount = new Counter("IROptimizationStage.CombineDivision");
		private Counter CombineLogicalOrCount = new Counter("IROptimizationStage.CombineLogicalOrCount");
		private Counter CombineLogicalAndCount = new Counter("IROptimizationStage.CombineLogicalAndCount");
		private Counter ConstantFoldingPhiCount = new Counter("IROptimizationStage.ConstantFoldingPhi");
		private Counter FoldIntegerCompareBranchCount = new Counter("IROptimizationStage.FoldIntegerCompareBranch");
		private Counter FoldIntegerCompareCount = new Counter("IROptimizationStage.FoldIntegerCompare");
		private Counter FoldLoadStoreOffsetsCount = new Counter("IROptimizationStage.FoldLoadStoreOffsets");
		private Counter DeadCodeEliminationCount = new Counter("IROptimizationStage.DeadCodeElimination");
		private Counter DeadCodeEliminationPhiCount = new Counter("IROptimizationStage.DeadCodeEliminationPhi");
		private Counter CombineIntegerCompareBranchCount = new Counter("IROptimizationStage.CombineIntegerCompareBranch");
		private Counter SimplifyPhiCount = new Counter("IROptimizationStage.SimplifyPhi");
		private Counter SimplifyIntegerCompareCount = new Counter("IROptimizationStage.SimplifyIntegerCompare");
		private Counter SimplifyGeneralCount = new Counter("IROptimizationStage.SimplifyGeneral");
		private Counter SimplifyParamLoadCount = new Counter("IROptimizationStage.SimplifyParamLoad");
		private Counter SimplifyBranchComparisonCount = new Counter("IROptimizationStage.SimplifyBranchComparison");
		private Counter SimplifyGetLowCount = new Counter("IROptimizationStage.SimplifyGetLow");
		private Counter SimplifyGetHighCount = new Counter("IROptimizationStage.SimplifyGetHigh");
		private Counter RemoveUselessIntegerCompareBranchCount = new Counter("IROptimizationStage.RemoveUselessIntegerCompareBranch");
		private Counter LongPropagateCount = new Counter("IROptimizationStage.LongPropagateCount");
		private Counter LongConstantFoldingCount = new Counter("IROptimizationStage.LongConstantFolding");
		private Counter PropagateCompoundMoveCount = new Counter("IROptimizationStage.PropagateCompoundMove");
		private Counter FoldIfThenElseCount = new Counter("IROptimizationStage.FoldIfThenElse");

		private Stack<InstructionNode> worklist;

		private TraceLog trace;

		private delegate void Transformation(InstructionNode node);

		private List<Transformation> transformations;

		protected override void Initialize()
		{
			worklist = new Stack<InstructionNode>();

			transformations = CreateTransformationList();

			Register(InstructionsRemovedCount);
			Register(BlockRemovedCount);
			Register(PropagateConstantCount);
			Register(PropagateMoveCount);
			Register(ConstantFoldingAndStrengthReductionCount);
			Register(StrengthReductionAndSimplificationCount);
			Register(InstructionSimplificationCount);
			Register(CombineAdditionAndSubstractionCount);
			Register(CombineMultiplicationCount);
			Register(CombineDivisionCount);
			Register(CombineLogicalOrCount);
			Register(CombineLogicalAndCount);
			Register(ConstantFoldingPhiCount);
			Register(FoldIntegerCompareBranchCount);
			Register(FoldIntegerCompareCount);
			Register(FoldLoadStoreOffsetsCount);
			Register(DeadCodeEliminationCount);
			Register(DeadCodeEliminationPhiCount);
			Register(CombineIntegerCompareBranchCount);
			Register(SimplifyPhiCount);
			Register(SimplifyIntegerCompareCount);
			Register(SimplifyGeneralCount);
			Register(SimplifyParamLoadCount);
			Register(SimplifyBranchComparisonCount);
			Register(SimplifyGetLowCount);
			Register(SimplifyGetHighCount);
			Register(RemoveUselessIntegerCompareBranchCount);
			Register(LongPropagateCount);
			Register(LongConstantFoldingCount);
			Register(PropagateCompoundMoveCount);
			Register(FoldIfThenElseCount);
		}

		private List<Transformation> CreateTransformationList()
		{
			return new List<Transformation>()
			{
				PropagateConstant,
				PropagateMove,
				PropagateCompoundMove,
				DeadCodeElimination,
				ConstantFoldingAndStrengthReductionInteger,
				StrengthReduction,
				InstructionSimplification,
				CombineIntegerCompareBranch,
				FoldIntegerCompare,
				RemoveUselessIntegerCompareBranch,
				ConstantFoldIntegerCompareBranch,
				SimplifyIntegerCompare2,
				SimplifyIntegerCompare,
				SimplifyAddCarryOut32B,
				SimplifyAddWithCarry32,
				SimplifyCompareBranch,
				SimplifyGetLow64,
				SimplifyGetHigh64,
				SimplifyGetLow64b,
				SimplifyGetHigh64b,
				SimplifyPhi2,
				SimplifyParamLoad,
				GetHigh64Propagation,
				GetLow64Propagation,
				FoldGetLow64PointerConstant,
				FoldLoadStoreOffsets,
				ConstantFoldingPhi,
				DeadCodeEliminationPhi,
				CombineAdditionAndSubstraction,
				CombineMultiplication,
				CombineDivision,
				CombineLogicalOr,
				CombineLogicalAnd,
			};
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

		#region IR Optimizations

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

						trace?.Log("*** PropagateConstant");
						trace?.Log($"BEFORE:\t{useNode}");

						AddOperandUsageToWorkList(operand);
						AddOperandUsageToWorkList(useNode.GetOperand(i));

						useNode.SetOperand(i, source);
						PropagateConstantCount++;
						trace?.Log($"AFTER: \t{useNode}");
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
						trace?.Log("*** PropagateMove");
						trace?.Log($"BEFORE:\t{useNode}");
						useNode.SetOperand(i, source);
						PropagateMoveCount++;
						trace?.Log($"AFTER: \t{useNode}");
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			trace?.Log($"REMOVED:\t{node}");
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount++;
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
						trace?.Log("*** PropagateCompoundMove");
						trace?.Log($"BEFORE:\t{useNode}");
						useNode.SetOperand(i, source);
						PropagateCompoundMoveCount++;
						trace?.Log($"AFTER: \t{useNode}");
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			trace?.Log($"REMOVED:\t{node}");
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount++;
		}

		private void RemoveUselessIntegerCompareBranch(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64))
				return;

			if (node.Block.NextBlocks.Count != 1)
				return;

			trace?.Log($"REMOVED:\t{node}");
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount++;
			RemoveUselessIntegerCompareBranchCount++;
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

			var nextNode = node.Next;

			if (nextNode.Instruction != IRInstruction.Jmp)
				return;

			if (node.BranchTargets[0] == nextNode.BranchTargets[0])
			{
				trace?.Log("*** FoldIntegerCompareBranch-Useless");
				trace?.Log($"REMOVED:\t{node}");
				AddOperandUsageToWorkList(node);
				node.SetInstruction(IRInstruction.Nop);
				InstructionsRemovedCount++;
				FoldIntegerCompareBranchCount++;
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

			trace?.Log("*** FoldIntegerCompareBranch");

			if (compareResult)
			{
				notTaken = nextNode.BranchTargets[0];
				trace?.Log($"BEFORE:\t{node}");
				node.SetInstruction(IRInstruction.Jmp, node.BranchTargets[0]);
				trace?.Log($"AFTER:\t{node}");

				notUsed = nextNode;
			}
			else
			{
				notTaken = node.BranchTargets[0];
				notUsed = node;
			}

			trace?.Log($"REMOVED:\t{node}");
			AddOperandUsageToWorkList(notUsed);
			notUsed.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount++;
			FoldIntegerCompareBranchCount++;

			// if target block no longer has any predecessors (or the only predecessor is itself), remove all instructions from it.
			CheckAndClearEmptyBlock(notTaken);
		}

		private void CheckAndClearEmptyBlock(BasicBlock block)
		{
			if (block.PreviousBlocks.Count != 0 || block.IsHeadBlock)
				return;

			trace?.Log($"*** RemoveBlock: {block}");

			BlockRemovedCount++;

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

		private void CombineAdditionAndSubstraction(InstructionNode node)
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

			trace?.Log("*** CombineAdditionAndSubstraction");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineAdditionAndSubstractionCount++;
		}

		private void CombineLogicalOr(InstructionNode node)
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

			trace?.Log("*** CombineLogicalOr");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineLogicalOrCount++;
		}

		private void CombineLogicalAnd(InstructionNode node)
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

			trace?.Log("*** CombineLogicalAnd");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineLogicalAndCount++;
		}

		private void CombineMultiplication(InstructionNode node)
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

			trace?.Log("*** CombineMultiplication");
			AddOperandUsageToWorkList(node2);
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineMultiplicationCount++;
		}

		private void CombineDivision(InstructionNode node)
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

			trace?.Log("*** CombineDivision");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineDivisionCount++;
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
			trace?.Log("*** CombineIntegerCompareBranch");
			trace?.Log($"BEFORE:\t{node}");

			node.ConditionCode = node.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();
			node.Operand1 = node2.Operand1;
			node.Operand2 = node2.Operand2;
			node.Instruction = Select(node2.Operand1, IRInstruction.CompareIntBranch32, IRInstruction.CompareIntBranch64);

			trace?.Log($"AFTER: \t{node}");
			trace?.Log($"REMOVED:\t{node2}");
			node2.SetInstruction(IRInstruction.Nop);
			CombineIntegerCompareBranchCount++;
			InstructionsRemovedCount++;
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
			trace?.Log("*** FoldIntegerCompare");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(compareInteger, conditionCode, node.Result, node2.Operand1, node2.Operand2);
			trace?.Log($"AFTER: \t{node}");
			trace?.Log($"REMOVED:\t{node2}");
			node2.SetInstruction(IRInstruction.Nop);
			FoldIntegerCompareCount++;
			InstructionsRemovedCount++;
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

			trace?.Log("*** FoldLoadStoreOffsets");
			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);
			trace?.Log($"BEFORE:\t{node}");
			node.Operand1 = node2.Operand1;
			node.Operand2 = constant;
			trace?.Log($"AFTER: \t{node}");
			trace?.Log($"REMOVED:\t{node2}");
			node2.SetInstruction(IRInstruction.Nop);
			FoldLoadStoreOffsetsCount++;
			InstructionsRemovedCount++;
		}

		private void ConstantFoldingPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Result.IsInteger)
				return;

			var operand1 = node.Operand1;
			var result = node.Result;

			foreach (var operand in node.Operands)
			{
				if (!operand.IsResolvedConstant)
					return;

				if (operand.ConstantUnsignedLongInteger != operand1.ConstantUnsignedLongInteger)
					return;
			}

			trace?.Log("*** FoldConstantPhiInstruction");
			trace?.Log($"BEFORE:\t{node}");
			AddOperandUsageToWorkList(node);
			node.SetInstruction(GetMoveInteger(result), result, operand1);
			trace?.Log($"AFTER: \t{node}");
			ConstantFoldingPhiCount++;
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

			trace?.Log("*** SimplifyPhiInstruction");
			trace?.Log($"BEFORE:\t{node}");
			AddOperandUsageToWorkList(node);

			node.SetInstruction(GetMoveInteger(node.Result), node.Result, node.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyPhiCount++;
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

			trace?.Log("*** SimplifyIntegerCompare");
			trace?.Log($"BEFORE:\t{node}");
			AddOperandUsageToWorkList(node);

			node.SetInstruction(GetMoveInteger(node.Result), node.Result, operand);
			trace?.Log($"AFTER: \t{node}");
			SimplifyIntegerCompareCount++;
		}

		private void SimplifyAddCarryOut32B(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddCarryOut32)
				return;

			var result = node.Result;
			var result2 = node.Result2;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;

			if (operand1.IsConstantZero || operand2.IsConstantZero)
			{
				trace?.Log("*** SimplifyAddCarryOut2");
				trace?.Log($"BEFORE:\t{node}");
				AddOperandUsageToWorkList(node);

				var context = new Context(node);
				context.SetInstruction(IRInstruction.MoveInt32, result, operand1.IsConstantZero ? operand1 : operand2);
				context.AppendInstruction(IRInstruction.MoveInt32, result2, ConstantZero);

				trace?.Log($"AFTER: \t{context}");
				trace?.Log($"AFTER: \t{context.Previous}");
				SimplifyGeneralCount++;
			}
		}

		private void SimplifyAddWithCarry32(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.AddWithCarry32)
				return;

			var result = node.Result;
			var operand1 = node.Operand1;
			var operand2 = node.Operand2;
			var operand3 = node.Operand3;

			if (operand3.IsResolvedConstant && operand3.IsConstantZero)
			{
				trace?.Log("*** SimplifyAddWithCarry");
				trace?.Log($"BEFORE:\t{node}");
				AddOperandUsageToWorkList(node);

				node.SetInstruction(IRInstruction.Add32, result, operand1, operand2);
				trace?.Log($"AFTER: \t{node}");
				SimplifyGeneralCount++;
				return;
			}

			if (operand3.IsResolvedConstant && !operand3.IsConstantZero)
			{
				var v1 = AllocateVirtualRegister(result.Type);

				var context = new Context(node);

				trace?.Log("*** SimplifyAddWithCarry");
				trace?.Log($"BEFORE:\t{node}");
				AddOperandUsageToWorkList(node);

				context.SetInstruction(IRInstruction.Add32, v1, operand1, operand2);
				context.AppendInstruction(IRInstruction.Add32, result, v1, CreateConstant(1));
				trace?.Log($"AFTER: \t{context.Previous}");
				trace?.Log($"AFTER: \t{context}");
				SimplifyGeneralCount++;
				return;
			}
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

			trace?.Log("*** SimplifyIntegerCompare");
			trace?.Log($"BEFORE:\t{node}");
			AddOperandUsageToWorkList(node);

			node.SetInstruction(Select(node.Result, !node.Operand1.Is64BitInteger ? (BaseInstruction)IRInstruction.CompareInt32x32 : IRInstruction.CompareInt64x32, IRInstruction.CompareInt64x64), ConditionCode.NotEqual, node.Result, node.Operand1, node.Operand2);
			trace?.Log($"AFTER: \t{node}");
			SimplifyIntegerCompareCount++;
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
			trace?.Log("*** DeadCodeEliminationPhi");
			trace?.Log($"REMOVED:\t{node}");
			node.SetInstruction(IRInstruction.Nop);
			DeadCodeEliminationPhiCount++;
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

			trace?.Log("*** GetLow64Propagation");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			LongPropagateCount++;
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

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			trace?.Log("*** GetHigh64Propagation");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, node2.Operand2);
			trace?.Log($"AFTER: \t{node}");
			LongPropagateCount++;
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

			trace?.Log("*** FoldGetLow64PointerConstant");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, node.Operand1);
			trace?.Log($"AFTER: \t{node}");
			LongConstantFoldingCount++;
		}

		private void SimplifyParamLoad(InstructionNode node)
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

			Debug.Assert(node2.Operand1.IsParameter);

			var paramType = MethodCompiler.Parameters[node2.Operand1.Index];

			// Don't mix int and floating point otherwise the inliner will break
			// Rational - the inliner can not transform the bits between floating point and regular registers without a load and store to memory (at least on Intel Platforms)
			if ((node.Instruction == IRInstruction.LoadInt32 || node.Instruction == IRInstruction.LoadInt64) && paramType.IsFloatingPoint)
				return;
			else if ((node.Instruction == IRInstruction.LoadFloatR4 || node.Instruction == IRInstruction.LoadFloatR8) && paramType.IsInteger)
				return;

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
			trace?.Log("*** SimplifyParamLoadCount");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(instruction, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyParamLoadCount++;
		}

		//private void FoldIfThenElse(InstructionNode node)
		//{
		//	if (!(node.Instruction == IRInstruction.IfThenElse64 || node.Instruction == IRInstruction.IfThenElse32))
		//		return;

		//	bool simplify = false;
		//	bool result = false;

		//	if (node.Operand2.IsVirtualRegister && node.Operand3.IsVirtualRegister && node.Operand2 == node.Operand3)
		//	{
		//		// always true
		//		simplify = true;
		//		result = true;
		//	}
		//	else if (node.Operand1.IsResolvedConstant)
		//	{
		//		simplify = true;
		//		result = node.Operand1.ConstantUnsignedLongInteger != 0;
		//	}
		//	else if (node.Operand2.IsResolvedConstant && node.Operand3.IsResolvedConstant)
		//	{
		//		simplify = true;
		//		result = node.Operand2.ConstantUnsignedLongInteger == node.Operand3.ConstantUnsignedLongInteger;
		//	}

		//	if (simplify)
		//	{
		//		var move = (node.Instruction == IRInstruction.IfThenElse32) ? (BaseInstruction)IRInstruction.MoveInt32 : IRInstruction.MoveInt64;
		//		var operand = result ? node.Operand2 : node.Operand3;

		//		AddOperandUsageToWorkList(node);
		//		trace?.Log("*** FoldIfThenElse");
		//		trace?.Log("BEFORE:\t" + node);
		//		node.SetInstruction(move, node.Result, operand);
		//		trace?.Log("AFTER: \t" + node);
		//		FoldIfThenElseCount++;
		//	}
		//}

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

			trace?.Log("*** SimplifyCompareBranch");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.CompareIntBranch32, node.ConditionCode, null, node.Operand1.Definitions[0].Operand1, node.Operand2.Definitions[0].Operand1, node.BranchTargets[0]);
			trace?.Log($"AFTER: \t{node}");
			SimplifyBranchComparisonCount++;
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

			trace?.Log("*** SimplifyGetLow64");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.GetHigh64, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetLowCount++;
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

			trace?.Log("*** SimplifyGetHigh64");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.GetLow64, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetHighCount++;
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

			trace?.Log("*** SimplifyGetLow64b");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, ConstantZero);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetHighCount++;
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

			trace?.Log("*** SimplifyGetHigh64b");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.MoveInt32, node.Result, ConstantZero);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetHighCount++;
		}

		#endregion IR Optimizations

		#region BuiltIn Optimizations

		private void DeadCodeElimination(InstructionNode node)
		{
			Update(
				node,
				BuiltInOptimizations.DeadCodeElimination(node),
				DeadCodeEliminationCount,
				InstructionsRemovedCount
			);
		}

		private void ConstantFoldingAndStrengthReductionInteger(InstructionNode node)
		{
			Update(
				node,
				BuiltInOptimizations.ConstantFoldingAndStrengthReductionInteger(node),
				ConstantFoldingAndStrengthReductionCount
			);
		}

		private void StrengthReduction(InstructionNode node)
		{
			Update(
				node,
				BuiltInOptimizations.StrengthReduction(node),
				StrengthReductionAndSimplificationCount
			);
		}

		private void InstructionSimplification(InstructionNode node)
		{
			Update(
				node,
				BuiltInOptimizations.Simplification(node),
				InstructionSimplificationCount
			);
		}

		#endregion BuiltIn Optimizations

		private void Update(InstructionNode node, SimpleInstruction simpleInstruction, Counter counter = null, Counter counter2 = null)
		{
			if (simpleInstruction == null)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log($"*** {counter.Name}");
			trace?.Log($"BEFORE:\t{node}");

			node.SetInstruction(simpleInstruction);

			trace?.Log($"AFTER: \t{node}");

			if (counter != null)
			{
				counter++;
			}

			if (counter2 != null)
			{
				counter2++;
			}
		}
	}
}
