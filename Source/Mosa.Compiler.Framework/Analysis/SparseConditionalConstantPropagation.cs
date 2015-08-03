// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.IR;
using Mosa.Compiler.Trace;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mosa.Compiler.Framework.Analysis
{
	/// <summary>
	///
	/// </summary>
	public sealed class SparseConditionalConstantPropagation
	{
		private class VariableState
		{
			private enum VariableStatus { NeverDefined, OverDefined, Constant };

			private VariableStatus Status;

			public ulong ConstantUnsignedLongInteger { get; set; }

			public long ConstantSignedLongInteger { get { return (long)ConstantUnsignedLongInteger; } set { ConstantUnsignedLongInteger = (ulong)value; } }

			public Operand Operand { get; private set; }

			public bool IsOverDefined { get { return Status == VariableStatus.OverDefined; } set { Status = VariableStatus.OverDefined; Debug.Assert(value); } }

			public bool IsNeverDefined { get { return Status == VariableStatus.NeverDefined; } }

			public bool IsConstant { get { return Status == VariableStatus.Constant; } set { Status = VariableStatus.Constant; Debug.Assert(value); } }

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
					ConstantUnsignedLongInteger = operand.ConstantUnsignedLongInteger;
					IsConstant = true;
				}
				else
				{
					IsOverDefined = true;
				}
			}

			public bool AreConstantsEqual(VariableState other)
			{
				if (!other.IsConstant || !IsConstant)
					return false;

				return (other.ConstantUnsignedLongInteger == ConstantUnsignedLongInteger);
			}

			public override string ToString()
			{
				string s = Operand.ToString() + " : " + Status.ToString();

				if (IsConstant)
				{
					s = s + " = " + ConstantUnsignedLongInteger.ToString();
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
			this.TraceFactory = traceFactory;
			this.BasicBlocks = basicBlocks;

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
				if (variable.IsVirtualRegister && variable.IsConstant)
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

			if (variable.IsConstant && variable.ConstantUnsignedLongInteger != value)
			{
				UpdateToOverDefined(variable);
				return;
			}

			if (variable.IsConstant)
				return;

			variable.IsConstant = true;
			variable.ConstantUnsignedLongInteger = value;

			if (MainTrace.Active) MainTrace.Log(variable.ToString());

			AddInstruction(variable);
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
			else if (operand.IsConstant)
			{
				UpdateToConstant(result, operand.ConstantUnsignedLongInteger);
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
			}
			else if (operand1.IsNeverDefined || operand2.IsNeverDefined)
			{
				//result.IsNeverDefined = true;
				Debug.Assert(result.IsNeverDefined);
				return;
			}
			else if (operand1.IsConstant && operand2.IsConstant)
			{
				var instruction = node.Instruction;

				if (instruction == IRInstruction.AddSigned || instruction == IRInstruction.AddUnsigned)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger + operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.SubSigned || instruction == IRInstruction.SubUnsigned)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger - operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.MulUnsigned || instruction == IRInstruction.MulSigned)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger * operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.DivUnsigned && operand2.ConstantUnsignedLongInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger / operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.DivSigned && operand2.ConstantUnsignedLongInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantSignedLongInteger / operand2.ConstantSignedLongInteger);
				}
				else if (instruction == IRInstruction.RemUnsigned && operand2.ConstantUnsignedLongInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger % operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.RemSigned && operand2.ConstantUnsignedLongInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantSignedLongInteger % operand2.ConstantSignedLongInteger);
				}
				else if (instruction == IRInstruction.ArithmeticShiftRight)
				{
					UpdateToConstant(result, ((long)operand1.ConstantUnsignedLongInteger) >> (int)operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.ShiftRight)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger >> (int)operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.ShiftLeft)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedLongInteger << (int)operand2.ConstantUnsignedLongInteger);
				}
				else if (instruction == IRInstruction.IntegerCompare)
				{
					bool? compare = Compare(operand1, operand2, node.ConditionCode);

					if (compare.HasValue)
					{
						UpdateToConstant(result, compare.Value ? 1 : 0);
						return;
					}

					// unknown integer operation
					UpdateToOverDefined(result);
				}
				else
				{
					// unknown integer operation
					UpdateToOverDefined(result);
				}
			}
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
			//var result2 = GetVariableState(context.Result2);

			UpdateToOverDefined(result);
			//UpdateToOverDefined(result2);
		}

		private void Default(InstructionNode node)
		{
			if (node.ResultCount == 0)
				return;

			Debug.Assert(node.ResultCount == 1);

			var result = GetVariableState(node.Result);

			if (result.IsOverDefined)
				return;

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
			else if (operand1.IsConstant && operand2.IsConstant)
			{
				bool? compare = Compare(operand1, operand2, node.ConditionCode);

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

			Branch(node);
			return true;
		}

		private static bool? Compare(VariableState operand1, VariableState operand2, ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return operand1.ConstantUnsignedLongInteger == operand2.ConstantUnsignedLongInteger;
				case ConditionCode.NotEqual: return operand1.ConstantUnsignedLongInteger != operand2.ConstantUnsignedLongInteger;
				case ConditionCode.GreaterOrEqual: return operand1.ConstantUnsignedLongInteger >= operand2.ConstantUnsignedLongInteger;
				case ConditionCode.GreaterThan: return operand1.ConstantUnsignedLongInteger > operand2.ConstantUnsignedLongInteger;
				case ConditionCode.LessOrEqual: return operand1.ConstantUnsignedLongInteger <= operand2.ConstantUnsignedLongInteger;
				case ConditionCode.LessThan: return operand1.ConstantUnsignedLongInteger < operand2.ConstantUnsignedLongInteger;

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

			VariableState first = null;

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

				var operand = GetVariableState(op);

				//if (Trace.Active) Trace.Log("# " + index.ToString() + ": " + operand.ToString());

				if (operand.IsOverDefined)
				{
					UpdateToOverDefined(result);
					return;
				}

				if (operand.IsConstant)
				{
					if (first == null)
					{
						first = operand;
						continue;
					}

					if (!first.AreConstantsEqual(operand))
					{
						UpdateToOverDefined(result);
						return;
					}
				}
			}

			if (first != null)
			{
				UpdateToConstant(result, first.ConstantUnsignedLongInteger);
			}
		}
	}
}