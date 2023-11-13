﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

	private readonly Counter BranchesRemovedCount = new("BitTracker.BranchesRemoved");
	private readonly Counter InstructionsRemovedCount = new("BitTracker.InstructionsRemoved");
	private readonly Counter InstructionsUpdatedCount = new("BitTracker.InstructionsUpdated");

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

		Register(IR.Phi32, Phi32);
		Register(IR.Phi64, Phi64);

		Register(IR.Move32, Move32);
		Register(IR.Move64, Move64);

		Register(IR.Truncate64x32, Truncate64x32);

		Register(IR.GetLow32, GetLow32);
		Register(IR.GetHigh32, GetHigh32);
		Register(IR.To64, To64);

		Register(IR.Or32, Or32);
		Register(IR.Or64, Or64);
		Register(IR.And32, And32);
		Register(IR.And64, And64);
		Register(IR.Xor32, Xor32);
		Register(IR.Xor64, Xor64);
		Register(IR.Neg32, Neg32);
		Register(IR.Neg64, Neg64);
		Register(IR.Not32, Not32);
		Register(IR.Not64, Not64);

		Register(IR.LoadZeroExtend8x32, LoadZeroExtend8x32);
		Register(IR.LoadZeroExtend16x32, LoadZeroExtend16x32);

		Register(IR.LoadZeroExtend8x64, LoadZeroExtend8x64);
		Register(IR.LoadZeroExtend16x64, LoadZeroExtend16x64);
		Register(IR.LoadZeroExtend32x64, LoadZeroExtend32x64);

		Register(IR.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
		Register(IR.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
		Register(IR.LoadParamZeroExtend8x64, LoadParamZeroExtend8x64);
		Register(IR.LoadParamZeroExtend16x64, LoadParamZeroExtend16x64);
		Register(IR.LoadParamZeroExtend32x64, LoadParamZeroExtend32x64);

		Register(IR.ShiftRight32, ShiftRight32);
		Register(IR.ShiftRight64, ShiftRight64);

		Register(IR.ArithShiftRight32, ArithShiftRight32);
		Register(IR.ArithShiftRight64, ArithShiftRight64);

		Register(IR.ShiftLeft32, ShiftLeft32);
		Register(IR.ShiftLeft64, ShiftLeft64);

		Register(IR.Compare32x32, Compare32x32);
		Register(IR.Compare64x64, Compare64x64);
		Register(IR.Compare64x32, Compare64x32);

		Register(IR.MulUnsigned32, MulUnsigned32);
		Register(IR.MulUnsigned64, MulUnsigned64);

		Register(IR.MulSigned32, MulSigned32);
		Register(IR.MulSigned64, MulSigned64);

		Register(IR.Add32, Add32);
		Register(IR.Add64, Add64);
		Register(IR.AddCarryIn32, AddCarryIn32);

		Register(IR.SignExtend16x32, SignExtend16x32);
		Register(IR.SignExtend8x32, SignExtend8x32);
		Register(IR.SignExtend16x64, SignExtend16x64);
		Register(IR.SignExtend8x64, SignExtend8x64);
		Register(IR.SignExtend32x64, SignExtend32x64);

		Register(IR.ZeroExtend16x32, ZeroExtend16x32);
		Register(IR.ZeroExtend8x32, ZeroExtend8x32);
		Register(IR.ZeroExtend16x64, ZeroExtend16x64);
		Register(IR.ZeroExtend8x64, ZeroExtend8x64);
		Register(IR.ZeroExtend32x64, ZeroExtend32x64);

		Register(IR.RemUnsigned32, RemUnsigned32);
		Register(IR.RemUnsigned64, RemUnsigned64);

		Register(IR.IfThenElse32, IfThenElse32);
		Register(IR.IfThenElse64, IfThenElse64);
		Register(IR.NewString, NewString);
		Register(IR.NewObject, NewObject);
		Register(IR.NewArray, NewArray);

		Register(IR.DivUnsigned32, DivUnsigned32);
		Register(IR.DivUnsigned64, DivUnsigned64);

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

	private void UpdateInstructions()
	{
		foreach (var register in MethodCompiler.VirtualRegisters)
		{
			UpdateInstruction(register);
		}
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

			while (node.IsEmptyOrNop || node.Instruction == IR.Jmp)
			{
				node = node.Previous;
			}

			// Skip Switch instructions
			if (node.Instruction == IR.Switch)
				continue;

			Debug.Assert(node.Instruction.IsBranch);

			var value1 = node.Operand1.BitValue;
			var value2 = node.Operand2.BitValue;

			var result = BaseTransform.EvaluateCompare(value1, value2, node.ConditionCode);

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
				.NarrowMin((value1.MinValue >> shift) | highbits)
				.NarrowMax((value1.MaxValue >> shift) | highbits)
				.NarrowSetBits(value1.BitsSet >> shift | highbits)
				.NarrowClearBits(Upper32BitsSet | (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | highbits)
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
				.NarrowMin((value1.MinValue >> shift) | highbits)
				.NarrowMax((value1.MaxValue >> shift) | highbits)
				.NarrowSetBits(value1.BitsSet >> shift | highbits)
				.NarrowClearBits((value1.BitsClear >> shift) | ~(ulong.MaxValue >> shift) | highbits)
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

		var compare = BaseTransform.EvaluateCompare(value1, value2, node.ConditionCode);

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

		var compare = BaseTransform.EvaluateCompare(value1, value2, node.ConditionCode);

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

		var compare = BaseTransform.EvaluateCompare(value1, value2, node.ConditionCode);

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
				.NarrowMin(value1.MinValue >> 32)
				.NarrowMax(value1.MaxValue >> 32)
				.NarrowSetBits(value1.BitsSet >> 32)
				.NarrowClearBits(Upper32BitsSet | (value1.BitsClear >> 32))
				.SetStable(value1);
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
				.NarrowSetBits(value1.BitsSet & uint.MaxValue)
				.NarrowClearBits(value1.BitsClear & uint.MaxValue)
				.SetStable(value1);
		}
	}

	private static void LoadParamZeroExtend16x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	private static void LoadParamZeroExtend16x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	private static void LoadParamZeroExtend8x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	private static void LoadParamZeroExtend8x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	private static void LoadParamZeroExtend32x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(uint.MaxValue)
			.NarrowClearBits(~uint.MaxValue);
	}

	private static void LoadZeroExtend16x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	private static void LoadZeroExtend16x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	private static void LoadZeroExtend8x32(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	private static void LoadZeroExtend8x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	private static void LoadZeroExtend32x64(Node node)
	{
		var result = node.Result.BitValue;

		result
			.NarrowMax(uint.MaxValue)
			.NarrowClearBits(~uint.MaxValue);
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
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.BitsSet & value2.BitsSet)
				.NarrowClearBits(value2.BitsClear | value1.BitsClear)
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
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.BitsSet & value2.BitsSet)
				.NarrowClearBits(value2.BitsClear | value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	private static void Neg32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreLower32BitsKnown)
		{
			result.SetValue((ulong)(-(int)value1.BitsSet32));
		}
		else
		{
			// FUTURE: (~b + 1)
			result.SetStable(value1);
		}
	}

	private static void Neg64(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		if (value1.AreAll64BitsKnown)
		{
			result.SetValue((ulong)(-value1.BitsSet32));
		}
		else
		{
			// FUTURE: (~b + 1)
			result.SetStable(value1);
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
			result
				.NarrowSetBits(value1.BitsClear32)
				.NarrowClearBits(value1.BitsSet32).SetStable(value1);
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
				.NarrowSetBits(value1.BitsClear)
				.NarrowClearBits(value1.BitsSet)
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
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.BitsSet | value2.BitsSet)
				.NarrowClearBits(value2.BitsClear & value1.BitsClear)
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
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.BitsSet | value2.BitsSet)
				.NarrowClearBits(value2.BitsClear & value1.BitsClear)
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
				.NarrowSetBits((value1.BitsSet ^ value2.BitsSet) & bitsKnown)
				.NarrowClearBits((value2.BitsClear ^ value1.BitsClear) & bitsKnown)
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
				.NarrowSetBits((value1.BitsSet ^ value2.BitsSet) & bitsKnown)
				.NarrowClearBits((value2.BitsClear ^ value1.BitsClear) & bitsKnown)
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
				.NarrowMin((uint)(min * min))
				.NarrowMax((uint)(max * max))
				.NarrowClearBits(Upper32BitsSet | BitTwiddling.GetBitsOver((uint)uppermax))
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
				.NarrowClearBits(BitTwiddling.GetBitsOver(uppermax))
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
				.NarrowClearBits(Upper32BitsSet | BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue))
				.SetStable(value1, value2);
		}
		else
		{
			result
				.NarrowMin((uint)(value1.MinValue * value2.MinValue))
				.NarrowMax((uint)(value1.MaxValue * value2.MaxValue))
				.NarrowClearBits(Upper32BitsSet | BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue))
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
				.NarrowMin(value1.MinValue * value2.MinValue)
				.NarrowMax(value1.MaxValue * value2.MaxValue)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value1.MaxValue * value2.MaxValue))
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
				.NarrowMin(value1.MinValue << shift > value1.MinValue ? value1.MinValue << shift : 0)
				.NarrowMax(value1.MaxValue << shift < uint.MaxValue ? value1.MaxValue << shift : ulong.MaxValue)
				.NarrowSetBits(value1.BitsSet << shift)
				.NarrowClearBits(Upper32BitsSet | (value1.BitsClear << shift) | ~(ulong.MaxValue << shift))
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
				.NarrowMin(value1.MinValue << shift > value1.MinValue ? value1.MinValue << shift : 0)
				.NarrowMax(value1.MaxValue << shift > value1.MaxValue ? value1.MaxValue << shift : ulong.MaxValue)
				.NarrowSetBits(value1.BitsSet << shift)
				.NarrowClearBits((value1.BitsClear << shift) | ~(ulong.MaxValue << shift))
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
				.NarrowMin(value1.MinValue >> shift)
				.NarrowMax(value1.MaxValue >> shift)
				.NarrowSetBits(value1.BitsSet >> shift)
				.NarrowClearBits(Upper32BitsSet | (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift))
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
				.NarrowMin(value1.MinValue >> shift)
				.NarrowMax(value1.MaxValue >> shift)
				.NarrowSetBits(value1.BitsSet >> shift)
				.NarrowClearBits(value1.BitsClear >> shift | ~(ulong.MaxValue >> shift))
				.SetStable(value1);
		}
		else if (value1.AreUpper32BitsKnown && value1.BitsSet >> 32 == 0)
		{
			result
				.NarrowMax(uint.MaxValue)
				.NarrowClearBits(~uint.MaxValue)
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
				.NarrowSetBits(value1.BitsSet16)
				.NarrowClearBits(value1.BitsClear16)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsSet16 | (signed ? Upper48BitsSet : 0))
				.NarrowClearBits(value1.BitsClear16 | (signed ? 0 : Upper48BitsSet))
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
				.NarrowSetBits(value1.BitsSet16)
				.NarrowClearBits(value1.BitsClear16)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsSet16 | (signed ? Upper48BitsSet : 0))
				.NarrowClearBits(value1.BitsClear16 | (signed ? 0 : Upper48BitsSet))
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
				.NarrowSetBits(value1.BitsSet32)
				.NarrowClearBits(value1.BitsClear32)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsSet32 | (signed ? Upper56BitsSet : 0))
				.NarrowClearBits(value1.BitsClear32 | (signed ? 0 : Upper56BitsSet))
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
				.NarrowSetBits(value1.BitsSet8)
				.NarrowClearBits(value1.BitsClear8)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsSet8 | (signed ? Upper56BitsSet : 0))
				.NarrowClearBits(value1.BitsClear8 | (signed ? 0 : Upper56BitsSet))
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
				.NarrowSetBits(value1.BitsSet8)
				.NarrowClearBits(value1.BitsClear8)
				.SetStable(value1);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsSet8 | (signed ? Upper56BitsSet : 0))
				.NarrowClearBits(value1.BitsClear8 | (signed ? 0 : Upper56BitsSet))
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
				.NarrowMin((value2.MinValue << 32) | (value1.MinValue & uint.MaxValue))
				.NarrowMax((value2.MaxValue << 32) | (value1.MaxValue & uint.MaxValue))
				.NarrowSetBits((value2.BitsSet << 32) | value1.BitsSet32)
				.NarrowClearBits((value2.BitsClear << 32) | value1.BitsClear32)
				.SetStable(value1, value2);
		}
	}

	private static void Truncate64x32(Node node)
	{
		var result = node.Result.BitValue;
		var value1 = node.Operand1.BitValue;

		result
			.NarrowMin(value1.MinValue > uint.MaxValue ? 0 : value1.MinValue)
			.NarrowMax(Math.Min(uint.MaxValue, value1.MaxValue))
			.NarrowSetBits(value1.BitsSet & uint.MaxValue)
			.NarrowClearBits(Upper32BitsSet | value1.BitsClear)
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
				.NarrowMin(value1.MinValue > byte.MaxValue ? 0 : value1.MinValue)
				.NarrowMax(Math.Min(byte.MaxValue, value1.MaxValue))
				.NarrowSetBits(value1.BitsSet16)
				.NarrowClearBits(value1.BitsClear | Upper48BitsSet)
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
				.NarrowMin(value1.MinValue > uint.MaxValue ? 0 : value1.MinValue)
				.NarrowMax(Math.Min(uint.MaxValue, value1.MaxValue))
				.NarrowSetBits(value1.BitsSet32)
				.NarrowClearBits(Upper32BitsSet | value1.BitsClear)
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
				.NarrowMin(value1.MinValue > byte.MaxValue ? 0 : value1.MinValue)
				.NarrowMax(Math.Min(byte.MaxValue, value1.MaxValue))
				.NarrowSetBits(value1.BitsSet8)
				.NarrowClearBits(value1.BitsClear | Upper56BitsSet)
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
				.NarrowMin(value1.MinValue > byte.MaxValue ? 0 : value1.MinValue)
				.NarrowMax(Math.Min(byte.MaxValue, value1.MaxValue))
				.NarrowSetBits(value1.BitsSet8)
				.NarrowClearBits(value1.BitsClear | Upper56BitsSet)
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
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.MaxValue & value2.MaxValue)
				.NarrowClearBits(~value1.MaxValue & ~value2.MaxValue)
				.SetStable();
		}
		else
		{
			result
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.MaxValue & value2.MaxValue)
				.NarrowClearBits(~value1.MaxValue & ~value2.MaxValue)
				.SetStable(value1, value2);
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
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.MaxValue & value2.MaxValue)
				.NarrowClearBits(~value1.MaxValue & ~value2.MaxValue)
				.SetStable();
		}
		else
		{
			result
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.MaxValue & value2.MaxValue)
				.NarrowClearBits(~value1.MaxValue & ~value2.MaxValue)
				.SetStable(value1, value2);
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
				.NarrowMin(value2.MaxValue == 0 ? 0 : value1.MinValue / value2.MaxValue)
				.NarrowMax(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue)
				.NarrowClearBits(Upper32BitsSet | BitTwiddling.GetBitsOver(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue))
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
				.NarrowMin(value2.MaxValue == 0 ? 0 : value1.MinValue / value2.MaxValue)
				.NarrowMax(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.MinValue == 0 ? value1.MaxValue : value1.MaxValue / value2.MinValue))
				.SetStable(value1, value2);
		}
	}

	#endregion IR Instructions
}
