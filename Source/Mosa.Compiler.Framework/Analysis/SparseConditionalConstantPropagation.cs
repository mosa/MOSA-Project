// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Analysis;

/// <summary>
/// Sparse Conditional Constant Propagation (SCCP) Optimization
/// </summary>
public sealed class SparseConditionalConstantPropagation
{
	private const int MAXCONSTANTS = 5;

	private sealed class VariableState
	{
		private enum VariableStatusType
		{ Unknown, OverDefined, SingleConstant, MultipleConstants }

		private enum ReferenceStatusType
		{ Unknown, DefinedNotNull, OverDefined }

		private VariableStatusType Status;

		private ReferenceStatusType ReferenceStatus;

		public int ConstantCount => Constants?.Count ?? 0;

		public List<ulong> Constants { get; private set; }

		public ulong ConstantUnsignedLongInteger => Constants[0];

		public long ConstantSignedLongInteger => (long)Constants[0];

		public bool ConstantsContainZero { get; set; }

		public Operand Operand { get; }

		public bool IsOverDefined
		{
			get => Status == VariableStatusType.OverDefined;
			set { Status = VariableStatusType.OverDefined; Constants = null; Debug.Assert(value); }
		}

		public bool IsUnknown => Status == VariableStatusType.Unknown;

		public bool IsSingleConstant
		{
			get => Status == VariableStatusType.SingleConstant;
			set { Status = VariableStatusType.SingleConstant; Debug.Assert(value); }
		}

		public bool HasMultipleConstants => Status == VariableStatusType.MultipleConstants;

		public bool HasOnlyConstants => Status is VariableStatusType.SingleConstant or VariableStatusType.MultipleConstants;

		public bool IsVirtualRegister { get; set; }

		public bool IsReferenceType { get; set; }

		public bool IsReferenceDefinedUnknown => ReferenceStatus == ReferenceStatusType.Unknown;

		public bool IsReferenceDefinedNotNull
		{
			get => ReferenceStatus == ReferenceStatusType.DefinedNotNull;
			set
			{
				Debug.Assert(value);
				ReferenceStatus = ReferenceStatusType.DefinedNotNull;
			}
		}

		public bool IsReferenceOverDefined
		{
			get => ReferenceStatus == ReferenceStatusType.OverDefined;
			set
			{
				Debug.Assert(value);
				ReferenceStatus = ReferenceStatusType.OverDefined;
			}
		}

		public VariableState(Operand operand)
		{
			Operand = operand;

			IsVirtualRegister = operand.IsVirtualRegister;
			IsReferenceType = operand.IsObject;
			ConstantsContainZero = false;

			if (IsVirtualRegister)
			{
				Status = VariableStatusType.Unknown;
				IsVirtualRegister = true;
			}
			else if (operand.IsUnresolvedConstant)
			{
				IsOverDefined = true;
			}
			else if (operand.IsConstant && operand.IsInteger)
			{
				AddConstant(operand.ConstantUnsigned64);
			}
			else if (operand.IsNull)
			{
				AddConstant(0);
			}
			else
			{
				IsOverDefined = true;
			}

			if (!IsReferenceType || !IsVirtualRegister)
			{
				ReferenceStatus = ReferenceStatusType.OverDefined;
			}
			else
			{
				ReferenceStatus = ReferenceStatusType.Unknown;
			}
		}

		private void AppendConstant(ulong value)
		{
			Constants.Add(value);

			if (value == 0)
			{
				ConstantsContainZero = true;
			}
		}

		public bool AddConstant(ulong value)
		{
			if (Status == VariableStatusType.OverDefined)
				return false;

			if (Constants != null)
			{
				if (Constants.Contains(value))
					return false;
			}
			else
			{
				Constants = new List<ulong>(2);
				AppendConstant(value);
				Status = VariableStatusType.SingleConstant;
				return true;
			}

			if (Constants.Count > MAXCONSTANTS)
			{
				Status = VariableStatusType.OverDefined;
				Constants = null;
				return true;
			}

			AppendConstant(value);

			Status = VariableStatusType.MultipleConstants;
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

			return other.ConstantUnsignedLongInteger == ConstantUnsignedLongInteger;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();
			sb.Append($"{Operand} : {Status}");

			if (IsSingleConstant)
			{
				sb.Append($" = {ConstantUnsignedLongInteger}");
			}
			else if (HasMultipleConstants)
			{
				sb.Append($" ({Constants.Count}) =");
				foreach (var i in Constants)
				{
					sb.Append($" {i},");
				}
				sb.Length--;
			}

			sb.Append(" [null: ");
			if (IsReferenceOverDefined)
				sb.Append("OverDefined");
			else if (IsReferenceDefinedNotNull)
				sb.Append("NotNull");
			else if (IsReferenceDefinedUnknown)
				sb.Append("Unknown");
			sb.Append(']');

			return sb.ToString();
		}
	}

	private readonly bool[] blockStates;

	private readonly Dictionary<Operand, VariableState> variableStates;

	private readonly Stack<Node> instructionWorkList;
	private readonly Stack<BasicBlock> blockWorklist;

	private readonly HashSet<Node> executedStatements;

	private readonly BasicBlocks BasicBlocks;
	private readonly BaseMethodCompilerStage.CreateTraceHandler CreateTrace;
	private readonly TraceLog MainTrace;

	private readonly KeyedList<BasicBlock, Node> phiStatements;

	private readonly bool Is32BitPlatform;

	public SparseConditionalConstantPropagation(BasicBlocks basicBlocks, BaseMethodCompilerStage.CreateTraceHandler createTrace, bool is32BitPlatform)
	{
		// Method is empty - must be a plugged method
		if (basicBlocks.HeadBlocks.Count == 0)
			return;

		CreateTrace = createTrace;
		BasicBlocks = basicBlocks;
		Is32BitPlatform = is32BitPlatform;

		variableStates = new Dictionary<Operand, VariableState>();
		instructionWorkList = new Stack<Node>();
		blockWorklist = new Stack<BasicBlock>();
		phiStatements = new KeyedList<BasicBlock, Node>();
		executedStatements = new HashSet<Node>();

		MainTrace = CreateTrace("SparseConditionalConstantPropagation", 5);

		blockStates = new bool[BasicBlocks.Count];

		for (var i = 0; i < BasicBlocks.Count; i++)
		{
			blockStates[i] = false;
		}

		AddExecutionBlocks(BasicBlocks.HeadBlocks);
		AddExecutionBlocks(BasicBlocks.HandlerHeadBlocks);

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

		for (var i = 0; i < BasicBlocks.Count; i++)
		{
			if (!blockStates[i])
			{
				list.Add(BasicBlocks[i]);
			}
		}

		return list;
	}

	private VariableState GetVariableState(Operand operand)
	{
		if (!variableStates.TryGetValue(operand, out VariableState variable))
		{
			variable = new VariableState(operand);
			variableStates.Add(operand, variable);
		}

		return variable;
	}

	private void DumpTrace()
	{
		if (MainTrace == null)
			return;

		var variableTrace = CreateTrace("Variables", 5);

		if (variableTrace == null)
			return;

		foreach (var variable in variableStates.Values)
		{
			if (variable.IsVirtualRegister)
			{
				variableTrace?.Log(variable.ToString());
			}
		}

		var blockTrace = CreateTrace("Blocks", 5);

		for (var i = 0; i < BasicBlocks.Count; i++)
		{
			blockTrace.Log($"{BasicBlocks[i]} = {(blockStates[i] ? "Executable" : "Dead")}");
		}
	}

	private void AddExecutionBlocks(List<BasicBlock> blocks)
	{
		foreach (var block in blocks)
			AddExecutionBlock(block);
	}

	private void AddExecutionBlock(BasicBlock block)
	{
		if (blockStates[block.Sequence])
			return;

		blockStates[block.Sequence] = true;
		blockWorklist.Push(block);
	}

	private void AddInstruction(Node node)
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
		MainTrace?.Log($"Process Block: {block}");

		// if the block has only one successor block, add successor block to executed block list
		if (block.NextBlocks.Count == 1)
		{
			AddExecutionBlock(block.NextBlocks[0]);
		}

		ProcessInstructionsContinuously(block.First);

		// re-analyze phi statements
		var phiUse = phiStatements.Get(block);

		if (phiUse == null)
			return;

		foreach (var index in phiUse)
		{
			AddInstruction(index);
		}
	}

	private void ProcessInstructionsContinuously(Node node)
	{
		// instead of adding items to the worklist, the whole block will be processed
		for (; !node.IsBlockEndInstruction; node = node.Next)
		{
			if (node.IsEmpty)
				continue;

			var @continue = ProcessInstruction(node);

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

			if (node.Instruction == IR.Branch32
				|| node.Instruction == IR.Branch64
				|| node.Instruction == IR.BranchObject)
			{
				// special case
				ProcessInstructionsContinuously(node);
			}
			else
			{
				ProcessInstruction(node);
			}
		}
	}

	private bool ProcessInstruction(Node node)
	{
		//MainTrace?.Log(node.ToString());

		var instruction = node.Instruction;

		if (instruction == IR.Move32
			|| instruction == IR.Move64
			|| instruction == IR.MoveObject)
		{
			Move(node);
		}
		else if (instruction == IR.NewObject
				 || instruction == IR.NewArray
				 || instruction == IR.NewString)
		{
			NewObject(node);
		}
		else if (instruction == IR.CallDynamic
				 || instruction == IR.CallInterface
				 || instruction == IR.CallDirect
				 || instruction == IR.CallStatic
				 || instruction == IR.CallVirtual
				 || instruction == IR.IntrinsicMethodCall)
		{
			Call(node);
		}
		else if (instruction == IR.Load32
				 || instruction == IR.Load64
				 || instruction == IR.LoadObject

				 || instruction == IR.LoadSignExtend8x32
				 || instruction == IR.LoadSignExtend16x32
				 || instruction == IR.LoadSignExtend8x64
				 || instruction == IR.LoadSignExtend16x64
				 || instruction == IR.LoadSignExtend32x64

				 || instruction == IR.LoadZeroExtend8x32
				 || instruction == IR.LoadZeroExtend16x32
				 || instruction == IR.LoadZeroExtend8x64
				 || instruction == IR.LoadZeroExtend16x64
				 || instruction == IR.LoadZeroExtend32x64

				 || instruction == IR.LoadR4
				 || instruction == IR.LoadR8

				 || instruction == IR.LoadParamSignExtend8x32
				 || instruction == IR.LoadParamSignExtend16x32
				 || instruction == IR.LoadParam32
				 || instruction == IR.LoadParam64
				 || instruction == IR.LoadParamSignExtend8x64
				 || instruction == IR.LoadParamSignExtend16x64
				 || instruction == IR.LoadParamSignExtend32x64
				 || instruction == IR.LoadParamZeroExtend8x32
				 || instruction == IR.LoadParamZeroExtend16x32
				 || instruction == IR.LoadParamZeroExtend8x64
				 || instruction == IR.LoadParamZeroExtend16x64
				 || instruction == IR.LoadParamZeroExtend32x64
				 || instruction == IR.LoadParamR4
				 || instruction == IR.LoadParamR8)
		{
			Load(node);
		}
		else if (instruction == IR.Add32
				 || instruction == IR.Add64
				 || instruction == IR.Sub32
				 || instruction == IR.Sub64
				 || instruction == IR.MulSigned32
				 || instruction == IR.MulUnsigned32
				 || instruction == IR.MulSigned64
				 || instruction == IR.MulUnsigned64
				 || instruction == IR.DivSigned32
				 || instruction == IR.DivUnsigned32
				 || instruction == IR.RemSigned32
				 || instruction == IR.RemUnsigned32
				 || instruction == IR.DivSigned64
				 || instruction == IR.DivUnsigned64
				 || instruction == IR.RemSigned64
				 || instruction == IR.RemUnsigned64
				 || instruction == IR.ShiftLeft32
				 || instruction == IR.ShiftRight32
				 || instruction == IR.ShiftLeft64
				 || instruction == IR.ShiftRight64
				 || instruction == IR.ArithShiftRight32
				 || instruction == IR.ArithShiftRight64)
		{
			IntegerOperation2(node);
		}
		else if (instruction == IR.Neg32
				 || instruction == IR.Neg64
				 || instruction == IR.Not32
				 || instruction == IR.Not64)
		{
			IntegerOperation1(node);
		}
		else if (instruction == IR.Compare32x32
				 || instruction == IR.Compare32x64
				 || instruction == IR.Compare64x32
				 || instruction == IR.Compare64x64
				 || instruction == IR.CompareObject
				 || instruction == IR.CompareManagedPointer)
		{
			CompareOperation(node);
		}
		else if (instruction.IsPhi)
		{
			Phi(node);
		}
		else if (instruction == IR.Jmp)
		{
			Jmp(node);
		}
		else if (instruction == IR.Branch32
				 || instruction == IR.Branch64
				 || instruction == IR.BranchObject
				 || instruction == IR.BranchManagedPointer)
		{
			return CompareBranch(node);
		}
		else if (instruction == IR.AddressOf)
		{
			AddressOf(node);
		}
		else if (instruction == IR.SignExtend8x32
				 || instruction == IR.SignExtend16x32
				 || instruction == IR.SignExtend8x64
				 || instruction == IR.SignExtend16x64
				 || instruction == IR.SignExtend32x64
				 || instruction == IR.ZeroExtend8x32
				 || instruction == IR.ZeroExtend16x32
				 || instruction == IR.ZeroExtend8x64
				 || instruction == IR.ZeroExtend16x64
				 || instruction == IR.ZeroExtend32x64)
		{
			SignOrZeroExtend(node);
		}
		else if (instruction == IR.Switch)
		{
			Switch(node);
		}
		else if (instruction == IR.IfThenElse32
				 || instruction == IR.IfThenElse64)
		{
			IfThenElse(node);
		}
		else if (instruction == IR.FinallyStart)
		{
			FinallyStart(node);
		}
		else if (instruction == IR.SetReturn32
				 || instruction == IR.SetReturnObject
				 || instruction == IR.SetReturnManagedPointer
				 || instruction == IR.SetReturn64
				 || instruction == IR.SetReturnR4
				 || instruction == IR.SetReturnR8
				 || instruction == IR.SetReturnCompound)
		{
			// nothing
		}
		else
		{
			// for all other instructions
			Default(node);
		}

		return true;
	}

	private static bool? NullComparisionCheck(ConditionCode condition, VariableState operand1, VariableState operand2)
	{
		// not null check
		if (condition == ConditionCode.Equal)
		{
			if (operand2.IsSingleConstant && operand2.ConstantSignedLongInteger == 0 && operand1.IsReferenceDefinedNotNull)
			{
				return false;
			}

			if (operand1.IsSingleConstant && operand1.ConstantSignedLongInteger == 0 && operand2.IsReferenceDefinedNotNull)
			{
				return false;
			}
		}
		else if (condition == ConditionCode.NotEqual)
		{
			if (operand2.IsSingleConstant && operand2.ConstantSignedLongInteger == 0 && operand1.IsReferenceDefinedNotNull)
			{
				return true;
			}

			if (operand1.IsSingleConstant && operand1.ConstantSignedLongInteger == 0 && operand2.IsReferenceDefinedNotNull)
			{
				return true;
			}
		}
		else if (condition == ConditionCode.UnsignedGreater)
		{
			if (operand2.IsSingleConstant && operand2.ConstantSignedLongInteger == 0 && operand1.IsReferenceDefinedNotNull)
			{
				return true;
			}
		}

		return null;
	}

	private void CompareOperation(Node node)
	{
		var result = GetVariableState(node.Result);

		if (result.IsOverDefined)
			return;

		var operand1 = GetVariableState(node.Operand1);
		var operand2 = GetVariableState(node.Operand2);

		var compare = NullComparisionCheck(node.ConditionCode, operand1, operand2);

		if (compare.HasValue)
		{
			UpdateToConstant(result, compare.Value ? 1u : 0u);
			return;
		}

		IntegerOperation2(node);
	}

	private void UpdateToConstant(VariableState variable, ulong value)
	{
		Debug.Assert(!variable.IsOverDefined);

		if (variable.AddConstant(value))
		{
			MainTrace?.Log(variable.ToString());

			AddInstruction(variable);
		}
	}

	private void UpdateToOverDefined(VariableState variable)
	{
		if (variable.IsOverDefined)
			return;

		variable.IsOverDefined = true;

		MainTrace?.Log(variable.ToString());

		AddInstruction(variable);
	}

	private void AssignedNewObject(VariableState variable)
	{
		SetReferenceNotNull(variable);
	}

	private void SetReferenceOverdefined(VariableState variable)
	{
		SetReferenceNull(variable);
	}

	private void SetReferenceNull(VariableState variable)
	{
		if (variable.IsReferenceOverDefined)
			return;

		variable.IsReferenceOverDefined = true;

		MainTrace?.Log(variable.ToString());

		AddInstruction(variable);
	}

	private void SetReferenceNotNull(VariableState variable)
	{
		if (variable.IsReferenceOverDefined || variable.IsReferenceDefinedNotNull)
			return;

		variable.IsReferenceDefinedNotNull = true;

		MainTrace?.Log(variable.ToString());

		AddInstruction(variable);
	}

	private void Jmp(Node node)
	{
		if (node.BranchTargets == null || node.BranchTargetsCount == 0)
			return;

		Branch(node);
	}

	private void Move(Node node)
	{
		if (!node.Result.IsVirtualRegister)
			return;

		var result = GetVariableState(node.Result);
		var operand = GetVariableState(node.Operand1);

		CheckAndUpdateNullAssignment(result, operand);

		if (result.IsOverDefined)
			return;

		if (operand.IsOverDefined)
		{
			UpdateToOverDefined(result);
		}
		else if (operand.HasOnlyConstants)
		{
			foreach (var c in operand.Constants)
			{
				UpdateToConstant(result, c);

				if (result.IsOverDefined)
					return;
			}
		}
		else if (operand.IsUnknown)
		{
			Debug.Assert(result.IsUnknown);
		}
	}

	private void CheckAndUpdateNullAssignment(VariableState result, VariableState operand)
	{
		if (result.IsReferenceDefinedUnknown || result.IsReferenceDefinedNotNull)
		{
			if (operand.IsReferenceType)
			{
				if (operand.IsReferenceDefinedNotNull)
				{
					SetReferenceNotNull(result);
				}
				else if (operand.Operand.IsParameter)
				{
					// the this pointer can not be null
					AssignedNewObject(result);
				}
				else
				{
					SetReferenceOverdefined(result);
				}
			}
			else if (operand.IsSingleConstant && operand.ConstantUnsignedLongInteger != 0)
			{
				SetReferenceNotNull(result);
			}
			else if (operand.HasMultipleConstants && !operand.ConstantsContainZero)
			{
				SetReferenceNotNull(result);
			}
			else
			{
				SetReferenceOverdefined(result);
			}
		}
	}

	private void Call(Node node)
	{
		if (node.ResultCount == 0)
			return;

		Debug.Assert(node.ResultCount == 1);

		var result = GetVariableState(node.Result);

		//todo: go thru parameter operands, if any are out, then set overdefine on that parameter operand

		UpdateToOverDefined(result);
		SetReferenceOverdefined(result);
	}

	private void NewObject(Node node)
	{
		if (node.ResultCount == 0)
			return;

		Debug.Assert(node.ResultCount == 1);

		var result = GetVariableState(node.Result);

		UpdateToOverDefined(result);
		SetReferenceNotNull(result);
	}

	private void SignOrZeroExtend(Node node)
	{
		var result = GetVariableState(node.Result);

		if (result.IsOverDefined)
			return;

		var operand1 = GetVariableState(node.Operand1);

		if (operand1.IsOverDefined)
		{
			UpdateToOverDefined(result);
			return;
		}
		else if (operand1.IsUnknown)
		{
			Debug.Assert(result.IsUnknown);
			return;
		}
		else if (operand1.IsSingleConstant)
		{
			if (SignOrZeroExtend(node.Instruction, operand1.ConstantUnsignedLongInteger, out var value))
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
		else if (operand1.HasOnlyConstants)
		{
			foreach (var c1 in operand1.Constants)
			{
				if (SignOrZeroExtend(node.Instruction, c1, out var value))
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

	private void IntegerOperation1(Node node)
	{
		var result = GetVariableState(node.Result);

		if (result.IsOverDefined)
			return;

		var operand1 = GetVariableState(node.Operand1);

		if (operand1.IsOverDefined)
		{
			UpdateToOverDefined(result);
			return;
		}
		else if (operand1.IsUnknown)
		{
			Debug.Assert(result.IsUnknown);
			return;
		}
		else if (operand1.IsSingleConstant)
		{
			if (IntegerOperation1(node.Instruction, operand1.ConstantUnsignedLongInteger, out var value))
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
		else if (operand1.HasOnlyConstants)
		{
			foreach (var c1 in operand1.Constants)
			{
				if (IntegerOperation1(node.Instruction, c1, out var value))
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

	private static bool IntegerOperation1(BaseInstruction instruction, ulong operand1, out ulong result)
	{
		if (instruction == IR.Neg32)
		{
			result = (uint)-((int)operand1);
			return true;
		}
		else if (instruction == IR.Neg64)
		{
			result = (ulong)-((long)operand1);
			return true;
		}
		else if (instruction == IR.Not32 || instruction == IR.Neg64)
		{
			result = ~operand1;
			return true;
		}

		result = 0;
		return false;
	}

	private void IntegerOperation2(Node node)
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
		else if (operand1.IsUnknown || operand2.IsUnknown)
		{
			Debug.Assert(result.IsUnknown);
			return;
		}
		else if (operand1.IsSingleConstant && operand2.IsSingleConstant)
		{
			if (IntegerOperation(node.Instruction, operand1.ConstantUnsignedLongInteger, operand2.ConstantUnsignedLongInteger, node.ConditionCode, out var value))
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
		else if (operand1.HasOnlyConstants && operand2.HasOnlyConstants)
		{
			foreach (var c1 in operand1.Constants)
			{
				foreach (var c2 in operand2.Constants)
				{
					if (IntegerOperation(node.Instruction, c1, c2, node.ConditionCode, out var value))
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

	private static bool IntegerOperation(BaseInstruction instruction, ulong operand1, ulong operand2, ConditionCode conditionCode, out ulong result)
	{
		if (instruction == IR.Add32
			|| instruction == IR.Add64)
		{
			result = operand1 + operand2;
			return true;
		}
		else if (instruction == IR.Sub32
				 || instruction == IR.Sub64)
		{
			result = operand1 - operand2;
			return true;
		}
		else if (instruction == IR.MulUnsigned32
				 || instruction == IR.MulSigned32
				 || instruction == IR.MulUnsigned64
				 || instruction == IR.MulSigned64)
		{
			result = operand1 * operand2;
			return true;
		}
		else if ((instruction == IR.DivUnsigned32 || instruction == IR.DivUnsigned64) && operand2 != 0)
		{
			result = operand1 / operand2;
			return true;
		}
		else if ((instruction == IR.DivSigned32 || instruction == IR.DivSigned64) && operand2 != 0)
		{
			result = (ulong)((long)operand1 / (long)operand2);
			return true;
		}
		else if ((instruction == IR.RemUnsigned32 || instruction == IR.RemUnsigned64) && operand2 != 0)
		{
			result = operand1 % operand2;
			return true;
		}
		else if ((instruction == IR.RemSigned32 || instruction == IR.RemSigned64) && operand2 != 0)
		{
			result = (ulong)((long)operand1 % (long)operand2);
			return true;
		}
		else if (instruction == IR.ArithShiftRight32 || instruction == IR.ArithShiftRight64)
		{
			result = (ulong)((long)operand1 >> (int)operand2);
			return true;
		}
		else if (instruction == IR.ShiftRight32 || instruction == IR.ShiftRight64)
		{
			result = operand1 >> (int)operand2;
			return true;
		}
		else if (instruction == IR.ShiftLeft32 || instruction == IR.ShiftLeft64)
		{
			result = operand1 << (int)operand2;
			return true;
		}
		else if (instruction == IR.Compare32x32)
		{
			var compare = Compare32((uint)operand1, (uint)operand2, conditionCode);

			if (compare.HasValue)
			{
				result = compare.Value ? 1u : 0u;
				return true;
			}
		}
		else if (instruction == IR.Compare64x32
				 || instruction == IR.Compare64x64)
		{
			var compare = Compare64(operand1, operand2, conditionCode);

			if (compare.HasValue)
			{
				result = compare.Value ? 1u : 0u;
				return true;
			}
		}
		result = 0;
		return false;
	}

	private static bool SignOrZeroExtend(BaseInstruction instruction, ulong operand1, out ulong result)
	{
		if (instruction == IR.SignExtend8x32)
		{
			var value = (byte)operand1;
			result = (value & 0x80) == 0 ? value : value | 0xFFFFFF00;
			return true;
		}
		else if (instruction == IR.SignExtend16x32)
		{
			var value = (ushort)operand1;
			result = (value & 0x8000) == 0 ? value : value | 0xFFFF0000;
			return true;
		}
		else if (instruction == IR.SignExtend8x64)
		{
			var value = (byte)operand1;
			result = (value & 0x80) == 0 ? value : value | 0xFFFFFFFFFFFFFF00ul;
			return true;
		}
		else if (instruction == IR.SignExtend16x64)
		{
			var value = (ushort)operand1;
			result = (value & 0x8000) == 0 ? value : value | 0xFFFFFFFFFFFF0000ul;
			return true;
		}
		else if (instruction == IR.SignExtend32x64)
		{
			var value = (uint)operand1;
			result = (value & 0x80000000) == 0 ? value : value | 0xFFFFFFFF00000000ul;
			return true;
		}
		else if (instruction == IR.ZeroExtend8x32)
		{
			result = (byte)operand1;
			return true;
		}
		else if (instruction == IR.ZeroExtend16x32)
		{
			result = (byte)operand1;
			return true;
		}
		else if (instruction == IR.ZeroExtend8x64)
		{
			result = (byte)operand1;
			return true;
		}
		else if (instruction == IR.ZeroExtend16x64)
		{
			result = (ushort)operand1;
			return true;
		}
		else if (instruction == IR.ZeroExtend32x64)
		{
			result = (uint)operand1;
			return true;
		}
		result = 0;
		return false;
	}

	private void Load(Node node)
	{
		var result = GetVariableState(node.Result);
		UpdateToOverDefined(result);
		SetReferenceOverdefined(result);
	}

	private void AddressOf(Node node)
	{
		var result = GetVariableState(node.Result);
		var operand1 = GetVariableState(node.Operand1);

		UpdateToOverDefined(result);
		UpdateToOverDefined(operand1);
		SetReferenceOverdefined(result);
		SetReferenceOverdefined(operand1);
	}

	private void FinallyStart(Node node)
	{
		var result = GetVariableState(node.Result);

		UpdateToOverDefined(result);
		SetReferenceOverdefined(result);
	}

	private void Default(Node node)
	{
		if (node.ResultCount == 0)
			return;

		var result = GetVariableState(node.Result);

		UpdateToOverDefined(result);
		SetReferenceOverdefined(result);

		if (node.ResultCount == 2)
		{
			var result2 = GetVariableState(node.Result);

			UpdateToOverDefined(result2);
			SetReferenceOverdefined(result2);
		}
	}

	private bool CompareBranch(Node node)
	{
		var operand1 = GetVariableState(node.Operand1);
		var operand2 = GetVariableState(node.Operand2);

		var compareNull = NullComparisionCheck(node.ConditionCode, operand1, operand2);

		if (compareNull.HasValue)
		{
			if (compareNull.Value)
			{
				Branch(node);
			}

			return !compareNull.Value;
		}

		var is32Bit = node.Instruction == IR.Branch32 || (node.Instruction == IR.BranchObject && Is32BitPlatform);

		if (operand1.IsOverDefined || operand2.IsOverDefined)
		{
			Branch(node);
			return true;
		}
		else if (operand1.IsSingleConstant && operand2.IsSingleConstant)
		{
			var compare = is32Bit
				? Compare32((uint)operand1.ConstantUnsignedLongInteger, (uint)operand2.ConstantUnsignedLongInteger, node.ConditionCode)
				: Compare64(operand1.ConstantUnsignedLongInteger, operand2.ConstantUnsignedLongInteger, node.ConditionCode);

			if (!compare.HasValue)
			{
				// assume it always branches
				Branch(node);
				return true;
			}

			if (compare.Value)
			{
				Branch(node);
			}

			return !compare.Value;
		}
		else if (operand1.HasOnlyConstants && operand2.HasOnlyConstants)
		{
			bool? final = null;

			foreach (var c1 in operand1.Constants)
			{
				foreach (var c2 in operand2.Constants)
				{
					//bool? compare = Compare(c1, c2, node.ConditionCode);

					var compare = is32Bit
						? Compare32((uint)c1, (uint)c2, node.ConditionCode)
						: Compare64(c1, c2, node.ConditionCode);

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
						Branch(node);
						return true;
					}
				}
			}

			if (final.Value)
			{
				Branch(node);
			}

			return !final.Value;
		}

		Branch(node);
		return true;
	}

	private static bool? Compare32(uint operand1, uint operand2, ConditionCode conditionCode)
	{
		return conditionCode switch
		{
			ConditionCode.Equal => operand1 == operand2,
			ConditionCode.NotEqual => operand1 != operand2,
			ConditionCode.GreaterOrEqual => (int)operand1 >= (int)operand2,
			ConditionCode.Greater => (int)operand1 > (int)operand2,
			ConditionCode.LessOrEqual => (int)operand1 <= (int)operand2,
			ConditionCode.Less => (int)operand1 < (int)operand2,
			ConditionCode.UnsignedGreaterOrEqual => operand1 >= operand2,
			ConditionCode.UnsignedGreater => operand1 > operand2,
			ConditionCode.UnsignedLessOrEqual => operand1 <= operand2,
			ConditionCode.UnsignedLess => operand1 < operand2,
			ConditionCode.Always => true,
			ConditionCode.Never => false,
			_ => null
		};
	}

	private static bool? Compare64(ulong operand1, ulong operand2, ConditionCode conditionCode)
	{
		return conditionCode switch
		{
			ConditionCode.Equal => operand1 == operand2,
			ConditionCode.NotEqual => operand1 != operand2,
			ConditionCode.GreaterOrEqual => (long)operand1 >= (long)operand2,
			ConditionCode.Greater => (long)operand1 > (long)operand2,
			ConditionCode.LessOrEqual => (long)operand1 <= (long)operand2,
			ConditionCode.Less => (long)operand1 < (long)operand2,
			ConditionCode.UnsignedGreaterOrEqual => operand1 >= operand2,
			ConditionCode.UnsignedGreater => operand1 > operand2,
			ConditionCode.UnsignedLessOrEqual => operand1 <= operand2,
			ConditionCode.UnsignedLess => operand1 < operand2,
			ConditionCode.Always => true,
			ConditionCode.Never => false,
			_ => null
		};
	}

	private void Branch(Node node)
	{
		//Debug.Assert(node.BranchTargets.Length == 1);

		foreach (var block in node.BranchTargets)
		{
			AddExecutionBlock(block);
		}
	}

	private void Switch(Node node)
	{
		// no optimization attempted
		Branch(node);
	}

	private void IfThenElse(Node node)
	{
		MainTrace?.Log(node.ToString());

		var result = GetVariableState(node.Result);
		var operand1 = GetVariableState(node.Operand2);
		var operand2 = GetVariableState(node.Operand3);

		if (result.IsOverDefined)
			return;

		if (operand1.IsOverDefined is true or true)
		{
			UpdateToOverDefined(result);
		}
		else if (operand1.HasOnlyConstants && operand2.HasOnlyConstants)
		{
			foreach (var c in operand1.Constants)
			{
				UpdateToConstant(result, c);

				if (result.IsOverDefined)
					return;
			}
			foreach (var c in operand2.Constants)
			{
				UpdateToConstant(result, c);

				if (result.IsOverDefined)
					return;
			}
		}
		else if (operand1.IsUnknown || operand2.IsUnknown)
		{
			Debug.Assert(result.IsUnknown);
		}
		else
		{
			UpdateToOverDefined(result);
		}
	}

	private void Phi(Node node)
	{
		MainTrace?.Log(node.ToString());

		var result = GetVariableState(node.Result);

		if (result.IsOverDefined)
			return;

		var sourceBlocks = node.PhiBlocks;
		var currentBlock = node.Block;

		MainTrace?.Log($"Loop: {currentBlock.PreviousBlocks.Count}");

		// Check if any source blocks haven't been analyzed yet (back-edges in loops)
		// If so, we must be conservative and mark as OverDefined to avoid incorrect optimizations
		var hasUnanalyzedPredecessors = false;
		var analyzedCount = 0;

		for (var index = 0; index < currentBlock.PreviousBlocks.Count; index++)
		{
			var predecessor = sourceBlocks[index];

			phiStatements.AddIfNew(predecessor, node);

			var executable = blockStates[predecessor.Sequence];

			MainTrace?.Log($"# {index}: {predecessor} {(executable ? "Yes" : "No")}");

			if (executable)
			{
				analyzedCount++;
			}
			else
			{
				hasUnanalyzedPredecessors = true;
			}
		}

		// CRITICAL FIX: If we have unanalyzed predecessors (back-edges in loops),
		// and we're dealing with object references, mark as OverDefined to prevent
		// incorrect null-check elimination
		if (hasUnanalyzedPredecessors && analyzedCount > 0 && result.IsReferenceType)
		{
			// We have a partial view - some blocks analyzed, some not (loop back-edge case)
			// Mark both value and reference state as OverDefined to be conservative
			UpdateToOverDefined(result);
			SetReferenceOverdefined(result);
			return;
		}

		for (var index = 0; index < currentBlock.PreviousBlocks.Count; index++)
		{
			var predecessor = sourceBlocks[index];

			var executable = blockStates[predecessor.Sequence];

			if (!executable)
				continue;

			if (result.IsOverDefined)
				continue;

			var op = node.GetOperand(index);

			var operand = GetVariableState(op);

			MainTrace?.Log($"# {index}: {operand}");

			CheckAndUpdateNullAssignment(result, operand);

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
			else if (operand.HasMultipleConstants)
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
