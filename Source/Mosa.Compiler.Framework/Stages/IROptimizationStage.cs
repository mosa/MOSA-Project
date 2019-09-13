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
		private Counter GetLowWhenZeroHighSimplificationCount = new Counter("IROptimizationStage.GetLowWhenZeroHighSimplification");

		private Counter To64PropagationCount = new Counter("IROptimizationStage.To64Propagation");

		//private Counter FoldIfThenElseCount = new Counter("IROptimizationStage.FoldIfThenElse");

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
			Register(GetLowWhenZeroHighSimplificationCount);
			Register(To64PropagationCount);

			//Register(FoldIfThenElseCount);
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

				//FoldGetLow64PointerConstant,
				FoldLoadStoreOffsets,
				ConstantFoldingPhi,
				DeadCodeEliminationPhi,
				CombineAdditionAndSubstraction,
				CombineMultiplication,
				CombineDivision,
				CombineLogicalOr,
				CombineLogicalAnd,
				GetLowWhenZeroHighSimplification,
				To64Propagation,
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

			foreach (var node in operand.Uses)
			{
				AddToWorkList(node);
			}

			foreach (var node in operand.Definitions)
			{
				AddToWorkList(node);
			}
		}

		private void AddOperandUsageToWorkList(Context context)
		{
			AddOperandUsageToWorkList(context.Node);
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
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.Move64 : IRInstruction.Move32;
		}

		#region IR Optimizations

		private void PropagateConstant(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Move32 || node.Instruction == IRInstruction.Move64))
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			var destination = node.Result;
			var constant = node.Operand1;

			// for each statement T that uses operand, substituted c in statement T
			foreach (var useNode in destination.Uses.ToArray())
			{
				if (useNode.Instruction == IRInstruction.AddressOf)
					continue;   // Not sure if this is even possible

				bool propogated = false;

				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (operand == destination)
					{
						propogated = true;

						trace?.Log("*** PropagateConstant");
						trace?.Log($"BEFORE:\t{useNode}");
						useNode.SetOperand(i, constant);
						trace?.Log($"AFTER: \t{useNode}");
					}
				}

				if (propogated)
				{
					AddToWorkList(useNode);
				}
			}

			PropagateConstantCount.Increment();

			AddToWorkList(node);
		}

		private void PropagateMove(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Move32
				|| node.Instruction == IRInstruction.Move64
				|| node.Instruction == IRInstruction.MoveR4
				|| node.Instruction == IRInstruction.MoveR8))
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
						trace?.Log($"AFTER: \t{useNode}");
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			AddOperandUsageToWorkList(node);

			trace?.Log($"REMOVED:\t{node}");
			node.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount.Increment();
			PropagateMoveCount.Increment();
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
						PropagateCompoundMoveCount.Increment();
						trace?.Log($"AFTER: \t{useNode}");
					}
				}
			}

			Debug.Assert(destination.Uses.Count == 0);

			AddOperandUsageToWorkList(node);

			trace?.Log($"REMOVED:\t{node}");
			node.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount.Increment();
		}

		private void RemoveUselessIntegerCompareBranch(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64))
				return;

			if (node.Block.NextBlocks.Count != 1)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log($"REMOVED:\t{node}");
			node.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount.Increment();
			RemoveUselessIntegerCompareBranchCount.Increment();
		}

		private void ConstantFoldIntegerCompareBranch(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareIntBranch32
				|| node.Instruction == IRInstruction.CompareIntBranch64))
				return;

			Debug.Assert(node.OperandCount == 2);

			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op1.IsResolvedConstant || !op2.IsResolvedConstant)
				return;

			var nextNode = node.Next;

			if (nextNode.Instruction != IRInstruction.Jmp)
				return;

			if (node.BranchTargets[0] == nextNode.BranchTargets[0])
			{
				AddOperandUsageToWorkList(node);

				trace?.Log("*** FoldIntegerCompareBranch-Useless");
				trace?.Log($"REMOVED:\t{node}");
				node.SetInstruction(IRInstruction.Nop);
				InstructionsRemovedCount.Increment();
				FoldIntegerCompareBranchCount.Increment();
				return;
			}

			bool compareResult;
			switch (node.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ConstantUnsigned64 == op2.ConstantUnsigned64); break;
				case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsigned64 != op2.ConstantUnsigned64); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsigned64 >= op2.ConstantUnsigned64); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsigned64 > op2.ConstantUnsigned64); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsigned64 <= op2.ConstantUnsigned64); break;
				case ConditionCode.LessThan: compareResult = (op1.ConstantUnsigned64 < op2.ConstantUnsigned64); break;

				case ConditionCode.UnsignedGreaterThan: compareResult = (op1.ConstantUnsigned64 > op2.ConstantUnsigned64); break;
				case ConditionCode.UnsignedGreaterOrEqual: compareResult = (op1.ConstantUnsigned64 >= op2.ConstantUnsigned64); break;
				case ConditionCode.UnsignedLessThan: compareResult = (op1.ConstantUnsigned64 < op2.ConstantUnsigned64); break;
				case ConditionCode.UnsignedLessOrEqual: compareResult = (op1.ConstantUnsigned64 <= op2.ConstantUnsigned64); break;

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

			AddOperandUsageToWorkList(notUsed);

			trace?.Log($"REMOVED:\t{node}");
			notUsed.SetInstruction(IRInstruction.Nop);
			InstructionsRemovedCount.Increment();
			FoldIntegerCompareBranchCount.Increment();

			// if target block no longer has any predecessors (or the only predecessor is itself), remove all instructions from it.
			CheckAndClearEmptyBlock(notTaken);
		}

		private void CheckAndClearEmptyBlock(BasicBlock block)
		{
			if (block.PreviousBlocks.Count != 0 || block.IsHeadBlock)
				return;

			trace?.Log($"*** RemoveBlock: {block}");

			BlockRemovedCount.Increment();

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

			ulong r = add ? node.Operand2.ConstantUnsigned64 + node2.Operand2.ConstantUnsigned64 :
				node.Operand2.ConstantUnsigned64 - node2.Operand2.ConstantUnsigned64;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);

			trace?.Log("*** CombineAdditionAndSubstraction");
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineAdditionAndSubstractionCount.Increment();
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

			ulong r = node.Operand2.ConstantUnsigned64 | node2.Operand2.ConstantUnsigned64;

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);

			trace?.Log("*** CombineLogicalOr");
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineLogicalOrCount.Increment();
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

			ulong r = node.Operand2.ConstantUnsigned64 & node2.Operand2.ConstantUnsigned64;

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);

			trace?.Log("*** CombineLogicalAnd");
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineLogicalAndCount.Increment();
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

			ulong r = node.Operand2.ConstantUnsigned64 * node2.Operand2.ConstantUnsigned64;

			AddOperandUsageToWorkList(node2);

			trace?.Log("*** CombineMultiplication");
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineMultiplicationCount.Increment();
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
				(ulong)(node.Operand2.ConstantSigned64 * node2.Operand2.ConstantSigned64) :
				node.Operand2.ConstantUnsigned64 * node2.Operand2.ConstantUnsigned64;

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);

			trace?.Log("*** CombineDivision");
			trace?.Log($"BEFORE:\t{node2}");
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			trace?.Log($"AFTER: \t{node2}");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			trace?.Log($"AFTER: \t{node}");
			CombineDivisionCount.Increment();
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

			if (!(node2.Instruction == IRInstruction.Compare32x32
				|| node2.Instruction == IRInstruction.Compare64x32
				|| node2.Instruction == IRInstruction.Compare64x64))
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
			CombineIntegerCompareBranchCount.Increment();
			InstructionsRemovedCount.Increment();
		}

		private void FoldIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Compare32x32
				|| node.Instruction == IRInstruction.Compare64x32
				|| node.Instruction == IRInstruction.Compare64x64))
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

			if (!(node2.Instruction == IRInstruction.Compare32x32
				|| node2.Instruction == IRInstruction.Compare64x32
				|| node2.Instruction == IRInstruction.Compare64x64))
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
			FoldIntegerCompareCount.Increment();
			InstructionsRemovedCount.Increment();
		}

		private void FoldLoadStoreOffsets(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Load32
				|| node.Instruction == IRInstruction.Load64
				|| node.Instruction == IRInstruction.Store8
				|| node.Instruction == IRInstruction.Store16
				|| node.Instruction == IRInstruction.Store32
				|| node.Instruction == IRInstruction.Store64
				|| node.Instruction == IRInstruction.StoreR4
				|| node.Instruction == IRInstruction.StoreR8
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
				constant = Operand.CreateConstant(node.Operand2.Type, node2.Operand2.ConstantSigned64 + node.Operand2.ConstantSigned64);
			}
			else
			{
				constant = Operand.CreateConstant(node.Operand2.Type, node.Operand2.ConstantSigned64 - node2.Operand2.ConstantSigned64);
			}

			AddOperandUsageToWorkList(node);
			AddOperandUsageToWorkList(node2);

			trace?.Log("*** FoldLoadStoreOffsets");
			trace?.Log($"BEFORE:\t{node}");
			node.Operand1 = node2.Operand1;
			node.Operand2 = constant;
			trace?.Log($"AFTER: \t{node}");
			trace?.Log($"REMOVED:\t{node2}");
			node2.SetInstruction(IRInstruction.Nop);
			FoldLoadStoreOffsetsCount.Increment();
			InstructionsRemovedCount.Increment();
		}

		private void ConstantFoldingPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
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

				if (operand.ConstantUnsigned64 != operand1.ConstantUnsigned64)
					return;
			}

			AddOperandUsageToWorkList(node);

			trace?.Log("*** FoldConstantPhiInstruction");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(GetMoveInteger(result), result, operand1);
			trace?.Log($"AFTER: \t{node}");
			ConstantFoldingPhiCount.Increment();
		}

		private void SimplifyPhi2(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			var operand = node.Operand1;

			foreach (var op in node.Operands)
			{
				if (op != operand)
					return;
			}

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyPhiInstruction");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(GetMoveInteger(node.Result), node.Result, node.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyPhiCount.Increment();
		}

		// Doesn't work --- since non-lower byte is ignored
		private void SimplifyIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Compare32x32
				|| node.Instruction == IRInstruction.Compare64x32
				|| node.Instruction == IRInstruction.Compare64x64))
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

			if (!(node2.Instruction == IRInstruction.Compare32x32
				|| node2.Instruction == IRInstruction.Compare64x32
				|| node2.Instruction == IRInstruction.Compare64x64))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (!ValidateSSAForm(operand))
				return;

			if (operand.Is64BitInteger != node.Result.Is64BitInteger)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyIntegerCompare");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(GetMoveInteger(node.Result), node.Result, operand);
			trace?.Log($"AFTER: \t{node}");
			SimplifyIntegerCompareCount.Increment();
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
				AddOperandUsageToWorkList(node);

				trace?.Log("*** SimplifyAddCarryOut2");
				trace?.Log($"BEFORE:\t{node}");

				var context = new Context(node);
				context.SetInstruction(IRInstruction.Move32, result, operand1.IsConstantZero ? operand1 : operand2);
				context.AppendInstruction(IRInstruction.Move32, result2, ConstantZero);

				trace?.Log($"AFTER: \t{context}");
				trace?.Log($"AFTER: \t{context.Node.Previous}");
				SimplifyGeneralCount.Increment();
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
				AddOperandUsageToWorkList(node);

				trace?.Log("*** SimplifyAddWithCarry");
				trace?.Log($"BEFORE:\t{node}");
				node.SetInstruction(IRInstruction.Add32, result, operand1, operand2);
				trace?.Log($"AFTER: \t{node}");
				SimplifyGeneralCount.Increment();
				return;
			}

			if (operand3.IsResolvedConstant && !operand3.IsConstantZero)
			{
				var v1 = AllocateVirtualRegister(result.Type);

				var context = new Context(node);

				AddOperandUsageToWorkList(node);

				trace?.Log("*** SimplifyAddWithCarry");
				trace?.Log($"BEFORE:\t{node}");
				context.SetInstruction(IRInstruction.Add32, v1, operand1, operand2);
				context.AppendInstruction(IRInstruction.Add32, result, v1, CreateConstant(1));
				trace?.Log($"AFTER: \t{context.Node.Previous}");
				trace?.Log($"AFTER: \t{context}");
				SimplifyGeneralCount.Increment();
				return;
			}
		}

		private void SimplifyIntegerCompare2(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Compare32x32
				|| node.Instruction == IRInstruction.Compare64x32
				|| node.Instruction == IRInstruction.Compare64x64))
				return;

			if (node.ConditionCode != ConditionCode.UnsignedGreaterThan)
				return;

			if (!ValidateSSAForm(node.Result))
				return;

			if (!node.Operand2.IsConstantZero)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyIntegerCompare");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(Select(node.Result, !node.Operand1.Is64BitInteger ? (BaseInstruction)IRInstruction.Compare32x32 : IRInstruction.Compare64x32, IRInstruction.Compare64x64), ConditionCode.NotEqual, node.Result, node.Operand1, node.Operand2);
			trace?.Log($"AFTER: \t{node}");
			SimplifyIntegerCompareCount.Increment();
		}

		private void DeadCodeEliminationPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
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
			DeadCodeEliminationPhiCount.Increment();
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

			trace?.Log("*** GetLow64Propagation");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.Move32, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			LongPropagateCount.Increment();
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

			trace?.Log("*** GetHigh64Propagation");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.Move32, node.Result, node2.Operand2);
			trace?.Log($"AFTER: \t{node}");
			LongPropagateCount.Increment();
		}

		private void To64Propagation(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.To64)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (!node.Operand2.IsVirtualRegister)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			if (!ValidateSSAForm(node.Operand2))
				return;

			var node1 = node.Operand1.Definitions[0];

			if (node1.Instruction != IRInstruction.GetLow64)
				return;

			var node2 = node.Operand2.Definitions[0];

			if (node2.Instruction != IRInstruction.GetHigh64)
				return;

			if (node1.Operand1 != node2.Operand1)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** To64Propagation");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.Move32, node.Result, node1.Operand1);
			trace?.Log($"AFTER: \t{node}");
			To64PropagationCount.Increment();
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
			node.SetInstruction(IRInstruction.Move32, node.Result, node.Operand1);
			trace?.Log($"AFTER: \t{node}");
			LongConstantFoldingCount.Increment();
		}

		private void SimplifyParamLoad(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.Load32
				|| node.Instruction == IRInstruction.Load64
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
				|| node.Instruction == IRInstruction.LoadR4
				|| node.Instruction == IRInstruction.LoadR8))
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
			if ((node.Instruction == IRInstruction.Load32 || node.Instruction == IRInstruction.Load64) && paramType.IsFloatingPoint)
				return;
			else if ((node.Instruction == IRInstruction.LoadR4 || node.Instruction == IRInstruction.LoadR8) && paramType.IsInteger)
				return;

			if (node.Instruction == IRInstruction.Load32)
				instruction = IRInstruction.LoadParam32;
			else if (node.Instruction == IRInstruction.Load64)
				instruction = IRInstruction.LoadParam64;
			else if (node.Instruction == IRInstruction.LoadR4)
				instruction = IRInstruction.LoadParamR4;
			else if (node.Instruction == IRInstruction.LoadR8)
				instruction = IRInstruction.LoadParamR8;
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
			SimplifyParamLoadCount.Increment();
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
		//		FoldIfThenElseCount.Increment();
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

			trace?.Log("*** SimplifyCompareBranch");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.CompareIntBranch32, node.ConditionCode, null, node.Operand1.Definitions[0].Operand1, node.Operand2.Definitions[0].Operand1, node.BranchTargets[0]);
			trace?.Log($"AFTER: \t{node}");
			SimplifyBranchComparisonCount.Increment();
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

			if (node2.Operand2.ConstantSigned32 != 32)
				return;

			if (!ValidateSSAForm(node2.Operand1))
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyGetLow64");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.GetHigh64, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetLowCount.Increment();
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

			if (node2.Operand2.ConstantSigned32 != 32)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyGetHigh64");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.GetLow64, node.Result, node2.Operand1);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetHighCount.Increment();
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

			if (node2.Operand2.ConstantSigned32 < 32)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyGetLow64b");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.Move32, node.Result, ConstantZero);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetHighCount.Increment();
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

			if (node2.Operand2.ConstantSigned32 < 32)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** SimplifyGetHigh64b");
			trace?.Log($"BEFORE:\t{node}");
			node.SetInstruction(IRInstruction.Move32, node.Result, ConstantZero);
			trace?.Log($"AFTER: \t{node}");
			SimplifyGetHighCount.Increment();
		}

		private void GetLowWhenZeroHighSimplification(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!ValidateSSAForm(node.Operand1))
				return;

			var use1 = node.Operand1.Definitions[0];

			var instruction = use1.Instruction;

			if (instruction != IRInstruction.ShiftLeft64 && instruction != IRInstruction.ArithShiftRight64 && instruction != IRInstruction.ShiftRight64)
				return;

			if (!use1.Operand1.IsVirtualRegister)
				return;

			// Below is optional -- hard to see which way is better
			if (use1.Result.Uses.Count > 1)
				return;

			if (!ValidateSSAForm(use1.Operand1))
				return;

			var use2 = use1.Operand1.Definitions[0];

			if (use2.Instruction != IRInstruction.To64)
				return;

			if (!use2.Operand2.IsConstantZero)
				return;

			AddOperandUsageToWorkList(node);

			trace?.Log("*** GetLowWhenZeroHighSimplification");
			trace?.Log($"BEFORE:\t{node}");

			if (instruction == IRInstruction.ShiftLeft64)
			{
				node.SetInstruction(IRInstruction.ShiftLeft32, node.Result, use2.Operand1, use1.Operand2);
			}
			else if (instruction == IRInstruction.ArithShiftRight64 || instruction == IRInstruction.ShiftRight64)
			{
				node.SetInstruction(IRInstruction.ShiftRight32, node.Result, use2.Operand1, use1.Operand2);
			}

			trace?.Log($"AFTER: \t{node}");
			GetLowWhenZeroHighSimplificationCount.Increment();
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
