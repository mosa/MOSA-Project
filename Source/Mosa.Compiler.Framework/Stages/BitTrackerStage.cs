// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Diagnostics;
using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Analysis;
using Mosa.Compiler.Framework.Trace;
using Mosa.Compiler.Framework.Transforms;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Bit Tracker Stage
/// </summary>
public sealed class BitTrackerStage : BaseMethodCompilerStage
{
	// This stage propagates bit and value range knowledge thru the various operations. This additional knowledge may enable additional optimizations opportunities.

	private const int MaxInstructions = 1024;

	private const ulong Upper32BitsSet = ~(ulong)uint.MaxValue;
	private const ulong Upper48BitsSet = ~(ulong)ushort.MaxValue;
	private const ulong Upper56BitsSet = ~(ulong)byte.MaxValue;

	private readonly Counter BranchesRemovedCount = new Counter("BitTrackerStage.BranchesRemoved");
	private readonly Counter InstructionsRemovedCount = new Counter("BitTrackerStage.InstructionsRemoved");
	private readonly Counter InstructionsUpdatedCount = new Counter("BitTrackerStage.InstructionsUpdated");
	private TraceLog trace;

	private NodeVisitationDelegate[] visitation = new NodeVisitationDelegate[MaxInstructions];

	private delegate BitValue NodeVisitationDelegate(InstructionNode node, TransformContext transform);

	private delegate (BitValue, BitValue) NodeVisitationDelegate2(InstructionNode node, TransformContext transform);

	private TransformContext TransformContext;

	private BitValueManager BitValueManager;

	protected override void Finish()
	{
		trace = null;
		TransformContext = null;
		BitValueManager = null;
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

		Register(IRInstruction.LoadParamSignExtend16x32, Any32);
		Register(IRInstruction.LoadParamSignExtend16x64, Any64);
		Register(IRInstruction.LoadParamSignExtend32x64, Any64);
		Register(IRInstruction.LoadParamSignExtend8x32, Any32);
		Register(IRInstruction.LoadParamSignExtend8x64, Any64);

		Register(IRInstruction.LoadSignExtend16x32, Any32);
		Register(IRInstruction.LoadSignExtend16x64, Any64);
		Register(IRInstruction.LoadSignExtend32x64, Any64);
		Register(IRInstruction.LoadSignExtend8x32, Any32);
		Register(IRInstruction.LoadSignExtend8x64, Any64);

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

		BitValueManager = new BitValueManager(Is32BitPlatform);

		TransformContext = new TransformContext(MethodCompiler, BitValueManager);

		TransformContext.SetLog(trace);

		EvaluateVirtualRegisters();

		UpdateBranchInstructions();

		DumpValues();

		if (CompilerSettings.FullCheckMode)
			CheckAllPhiInstructions();
	}

	private void DumpValues()
	{
		var valueTrace = CreateTraceLog("Values", 5);

		if (valueTrace == null)
			return;

		int count = MethodCompiler.VirtualRegisters.Count;

		for (int i = 0; i < count; i++)
		{
			var virtualRegister = MethodCompiler.VirtualRegisters[i];
			var value = BitValueManager.GetBitValue(virtualRegister);

			valueTrace?.Log($"Virtual Register: {virtualRegister}");

			if (virtualRegister.Definitions.Count == 1)
			{
				valueTrace?.Log($"Definition: {virtualRegister.Definitions[0]}");
			}

			if (value == null)
			{
				valueTrace?.Log($"*** INDETERMINATE");
				continue;
			}

			valueTrace?.Log($"  MaxValue:  {value.MaxValue}");
			valueTrace?.Log($"  MinValue:  {value.MinValue}");
			valueTrace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
			valueTrace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
			valueTrace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");

			valueTrace?.Log();
		}
	}

	private void EvaluateVirtualRegisters()
	{
		bool change = true;

		while (change)
		{
			change = false;

			foreach (var register in MethodCompiler.VirtualRegisters)
			{
				change |= Evaluate(register, TransformContext);
			}
		}
	}

	private bool Evaluate(Operand virtualRegister, TransformContext transform)
	{
		var value = BitValueManager.GetBitValue(virtualRegister);

		if (value != null)
			return false;

		value = EvaluateBitValue(virtualRegister);

		if (value == null)
			return false;

		BitValueManager.UpdateBitValue(virtualRegister, value);

		UpdateInstruction(virtualRegister, value);

		return true;
	}

	private BitValue EvaluateBitValue(Operand virtualRegister)
	{
		var value = BitValueManager.GetBitValue(virtualRegister);

		if (!IsBitTrackable(virtualRegister))
			return null;

		if (virtualRegister.Definitions.Count != 1)
			return Any(virtualRegister);

		var node = virtualRegister.Definitions[0];

		Debug.Assert(node.ResultCount != 0);

		if (!IsBitTrackable(node.Result))
			return Any(virtualRegister);

		if (node.Instruction.FlowControl == FlowControl.Call)
			return Any(virtualRegister);

		var method = visitation[node.Instruction.ID];

		if (method == null)
			return Any(virtualRegister);

		value = method.Invoke(node, TransformContext);

		return value;
	}

	private static bool IsBitTrackable(Operand virtualRegister)
	{
		if (virtualRegister.IsFloatingPoint)
			return false;

		if (virtualRegister.IsInteger || virtualRegister.IsReferenceType || virtualRegister.IsPointer)
			return true;

		//if (virtualRegister.IsValueType && virtualRegister.FitsIntegerRegister)
		//	return true;

		return false;
	}

	private BitValue Any(Operand virtualRegister)
	{
		if (virtualRegister.IsInteger)
			return virtualRegister.IsInteger32 ? BitValue.Any32 : BitValue.Any64;
		else if (virtualRegister.IsReferenceType || virtualRegister.IsPointer)
			return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		else if (virtualRegister.IsR4)
			return BitValue.Any32;
		else if (virtualRegister.IsR8)
			return BitValue.Any64;

		//else if (virtualRegister.IsValueType)
		//{
		//	if (virtualRegister.FitsNativeSizeRegister)
		//		return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		//	else
		//		return virtualRegister.IsInteger32 ? BitValue.Any32 : BitValue.Any64;
		//}
		throw new InvalidProgramException();
	}

	private static BitValue Any(Operand virtualRegister, TransformContext transform)
	{
		if (virtualRegister.IsInteger)
			return virtualRegister.IsInteger32 ? BitValue.Any32 : BitValue.Any64;
		else if (virtualRegister.IsReferenceType || virtualRegister.IsPointer)
			return transform.Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		else if (virtualRegister.IsR4)
			return BitValue.Any32;
		else if (virtualRegister.IsR8)
			return BitValue.Any64;

		//else if (virtualRegister.IsValueType)
		//{
		//	if (virtualRegister.FitsNativeSizeRegister)
		//		return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
		//	else
		//		return virtualRegister.IsInteger32 ? BitValue.Any32 : BitValue.Any64;
		//}
		throw new InvalidProgramException();
	}

	private void UpdateInstruction(Operand virtualRegister, BitValue value)
	{
		Debug.Assert(!virtualRegister.IsFloatingPoint);

		//Debug.Assert(virtualRegister.Definitions.Count == 1);

		if (virtualRegister.Definitions.Count != 1)
			return;

		ulong replaceValue;

		if (value.AreAll64BitsKnown)
		{
			replaceValue = value.BitsSet;
		}
		else if (value.MaxValue == value.MinValue)
		{
			replaceValue = value.MaxValue;
		}
		else if (virtualRegister.IsInteger32 && value.AreLower32BitsKnown)
		{
			replaceValue = value.BitsSet32;
		}
		else
		{
			return;
		}

		var constantOperand = CreateConstant(virtualRegister.Type, replaceValue);

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

			for (int i = 0; i < node2.OperandCount; i++)
			{
				if (node2.GetOperand(i) == virtualRegister)
				{
					node2.SetOperand(i, constantOperand);
				}
			}

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

			Debug.Assert(node.Instruction == IRInstruction.Branch32 || node.Instruction == IRInstruction.Branch64 || node.Instruction == IRInstruction.BranchObject);

			var value1 = TransformContext.GetBitValue(node.Operand1);
			var value2 = TransformContext.GetBitValue(node.Operand2);

			if (value1 == null || value2 == null)
				continue;

			var result = EvaluateCompare(value1, value2, node.ConditionCode, TransformContext);

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

	private static bool? EvaluateCompare(BitValue value1, BitValue value2, ConditionCode condition, TransformContext transform)
	{
		if (value1 == null || value2 == null)
			return null;

		//Debug.Assert(value1.Is32Bit == value2.Is32Bit);

		//var bitsSet1 = value1.BitsSet;
		//var bitsClear1 = value1.BitsClear;
		//var maxValue1 = value1.MaxValue;
		//var minValue1 = value1.MinValue;

		//var bitsSet2 = value2.BitsSet;
		//var bitsClear2 = value2.BitsClear;
		//var maxValue2 = value2.MaxValue;
		//var minValue2 = value2.MinValue;

		//var areAll64BitsKnown1 = value1.AreAll64BitsKnown;
		//var areAll64BitsKnown2 = value2.AreAll64BitsKnown;

		//var areAnyBitsKnown1 = value1.AreAnyBitsKnown;
		//var areAnyBitsKnown2 = value2.AreAnyBitsKnown;

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
				else if (((value1.BitsSet & value2.BitsClear) != 0) || ((value2.BitsSet & value1.BitsClear) != 0))
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
				else if (((value1.BitsSet & value2.BitsClear) != 0) || ((value2.BitsSet & value1.BitsClear) != 0))
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

	private static BitValue Add32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 + value2.BitsSet32, true);
		}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return value2;
		}

		if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			return value1;
		}

		if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue) || value1.MaxValue > uint.MaxValue || value2.MaxValue > uint.MaxValue)
		{
			return BitValue.Any32;
		}

		if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MinValue, (uint)value2.MinValue) || value1.MaxValue > uint.MaxValue || value2.MaxValue > uint.MaxValue)
		{
			return BitValue.Any32;
		}

		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: Upper32BitsSet | BitTwiddling.GetBitsOver(value1.MaxValue + value2.MaxValue),
			maxValue: value1.MaxValue + value2.MaxValue,
			minValue: value1.MinValue + value2.MinValue,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue Add64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet + value2.BitsSet, false);
		}

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			return value2;
		}

		if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			return value1;
		}

		if (IntegerTwiddling.IsAddUnsignedCarry(value1.MaxValue, value2.MaxValue))
		{
			return BitValue.Any64;
		}

		if (IntegerTwiddling.IsAddUnsignedCarry(value1.MinValue, value2.MinValue))
		{
			return BitValue.Any64;
		}

		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: BitTwiddling.GetBitsOver(value1.MaxValue + value2.MaxValue),
			maxValue: value1.MaxValue + value2.MaxValue,
			minValue: value1.MinValue + value2.MinValue,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue AddCarryIn32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);
		var value3 = transform.GetBitValue(node.Operand3);

		if (value1 == null || value2 == null || value3 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && value3.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 + value2.BitsSet32 + value3.BitsSet32, true);
		}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0 && value3.AreLower32BitsKnown && value3.BitsSet32 == 0)
		{
			return value2;
		}

		if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0 && value3.AreLower32BitsKnown && value3.BitsSet32 == 0)
		{
			return value1;
		}

		// TODO

		return BitValue.Any32;
	}

	private static BitValue ArithShiftRight32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		var shift = (int)(value2.BitsSet & 0b11111);
		bool knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;
		bool signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;
		ulong highbits = (knownSignedBit && signed) ? ~(~uint.MaxValue >> shift) : 0;

		if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 >> shift | highbits, true);
		}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2.AreLower5BitsKnown && knownSignedBit && shift == 0)
		{
			return value1;
		}

		if (value2.AreLower5BitsKnown && knownSignedBit && shift != 0)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet >> shift | highbits,
				bitsClear: Upper32BitsSet | (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | highbits,
				maxValue: (value1.MaxValue >> shift) | highbits,
				minValue: (value1.MinValue >> shift) | highbits,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		return BitValue.Any32;
	}

	private static BitValue ArithShiftRight64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		var shift = (int)(value2.BitsSet & 0b111111);
		bool knownSignedBit = ((value1.BitsKnown >> 63) & 1) == 1;
		bool signed = ((value1.BitsSet >> 63) & 1) == 1 || ((value1.BitsClear >> 63) & 1) != 1;
		ulong highbits = (knownSignedBit && signed) ? ~(~ulong.MaxValue >> shift) : 0;

		if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet >> shift | highbits, true);
		}

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			return BitValue.Zero64;
		}

		if (value2.AreAll64BitsKnown && knownSignedBit && shift == 0)
		{
			return value1;
		}

		if (value2.AreLower6BitsKnown && knownSignedBit && shift != 0)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet >> shift | highbits,
				bitsClear: (value1.BitsClear >> shift) | ~(ulong.MaxValue >> shift) | highbits,
				maxValue: (value1.MaxValue >> shift) | highbits,
				minValue: (value1.MinValue >> shift) | highbits,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		return BitValue.Any64;
	}

	private static BitValue Compare32x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		var result = EvaluateCompare(value1, value2, node.ConditionCode, transform);

		if (!result.HasValue)
			return BitValue.Any32;

		return BitValue.CreateValue(result.Value, true);
	}

	private static BitValue Compare64x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		var result = EvaluateCompare(value1, value2, node.ConditionCode, transform);

		if (!result.HasValue)
			return BitValue.Any32;

		return BitValue.CreateValue(result.Value, true);
	}

	private static BitValue Compare64x64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		var result = EvaluateCompare(value1, value2, node.ConditionCode, transform);

		if (!result.HasValue)
			return BitValue.Any64;

		return BitValue.CreateValue(result.Value, false);
	}

	private static BitValue GetHigh32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreUpper32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet >> 32, true);
		}

		if (value1.MaxValue <= uint.MaxValue)
		{
			return BitValue.Zero32;
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet >> 32,
			bitsClear: Upper32BitsSet | (value1.BitsClear >> 32),
			maxValue: value1.MaxValue >> 32,
			minValue: value1.MinValue >> 32,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue GetLow32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet & uint.MaxValue, true);
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet & uint.MaxValue,
			bitsClear: value1.BitsClear & uint.MaxValue,
			maxValue: uint.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: true
		); ;
	}

	private static BitValue LoadParamZeroExtend16x32(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(ushort.MaxValue),
			maxValue: ushort.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue LoadParamZeroExtend16x64(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(ushort.MaxValue),
			maxValue: ushort.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue LoadParamZeroExtend8x32(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(byte.MaxValue),
			maxValue: byte.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue LoadParamZeroExtend8x64(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(byte.MaxValue),
			maxValue: byte.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue LoadParamZeroExtend32x64(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~uint.MaxValue,
			maxValue: uint.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue LoadZeroExtend16x32(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(ushort.MaxValue),
			maxValue: ushort.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue LoadZeroExtend16x64(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(ushort.MaxValue),
			maxValue: ushort.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue LoadZeroExtend8x32(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(byte.MaxValue),
			maxValue: byte.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue LoadZeroExtend8x64(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~(ulong)(byte.MaxValue),
			maxValue: byte.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue LoadZeroExtend32x64(InstructionNode node, TransformContext transform)
	{
		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: ~uint.MaxValue,
			maxValue: uint.MaxValue,
			minValue: 0,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue And32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == 0)
		{
			return BitValue.Zero32;
		}

		if (value2.AreLower32BitsKnown && (value2.BitsSet & ulong.MaxValue) == 0)
		{
			return BitValue.Zero32;
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet & value2.BitsSet,
			bitsClear: value2.BitsClear | value1.BitsClear,
			maxValue: Math.Max(value1.MaxValue, value2.MaxValue),
			minValue: Math.Min(value1.MinValue, value2.MinValue),
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue And64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			return BitValue.Zero64;
		}

		if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			return BitValue.Zero64;
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet & value2.BitsSet,
			bitsClear: value2.BitsClear | value1.BitsClear,
			maxValue: Math.Max(value1.MaxValue, value2.MaxValue),
			minValue: Math.Min(value1.MinValue, value2.MinValue),
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue Not32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(~value1.BitsSet32, true);
		}

		return BitValue.CreateValue(

			bitsSet: value1.BitsClear32,
			bitsClear: value1.BitsSet32,
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: true
		);
	}

	private static BitValue Not64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreAll64BitsKnown)
		{
			return BitValue.CreateValue(~value1.BitsSet, false);
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsClear,
			bitsClear: value1.BitsSet,
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: false

		);
	}

	private static BitValue Or32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
		{
			return value1;
		}

		if (value2 != null && value2.AreLower32BitsKnown && value2.BitsSet32 == uint.MaxValue)
		{
			return value2;
		}

		if (value1 == null || value2 == null)
			return null;

		//if (value1.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
		//{
		//	return value2;
		//}

		//if (value2.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
		//{
		//	return value1;
		//}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet | value2.BitsSet,
			bitsClear: value2.BitsClear & value1.BitsClear,
			maxValue: Math.Max(value1.MaxValue, value2.MaxValue),
			minValue: Math.Min(value1.MinValue, value2.MinValue),
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue Or64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet == ulong.MaxValue)
		{
			return value1;
		}

		if (value2 != null && value2.AreAll64BitsKnown && value2.BitsSet == ulong.MaxValue)
		{
			return value2;
		}

		if (value1 == null || value2 == null)
			return null;

		//if (value1.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == ulong.MaxValue)
		//{
		//	return value2;
		//}

		//if (value2.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == ulong.MaxValue)
		//{
		//	return value1;
		//}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet | value2.BitsSet,
			bitsClear: value2.BitsClear & value1.BitsClear,
			maxValue: Math.Max(value1.MaxValue, value2.MaxValue),
			minValue: Math.Min(value1.MinValue, value2.MinValue),
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue Xor32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue((value1.BitsSet32 ^ value2.BitsSet32) & uint.MaxValue, true);
		}

		ulong bitsKnown = value1.BitsKnown & value2.BitsKnown & uint.MaxValue;

		return BitValue.CreateValue(
			bitsSet: (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
			bitsClear: (value2.BitsClear ^ value1.BitsClear) & bitsKnown,
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: true
		);
	}

	private static BitValue Xor64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet ^ value2.BitsSet, true);
		}

		ulong bitsKnown = value1.BitsKnown & value2.BitsKnown;

		return BitValue.CreateValue(
			bitsSet: (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
			bitsClear: (value2.BitsClear ^ value1.BitsClear) & bitsKnown,
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: false
		);
	}

	private static BitValue Move32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.Is32Bit)
			return value1;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet & uint.MaxValue,
			bitsClear: value1.BitsClear | Upper32BitsSet,
			maxValue: Math.Min(uint.MaxValue, value1.MaxValue),
			minValue: value1.MinValue > uint.MaxValue ? 0 : value1.MinValue,
			rangeDeterminate: false,
			is32Bit: true
		);
	}

	private static BitValue Move64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		return value1;
	}

	private static BitValue MulSigned32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue((ulong)((int)value1.BitsSet32 * (int)value2.BitsSet32), true);
		}

		//if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		//if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 1)
		{
			return value2;
		}

		if (value2.AreLower32BitsKnown && value2.BitsSet32 == 1)
		{
			return value1;
		}

		// TODO: Special power of two handling for bits, handle similar to shift left

		if (!IntegerTwiddling.HasSignBitSet((int)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MinValue)
			&& !IntegerTwiddling.IsMultiplySignedOverflow((int)value1.MaxValue, (int)value2.MaxValue))
		{
			var max = Math.Max(value1.MaxValue, value2.MaxValue);
			var min = Math.Min(value1.MinValue, value2.MinValue);
			var uppermax = max * max;

			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: Upper32BitsSet | BitTwiddling.GetBitsOver((uint)(uppermax)),
				maxValue: (uint)(max * max),
				minValue: (uint)(min * min),
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		return BitValue.Any32;
	}

	private static BitValue MulSigned64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			return BitValue.CreateValue((ulong)((long)value1.BitsSet * (long)value2.BitsSet), false);
		}

		//if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		//{
		//	return BitValue.Zero64;
		//}

		//if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		//{
		//	return BitValue.Zero64;
		//}

		if (value1.AreAll64BitsKnown && value1.BitsSet == 1)
		{
			return value2;
		}

		if (value2.AreAll64BitsKnown && value2.BitsSet == 1)
		{
			return value1;
		}

		// TODO: Special power of two handling for bits, handle similar to shift left

		if (!IntegerTwiddling.HasSignBitSet((long)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value2.MinValue)
			&& !IntegerTwiddling.IsMultiplySignedOverflow((long)value1.MaxValue, (long)value2.MaxValue))
		{
			var max = Math.Max(value1.MaxValue, value2.MaxValue);
			var min = Math.Min(value1.MinValue, value2.MinValue);
			var uppermax = max * max;

			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: BitTwiddling.GetBitsOver(uppermax),
				maxValue: max * max,
				minValue: min * min,
				rangeDeterminate: false,
				is32Bit: false
			);
		}

		return BitValue.Any64;
	}

	private static BitValue MulUnsigned32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 * value2.BitsSet32, true);
		}

		//if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		//if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 1)
		{
			return value2;
		}

		if (value2.AreLower32BitsKnown && value2.BitsSet32 == 1)
		{
			return value1;
		}

		// TODO: Special power of two handling for bits, handle similar to shift left

		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: Upper32BitsSet | BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue),
			maxValue: (uint)(value1.MaxValue * value2.MaxValue),
			minValue: (uint)(value1.MinValue * value2.MinValue),
			rangeDeterminate: !IntegerTwiddling.IsMultiplyUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue),
			is32Bit: true
		);
	}

	private static BitValue MulUnsigned64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet * value2.BitsSet, false);
		}

		//if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		//{
		//	return BitValue.Zero64;
		//}

		//if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		//{
		//	return BitValue.Zero64;
		//}

		if (value1.AreAll64BitsKnown && value1.BitsSet == 1)
		{
			return value2;
		}

		if (value2.AreAll64BitsKnown && value2.BitsSet == 1)
		{
			return value1;
		}

		// TODO: Special power of two handling for bits, handle similar to shift left

		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue),
			maxValue: value1.MaxValue * value2.MaxValue,
			minValue: value1.MinValue * value2.MinValue,
			rangeDeterminate: !IntegerTwiddling.IsMultiplyUnsignedCarry(value1.MaxValue, value2.MaxValue),
			is32Bit: false
		);
	}

	private static BitValue Phi32(InstructionNode node, TransformContext transform)
	{
		Debug.Assert(node.OperandCount != 0);

		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		ulong max = value1.MaxValue;
		ulong min = value1.MinValue;
		ulong bitsset = value1.BitsSet;
		ulong bitsclear = value1.BitsClear;

		for (int i = 1; i < node.OperandCount; i++)
		{
			var operand = node.GetOperand(i);
			var value = transform.GetBitValue(operand);

			if (value == null)
				return null;

			max = Math.Max(max, value.MaxValue);
			min = Math.Min(min, value.MinValue);
			bitsset &= value.BitsSet;
			bitsclear &= value.BitsClear;
		}

		return BitValue.CreateValue(
			bitsSet: bitsset,
			bitsClear: bitsclear,
			maxValue: max,
			minValue: min,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue Phi64(InstructionNode node, TransformContext transform)
	{
		Debug.Assert(node.OperandCount != 0);

		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		ulong max = value1.MaxValue;
		ulong min = value1.MinValue;
		ulong bitsset = value1.BitsSet;
		ulong bitsclear = value1.BitsClear;

		for (int i = 1; i < node.OperandCount; i++)
		{
			var operand = node.GetOperand(i);
			var value = transform.GetBitValue(operand);

			if (value == null)
				return null;

			max = Math.Max(max, value.MaxValue);
			min = Math.Min(min, value.MinValue);
			bitsset &= value.BitsSet;
			bitsclear &= value.BitsClear;
		}

		return BitValue.CreateValue(
			bitsSet: bitsset,
			bitsClear: bitsclear,
			maxValue: max,
			minValue: min,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue RemUnsigned32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			// divide by zero!
			return null;
		}

		if (value1 == null || value2 == null)
			return null;

		//if (value2.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		//{
		//	// divide by zero!
		//	return null; // BitValue.Any32;
		//}

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 % value2.BitsSet32, true);
		}

		//if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		if (value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
		{
			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: BitTwiddling.GetBitsOver(value2.BitsSet - 1),
				maxValue: value2.BitsSet - 1,
				minValue: 0,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		return BitValue.Any32;
	}

	private static BitValue RemUnsigned64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreAll64BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			// divide by zero!
			return null;
		}

		if (value1 == null || value2 == null)
			return null;

		//if (value2.AreAll64BitsKnown && value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
		//{
		//	// divide by zero!
		//	return null; // BitValue.Any64;
		//}

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet % value2.BitsSet, true);
		}

		//if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		//{
		//	return BitValue.Zero64;
		//}

		if (value2.AreAll64BitsKnown && value2.BitsSet32 != 0)
		{
			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: BitTwiddling.GetBitsOver(value2.BitsSet - 1),
				maxValue: value2.BitsSet - 1,
				minValue: 0,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		return BitValue.Any64;
	}

	private static BitValue ShiftLeft32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		var shift = (int)(value2.BitsSet & 0b11111);

		if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 << shift, true);
		}

		if (value2.AreLower5BitsKnown && shift == 0)
		{
			return value1;
		}

		if (value2.AreLower5BitsKnown && shift != 0)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet << shift,
				bitsClear: Upper32BitsSet | (value1.BitsClear << shift) | ~(ulong.MaxValue << shift),
				maxValue: (value1.MaxValue << shift) < uint.MaxValue ? value1.MaxValue << shift : ulong.MaxValue,
				minValue: (value1.MinValue << shift) > value1.MinValue ? value1.MinValue << shift : 0,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		if (value1.AreAnyBitsKnown)
		{
			var lowest = BitTwiddling.CountTrailingZeros(value1.BitsSet | value1.BitsUnknown);

			// FUTURE: if the value2 has any set bits, the lower bound could be raised

			if (lowest >= 1)
			{
				var low = (ulong)(1 << lowest);

				return BitValue.CreateValue(
					bitsSet: 0,
					bitsClear: low - 1,
					maxValue: ulong.MaxValue,
					minValue: low,
					rangeDeterminate: true,
					is32Bit: true
				);
			}
		}

		return BitValue.Any32;
	}

	private static BitValue ShiftLeft64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		var shift = (int)(value2.BitsSet & 0b111111);

		if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 << shift, false);
		}

		if (value2.AreLower6BitsKnown && shift == 0)
		{
			return value1;
		}

		if (value2.AreLower6BitsKnown && shift != 0)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet << shift,
				bitsClear: (value1.BitsClear << shift) | ~(ulong.MaxValue << shift),
				maxValue: (value1.MaxValue << shift) > value1.MaxValue ? value1.MaxValue << shift : ulong.MaxValue,
				minValue: (value1.MinValue << shift) > value1.MinValue ? value1.MinValue << shift : 0,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		if (value1.AreLower32BitsKnown && ((value1.BitsSet & uint.MaxValue) == 0))
		{
			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: uint.MaxValue,
				maxValue: ulong.MaxValue,
				minValue: 0,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively

		return BitValue.Any64;
	}

	private static BitValue ShiftRight32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		var shift = (int)(value2.BitsSet & 0b11111);

		if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 >> shift, true);
		}

		//if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		if (value2.AreLower5BitsKnown && shift == 0)
		{
			return value1;
		}

		if (value2.AreLower5BitsKnown && shift != 0)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet >> shift,
				bitsClear: Upper32BitsSet | (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift),
				maxValue: value1.MaxValue >> shift,
				minValue: value1.MinValue >> shift,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively
		// FUTURE: If the shift has a range, and some bits are known --- then some bits might be determinable

		//if (value1.AreRangeValuesDeterminate)
		//{
		//	return new Value(
		//	{
		//		maxValue: value1.MaxValue, // must be equal or less, but not more
		//		minValue: 0,
		//		rangeValuesDeterminate: true,
		//		bitsSet:0,
		//		bitsClear: 0,
		//		Is64Bit = false
		//	};
		//}

		return BitValue.Any32;
	}

	private static BitValue ShiftRight64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		var shift = (int)(value2.BitsSet & 0b111111);

		if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet >> shift, false);
		}

		//if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		//{
		//	return BitValue.Zero64;
		//}

		if (value2.AreLower6BitsKnown && shift == 0)
		{
			return value1;
		}

		if (value2.AreLower6BitsKnown && shift != 0)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet >> shift,
				bitsClear: value1.BitsClear >> shift | ~(ulong.MaxValue >> shift),
				maxValue: value1.MaxValue >> shift,
				minValue: value1.MinValue >> shift,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		if (value1.AreUpper32BitsKnown && ((value1.BitsSet >> 32) == 0))
		{
			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: ~uint.MaxValue,
				maxValue: uint.MaxValue,
				minValue: 0,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively
		// FUTURE: If the shift has a range, and some bits are known --- then some bits might be determinable

		//if (value1.AreRangeValuesDeterminate)
		//{
		//	return new Value(
		//	{
		//		maxValue: value1.MaxValue, // must be equal or less, but not more
		//		minValue: 0,
		//		rangeValuesDeterminate: true,
		//		bitsSet:0,
		//		bitsClear: 0,
		//		Is64Bit = true
		//	};
		//}

		return BitValue.Any64;
	}

	private static BitValue SignExtend16x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower16BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet16 | ((((value1.BitsSet >> 15) & 1) == 1) ? Upper48BitsSet : 0), true);
		}

		bool knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;

		if (!knownSignedBit)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet16,
				bitsClear: value1.BitsClear16,
				maxValue: 0,
				minValue: 0,
				rangeDeterminate: false,
				is32Bit: true
			);
		}

		bool signed = ((value1.BitsSet >> 15) & 1) == 1 || ((value1.BitsClear >> 15) & 1) != 1;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
			bitsClear: value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: true
		);
	}

	private static BitValue SignExtend16x64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower16BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet16 | ((((value1.BitsSet >> 15) & 1) == 1) ? Upper48BitsSet : 0), true);
		}

		bool knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;

		if (!knownSignedBit)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet16,
				bitsClear: value1.BitsClear16,
				maxValue: 0,
				minValue: 0,
				rangeDeterminate: false,
				is32Bit: false
			);
		}

		bool signed = ((value1.BitsSet >> 15) & 1) == 1 || ((value1.BitsClear >> 15) & 1) != 1;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
			bitsClear: value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: false
		);
	}

	private static BitValue SignExtend32x64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32 | ((((value1.BitsSet >> 31) & 1) == 1) ? Upper32BitsSet : 0), false);
		}

		bool knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;

		if (!knownSignedBit)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet32,
				bitsClear: value1.BitsClear32,
				maxValue: 0,
				minValue: 0,
				rangeDeterminate: false,
				is32Bit: false
			);
		}

		bool signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet32 | (signed ? Upper56BitsSet : 0),
			bitsClear: value1.BitsClear32 | (signed ? 0 : Upper56BitsSet),
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: false
		);
	}

	private static BitValue SignExtend8x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower8BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet16 | ((((value1.BitsSet >> 7) & 1) == 1) ? Upper56BitsSet : 0), true);
		}

		bool knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

		if (!knownSignedBit)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet8,
				bitsClear: value1.BitsClear8,
				maxValue: 0,
				minValue: 0,
				rangeDeterminate: false,
				is32Bit: true
			);
		}

		bool signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
			bitsClear: value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: true
		);
	}

	private static BitValue SignExtend8x64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower8BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet16 | ((((value1.BitsSet >> 7) & 1) == 1) ? Upper56BitsSet : 0), false);
		}

		bool knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

		if (!knownSignedBit)
		{
			return BitValue.CreateValue(
				bitsSet: value1.BitsSet8,
				bitsClear: value1.BitsClear8,
				maxValue: 0,
				minValue: 0,
				rangeDeterminate: false,
				is32Bit: false
			);
		}

		bool signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
			bitsClear: value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
			maxValue: 0,
			minValue: 0,
			rangeDeterminate: false,
			is32Bit: false
		);
	}

	private static BitValue To64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value2.MaxValue << 32 | (value1.MaxValue & uint.MaxValue), false);
		}

		return BitValue.CreateValue(
			bitsSet: (value2.BitsSet << 32) | value1.BitsSet32,
			bitsClear: (value2.BitsClear << 32) | (value1.BitsClear32),
			maxValue: (value2.MaxValue << 32) | (value1.MaxValue & uint.MaxValue),
			minValue: (value2.MinValue << 32) | (value1.MinValue & uint.MaxValue),
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue Truncate64x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet & uint.MaxValue,
			bitsClear: Upper32BitsSet | value1.BitsClear,
			maxValue: Math.Min(uint.MaxValue, value1.MaxValue),
			minValue: value1.MinValue > uint.MaxValue ? 0 : value1.MinValue,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue ZeroExtend16x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower16BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet16, true);
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet16,
			bitsClear: value1.BitsClear | Upper48BitsSet,
			maxValue: Math.Min(byte.MaxValue, value1.MaxValue),
			minValue: value1.MinValue > byte.MaxValue ? 0 : value1.MinValue,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue ZeroExtend16x64(InstructionNode node, TransformContext transform)
	{
		return ZeroExtend16x32(node, transform);
	}

	private static BitValue ZeroExtend32x64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet32, false);
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet32,
			bitsClear: Upper32BitsSet | value1.BitsClear,
			maxValue: Math.Min(uint.MaxValue, value1.MaxValue),
			minValue: value1.MinValue > uint.MaxValue ? 0 : value1.MinValue,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue ZeroExtend8x32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower8BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet8, true);
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet8,
			bitsClear: value1.BitsClear | Upper56BitsSet,
			maxValue: Math.Min(byte.MaxValue, value1.MaxValue),
			minValue: value1.MinValue > byte.MaxValue ? 0 : value1.MinValue,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue ZeroExtend8x64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);

		if (value1 == null)
			return null;

		if (value1.AreLower8BitsKnown)
		{
			return BitValue.CreateValue(value1.BitsSet8, true);
		}

		return BitValue.CreateValue(
			bitsSet: value1.BitsSet8,
			bitsClear: value1.BitsClear | Upper56BitsSet,
			maxValue: Math.Min(byte.MaxValue, value1.MaxValue),
			minValue: value1.MinValue > byte.MaxValue ? 0 : value1.MinValue,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue IfThenElse32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand2);
		var value2 = transform.GetBitValue(node.Operand3);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			return BitValue.CreateValue(
				bitsSet: value1.MaxValue & value2.MaxValue,
				bitsClear: ~value1.MaxValue & ~value2.MaxValue,
				maxValue: Math.Max(value1.MaxValue, value2.MaxValue),
				minValue: Math.Min(value1.MinValue, value2.MinValue),
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		return BitValue.Any32;
	}

	private static BitValue IfThenElse64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand2);
		var value2 = transform.GetBitValue(node.Operand3);

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			return BitValue.CreateValue(
				bitsSet: value1.MaxValue & value2.MaxValue,
				bitsClear: ~value1.MaxValue & ~value2.MaxValue,
				maxValue: Math.Max(value1.MaxValue, value2.MaxValue),
				minValue: Math.Min(value1.MinValue, value2.MinValue),
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		return BitValue.Any64;
	}

	private static BitValue NewString(InstructionNode node, TransformContext transform)
	{
		return transform.Is32BitPlatform ? BitValue.AnyExceptZero32 : BitValue.AnyExceptZero64;
	}

	private static BitValue NewObject(InstructionNode node, TransformContext transform)
	{
		return transform.Is32BitPlatform ? BitValue.AnyExceptZero32 : BitValue.AnyExceptZero64;
	}

	private static BitValue NewArray(InstructionNode node, TransformContext transform)
	{
		return transform.Is32BitPlatform ? BitValue.AnyExceptZero32 : BitValue.AnyExceptZero64;
	}

	private static BitValue DivUnsigned32(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
		{
			return BitValue.CreateValue(value1.BitsSet32 / value2.BitsSet32, true);
		}

		if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			// divide by zero!
			return null; // BitValue.Any64;
		}

		//if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		//{
		//	return BitValue.Zero32;
		//}

		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: Upper32BitsSet | BitTwiddling.GetBitsOver(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue),
			maxValue: value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue,
			minValue: value2.MaxValue == 0 ? 0 : value1.MinValue / value2.MaxValue,
			rangeDeterminate: true,
			is32Bit: true
		);
	}

	private static BitValue DivUnsigned64(InstructionNode node, TransformContext transform)
	{
		var value1 = transform.GetBitValue(node.Operand1);
		var value2 = transform.GetBitValue(node.Operand2);

		if (value1 != null && value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
		{
			return BitValue.Zero32;
		}

		if (value2 != null && value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			// divide by zero!
			return null; // BitValue.Any64;
		}

		if (value1 == null || value2 == null)
			return null;

		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value2.BitsSet32 != 0)
		{
			return BitValue.CreateValue(value1.BitsSet / value2.BitsSet, true);
		}

		//if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		//{
		//	// divide by zero!
		//	return null; // BitValue.Any64;
		//}

		//if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		//{
		//	return BitValue.Zero64;
		//}

		return BitValue.CreateValue(
			bitsSet: 0,
			bitsClear: BitTwiddling.GetBitsOver(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue),
			maxValue: value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue,
			minValue: value2.MaxValue == 0 ? 0 : value1.MinValue / value2.MaxValue,
			rangeDeterminate: true,
			is32Bit: false
		);
	}

	private static BitValue Any32(InstructionNode node, TransformContext transform)
	{
		return BitValue.Any32;
	}

	private static BitValue Any64(InstructionNode node, TransformContext transform)
	{
		return BitValue.Any64;
	}

	#endregion IR Instructions
}
