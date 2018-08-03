// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common.Exceptions;
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
	public class ValueNumberingStage : BaseMethodCompilerStage
	{
		private SimpleFastDominance AnalysisDominance;
		private List<BasicBlock> ReversePostOrder;

		private Dictionary<Operand, Operand> MapToValueNumber;
		private BitArray Processed;
		private TraceLog trace;

		private int instructionRemovalCount;
		private int constantFoldingIntegerOperationCount;
		private int constantFoldingIntegerCompareCount;

		private class Expression
		{
			public int Hash;
			public BaseInstruction Instruction;
			public Operand Operand1;
			public Operand Operand2;

			public Operand ValueNumber;
		}

		private Dictionary<int, List<Expression>> Expressions;

		protected override void Setup()
		{
			base.Setup();

			trace = CreateTraceLog();

			constantFoldingIntegerOperationCount = 0;
			instructionRemovalCount = 0;
			constantFoldingIntegerCompareCount = 0;
		}

		protected override void Run()
		{
			if (true)
				return;

			if (HasProtectedRegions)
				return;

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count != 1)
				return;

			if (BasicBlocks.PrologueBlock == null)
				return;

			trace = CreateTraceLog("ValueNumbering");

			MapToValueNumber = new Dictionary<Operand, Operand>(MethodCompiler.VirtualRegisters.Count);
			Expressions = new Dictionary<int, List<Expression>>();
			Processed = new BitArray(BasicBlocks.Count, false);

			AnalysisDominance = new SimpleFastDominance(BasicBlocks, BasicBlocks.PrologueBlock);
			ReversePostOrder = AnalysisDominance.GetReversePostOrder();

			ValueNumber();
		}

		protected override void Finish()
		{
			base.Finish();

			UpdateCounter("ValueNumbering.IRInstructionRemoved", instructionRemovalCount);
			UpdateCounter("ValueNumbering.ConstantFoldingIntegerOperation", constantFoldingIntegerOperationCount);
			UpdateCounter("ValueNumbering.ConstantFoldingIntegerCompare", constantFoldingIntegerCompareCount);

			MapToValueNumber = null;
			Expressions = null;
			Processed = null;

			AnalysisDominance = null;
			ReversePostOrder = null;

			trace = null;
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
						for (int i = nextBlocks.Count - 1; i >= 0; i--)
						{
							blockWorklist.Push(nextBlocks[i]);
						}

						if (newExpressions != null)
						{
							blockWorklist.Push(null);

							expressionWorklist.Push(newExpressions);
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
			if (trace.Active) trace.Log("Processing Block: " + block);

			Debug.Assert(!Processed.Get(block.Sequence));

			newExpressions = null;
			nextblocks = null;

			// Mark the beginning of the new scope by keeping a list
			bool successorValidated = false;
			bool successorProcessed = true;

			Processed.Set(block.Sequence, true);

			for (var node = block.First; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				if (node.Instruction == IRInstruction.Phi)
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

						if (trace.Active) trace.Log("Removed Unless PHI: " + node);

						node.SetInstruction(IRInstruction.Nop);
						instructionRemovalCount++;
						continue;
					}

					// check for redundant
					var redundant = CheckRedundant(ref node);

					if (redundant != null)
					{
						var w = GetValueNumber(redundant);
						SetValueNumber(node.Result, w);

						if (trace.Active) trace.Log("Removed Redundant PHI: " + node);

						node.SetInstruction(IRInstruction.Nop);
						instructionRemovalCount++;
						continue;
					}

					SetValueNumber(node.Result, node.Result);

					continue;
				}

				UpdateNodeWithValueNumbers(node);

				if (node.Instruction == IRInstruction.MoveInt32
					|| node.Instruction == IRInstruction.MoveInt64
					|| node.Instruction == IRInstruction.MoveFloatR4
					|| node.Instruction == IRInstruction.MoveFloatR8)
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
					instructionRemovalCount++;
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

				// FUTURE - simplify expression: constant folding & strength reduction
				//var newOperand = ConstantFoldingIntegerOperations(node)
				//		?? ConstantFoldingIntegerCompare(node);

				//if (newOperand != null)
				//{
				//	SetValueNumber(node.Result, newOperand);
				//	node.SetInstruction(IRInstruction.Nop);
				//	instructionRemovalCount++;
				//	constantFoldingIntegerOperationCount++;
				//	continue;
				//}

				var hash = ComputeExpressionHash(node);
				var list = GetExpressionsByHash(hash);
				var match = FindMatch(list, node);

				if (match != null)
				{
					var w = GetValueNumber(match.ValueNumber);

					if (trace.Active) trace.Log("Found Expression Match: " + node);

					SetValueNumber(node.Result, w);

					node.SetInstruction(IRInstruction.Nop);
					instructionRemovalCount++;
					continue;
				}

				var newExpression = new Expression()
				{
					Hash = hash,
					Instruction = node.Instruction,
					Operand1 = node.Operand1,
					Operand2 = node.Operand2,
					ValueNumber = node.Result
				};

				AddExpressiontoHashTable(newExpression);

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

					AddExpressiontoHashTable(newExpression2);
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

					if (trace.Active) trace.Log("Queue Block:" + children[0]);
				}
				else if (ReversePostOrder.Count < 32)
				{
					// Efficient for small sets
					foreach (var child in ReversePostOrder)
					{
						if (children.Contains(child))
						{
							nextblocks.Add(child);

							if (trace.Active) trace.Log("Queue Block:" + child);
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

							if (trace.Active) trace.Log("Queue Block:" + child);
						}
					}
				}
			}
		}

		private static bool CanAssignValueNumberToExpression(InstructionNode node)
		{
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
				hash = UpdateHash(hash, (int)node.Operand1.ConstantUnsignedLongInteger);
			else if (node.Operand1.IsVirtualRegister)
				hash = UpdateHash(hash, (int)node.Operand1.Index);

			if (node.OperandCount >= 2)
				if (node.Operand2.IsConstant)
					hash = UpdateHash(hash, (int)node.Operand2.ConstantUnsignedLongInteger);
				else if (node.Operand2.IsVirtualRegister)
					hash = UpdateHash(hash, (int)node.Operand2.Index);

			return hash;
		}

		private List<Expression> GetExpressionsByHash(int hash)
		{
			Expressions.TryGetValue(hash, out List<Expression> list);
			return list;
		}

		private static Expression FindMatch(List<Expression> expressions, InstructionNode node)
		{
			if (expressions == null)
				return null;

			foreach (var expression in expressions)
			{
				if (node.Instruction == expression.Instruction
					&& node.Operand1 == expression.Operand1
					&& (node.OperandCount == 1 || (node.OperandCount == 2 && node.Operand2 == expression.Operand2)))
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
			//if (trace.Active) trace.Log("Set: " + operand + " => " + valueVumber);

			MapToValueNumber[operand] = valueVumber;
		}

		private bool IsPhiUseless(InstructionNode node)
		{
			Debug.Assert(node.Instruction == IRInstruction.Phi);

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
			Debug.Assert(a.Instruction == IRInstruction.Phi);
			Debug.Assert(b.Instruction == IRInstruction.Phi);

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

		private void AddExpressiontoHashTable(Expression expression)
		{
			if (!Expressions.TryGetValue(expression.Hash, out List<Expression> list))
			{
				list = new List<Expression>();
				Expressions.Add(expression.Hash, list);
			}

			list.Add(expression);

			//if (trace.Active) trace.Log("Added Expression: " + expression.ValueNumber + " <= " + expression.Instruction + " " + expression.Operand1 + " " + expression.Operand2 ?? string.Empty);
		}

		private void RemoveExpressionFromHashTable(Expression expression)
		{
			var list = Expressions[expression.Hash];

			list.Remove(expression);

			//if (trace.Active) trace.Log("Removed Expression: " + expression.ValueNumber + " <= " + expression.Instruction + " " + expression.Operand1 + " " + expression.Operand2 ?? string.Empty);
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
							//if (trace.Active) trace.Log("BEFORE: " + node);
							//if (trace.Active) trace.Log("Replaced: " + operand + " with " + valueNumber);
							node.SetOperand(i, valueNumber);

							//if (trace.Active) trace.Log("AFTER: " + node);
						}
					}
					else
					{
						if (node.Instruction == IRInstruction.Phi)
							continue;

						Debug.Assert(node.Instruction == IRInstruction.Phi);

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
				for (var node = next.First.Next; !node.IsBlockEndInstruction; node = node.Next)
				{
					if (node.IsEmpty)
						continue;

					if (node.Instruction == IRInstruction.Nop)
						continue;

					if (node.Instruction != IRInstruction.Phi)
						break;

					// update operands with their value number
					UpdateNodeWithValueNumbers(node);
				}
			}
		}

		private Operand CheckRedundant(ref InstructionNode node)
		{
			Operand redundant = null;

			for (var previousPhi = node.Previous; !previousPhi.IsBlockStartInstruction; previousPhi = previousPhi.Previous)
			{
				if (previousPhi.IsEmpty)
					continue;

				if (previousPhi.Instruction == IRInstruction.Nop)
					continue;

				Debug.Assert(previousPhi.Instruction == IRInstruction.Phi);

				if (ArePhisRedundant(node, previousPhi))
				{
					redundant = previousPhi.Result;
					break;
				}
			}

			return redundant;
		}

		private static BaseInstruction GetMoveInteger(Operand operand)
		{
			return operand.Is64BitInteger ? (BaseInstruction)IRInstruction.MoveInt64 : IRInstruction.MoveInt32;
		}

		private Operand ConstantFoldingIntegerOperations(InstructionNode node)
		{
			if (node.OperandCount != 2 && node.ResultCount != 1)
				return null;

			var instruction = node.Instruction;

			if (!(instruction == IRInstruction.Add32
				|| instruction == IRInstruction.Sub32
				|| instruction == IRInstruction.LogicalAnd32
				|| instruction == IRInstruction.LogicalOr32
				|| instruction == IRInstruction.LogicalXor32
				|| instruction == IRInstruction.MulSigned32
				|| instruction == IRInstruction.MulUnsigned32
				|| instruction == IRInstruction.DivSigned32
				|| instruction == IRInstruction.DivUnsigned32
				|| instruction == IRInstruction.RemSigned32
				|| instruction == IRInstruction.RemUnsigned32
				|| instruction == IRInstruction.ArithShiftRight32
				|| instruction == IRInstruction.ShiftLeft32
				|| instruction == IRInstruction.ShiftRight32
				|| instruction == IRInstruction.Add64
				|| instruction == IRInstruction.Sub64
				|| instruction == IRInstruction.LogicalAnd64
				|| instruction == IRInstruction.LogicalOr64
				|| instruction == IRInstruction.LogicalXor64
				|| instruction == IRInstruction.MulSigned64
				|| instruction == IRInstruction.MulUnsigned64
				|| instruction == IRInstruction.DivSigned64
				|| instruction == IRInstruction.DivUnsigned64
				|| instruction == IRInstruction.RemSigned64
				|| instruction == IRInstruction.RemUnsigned64
				|| instruction == IRInstruction.ArithShiftRight64
				|| instruction == IRInstruction.ShiftLeft64
				|| instruction == IRInstruction.ShiftRight64))
				return null;

			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (!op1.IsResolvedConstant || !op2.IsResolvedConstant)
				return null;

			// Divide by zero!
			if (op2.IsConstantZero &&
				(instruction == IRInstruction.DivSigned32
				|| instruction == IRInstruction.DivUnsigned32
				|| instruction == IRInstruction.RemSigned32
				|| instruction == IRInstruction.RemUnsigned32
				|| instruction == IRInstruction.DivSigned64
				|| instruction == IRInstruction.DivUnsigned64
				|| instruction == IRInstruction.RemSigned64
				|| instruction == IRInstruction.RemUnsigned64))
				return null;

			ulong value = 0;

			var op1Value = op1.ConstantUnsignedLongInteger;
			var op2Value = op1.ConstantUnsignedLongInteger;

			if (instruction == IRInstruction.Add32)
			{
				value = (op1Value + op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.Add64)
			{
				value = op1Value + op2Value;
			}
			else if (instruction == IRInstruction.Sub32)
			{
				value = (op1Value - op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.Sub64)
			{
				value = op1Value - op2Value;
			}
			else if (instruction == IRInstruction.LogicalAnd32)
			{
				value = (op1Value & op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.LogicalAnd64)
			{
				value = op1Value & op2Value;
			}
			else if (instruction == IRInstruction.LogicalOr32)
			{
				value = (op1Value | op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.LogicalOr64)
			{
				value = op1Value | op2Value;
			}
			else if (instruction == IRInstruction.LogicalXor32)
			{
				value = (op1Value ^ op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.LogicalXor64)
			{
				value = op1Value ^ op2Value;
			}
			else if (instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32)
			{
				value = (op1Value * op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64)
			{
				value = op1Value * op2Value;
			}
			else if (instruction == IRInstruction.DivUnsigned32)
			{
				value = (op1Value / op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.DivUnsigned64)
			{
				value = op1Value / op2Value;
			}
			else if (instruction == IRInstruction.DivSigned32)
			{
				value = ((ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger)) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.DivSigned64)
			{
				value = (ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger);
			}
			else if (instruction == IRInstruction.ArithShiftRight32)
			{
				value = ((ulong)(((long)op1Value) >> (int)op2Value)) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.ArithShiftRight64)
			{
				value = (ulong)(((long)op1Value) >> (int)op2Value);
			}
			else if (instruction == IRInstruction.ShiftRight32)
			{
				value = (op1Value >> (int)op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.ShiftRight64)
			{
				value = op1Value >> (int)op2Value;
			}
			else if (instruction == IRInstruction.ShiftLeft32)
			{
				value = (op1Value << (int)op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.ShiftLeft64)
			{
				value = op1Value << (int)op2Value;
			}
			else if (instruction == IRInstruction.RemSigned32)
			{
				value = ((ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger)) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.RemSigned64)
			{
				value = (ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger);
			}
			else if (instruction == IRInstruction.RemUnsigned32)
			{
				value = (op1Value % op2Value) & 0xFFFFFFFF;
			}
			else if (instruction == IRInstruction.RemUnsigned64)
			{
				value = op1Value % op2Value;
			}
			else
			{
				return null;
			}

			return CreateConstant(result.Type, value);
		}

		private Operand ConstantFoldingIntegerCompare(InstructionNode node)
		{
			if (node.OperandCount != 2 && node.ResultCount != 1)
				return null;

			var instruction = node.Instruction;

			if (!(instruction == IRInstruction.CompareInt32x32
				|| instruction == IRInstruction.CompareInt64x32
				|| instruction == IRInstruction.CompareInt64x64))
				return null;

			Operand result = node.Result;
			Operand op1 = node.Operand1;
			Operand op2 = node.Operand2;

			if (!(op1.IsResolvedConstant && op2.IsResolvedConstant))
				return null;

			if (!op1.IsValueType || !op2.IsValueType)
				return null;

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
				default: return null;
			}

			return CreateConstant(result.Type, compareResult ? 1 : 0);
		}
	}
}
