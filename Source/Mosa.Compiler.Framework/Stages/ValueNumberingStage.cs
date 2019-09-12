// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Framework.Trace;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Stages
{
	/// <summary>
	/// Value Numbering Stage
	/// </summary>
	public sealed class ValueNumberingStage : BaseMethodCompilerStage
	{
		private SimpleFastDominance AnalysisDominance;
		private List<BasicBlock> ReversePostOrder;

		private Dictionary<Operand, Operand> MapToValueNumber;
		private BitArray Processed;

		private TraceLog trace;

		private BitArray ParamReadOnly;

		private Counter InstructionRemovalCount = new Counter("ValueNumberingStage.IRInstructionRemoved");
		private Counter ConstantFoldingAndStrengthReductionCount = new Counter("ValueNumberingStage.ConstantFoldingAndStrengthReduction");
		private Counter SubexpressionEliminationCount = new Counter("ValueNumberingStage.SubexpressionElimination");
		private Counter ParameterLoadEliminationCount = new Counter("ValueNumberingStage.ParameterLoadElimination");
		private Counter DeadCodeEliminationCount = new Counter("ValueNumberingStage.DeadCodeElimination");
		private Counter StrengthReductionAndSimplificationCount = new Counter("ValueNumberingStage.StrengthReductionAndSimplification");

		private class Expression
		{
			public int Hash;
			public BaseInstruction Instruction;
			public Operand Operand1;
			public Operand Operand2;

			public Operand ValueNumber;
		}

		private Dictionary<int, List<Expression>> Expressions;

		protected override void Initialize()
		{
			Register(ConstantFoldingAndStrengthReductionCount);
			Register(InstructionRemovalCount);
			Register(SubexpressionEliminationCount);
			Register(ParameterLoadEliminationCount);
			Register(DeadCodeEliminationCount);
			Register(StrengthReductionAndSimplificationCount);
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

			trace = CreateTraceLog(5);

			MapToValueNumber = new Dictionary<Operand, Operand>(MethodCompiler.VirtualRegisters.Count);
			Expressions = new Dictionary<int, List<Expression>>();
			Processed = new BitArray(BasicBlocks.Count, false);

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
			ParamReadOnly = null;

			trace = null;
		}

		private void DetermineReadOnlyParameters()
		{
			if (MethodCompiler.Parameters.Length == 0)
				return;

			ParamReadOnly = new BitArray(MethodCompiler.Parameters.Length, false);

			var traceParameters = CreateTraceLog("Parameters", 5);

			foreach (var operand in MethodCompiler.Parameters)
			{
				bool write = false;

				foreach (var use in operand.Uses)
				{
					if (use.Instruction.IsParameterStore)
					{
						write = true;
						break;
					}
				}

				if (!write && operand.Low != null)
				{
					foreach (var use in operand.Low.Uses)
					{
						if (use.Instruction.IsParameterStore)
						{
							write = true;
							break;
						}
					}
				}

				if (!write && operand.High != null)
				{
					foreach (var use in operand.High.Uses)
					{
						if (use.Instruction.IsParameterStore)
						{
							write = true;
							break;
						}
					}
				}

				ParamReadOnly[operand.Index] = !write;

				traceParameters?.Log($"{operand}: {(write ? "Writable" : "ReadOnly")}");
			}
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

						for (int i = nextBlocks.Count - 1; i >= 0; i--)
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
			trace?.Log($"Processing Block: {block}");

			//Debug.Assert(!Processed.Get(block.Sequence));

			newExpressions = null;
			nextblocks = null;

			// Mark the beginning of the new scope by keeping a list
			bool successorValidated = false;
			bool successorProcessed = true;

			Processed.Set(block.Sequence, true);

			for (var node = block.AfterFirst; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (node.Instruction == IRInstruction.Phi32 || node.Instruction == IRInstruction.Phi64 || node.Instruction == IRInstruction.PhiR4 || node.Instruction == IRInstruction.PhiR8)
				{
					// Validate all successor are already processed
					// and if not, just set the value number
					if (!successorValidated)
					{
						successorValidated = true;
						foreach (var processed in block.PreviousBlocks)
						{
							if (!Processed.Get(block.Sequence))
							{
								successorProcessed = false;
								break;
							}
						}
					}

					if (successorValidated && !successorProcessed)
					{
						SetValueNumber(node.Result, node.Result);
						continue;
					}

					// check for useless
					if (IsPhiUseless(node))
					{
						var w = GetValueNumber(node.Operand1);
						SetValueNumber(node.Result, w);

						trace?.Log($"Removed Unless PHI: {node}");

						node.SetInstruction(IRInstruction.Nop);
						InstructionRemovalCount++;
						continue;
					}

					// check for redundant
					var redundant = CheckRedundant(node);

					if (redundant != null)
					{
						var w = GetValueNumber(redundant);
						SetValueNumber(node.Result, w);

						trace?.Log($"Removed Redundant PHI: {node}");

						node.SetInstruction(IRInstruction.Nop);
						InstructionRemovalCount++;
						continue;
					}

					SetValueNumber(node.Result, node.Result);

					continue;
				}

				UpdateNodeWithValueNumbers(node);

				var newInstruction = BuiltInOptimizations.DeadCodeElimination(node);
				if (newInstruction != null)
				{
					node.SetInstruction(newInstruction);
					DeadCodeEliminationCount++;
					InstructionRemovalCount++;
					continue;
				}

				newInstruction = BuiltInOptimizations.StrengthReduction(node)
					?? BuiltInOptimizations.Simplification(node);

				if (newInstruction != null)
				{
					node.SetInstruction(newInstruction);
					StrengthReductionAndSimplificationCount++;
				}

				// move constant to right for commutative operations - helpful later
				if (node.Instruction.IsCommutative && node.Operand1.IsResolvedConstant && node.Operand2.IsVirtualRegister)
				{
					var operand1 = node.Operand1;
					node.Operand1 = node.Operand2;
					node.Operand2 = operand1;
				}

				if (node.Instruction == IRInstruction.Move32
					|| node.Instruction == IRInstruction.Move64
					|| node.Instruction == IRInstruction.MoveR4
					|| node.Instruction == IRInstruction.MoveR8)
				{
					if (node.Result.IsCPURegister || node.Operand1.IsCPURegister)
					{
						SetValueNumber(node.Result, node.Result);
						continue;
					}

					// FUTURE: The check for IsSymbol (unresolved constant) should not be necessary
					if (node.Operand1.IsStackLocal || node.Operand1.IsOnStack || node.Operand1.IsSymbol)
					{
						SetValueNumber(node.Result, node.Result);
						continue;
					}

					SetValueNumber(node.Result, node.Operand1);
					node.SetInstruction(IRInstruction.Nop);
					InstructionRemovalCount++;
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

				// Simplify expression: constant folding & strength reduction
				var newOperand = BuiltInOptimizations.ConstantFoldingAndStrengthReductionIntegerToOperand(node);

				if (newOperand != null)
				{
					Debug.Assert(newOperand != node.Result);

					SetValueNumber(node.Result, newOperand);
					node.SetInstruction(IRInstruction.Nop);
					ConstantFoldingAndStrengthReductionCount++;
					InstructionRemovalCount++;
					continue;
				}

				var hash = ComputeExpressionHash(node);
				var list = GetExpressionsByHash(hash);
				var match = FindMatch(list, node);

				if (match != null)
				{
					var w = GetValueNumber(match.ValueNumber);

					trace?.Log($"Found Expression Match: {node}");

					SetValueNumber(node.Result, w);

					if (node.Instruction.IsParameterLoad)
						ParameterLoadEliminationCount++;

					node.SetInstruction(IRInstruction.Nop);
					InstructionRemovalCount++;
					SubexpressionEliminationCount++;
					continue;
				}
				else
				{
					trace?.Log($"No Expression Found: {node}");
				}

				var newExpression = new Expression()
				{
					Hash = hash,
					Instruction = node.Instruction,
					Operand1 = node.Operand1,
					Operand2 = node.Operand2,
					ValueNumber = node.Result
				};

				AddExpressionToHashTable(newExpression);

				Debug.Assert(FindMatch(GetExpressionsByHash(ComputeExpressionHash(node)), node) == newExpression);

				(newExpressions ?? (newExpressions = new List<Expression>())).Add(newExpression);

				if (node.Instruction.IsCommutative && node.Operand1 != node.Operand2)
				{
					var newExpression2 = new Expression()
					{
						Hash = hash,
						Instruction = node.Instruction,
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

			var children = AnalysisDominance.GetChildren(block);
			if (children != null || children.Count == 0)
			{
				nextblocks = new List<BasicBlock>(children.Capacity);

				if (children.Count == 1)
				{
					// Efficient!
					nextblocks.Add(children[0]);

					//trace?.Log("Queue Block:" + children[0]);
				}
				else if (ReversePostOrder.Count < 32)
				{
					// Efficient for small sets
					foreach (var child in ReversePostOrder)
					{
						if (children.Contains(child))
						{
							nextblocks.Add(child);

							//trace?.Log("Queue Block:" + child);
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

							//trace?.Log("Queue Block:" + child);
						}
					}
				}
			}
		}

		private bool CanAssignValueNumberToExpression(InstructionNode node)
		{
			if (node.Instruction.IsParameterLoad
				&& node.Instruction != IRInstruction.LoadParamCompound
				&& ParamReadOnly.Get(node.Operand1.Index))
			{
				return true;
			}

			if (node.Instruction == IRInstruction.AddressOf
				&& (node.Operand1.IsStackLocal || node.Operand1.IsStaticField))
			{
				return true;
			}

			if (node.ResultCount != 1
				|| node.OperandCount == 0
				|| node.OperandCount > 2
				|| node.Instruction.IsMemoryWrite
				|| node.Instruction.IsMemoryRead
				|| node.Instruction.IsIOOperation
				|| node.Instruction.HasUnspecifiedSideEffect
				|| node.Instruction.VariableOperands
				|| node.Instruction.FlowControl != FlowControl.Next
				|| node.Instruction.IgnoreDuringCodeGeneration
				|| node.Operand1.IsUnresolvedConstant
				|| (node.OperandCount == 2 && node.Operand2.IsUnresolvedConstant))
				return false;

			return true;
		}

		private static int UpdateHash(int hash, int addition)
		{
			return (hash << 8) + addition;
		}

		private static int ComputeExpressionHash(InstructionNode node)
		{
			int hash = node.Instruction.ID;

			if (node.Operand1.IsConstant)
				hash = UpdateHash(hash, (int)node.Operand1.ConstantUnsigned64);
			else if (node.Operand1.IsVirtualRegister || node.Operand1.IsStackLocal)
				hash = UpdateHash(hash, node.Operand1.Index);

			if (node.OperandCount >= 2)
				if (node.Operand2.IsConstant)
					hash = UpdateHash(hash, (int)node.Operand2.ConstantUnsigned64);
				else if (node.Operand2.IsVirtualRegister || node.Operand2.IsStackLocal)
					hash = UpdateHash(hash, node.Operand2.Index);

			return hash;
		}

		private List<Expression> GetExpressionsByHash(int hash)
		{
			Expressions.TryGetValue(hash, out List<Expression> list);
			return list;
		}

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
				&& operand1.IsR
				&& operand2.IsR
				&& operand1.ConstantDouble == operand2.ConstantDouble)
				return true;

			if (instruction != null
				&& instruction == IRInstruction.AddressOf
				&& operand1.IsStaticField
				&& operand2.IsStaticField
				&& operand1.Field == operand2.Field)
				return true;

			if (instruction != null
				&& instruction == IRInstruction.AddressOf
				&& operand1.IsStackLocal
				&& operand2.IsStackLocal
				&& operand1.Index == operand2.Index)
				return true;

			return false;
		}

		private static Expression FindMatch(List<Expression> expressions, InstructionNode node)
		{
			if (expressions == null)
				return null;

			foreach (var expression in expressions)
			{
				if (node.Instruction == expression.Instruction
					&& IsEqual(node.Operand1, expression.Operand1, node.Instruction)
					&& (node.OperandCount == 1 || (node.OperandCount == 2 && IsEqual(node.Operand2, expression.Operand2))))
				{
					return expression;
				}
			}

			return null;
		}

		private Operand GetValueNumber(Operand operand)
		{
			if (MapToValueNumber.TryGetValue(operand, out Operand value))
				return value;
			else
				return null;
		}

		private void SetValueNumber(Operand operand, Operand valueVumber)
		{
			trace?.Log($"Set: {operand} => {valueVumber}");

			MapToValueNumber[operand] = valueVumber;
		}

		private bool IsPhiUseless(InstructionNode node)
		{
			Debug.Assert(node.Instruction == IRInstruction.Phi32 || node.Instruction == IRInstruction.Phi64 || node.Instruction == IRInstruction.PhiR4 || node.Instruction == IRInstruction.PhiR8);

			var operand = node.Operand1;
			var operandVN = GetValueNumber(operand);

			foreach (var op in node.Operands)
			{
				if (operandVN == GetValueNumber(op))
					return false;
			}

			return true;
		}

		private bool ArePhisRedundant(InstructionNode a, InstructionNode b)
		{
			Debug.Assert(a.Instruction == IRInstruction.Phi32 || a.Instruction == IRInstruction.Phi64 || a.Instruction == IRInstruction.PhiR4 || a.Instruction == IRInstruction.PhiR8);
			Debug.Assert(b.Instruction == IRInstruction.Phi32 || b.Instruction == IRInstruction.Phi64 || b.Instruction == IRInstruction.PhiR4 || b.Instruction == IRInstruction.PhiR8);

			if (a.OperandCount != b.OperandCount)
				return false;

			for (int i = 0; i < a.OperandCount; i++)
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

			trace?.Log($"Added Expression: {expression.ValueNumber} <= {expression.Instruction} {expression.Operand1} {expression.Operand2}" ?? string.Empty);
		}

		private void RemoveExpressionFromHashTable(Expression expression)
		{
			var list = Expressions[expression.Hash];

			list.Remove(expression);

			trace?.Log($"Removed Expression: {expression.ValueNumber} <= {expression.Instruction} {expression.Operand1} {expression.Operand2}" ?? string.Empty);
		}

		private void UpdateNodeWithValueNumbers(InstructionNode node)
		{
			// update operands with their value number
			for (int i = 0; i < node.OperandCount; i++)
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
							node.SetOperand(i, valueNumber);

							trace?.Log($"UPDATED: {node}");
						}
					}
					else
					{
						if (node.Instruction == IRInstruction.Phi32 || node.Instruction == IRInstruction.Phi64 || node.Instruction == IRInstruction.PhiR4 || node.Instruction == IRInstruction.PhiR8)
							continue;

						Debug.Assert(node.Instruction == IRInstruction.Phi32 || node.Instruction == IRInstruction.Phi64 || node.Instruction == IRInstruction.PhiR4 || node.Instruction == IRInstruction.PhiR8);

						//throw new CompilerException("ValueNumbering Stage: Expected PHI instruction but found instead: " + node + " for " + operand);
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

					if (node.Instruction != IRInstruction.Phi32 && node.Instruction != IRInstruction.Phi64 && node.Instruction != IRInstruction.PhiR4 && node.Instruction != IRInstruction.PhiR8)
						break;

					// update operands with their value number
					UpdateNodeWithValueNumbers(node);
				}
			}
		}

		private Operand CheckRedundant(InstructionNode node)
		{
			Operand redundant = null;

			for (var previousPhi = node.Previous; !previousPhi.IsBlockStartInstruction; previousPhi = previousPhi.Previous)
			{
				if (previousPhi.IsEmptyOrNop)
					continue;

				Debug.Assert(previousPhi.Instruction == IRInstruction.Phi32 || previousPhi.Instruction == IRInstruction.Phi64 || previousPhi.Instruction == IRInstruction.PhiR4 || previousPhi.Instruction == IRInstruction.PhiR8);

				if (ArePhisRedundant(node, previousPhi))
				{
					redundant = previousPhi.Result;
					break;
				}
			}

			return redundant;
		}
	}
}
