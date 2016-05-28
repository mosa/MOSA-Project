// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	///
	/// </summary>
	public sealed class SparseConditionalConstantPropagation
	{
		private static int MAXCONSTANTS = 1;

		private class VariableState
		{
			private enum VariableStatus { NeverDefined, OverDefined, SingleConstant, MultipleConstants };

			private VariableStatus Status;

			private List<ulong> constants;

			public int ConstantCount { get { return constants == null ? 0 : constants.Count; } }

			public IList<ulong> Constants { get { return constants; } }

			private ulong SingleConstant { get { return constants[0]; } }

			public ulong ConstantUnsignedLongInteger { get { return SingleConstant; } }

			public long ConstantSignedLongInteger { get { return (long)SingleConstant; } }

			public Operand Operand { get; private set; }

			public bool IsOverDefined { get { return Status == VariableStatus.OverDefined; } set { Status = VariableStatus.OverDefined; constants = null; Debug.Assert(value); } }

			public bool IsNeverDefined { get { return Status == VariableStatus.NeverDefined; } }

			public bool IsSingleConstant { get { return Status == VariableStatus.SingleConstant; } set { Status = VariableStatus.SingleConstant; Debug.Assert(value); } }

			public bool IsMultipleConstants { get { return Status == VariableStatus.MultipleConstants; } }

			public bool IsOnlyConstants { get { return Status == VariableStatus.SingleConstant || Status == VariableStatus.MultipleConstants; } }

			public bool IsVirtualRegister { get { return Operand.IsVirtualRegister; } }

			public VariableState(Operand operand)
			{
				Operand = operand;

				if (operand.IsVirtualRegister)
				{
					Status = VariableStatus.NeverDefined;
				}
				else if (operand.IsConstant && operand.IsInteger)
				{
					AddConstant(operand.ConstantUnsignedLongInteger);
				}
				else
				{
					IsOverDefined = true;
				}
			}

			public bool AddConstant(ulong value)
			{
				if (Status == VariableStatus.OverDefined)
					return false;

				if (constants != null)
				{
					if (constants.Contains(value))
						return false;
				}
				else
				{
					constants = new List<ulong>(2);
					constants.Add(value);
					Status = VariableStatus.SingleConstant;
					return true;
				}

				if (constants.Count > MAXCONSTANTS)
				{
					Status = VariableStatus.OverDefined;
					constants = null;
					return true;
				}

				constants.Add(value);
				Status = VariableStatus.MultipleConstants;
				return true;
			}

			public void AddConstant(long value)
			{
				AddConstant((ulong)value);
			}

			public bool AreConstantsEqual(VariableState other)
			{
				if (!other.IsSingleConstant || !IsSingleConstant)
					return false;

				return (other.ConstantUnsignedLongInteger == ConstantUnsignedLongInteger);
			}

			public override string ToString()
			{
				string s = Operand.ToString() + " : " + Status.ToString();

				if (IsSingleConstant)
				{
					s = s + " = " + ConstantUnsignedLongInteger.ToString();
				}
				else if (IsMultipleConstants)
				{
					s = s + " (" + constants.Count.ToString() + ") =";
					foreach (ulong i in constants)
					{
						s = s + " " + i.ToString();
						s = s + ",";
					}
					s = s.TrimEnd(',');
				}

				return s;
			}
		}

		private bool[] blockStates;

		private Dictionary<Operand, VariableState> variableStates = new Dictionary<Operand, VariableState>();

		private Stack<InstructionNode> instructionWorkList = new Stack<InstructionNode>();
		private Stack<BasicBlock> blockWorklist = new Stack<BasicBlock>();

		private HashSet<InstructionNode> executedStatements = new HashSet<InstructionNode>();

		private readonly BasicBlocks BasicBlocks;
		private readonly ITraceFactory TraceFactory;
		private readonly TraceLog MainTrace;

		private readonly KeyedList<BasicBlock, InstructionNode> phiStatements = new KeyedList<BasicBlock, InstructionNode>();

		public SparseConditionalConstantPropagation(BasicBlocks basicBlocks, ITraceFactory traceFactory)
		{
			TraceFactory = traceFactory;
			BasicBlocks = basicBlocks;

			MainTrace = CreateTrace("SparseConditionalConstantPropagation");

			// Method is empty - must be a plugged method
			if (BasicBlocks.HeadBlocks.Count == 0)
				return;

			blockStates = new bool[BasicBlocks.Count];

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				blockStates[i] = false;
			}

			// Initialize
			foreach (var block in BasicBlocks.HeadBlocks)
			{
				AddExecutionBlock(block);
			}

			foreach (var block in BasicBlocks.HandlerHeadBlocks)
			{
				AddExecutionBlock(block);
			}

			while (blockWorklist.Count > 0 || instructionWorkList.Count > 0)
			{
				ProcessBlocks();
				ProcessInstructions();
			}

			DumpTrace();

			// Release
			phiStatements = null;
		}

		public List<Tuple<Operand, ulong>> GetIntegerConstants()
		{
			var list = new List<Tuple<Operand, ulong>>();

			foreach (var variable in variableStates.Values)
			{
				if (variable.IsVirtualRegister && variable.IsSingleConstant)
				{
					list.Add(new Tuple<Operand, ulong>(variable.Operand, variable.ConstantUnsignedLongInteger));
				}
			}

			return list;
		}

		public List<BasicBlock> GetDeadBlocked()
		{
			var list = new List<BasicBlock>();

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				if (!blockStates[i])
				{
					list.Add(BasicBlocks[i]);
				}
			}

			return list;
		}

		private TraceLog CreateTrace(string name)
		{
			var sectionTrace = TraceFactory.CreateTraceLog(name);
			return sectionTrace;
		}

		private VariableState GetVariableState(Operand operand)
		{
			VariableState variable;

			if (!variableStates.TryGetValue(operand, out variable))
			{
				variable = new VariableState(operand);
				variableStates.Add(operand, variable);
			}

			return variable;
		}

		private void DumpTrace()
		{
			if (!MainTrace.Active)
				return;

			var variableTrace = CreateTrace("Variables");

			foreach (var variable in variableStates.Values)
			{
				if (variable.IsVirtualRegister)
				{
					variableTrace.Log(variable.ToString());
				}
			}

			var blockTrace = CreateTrace("Blocks");

			for (int i = 0; i < BasicBlocks.Count; i++)
			{
				blockTrace.Log(BasicBlocks[i].ToString() + " = " + (blockStates[i] ? "Executable" : "Dead"));
			}
		}

		private void AddExecutionBlock(BasicBlock block)
		{
			if (blockStates[block.Sequence])
				return;

			blockStates[block.Sequence] = true;
			blockWorklist.Push(block);
		}

		private void AddInstruction(InstructionNode node)
		{
			instructionWorkList.Push(node);
		}

		private void AddInstruction(VariableState variable)
		{
			foreach (var use in variable.Operand.Uses)
			{
				if (executedStatements.Contains(use))
				{
					AddInstruction(use);
				}
			}
		}

		private void ProcessBlocks()
		{
			while (blockWorklist.Count > 0)
			{
				var block = blockWorklist.Pop();
				ProcessBlock(block);
			}
		}

		private void ProcessBlock(BasicBlock block)
		{
			if (MainTrace.Active) MainTrace.Log("Process Block: " + block.ToString());

			// if the block has only one successor block, add successor block to executed block list
			if (block.NextBlocks.Count == 1)
			{
				AddExecutionBlock(block.NextBlocks[0]);
			}

			ProcessInstructionsContinuiously(block.First);

			// re-analysis phi statements
			var phiUse = phiStatements.Get(block);

			if (phiUse == null)
				return;

			foreach (var index in phiUse)
			{
				AddInstruction(index);
			}
		}

		private void ProcessInstructionsContinuiously(InstructionNode node)
		{
			// instead of adding items to the worklist, the whole block will be processed
			for (; !node.IsBlockEndInstruction; node = node.Next)
			{
				if (node.IsEmpty)
					continue;

				bool @continue = ProcessInstruction(node);

				executedStatements.Add(node);

				if (!@continue)
					return;
			}
		}

		private void ProcessInstructions()
		{
			while (instructionWorkList.Count > 0)
			{
				var node = instructionWorkList.Pop();

				if (node.Instruction == IRInstruction.IntegerCompareBranch)
				{
					// special case
					ProcessInstructionsContinuiously(node);
				}
				else
				{
					ProcessInstruction(node);
				}
			}
		}

		private bool ProcessInstruction(InstructionNode node)
		{
			//if (MainTrace.Active) MainTrace.Log(context.ToString());

			var instruction = node.Instruction;

			if (instruction == IRInstruction.Move)
			{
				Move(node);
			}
			else if (instruction == IRInstruction.Call ||
				instruction == IRInstruction.IntrinsicMethodCall)
			{
				Call(node);
			}
			else if (instruction == IRInstruction.Load ||
				node.Instruction == IRInstruction.LoadSignExtended ||
				node.Instruction == IRInstruction.LoadZeroExtended)
			{
				Load(node);
			}
			else if (instruction == IRInstruction.AddSigned ||
				instruction == IRInstruction.AddUnsigned ||
				instruction == IRInstruction.SubSigned ||
				instruction == IRInstruction.SubUnsigned ||
				instruction == IRInstruction.MulSigned ||
				instruction == IRInstruction.MulUnsigned ||
				instruction == IRInstruction.DivSigned ||
				instruction == IRInstruction.DivUnsigned ||
				instruction == IRInstruction.RemSigned ||
				instruction == IRInstruction.RemUnsigned ||
				instruction == IRInstruction.IntegerCompare ||
				instruction == IRInstruction.ShiftLeft ||
				instruction == IRInstruction.ShiftRight ||
				instruction == IRInstruction.ArithmeticShiftRight)
			{
				IntegerOperation(node);
			}
			else if (node.Instruction == IRInstruction.Phi)
			{
				Phi(node);
			}
			else if (node.Instruction == IRInstruction.Jmp)
			{
				Jmp(node);
			}
			else if (node.Instruction == IRInstruction.IntegerCompareBranch)
			{
				return IntegerCompareBranch(node);
			}
			else if (node.Instruction == IRInstruction.AddressOf)
			{
				AddressOf(node);
			}
			else if (instruction == IRInstruction.ZeroExtendedMove ||
				instruction == IRInstruction.SignExtendedMove)
			{
				Move(node);
			}
			else if (instruction == IRInstruction.Switch)
			{
				Switch(node);
			}
			else if (instruction == IRInstruction.FinallyStart)
			{
				FinallyStart(node);
			}
			else
			{
				// for all other instructions
				Default(node);
			}

			return true;
		}

		private void UpdateToConstant(VariableState variable, long value)
		{
			UpdateToConstant(variable, (ulong)value);
		}

		private void UpdateToConstant(VariableState variable, ulong value)
		{
			Debug.Assert(!variable.IsOverDefined);

			if (variable.AddConstant(value))
			{
				if (MainTrace.Active) MainTrace.Log(variable.ToString());

				AddInstruction(variable);
			}
		}

		private void UpdateToOverDefined(VariableState variable)
		{
			if (variable.IsOverDefined)
				return;

			variable.IsOverDefined = true;

			if (MainTrace.Active) MainTrace.Log(variable.ToString());

			AddInstruction(variable);
		}

		private void Jmp(InstructionNode node)
		{
			if (node.BranchTargets == null || node.BranchTargetsCount == 0)
				return;

			Branch(node);
		}

		private void Move(InstructionNode node)
		{
			if (!node.Result.IsVirtualRegister)
				return;

			var result = GetVariableState(node.Result);
			var operand = GetVariableState(node.Operand1);

			if (result.IsOverDefined)
				return;

			if (operand.IsOverDefined)
			{
				UpdateToOverDefined(result);
			}
			else if (operand.IsOnlyConstants)
			{
				foreach (var c in operand.Constants)
				{
					UpdateToConstant(result, c);

					if (result.IsOverDefined)
						return;
				}
			}
			else if (operand.IsNeverDefined)
			{
				Debug.Assert(result.IsNeverDefined);
			}

			return;
		}

		private void Call(InstructionNode node)
		{
			if (node.ResultCount == 0)
				return;

			Debug.Assert(node.ResultCount == 1);

			var result = GetVariableState(node.Result);

			UpdateToOverDefined(result);
		}

		private void IntegerOperation(InstructionNode node)
		{
			var result = GetVariableState(node.Result);

			if (result.IsOverDefined)
				return;

			var operand1 = GetVariableState(node.Operand1);
			var operand2 = GetVariableState(node.Operand2);

			if (operand1.IsOverDefined || operand2.IsOverDefined)
			{
				UpdateToOverDefined(result);
				return;
			}
			else if (operand1.IsNeverDefined || operand2.IsNeverDefined)
			{
				Debug.Assert(result.IsNeverDefined);
				return;
			}
			else if (operand1.IsSingleConstant && operand2.IsSingleConstant)
			{
				ulong value;

				if (IntegerOperation(node.Instruction, operand1.ConstantUnsignedLongInteger, operand2.ConstantUnsignedLongInteger, node.ConditionCode, out value))
				{
					UpdateToConstant(result, value);
					return;
				}
				else
				{
					UpdateToOverDefined(result);
					return;
				}
			}
			else if (operand1.IsOnlyConstants && operand2.IsOnlyConstants)
			{
				foreach (var c1 in operand1.Constants)
				{
					foreach (var c2 in operand2.Constants)
					{
						ulong value;

						if (IntegerOperation(node.Instruction, c1, c2, node.ConditionCode, out value))
						{
							UpdateToConstant(result, value);
						}
						else
						{
							UpdateToOverDefined(result);
							return;
						}

						if (result.IsOverDefined)
							return;
					}
				}
			}
		}

		private bool IntegerOperation(BaseInstruction instruction, ulong operand1, ulong operand2, ConditionCode conditionCode, out ulong result)
		{
			if (instruction == IRInstruction.AddSigned || instruction == IRInstruction.AddUnsigned)
			{
				result = operand1 + operand2;
				return true;
			}
			else if (instruction == IRInstruction.SubSigned || instruction == IRInstruction.SubUnsigned)
			{
				result = operand1 - operand2;
				return true;
			}
			else if (instruction == IRInstruction.MulUnsigned || instruction == IRInstruction.MulSigned)
			{
				result = operand1 * operand2;
				return true;
			}
			else if (instruction == IRInstruction.DivUnsigned && operand2 != 0)
			{
				result = operand1 / operand2;
				return true;
			}
			else if (instruction == IRInstruction.DivSigned && operand2 != 0)
			{
				result = (ulong)((long)operand1 / (long)operand2);
				return true;
			}
			else if (instruction == IRInstruction.RemUnsigned && operand2 != 0)
			{
				result = operand1 % operand2;
				return true;
			}
			else if (instruction == IRInstruction.RemSigned && operand2 != 0)
			{
				result = (ulong)((long)operand1 % (long)operand2);
				return true;
			}
			else if (instruction == IRInstruction.ArithmeticShiftRight)
			{
				result = (ulong)(((long)operand1) >> (int)operand2);
				return true;
			}
			else if (instruction == IRInstruction.ShiftRight)
			{
				result = operand1 >> (int)operand2;
				return true;
			}
			else if (instruction == IRInstruction.ShiftLeft)
			{
				result = operand1 << (int)operand2;
				return true;
			}
			else if (instruction == IRInstruction.IntegerCompare)
			{
				bool? compare = Compare(operand1, operand2, conditionCode);

				if (compare.HasValue)
				{
					result = compare.Value ? 1u : 0u;
					return true;
				}
			}

			result = 0;
			return false;
		}

		private void Load(InstructionNode node)
		{
			var result = GetVariableState(node.Result);
			UpdateToOverDefined(result);
		}

		private void AddressOf(InstructionNode node)
		{
			var result = GetVariableState(node.Result);
			var operand1 = GetVariableState(node.Operand1);

			UpdateToOverDefined(result);
			UpdateToOverDefined(operand1);
		}

		private void FinallyStart(InstructionNode node)
		{
			var result = GetVariableState(node.Result);

			UpdateToOverDefined(result);
		}

		private void Default(InstructionNode node)
		{
			if (node.ResultCount == 0)
				return;

			Debug.Assert(node.ResultCount == 1);

			var result = GetVariableState(node.Result);

			UpdateToOverDefined(result);
		}

		private bool IntegerCompareBranch(InstructionNode node)
		{
			var operand1 = GetVariableState(node.Operand1);
			var operand2 = GetVariableState(node.Operand2);

			if (operand1.IsOverDefined || operand2.IsOverDefined)
			{
				Branch(node);
				return true;
			}
			else if (operand1.IsSingleConstant && operand2.IsSingleConstant)
			{
				bool? compare = Compare(operand1.ConstantUnsignedLongInteger, operand2.ConstantUnsignedLongInteger, node.ConditionCode);

				if (!compare.HasValue)
				{
					Branch(node);
					return true;
				}

				if (compare.Value)
				{
					Branch(node);
				}

				return !compare.Value;
			}
			else if (operand1.IsOnlyConstants && operand2.IsOnlyConstants)
			{
				bool? final = null;

				foreach (var c1 in operand1.Constants)
				{
					foreach (var c2 in operand2.Constants)
					{
						bool? compare = Compare(c1, c2, node.ConditionCode);

						if (!compare.HasValue)
						{
							Branch(node);
							return true;
						}

						if (!final.HasValue)
						{
							final = compare;
							continue;
						}
						else if (final.Value == compare.Value)
						{
							continue;
						}
						else
						{
							return true;
						}
					}
				}

				return !final.Value;
			}

			Branch(node);

			return true;
		}

		private static bool? Compare(ulong operand1, ulong operand2, ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return operand1 == operand2;
				case ConditionCode.NotEqual: return operand1 != operand2;
				case ConditionCode.GreaterOrEqual: return operand1 >= operand2;
				case ConditionCode.GreaterThan: return operand1 > operand2;
				case ConditionCode.LessOrEqual: return operand1 <= operand2;
				case ConditionCode.LessThan: return operand1 < operand2;

				case ConditionCode.UnsignedGreaterOrEqual: return operand1 >= operand2;
				case ConditionCode.UnsignedGreaterThan: return operand1 > operand2;
				case ConditionCode.UnsignedLessOrEqual: return operand1 <= operand2;
				case ConditionCode.UnsignedLessThan: return operand1 < operand2;

				case ConditionCode.Always: return true;
				case ConditionCode.Never: return false;

				// unknown integer comparison
				default: return null;
			}
		}

		private void Branch(InstructionNode node)
		{
			//Debug.Assert(node.BranchTargets.Length == 1);

			foreach (var block in node.BranchTargets)
			{
				AddExecutionBlock(block);
			}
		}

		private void Switch(InstructionNode node)
		{
			// no optimization attempted
			Branch(node);
		}

		private void Phi(InstructionNode node)
		{
			//if (Trace.Active) Trace.Log(node.ToString());

			var result = GetVariableState(node.Result);

			if (result.IsOverDefined)
				return;

			var sourceBlocks = node.PhiBlocks;

			var currentBlock = node.Block;

			//if (Trace.Active) Trace.Log("Loop: " + currentBlock.PreviousBlocks.Count.ToString());

			for (var index = 0; index < currentBlock.PreviousBlocks.Count; index++)
			{
				var op = node.GetOperand(index);

				var predecessor = sourceBlocks[index];
				bool executable = blockStates[predecessor.Sequence];

				//if (Trace.Active) Trace.Log("# " + index.ToString() + ": " + predecessor.ToString() + " " + (executable ? "Yes" : "No"));

				phiStatements.AddIfNew(predecessor, node);

				if (!executable)
					continue;

				if (result.IsOverDefined)
					continue;

				var operand = GetVariableState(op);

				//if (Trace.Active) Trace.Log("# " + index.ToString() + ": " + operand.ToString());

				if (operand.IsOverDefined)
				{
					UpdateToOverDefined(result);
					continue;
				}
				else if (operand.IsSingleConstant)
				{
					UpdateToConstant(result, operand.ConstantUnsignedLongInteger);
					continue;
				}
				else if (operand.IsMultipleConstants)
				{
					foreach (var c in operand.Constants)
					{
						UpdateToConstant(result, c);

						if (result.IsOverDefined)
							break;
					}
				}
			}
		}
	}
}
