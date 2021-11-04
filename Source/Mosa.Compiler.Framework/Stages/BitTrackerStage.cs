// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Mosa.Compiler.Framework.Trace;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.Stages
{
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

		private Counter BranchesRemovedCount = new Counter("BitTrackerStage.BranchesRemoved");
		private Counter InstructionsRemovedCount = new Counter("BitTrackerStage.InstructionsRemoved");
		private Counter InstructionsUpdatedCount = new Counter("BitTrackerStage.InstructionsUpdated");
		private TraceLog trace;

		private NodeVisitationDelegate[] visitation = new NodeVisitationDelegate[MaxInstructions];
		private NodeVisitationDelegate2[] visitation2 = new NodeVisitationDelegate2[MaxInstructions];

		private delegate BitValue NodeVisitationDelegate(InstructionNode node);

		private delegate (BitValue, BitValue) NodeVisitationDelegate2(InstructionNode node);

		protected override void Finish()
		{
			trace = null;
		}

		protected override void Initialize()
		{
			visitation = new NodeVisitationDelegate[MaxInstructions];
			visitation2 = new NodeVisitationDelegate2[MaxInstructions];

			Register(InstructionsUpdatedCount);
			Register(InstructionsRemovedCount);
			Register(BranchesRemovedCount);

			Register(IRInstruction.Phi32, Phi32);
			Register(IRInstruction.Phi64, Phi64);

			Register(IRInstruction.Move32, MoveInt32);
			Register(IRInstruction.Move64, MoveInt64);

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

			// TODO:

			// AddCarryOut32
			// AddCarryOut64
			// AddCarryIn32
			// AddCarryIn64

			// DivUnsigned32
			// DivUnsigned64

			// LoadParamSignExtend16x32
			// LoadParamSignExtend16x64
			// LoadParamSignExtend32x64
			// LoadParamSignExtend8x32
			// LoadParamSignExtend8x64

			// LoadSignExtend16x32
			// LoadSignExtend16x64
			// LoadSignExtend32x64
			// LoadSignExtend8x32
			// LoadSignExtend8x64

			// RemSigned32
			// RemSigned64

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

		private void Register2(BaseInstruction instruction, NodeVisitationDelegate2 method)
		{
			visitation2[instruction.ID] = method;
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
				var value = virtualRegister.BitValue;

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
			foreach (var register in MethodCompiler.VirtualRegisters)
			{
				register.BitValue = null;
			}

			bool change = true;

			while (change)
			{
				change = false;

				foreach (var register in MethodCompiler.VirtualRegisters)
				{
					change |= Evaluate(register);
				}
			}
		}

		private bool Evaluate(Operand virtualRegister)
		{
			// already evaluated
			if (virtualRegister.BitValue != null)
				return false;

			if (virtualRegister.IsFloatingPoint || !(virtualRegister.IsInteger || virtualRegister.IsReferenceType))
				return false;

			if (virtualRegister.Definitions.Count != 1)
			{
				virtualRegister.BitValue = Any(virtualRegister);
				return true;
			}

			var node = virtualRegister.Definitions[0];

			if (node.ResultCount == 0)
				return false;

			if (!(node.Result.IsInteger || node.Result.IsReferenceType))
			{
				virtualRegister.BitValue = Any(virtualRegister);
				return true;
			}

			if (node.Instruction.FlowControl == FlowControl.Call)
			{
				virtualRegister.BitValue = Any(virtualRegister);
				return true;
			}

			var method = visitation[node.Instruction.ID];

			if (method == null)
			{
				virtualRegister.BitValue = Any(virtualRegister);
				return true;
			}

			if (!node.Instruction.IsMemoryRead)
			{
				// check dependencies
				foreach (var operand in node.Operands)
				{
					if (!operand.IsVirtualRegister)
						continue;

					if (!(operand.IsInteger || operand.IsReferenceType))
						continue;

					if (operand.BitValue == null)
						return false; // can not evaluate yet
				}
			}

			var value = method.Invoke(node);
			virtualRegister.BitValue = value;

			UpdateInstruction(node.Result, value);
			return true;
		}

		private BitValue Any(Operand virtualRegister)
		{
			if (virtualRegister.IsInteger)
				return virtualRegister.IsInteger32 ? BitValue.Any32 : BitValue.Any64;
			else if (virtualRegister.IsReferenceType)
				return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
			else if (virtualRegister.IsR4)
				return BitValue.Any32;
			else if (virtualRegister.IsR8)
				return BitValue.Any64;

			throw new InvalidProgramException();
		}

		private void UpdateInstruction(Operand virtualRegister, BitValue value)
		{
			Debug.Assert(!virtualRegister.IsFloatingPoint);
			Debug.Assert(virtualRegister.Definitions.Count == 1);

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

				var is32Bit = node.Instruction == IRInstruction.Branch32 || (node.Instruction == IRInstruction.BranchObject && Is32BitPlatform);

				var result = EvaluateCompare(node, is32Bit);

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

		private bool? EvaluateCompare(InstructionNode node, bool is32Bit)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

			switch (node.ConditionCode)
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
					else if (value1.MaxValue < value2.MinValue)
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
					else if (value1.MaxValue < value2.MinValue)
					{
						return true;
					}
					else if (value1.MinValue > value2.MaxValue)
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
					else if (value1.MaxValue <= value2.MinValue)
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
					else if (value1.MaxValue <= value2.MinValue)   // correct
					{
						return true;
					}
					else if (value1.MinValue >= value2.MaxValue)
					{
						return false;
					}

					break;

				case ConditionCode.Greater:
					if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && !value1.Is32Bit)
					{
						return (long)value1.BitsSet > (long)value2.BitsSet;
					}
					else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit)
					{
						return (int)value1.BitsSet > (int)value2.BitsSet;
					}

					break;

				case ConditionCode.Less:
					if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && !value1.Is32Bit)
					{
						return (long)value1.BitsSet < (long)value2.BitsSet;
					}
					else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit)
					{
						return (int)value1.BitsSet < (int)value2.BitsSet;
					}
					break;

				case ConditionCode.GreaterOrEqual:
					if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && !value1.Is32Bit)
					{
						return (long)value1.BitsSet >= (long)value2.BitsSet;
					}
					else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit)
					{
						return (int)value1.BitsSet >= (int)value2.BitsSet;
					}
					break;

				case ConditionCode.LessOrEqual:
					if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && !value1.Is32Bit)
					{
						return (long)value1.BitsSet <= (long)value2.BitsSet;
					}
					else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value1.Is32Bit)
					{
						return (int)value1.BitsSet <= (int)value2.BitsSet;
					}
					break;

				default:
					return null;
			}

			return null;
		}

		private BitValue GetValue(Operand operand)
		{
			var value = operand.BitValue;

			if (value == null)
				if (operand.IsReferenceType)
					if (operand.IsNull)
						return Is32BitPlatform ? BitValue.Zero32 : BitValue.Zero64;
					else
						return Is32BitPlatform ? BitValue.Any32 : BitValue.Any64;
				else
					return operand.IsInteger32 ? BitValue.Any32 : BitValue.Any64;
			else
				return value;
		}

		#region IR Instructions

		private BitValue Add32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

			if (IntegerTwiddling.IsAddOverflow((uint)value1.MaxValue, (uint)value2.MaxValue) || value1.MaxValue > uint.MaxValue || value2.MaxValue > uint.MaxValue)
			{
				return BitValue.Any32;
			}

			if (IntegerTwiddling.IsAddOverflow((uint)value1.MinValue, (uint)value2.MinValue) || value1.MaxValue > uint.MaxValue || value2.MaxValue > uint.MaxValue)
			{
				return BitValue.Any32;
			}

			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: Upper32BitsSet | BitTwiddling.GetClearBitsOver(value1.MaxValue + value2.MaxValue),
				maxValue: value1.MaxValue + value2.MaxValue,
				minValue: value1.MinValue + value2.MinValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue Add64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

			if (IntegerTwiddling.IsAddOverflow(value1.MaxValue, value2.MaxValue))
			{
				return BitValue.Any64;
			}

			if (IntegerTwiddling.IsAddOverflow(value1.MinValue, value2.MinValue))
			{
				return BitValue.Any64;
			}

			return BitValue.CreateValue(
				bitsSet: 0,
				bitsClear: BitTwiddling.GetClearBitsOver(value1.MaxValue + value2.MaxValue),
				maxValue: value1.MaxValue + value2.MaxValue,
				minValue: value1.MinValue + value2.MinValue,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		private BitValue AddCarryIn32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);
			var value3 = GetValue(node.Operand3);

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

		private BitValue ArithShiftRight32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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
					bitsClear: (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | highbits | Upper32BitsSet,
					maxValue: (value1.MaxValue >> shift) | highbits,
					minValue: (value1.MinValue >> shift) | highbits,
					rangeDeterminate: true,
					is32Bit: true
			);
			}

			return BitValue.Any32;
		}

		private BitValue ArithShiftRight64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

		private BitValue Compare32x32(InstructionNode node)
		{
			var result = EvaluateCompare(node, true);

			if (!result.HasValue)
				return BitValue.Any32;

			return BitValue.CreateValue(result.Value, true);
		}

		private BitValue Compare64x32(InstructionNode node)
		{
			var result = EvaluateCompare(node, false);

			if (!result.HasValue)
				return BitValue.Any32;

			return BitValue.CreateValue(result.Value, true);
		}

		private BitValue Compare64x64(InstructionNode node)
		{
			var result = EvaluateCompare(node, false);

			if (!result.HasValue)
				return BitValue.Any64;

			return BitValue.CreateValue(result.Value, false);
		}

		private BitValue GetHigh32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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
				bitsClear: (value1.BitsClear >> 32) | Upper32BitsSet,
				maxValue: value1.MaxValue >> 32,
				minValue: value1.MinValue >> 32,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue GetLow32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			if (value1.AreLower32BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet & uint.MaxValue, true);
			}

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet,
				bitsClear: value1.BitsClear,
				maxValue: value1.MaxValue,
				minValue: value1.MinValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue LoadParamZeroExtend16x32(InstructionNode node)
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

		private BitValue LoadParamZeroExtend16x64(InstructionNode node)
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

		private BitValue LoadParamZeroExtend8x32(InstructionNode node)
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

		private BitValue LoadParamZeroExtend8x64(InstructionNode node)
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

		private BitValue LoadParamZeroExtend32x64(InstructionNode node)
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

		private BitValue LoadZeroExtend16x32(InstructionNode node)
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

		private BitValue LoadZeroExtend16x64(InstructionNode node)
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

		private BitValue LoadZeroExtend8x32(InstructionNode node)
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

		private BitValue LoadZeroExtend8x64(InstructionNode node)
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

		private BitValue LoadZeroExtend32x64(InstructionNode node)
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

		private BitValue And32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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
				maxValue: value1.MaxValue & value2.MaxValue,
				minValue: value1.MinValue & value2.MinValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue And64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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
				maxValue: value1.MaxValue & value2.MaxValue,
				minValue: value1.MinValue & value2.MinValue,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		private BitValue Not32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue Not64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue Or32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
			{
				return value1;
			}

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet | value2.BitsSet,
				bitsClear: value2.BitsClear & value1.BitsClear,
				maxValue: value1.MaxValue | value2.MaxValue,
				minValue: value1.MinValue | value2.MinValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue Or64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value1.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == ulong.MaxValue)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == ulong.MaxValue)
			{
				return value1;
			}

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet | value2.BitsSet,
				bitsClear: value2.BitsClear & value1.BitsClear,
				maxValue: value1.MaxValue | value2.MaxValue,
				minValue: value1.MinValue | value2.MinValue,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		private BitValue Xor32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

		private BitValue Xor64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

		private BitValue MoveInt32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			if (value1.AreLower32BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32, true);
			}

			return value1;
		}

		private BitValue MoveInt64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			return value1;
		}

		private BitValue MulSigned32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return BitValue.CreateValue((ulong)((int)value1.BitsSet32 * (int)value2.BitsSet32), true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 1)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 1)
			{
				return value1;
			}

			// TODO: Special power of two handling for bits, handle similar to shift left

			if (!IntegerTwiddling.HasSignBitSet32((int)value1.MaxValue)
				&& !IntegerTwiddling.HasSignBitSet32((int)value2.MaxValue)
				&& !IntegerTwiddling.HasSignBitSet32((int)value1.MinValue)
				&& !IntegerTwiddling.HasSignBitSet32((int)value2.MinValue)
				&& !IntegerTwiddling.IsMultiplyOverflow((int)value1.MaxValue, (int)value2.MaxValue))
			{
				var max = Math.Max(value1.MaxValue, value2.MaxValue);
				var min = Math.Min(value1.MinValue, value2.MinValue);
				var uppermax = max * max;

				return BitValue.CreateValue(
					bitsSet: 0,
					bitsClear: Upper32BitsSet | BitTwiddling.GetClearBitsOver((uint)(uppermax)),
					maxValue: (uint)(max * max),
					minValue: (uint)(min * min),
					rangeDeterminate: true,
					is32Bit: true
			);
			}

			return BitValue.Any32;
		}

		private BitValue MulSigned64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return BitValue.CreateValue((ulong)((long)value1.BitsSet * (long)value2.BitsSet), false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return BitValue.Zero64;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return BitValue.Zero64;
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 1)
			{
				return value2;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 1)
			{
				return value1;
			}

			// TODO: Special power of two handling for bits, handle similar to shift left

			if (!IntegerTwiddling.HasSignBitSet64((long)value1.MaxValue)
				&& !IntegerTwiddling.HasSignBitSet64((long)value2.MaxValue)
				&& !IntegerTwiddling.HasSignBitSet64((long)value1.MinValue)
				&& !IntegerTwiddling.HasSignBitSet64((long)value2.MinValue)
				&& !IntegerTwiddling.IsMultiplyOverflow((long)value1.MaxValue, (long)value2.MaxValue))
			{
				var max = Math.Max(value1.MaxValue, value2.MaxValue);
				var min = Math.Min(value1.MinValue, value2.MinValue);
				var uppermax = max * max;

				return BitValue.CreateValue(
					bitsSet: 0,
					bitsClear: BitTwiddling.GetClearBitsOver(uppermax),
					maxValue: max * max,
					minValue: min * min,
					rangeDeterminate: false,
					is32Bit: false
			);
			}

			return BitValue.Any64;
		}

		private BitValue MulUnsigned32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32 * value2.BitsSet32, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

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
				bitsClear: Upper32BitsSet | BitTwiddling.GetClearBitsOver(value1.MaxValue * value2.MaxValue),
				maxValue: (uint)(value1.MaxValue * value2.MaxValue),
				minValue: (uint)(value1.MinValue * value2.MinValue),
				rangeDeterminate: !IntegerTwiddling.IsMultiplyOverflow((uint)value1.MaxValue, (uint)value2.MaxValue),
				is32Bit: true
			);
		}

		private BitValue MulUnsigned64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet * value2.BitsSet, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return BitValue.Zero64;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return BitValue.Zero64;
			}

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
				bitsClear: BitTwiddling.GetClearBitsOver(value1.MaxValue * value2.MaxValue),
				maxValue: value1.MaxValue * value2.MaxValue,
				minValue: value1.MinValue * value2.MinValue,
				rangeDeterminate: !IntegerTwiddling.IsMultiplyOverflow(value1.MaxValue, value2.MaxValue),
				is32Bit: false
		);
		}

		private BitValue Phi32(InstructionNode node)
		{
			Debug.Assert(node.OperandCount != 0);

			var value1 = GetValue(node.Operand1);

			ulong max = value1.MaxValue;
			ulong min = value1.MinValue;
			ulong bitsset = value1.BitsSet;
			ulong bitsclear = value1.BitsClear;

			for (int i = 1; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);
				var value = GetValue(operand);

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

		private BitValue Phi64(InstructionNode node)
		{
			Debug.Assert(node.OperandCount != 0);

			var value1 = GetValue(node.Operand1);

			ulong max = value1.MaxValue;
			ulong min = value1.MinValue;
			ulong bitsset = value1.BitsSet;
			ulong bitsclear = value1.BitsClear;

			for (int i = 1; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);
				var value = GetValue(operand);

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

		private BitValue RemUnsigned32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value2.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				// divide by zero!
				return BitValue.Any32;
			}

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32 % value2.BitsSet32, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
			{
				return BitValue.CreateValue(
					bitsSet: 0,
					bitsClear: BitTwiddling.GetClearBitsOver(value2.BitsSet - 1),
					maxValue: value2.BitsSet - 1,
					minValue: 0,
					rangeDeterminate: true,
					is32Bit: true
			);
			}

			return BitValue.Any32;
		}

		private BitValue RemUnsigned64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			if (value2.AreAll64BitsKnown && value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
			{
				// divide by zero!
				return BitValue.Any64;
			}

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet % value2.BitsSet, true);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
			{
				return BitValue.Zero64;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet32 != 0)
			{
				return BitValue.CreateValue(
					bitsSet: 0,
					bitsClear: BitTwiddling.GetClearBitsOver(value2.BitsSet - 1),
					maxValue: value2.BitsSet - 1,
					minValue: 0,
					rangeDeterminate: true,
					is32Bit: false
			);
			}

			return BitValue.Any64;
		}

		private BitValue ShiftLeft32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			var shift = (int)(value2.BitsSet & 0b11111);

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32 << shift, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

			if (value2.AreLower5BitsKnown && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower5BitsKnown && shift != 0)
			{
				return BitValue.CreateValue(
					bitsSet: value1.BitsSet << shift,
					bitsClear: value1.BitsClear << shift | ~(ulong.MaxValue << shift) | Upper32BitsSet,
					maxValue: value1.MaxValue << shift,
					minValue: value1.MinValue << shift,
					rangeDeterminate: true,
					is32Bit: true
				);
			}

			// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively

			return BitValue.Any32;
		}

		private BitValue ShiftLeft64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			var shift = (int)(value2.BitsSet & 0b111111);

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32 << shift, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return BitValue.Zero64;
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
					maxValue: value1.MaxValue << shift,
					minValue: value1.MinValue << shift,
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

		private BitValue ShiftRight32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			var shift = (int)(value2.BitsSet & 0b11111);

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32 >> shift, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return BitValue.Zero32;
			}

			if (value2.AreLower5BitsKnown && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower5BitsKnown && shift != 0)
			{
				return BitValue.CreateValue(
					bitsSet: value1.BitsSet >> shift,
					bitsClear: (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | Upper32BitsSet,
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

		private BitValue ShiftRight64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

			var shift = (int)(value2.BitsSet & 0b111111);

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet >> shift, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return BitValue.Zero64;
			}

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

		private BitValue SignExtend16x32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue SignExtend16x64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue SignExtend32x64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue SignExtend8x32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue SignExtend8x64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

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

		private BitValue To64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);
			var value2 = GetValue(node.Operand2);

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

		private BitValue Truncate64x32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet,
				bitsClear: value1.BitsClear | Upper32BitsSet,
				maxValue: value1.MaxValue & uint.MaxValue,
				minValue: value1.MinValue & uint.MaxValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue ZeroExtend16x32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			if (value1.AreLower16BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet16, true);
			}

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet16,
				bitsClear: value1.BitsClear | Upper48BitsSet,
				maxValue: value1.MaxValue & ushort.MaxValue,
				minValue: value1.MinValue & ushort.MaxValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue ZeroExtend16x64(InstructionNode node)
		{
			return ZeroExtend16x32(node);
		}

		private BitValue ZeroExtend32x64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			if (value1.AreLower32BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet32, false);
			}

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet32,
				bitsClear: value1.BitsClear | Upper32BitsSet,
				maxValue: value1.MaxValue & uint.MaxValue,
				minValue: value1.MinValue & uint.MaxValue,
				rangeDeterminate: true,
				is32Bit: false
			);
		}

		private BitValue ZeroExtend8x32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand1);

			if (value1.AreLower8BitsKnown)
			{
				return BitValue.CreateValue(value1.BitsSet8, true);
			}

			return BitValue.CreateValue(
				bitsSet: value1.BitsSet8,
				bitsClear: value1.BitsClear | Upper56BitsSet,
				maxValue: value1.MaxValue & byte.MaxValue,
				minValue: value1.MinValue & byte.MaxValue,
				rangeDeterminate: true,
				is32Bit: true
			);
		}

		private BitValue ZeroExtend8x64(InstructionNode node)
		{
			return ZeroExtend8x32(node);
		}

		private BitValue IfThenElse32(InstructionNode node)
		{
			var value1 = GetValue(node.Operand2);
			var value2 = GetValue(node.Operand3);

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

		private BitValue IfThenElse64(InstructionNode node)
		{
			var value1 = GetValue(node.Operand2);
			var value2 = GetValue(node.Operand3);

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

		private BitValue NewString(InstructionNode node)
		{
			return Is32BitPlatform ? BitValue.AnyExceptZero32 : BitValue.AnyExceptZero64;
		}

		private BitValue NewObject(InstructionNode node)
		{
			return Is32BitPlatform ? BitValue.AnyExceptZero32 : BitValue.AnyExceptZero64;
		}

		private BitValue NewArray(InstructionNode node)
		{
			return Is32BitPlatform ? BitValue.AnyExceptZero32 : BitValue.AnyExceptZero64;
		}

		#endregion IR Instructions
	}
}
