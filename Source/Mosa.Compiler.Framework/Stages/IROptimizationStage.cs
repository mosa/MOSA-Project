// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.MosaTypeSystem;
using Mosa.Compiler.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// IR Optimization Stage
	/// </summary>
	public sealed class IROptimizationStage : BaseMethodCompilerStage
	{
		private int instructionsRemovedCount = 0;
		private int simplifyExtendedMoveWithConstantCount = 0;
		private int arithmeticSimplificationSubtractionCount = 0;
		private int arithmeticSimplificationMultiplicationCount = 0;
		private int arithmeticSimplificationDivisionCount = 0;
		private int arithmeticSimplificationAdditionAndSubstractionCount = 0;
		private int arithmeticSimplificationLogicalOperatorsCount = 0;
		private int arithmeticSimplificationShiftOperators = 0;
		private int simpleConstantPropagationCount = 0;
		private int forwardPropagateMove = 0;
		private int forwardPropagateCompoundMove = 0;
		private int deadCodeEliminationCount = 0;
		private int reduceTruncationAndExpansionCount = 0;
		private int constantFoldingIntegerOperationsCount = 0;
		private int constantFoldingIntegerCompareCount = 0;
		private int constantFoldingAdditionAndSubstractionCount = 0;
		private int constantFoldingMultiplicationCount = 0;
		private int constantFoldingDivisionCount = 0;
		private int constantFoldingPhiCount = 0;
		private int blockRemovedCount = 0;
		private int foldIntegerCompareBranchCount = 0;
		private int reduceZeroExtendedMoveCount = 0;
		private int foldIntegerCompareCount = 0;
		private int simplifyExtendedMoveCount = 0;
		private int foldLoadStoreOffsetsCount = 0;
		private int constantMoveToRightCount = 0;
		private int simplifyPhiCount = 0;
		private int deadCodeEliminationPhi = 0;
		private int constantFoldingLogicalOrCount = 0;
		private int constantFoldingLogicalAndCount = 0;
		private int combineIntegerCompareBranchCount = 0;
		private int removeUselessIntegerCompareBranch = 0;
		private int arithmeticSimplificationModulus = 0;
		private int split64Constant = 0;
		private int simplifyTo64 = 0;
		private int simplifySplit64 = 0;
		private int reduceSplit64 = 0;
		private int simplifyIntegerCompare = 0;

		private Stack<InstructionNode> worklist;

		private HashSet<Operand> virtualRegisters;

		private TraceLog trace;

		private delegate void Transformation(InstructionNode node);

		private List<Transformation> transformations;

		private int debugRestrictOptimizationByCount = 0;

		protected override void Initialize()
		{
			base.Initialize();

			worklist = new Stack<InstructionNode>();
			virtualRegisters = new HashSet<Operand>();

			debugRestrictOptimizationByCount = CompilerOptions.DebugRestrictOptimizationByCount;

			transformations = CreateTransformationList();
		}

		protected override void Setup()
		{
			base.Setup();

			instructionsRemovedCount = 0;
			simplifyExtendedMoveWithConstantCount = 0;
			arithmeticSimplificationSubtractionCount = 0;
			arithmeticSimplificationMultiplicationCount = 0;
			arithmeticSimplificationDivisionCount = 0;
			arithmeticSimplificationAdditionAndSubstractionCount = 0;
			arithmeticSimplificationLogicalOperatorsCount = 0;
			arithmeticSimplificationShiftOperators = 0;
			simpleConstantPropagationCount = 0;
			forwardPropagateMove = 0;
			forwardPropagateCompoundMove = 0;
			deadCodeEliminationCount = 0;
			reduceTruncationAndExpansionCount = 0;
			constantFoldingIntegerOperationsCount = 0;
			constantFoldingIntegerCompareCount = 0;
			constantFoldingAdditionAndSubstractionCount = 0;
			constantFoldingMultiplicationCount = 0;
			constantFoldingDivisionCount = 0;
			constantFoldingPhiCount = 0;
			blockRemovedCount = 0;
			foldIntegerCompareBranchCount = 0;
			reduceZeroExtendedMoveCount = 0;
			foldIntegerCompareCount = 0;
			simplifyExtendedMoveCount = 0;
			foldLoadStoreOffsetsCount = 0;
			constantMoveToRightCount = 0;
			simplifyPhiCount = 0;
			deadCodeEliminationPhi = 0;
			constantFoldingLogicalOrCount = 0;
			constantFoldingLogicalAndCount = 0;
			combineIntegerCompareBranchCount = 0;
			removeUselessIntegerCompareBranch = 0;
			arithmeticSimplificationModulus = 0;
			split64Constant = 0;
			//simplifyTo64 = 0;
			//simplifySplit64 = 0;
			//reduceSplit64 = 0;
			simplifyIntegerCompare = 0;
		}

		protected override void Run()
		{
			// Method is empty - must be a plugged method
			if (!HasCode)
				return;

			trace = CreateTraceLog();

			Optimize();

			UpdateCounter("IROptimizations.IRInstructionRemoved", instructionsRemovedCount);
			UpdateCounter("IROptimizations.ConstantFoldingIntegerOperations", constantFoldingIntegerOperationsCount);
			UpdateCounter("IROptimizations.ConstantFoldingIntegerCompare", constantFoldingIntegerCompareCount);
			UpdateCounter("IROptimizations.ConstantFoldingAdditionAndSubstraction", constantFoldingAdditionAndSubstractionCount);
			UpdateCounter("IROptimizations.ConstantFoldingMultiplication", constantFoldingMultiplicationCount);
			UpdateCounter("IROptimizations.ConstantFoldingDivision", constantFoldingDivisionCount);
			UpdateCounter("IROptimizations.ConstantFoldingLogicalOr", constantFoldingLogicalOrCount);
			UpdateCounter("IROptimizations.ConstantFoldingLogicalAnd", constantFoldingLogicalAndCount);
			UpdateCounter("IROptimizations.ConstantFoldingPhi", constantFoldingPhiCount);
			UpdateCounter("IROptimizations.ConstantMoveToRight", constantMoveToRightCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationSubtraction", arithmeticSimplificationSubtractionCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationMultiplication", arithmeticSimplificationMultiplicationCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationDivision", arithmeticSimplificationDivisionCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationAdditionAndSubstraction", arithmeticSimplificationAdditionAndSubstractionCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationLogicalOperators", arithmeticSimplificationLogicalOperatorsCount);
			UpdateCounter("IROptimizations.ArithmeticSimplificationShiftOperators", arithmeticSimplificationShiftOperators);
			UpdateCounter("IROptimizations.ArithmeticSimplificationModulus", arithmeticSimplificationModulus);
			UpdateCounter("IROptimizations.SimpleConstantPropagation", simpleConstantPropagationCount);
			UpdateCounter("IROptimizations.ForwardPropagateMove", forwardPropagateMove);
			UpdateCounter("IROptimizations.ForwardPropagateCompoundMove", forwardPropagateCompoundMove);
			UpdateCounter("IROptimizations.FoldIntegerCompareBranch", foldIntegerCompareBranchCount);
			UpdateCounter("IROptimizations.FoldIntegerCompare", foldIntegerCompareCount);
			UpdateCounter("IROptimizations.FoldLoadStoreOffsets", foldLoadStoreOffsetsCount);
			UpdateCounter("IROptimizations.DeadCodeElimination", deadCodeEliminationCount);
			UpdateCounter("IROptimizations.DeadCodeEliminationPhi", deadCodeEliminationPhi);
			UpdateCounter("IROptimizations.ReduceTruncationAndExpansion", reduceTruncationAndExpansionCount);
			UpdateCounter("IROptimizations.CombineIntegerCompareBranch", combineIntegerCompareBranchCount);
			UpdateCounter("IROptimizations.ReduceZeroExtendedMove", reduceZeroExtendedMoveCount);
			UpdateCounter("IROptimizations.SimplifyExtendedMove", simplifyExtendedMoveCount);
			UpdateCounter("IROptimizations.SimplifyExtendedMoveWithConstant", simplifyExtendedMoveWithConstantCount);
			UpdateCounter("IROptimizations.SimplifyPhi", simplifyPhiCount);
			UpdateCounter("IROptimizations.BlockRemoved", blockRemovedCount);
			UpdateCounter("IROptimizations.RemoveUselessIntegerCompareBranch", removeUselessIntegerCompareBranch);
			//UpdateCounter("IROptimizations.Split64Constant", split64Constant);
			//UpdateCounter("IROptimizations.SimplifyTo64", simplifyTo64);
			//UpdateCounter("IROptimizations.SimplifySplit64", simplifySplit64);
			//UpdateCounter("IROptimizations.ReduceSplit64", reduceSplit64);
			UpdateCounter("IROptimizations.SimplifyIntegerCompare", simplifyIntegerCompare);
		}

		protected override void Finish()
		{
			base.Finish();

			virtualRegisters.Clear();
			worklist.Clear();
			trace = null;
		}

		private void Optimize()
		{
			foreach (var block in BasicBlocks)
			{
				for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.ResultCount == 0 && node.OperandCount == 0)
						continue;

					Do(node);

					ProcessWorkList();

					// Collect virtual registers
					if (node.IsEmpty)
						continue;

					// add virtual registers
					foreach (var op in node.Results)
					{
						if (op.IsVirtualRegister)
							virtualRegisters.AddIfNew(op);
					}

					foreach (var op in node.Operands)
					{
						if (op.IsVirtualRegister)
							virtualRegisters.AddIfNew(op);
					}
				}
			}
		}

		private List<Transformation> CreateTransformationList()
		{
			return new List<Transformation>()
			{
				SimpleConstantPropagation,
				ForwardPropagateMove,
				ForwardPropagateCompoundMove,
				DeadCodeElimination,
				ConstantFoldingIntegerOperations32,
				ConstantFoldingIntegerOperations64,
				ConstantMoveToRight,
				ArithmeticSimplificationSubtraction,
				ArithmeticSimplificationMultiplication,
				ArithmeticSimplificationDivision,
				ArithmeticSimplificationAdditionAndSubstraction,
				ArithmeticSimplificationRemUnsignedModulus,
				ArithmeticSimplificationRemSignedModulus,
				ArithmeticSimplificationLogicalOperators,
				ArithmeticSimplificationShiftOperators,

				//ReduceZeroExtendedMove,
				ConstantFoldingAdditionAndSubstraction,
				ConstantFoldingMultiplication,
				ConstantFoldingDivision,
				ConstantFoldingIntegerCompare,
				ConstantFoldingLogicalOr,
				ConstantFoldingLogicalAnd,
				CombineIntegerCompareBranch,
				FoldIntegerCompare,
				RemoveUselessIntegerCompareBranch,
				ConstantFoldIntegerCompareBranch,

				//ReduceTruncationAndExpansion,
				SimplifyExtendedMoveWithConstant,
				SimplifyExtendedMove,
				SimplifyIntegerCompare2,
				SimplifyIntegerCompare,
				FoldLoadStoreOffsets,
				ConstantFoldingPhi,
				SimplifyPhi,
				SimplifyPhi2,
				DeadCodeEliminationPhi,
				NormalizeConstantTo32Bit,
				GetLow64Constant,
				GetHigh64Constant,
			};
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
				if (node.IsEmpty)
					return;

				method.Invoke(node);
			}
		}

		private void AddToWorkList(InstructionNode node)
		{
			if (node.IsEmpty)
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

		private BaseInstruction GetMoveInteger(Operand operand)
		{
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.MoveInteger64 : IRInstruction.MoveInteger32;
		}

		private bool ContainsAddressOf(Operand local)
		{
			foreach (var node in local.Uses)
			{
				if (node.Instruction == IRInstruction.AddressOf)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Removes the useless (dead) code
		/// </summary>
		/// <param name="node">The node.</param>
		private void DeadCodeElimination(InstructionNode node)
		{
			if (node.ResultCount == 0 || node.ResultCount > 2)
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (node.ResultCount == 2 && !node.Result2.IsVirtualRegister)
				return;

			if (node.ResultCount == 2 && node.Result2.Definitions.Count != 1)
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

			if ((node.Instruction == IRInstruction.MoveInteger32 || node.Instruction == IRInstruction.MoveInteger64)
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

		/// <summary>
		/// Simple constant propagation.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SimpleConstantPropagation(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MoveInteger32 || node.Instruction == IRInstruction.MoveInteger64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			Debug.Assert(node.Result.Definitions.Count == 1);

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

					if (operand == node.Result)
					{
						propogated = true;

						if (trace.Active) trace.Log("*** SimpleConstantPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);

						AddOperandUsageToWorkList(operand);
						useNode.SetOperand(i, source);
						simpleConstantPropagationCount++;
						if (trace.Active) trace.Log("AFTER: \t" + useNode);
					}
				}

				if (propogated)
				{
					AddToWorkList(useNode);
				}
			}
		}

		private bool CanCopyPropagation(Operand source, Operand destination)
		{
			if (source.IsReferenceType && destination.IsReferenceType)
				return true;

			if (source.IsUnmanagedPointer && destination.IsUnmanagedPointer)
				return true;

			if (source.IsManagedPointer && destination.IsManagedPointer)
				return true;

			if (source.IsFunctionPointer && destination.IsFunctionPointer)
				return true;

			if (source.Type.IsUI1 & destination.Type.IsUI1)
				return true;

			if (source.Type.IsUI2 & destination.Type.IsUI2)
				return true;

			if (source.Type.IsUI4 & destination.Type.IsUI4)
				return true;

			if (source.Type.IsUI8 & destination.Type.IsUI8)
				return true;

			if (NativePointerSize == 4 && (destination.IsI || destination.IsU || destination.IsUnmanagedPointer) && (source.IsI4 || source.IsU4))
				return true;

			if (NativePointerSize == 4 && (source.IsI || source.IsU || source.IsUnmanagedPointer) && (destination.IsI4 || destination.IsU4))
				return true;

			if (NativePointerSize == 8 && (destination.IsI || destination.IsU || destination.IsUnmanagedPointer) && (source.IsI8 || source.IsU8))
				return true;

			if (NativePointerSize == 8 && (source.IsI || source.IsU || source.IsUnmanagedPointer) && (destination.IsI8 || destination.IsU8))
				return true;

			if (source.IsR4 && destination.IsR4)
				return true;

			if (source.IsR8 && destination.IsR8)
				return true;

			if ((source.IsI || source.IsU) && (destination.IsI || destination.IsU))
				return true;

			if (source.IsValueType || destination.IsValueType)
				return false;

			if (source.Type == destination.Type)
				return true;

			if (source.Type.IsArray & destination.Type.IsArray & source.Type.ElementType == destination.Type.ElementType)
				return true;

			return false;
		}

		/// <summary>
		/// Simple copy propagation.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ForwardPropagateMove(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MoveInteger32
				|| node.Instruction == IRInstruction.MoveInteger64
				|| node.Instruction == IRInstruction.MoveFloatR4
				|| node.Instruction == IRInstruction.MoveFloatR8))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (node.Operand1.Definitions.Count != 1)
				return;

			if (node.Operand1.IsResolvedConstant)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			// If the pointer or reference types are different, we can not copy propagation because type information would be lost.
			// Also if the operand sign is different, we cannot do it as it requires a signed/unsigned extended move, not a normal move
			if (!CanCopyPropagation(node.Result, node.Operand1))
				return;

			Operand destination = node.Result;
			Operand source = node.Operand1;

			Debug.Assert(destination != source);

			if (ContainsAddressOf(destination))
				return;

			// for each statement T that uses operand, substituted c in statement T
			AddOperandUsageToWorkList(node);

			foreach (var useNode in destination.Uses.ToArray())
			{
				for (int i = 0; i < useNode.OperandCount; i++)
				{
					var operand = useNode.GetOperand(i);

					if (destination == operand)
					{
						if (trace.Active) trace.Log("*** SimpleForwardCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);
						useNode.SetOperand(i, source);
						forwardPropagateMove++;
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
		/// Simple copy propagation.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ForwardPropagateCompoundMove(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.MoveCompound)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (node.Operand1.Definitions.Count != 1)
				return;

			Operand destination = node.Result;
			Operand source = node.Operand1;

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
						if (trace.Active) trace.Log("*** FoldCompoundCopyPropagation");
						if (trace.Active) trace.Log("BEFORE:\t" + useNode);
						useNode.SetOperand(i, source);
						forwardPropagateCompoundMove++;
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

		private void ConstantFoldingIntegerOperations64(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.AddSigned64
				|| node.Instruction == IRInstruction.AddUnsigned64
				|| node.Instruction == IRInstruction.SubSigned64
				|| node.Instruction == IRInstruction.SubUnsigned64
				|| node.Instruction == IRInstruction.LogicalAnd64
				|| node.Instruction == IRInstruction.LogicalOr64
				|| node.Instruction == IRInstruction.LogicalXor64
				|| node.Instruction == IRInstruction.MulSigned64
				|| node.Instruction == IRInstruction.MulUnsigned64
				|| node.Instruction == IRInstruction.DivSigned64
				|| node.Instruction == IRInstruction.DivUnsigned64
				|| node.Instruction == IRInstruction.RemSigned64
				|| node.Instruction == IRInstruction.RemUnsigned64
				|| node.Instruction == IRInstruction.ArithmeticShiftRight64
				|| node.Instruction == IRInstruction.ShiftLeft64
				|| node.Instruction == IRInstruction.ShiftRight64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!op1.IsResolvedConstant || !op2.IsResolvedConstant)
				return;

			// Divide by zero!
			if ((node.Instruction == IRInstruction.DivSigned64
				|| node.Instruction == IRInstruction.DivUnsigned64
				|| node.Instruction == IRInstruction.RemSigned64
				|| node.Instruction == IRInstruction.RemUnsigned64)
				&& op2.IsConstantZero)
				return;

			ulong value = 0;

			if (node.Instruction == IRInstruction.AddSigned64 || node.Instruction == IRInstruction.AddUnsigned64)
			{
				value = op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.SubSigned64 || node.Instruction == IRInstruction.SubUnsigned64)
			{
				value = op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.LogicalAnd64)
			{
				value = op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.LogicalOr64)
			{
				value = op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.LogicalXor64)
			{
				value = op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.MulSigned64 || node.Instruction == IRInstruction.MulUnsigned64)
			{
				value = op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.DivUnsigned64 || node.Instruction == IRInstruction.DivUnsigned64)
			{
				value = op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivSigned64)
			{
				value = (ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger);
			}
			else if (node.Instruction == IRInstruction.ArithmeticShiftRight64 || node.Instruction == IRInstruction.ArithmeticShiftRight64)
			{
				value = (ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger);
			}
			else if (node.Instruction == IRInstruction.ShiftRight64)
			{
				value = op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.ShiftLeft64)
			{
				value = op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.RemSigned64)
			{
				value = (ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger);
			}
			else if (node.Instruction == IRInstruction.RemUnsigned64)
			{
				value = op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger;
			}
			else
			{
				return;
			}

			var constant = CreateConstant(result.Type, value);

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ConstantFoldingIntegerOperations64");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInteger64, node.Result, constant);
			constantFoldingIntegerOperationsCount++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
		}

		private void ConstantFoldingIntegerOperations32(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.AddSigned32
				|| node.Instruction == IRInstruction.AddUnsigned32
				|| node.Instruction == IRInstruction.SubSigned32
				|| node.Instruction == IRInstruction.SubUnsigned32
				|| node.Instruction == IRInstruction.LogicalAnd32
				|| node.Instruction == IRInstruction.LogicalOr32
				|| node.Instruction == IRInstruction.LogicalXor32
				|| node.Instruction == IRInstruction.MulSigned32
				|| node.Instruction == IRInstruction.MulUnsigned32
				|| node.Instruction == IRInstruction.DivSigned32
				|| node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.RemSigned32
				|| node.Instruction == IRInstruction.RemUnsigned32
				|| node.Instruction == IRInstruction.ArithmeticShiftRight32
				|| node.Instruction == IRInstruction.ShiftLeft32
				|| node.Instruction == IRInstruction.ShiftRight32))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!op1.IsResolvedConstant || !op2.IsResolvedConstant)
				return;

			// Divide by zero!
			if ((node.Instruction == IRInstruction.DivSigned32
				|| node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.RemSigned32
				|| node.Instruction == IRInstruction.RemUnsigned32)
				&& op2.IsConstantZero)
				return;

			ulong value = 0;

			if (node.Instruction == IRInstruction.AddSigned32 || node.Instruction == IRInstruction.AddUnsigned32)
			{
				value = op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.SubSigned32 || node.Instruction == IRInstruction.SubUnsigned32)
			{
				value = op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.LogicalAnd32)
			{
				value = op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.LogicalOr32)
			{
				value = op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.LogicalXor32)
			{
				value = op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.MulSigned32 || node.Instruction == IRInstruction.MulUnsigned32)
			{
				value = op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivUnsigned64)
			{
				value = op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivSigned64)
			{
				value = (ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger);
			}
			else if (node.Instruction == IRInstruction.ArithmeticShiftRight32 || node.Instruction == IRInstruction.ArithmeticShiftRight64)
			{
				value = (ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger);
			}
			else if (node.Instruction == IRInstruction.ShiftRight32)
			{
				value = op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.ShiftLeft32)
			{
				value = op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger;
			}
			else if (node.Instruction == IRInstruction.RemSigned32)
			{
				value = (ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger);
			}
			else if (node.Instruction == IRInstruction.RemUnsigned32)
			{
				value = op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger;
			}
			else
			{
				return;
			}

			var constant = CreateConstant(result.Type, value & 0xFFFFFFFF);

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ConstantFoldingIntegerOperations32");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInteger32, node.Result, constant);
			constantFoldingIntegerOperationsCount++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
		}

		/// <summary>
		/// Folds the integer compare on constants
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConstantFoldingIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInteger32x32
				|| node.Instruction == IRInstruction.CompareInteger64x32
				|| node.Instruction == IRInstruction.CompareInteger64x64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!(op1.IsResolvedConstant && op2.IsResolvedConstant))
				return;

			if (!op1.IsValueType || !op2.IsValueType)
				return;

			bool compareResult = true;

			switch (node.ConditionCode)
			{
				case ConditionCode.Equal: compareResult = (op1.ConstantUnsignedLongInteger == op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.NotEqual: compareResult = (op1.ConstantUnsignedLongInteger != op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.GreaterOrEqual: compareResult = (op1.ConstantUnsignedLongInteger >= op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.GreaterThan: compareResult = (op1.ConstantUnsignedLongInteger > op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.LessOrEqual: compareResult = (op1.ConstantUnsignedLongInteger <= op2.ConstantUnsignedLongInteger); break;
				case ConditionCode.LessThan: compareResult = (op1.ConstantUnsignedLongInteger < op2.ConstantUnsignedLongInteger); break;

				// TODO: Add more
				default: return;
			}

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ConstantFoldingIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(GetMoveInteger(result), result, CreateConstant(result.Type, compareResult ? 1 : 0));
			constantFoldingIntegerCompareCount++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
		}

		/// <summary>
		/// Strength reduction for integer addition when one of the constants is zero
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationAdditionAndSubstraction(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.AddSigned32
				|| node.Instruction == IRInstruction.AddUnsigned32
				|| node.Instruction == IRInstruction.AddSigned64
				|| node.Instruction == IRInstruction.AddUnsigned64
				|| node.Instruction == IRInstruction.SubSigned32
				|| node.Instruction == IRInstruction.SubUnsigned32
				|| node.Instruction == IRInstruction.SubSigned64
				|| node.Instruction == IRInstruction.SubUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!op1.IsResolvedConstant && op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationAdditionAndSubstraction");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, op1);
				if (trace.Active) trace.Log("AFTER: \t" + node);
				arithmeticSimplificationAdditionAndSubstractionCount++;
				return;
			}
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

			if (!node.Result.IsVirtualRegister)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (op2.IsConstantZero)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, ConstantZero);
				arithmeticSimplificationMultiplicationCount++;
				if (trace.Active) trace.Log("AFTER: \t" + node);
				return;
			}

			if (op2.IsConstantOne)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationMultiplication");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, op1);
				arithmeticSimplificationMultiplicationCount++;
				if (trace.Active) trace.Log("AFTER: \t" + node);
				return;
			}

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

		/// <summary>
		/// Strength reduction for division when one of the constants is zero or one
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationDivision(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.DivSigned32
				|| node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.DivSigned64
				|| node.Instruction == IRInstruction.DivUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!op2.IsResolvedConstant || op2.IsConstantZero)
			{
				// Possible divide by zero
				return;
			}

			if (op1.IsResolvedConstant && op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationDivision");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, ConstantZero);
				arithmeticSimplificationDivisionCount++;
				if (trace.Active) trace.Log("AFTER: \t" + node);
				return;
			}

			if (op2.IsResolvedConstant && op2.IsConstantOne)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationDivision");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, op1);
				arithmeticSimplificationDivisionCount++;
				if (trace.Active) trace.Log("AFTER: \t" + node);
				return;
			}

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

		/// <summary>
		/// Simplifies extended moves with a constant
		/// </summary>
		/// <param name="node">The node.</param>
		private void SimplifyExtendedMoveWithConstant(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.SignExtended8x32
				|| node.Instruction == IRInstruction.SignExtended16x32
				|| node.Instruction == IRInstruction.SignExtended8x64
				|| node.Instruction == IRInstruction.SignExtended16x64
				|| node.Instruction == IRInstruction.SignExtended32x64
				|| node.Instruction == IRInstruction.ZeroExtended8x32
				|| node.Instruction == IRInstruction.ZeroExtended16x32
				|| node.Instruction == IRInstruction.ZeroExtended8x64
				|| node.Instruction == IRInstruction.ZeroExtended16x64
				|| node.Instruction == IRInstruction.ZeroExtended32x64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;

			Operand newOperand;

			if ((node.Instruction == IRInstruction.ZeroExtended8x32
				|| node.Instruction == IRInstruction.ZeroExtended16x32
				|| node.Instruction == IRInstruction.ZeroExtended8x64
				|| node.Instruction == IRInstruction.ZeroExtended16x64
				|| node.Instruction == IRInstruction.ZeroExtended32x64)
				&& result.IsUnsigned && op1.IsSigned)
			{
				var newConstant = Unsign(op1.Type, op1.ConstantSignedLongInteger);
				newOperand = CreateConstant(node.Result.Type, newConstant);
			}
			else
			{
				newOperand = CreateConstant(node.Result.Type, op1.ConstantUnsignedLongInteger);
			}

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** SimplifyExtendedMoveWithConstant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(GetMoveInteger(result), result, newOperand);
			simplifyExtendedMoveWithConstantCount++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
		}

		static private ulong Unsign(MosaType type, long value)
		{
			if (type.IsI1) return (ulong)((sbyte)value);
			else if (type.IsI2) return (ulong)((short)value);
			else if (type.IsI4) return (ulong)((int)value);
			else if (type.IsI8) return (ulong)value;
			else return (ulong)value;
		}

		/// <summary>
		/// Simplifies subtraction where both operands are the same
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationSubtraction(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.SubSigned32
				|| node.Instruction == IRInstruction.SubUnsigned32
				|| node.Instruction == IRInstruction.SubSigned64
				|| node.Instruction == IRInstruction.SubUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (op1 != op2)
				return;

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ArithmeticSimplificationSubtraction");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(GetMoveInteger(result), result, ConstantZero);
			arithmeticSimplificationSubtractionCount++;
		}

		/// <summary>
		/// Strength reduction for logical operators
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationLogicalOperators(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LogicalAnd32
				|| node.Instruction == IRInstruction.LogicalOr32
				|| node.Instruction == IRInstruction.LogicalAnd64
				|| node.Instruction == IRInstruction.LogicalOr64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (node.Instruction == IRInstruction.LogicalOr32 || node.Instruction == IRInstruction.LogicalOr64)
			{
				if (op2.IsConstantZero)
				{
					AddOperandUsageToWorkList(node);
					if (trace.Active) trace.Log("*** ArithmeticSimplificationLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(GetMoveInteger(result), result, op1);
					arithmeticSimplificationLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + node);
					return;
				}
			}
			else if (node.Instruction == IRInstruction.LogicalAnd32 || node.Instruction == IRInstruction.LogicalAnd64)
			{
				if (op2.IsConstantZero)
				{
					AddOperandUsageToWorkList(node);
					if (trace.Active) trace.Log("*** ArithmeticSimplificationLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(GetMoveInteger(result), result, ConstantZero);
					arithmeticSimplificationLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + node);
					return;
				}

				if (((result.IsI4 || result.IsU4 || result.IsI || result.IsU) && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
					|| ((result.IsI8 || result.IsU8) && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF))
				{
					AddOperandUsageToWorkList(node);
					if (trace.Active) trace.Log("*** ArithmeticSimplificationLogicalOperators");
					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.SetInstruction(GetMoveInteger(result), result, op1);
					arithmeticSimplificationLogicalOperatorsCount++;
					if (trace.Active) trace.Log("AFTER: \t" + node);
					return;
				}
			}

			// TODO: Add more strength reductions especially for AND w/ 0xFF, 0xFFFF, 0xFFFFFFFF, etc when source or destination are same or smaller
		}

		/// <summary>
		/// Strength reduction shift operators.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationShiftOperators(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.ShiftLeft32
				|| node.Instruction == IRInstruction.ShiftRight32
				|| node.Instruction == IRInstruction.ShiftLeft64
				|| node.Instruction == IRInstruction.ShiftRight64
				|| node.Instruction == IRInstruction.ArithmeticShiftRight32
				|| node.Instruction == IRInstruction.ArithmeticShiftRight64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (op2.IsConstantZero || op1.IsConstantZero)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationShiftOperators");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, op1);
				arithmeticSimplificationShiftOperators++;
				if (trace.Active) trace.Log("AFTER: \t" + node);
				return;
			}
		}

		/// <summary>
		/// Removes the unless integer compare branch.
		/// </summary>
		/// <param name="node">The node.</param>
		private void RemoveUselessIntegerCompareBranch(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.CompareIntegerBranch)
				return;

			if (node.Block.NextBlocks.Count != 1)
				return;

			if (trace.Active) trace.Log("REMOVED:\t" + node);
			AddOperandUsageToWorkList(node);
			node.SetInstruction(IRInstruction.Nop);
			instructionsRemovedCount++;
			removeUselessIntegerCompareBranch++;
		}

		/// <summary>
		/// Folds integer compare branch.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConstantFoldIntegerCompareBranch(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.CompareIntegerBranch)
				return;

			Debug.Assert(node.OperandCount == 2);

			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!op1.IsResolvedConstant || !op2.IsResolvedConstant)
				return;

			Operand result = node.Result;
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

			UpdatePhiList(block, nextBlocks);

			Debug.Assert(block.NextBlocks.Count == 0);
			Debug.Assert(block.PreviousBlocks.Count == 0);
		}

		private void ConstantMoveToRight(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.AddSigned32
				|| node.Instruction == IRInstruction.AddUnsigned32
				|| node.Instruction == IRInstruction.AddSigned64
				|| node.Instruction == IRInstruction.AddUnsigned64
				|| node.Instruction == IRInstruction.MulSigned32
				|| node.Instruction == IRInstruction.MulUnsigned32
				|| node.Instruction == IRInstruction.MulSigned64
				|| node.Instruction == IRInstruction.MulUnsigned64
				|| node.Instruction == IRInstruction.LogicalAnd32
				|| node.Instruction == IRInstruction.LogicalOr32
				|| node.Instruction == IRInstruction.LogicalXor32
				|| node.Instruction == IRInstruction.LogicalAnd64
				|| node.Instruction == IRInstruction.LogicalOr64
				|| node.Instruction == IRInstruction.LogicalXor64))
				return;

			if (node.Operand2.IsResolvedConstant)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ConstantMoveToRight");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			var op1 = node.Operand1;
			node.Operand1 = node.Operand2;
			node.Operand2 = op1;
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantMoveToRightCount++;
		}

		private void ConstantFoldingAdditionAndSubstraction(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.AddSigned32
				|| node.Instruction == IRInstruction.AddUnsigned32
				|| node.Instruction == IRInstruction.AddSigned64
				|| node.Instruction == IRInstruction.AddUnsigned64
				|| node.Instruction == IRInstruction.SubSigned32
				|| node.Instruction == IRInstruction.SubUnsigned32
				|| node.Instruction == IRInstruction.SubSigned64
				|| node.Instruction == IRInstruction.SubUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.AddSigned32
				|| node2.Instruction == IRInstruction.AddUnsigned32
				|| node2.Instruction == IRInstruction.AddSigned64
				|| node2.Instruction == IRInstruction.AddUnsigned64
				|| node2.Instruction == IRInstruction.SubSigned32
				|| node2.Instruction == IRInstruction.SubUnsigned32
				|| node2.Instruction == IRInstruction.SubSigned64
				|| node2.Instruction == IRInstruction.SubUnsigned64))
				return;

			if (!node2.Result.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			bool add = true;

			if ((node.Instruction == IRInstruction.AddSigned32
				|| node.Instruction == IRInstruction.AddUnsigned32
				|| node.Instruction == IRInstruction.AddSigned64
				|| node.Instruction == IRInstruction.AddUnsigned64)
				&& (node2.Instruction == IRInstruction.SubSigned32 || node2.Instruction == IRInstruction.SubUnsigned32
				|| node2.Instruction == IRInstruction.SubSigned64 || node2.Instruction == IRInstruction.SubUnsigned64))
			{
				add = false;
			}
			else if ((node.Instruction == IRInstruction.SubSigned32 || node.Instruction == IRInstruction.SubUnsigned32
				|| node.Instruction == IRInstruction.SubSigned64 || node.Instruction == IRInstruction.SubUnsigned64)
				&& (node2.Instruction == IRInstruction.AddSigned32 || node2.Instruction == IRInstruction.AddUnsigned32
				|| node2.Instruction == IRInstruction.AddSigned64 || node2.Instruction == IRInstruction.AddUnsigned64))
			{
				add = false;
			}

			ulong r = add ? node.Operand2.ConstantUnsignedLongInteger + node2.Operand2.ConstantUnsignedLongInteger :
				node.Operand2.ConstantUnsignedLongInteger - node2.Operand2.ConstantUnsignedLongInteger;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			if (trace.Active) trace.Log("*** ConstantFoldingAdditionAndSubstraction");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantFoldingAdditionAndSubstractionCount++;
		}

		private void ConstantFoldingLogicalOr(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LogicalOr32 || node.Instruction == IRInstruction.LogicalOr64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.LogicalOr32 || node2.Instruction == IRInstruction.LogicalOr64))
				return;

			if (!node2.Result.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = node.Operand2.ConstantUnsignedLongInteger | node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ConstantFoldingLogicalOr");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantFoldingLogicalOrCount++;
		}

		private void ConstantFoldingLogicalAnd(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.LogicalAnd32 || node.Instruction == IRInstruction.LogicalAnd64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Result.Uses.Count != 1)
				return;

			var node2 = node.Result.Uses[0];

			if (!(node2.Instruction == IRInstruction.LogicalAnd32 || node2.Instruction == IRInstruction.LogicalAnd64))
				return;

			if (!node2.Result.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = node.Operand2.ConstantUnsignedLongInteger & node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ConstantFoldingLogicalOr");
			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantFoldingLogicalAndCount++;
		}

		private void ConstantFoldingMultiplication(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.MulSigned32
				|| node.Instruction == IRInstruction.MulUnsigned32
				|| node.Instruction == IRInstruction.MulSigned64
				|| node.Instruction == IRInstruction.MulUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
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

			if (!node2.Result.IsVirtualRegister)
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Debug.Assert(node2.Result.Definitions.Count == 1);

			ulong r = node.Operand2.ConstantUnsignedLongInteger * node2.Operand2.ConstantUnsignedLongInteger;

			if (trace.Active) trace.Log("*** ConstantFoldingMultiplication");
			AddOperandUsageToWorkList(node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node2);
			node2.SetInstruction(GetMoveInteger(node2.Result), node2.Result, node.Result);
			if (trace.Active) trace.Log("AFTER: \t" + node2);
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.Operand2 = CreateConstant(node.Operand2.Type, r);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			constantFoldingMultiplicationCount++;
		}

		private void ConstantFoldingDivision(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.DivSigned32
				|| node.Instruction == IRInstruction.DivUnsigned32
				|| node.Instruction == IRInstruction.DivSigned64
				|| node.Instruction == IRInstruction.DivUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
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

			if (!node2.Result.IsVirtualRegister)
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
			constantFoldingDivisionCount++;
		}

		private void CombineIntegerCompareBranch(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.CompareIntegerBranch)
				return;

			if (!(node.ConditionCode == ConditionCode.NotEqual || node.ConditionCode == ConditionCode.Equal))
				return;

			if (!((node.Operand1.IsVirtualRegister && node.Operand2.IsConstantZero)
				|| (node.Operand2.IsVirtualRegister && node.Operand1.IsConstantZero)))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (operand.Definitions.Count != 1)
				return;

			var node2 = operand.Definitions[0];

			if (!(node2.Instruction == IRInstruction.CompareInteger32x32
				|| node2.Instruction == IRInstruction.CompareInteger64x32
				|| node2.Instruction == IRInstruction.CompareInteger64x64))
				return;

			AddOperandUsageToWorkList(node2);
			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** CombineIntegerCompareBranch");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.ConditionCode = node.ConditionCode == ConditionCode.NotEqual ? node2.ConditionCode : node2.ConditionCode.GetOpposite();
			node.Operand1 = node2.Operand1;
			node.Operand2 = node2.Operand2;
			if (trace.Active) trace.Log("AFTER: \t" + node);
			if (trace.Active) trace.Log("REMOVED:\t" + node2);
			node2.SetInstruction(IRInstruction.Nop);
			combineIntegerCompareBranchCount++;
			instructionsRemovedCount++;
		}

		private void FoldIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInteger32x32
				|| node.Instruction == IRInstruction.CompareInteger64x32
				|| node.Instruction == IRInstruction.CompareInteger64x64))
				return;

			if (!(node.ConditionCode == ConditionCode.NotEqual || node.ConditionCode == ConditionCode.Equal))
				return;

			if (!((node.Operand1.IsVirtualRegister && node.Operand2.IsConstantZero)
				|| (node.Operand2.IsVirtualRegister && node.Operand1.IsConstantZero)))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (operand.Definitions.Count != 1)
				return;

			Debug.Assert(operand.Definitions.Count == 1);

			var node2 = operand.Definitions[0];

			if (!(node2.Instruction == IRInstruction.CompareInteger32x32
				|| node2.Instruction == IRInstruction.CompareInteger64x32
				|| node2.Instruction == IRInstruction.CompareInteger64x64))
				return;

			BaseInstruction compareInteger = node2.Instruction;
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

		/// <summary>
		/// Simplifies sign/zero extended move
		/// </summary>
		/// <param name="node">The node.</param>
		private void SimplifyExtendedMove(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.SignExtended8x32
				|| node.Instruction == IRInstruction.SignExtended16x32
				|| node.Instruction == IRInstruction.SignExtended8x64
				|| node.Instruction == IRInstruction.SignExtended16x64
				|| node.Instruction == IRInstruction.SignExtended32x64
				|| node.Instruction == IRInstruction.ZeroExtended8x32
				|| node.Instruction == IRInstruction.ZeroExtended16x32
				|| node.Instruction == IRInstruction.ZeroExtended8x64
				|| node.Instruction == IRInstruction.ZeroExtended16x64
				|| node.Instruction == IRInstruction.ZeroExtended32x64))
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
			if (!(node.Instruction == IRInstruction.LoadInteger32
				|| node.Instruction == IRInstruction.LoadInteger64
				|| node.Instruction == IRInstruction.StoreInteger8
				|| node.Instruction == IRInstruction.StoreInteger16
				|| node.Instruction == IRInstruction.StoreInteger32
				|| node.Instruction == IRInstruction.StoreInteger64
				|| node.Instruction == IRInstruction.StoreFloatR4
				|| node.Instruction == IRInstruction.StoreFloatR8
				|| node.Instruction == IRInstruction.LoadSignExtended8x32
				|| node.Instruction == IRInstruction.LoadSignExtended16x32
				|| node.Instruction == IRInstruction.LoadSignExtended8x64
				|| node.Instruction == IRInstruction.LoadSignExtended16x64
				|| node.Instruction == IRInstruction.LoadSignExtended32x64
				|| node.Instruction == IRInstruction.LoadZeroExtended8x32
				|| node.Instruction == IRInstruction.LoadZeroExtended16x32
				|| node.Instruction == IRInstruction.LoadZeroExtended8x64
				|| node.Instruction == IRInstruction.LoadZeroExtended16x64
				|| node.Instruction == IRInstruction.LoadZeroExtended32x64))
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (node.Operand1.Uses.Count != 1)
				return;

			var node2 = node.Operand1.Definitions[0];

			if (!(node2.Instruction == IRInstruction.AddSigned32
				|| node2.Instruction == IRInstruction.AddSigned64
				|| node2.Instruction == IRInstruction.SubSigned32
				|| node2.Instruction == IRInstruction.SubSigned64
				|| node2.Instruction == IRInstruction.AddUnsigned32
				|| node2.Instruction == IRInstruction.AddUnsigned64
				|| node2.Instruction == IRInstruction.SubUnsigned32
				|| node2.Instruction == IRInstruction.SubUnsigned64))
				return;

			if (!node2.Operand2.IsResolvedConstant)
				return;

			Operand constant;

			if (node2.Instruction == IRInstruction.AddUnsigned32
				|| node2.Instruction == IRInstruction.AddSigned32
				|| node2.Instruction == IRInstruction.AddUnsigned64
				|| node2.Instruction == IRInstruction.AddSigned64)
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

		/// <summary>
		/// Folds the constant phi instruction.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ConstantFoldingPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (node.Result.Definitions.Count != 1)
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

		/// <summary>
		/// Simplifies the phi.
		/// </summary>
		/// <param name="node">The node.</param>
		private void SimplifyPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (node.OperandCount != 1)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Result.IsInteger) // future: should work on other types as well
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

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Result.IsInteger) // future: should work on other types as well
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

		private void SimplifyIntegerCompare(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInteger32x32
				|| node.Instruction == IRInstruction.CompareInteger64x32
				|| node.Instruction == IRInstruction.CompareInteger64x64))
				return;

			if (node.ConditionCode != ConditionCode.NotEqual)
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!((node.Operand1.IsVirtualRegister && node.Operand2.IsConstantZero)
				|| (node.Operand2.IsVirtualRegister && node.Operand1.IsConstantZero)))
				return;

			var operand = (node.Operand2.IsConstantZero) ? node.Operand1 : node.Operand2;

			if (operand.Uses.Count != 1)
				return;

			if (operand.Definitions.Count != 1)
				return;

			if (operand.Is64BitInteger != node.Result.Is64BitInteger)
				return;

			if (trace.Active) trace.Log("*** SimplifyIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(GetMoveInteger(node.Result), node.Result, operand);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyIntegerCompare++;
		}

		private void SimplifyIntegerCompare2(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.CompareInteger32x32
				|| node.Instruction == IRInstruction.CompareInteger64x32
				|| node.Instruction == IRInstruction.CompareInteger64x64))
				return;

			if (node.ConditionCode != ConditionCode.UnsignedGreaterThan)
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (node.Result.Definitions.Count != 1)
				return;

			if (!node.Operand2.IsConstantZero)
				return;

			if (!node.Operand1.IsVirtualRegister)
				return;

			if (trace.Active) trace.Log("*** SimplifyIntegerCompare");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			AddOperandUsageToWorkList(node);

			node.SetInstruction(Select(node.Result, !node.Operand1.Is64BitInteger ? (BaseInstruction)IRInstruction.CompareInteger32x32 : IRInstruction.CompareInteger64x32, IRInstruction.CompareInteger64x64), ConditionCode.NotEqual, node.Result, node.Operand1, node.Operand2);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			simplifyIntegerCompare++;
		}

		private void DeadCodeEliminationPhi(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.Phi)
				return;

			if (node.Result.Definitions.Count != 1)
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
			deadCodeEliminationPhi++;
		}

		/// <summary>
		/// Arithmetics the simplification rem unsigned modulus.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationRemUnsignedModulus(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.RemUnsigned32 || node.Instruction == IRInstruction.RemUnsigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Operand2.ConstantUnsignedLongInteger == 0)
				return;

			if (!IsPowerOfTwo(node.Operand2.ConstantUnsignedLongInteger))
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (op2.ConstantUnsignedLongInteger == 0)
			{
				AddOperandUsageToWorkList(node);
				if (trace.Active) trace.Log("*** ArithmeticSimplificationRemUnsignedModulus");
				if (trace.Active) trace.Log("BEFORE:\t" + node);
				node.SetInstruction(GetMoveInteger(result), result, ConstantZero);
				arithmeticSimplificationModulus++;
				if (trace.Active) trace.Log("AFTER: \t" + node);
				return;
			}

			int power = GetPowerOfTwo(op2.ConstantUnsignedLongInteger);

			var mask = (1 << power) - 1;

			var constant = CreateConstant(mask);

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ArithmeticSimplificationRemUnsignedModulus");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(Select(result, IRInstruction.LogicalAnd32, IRInstruction.LogicalAnd64), result, op1, constant);
			arithmeticSimplificationModulus++;
			if (trace.Active) trace.Log("AFTER: \t" + node);
			return;
		}

		/// <summary>
		/// Arithmetics the simplification rem signed modulus.
		/// </summary>
		/// <param name="node">The node.</param>
		private void ArithmeticSimplificationRemSignedModulus(InstructionNode node)
		{
			if (!(node.Instruction == IRInstruction.RemSigned32 || node.Instruction == IRInstruction.RemSigned64))
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			if (!node.Operand2.IsResolvedConstant)
				return;

			if (node.Operand2.ConstantUnsignedLongInteger != 1)
				return;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			AddOperandUsageToWorkList(node);
			if (trace.Active) trace.Log("*** ArithmeticSimplificationRemSignedModulus");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(GetMoveInteger(result), result, ConstantZero);
			arithmeticSimplificationModulus++;
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

		private void NormalizeConstantTo32Bit(InstructionNode node)
		{
			if (node.ResultCount != 1)
				return;

			if (!node.Result.IsInt)
				return;

			if (node.Instruction == IRInstruction.LogicalAnd32
				|| node.Instruction == IRInstruction.LogicalOr32
				|| node.Instruction == IRInstruction.LogicalXor32
				|| node.Instruction == IRInstruction.LogicalAnd64
				|| node.Instruction == IRInstruction.LogicalOr64
				|| node.Instruction == IRInstruction.LogicalXor64
				|| node.Instruction == IRInstruction.LogicalNot32
				|| node.Instruction == IRInstruction.LogicalNot64)
			{
				if (node.Operand1.IsResolvedConstant && node.Operand1.IsLong)
				{
					if (trace.Active) trace.Log("*** NormalizeConstantTo32Bit");

					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.Operand1 = CreateConstant((int)(node.Operand1.ConstantUnsignedLongInteger & uint.MaxValue));
					if (trace.Active) trace.Log("AFTER: \t" + node);
					AddOperandUsageToWorkList(node);
				}
				if (node.OperandCount >= 2 && node.Operand2.IsResolvedConstant && node.Operand2.IsLong)
				{
					if (trace.Active) trace.Log("*** NormalizeConstantTo32Bit");

					if (trace.Active) trace.Log("BEFORE:\t" + node);
					node.Operand2 = CreateConstant((int)(node.Operand2.ConstantUnsignedLongInteger & uint.MaxValue));
					if (trace.Active) trace.Log("AFTER: \t" + node);
					AddOperandUsageToWorkList(node);
				}
			}
		}

		private void GetLow64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			AddOperandUsageToWorkList(node);

			var low = CreateConstant((uint)(node.Operand1.ConstantUnsignedLongInteger & 0xFFFFFFFF));

			if (trace.Active) trace.Log("*** Split64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInteger32, node.Result, low);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			split64Constant++;
		}

		private void GetHigh64Constant(InstructionNode node)
		{
			if (node.Instruction != IRInstruction.GetLow64)
				return;

			if (!node.Operand1.IsResolvedConstant)
				return;

			if (!node.Result.IsVirtualRegister)
				return;

			AddOperandUsageToWorkList(node);

			var high = CreateConstant((uint)(node.Operand1.ConstantUnsignedLongInteger >> 32) & 0xFFFFFFFF);

			if (trace.Active) trace.Log("*** Split64Constant");
			if (trace.Active) trace.Log("BEFORE:\t" + node);
			node.SetInstruction(IRInstruction.MoveInteger32, node.Result, high);
			if (trace.Active) trace.Log("AFTER: \t" + node);
			split64Constant++;
		}
	}
}
