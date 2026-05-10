// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Framework.Analysis;

namespace Mosa.Compiler.Framework.Stages;

/// <summary>
/// Bit Tracker Stage
/// </summary>
public sealed class BitTrackerStage : BaseMethodCompilerStage
{
	// This stage propagates bit and value range knowledge thru the various operations.
	// This additional knowledge may enable additional optimizations opportunities.

	private const int MaxInstructions = 1024;

	private readonly Counter BranchesRemovedCount = new("BitTracker.BranchesRemoved");
	private readonly Counter InstructionsRemovedCount = new("BitTracker.InstructionsRemoved");
	private readonly Counter InstructionsUpdatedCount = new("BitTracker.InstructionsUpdated");

	private TraceLog Trace;

	private readonly NodeVisitationDelegate[] visitation = new NodeVisitationDelegate[MaxInstructions];

	private delegate void NodeVisitationDelegate(Node node);

	protected override void Finish()
	{
		Trace = null;
	}

	protected override void Initialize()
	{
		Register(InstructionsUpdatedCount);
		Register(InstructionsRemovedCount);
		Register(BranchesRemovedCount);

		Register(IR.Add32, Add32);
		Register(IR.Add64, Add64);
		Register(IR.AddCarryIn32, AddCarryIn32);
		Register(IR.AddCarryIn64, AddCarryIn64);
		Register(IR.AddCarryOut32, Result2NarrowToBoolean);
		Register(IR.AddCarryOut64, Result2NarrowToBoolean);
		Register(IR.AddOverflowOut32, AddOverflowOut32);
		Register(IR.AddOverflowOut64, AddOverflowOut64);
		Register(IR.And32, And32);
		Register(IR.And64, And64);
		Register(IR.ArithShiftRight32, ArithShiftRight32);
		Register(IR.ArithShiftRight64, ArithShiftRight64);
		Register(IR.Compare32x32, Compare);
		Register(IR.Compare32x64, Compare);
		Register(IR.Compare64x32, Compare);
		Register(IR.Compare64x64, Compare);
		Register(IR.CompareManagedPointer, Compare);
		Register(IR.CompareObject, Compare);
		Register(IR.CompareR4, Compare);
		Register(IR.CompareR8, Compare);
		Register(IR.DivSigned32, DivSigned32);
		Register(IR.DivSigned64, DivSigned64);
		Register(IR.DivUnsigned32, DivUnsigned32);
		Register(IR.DivUnsigned64, DivUnsigned64);
		Register(IR.GetHigh32, GetHigh32);
		Register(IR.GetLow32, GetLow32);
		Register(IR.IfThenElse32, IfThenElse32);
		Register(IR.IfThenElse64, IfThenElse64);
		Register(IR.LoadParamZeroExtend8x32, LoadParamZeroExtend8x32);
		Register(IR.LoadParamZeroExtend16x32, LoadParamZeroExtend16x32);
		Register(IR.LoadParamZeroExtend8x64, LoadParamZeroExtend8x64);
		Register(IR.LoadParamZeroExtend16x64, LoadParamZeroExtend16x64);
		Register(IR.LoadParamZeroExtend32x64, LoadParamZeroExtend32x64);
		Register(IR.LoadZeroExtend8x32, LoadZeroExtend8x32);
		Register(IR.LoadZeroExtend16x32, LoadZeroExtend16x32);
		Register(IR.LoadZeroExtend8x64, LoadZeroExtend8x64);
		Register(IR.LoadZeroExtend16x64, LoadZeroExtend16x64);
		Register(IR.LoadZeroExtend32x64, LoadZeroExtend32x64);
		Register(IR.Move32, Move32);
		Register(IR.Move64, Move64);
		Register(IR.MulSigned32, MulSigned32);
		Register(IR.MulSigned64, MulSigned64);
		Register(IR.MulUnsigned32, MulUnsigned32);
		Register(IR.MulUnsigned64, MulUnsigned64);
		Register(IR.Neg32, Neg32);
		Register(IR.Neg64, Neg64);
		Register(IR.NewArray, NewArray);
		Register(IR.NewObject, NewObject);
		Register(IR.NewString, NewString);
		Register(IR.Not32, Not32);
		Register(IR.Not64, Not64);
		Register(IR.Or32, Or32);
		Register(IR.Or64, Or64);
		Register(IR.Phi32, Phi32);
		Register(IR.Phi64, Phi64);
		Register(IR.RemSigned32, RemSigned32);
		Register(IR.RemSigned64, RemSigned64);
		Register(IR.RemUnsigned32, RemUnsigned32);
		Register(IR.RemUnsigned64, RemUnsigned64);
		Register(IR.ShiftLeft32, ShiftLeft32);
		Register(IR.ShiftLeft64, ShiftLeft64);
		Register(IR.ShiftRight32, ShiftRight32);
		Register(IR.ShiftRight64, ShiftRight64);
		Register(IR.SignExtend8x32, SignExtend8x32);
		Register(IR.SignExtend16x32, SignExtend16x32);
		Register(IR.SignExtend8x64, SignExtend8x64);
		Register(IR.SignExtend16x64, SignExtend16x64);
		Register(IR.SignExtend32x64, SignExtend32x64);
		Register(IR.Sub32, Sub32);
		Register(IR.Sub64, Sub64);
		Register(IR.SubCarryIn32, SubCarryIn32);
		Register(IR.SubCarryIn64, SubCarryIn64);
		Register(IR.SubCarryOut32, Result2NarrowToBoolean);
		Register(IR.SubCarryOut64, Result2NarrowToBoolean);
		Register(IR.SubOverflowOut32, SubOverflowOut32);
		Register(IR.SubOverflowOut64, SubOverflowOut64);
		Register(IR.To64, To64);
		Register(IR.Truncate64x32, Truncate64x32);
		Register(IR.Xor32, Xor32);
		Register(IR.Xor64, Xor64);
		Register(IR.ZeroExtend8x32, ZeroExtend8x32);
		Register(IR.ZeroExtend16x32, ZeroExtend16x32);
		Register(IR.ZeroExtend8x64, ZeroExtend8x64);
		Register(IR.ZeroExtend16x64, ZeroExtend16x64);
		Register(IR.ZeroExtend32x64, ZeroExtend32x64);
	}

	private void Register(BaseInstruction instruction, NodeVisitationDelegate method)
	{
		visitation[instruction.ID] = method;

		Compiler.CompilerHooks.RegisterTransform?.Invoke(Name, instruction.Name);
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

		Trace = CreateTraceLog(5);

		Transform.SetLog(Trace);

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

		if (method == null)
			return;

		if (Compiler.CompilerHooks.IsTransformDisabled?.Invoke(Name, node.Instruction.Name) == true)
			return;

		method(node);
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

		ulong constantValue;

		if (value.MinValue == value.MaxValue)
		{
			constantValue = value.MinValue;
		}
		else if (value.AreAll64BitsKnown)
		{
			constantValue = value.BitsSet;
		}
		else
		{
			return;
		}

		var constantOperand = virtualRegister.IsInt32
			? Operand.CreateConstant32((uint)constantValue)
			: Operand.CreateConstant64(constantValue);

		if (Trace != null)
		{
			Trace?.Log($"Virtual Register: {virtualRegister}");

			Trace?.Log($"  MaxValue:  {value.MaxValue}");
			Trace?.Log($"  MinValue:  {value.MinValue}");

			if (value.BitsKnown != 0)
			{
				Trace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
				Trace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
				Trace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");
			}
		}

		foreach (var node2 in virtualRegister.Uses.ToArray())
		{
			Trace?.Log($"BEFORE:\t{node2}");

			node2.ReplaceOperand(virtualRegister, constantOperand);

			Trace?.Log($"AFTER: \t{node2}");
			InstructionsUpdatedCount.Increment();
		}

		Debug.Assert(virtualRegister.Uses.Count == 0);

		var node = virtualRegister.Definitions[0];
		Trace?.Log($"REMOVED:\t{node}");

		node.SetNop();
		Trace?.Log();

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

			var newBranch = node.BranchTarget1;

			Trace?.Log($"REMOVED:\t{node}");
			node.SetNop();
			InstructionsRemovedCount.Increment();
			BranchesRemovedCount.Increment();

			if (!result.Value)
				continue;

			while (node.IsEmptyOrNop)
			{
				node = node.Next;
			}

			Trace?.Log($"BEFORE:\t{node}");
			node.UpdateBranchTarget(0, newBranch);
			Trace?.Log($"AFTER:\t{node}");
		}
	}

	#region IR Instructions

	private static void Add32(Node node)
	{
		BitTrackerOperations.Add32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Add64(Node node)
	{
		BitTrackerOperations.Add64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void AddCarryIn32(Node node)
	{
		BitTrackerOperations.AddCarryIn32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.Operand3.BitValue);
	}

	private static void AddCarryIn64(Node node)
	{
		BitTrackerOperations.AddCarryIn64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.Operand3.BitValue);
	}

	private static void AddCarryOut32(Node node)
	{
		BitTrackerOperations.AddCarryOut32(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void AddCarryOut64(Node node)
	{
		BitTrackerOperations.AddCarryOut64(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void AddOverflowOut32(Node node)
	{
		BitTrackerOperations.AddOverflowOut32(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void AddOverflowOut64(Node node)
	{
		BitTrackerOperations.AddOverflowOut64(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void SubCarryOut32(Node node)
	{
		BitTrackerOperations.SubCarryOut32(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void SubCarryOut64(Node node)
	{
		BitTrackerOperations.SubCarryOut64(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void SubOverflowOut32(Node node)
	{
		BitTrackerOperations.SubOverflowOut32(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void SubOverflowOut64(Node node)
	{
		BitTrackerOperations.SubOverflowOut64(node.Result.BitValue, node.Result2.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Sub32(Node node)
	{
		BitTrackerOperations.Sub32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Sub64(Node node)
	{
		BitTrackerOperations.Sub64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void SubCarryIn32(Node node)
	{
		BitTrackerOperations.SubCarryIn32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.Operand3.BitValue);
	}

	private static void SubCarryIn64(Node node)
	{
		BitTrackerOperations.SubCarryIn64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.Operand3.BitValue);
	}

	private static void ArithShiftRight32(Node node)
	{
		BitTrackerOperations.ArithShiftRight32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void ArithShiftRight64(Node node)
	{
		BitTrackerOperations.ArithShiftRight64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Compare(Node node)
	{
		BitTrackerOperations.Compare(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.ConditionCode);
	}

	private static void GetHigh32(Node node)
	{
		BitTrackerOperations.GetHigh32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void GetLow32(Node node)
	{
		BitTrackerOperations.GetLow32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void LoadParamZeroExtend16x32(Node node)
	{
		BitTrackerOperations.LoadParamZeroExtend16x32(node.Result.BitValue);
	}

	private static void LoadParamZeroExtend16x64(Node node)
	{
		BitTrackerOperations.LoadParamZeroExtend16x64(node.Result.BitValue);
	}

	private static void LoadParamZeroExtend8x32(Node node)
	{
		BitTrackerOperations.LoadParamZeroExtend8x32(node.Result.BitValue);
	}

	private static void LoadParamZeroExtend8x64(Node node)
	{
		BitTrackerOperations.LoadParamZeroExtend8x64(node.Result.BitValue);
	}

	private static void LoadParamZeroExtend32x64(Node node)
	{
		BitTrackerOperations.LoadParamZeroExtend32x64(node.Result.BitValue);
	}

	private static void LoadZeroExtend16x32(Node node)
	{
		BitTrackerOperations.LoadZeroExtend16x32(node.Result.BitValue);
	}

	private static void LoadZeroExtend16x64(Node node)
	{
		BitTrackerOperations.LoadZeroExtend16x64(node.Result.BitValue);
	}

	private static void LoadZeroExtend8x32(Node node)
	{
		BitTrackerOperations.LoadZeroExtend8x32(node.Result.BitValue);
	}

	private static void LoadZeroExtend8x64(Node node)
	{
		BitTrackerOperations.LoadZeroExtend8x64(node.Result.BitValue);
	}

	private static void LoadZeroExtend32x64(Node node)
	{
		BitTrackerOperations.LoadZeroExtend32x64(node.Result.BitValue);
	}

	private static void And32(Node node)
	{
		BitTrackerOperations.And32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void And64(Node node)
	{
		BitTrackerOperations.And64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Neg32(Node node)
	{
		BitTrackerOperations.Neg32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void Neg64(Node node)
	{
		BitTrackerOperations.Neg64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void Not32(Node node)
	{
		BitTrackerOperations.Not32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void Not64(Node node)
	{
		BitTrackerOperations.Not64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void Or32(Node node)
	{
		BitTrackerOperations.Or32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Or64(Node node)
	{
		BitTrackerOperations.Or64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Xor32(Node node)
	{
		BitTrackerOperations.Xor32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Xor64(Node node)
	{
		BitTrackerOperations.Xor64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Move32(Node node)
	{
		BitTrackerOperations.Move32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void Move64(Node node)
	{
		BitTrackerOperations.Move64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void MulSigned32(Node node)
	{
		BitTrackerOperations.MulSigned32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void MulSigned64(Node node)
	{
		BitTrackerOperations.MulSigned64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void MulUnsigned32(Node node)
	{
		BitTrackerOperations.MulUnsigned32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void MulUnsigned64(Node node)
	{
		BitTrackerOperations.MulUnsigned64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void RemUnsigned32(Node node)
	{
		BitTrackerOperations.RemUnsigned32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void RemUnsigned64(Node node)
	{
		BitTrackerOperations.RemUnsigned64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void RemSigned32(Node node)
	{
		BitTrackerOperations.RemSigned32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void RemSigned64(Node node)
	{
		BitTrackerOperations.RemSigned64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void ShiftLeft32(Node node)
	{
		BitTrackerOperations.ShiftLeft32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void ShiftLeft64(Node node)
	{
		BitTrackerOperations.ShiftLeft64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void ShiftRight32(Node node)
	{
		BitTrackerOperations.ShiftRight32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void ShiftRight64(Node node)
	{
		BitTrackerOperations.ShiftRight64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void SignExtend8x32(Node node)
	{
		BitTrackerOperations.SignExtend8x32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void SignExtend16x32(Node node)
	{
		BitTrackerOperations.SignExtend16x32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void SignExtend8x64(Node node)
	{
		BitTrackerOperations.SignExtend8x64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void SignExtend16x64(Node node)
	{
		BitTrackerOperations.SignExtend16x64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void SignExtend32x64(Node node)
	{
		BitTrackerOperations.SignExtend32x64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void To64(Node node)
	{
		BitTrackerOperations.To64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Truncate64x32(Node node)
	{
		BitTrackerOperations.Truncate64x32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void ZeroExtend8x32(Node node)
	{
		BitTrackerOperations.ZeroExtend8x32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void ZeroExtend16x32(Node node)
	{
		BitTrackerOperations.ZeroExtend16x32(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void ZeroExtend8x64(Node node)
	{
		BitTrackerOperations.ZeroExtend8x64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void ZeroExtend16x64(Node node)
	{
		BitTrackerOperations.ZeroExtend16x64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void ZeroExtend32x64(Node node)
	{
		BitTrackerOperations.ZeroExtend32x64(node.Result.BitValue, node.Operand1.BitValue);
	}

	private static void IfThenElse32(Node node)
	{
		BitTrackerOperations.IfThenElse32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.Operand3.BitValue);
	}

	private static void IfThenElse64(Node node)
	{
		BitTrackerOperations.IfThenElse64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue, node.Operand3.BitValue);
	}

	private static void NewString(Node node)
	{
		BitTrackerOperations.NewString(node.Result.BitValue);
	}

	private static void NewObject(Node node)
	{
		BitTrackerOperations.NewObject(node.Result.BitValue);
	}

	private static void NewArray(Node node)
	{
		BitTrackerOperations.NewArray(node.Result.BitValue);
	}

	private static void DivUnsigned32(Node node)
	{
		BitTrackerOperations.DivUnsigned32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void DivUnsigned64(Node node)
	{
		BitTrackerOperations.DivUnsigned64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void DivSigned32(Node node)
	{
		BitTrackerOperations.DivSigned32(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void DivSigned64(Node node)
	{
		BitTrackerOperations.DivSigned64(node.Result.BitValue, node.Operand1.BitValue, node.Operand2.BitValue);
	}

	private static void Result2NarrowToBoolean(Node node)
	{
		BitTrackerOperations.Result2NarrowToBoolean(node.Result2.BitValue);
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

	#endregion IR Instructions
}
