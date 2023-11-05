// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Managers;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Bit Tracker Stage
/// </summary>
public sealed class BitTrackerStage : BaseMethodCompilerStage
{
	// This stage propagates bit and value range knowledge thru the various operations.
	// This additional knowledge may enable additional optimizations opportunities.

	private const int MaxInstructions = 1024;

	private const ulong Upper32BitsSet = ~(ulong)uint.MaxValue;
	private const ulong Upper48BitsSet = ~(ulong)ushort.MaxValue;
	private const ulong Upper56BitsSet = ~(ulong)byte.MaxValue;

	private readonly Counter BranchesRemovedCount = new("BitTrackerStage.BranchesRemoved");
	private readonly Counter InstructionsRemovedCount = new("BitTrackerStage.InstructionsRemoved");
	private readonly Counter InstructionsUpdatedCount = new("BitTrackerStage.InstructionsUpdated");

	private TraceLog trace;

	private readonly NodeVisitationDelegate[] visitation = new NodeVisitationDelegate[MaxInstructions];

	private delegate void NodeVisitationDelegate(Node node);

	protected override void Finish()
	{
		trace = null;
	}

	protected override void Initialize()
	{
		Register(InstructionsUpdatedCount);
		Register(InstructionsRemovedCount);
		Register(BranchesRemovedCount);

		Register(IRInstruction.Phi32, Phi32);
		Register(IRInstruction.Phi64, Phi64);

		Register(IRInstruction.Move32, Move32);
		Register(IRInstruction.Move64, Move64);

		Register(IRInstruction.Truncate64x32, Truncate64x32);

		Register(IRInstruction.GetLow32, GetLow32);
		Register(IRInstruction.GetHigh32, GetHigh32);
		Register(IRInstruction.To64, To64);

		Register(IRInstruction.Or32, Or32);
		Register(IRInstruction.Or64, Or64);
		Register(IRInstruction.And32, And32);
		Register(IRInstruction.And64, And64);
		Register(IRInstruction.Xor32, Xor32);
		Register(IRInstruction.Xor64, Xor64);
		Register(IRInstruction.Not32, Not32);
		Register(IRInstruction.Not64, Not64);

		Register(IRInstruction.LoadZeroExtend8x32, LoadZeroExtend8x32);
		Register(IRInstruction.LoadZeroExtend16x32, LoadZeroExtend16x32);

		Register(IRInstruction.LoadZeroExtend8x64, LoadZeroExtend8x64);
		Register(IRInstruction.LoadZeroExtend16x64, LoadZeroExtend16x64);
		Register(IRInstruction.LoadZeroExtend32x64, LoadZeroExtend32x64);

		Register(IRInstruction.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
		Register(IRInstruction.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
		Register(IRInstruction.LoadParamZeroExtend8x64, LoadParamZeroExtend8x64);
		Register(IRInstruction.LoadParamZeroExtend16x64, LoadParamZeroExtend16x64);
		Register(IRInstruction.LoadParamZeroExtend32x64, LoadParamZeroExtend32x64);

		Register(IRInstruction.ShiftRight32, ShiftRight32);
		Register(IRInstruction.ShiftRight64, ShiftRight64);

		Register(IRInstruction.ArithShiftRight32, ArithShiftRight32);
		Register(IRInstruction.ArithShiftRight64, ArithShiftRight64);

		Register(IRInstruction.ShiftLeft32, ShiftLeft32);
		Register(IRInstruction.ShiftLeft64, ShiftLeft64);

		Register(IRInstruction.Compare32x32, Compare32x32);
		Register(IRInstruction.Compare64x64, Compare64x64);
		Register(IRInstruction.Compare64x32, Compare64x32);

		Register(IRInstruction.MulUnsigned32, MulUnsigned32);
		Register(IRInstruction.MulUnsigned64, MulUnsigned64);

		Register(IRInstruction.MulSigned32, MulSigned32);
		Register(IRInstruction.MulSigned64, MulSigned64);

		Register(IRInstruction.Add32, Add32);
		Register(IRInstruction.Add64, Add64);
		Register(IRInstruction.AddCarryIn32, AddCarryIn32);

		Register(IRInstruction.SignExtend16x32, SignExtend16x32);
		Register(IRInstruction.SignExtend8x32, SignExtend8x32);
		Register(IRInstruction.SignExtend16x64, SignExtend16x64);
		Register(IRInstruction.SignExtend8x64, SignExtend8x64);
		Register(IRInstruction.SignExtend32x64, SignExtend32x64);

		Register(IRInstruction.ZeroExtend16x32, ZeroExtend16x32);
		Register(IRInstruction.ZeroExtend8x32, ZeroExtend8x32);
		Register(IRInstruction.ZeroExtend16x64, ZeroExtend16x64);
		Register(IRInstruction.ZeroExtend8x64, ZeroExtend8x64);
		Register(IRInstruction.ZeroExtend32x64, ZeroExtend32x64);

		Register(IRInstruction.RemUnsigned32, RemUnsigned32);
		Register(IRInstruction.RemUnsigned64, RemUnsigned64);

		Register(IRInstruction.IfThenElse32, IfThenElse32);
		Register(IRInstruction.IfThenElse64, IfThenElse64);
		Register(IRInstruction.NewString, NewString);
		Register(IRInstruction.NewObject, NewObject);
		Register(IRInstruction.NewArray, NewArray);

		Register(IRInstruction.DivUnsigned32, DivUnsigned32);
		Register(IRInstruction.DivUnsigned64, DivUnsigned64);

		// Any result

		//Register(IRInstruction.LoadParamSignExtend16x32, Any32);
		//Register(IRInstruction.LoadParamSignExtend16x64, Any64);
		//Register(IRInstruction.LoadParamSignExtend32x64, Any64);
		//Register(IRInstruction.LoadParamSignExtend8x32, Any32);
		//Register(IRInstruction.LoadParamSignExtend8x64, Any64);

		//Register(IRInstruction.LoadSignExtend16x32, Any32);
		//Register(IRInstruction.LoadSignExtend16x64, Any64);
		//Register(IRInstruction.LoadSignExtend32x64, Any64);
		//Register(IRInstruction.LoadSignExtend8x32, Any32);
		//Register(IRInstruction.LoadSignExtend8x64, Any64);

		// TODO:

		// DivSigned32	-- if known to be unsigned, then treat like DivUnsigned
		// DivSigned64	-- if known to be unsigned, then treat like DivUnsigned
		// RemSigned32	-- if known to be unsigned, then treat like RemUnsigned
		// RemSigned64	-- if known to be unsigned, then treat like RemUnsigned

		// AddCarryOut32
		// AddCarryOut64
		// AddCarryIn32
		// AddCarryIn64

		// Sub32
		// Sub64
		// SubCarryOut32
		// SubCarryOut64
		// SubCarryIn32
		// SubCarryIn64
	}

	private void Register(BaseInstruction instruction, NodeVisitationDelegate method)
	{
		visitation[instruction.ID] = method;
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

		Transform.SetLog(trace);

		EvaluateVirtualRegisters();

		UpdateInstructions();

		UpdateBranchInstructions();

		DumpValues();
	}

	private void DumpValues()
	{
		var valueTrace = CreateTraceLog("Values", 5);

		if (valueTrace == null)
			return;

		var count = MethodCompiler.VirtualRegisters.Count;

		for (var i = 0; i < count; i++)
		{
			var virtualRegister = MethodCompiler.VirtualRegisters[i];
			var value = virtualRegister.BitValue;

			valueTrace?.Log($"Virtual Register: {virtualRegister}");

			if (virtualRegister.IsDefinedOnce)
			{
				valueTrace?.Log($"Definition: {virtualRegister.Definitions[0]}");
			}

			valueTrace?.Log($"  MinValue:  {value.MinValue}");
			valueTrace?.Log($"  MaxValue:  {value.MaxValue}");
			valueTrace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
			valueTrace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
			valueTrace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");

			valueTrace?.Log();
		}
	}

	private void EvaluateVirtualRegisters()
	{
		var partial = 0;
		var last = 0;

		for (var i = 0; i < 10; i++)
		{
			foreach (var register in MethodCompiler.VirtualRegisters)
			{
				EvaluateBitValue(register, 10);

				if (!register.BitValue.IsStable)
					partial++;
			}

			if (partial == 0)
				return;

			if (partial == last)
				return;

			last = partial;
			partial = 0;
		}
	}

	private void UpdateInstructions()
	{
		foreach (var register in MethodCompiler.VirtualRegisters)
		{
			UpdateInstruction(register);
		}
	}

	private void EvaluateBitValue(Operand virtualRegister, int recursion = 0)
	{
		if (!IsBitTrackable(virtualRegister))
			return;

		if (virtualRegister.BitValue.IsResolved)
			return;

		var node = virtualRegister.Definitions[0];

		if (recursion > 0)
		{
			if (!node.Instruction.IsPhi)
			{
				foreach (var operand in node.Operands)
				{
					if (operand.BitValue.IsStable)
						continue;

					EvaluateBitValue(operand, recursion - 1);
				}
			}
		}

		if (virtualRegister.BitValue.IsResolved)
			return;

		var method = visitation[node.Instruction.ID];

		method?.Invoke(node);
	}

	private static bool IsBitTrackable(Operand operand)
	{
		if (operand.IsFloatingPoint)
			return false;

		if (!operand.IsDefinedOnce)
			return false;

		if (operand.IsInteger || operand.IsObject || operand.IsManagedPointer)
			return true;

		return false;
	}

	private void UpdateInstruction(Operand virtualRegister)
	{
		if (!IsBitTrackable(virtualRegister))
			return;

		var value = virtualRegister.BitValue;

		if (!value.IsResolved)
			return;

		var constantOperand = virtualRegister.IsInt32
			? Operand.CreateConstant32((uint)value.BitsSet)
			: Operand.CreateConstant64(value.BitsSet);

		if (trace != null)
		{
			trace?.Log($"Virtual Register: {virtualRegister}");

			trace?.Log($"  MaxValue:  {value.MaxValue}");
			trace?.Log($"  MinValue:  {value.MinValue}");

			if (value.BitsKnown != 0)
			{
				trace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
				trace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
				trace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");
			}
		}

		foreach (var node2 in virtualRegister.Uses.ToArray())
		{
			trace?.Log($"BEFORE:\t{node2}");

			node2.ReplaceOperand(virtualRegister, constantOperand);

			trace?.Log($"AFTER: \t{node2}");
			InstructionsUpdatedCount.Increment();
		}

		Debug.Assert(virtualRegister.Uses.Count == 0);

		var node = virtualRegister.Definitions[0];
		trace?.Log($"REMOVED:\t{node}");

		node.SetNop();
		trace?.Log();

		InstructionsRemovedCount.Increment();
	}

	private void UpdateBranchInstructions()
	{
		foreach (var block in BasicBlocks)
		{
			if (block.NextBlocks.Count != 2)
				continue;

			var node = block.BeforeLast;

			while (node.IsEmptyOrNop || node.Instruction == IRInstruction.Jmp)
			{
				node = node.Previous;
			}

			// Skip Switch instructions
			if (node.Instruction == IRInstruction.Switch)
				continue;

			Debug.Assert(node.Instruction.IsBranch);

			var value1 = node.Operand1.BitValue;
			var value2 = node.Operand2.BitValue;

			var result = EvaluateCompare(value1, value2, node.ConditionCode);

			if (!result.HasValue)
				continue;

			var newBranch = node.BranchTargets[0];

			trace?.Log($"REMOVED:\t{node}");
			node.SetNop();
			InstructionsRemovedCount.Increment();
			BranchesRemovedCount.Increment();

			if (!result.Value)
				continue;

			while (node.IsEmptyOrNop)
			{
				node = node.Next;
			}

			trace?.Log($"BEFORE:\t{node}");
			node.UpdateBranchTarget(0, newBranch);
			trace?.Log($"AFTER:\t{node}");
		}
	}

	private static bool? EvaluateCompare(BitValue value1, BitValue value2, ConditionCode condition)
	{
		switch (condition)
		{
			case ConditionCode.Equal:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet == value2.BitsSet;
				}
				else if (value1.MaxValue == value1.MinValue && value1.MaxValue == value2.MaxValue && value1.MinValue == value2.MinValue)
				{
					return true;
				}
				else if (((value1.BitsSet & value2.BitsSet) != value1.BitsSet || (value1.BitsClear & value2.BitsClear) != value1.BitsClear) && !value1.AreAnyBitsKnown && !value2.AreAnyBitsKnown)
				{
					return false;
				}
				else if ((value1.BitsSet & value2.BitsClear) != 0 || (value2.BitsSet & value1.BitsClear) != 0)
				{
					return false;
				}
				else if (value1.MaxValue < value2.MinValue)
				{
					return false;
				}
				else if (value1.MinValue > value2.MaxValue)
				{
					return false;
				}
				break;

			case ConditionCode.NotEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet != value2.BitsSet;
				}
				else if (value1.MaxValue == value1.MinValue && value1.MaxValue == value2.MaxValue && value1.MinValue == value2.MinValue)
				{
					return false;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value2.AreAll64BitsKnown && value2.MaxValue == 0 && value1.BitsSet != 0)
				{
					return true;
				}
				else if ((value1.BitsSet & value2.BitsClear) != 0 || (value2.BitsSet & value1.BitsClear) != 0)
				{
					return true;
				}
				else if (value1.MaxValue < value2.MinValue)
				{
					return true;
				}
				else if (value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				break;

			case ConditionCode.UnsignedGreater:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet > value2.BitsSet;
				}
				else if (value2.AreAll64BitsKnown && value2.MaxValue == 0 && value1.BitsSet != 0)
				{
					return true;
				}
				else if (value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				else if (value1.MaxValue <= value2.MinValue)
				{
					return false;
				}
				break;

			case ConditionCode.UnsignedLess:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.MaxValue < value2.MaxValue;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value2.MinValue > value1.MaxValue)
				{
					return true;
				}
				else if (value2.MaxValue <= value1.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.UnsignedGreaterOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet <= value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value1.MinValue >= value2.MaxValue)
				{
					return true;
				}
				else if (value1.MaxValue < value2.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.UnsignedLessOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					return value1.BitsSet <= value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value1.MaxValue == 0 && value2.BitsSet != 0)
				{
					return true;
				}
				else if (value2.MinValue >= value1.MaxValue)
				{
					return true;
				}
				else if (value2.MaxValue < value1.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.Greater:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet > (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet > (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue < value2.MinValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue > value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue < value2.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.Less:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet < (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet < (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue < value2.MinValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue > value2.MaxValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue < value2.MinValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue > value2.MaxValue)
				{
					return false;
				}

				break;

			case ConditionCode.GreaterOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet >= (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet >= (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue >= value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue <= value2.MinValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue >= value2.MaxValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue <= value2.MinValue)
				{
					return false;
				}

				break;

			case ConditionCode.LessOrEqual:
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is64Bit && value2.Is64Bit)
				{
					return (long)value1.BitsSet <= (long)value2.BitsSet;
				}
				else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit && value2.Is32Bit)
				{
					return (int)value1.BitsSet <= (int)value2.BitsSet;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MaxValue <= value2.MinValue)
				{
					return true;
				}
				else if (value1.Is32Bit && value2.Is32Bit && value1.IsSignBitClear32 && value2.IsSignBitClear32 && value1.MinValue >= value2.MaxValue)
				{
					return false;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MaxValue <= value2.MinValue)
				{
					return true;
				}
				else if (value1.Is64Bit && value2.Is64Bit && value1.IsSignBitClear64 && value2.IsSignBitClear64 && value1.MinValue >= value2.MaxValue)
				{
					return false;
				}

				break;

			default:
				return null;
		}

		return null;
	}

	#region IR Instructions

	private static void Add32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 + value2.BitsSet32);
		}
		else if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue)
			|| value1.MaxValue > uint.MaxValue
			|| value2.MaxValue > uint.MaxValue)
		{
			result.SetStable(value1, value2); // Indeterminate, but stable
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MinValue, (uint)value2.MinValue)
			|| value1.MaxValue > uint.MaxValue
			|| value2.MaxValue > uint.MaxValue)
		{
			result.SetStable(value1, value2); // Indeterminate, but stable
		}
		else
		{
			result
				.NarrowMin(value1.MinValue + value2.MinValue)
				.NarrowMax(value1.MaxValue + value2.MaxValue)
				.SetStable(value1, value2);
		}
	}

	private static void Add64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet32 + value2.BitsSet32);
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry(value1.MaxValue, value2.MaxValue))
		{
			result.SetStable(value1, value2); // Indeterminate, but stable
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry(value1.MinValue, value2.MinValue))
		{
			result.SetStable(value1, value2); // Indeterminate, but stable
		}
		else if (!IntegerTwiddling.IsAddUnsignedCarry(value1.MaxValue, value2.MaxValue))
		{
			result
				.NarrowMin(value1.MinValue + value2.MinValue)
				.NarrowMax(value1.MaxValue + value2.MaxValue)
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void AddCarryIn32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;
		var value3 = node.Operand3.BitValue;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && value3.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 + value2.BitsSet32 + value3.BitsSet32);
		}
		else if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0 && value3.AreLower32BitsKnown && value3.BitsSet32 == 0)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0 && value3.AreLower32BitsKnown && value3.BitsSet32 == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2, value3);
		}
	}

	private static void ArithShiftRight32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var shift = (int)(value2.BitsSet & 0b11111);
		var knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;
		var signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;
		var highbits = knownSignedBit && signed ? ~(~uint.MaxValue >> shift) : 0;

		if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
		{
			result.SetValue(value1.BitsSet32 >> shift | highbits);
		}
		else if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreLower5BitsKnown && knownSignedBit && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower5BitsKnown && knownSignedBit && shift != 0)
		{
			result
				.NarrowRange(
					(value1.MinValue >> shift) | highbits,
					(value1.MaxValue >> shift) | highbits)
				.NarrowBits(
					value1.BitsSet >> shift | highbits,
					Upper32BitsSet | (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | highbits)
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void ArithShiftRight64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var shift = (int)(value2.BitsSet & 0b111111);
		var knownSignedBit = ((value1.BitsKnown >> 63) & 1) == 1;
		var signed = ((value1.BitsSet >> 63) & 1) == 1 || ((value1.BitsClear >> 63) & 1) != 1;
		var highbits = knownSignedBit && signed ? ~(~ulong.MaxValue >> shift) : 0;

		if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			result.SetValue(value1.BitsSet >> shift | highbits);
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && knownSignedBit && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower6BitsKnown && knownSignedBit && shift != 0)
		{
			result
				.NarrowRange(
					(value1.MinValue >> shift) | highbits,
					(value1.MaxValue >> shift) | highbits)
				.NarrowBits(
					value1.BitsSet >> shift | highbits,
					(value1.BitsClear >> shift) | ~(ulong.MaxValue >> shift) | highbits)
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void Compare32x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var compare = EvaluateCompare(value1, value2, node.ConditionCode);

		if (compare.HasValue)
		{
			result.SetValue(compare.Value);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void Compare64x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var compare = EvaluateCompare(value1, value2, node.ConditionCode);

		if (compare.HasValue)
		{
			result.SetValue(compare.Value);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void Compare64x64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var compare = EvaluateCompare(value1, value2, node.ConditionCode);

		if (compare.HasValue)
		{
			result.SetValue(compare.Value);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void GetHigh32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreUpper32BitsKnown)
		{
			result.SetValue(value1.BitsSet >> 32);
		}
		else if (value1.MaxValue <= uint.MaxValue)
		{
			result.SetValue(0);
		}
		else
		{
			result
				.NarrowRange(
					value1.MinValue >> 32,
					value1.MaxValue >> 32)
				.NarrowBits(value1.BitsSet >> 32,
					Upper32BitsSet | (value1.BitsClear >> 32)
				).SetStable(value1);
		}
	}

	private static void GetLow32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet & uint.MaxValue);
		}
		else
		{
			result
				.NarrowMax(uint.MaxValue)
				.NarrowBits(
					value1.BitsSet & uint.MaxValue,
					value1.BitsClear & uint.MaxValue
				).SetStable(value1);
		}
	}

	private static void LoadParamZeroExtend16x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowBits(
				0,
				~(ulong)ushort.MaxValue
			);
	}

	private static void LoadParamZeroExtend16x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowBits(
			0,
			~(ulong)ushort.MaxValue
		);
	}

	private static void LoadParamZeroExtend8x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowBits(
			0,
			~(ulong)byte.MaxValue
		);
	}

	private static void LoadParamZeroExtend8x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowBits(
				0,
				~(ulong)byte.MaxValue
				);
	}

	private static void LoadParamZeroExtend32x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(uint.MaxValue)
			.NarrowBits(
				0,
				~uint.MaxValue
				);
	}

	private static void LoadZeroExtend16x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowBits(
			0,
			~(ulong)ushort.MaxValue
		);
	}

	private static void LoadZeroExtend16x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowBits(
			0,
			~(ulong)ushort.MaxValue
		);
	}

	private static void LoadZeroExtend8x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowBits(
			0,
			~(ulong)byte.MaxValue
		);
	}

	private static void LoadZeroExtend8x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowBits(
			0,
			~(ulong)byte.MaxValue
		);
	}

	private static void LoadZeroExtend32x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(uint.MaxValue)
			.NarrowBits(
			0,
			~uint.MaxValue
		);
	}

	private static void And32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreLower32BitsKnown && (value2.BitsSet & ulong.MaxValue) == 0)
		{
			result.SetValue(0);
		}
		else
		{
			result
				.NarrowRange(
					Math.Min(value1.MinValue, value2.MinValue),
					Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowBits(
					value1.BitsSet & value2.BitsSet,
					value2.BitsClear | value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	private static void And64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else
		{
			result
				.NarrowRange(
					Math.Min(value1.MinValue, value2.MinValue),
					Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowBits(
					value1.BitsSet & value2.BitsSet,
					value2.BitsClear | value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	private static void Not32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower32BitsKnown)
		{
			result.SetValue(~value1.BitsSet32);
		}
		else
		{
			result.NarrowBits(
				 value1.BitsClear32,
				 value1.BitsSet32
			).SetStable(value1);
		}
	}

	private static void Not64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreAll64BitsKnown)
		{
			result.SetValue(~value1.BitsSet);
		}
		else
		{
			result
				.NarrowBits(
					 value1.BitsClear,
					 value1.BitsSet)
				.SetStable(value1);
		}
	}

	private static void Or32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
		{
			result.SetValue(value1);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == uint.MaxValue)
		{
			result.SetValue(value2);
		}
		else
		{
			result
				.NarrowRange(
					Math.Min(value1.MinValue, value2.MinValue),
					Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowBits(
					value1.BitsSet | value2.BitsSet,
					value2.BitsClear & value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	private static void Or64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreAll64BitsKnown && value1.BitsSet == ulong.MaxValue)
		{
			result.SetValue(value1);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == ulong.MaxValue)
		{
			result.SetValue(value2);
		}
		else
		{
			result
				.NarrowRange(
					Math.Min(value1.MinValue, value2.MinValue),
					Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowBits(
					value1.BitsSet | value2.BitsSet,
					value2.BitsClear & value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	private static void Xor32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var bitsKnown = value1.BitsKnown & value2.BitsKnown & uint.MaxValue;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue((value1.BitsSet32 ^ value2.BitsSet32) & uint.MaxValue);
		}
		else
		{
			result
				.NarrowBits(
					(value1.BitsSet ^ value2.BitsSet) & bitsKnown,
					(value2.BitsClear ^ value1.BitsClear) & bitsKnown)
				.SetStable(value1, value2);
		}
	}

	private static void Xor64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var bitsKnown = value1.BitsKnown & value2.BitsKnown;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet ^ value2.BitsSet);
		}
		else
		{
			result
				.NarrowBits(
					 (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
					 (value2.BitsClear ^ value1.BitsClear) & bitsKnown)
				.SetStable(value1, value2);
		}
	}

	private static void Move32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		result.Narrow(value1).SetStable();
	}

	private static void Move64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		result.Narrow(value1).SetStable();
	}

	private static void MulSigned32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		// FUTURE: Special power of two handling for bits, handle similar to shift left

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue((ulong)((int)value1.BitsSet32 * (int)value2.BitsSet32));
		}
		else if (value1.AreLower32BitsKnown && value1.BitsSet32 == 1)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 1)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (!IntegerTwiddling.HasSignBitSet((int)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MinValue)
			&& !IntegerTwiddling.IsMultiplySignedOverflow((int)value1.MaxValue, (int)value2.MaxValue))
		{
			var max = Math.Max(value1.MaxValue, value2.MaxValue);
			var min = Math.Min(value1.MinValue, value2.MinValue);
			var uppermax = max * max;

			result
				.NarrowRange(
					(uint)(min * min),
					(uint)(max * max))
				.NarrowBits(
					0,
					Upper32BitsSet | BitTwiddling.GetBitsOver((uint)uppermax))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void MulSigned64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		// TODO: Special power of two handling for bits, handle similar to shift left

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue((ulong)((long)value1.BitsSet * (long)value2.BitsSet));
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 1)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 1)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (!IntegerTwiddling.HasSignBitSet((long)value1.MaxValue)
				&& !IntegerTwiddling.HasSignBitSet((long)value2.MaxValue)
				&& !IntegerTwiddling.HasSignBitSet((long)value1.MinValue)
				&& !IntegerTwiddling.HasSignBitSet((long)value2.MinValue)
				&& !IntegerTwiddling.IsMultiplySignedOverflow((long)value1.MaxValue, (long)value2.MaxValue)
				&& !IntegerTwiddling.IsMultiplySignedOverflow((long)value1.MinValue, (long)value2.MinValue))
		{
			var max = Math.Max(value1.MaxValue, value2.MaxValue);
			var min = Math.Min(value1.MinValue, value2.MinValue);
			var uppermax = max * max;

			result
				.NarrowBits(
					0,
					BitTwiddling.GetBitsOver(uppermax))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void MulUnsigned32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		// FUTURE: Special power of two handling for bits, handle similar to shift left

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 * value2.BitsSet32);
		}
		else if (value1.AreLower32BitsKnown && value1.BitsSet32 == 1)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 1)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (IntegerTwiddling.IsMultiplyUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue))
		{
			result
				.NarrowBits(
					0,
					Upper32BitsSet | BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue))
				.SetStable(value1, value2);
		}
		else
		{
			result
				.NarrowRange(
					(uint)(value1.MinValue * value2.MinValue),
					(uint)(value1.MaxValue * value2.MaxValue))
				.NarrowBits(
					0,
					Upper32BitsSet | BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue))
				.SetStable(value1, value2);
		}
	}

	private static void MulUnsigned64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		// FUTURE: Special power of two handling for bits, handle similar to shift left

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet * value2.BitsSet);
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 1)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 1)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (!IntegerTwiddling.IsMultiplyUnsignedOverflow(value1.MaxValue, value2.MaxValue)
			&& !IntegerTwiddling.IsMultiplyUnsignedOverflow(value1.MinValue, value2.MinValue))
		{
			result
				.NarrowRange(
					value1.MinValue * value2.MinValue,
					value1.MaxValue * value2.MaxValue)
				.NarrowBits(
					0,
					BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void Phi32(Node node)
	{
		Debug.Assert(node.OperandCount != 0);

		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var max = value1.MaxValue;
		var min = value1.MinValue;
		var bitsset = value1.BitsSet;
		var bitsclear = value1.BitsClear;
		var stable = node.Operand1.BitValue.IsStable;

		for (var i = 1; i < node.OperandCount; i++)
		{
			var value = node.GetOperand(i).BitValue;

			max = Math.Max(max, value.MaxValue);
			min = Math.Min(min, value.MinValue);
			bitsset &= value.BitsSet;
			bitsclear &= value.BitsClear;
			stable &= value.IsStable;
		}

		result
			.NarrowMin(min)
			.NarrowMax(max)
			.NarrowSetBits(bitsset)
			.NarrowClearBits(bitsclear)
			.SetStable(stable);
	}

	private static void Phi64(Node node)
	{
		Debug.Assert(node.OperandCount != 0);

		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var max = value1.MaxValue;
		var min = value1.MinValue;
		var bitsset = value1.BitsSet;
		var bitsclear = value1.BitsClear;
		var stable = node.Operand1.BitValue.IsStable;

		for (var i = 1; i < node.OperandCount; i++)
		{
			var value = node.GetOperand(i).BitValue;

			max = Math.Max(max, value.MaxValue);
			min = Math.Min(min, value.MinValue);
			bitsset &= value.BitsSet;
			bitsclear &= value.BitsClear;

			stable &= value.IsStable;
		}

		result
			.NarrowMin(min)
			.NarrowMax(max)
			.NarrowSetBits(bitsset)
			.NarrowClearBits(bitsclear)
			.SetStable(stable);
	}

	private static void RemUnsigned32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			// divide by zero!
			return;
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 % value2.BitsSet32);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
		{
			result
				.NarrowMax(value2.BitsSet - 1)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.BitsSet - 1))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void RemUnsigned64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			// divide by zero!
			return;
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet % value2.BitsSet);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet32 != 0)
		{
			result
				.NarrowMax(value2.BitsSet - 1)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.BitsSet - 1))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void ShiftLeft32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var shift = (int)(value2.BitsSet & 0b11111);
		var lowest = BitTwiddling.CountTrailingZeros(value1.BitsSet | value1.BitsUnknown);

		// FUTURE: if the value2 has any set bits, the lower bound could be raised

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
		{
			result.SetValue(value1.BitsSet32 << shift);
		}
		else if (value2.AreLower5BitsKnown && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower5BitsKnown && shift != 0)
		{
			result
				.NarrowRange(
					 value1.MinValue << shift > value1.MinValue ? value1.MinValue << shift : 0,
					 value1.MaxValue << shift < uint.MaxValue ? value1.MaxValue << shift : ulong.MaxValue)
				.NarrowBits(
					 value1.BitsSet << shift,
					 Upper32BitsSet | (value1.BitsClear << shift) | ~(ulong.MaxValue << shift))
				.SetStable(value1, value2);
		}
		else if (value1.AreAnyBitsKnown && lowest >= 1)
		{
			var low = (ulong)(1 << lowest);

			result
				.NarrowMin(low)
				.NarrowClearBits(low - 1)
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void ShiftLeft64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var shift = (int)(value2.BitsSet & 0b111111);

		// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			result.SetValue(value1.BitsSet32 << shift);
		}
		else if (value2.AreLower6BitsKnown && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower6BitsKnown && shift != 0)
		{
			result
				.NarrowRange(
					 value1.MinValue << shift > value1.MinValue ? value1.MinValue << shift : 0,
					 value1.MaxValue << shift > value1.MaxValue ? value1.MaxValue << shift : ulong.MaxValue)
				.NarrowBits(
					 value1.BitsSet << shift,
					 (value1.BitsClear << shift) | ~(ulong.MaxValue << shift))
				.SetStable(value1, value2);
		}
		else if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 0)
		{
			// REVIEW: May not be useful
			result.NarrowClearBits(uint.MaxValue).SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void ShiftRight32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var shift = (int)(value2.BitsSet & 0b11111);

		// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively
		// FUTURE: If the shift has a range, and some bits are known --- then some bits might be determinable

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
		{
			result.SetValue(value1.BitsSet32 >> shift);
		}
		else if (value2.AreLower5BitsKnown && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower5BitsKnown && shift != 0)
		{
			result
				.NarrowRange(
					value1.MinValue >> shift,
					value1.MaxValue >> shift)
				.NarrowBits(
					value1.BitsSet >> shift,
					Upper32BitsSet | (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void ShiftRight64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		var shift = (int)(value2.BitsSet & 0b111111);

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			result.SetValue(value1.BitsSet >> shift);
		}
		else if (value2.AreLower6BitsKnown && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower6BitsKnown && shift != 0)
		{
			result
				.NarrowRange(
					value1.MinValue >> shift,
					value1.MaxValue >> shift)
				.NarrowBits(
					value1.BitsSet >> shift,
					value1.BitsClear >> shift | ~(ulong.MaxValue >> shift))
				.SetStable(value1);
		}
		else if (value1.AreUpper32BitsKnown && value1.BitsSet >> 32 == 0)
		{
			result
				.NarrowMax(uint.MaxValue)
				.NarrowBits(0, ~uint.MaxValue)
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void SignExtend16x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;
		var signed = ((value1.BitsSet >> 15) & 1) == 1 || ((value1.BitsClear >> 15) & 1) != 1;

		if (value1.AreLower16BitsKnown)
		{
			result.SetValue(value1.BitsSet16 | (((value1.BitsSet >> 15) & 1) == 1 ? Upper48BitsSet : 0));
		}
		else if (!knownSignedBit)
		{
			result
				.NarrowBits(
					value1.BitsSet16,
					value1.BitsClear16)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowBits(
					value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					value1.BitsClear16 | (signed ? 0 : Upper48BitsSet))
				.SetStable(value1);
		}
	}

	private static void SignExtend16x64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;
		var signed = ((value1.BitsSet >> 15) & 1) == 1 || ((value1.BitsClear >> 15) & 1) != 1;

		if (value1.AreLower16BitsKnown)
		{
			result.SetValue(value1.BitsSet16 | (((value1.BitsSet >> 15) & 1) == 1 ? Upper48BitsSet : 0));
		}
		else if (!knownSignedBit)
		{
			result
				.NarrowBits(
					value1.BitsSet16,
					value1.BitsClear16)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowBits(
					 value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					 value1.BitsClear16 | (signed ? 0 : Upper48BitsSet))
				.SetStable(value1);
		}
	}

	private static void SignExtend32x64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;
		var signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;

		if (value1.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 | (((value1.BitsSet >> 31) & 1) == 1 ? Upper32BitsSet : 0));
		}
		else if (!knownSignedBit)
		{
			result
				.NarrowBits(
					 value1.BitsSet32,
					 value1.BitsClear32)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowBits(
					value1.BitsSet32 | (signed ? Upper56BitsSet : 0),
					 value1.BitsClear32 | (signed ? 0 : Upper56BitsSet))
				.SetStable(value1);
		}
	}

	private static void SignExtend8x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;
		var signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;

		if (value1.AreLower8BitsKnown)
		{
			result.SetValue(value1.BitsSet16 | (((value1.BitsSet >> 7) & 1) == 1 ? Upper56BitsSet : 0));
		}
		else if (!knownSignedBit)
		{
			result
				.NarrowBits(
					 value1.BitsSet8,
					 value1.BitsClear8)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowBits(
					 value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					 value1.BitsClear8 | (signed ? 0 : Upper56BitsSet))
				.SetStable(value1);
		}
	}

	private static void SignExtend8x64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		var signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;
		var knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

		if (value1.AreLower8BitsKnown)
		{
			result.SetValue(value1.BitsSet16 | (((value1.BitsSet >> 7) & 1) == 1 ? Upper56BitsSet : 0));
		}
		else if (!knownSignedBit)
		{
			result
				.NarrowBits(
					value1.BitsSet8,
					value1.BitsClear8)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowBits(
					value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					value1.BitsClear8 | (signed ? 0 : Upper56BitsSet))
				.SetStable(value1);
		}
	}

	private static void To64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value2.MaxValue << 32 | (value1.MaxValue & uint.MaxValue));
		}
		else
		{
			result
				.NarrowRange(
					(value2.MinValue << 32) | (value1.MinValue & uint.MaxValue),
					(value2.MaxValue << 32) | (value1.MaxValue & uint.MaxValue))
				.NarrowBits(
					(value2.BitsSet << 32) | value1.BitsSet32,
					(value2.BitsClear << 32) | value1.BitsClear32)
				.SetStable(value1, value2);
		}
	}

	private static void Truncate64x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		result
			.NarrowRange(
				value1.MinValue > uint.MaxValue ? 0 : value1.MinValue,
				Math.Min(uint.MaxValue, value1.MaxValue))
				.NarrowBits(
				value1.BitsSet & uint.MaxValue,
				Upper32BitsSet | value1.BitsClear)
			.SetStable(value1);
	}

	private static void ZeroExtend16x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower16BitsKnown)
		{
			result.SetValue(value1.BitsSet16);
		}
		else
		{
			result
				.NarrowRange(
					value1.MinValue > byte.MaxValue ? 0 : value1.MinValue,
					Math.Min(byte.MaxValue, value1.MaxValue))
				.NarrowBits(
					value1.BitsSet16,
					value1.BitsClear | Upper48BitsSet)
				.SetStable(value1);
		}
	}

	private static void ZeroExtend16x64(Node node)
	{
		ZeroExtend16x32(node);
	}

	private static void ZeroExtend32x64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32);
		}
		else
		{
			result
				.NarrowRange(
					value1.MinValue > uint.MaxValue ? 0 : value1.MinValue,
					Math.Min(uint.MaxValue, value1.MaxValue))
				.NarrowBits(
					value1.BitsSet32,
					Upper32BitsSet | value1.BitsClear)
				.SetStable(value1);
		}
	}

	private static void ZeroExtend8x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower8BitsKnown)
		{
			result.SetValue(value1.BitsSet8);
		}
		else
		{
			result
				.NarrowRange(
					value1.MinValue > byte.MaxValue ? 0 : value1.MinValue,
					Math.Min(byte.MaxValue, value1.MaxValue))
				.NarrowBits(
					value1.BitsSet8,
					value1.BitsClear | Upper56BitsSet)
				.SetStable(value1);
		}
	}

	private static void ZeroExtend8x64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower8BitsKnown)
		{
			result.SetValue(value1.BitsSet8);
		}
		else
		{
			result
				.NarrowRange(
					value1.MinValue > byte.MaxValue ? 0 : value1.MinValue,
					Math.Min(byte.MaxValue, value1.MaxValue))
				.NarrowBits(
					value1.BitsSet8,
					value1.BitsClear | Upper56BitsSet)
				.SetStable(value1);
		}
	}

	private static void IfThenElse32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand2.BitValue;
		var value2 = node.Operand3.BitValue;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result
				.NarrowRange(
					Math.Min(value1.MinValue, value2.MinValue),
					Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowBits(
					value1.MaxValue & value2.MaxValue,
					~value1.MaxValue & ~value2.MaxValue)
				.SetStable();
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void IfThenElse64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand2.BitValue;
		var value2 = node.Operand3.BitValue;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result
				.NarrowRange(
					Math.Min(value1.MinValue, value2.MinValue),
					Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowBits(
					value1.MaxValue & value2.MaxValue,
					~value1.MaxValue & ~value2.MaxValue)
				.SetStable();
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	private static void NewString(Node node)
	{
		var result = node.Result.BitValue;

		result.SetNotNull();
	}

	private static void NewObject(Node node)
	{
		var result = node.Result.BitValue;

		result.SetNotNull();
	}

	private static void NewArray(Node node)
	{
		var result = node.Result.BitValue;

		result.SetNotNull();
	}

	private static void DivUnsigned32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
		{
			result.SetValue(value1.BitsSet32 / value2.BitsSet32);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			// divide by zero!
			return;
		}
		else
		{
			result
				.NarrowRange(
					value2.MaxValue == 0 ? 0 : value1.MinValue / value2.MaxValue,
					value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue)
				.NarrowBits(
					0,
					Upper32BitsSet | BitTwiddling.GetBitsOver(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue))
				.SetStable(value1, value2);
		}
	}

	private static void DivUnsigned64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;
		var value2 = node.Operand2.BitValue;

		if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			// divide by zero!
			return;
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value2.BitsSet32 != 0)
		{
			result.SetValue(value1.BitsSet / value2.BitsSet);
		}
		else
		{
			result
				.NarrowRange(
					value2.MaxValue == 0 ? 0 : value1.MinValue / value2.MaxValue,
					value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue)
				.NarrowBits(
					0,
					BitTwiddling.GetBitsOver(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue))
				.SetStable(value1, value2);
		}
	}

	#endregion IR Instructions
}
