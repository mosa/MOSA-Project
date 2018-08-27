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

		private int instructionRemovalCount;
		private int constantFoldingCount;
		private int strengthReductionCount;
		private int subexpressionEliminationCount;
		private int parameterLoanEliminationCount;

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

			constantFoldingCount = 0;
			instructionRemovalCount = 0;
			strengthReductionCount = 0;
			subexpressionEliminationCount = 0;
			parameterLoanEliminationCount = 0;
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
			base.Finish();

			UpdateCounter("ValueNumbering.IRInstructionRemoved", instructionRemovalCount);
			UpdateCounter("ValueNumbering.ConstantFolding", constantFoldingCount);
			UpdateCounter("ValueNumbering.StrengthReduction", strengthReductionCount);
			UpdateCounter("ValueNumbering.SubexpressionEliminationCount", subexpressionEliminationCount);
			UpdateCounter("ValueNumbering.ParameterLoanEliminationCount", parameterLoanEliminationCount);

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

			var traceParameters = CreateTraceLog(5, "Parameters");

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

				ParamReadOnly[operand.Index] = !write;

				if (traceParameters.Active) traceParameters.Log(operand + ": " + (write ? "Writable" : "ReadOnly"));
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
			if (trace.Active) trace.Log($"Processing Block: {block}");

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

						if (trace.Active) trace.Log($"Removed Unless PHI: {node}");

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

						if (trace.Active) trace.Log($"Removed Redundant PHI: {node}");

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

				// Simplify expression: constant folding & strength reduction
				var newOperand = ConstantFoldingInteger(node);

				if (newOperand == null)
				{
					newOperand = StrengthReductionInteger(node);

					if (newOperand != null)
					{
						strengthReductionCount++;
					}
				}
				else
				{
					constantFoldingCount++;
				}

				if (newOperand != null)
				{
					Debug.Assert(newOperand != node.Result);

					SetValueNumber(node.Result, newOperand);
					node.SetInstruction(IRInstruction.Nop);
					instructionRemovalCount++;
					continue;
				}

				var hash = ComputeExpressionHash(node);
				var list = GetExpressionsByHash(hash);
				var match = FindMatch(list, node);

				if (match != null)
				{
					var w = GetValueNumber(match.ValueNumber);

					if (trace.Active) trace.Log($"Found Expression Match: {node}");

					SetValueNumber(node.Result, w);

					if (node.Instruction.IsParameterLoad)
						parameterLoanEliminationCount++;

					node.SetInstruction(IRInstruction.Nop);
					instructionRemovalCount++;
					subexpressionEliminationCount++;
					continue;
				}
				else
				{
					if (trace.Active) trace.Log($"No Expression Found: {node}");
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

					//if (trace.Active) trace.Log("Queue Block:" + children[0]);
				}
				else if (ReversePostOrder.Count < 32)
				{
					// Efficient for small sets
					foreach (var child in ReversePostOrder)
					{
						if (children.Contains(child))
						{
							nextblocks.Add(child);

							//if (trace.Active) trace.Log("Queue Block:" + child);
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

							//if (trace.Active) trace.Log("Queue Block:" + child);
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

		private static bool IsEqual(Operand operand1, Operand operand2)
		{
			if (operand1 == operand2)
				return true;

			if (operand1.IsResolvedConstant
				&& operand2.IsResolvedConstant
				&& operand1.IsInteger
				&& operand2.IsInteger
				&& operand1.ConstantUnsignedLongInteger == operand2.ConstantUnsignedLongInteger)
				return true;

			if (operand1.IsResolvedConstant
				&& operand2.IsResolvedConstant
				&& operand1.IsR
				&& operand2.IsR
				&& operand1.ConstantDoubleFloatingPoint == operand2.ConstantDoubleFloatingPoint)
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
					&& IsEqual(node.Operand1, expression.Operand1)
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
			if (trace.Active) trace.Log($"Set: {operand} => {valueVumber}");

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

		private void AddExpressionToHashTable(Expression expression)
		{
			if (!Expressions.TryGetValue(expression.Hash, out List<Expression> list))
			{
				list = new List<Expression>();
				Expressions.Add(expression.Hash, list);
			}

			list.Add(expression);

			if (trace.Active) trace.Log($"Added Expression: {expression.ValueNumber} <= {expression.Instruction} {expression.Operand1} {expression.Operand2}" ?? string.Empty);
		}

		private void RemoveExpressionFromHashTable(Expression expression)
		{
			var list = Expressions[expression.Hash];

			list.Remove(expression);

			if (trace.Active) trace.Log($"Removed Expression: {expression.ValueNumber} <= {expression.Instruction} {expression.Operand1} {expression.Operand2}" ?? string.Empty);
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
							//if (trace.Active) trace.Log($"BEFORE: {node}");
							//if (trace.Active) trace.Log($"Replaced: {operand} with {valueNumber}");
							node.SetOperand(i, valueNumber);

							if (trace.Active) trace.Log($"UPDATED: {node}");
						}
					}
					else
					{
						if (node.Instruction == IRInstruction.Phi)
							continue;

						Debug.Assert(node.Instruction == IRInstruction.Phi);

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
				if (previousPhi.IsEmptyOrNop)
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

		public static Operand ConstantFoldingInteger(InstructionNode node)
		{
			if (node.OperandCount != 2 || node.ResultCount != 1)
				return null;

			if (!node.Operand1.IsResolvedConstant || !node.Operand2.IsResolvedConstant)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if (instruction == IRInstruction.Add32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Add64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger + op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.Sub32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.Sub64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger - op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalAnd32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalAnd64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger & op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalOr32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger | op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.LogicalXor32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalXor64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger ^ op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger * op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.DivUnsigned32 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivUnsigned64 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger / op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.DivSigned32 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, ((ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.DivSigned64 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, (ulong)(op1.ConstantSignedLongInteger / op2.ConstantSignedLongInteger));
			}
			else if (instruction == IRInstruction.ArithShiftRight32)
			{
				return CreateConstant(result.Type, ((ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ArithShiftRight64)
			{
				return CreateConstant(result.Type, (ulong)(((long)op1.ConstantUnsignedLongInteger) >> (int)op2.ConstantUnsignedLongInteger));
			}
			else if (instruction == IRInstruction.ShiftRight32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftRight64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger >> (int)op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.ShiftLeft32)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.ShiftLeft64)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger << (int)op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.RemSigned32 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, ((ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger)) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemSigned64 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, (ulong)(op1.ConstantSignedLongInteger % op2.ConstantSignedLongInteger));
			}
			else if (instruction == IRInstruction.RemUnsigned32 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, (op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger) & 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.RemUnsigned64 && !op2.IsConstantZero)
			{
				return CreateConstant(result.Type, op1.ConstantUnsignedLongInteger % op2.ConstantUnsignedLongInteger);
			}
			else if (instruction == IRInstruction.CompareInt32x32 || instruction == IRInstruction.CompareInt64x32 || instruction == IRInstruction.CompareInt64x64)
			{
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

			return null;
		}

		public static Operand StrengthReductionInteger(InstructionNode node)
		{
			if (node.OperandCount != 2 || node.ResultCount != 1)
				return null;

			var instruction = node.Instruction;
			var result = node.Result;
			var op1 = node.Operand1;
			var op2 = node.Operand2;

			if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.Add32 || instruction == IRInstruction.Add64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.Sub32 || instruction == IRInstruction.Sub64) && (op1 == op2))
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op1.IsConstantZero)
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight32 || instruction == IRInstruction.ShiftRight64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.ShiftLeft32 || instruction == IRInstruction.ShiftRight32) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 32)
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.ShiftLeft64 || instruction == IRInstruction.ShiftRight64) && op2.IsResolvedConstant && op2.ConstantUnsignedLongInteger >= 64)
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && op1.IsConstantOne)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.MulSigned32 || instruction == IRInstruction.MulUnsigned32 || instruction == IRInstruction.MulSigned64 || instruction == IRInstruction.MulUnsigned64) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op2.IsConstantOne)
			{
				return op1;
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op1.IsConstantZero)
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((node.Instruction == IRInstruction.DivSigned32 || node.Instruction == IRInstruction.DivUnsigned32 || node.Instruction == IRInstruction.DivSigned64 || node.Instruction == IRInstruction.DivUnsigned64) && op1 == op2)
			{
				return CreateConstant(result.Type, 1);
			}
			else if ((node.Instruction == IRInstruction.RemUnsigned32 || node.Instruction == IRInstruction.RemUnsigned64) && op2.IsConstantOne)
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.LogicalAnd32 || instruction == IRInstruction.LogicalAnd64) && op1 == op2)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalAnd32 || instruction == IRInstruction.LogicalAnd64) && (op1.IsConstantZero || op2.IsConstantZero))
			{
				return CreateConstant(result.Type, 0);
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op1.IsConstant && op1.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op1.IsConstant && op1.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return op2;
			}
			else if (instruction == IRInstruction.LogicalAnd32 && op2.IsConstant && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalAnd64 && op2.IsConstant && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op1 == op2)
			{
				return op1;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.LogicalOr32 || instruction == IRInstruction.LogicalOr64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if (instruction == IRInstruction.LogicalOr32 && op1.IsConstant && op1.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return CreateConstant(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op1.IsConstant && op1.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return CreateConstant(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr32 && op2.IsConstant && op2.ConstantUnsignedInteger == 0xFFFFFFFF)
			{
				return CreateConstant(result.Type, 0xFFFFFFFF);
			}
			else if (instruction == IRInstruction.LogicalOr64 && op2.IsConstant && op2.ConstantUnsignedLongInteger == 0xFFFFFFFFFFFFFFFF)
			{
				return CreateConstant(result.Type, 0xFFFFFFFFFFFFFFFF);
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op1 == op2)
			{
				return CreateConstant(result.Type, 0);
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op1.IsConstantZero)
			{
				return op2;
			}
			else if ((instruction == IRInstruction.LogicalXor32 || instruction == IRInstruction.LogicalXor64) && op2.IsConstantZero)
			{
				return op1;
			}
			else if (instruction == IRInstruction.CompareInt32x32 || instruction == IRInstruction.CompareInt64x32 || instruction == IRInstruction.CompareInt64x64)
			{
				var condition = node.ConditionCode;

				if (op1 == op2 && (condition == ConditionCode.Equal || condition == ConditionCode.GreaterOrEqual || condition == ConditionCode.UnsignedGreaterOrEqual || condition == ConditionCode.UnsignedLessOrEqual || condition == ConditionCode.LessOrEqual))
				{
					return CreateConstant(result.Type, true ? 1 : 0);
				}
				else if (op1 == op2 && (condition == ConditionCode.NotEqual || condition == ConditionCode.GreaterThan || condition == ConditionCode.LessThan || condition == ConditionCode.UnsignedGreaterThan || condition == ConditionCode.UnsignedLessThan))
				{
					return CreateConstant(result.Type, false ? 1 : 0);
				}
			}

			return null;
		}
	}
}
