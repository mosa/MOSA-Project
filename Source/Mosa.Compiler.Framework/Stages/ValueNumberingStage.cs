// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Common.Exceptions;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Common;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Value Numbering Stage
/// </summary>
public sealed class ValueNumberingStage : BaseMethodCompilerStage
{
	private SimpleFastDominance AnalysisDominance;
	private List<BasicBlock> ReversePostOrder;

	private Dictionary<Operand, Operand> MapToValueNumber;
	private BlockBitSet Processed;

	private TraceLog Trace;

	private ParameterReadOnlyAnalysis ParameterAnalysis;

	private readonly Counter InstructionRemovalCount = new("ValueNumbering.IRInstructionsRemoved");
	private readonly Counter SubexpressionEliminationCount = new("ValueNumbering.SubexpressionElimination");
	private readonly Counter ParameterLoadEliminationCount = new("ValueNumbering.ParameterLoadElimination");

	private class Expression
	{
		public int Hash;
		public BaseInstruction Instruction;
		public Operand Operand1;
		public Operand Operand2;
		public ConditionCode ConditionCode;
		public Operand ValueNumber;
	}

	private Dictionary<int, List<Expression>> Expressions;

	protected override void Initialize()
	{
		Register(InstructionRemovalCount);
		Register(SubexpressionEliminationCount);
		Register(ParameterLoadEliminationCount);
	}

	protected override void Run()
	{
		if (HasProtectedRegions)
			return;

		// Method is empty - must be a plugged method
		if (BasicBlocks.HeadBlocks.Count != 1)
			return;

		if (BasicBlocks.PrologueBlock == null)
			return;

		var trace = CreateTraceLog(5);

		Transform.SetLog(trace);

		Transform.TraceInstructions();

		Trace = CreateTraceLog("Log", 5);

		MapToValueNumber = new Dictionary<Operand, Operand>(MethodCompiler.VirtualRegisters.Count);
		Expressions = new Dictionary<int, List<Expression>>();
		Processed = new BlockBitSet(BasicBlocks);

		AnalysisDominance = new SimpleFastDominance(BasicBlocks, BasicBlocks.PrologueBlock);

		ReversePostOrder = AnalysisDominance.GetReversePostOrder();

		DetermineReadOnlyParameters();

		ValueNumber();
	}

	protected override void Finish()
	{
		MapToValueNumber = null;
		Expressions = null;
		Processed = null;

		AnalysisDominance = null;
		ReversePostOrder = null;
		ParameterAnalysis = null;

		Trace = null;
	}

	private void DetermineReadOnlyParameters()
	{
		if (MethodCompiler.Parameters.Count == 0)
			return;

		ParameterAnalysis = new ParameterReadOnlyAnalysis(MethodCompiler.Parameters);

		var traceParameters = CreateTraceLog("Parameters", 5);

		ParameterAnalysis.Trace(traceParameters);
	}

	private void ValueNumber()
	{
		var blockWorklist = new Stack<BasicBlock>();
		var expressionWorklist = new Stack<List<Expression>>();

		blockWorklist.Push(BasicBlocks.PrologueBlock);
		expressionWorklist.Push(null);

		while (blockWorklist.Count != 0)
		{
			var block = blockWorklist.Pop();

			if (block != null)
			{
				ValueNumber(block, out List<BasicBlock> nextBlocks, out List<Expression> newExpressions);

				if (nextBlocks != null)
				{
					if (newExpressions != null)
					{
						blockWorklist.Push(null);

						expressionWorklist.Push(newExpressions);
					}

					for (var i = nextBlocks.Count - 1; i >= 0; i--)
					{
						blockWorklist.Push(nextBlocks[i]);
					}
				}
			}
			else
			{
				var expressions = expressionWorklist.Pop();

				if (expressions != null)
				{
					// Cleanup scope by removing new expressions
					foreach (var expression in expressions)
					{
						RemoveExpressionFromHashTable(expression);
					}
				}
			}
		}
	}

	private void ValueNumber(BasicBlock block, out List<BasicBlock> nextblocks, out List<Expression> newExpressions)
	{
		Trace?.Log($"Processing Block: {block}");

		//Debug.Assert(!Processed.Get(block.Sequence));

		newExpressions = null;
		nextblocks = null;

		// Mark the beginning of the new scope by keeping a list
		var successorValidated = false;
		var successorProcessed = true;

		Processed.Add(block);

		for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmptyOrNop)
				continue;

			if (node.Instruction.IsPhi && ProcessPhiInstruction(node, ref successorValidated, ref successorProcessed))
			{
				continue;
			}

			UpdateNodeWithValueNumbers(node);

			// move constant to right for commutative operations - helpful later
			//if (node.Instruction.IsCommutative && node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
			//{
			//	var operand1 = node.Operand1;
			//	node.Operand1 = node.Operand2;
			//	node.Operand2 = operand1;
			//}

			if (node.Instruction.IsMove && node.Instruction.IsIRInstruction)
			{
				if (node.Result.IsPhysicalRegister || node.Operand1.IsPhysicalRegister)
				{
					SetValueNumber(node.Result, node.Result);
					continue;
				}

				if (node.Operand1.IsLocalStack || node.Operand1.IsOnStack || node.Operand1.IsLabel)
				{
					SetValueNumber(node.Result, node.Result);
					continue;
				}

				SetValueNumber(node.Result, node.Operand1);

				Transform.TraceBefore(node, "MoveElimination");
				node.SetNop();
				Transform.TraceAfter(node);
				InstructionRemovalCount.Increment();

				continue;
			}

			if (!CanAssignValueNumberToExpression(node))
			{
				if (node.ResultCount == 1)
				{
					SetValueNumber(node.Result, node.Result);
				}
				else if (node.ResultCount == 2)
				{
					SetValueNumber(node.Result, node.Result);
					SetValueNumber(node.Result2, node.Result2);
				}

				continue;
			}

			var hash = ComputeExpressionHash(node);
			var list = GetExpressionsByHash(hash);
			var match = FindMatch(list, node);

			if (match != null)
			{
				var w = GetValueNumber(match.ValueNumber);

				Trace?.Log($"Found Expression Match: {node}");

				SetValueNumber(node.Result, w);

				Transform.TraceBefore(node, "SubexpressionElimination");
				node.SetNop();
				Transform.TraceAfter(node);

				InstructionRemovalCount.Increment();
				SubexpressionEliminationCount.Increment();

				if (node.Instruction.IsParameterLoad)
					ParameterLoadEliminationCount.Increment();

				continue;
			}

			Trace?.Log($"No Expression Found: {node}");

			var newExpression = new Expression
			{
				Hash = hash,
				Instruction = node.Instruction,
				ConditionCode = node.ConditionCode,
				Operand1 = node.Operand1,
				Operand2 = node.Operand2,
				ValueNumber = node.Result
			};

			AddExpressionToHashTable(newExpression);

			Debug.Assert(FindMatch(GetExpressionsByHash(ComputeExpressionHash(node)), node) == newExpression);

			(newExpressions ?? (newExpressions = new List<Expression>())).Add(newExpression);

			if (node.Instruction.IsCommutative && node.Operand1 != node.Operand2)
			{
				var newExpression2 = new Expression
				{
					Hash = hash,
					Instruction = node.Instruction,
					ConditionCode = node.ConditionCode,
					Operand1 = node.Operand2,
					Operand2 = node.Operand1,
					ValueNumber = node.Result
				};

				AddExpressionToHashTable(newExpression2);
				newExpressions.Add(newExpression2);
			}
			else if (node.Instruction.IsCompare
				&& node.Instruction.IsIRInstruction
				&& node.Operand1 != node.Operand2
				&& node.ConditionCode != ConditionCode.Equal
				&& node.ConditionCode != ConditionCode.NotEqual)
			{
				var newExpression2 = new Expression
				{
					Hash = hash,
					Instruction = node.Instruction,
					ConditionCode = node.ConditionCode.GetReverse(),
					Operand1 = node.Operand2,
					Operand2 = node.Operand1,
					ValueNumber = node.Result
				};

				AddExpressionToHashTable(newExpression2);
				newExpressions.Add(newExpression2);
			}

			SetValueNumber(node.Result, node.Result);
		}

		// For each successor of the block, adjust the φ-function inputs
		UpdatePhiSuccesors(block);

		// Determine next blocks
		var children = AnalysisDominance.GetChildren(block);
		if (children != null || children.Count != 0)
		{
			nextblocks = GetOrderedChildBlocks(children);
		}
	}

	private List<BasicBlock> GetOrderedChildBlocks(List<BasicBlock> children)
	{
		var nextblocks = new List<BasicBlock>(children.Capacity);

		if (children.Count == 1)
		{
			nextblocks.Add(children[0]);
		}
		else if (ReversePostOrder.Count < 32)
		{
			// Efficient for small sets
			foreach (var child in ReversePostOrder)
			{
				if (children.Contains(child))
				{
					nextblocks.Add(child);
				}
			}
		}
		else
		{
			// Scalable for large sets
			var bitArray = new BitArray(BasicBlocks.Count, false);

			foreach (var child in children)
			{
				bitArray.Set(child.Sequence, true);
			}

			foreach (var child in ReversePostOrder)
			{
				if (bitArray.Get(child.Sequence))
				{
					nextblocks.Add(child);
				}
			}
		}

		return nextblocks;
	}

	private bool CanAssignValueNumberToExpression(Node node)
	{
		if (node.Instruction.IsParameterLoad
			&& node.Instruction != IR.LoadParamCompound
			&& ParameterAnalysis.IsReadOnly(node.Operand1.Index) == true)
		{
			return true;
		}

		if (node.Instruction == IR.AddressOf
			&& (node.Operand1.IsLocalStack || node.Operand1.IsStaticField))
		{
			return true;
		}

		if (node.ResultCount != 1
			|| node.OperandCount is 0 or > 2
			|| node.Instruction.IsMemoryWrite
			|| node.Instruction.IsMemoryRead
			|| node.Instruction.IsIOOperation
			|| node.Instruction.HasUnspecifiedSideEffect
			|| node.Instruction.HasVariableOperands
			|| !node.Instruction.IsFlowNext
			|| node.Instruction.IgnoreDuringCodeGeneration
			|| node.Operand1.IsUnresolvedConstant
			|| (node.OperandCount == 2 && node.Operand2.IsUnresolvedConstant))
			return false;

		return true;
	}

	private static int ComputeExpressionHash(Node node)
	{
		var hash = new StableHashCode();

		hash.Add(node.Instruction.ID);
		hash.Add((int)node.ConditionCode);
		hash.Add((int)node.Operand1.Primitive);

		if (node.Operand1.IsConstant)
		{
			hash.Add(node.Operand1.ConstantUnsigned64);
		}
		else if (node.Operand1.IsVirtualRegister || node.Operand1.IsLocalStack)
		{
			hash.Add(node.Operand1.Index);
		}

		if (node.OperandCount >= 2)
		{
			hash.Add((int)node.Operand2.Primitive);

			if (node.Operand2.IsConstant)
			{
				hash.Add(node.Operand2.ConstantUnsigned64);
			}
			else if (node.Operand2.IsVirtualRegister || node.Operand2.IsLocalStack)
			{
				hash.Add(node.Operand2.Index);
			}
		}

		return hash.ToHashCode();
	}

	private List<Expression> GetExpressionsByHash(int hash) => Expressions.GetValueOrDefault(hash);

	private static bool IsEqual(Operand operand1, Operand operand2, BaseInstruction instruction = null)
	{
		if (operand1 == operand2)
			return true;

		if (operand1.IsResolvedConstant
			&& operand2.IsResolvedConstant
			&& operand1.IsInteger
			&& operand2.IsInteger
			&& operand1.ConstantUnsigned64 == operand2.ConstantUnsigned64)
			return true;

		if (operand1.IsResolvedConstant
			&& operand2.IsResolvedConstant
			&& operand1.IsR4
			&& operand2.IsR4
			&& operand1.ConstantFloat == operand2.ConstantFloat)
			return true;

		if (operand1.IsResolvedConstant
			&& operand2.IsResolvedConstant
			&& operand1.IsR8
			&& operand2.IsR8
			&& operand1.ConstantDouble == operand2.ConstantDouble)
			return true;

		if (instruction != null
			&& instruction == IR.AddressOf
			&& operand1.IsStaticField
			&& operand2.IsStaticField
			&& operand1.Field == operand2.Field)
			return true;

		if (instruction != null
			&& instruction == IR.AddressOf
			&& operand1.IsLocalStack
			&& operand2.IsLocalStack
			&& operand1.Index == operand2.Index)
			return true;

		return false;
	}

	private static Expression FindMatch(List<Expression> expressions, Node node)
	{
		if (expressions == null)
			return null;

		foreach (var expression in expressions)
		{
			if (node.Instruction == expression.Instruction
				&& node.ConditionCode == expression.ConditionCode
				&& IsEqual(node.Operand1, expression.Operand1, node.Instruction)
				&& (node.OperandCount == 1 || (node.OperandCount == 2 && IsEqual(node.Operand2, expression.Operand2))))
			{
				return expression;
			}
		}

		return null;
	}

	private Operand GetValueNumber(Operand operand) => MapToValueNumber.GetValueOrDefault(operand);

	private void SetValueNumber(Operand operand, Operand valueNumber)
	{
		Trace?.Log($"Set: {operand} => {valueNumber}");

		MapToValueNumber[operand] = valueNumber;
	}

	private bool IsPhiUseless(Node node)
	{
		Debug.Assert(node.Instruction.IsPhi);

		var operand = node.Operand1;
		var operandVN = GetValueNumber(operand);

		if (operandVN == null)
			return false;

		foreach (var op in node.Operands)
		{
			var vn = GetValueNumber(op);

			if (vn == null || operandVN != vn)
				return false;
		}

		return true;
	}

	private bool ArePhisRedundant(Node a, Node b)
	{
		Debug.Assert(a.Instruction.IsPhi);
		Debug.Assert(b.Instruction.IsPhi);

		if (a.OperandCount != b.OperandCount)
			return false;

		for (var i = 0; i < a.OperandCount; i++)
		{
			var va = a.GetOperand(i);
			var vb = b.GetOperand(i);

			if (va == null || vb == null || va != vb)
				return false;
		}

		return true;
	}

	private void AddExpressionToHashTable(Expression expression)
	{
		if (!Expressions.TryGetValue(expression.Hash, out List<Expression> list))
		{
			list = new List<Expression>();
			Expressions.Add(expression.Hash, list);
		}

		list.Add(expression);

		Trace?.Log($"Added Expression: {expression.ValueNumber} <= {expression.Instruction} {expression.Operand1} {expression.Operand2}");
	}

	private void RemoveExpressionFromHashTable(Expression expression)
	{
		var list = Expressions[expression.Hash];

		list.Remove(expression);

		Trace?.Log($"Removed Expression: {expression.ValueNumber} <= {expression.Instruction} {expression.Operand1} {expression.Operand2}" ?? string.Empty);
	}

	private void UpdateNodeWithValueNumbers(Node node)
	{
		// update operands with their value number
		for (var i = 0; i < node.OperandCount; i++)
		{
			var operand = node.GetOperand(i);

			if (operand.IsVirtualRegister)
			{
				var valueNumber = GetValueNumber(operand);

				if (valueNumber != null)
				{
					if (operand != valueNumber)
					{
						//trace?.Log($"BEFORE: {node}");
						//trace?.Log($"Replaced: {operand} with {valueNumber}");

						Transform.TraceBefore(node, "ValueNumber");
						node.SetOperand(i, valueNumber);
						Transform.TraceAfter(node);

						Trace?.Log($"UPDATED: {node}");
					}
				}
				else
				{
					// value has not been encountered yet --- skip it for now
					if (node.Instruction.IsPhi)
						continue;

					//Debug.Assert(node.Instruction.IsPhiInstruction);

					//MethodCompiler.Compiler.Stop();
					//return;
					throw new CompilerException("ValueNumbering Stage: Expected PHI instruction but found instead: " + node + " for " + operand);
				}
			}
		}
	}

	private void UpdatePhiSuccesors(BasicBlock block)
	{
		// For each successor of the block, adjust the φ-function inputs

		foreach (var next in block.NextBlocks)
		{
			for (var node = next.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmptyOrNop)
					continue;

				if (!node.Instruction.IsPhi)
					break;

				// update operands with their value number
				UpdateNodeWithValueNumbers(node);
			}
		}
	}

	private Operand CheckRedundant(Node node)
	{
		for (var previousPhi = node.Previous; !previousPhi.IsBlockStartInstruction; previousPhi = previousPhi.Previous)
		{
			if (previousPhi.IsEmptyOrNop)
				continue;

			Debug.Assert(previousPhi.Instruction.IsPhi);

			if (ArePhisRedundant(node, previousPhi))
			{
				return previousPhi.Result;
			}
		}

		return null;
	}

	private bool ProcessPhiInstruction(Node node, ref bool successorValidated, ref bool successorProcessed)
	{
		// Validate all successor are already processed
		// and if not, just set the value number
		if (!successorValidated)
		{
			successorValidated = true;
			foreach (var processed in node.Block.PreviousBlocks)
			{
				if (!Processed.Contains(processed))
				{
					successorProcessed = false;
					break;
				}
			}
		}

		if (successorValidated && !successorProcessed)
		{
			SetValueNumber(node.Result, node.Result);
			return true;
		}

		// check for useless
		if (IsPhiUseless(node))
		{
			var w = GetValueNumber(node.Operand1);
			SetValueNumber(node.Result, w);

			Trace?.Log($"Removed Unless PHI: {node}");

			Transform.TraceBefore(node, "UselessPhiElimination");
			node.SetNop();
			Transform.TraceAfter(node);
			InstructionRemovalCount.Increment();

			return true;
		}

		// check for redundant
		var redundant = CheckRedundant(node);

		if (redundant != null)
		{
			var w = GetValueNumber(redundant);
			SetValueNumber(node.Result, w);

			Trace?.Log($"Removed Redundant PHI: {node}");

			Transform.TraceBefore(node, "RedundantPhiElimination");
			node.SetNop();
			Transform.TraceAfter(node);
			InstructionRemovalCount.Increment();

			return true;
		}

		SetValueNumber(node.Result, node.Result);

		return true;
	}
}
