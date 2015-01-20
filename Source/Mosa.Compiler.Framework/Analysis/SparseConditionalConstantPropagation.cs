/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
	public class SparseConditionalConstantPropagation
	{
		protected class VariableState
		{
			private enum VariableStatus { NeverDefined, OverDefined, Constant };

			private VariableStatus Status;

			public ulong ConstantUnsignedInteger { get; set; }

			public long ConstantSignedInteger { get { return (long)ConstantUnsignedInteger; } set { ConstantUnsignedInteger = (ulong)value; } }

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
					ConstantUnsignedInteger = operand.ConstantUnsignedInteger;
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

				return (other.ConstantUnsignedInteger == ConstantUnsignedInteger);
			}

			public override string ToString()
			{
				string s = Operand.ToString() + " : " + Status.ToString();

				if (IsConstant)
				{
					s = s + " = " + ConstantUnsignedInteger.ToString();
				}

				return s;
			}
		}

		protected bool[] blockStates;

		protected Dictionary<Operand, VariableState> variableStates = new Dictionary<Operand, VariableState>();

		protected Stack<int> instructionWorkList = new Stack<int>();
		protected Stack<BasicBlock> blockWorklist = new Stack<BasicBlock>();

		protected HashSet<int> executedStatements = new HashSet<int>();

		protected readonly BasicBlocks BasicBlocks;
		protected readonly InstructionSet InstructionSet;
		protected readonly ITraceFactory TraceFactory;
		protected readonly TraceLog MainTrace;

		protected readonly KeyedList<BasicBlock, int> phiStatements = new KeyedList<BasicBlock, int>();

		public SparseConditionalConstantPropagation(BasicBlocks basicBlocks, InstructionSet instructionSet, ITraceFactory traceFactory)
		{
			this.TraceFactory = traceFactory;
			this.BasicBlocks = basicBlocks;
			this.InstructionSet = instructionSet;

			MainTrace = CreateTrace("ConditionalConstantPropagation");

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
					list.Add(new Tuple<Operand, ulong>(variable.Operand, variable.ConstantUnsignedInteger));
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

		protected void AddExecutionBlock(BasicBlock block)
		{
			if (blockStates[block.Sequence])
				return;

			blockStates[block.Sequence] = true;
			blockWorklist.Push(block);
		}

		protected void AddInstruction(int index)
		{
			instructionWorkList.Push(index);
		}

		protected void AddInstruction(Context context)
		{
			instructionWorkList.Push(context.Index);
		}

		protected void AddInstruction(VariableState variable)
		{
			foreach (var use in variable.Operand.Uses)
			{
				if (executedStatements.Contains(use))
				{
					var context = new Context(InstructionSet, use);
					AddInstruction(context);
				}
			}
		}

		protected void ProcessBlocks()
		{
			while (blockWorklist.Count > 0)
			{
				var block = blockWorklist.Pop();
				ProcessBlock(block);
			}
		}

		protected void ProcessBlock(BasicBlock block)
		{
			if (MainTrace.Active) MainTrace.Log("Process Block: " + block.ToString());

			// if the block has only one successor block, add successor block to executed block list
			if (block.NextBlocks.Count == 1)
			{
				AddExecutionBlock(block.NextBlocks[0]);
			}

			ProcessInstructionsContinuiously(new Context(InstructionSet, block));

			// re-analysis phi statements
			var phiUse = phiStatements.Get(block);

			if (phiUse == null)
				return;

			foreach (int index in phiUse)
			{
				AddInstruction(index);
			}
		}

		protected void ProcessInstructionsContinuiously(Context context)
		{
			// instead of adding items to the worklist, the whole block will be processed
			for (; !context.IsBlockEndInstruction; context.GotoNext())
			{
				if (context.IsEmpty)
					continue;

				bool @continue = ProcessInstruction(context);

				executedStatements.Add(context.Index);

				if (!@continue)
					return;
			}
		}

		protected void ProcessInstructions()
		{
			while (instructionWorkList.Count > 0)
			{
				var index = instructionWorkList.Pop();

				var context = new Context(InstructionSet, index);

				if (context.Instruction == IRInstruction.IntegerCompareBranch)
				{
					// special case
					ProcessInstructionsContinuiously(context);
				}
				else
				{
					ProcessInstruction(context);
				}
			}
		}

		protected bool ProcessInstruction(Context context)
		{
			//if (MainTrace.Active) MainTrace.Log(context.ToString());

			var instruction = context.Instruction;

			if (instruction == IRInstruction.Move)
			{
				Move(context);
			}
			else if (instruction == IRInstruction.Call ||
				instruction == IRInstruction.IntrinsicMethodCall)
			{
				Call(context);
			}
			else if (instruction == IRInstruction.Load ||
				context.Instruction == IRInstruction.LoadSignExtended ||
				context.Instruction == IRInstruction.LoadZeroExtended)
			{
				Load(context);
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
				IntegerOperation(context);
			}
			else if (context.Instruction == IRInstruction.Phi)
			{
				Phi(context);
			}
			else if (context.Instruction == IRInstruction.Jmp)
			{
				Jmp(context);
			}
			else if (context.Instruction == IRInstruction.IntegerCompareBranch)
			{
				return IntegerCompareBranch(context);
			}
			else if (context.Instruction == IRInstruction.AddressOf)
			{
				AddressOf(context);
			}
			else if (instruction == IRInstruction.ZeroExtendedMove ||
				instruction == IRInstruction.SignExtendedMove)
			{
				Move(context);
			}
			else if (instruction == IRInstruction.Switch)
			{
				Switch(context);
			}
			else if (instruction == IRInstruction.FinallyStart)
			{
				FinallyStart(context);
			}
			else
			{
				// for all other instructions
				Default(context);
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

			if (variable.IsConstant && variable.ConstantUnsignedInteger != value)
			{
				UpdateToOverDefined(variable);
				return;
			}

			if (variable.IsConstant)
				return;

			variable.IsConstant = true;
			variable.ConstantUnsignedInteger = value;

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

		private void Jmp(Context context)
		{
			if (context.BranchTargets == null || context.BranchTargets.Length == 0)
				return;

			Branch(context);
		}

		private void Move(Context context)
		{
			if (!context.Result.IsVirtualRegister)
				return;

			var result = GetVariableState(context.Result);
			var operand = GetVariableState(context.Operand1);

			if (result.IsOverDefined)
				return;

			if (operand.IsOverDefined)
			{
				UpdateToOverDefined(result);
			}
			else if (operand.IsConstant)
			{
				UpdateToConstant(result, operand.ConstantUnsignedInteger);
			}
			else if (operand.IsNeverDefined)
			{
				Debug.Assert(result.IsNeverDefined);
			}

			return;
		}

		private void Call(Context context)
		{
			if (context.ResultCount == 0)
				return;

			Debug.Assert(context.ResultCount == 1);

			var result = GetVariableState(context.Result);

			UpdateToOverDefined(result);
		}

		private void IntegerOperation(Context context)
		{
			var result = GetVariableState(context.Result);

			if (result.IsOverDefined)
				return;

			var operand1 = GetVariableState(context.Operand1);
			var operand2 = GetVariableState(context.Operand2);

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
				var instruction = context.Instruction;

				if (instruction == IRInstruction.AddSigned || instruction == IRInstruction.AddUnsigned)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger + operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.SubSigned || instruction == IRInstruction.SubUnsigned)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger - operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.MulUnsigned || instruction == IRInstruction.MulSigned)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger * operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.DivUnsigned && operand2.ConstantUnsignedInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger / operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.DivSigned && operand2.ConstantUnsignedInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantSignedInteger / operand2.ConstantSignedInteger);
				}
				else if (instruction == IRInstruction.RemUnsigned && operand2.ConstantUnsignedInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger % operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.RemSigned && operand2.ConstantUnsignedInteger != 0)
				{
					UpdateToConstant(result, operand1.ConstantSignedInteger % operand2.ConstantSignedInteger);
				}
				else if (instruction == IRInstruction.ArithmeticShiftRight)
				{
					UpdateToConstant(result, ((long)operand1.ConstantUnsignedInteger) >> (int)operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.ShiftRight)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger >> (int)operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.ShiftLeft)
				{
					UpdateToConstant(result, operand1.ConstantUnsignedInteger << (int)operand2.ConstantUnsignedInteger);
				}
				else if (instruction == IRInstruction.IntegerCompare)
				{
					bool? compare = Compare(operand1, operand2, context.ConditionCode);

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

		private void Load(Context context)
		{
			var result = GetVariableState(context.Result);
			UpdateToOverDefined(result);
		}

		private void AddressOf(Context context)
		{
			var result = GetVariableState(context.Result);
			var operand1 = GetVariableState(context.Operand1);

			UpdateToOverDefined(result);
			UpdateToOverDefined(operand1);
		}

		private void FinallyStart(Context context)
		{
			var result = GetVariableState(context.Result);
			//var result2 = GetVariableState(context.Result2);

			UpdateToOverDefined(result);
			//UpdateToOverDefined(result2);
		}

		private void Default(Context context)
		{
			if (context.ResultCount == 0)
				return;

			Debug.Assert(context.ResultCount == 1);

			var result = GetVariableState(context.Result);

			if (result.IsOverDefined)
				return;

			UpdateToOverDefined(result);
		}

		private bool IntegerCompareBranch(Context context)
		{
			var operand1 = GetVariableState(context.Operand1);
			var operand2 = GetVariableState(context.Operand2);

			if (operand1.IsOverDefined || operand2.IsOverDefined)
			{
				Branch(context);
				return true;
			}
			else if (operand1.IsConstant && operand2.IsConstant)
			{
				bool? compare = Compare(operand1, operand2, context.ConditionCode);

				if (!compare.HasValue)
				{
					Branch(context);
					return true;
				}

				if (compare.Value)
				{
					Branch(context);
				}

				return !compare.Value;
			}

			Branch(context);
			return true;
		}

		private static bool? Compare(VariableState operand1, VariableState operand2, ConditionCode conditionCode)
		{
			switch (conditionCode)
			{
				case ConditionCode.Equal: return operand1.ConstantUnsignedInteger == operand2.ConstantUnsignedInteger;
				case ConditionCode.NotEqual: return operand1.ConstantUnsignedInteger != operand2.ConstantUnsignedInteger;
				case ConditionCode.GreaterOrEqual: return operand1.ConstantUnsignedInteger >= operand2.ConstantUnsignedInteger;
				case ConditionCode.GreaterThan: return operand1.ConstantUnsignedInteger > operand2.ConstantUnsignedInteger;
				case ConditionCode.LessOrEqual: return operand1.ConstantUnsignedInteger <= operand2.ConstantUnsignedInteger;
				case ConditionCode.LessThan: return operand1.ConstantUnsignedInteger < operand2.ConstantUnsignedInteger;

				case ConditionCode.Always: return true;
				case ConditionCode.Never: return false;

				// unknown integer comparison
				default: return null;
			}
		}

		private void Branch(Context context)
		{
			//Debug.Assert(context.BranchTargets.Length == 1);

			foreach (var target in context.BranchTargets)
			{
				var block = BasicBlocks.GetByLabel(target);

				AddExecutionBlock(block);
			}
		}

		private BasicBlock GetBlock(Context context, bool backwards)
		{
			var block = context.BasicBlock;

			if (block == null)
			{
				Context clone = context.Clone();

				if (backwards)
					clone.GotoFirst();
				else
					clone.GotoLast();

				block = BasicBlocks.GetByLabel(clone.Label);
			}

			return block;
		}

		private void Switch(Context context)
		{
			// no optimization attempted
			Branch(context);
		}

		private void Phi(Context context)
		{
			//if (Trace.Active) Trace.Log(context.ToString());

			var result = GetVariableState(context.Result);

			if (result.IsOverDefined)
				return;

			var sourceBlocks = context.Other as List<BasicBlock>;

			VariableState first = null;

			var currentBlock = GetBlock(context, true);

			//if (Trace.Active) Trace.Log("Loop: " + currentBlock.PreviousBlocks.Count.ToString());

			for (var index = 0; index < currentBlock.PreviousBlocks.Count; index++)
			{
				var op = context.GetOperand(index);

				var predecessor = sourceBlocks[index];
				bool executable = blockStates[predecessor.Sequence];

				//if (Trace.Active) Trace.Log("# " + index.ToString() + ": " + predecessor.ToString() + " " + (executable ? "Yes" : "No"));

				phiStatements.AddIfNew(predecessor, context.Index);

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
				UpdateToConstant(result, first.ConstantUnsignedInteger);
			}
		}
	}
}