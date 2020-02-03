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

		#region Internal Value Class

		private struct Value
		{
			public static Value Indeterminate = new Value() { AreRangeValuesDeterminate = false, BitsSet = 0, BitsClear = 0, IsEvaluated = true };

			public static Value Any32 = new Value() { MaxValue = uint.MaxValue, MinValue = 0, AreRangeValuesDeterminate = true, BitsSet = 0, BitsClear = 0, IsEvaluated = true };
			public static Value Any64 = new Value() { MaxValue = ulong.MaxValue, MinValue = 0, AreRangeValuesDeterminate = true, BitsSet = 0, BitsClear = 0, IsEvaluated = true };

			public bool AreRangeValuesDeterminate;
			public ulong BitsClear;
			public ulong BitsSet;
			public ulong MaxValue;
			public ulong MinValue;

			public Value(ulong value, bool is32Bit)
			{
				MaxValue = value;
				MinValue = value;
				BitsSet = value;
				BitsClear = ~value;
				AreRangeValuesDeterminate = true;
				IsEvaluated = true;

				if (is32Bit)
					Set32Bit();
			}

			public Value(uint value)
			{
				MaxValue = value;
				MinValue = value;
				BitsSet = value;
				BitsClear = ~value;
				AreRangeValuesDeterminate = true;
				IsEvaluated = true;

				Set32Bit();
			}

			public bool AreAll64BitsKnown { get { return (BitsKnown & ulong.MaxValue) == ulong.MaxValue; } }
			public bool AreBitsDeterminate { get { return !AreBitsIndeterminate; } }
			public bool AreBitsIndeterminate { get { return BitsClear == 0 && BitsSet == 0; } }
			public bool AreLower16BitsKnown { get { return (BitsKnown & ushort.MaxValue) == ushort.MaxValue; } }
			public bool AreLower32BitsKnown { get { return (BitsKnown & uint.MaxValue) == uint.MaxValue; } }
			public bool AreLower5BitsKnown { get { return (BitsKnown & 0b11111) == 0b11111; } }
			public bool AreLower6BitsKnown { get { return (BitsKnown & 0b111111) == 0b111111; } }
			public bool AreLower8BitsKnown { get { return (BitsKnown & byte.MaxValue) == byte.MaxValue; } }
			public bool AreUpper32BitsKnown { get { return (BitsKnown & Upper32BitsSet) == Upper32BitsSet; } }
			public bool AreRangeValuesIndeterminate { get { return !AreRangeValuesDeterminate; } }
			public ulong BitsKnown { get { return BitsSet | BitsClear; } }
			public bool Is32Bit { set { IsEvaluated = true; Set32Bit(); } }
			public bool Is64Bit { set { IsEvaluated = true; } }
			public bool IsEvaluated { get; private set; }
			public bool IsIndeterminate { get { return AreRangeValuesIndeterminate && AreBitsIndeterminate; } }

			public uint BitsClear32 { get { return (uint)BitsClear; } }
			public uint BitsSet32 { get { return (uint)BitsSet; } }

			public byte BitsClear8 { get { return (byte)BitsClear; } }
			public byte BitsSet8 { get { return (byte)BitsSet; } }

			public ushort BitsClear16 { get { return (ushort)BitsClear; } }
			public ushort BitsSet16 { get { return (ushort)BitsSet; } }

			//public ulong BitsKnown32 { get { return (uint)BitsKnown; } }

			public override string ToString()
			{
				if (!IsEvaluated)
					return "Not Evaluated";

				var sb = new StringBuilder();

				if (AreRangeValuesDeterminate)
				{
					sb.Append($" MaxValue: {MaxValue.ToString()}");
					sb.Append($" MinValue: {MinValue.ToString()}");
				}

				if (AreBitsDeterminate)
				{
					sb.Append($" BitsSet: {Convert.ToString((long)BitsSet, 2).PadLeft(64, '0')}");
					sb.Append($" BitsClear: {Convert.ToString((long)BitsClear, 2).PadLeft(64, '0')}");
					sb.Append($" BitsKnown: {Convert.ToString((long)BitsKnown, 2).PadLeft(64, '0')}");
				}

				return sb.ToString();
			}

			private void Set32Bit()
			{
				MaxValue &= uint.MaxValue; MinValue &= uint.MaxValue; BitsSet &= uint.MaxValue; BitsClear |= Upper32BitsSet;
			}
		}

		#endregion Internal Value Class

		private Counter BranchesRemovedCount = new Counter("BitTrackerStage.BranchesRemoved");
		private Counter InstructionsRemovedCount = new Counter("BitTrackerStage.InstructionsRemoved");
		private Counter InstructionsUpdatedCount = new Counter("BitTrackerStage.InstructionsUpdated");
		private TraceLog trace;
		private Value[] Values;
		private NodeVisitationDelegate[] visitation = new NodeVisitationDelegate[MaxInstructions];
		private NodeVisitationDelegate2[] visitation2 = new NodeVisitationDelegate2[MaxInstructions];

		private delegate Value NodeVisitationDelegate(InstructionNode node);

		private delegate (Value, Value) NodeVisitationDelegate2(InstructionNode node);

		private HashSet<BaseInstruction> IntegerLoads = new HashSet<BaseInstruction>();

		protected override void Finish()
		{
			Values = null;
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

			Register(IRInstruction.GetLow64, GetLow64);
			Register(IRInstruction.GetHigh64, GetHigh64);
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

			// TODO:

			// AddCarryOut32
			// AddCarryOut64
			// AddCarryIn32
			// AddCarryIn64

			// DivUnsigned32
			// DivUnsigned64

			// IfThenElse32
			// IfThenElse64

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

			IntegerLoads.Add(IRInstruction.LoadParamSignExtend16x32);
			IntegerLoads.Add(IRInstruction.LoadParamSignExtend16x64);
			IntegerLoads.Add(IRInstruction.LoadParamSignExtend32x64);
			IntegerLoads.Add(IRInstruction.LoadParamSignExtend8x32);
			IntegerLoads.Add(IRInstruction.LoadParamSignExtend8x64);
			IntegerLoads.Add(IRInstruction.LoadSignExtend16x32);
			IntegerLoads.Add(IRInstruction.LoadSignExtend16x64);
			IntegerLoads.Add(IRInstruction.LoadSignExtend32x64);
			IntegerLoads.Add(IRInstruction.LoadSignExtend8x32);
			IntegerLoads.Add(IRInstruction.LoadSignExtend8x64);
			IntegerLoads.Add(IRInstruction.LoadZeroExtend8x32);
			IntegerLoads.Add(IRInstruction.LoadZeroExtend16x32);
			IntegerLoads.Add(IRInstruction.LoadZeroExtend8x64);
			IntegerLoads.Add(IRInstruction.LoadZeroExtend16x64);
			IntegerLoads.Add(IRInstruction.LoadZeroExtend32x64);
			IntegerLoads.Add(IRInstruction.LoadParamZeroExtend8x32);
			IntegerLoads.Add(IRInstruction.LoadParamZeroExtend16x32);
			IntegerLoads.Add(IRInstruction.LoadParamZeroExtend8x64);
			IntegerLoads.Add(IRInstruction.LoadParamZeroExtend16x64);
			IntegerLoads.Add(IRInstruction.LoadParamZeroExtend32x64);
			IntegerLoads.Add(IRInstruction.Load32);
			IntegerLoads.Add(IRInstruction.Load64);
			IntegerLoads.Add(IRInstruction.LoadParam32);
			IntegerLoads.Add(IRInstruction.LoadParam64);
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

			Values = new Value[MethodCompiler.VirtualRegisters.Count + 1];  // 0 entry is not used

			EvaluateVirtualRegisters();

			UpdateBranchInstructions();

			DumpValues();
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
				var value = Values[virtualRegister.Index];

				valueTrace?.Log($"Virtual Register: {virtualRegister.ToString()}");

				if (virtualRegister.Definitions.Count == 1)
				{
					valueTrace?.Log($"Definition: {virtualRegister.Definitions[0].ToString()}");
				}

				if (!value.IsEvaluated)
				{
					valueTrace?.Log($"*** NOT EVALUATED");
				}

				if (value.IsIndeterminate)
				{
					valueTrace?.Log($"*** INDETERMINATE");
				}

				if (value.AreRangeValuesDeterminate)
				{
					valueTrace?.Log($"  MaxValue:  {value.MaxValue.ToString()}");
					valueTrace?.Log($"  MinValue:  {value.MinValue.ToString()}");
				}

				if (value.AreBitsDeterminate)
				{
					valueTrace?.Log($"  BitsSet:   {Convert.ToString((long)value.BitsSet, 2).PadLeft(64, '0')}");
					valueTrace?.Log($"  BitsClear: {Convert.ToString((long)value.BitsClear, 2).PadLeft(64, '0')}");
					valueTrace?.Log($"  BitsKnown: {Convert.ToString((long)value.BitsKnown, 2).PadLeft(64, '0')}");
				}

				valueTrace?.Log();
			}
		}

		private bool Evaluate(Operand virtualRegister)
		{
			int index = virtualRegister.Index;

			if (Values[index].IsEvaluated)
				return false;

			if (virtualRegister.IsR || virtualRegister.Definitions.Count != 1 || virtualRegister.Definitions.Count == 0)
			{
				if (virtualRegister.IsInteger)
				{
					Values[index] = virtualRegister.Is64BitInteger ? Value.Any64 : Value.Any32;
					return true;
				}

				Values[index] = Value.Indeterminate;
				return true;
			}

			var node = virtualRegister.Definitions[0];

			var integerLoad = node.Result.IsInteger && IntegerLoads.Contains(node.Instruction);

			if (!integerLoad)
			{
				// check dependencies
				foreach (var operand in node.Operands)
				{
					if (operand.IsVirtualRegister)
					{
						var operandValue = Values[operand.Index];

						if (operandValue.IsEvaluated)
							continue;

						return false; // can not evaluate yet
					}

					if (operand.IsResolvedConstant && operand.IsInteger)
						continue;

					// everything else we assume is indeterminate
					Values[index] = Value.Indeterminate;

					return true;
				}
			}

			if (node.ResultCount == 1)
			{
				var method = visitation[node.Instruction.ID];

				if (method != null)
				{
					var value = method.Invoke(node);
					Values[index] = value;

					if (!value.IsIndeterminate)
					{
						UpdateInstruction(node.Result, value);
					}

					return true;
				}
			}
			else if (node.ResultCount == 2)
			{
				// TODO: This is a little more complicated

				//var method2 = visitation2[node.Instruction.ID];

				//if (method2 != null)
				//{
				//	var values = method2.Invoke(node);
				//	var value1 = values.Item1;
				//	var value2 = values.Item1;

				//Values[index] = value1;
				//Values[index] = value2;

				//if (!value1.IsIndeterminate)
				//{
				//	UpdateInstruction(node.Result, value1);
				//}

				//if (!value2.IsIndeterminate)
				//{
				//	UpdateInstruction(node.Result2, value2);
				//}

				//return true;
				//}
			}

			if (virtualRegister.IsInteger)
			{
				Values[index] = virtualRegister.Is64BitInteger ? Value.Any64 : Value.Any32;
				return true;
			}

			Values[index] = Value.Indeterminate;
			return true;
		}

		private void EvaluateVirtualRegisters()
		{
			int count = MethodCompiler.VirtualRegisters.Count;
			int evaluated = 0;

			while (evaluated != count)
			{
				int startedAt = evaluated;

				for (int i = 0; i < count; i++)
				{
					var virtualRegister = MethodCompiler.VirtualRegisters[i];

					if (Evaluate(virtualRegister))
					{
						evaluated++;

						if (evaluated == count)
							return;
					}
				}

				// cycle detected - exit
				if (startedAt == evaluated)
				{
					return;
				}
			}
		}

		private void UpdateInstruction(Operand virtualRegister, Value value)
		{
			Debug.Assert(!value.IsIndeterminate);
			Debug.Assert(value.IsEvaluated);
			Debug.Assert(!virtualRegister.IsR);
			Debug.Assert(virtualRegister.Definitions.Count == 1);

			var node = virtualRegister.Definitions[0];

			ulong replaceValue;

			if (value.AreAll64BitsKnown)
			{
				replaceValue = value.BitsSet;
			}
			else if (value.AreRangeValuesDeterminate && value.MaxValue == value.MinValue)
			{
				replaceValue = value.MaxValue;
			}
			else
			{
				return;
			}

			var constantOperand = CreateConstant(virtualRegister.Type, replaceValue);

			if (trace != null)
			{
				trace?.Log($"Virtual Register: {virtualRegister}");

				if (!value.AreRangeValuesIndeterminate)
				{
					trace?.Log($"  MaxValue:  {value.MaxValue.ToString()}");
					trace?.Log($"  MinValue:  {value.MinValue.ToString()}");
				}

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
				InstructionsUpdatedCount++;
			}

			Debug.Assert(virtualRegister.Uses.Count == 0);

			trace?.Log($"REMOVED:\t{node}");
			node.SetInstruction(IRInstruction.Nop);
			trace?.Log();

			InstructionsRemovedCount++;
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

				Debug.Assert(node.Instruction == IRInstruction.CompareBranch32 || node.Instruction == IRInstruction.CompareBranch64);

				var result = EvaluateCompare(node);

				if (!result.HasValue)
					continue;

				var newBranch = node.BranchTargets[0];

				trace?.Log($"REMOVED:\t{node}");
				node.SetInstruction(IRInstruction.Nop);
				InstructionsRemovedCount++;
				BranchesRemovedCount++;

				if (!result.Value)
				{
					continue;
				}

				while (node.IsEmptyOrNop)
				{
					node = node.Next;
				}

				trace?.Log($"BEFORE:\t{node}");
				node.UpdateBranchTarget(0, newBranch);
				trace?.Log($"AFTER:\t{node}");
			}
		}

		private bool? EvaluateCompare(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			var conditionCode = node.ConditionCode;

			bool hasResult = false;
			bool result = false;

			ulong maxValue1 = value1.MaxValue;
			ulong maxValue2 = value2.MaxValue;
			ulong minValue1 = value1.MinValue;
			ulong minValue2 = value2.MinValue;

			ulong bitsSet1 = value1.BitsSet;
			ulong bitsSet2 = value2.BitsSet;

			ulong bitsClear1 = value1.BitsClear;
			ulong bitsClear2 = value2.BitsClear;

			bool areRangeValuesDeterminate1 = value1.AreRangeValuesDeterminate && value1.IsEvaluated;
			bool areRangeValuesDeterminate2 = value2.AreRangeValuesDeterminate && value2.IsEvaluated;

			bool areBitsIndeterminate1 = value1.AreBitsIndeterminate && value1.IsEvaluated;
			bool areBitsIndeterminate2 = value2.AreBitsIndeterminate && value2.IsEvaluated;

			if (conditionCode == ConditionCode.Equal)
			{
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					hasResult = true;
					result = maxValue1 == maxValue2;
				}
				else if (areRangeValuesDeterminate1 && areRangeValuesDeterminate2 && maxValue1 == minValue1 && maxValue1 == maxValue2 && minValue1 == minValue2)
				{
					hasResult = true;
					result = true;
				}
				else if ((((bitsSet1 & bitsSet2) != bitsSet1) || ((bitsClear1 & bitsClear2) != bitsClear1)) && areBitsIndeterminate1 && areBitsIndeterminate2)
				{
					hasResult = true;
					result = false;
				}
			}
			else if (conditionCode == ConditionCode.NotEqual)
			{
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					hasResult = true;
					result = maxValue1 != maxValue2;
				}
				else if (areRangeValuesDeterminate1 && areRangeValuesDeterminate2 && maxValue1 == minValue1 && maxValue1 == maxValue2 && minValue1 == minValue2)
				{
					hasResult = true;
					result = false;
				}
				else if (value1.AreAll64BitsKnown && maxValue1 == 0 && bitsSet2 != 0)
				{
					hasResult = true;
					result = true;
				}
				else if (value2.AreAll64BitsKnown && maxValue2 == 0 && bitsSet1 != 0)
				{
					hasResult = true;
					result = true;
				}
			}
			else if (conditionCode == ConditionCode.UnsignedGreaterThan)
			{
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					hasResult = true;
					result = maxValue1 > maxValue2;
				}
				else if (value2.AreAll64BitsKnown && maxValue2 == 0 && bitsSet1 != 0)
				{
					hasResult = true;
					result = true;
				}
			}
			else if (conditionCode == ConditionCode.UnsignedLessThan)
			{
				if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
				{
					hasResult = true;
					result = maxValue1 < maxValue2;
				}
				else if (value1.AreAll64BitsKnown && maxValue1 == 0 && bitsSet2 != 0)
				{
					hasResult = true;
					result = true;
				}
			}

			if (!hasResult)
				return null;

			return result;
		}

		#region Helpers

		private static bool IsAddOverflow(ulong a, ulong b)
		{
			return ((b > 0) && (a > (ulong.MaxValue - b)));
		}

		private static bool IsAddOverflow(uint a, uint b)
		{
			return ((b > 0) && (a > (uint.MaxValue - b)));
		}

		private static bool IsAddOverflow(uint a, uint b, bool carry)
		{
			if (IsAddOverflow(a, b))
				return true;

			if (carry == true & (a + b) == uint.MaxValue)
				return true;

			return false;
		}

		private static bool IsMultiplyOverflow(uint a, uint b)
		{
			var r = (ulong)a * (ulong)b;

			return r > uint.MaxValue;
		}

		private static bool IsMultiplyOverflow(int a, int b)
		{
			var z = a * b;
			return (b < 0 && a == Int32.MinValue) | (b != 0 && z / b != a);
		}

		private static bool IsMultiplyOverflow(ulong a, ulong b)
		{
			if (a == 0 | b == 0)
				return false;

			var r = a * b;
			var r2 = r / b;

			return r2 == a;
		}

		private static bool IsMultiplyOverflow(long a, long b)
		{
			var z = a * b;
			return (b < 0 && a == long.MinValue) | (b != 0 && z / b != a);
		}

		#endregion Helpers

		#region IR Instructions

		private Value Add32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32 + value2.BitsSet32, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				return value1;
			}

			return new Value()
			{
				MaxValue = value1.MaxValue + value2.MaxValue,
				MinValue = value1.MinValue + value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsAddOverflow((uint)value1.MaxValue, (uint)value2.MaxValue),
				BitsSet = 0,
				BitsClear = Upper32BitsSet | ((value1.AreRangeValuesIndeterminate || value2.AreRangeValuesIndeterminate) ? 0 : BitTwiddling.GetClearBitsOver((value1.MaxValue + value2.MaxValue))),
				Is32Bit = true
			};
		}

		private Value Add64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return new Value(value1.BitsSet + value2.BitsSet, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return value2;
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return value2;
			}

			return new Value()
			{
				MaxValue = value1.MaxValue + value2.MaxValue,
				MinValue = value1.MinValue + value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsAddOverflow(value1.MaxValue, value2.MaxValue),
				BitsSet = 0,
				BitsClear = (value1.AreRangeValuesIndeterminate || value2.AreRangeValuesIndeterminate) ? 0 : BitTwiddling.GetClearBitsOver(value1.MaxValue + value2.MaxValue),
				Is64Bit = true
			};
		}

		private Value AddCarryIn32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];
			var value3 = node.Operand3.IsConstant ? new Value(node.Operand3.ConstantUnsigned64, true) : Values[node.Operand3.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && value3.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32 + value2.BitsSet32 + value3.BitsSet32, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0 && value3.BitsSet32 == 0)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0 && value3.BitsSet32 == 0)
			{
				return value1;
			}

			return new Value()
			{
				MaxValue = value1.MaxValue + value2.MaxValue + value3.MaxValue,
				MinValue = value1.MinValue + value2.MinValue + value3.MaxValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsAddOverflow((uint)value1.MaxValue, (uint)value2.MaxValue, true),
				BitsSet = 0,
				BitsClear = Upper32BitsSet | ((value1.AreRangeValuesIndeterminate || value2.AreRangeValuesIndeterminate) ? 0 : BitTwiddling.GetClearBitsOver(value1.MaxValue + value2.MaxValue + 1)),
				Is32Bit = true
			};
		}

		private Value ArithShiftRight32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			var shift = (int)(value2.BitsSet & 0b11111);
			bool knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;
			bool signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;
			ulong highbits = (knownSignedBit && signed) ? ~(~uint.MaxValue >> shift) : 0;

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return new Value(value1.BitsSet32 >> shift | highbits, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower5BitsKnown && knownSignedBit && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower5BitsKnown && knownSignedBit && shift != 0)
			{
				return new Value()
				{
					MaxValue = (value1.MaxValue >> shift) | highbits,
					MinValue = (value1.MinValue >> shift) | highbits,
					AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
					BitsSet = value1.BitsSet >> shift | highbits,
					BitsClear = (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | highbits | Upper32BitsSet,
					Is32Bit = true
				};
			}

			return Value.Indeterminate;
		}

		private Value ArithShiftRight64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			var shift = (int)(value2.BitsSet & 0b111111);
			bool knownSignedBit = ((value1.BitsKnown >> 63) & 1) == 1;
			bool signed = ((value1.BitsSet >> 63) & 1) == 1 || ((value1.BitsClear >> 63) & 1) != 1;
			ulong highbits = (knownSignedBit && signed) ? ~(~ulong.MaxValue >> shift) : 0;

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return new Value(value1.BitsSet >> shift | highbits, true);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreAll64BitsKnown && knownSignedBit && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower6BitsKnown && knownSignedBit && shift != 0)
			{
				return new Value()
				{
					MaxValue = (value1.MaxValue >> shift) | highbits,
					MinValue = (value1.MinValue >> shift) | highbits,
					AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
					BitsSet = value1.BitsSet >> shift | highbits,
					BitsClear = (value1.BitsClear >> shift) | ~(ulong.MaxValue >> shift) | highbits,
					Is32Bit = false
				};
			}

			return Value.Indeterminate;
		}

		private Value Compare32x32(InstructionNode node)
		{
			var result = EvaluateCompare(node);

			if (!result.HasValue)
				return Value.Indeterminate;

			return new Value(result.Value ? 1u : 0u, false);
		}

		private Value Compare64x32(InstructionNode node)
		{
			return Compare64x64(node);
		}

		private Value Compare64x64(InstructionNode node)
		{
			var result = EvaluateCompare(node);

			if (!result.HasValue)
				return Value.Indeterminate;

			return new Value(result.Value ? 1u : 0u, true);
		}

		private Value GetHigh64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreUpper32BitsKnown)
			{
				return new Value(value1.BitsSet >> 32, true);
			}

			if (value1.AreRangeValuesDeterminate && value1.MaxValue <= uint.MaxValue)
			{
				return new Value(0, true);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue >> 32,
				MinValue = value1.MinValue >> 32,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet >> 32,
				BitsClear = (value1.BitsClear >> 32) | Upper32BitsSet,
				Is32Bit = true
			};
		}

		private Value GetLow64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet & uint.MaxValue, true);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet,
				BitsClear = value1.BitsClear,
				Is32Bit = true
			};
		}

		private Value LoadParamZeroExtend16x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = ushort.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(ushort.MaxValue),
				Is32Bit = true
			};
		}

		private Value LoadParamZeroExtend16x64(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = ushort.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(ushort.MaxValue),
				Is32Bit = false
			};
		}

		private Value LoadParamZeroExtend8x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = byte.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(byte.MaxValue),
				Is32Bit = true
			};
		}

		private Value LoadParamZeroExtend8x64(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = byte.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(byte.MaxValue),
				Is32Bit = false
			};
		}

		private Value LoadParamZeroExtend32x64(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = uint.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(uint.MaxValue),
				Is32Bit = false
			};
		}

		private Value LoadZeroExtend16x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = ushort.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(ushort.MaxValue),
				Is32Bit = true
			};
		}

		private Value LoadZeroExtend16x64(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = ushort.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(ushort.MaxValue),
				Is32Bit = false
			};
		}

		private Value LoadZeroExtend8x32(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = byte.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(byte.MaxValue),
				Is32Bit = true,
			};
		}

		private Value LoadZeroExtend8x64(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = byte.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(byte.MaxValue),
				Is32Bit = false,
			};
		}

		private Value LoadZeroExtend32x64(InstructionNode node)
		{
			return new Value()
			{
				MaxValue = uint.MaxValue,
				MinValue = 0,
				AreRangeValuesDeterminate = true,
				BitsSet = 0,
				BitsClear = ~(ulong)(uint.MaxValue),
				Is32Bit = false
			};
		}

		private Value And32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == 0)
			{
				return new Value(0, true);
			}

			if (value2.AreLower32BitsKnown && (value2.BitsSet & ulong.MaxValue) == 0)
			{
				return new Value(0, true);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & value2.MaxValue,
				MinValue = value1.MinValue & value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet & value2.BitsSet,
				BitsClear = value2.BitsClear | value1.BitsClear,
				Is32Bit = true
			};
		}

		private Value And64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0, true);
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return new Value(0, true);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & value2.MaxValue,
				MinValue = value1.MinValue & value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet & value2.BitsSet,
				BitsClear = value2.BitsClear | value1.BitsClear,
				Is64Bit = true
			};
		}

		private Value Not32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(~value1.BitsSet32, true);
			}

			return new Value()
			{
				MaxValue = 0,
				MinValue = 0,
				AreRangeValuesDeterminate = false,
				BitsSet = value1.BitsClear32,
				BitsClear = value1.BitsSet32,
				Is32Bit = true
			};
		}

		private Value Not64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];

			if (value1.AreAll64BitsKnown)
			{
				return new Value(~value1.BitsSet, false);
			}

			return new Value()
			{
				MaxValue = 0,
				MinValue = 0,
				AreRangeValuesDeterminate = false,
				BitsSet = value1.BitsClear,
				BitsClear = value1.BitsSet,
				Is64Bit = true
			};
		}

		private Value Or32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && value1.BitsSet32 == uint.MaxValue)
			{
				return value1;
			}

			return new Value()
			{
				MaxValue = (value1.MaxValue | value2.MaxValue),
				MinValue = (value1.MinValue | value2.MinValue),
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate,
				BitsSet = (value1.BitsSet | value2.BitsSet),
				BitsClear = (value2.BitsClear & value1.BitsClear),
				Is32Bit = true
			};
		}

		private Value Or64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == ulong.MaxValue)
			{
				return value2;
			}

			if (value2.AreLower32BitsKnown && (value1.BitsSet & ulong.MaxValue) == ulong.MaxValue)
			{
				return value1;
			}

			return new Value()
			{
				MaxValue = value1.MaxValue | value2.MaxValue,
				MinValue = value1.MinValue | value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet | value2.BitsSet,
				BitsClear = value2.BitsClear & value1.BitsClear,
				Is64Bit = true
			};
		}

		private Value Xor32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			ulong bitsKnown = value1.BitsKnown & value2.BitsKnown & uint.MaxValue;

			// TODO: if all bits known then min/max are also known

			return new Value()
			{
				MaxValue = 0,
				MinValue = 0,
				AreRangeValuesDeterminate = false,
				BitsSet = (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
				BitsClear = (value2.BitsClear ^ value1.BitsClear) & bitsKnown,
				Is32Bit = true
			};
		}

		private Value Xor64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			ulong bitsKnown = value1.BitsKnown & value2.BitsKnown;

			// TODO: if all bits known then min/max are also known

			return new Value()
			{
				MaxValue = 0,
				MinValue = 0,
				AreRangeValuesDeterminate = false,
				BitsSet = (value1.BitsSet ^ value2.BitsSet) & bitsKnown,
				BitsClear = (value2.BitsClear ^ value1.BitsClear) & bitsKnown,
				Is64Bit = true
			};
		}

		private Value MoveInt32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32, true);
			}

			return value1;
		}

		private Value MoveInt64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];

			return value1;
		}

		private Value MulSigned32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value((ulong)((int)value1.BitsSet32 * (int)value2.BitsSet32), true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				return new Value(0);
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

			//if (value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsMultiplyOverflow((long)value1.MaxValue, (long)value2.MaxValue) && !IsMultiplyOverflow((long)value1.MinValue, (long)value2.MinValue))
			//{
			//	return new Value()
			//	{
			//		MaxValue = (ulong)((long)value1.MaxValue * (long)value2.MaxValue),
			//		MinValue = (ulong)((long)value1.MinValue * (long)value2.MinValue),
			//		AreRangeValuesDeterminate = false,
			//		BitsSet = 0,
			//		BitsClear = 0,
			//		Is32Bit = true
			//	};
			//}

			return Value.Indeterminate;
		}

		private Value MulSigned64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return new Value((ulong)((long)value1.BitsSet * (long)value2.BitsSet), false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return new Value(0);
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

			if (value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsMultiplyOverflow((int)value1.MaxValue, (int)value2.MaxValue) && !IsMultiplyOverflow((int)value1.MinValue, (int)value2.MinValue))
			{
				return new Value()
				{
					MaxValue = (ulong)((int)value1.MaxValue * (int)value2.MaxValue),
					MinValue = (ulong)((int)value1.MinValue * (int)value2.MinValue),
					AreRangeValuesDeterminate = false,
					BitsSet = 0,
					BitsClear = 0,
					Is64Bit = true
				};
			}

			return Value.Indeterminate;
		}

		private Value MulUnsigned32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32 * value2.BitsSet32, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				return new Value(0);
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

			return new Value()
			{
				MaxValue = value1.MaxValue * value2.MaxValue,
				MinValue = value1.MinValue * value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsMultiplyOverflow((uint)value1.MaxValue, (uint)value2.MaxValue),
				BitsSet = 0,
				BitsClear = Upper32BitsSet | ((value1.AreRangeValuesIndeterminate || value2.AreRangeValuesIndeterminate) ? 0 : BitTwiddling.GetClearBitsOver(value1.MaxValue * value2.MaxValue)),
				Is32Bit = true
			};
		}

		private Value MulUnsigned64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return new Value(value1.BitsSet * value2.BitsSet, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
			{
				return new Value(0);
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

			return new Value()
			{
				MaxValue = value1.MaxValue * value2.MaxValue,
				MinValue = value1.MinValue * value2.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate && !IsMultiplyOverflow(value1.MaxValue, value2.MaxValue),
				BitsSet = 0,
				BitsClear = (value1.AreRangeValuesIndeterminate || value2.AreRangeValuesIndeterminate) ? 0 : BitTwiddling.GetClearBitsOver(value1.MaxValue * value2.MaxValue),
				Is64Bit = true
			};
		}

		private Value Phi32(InstructionNode node)
		{
			Debug.Assert(node.OperandCount != 0);

			var value = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned32, false) : Values[node.Operand1.Index];

			for (int i = 1; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);
				var value2 = operand.IsConstant ? new Value(operand.ConstantUnsigned32, false) : Values[operand.Index];

				value.MaxValue = Math.Max(value.MaxValue, value2.MaxValue);
				value.MinValue = Math.Min(value.MinValue, value2.MinValue);
				value.AreRangeValuesDeterminate = value.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate;
				value.BitsSet &= value2.BitsSet;
				value.BitsClear &= value2.BitsClear;
			}

			value.Is32Bit = true;

			return value;
		}

		private Value Phi64(InstructionNode node)
		{
			Debug.Assert(node.OperandCount != 0);

			var value = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];

			for (int i = 1; i < node.OperandCount; i++)
			{
				var operand = node.GetOperand(i);
				var value2 = operand.IsConstant ? new Value(operand.ConstantUnsigned64, false) : Values[operand.Index];

				value.MaxValue = Math.Max(value.MaxValue, value2.MaxValue);
				value.MinValue = Math.Min(value.MinValue, value2.MinValue);
				value.AreRangeValuesDeterminate = value.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate;
				value.BitsSet &= value2.BitsSet;
				value.BitsClear &= value2.BitsClear;
			}

			value.Is64Bit = true;

			return value;
		}

		private Value RemUnsigned32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value2.AreLower32BitsKnown && value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
			{
				// divide by zero!
				return Value.Indeterminate;
			}

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32 % value2.BitsSet32, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0, true);
			}

			if (value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
			{
				return new Value()
				{
					MaxValue = value2.BitsSet - 1,
					MinValue = 0,
					AreRangeValuesDeterminate = true,
					BitsSet = 0,
					BitsClear = BitTwiddling.GetClearBitsOver(value2.BitsSet - 1),
					Is32Bit = true
				};
			}

			return Value.Indeterminate;
		}

		private Value RemUnsigned64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value2.AreAll64BitsKnown && value2.AreAll64BitsKnown && value2.BitsSet32 == 0)
			{
				// divide by zero!
				return Value.Indeterminate;
			}

			if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
			{
				return new Value(value1.BitsSet % value2.BitsSet, true);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0, true);
			}

			if (value2.AreAll64BitsKnown && value2.BitsSet32 != 0)
			{
				return new Value()
				{
					MaxValue = value2.BitsSet - 1,
					MinValue = 0,
					AreRangeValuesDeterminate = true,
					BitsSet = 0,
					BitsClear = BitTwiddling.GetClearBitsOver(value2.BitsSet - 1),
					Is32Bit = false
				};
			}

			return Value.Indeterminate;
		}

		private Value ShiftLeft32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			var shift = (int)(value2.BitsSet & 0b11111);

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return new Value(value1.BitsSet32 << shift, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0, true);
			}

			if (value2.AreLower5BitsKnown && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower5BitsKnown && shift != 0)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue << shift,
					MinValue = value1.MinValue << shift,
					AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
					BitsSet = value1.BitsSet << shift,
					BitsClear = value1.BitsClear << shift | ~(ulong.MaxValue << shift) | Upper32BitsSet,
					Is32Bit = true
				};
			}

			// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively

			return Value.Indeterminate;
		}

		private Value ShiftLeft64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			var shift = (int)(value2.BitsSet & 0b111111);

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return new Value(value1.BitsSet32 << shift, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower6BitsKnown && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower6BitsKnown && shift != 0)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue << shift,
					MinValue = value1.MinValue << shift,
					AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
					BitsSet = value1.BitsSet << shift,
					BitsClear = (value1.BitsClear << shift) | ~(ulong.MaxValue << shift),
					Is64Bit = true
				};
			}

			if (value1.AreLower32BitsKnown && ((value1.BitsSet & uint.MaxValue) == 0))
			{
				return new Value()
				{
					MaxValue = ulong.MaxValue,
					MinValue = 0,
					AreRangeValuesDeterminate = true,
					BitsSet = 0,
					BitsClear = uint.MaxValue,
					Is64Bit = true
				};
			}

			// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively

			return Value.Indeterminate;
		}

		private Value ShiftRight32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			var shift = (int)(value2.BitsSet & 0b11111);

			if (value1.AreLower32BitsKnown && value2.AreLower5BitsKnown)
			{
				return new Value(value1.BitsSet32 >> shift, true);
			}

			if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower5BitsKnown && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower5BitsKnown && shift != 0)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue >> shift,
					MinValue = value1.MinValue >> shift,
					AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
					BitsSet = value1.BitsSet >> shift,
					BitsClear = (value1.BitsClear >> shift) | ~(uint.MaxValue >> shift) | Upper32BitsSet,
					Is32Bit = true
				};
			}

			// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively
			// FUTURE: If the shift has a range, and some bits are known --- then some bits might be determinable

			//if (value1.AreRangeValuesDeterminate)
			//{
			//	return new Value()
			//	{
			//		MaxValue = value1.MaxValue, // must be equal or less, but not more
			//		MinValue = 0,
			//		AreRangeValuesDeterminate = true,
			//		BitsSet = 0,
			//		BitsClear = 0,
			//		Is64Bit = false
			//	};
			//}

			return Value.Indeterminate;
		}

		private Value ShiftRight64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, false) : Values[node.Operand2.Index];

			var shift = (int)(value2.BitsSet & 0b111111);

			if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
			{
				return new Value(value1.BitsSet >> shift, false);
			}

			if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
			{
				return new Value(0);
			}

			if (value2.AreLower6BitsKnown && shift == 0)
			{
				return value1;
			}

			if (value2.AreLower6BitsKnown && shift != 0)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue >> shift,
					MinValue = value1.MinValue >> shift,
					AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
					BitsSet = value1.BitsSet >> shift,
					BitsClear = value1.BitsClear >> shift | ~(ulong.MaxValue >> shift),
					Is64Bit = true
				};
			}

			if (value1.AreUpper32BitsKnown && ((value1.BitsSet >> 32) == 0))
			{
				return new Value()
				{
					MaxValue = uint.MaxValue,
					MinValue = 0,
					AreRangeValuesDeterminate = true,
					BitsSet = 0,
					BitsClear = ~uint.MaxValue,
					Is64Bit = true
				};
			}

			// FUTURE: Using the known highest and lowers bit sequences, the bit sets and ranges can be set and narrower respectively
			// FUTURE: If the shift has a range, and some bits are known --- then some bits might be determinable

			//if (value1.AreRangeValuesDeterminate)
			//{
			//	return new Value()
			//	{
			//		MaxValue = value1.MaxValue, // must be equal or less, but not more
			//		MinValue = 0,
			//		AreRangeValuesDeterminate = true,
			//		BitsSet = 0,
			//		BitsClear = 0,
			//		Is64Bit = true
			//	};
			//}

			return Value.Indeterminate;
		}

		private Value SignExtend16x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower16BitsKnown)
			{
				return new Value(value1.BitsSet16 | ((((value1.BitsSet >> 15) & 1) == 1) ? Upper48BitsSet : 0), true);
			}

			bool knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = 0,
					MinValue = 0,
					AreRangeValuesDeterminate = false,
					BitsSet = value1.BitsSet16,
					BitsClear = value1.BitsClear16,
					Is32Bit = true
				};
			}

			bool signed = ((value1.BitsSet >> 15) & 1) == 1 || ((value1.BitsClear >> 15) & 1) != 1;

			if (!signed && value1.AreRangeValuesDeterminate)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					BitsClear = value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
					Is32Bit = true
				};
			}

			if (!signed)
			{
				return new Value()
				{
					MaxValue = (ulong)short.MaxValue,
					MinValue = unchecked((ulong)short.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					BitsClear = value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
					Is32Bit = true
				};
			}
			else
			{
				return new Value()
				{
					MaxValue = (ulong)short.MaxValue,
					MinValue = unchecked((ulong)short.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					BitsClear = value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
					Is32Bit = true
				};
			}
		}

		private Value SignExtend16x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower16BitsKnown)
			{
				return new Value(value1.BitsSet16 | ((((value1.BitsSet >> 15) & 1) == 1) ? Upper48BitsSet : 0), true);
			}

			bool knownSignedBit = ((value1.BitsKnown >> 15) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = 0,
					MinValue = 0,
					AreRangeValuesDeterminate = false,
					BitsSet = value1.BitsSet16,
					BitsClear = value1.BitsClear16,
					Is64Bit = true
				};
			}

			bool signed = ((value1.BitsSet >> 15) & 1) == 1 || ((value1.BitsClear >> 15) & 1) != 1;

			if (!signed && value1.AreRangeValuesDeterminate)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					BitsClear = value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
					Is64Bit = true
				};
			}

			if (!signed)
			{
				return new Value()
				{
					MaxValue = (ulong)short.MaxValue,
					MinValue = unchecked((ulong)short.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					BitsClear = value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
					Is64Bit = true
				};
			}
			else
			{
				return new Value()
				{
					MaxValue = (ulong)short.MaxValue,
					MinValue = unchecked((ulong)short.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet16 | (signed ? Upper48BitsSet : 0),
					BitsClear = value1.BitsClear16 | (signed ? 0 : Upper48BitsSet),
					Is64Bit = true
				};
			}
		}

		private Value SignExtend32x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32 | ((((value1.BitsSet >> 31) & 1) == 1) ? Upper32BitsSet : 0), false);
			}

			bool knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = 0,
					MinValue = 0,
					AreRangeValuesDeterminate = false,
					BitsSet = value1.BitsSet32,
					BitsClear = value1.BitsClear32,
					Is64Bit = true
				};
			}

			bool signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;

			if (!signed && value1.AreRangeValuesDeterminate)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet32 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear32 | (signed ? 0 : Upper56BitsSet),
					Is64Bit = true
				};
			}

			if (!signed)
			{
				return new Value()
				{
					MaxValue = (ulong)int.MaxValue,
					MinValue = unchecked((ulong)int.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet32 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear32 | (signed ? 0 : Upper56BitsSet),
					Is64Bit = true
				};
			}
			else
			{
				return new Value()
				{
					MaxValue = (ulong)int.MaxValue,
					MinValue = unchecked((ulong)int.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet32 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear32 | (signed ? 0 : Upper56BitsSet),
					Is64Bit = true
				};
			}
		}

		private Value SignExtend8x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower8BitsKnown)
			{
				return new Value(value1.BitsSet16 | ((((value1.BitsSet >> 7) & 1) == 1) ? Upper56BitsSet : 0), true);
			}

			bool knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = 0,
					MinValue = 0,
					AreRangeValuesDeterminate = false,
					BitsSet = value1.BitsSet8,
					BitsClear = value1.BitsClear8,
					Is32Bit = true
				};
			}

			bool signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;

			if (!signed && value1.AreRangeValuesDeterminate)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
					Is32Bit = true
				};
			}

			if (!signed)
			{
				return new Value()
				{
					MaxValue = (ulong)sbyte.MaxValue,
					MinValue = unchecked((ulong)sbyte.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
					Is32Bit = true
				};
			}
			else
			{
				return new Value()
				{
					MaxValue = (ulong)sbyte.MaxValue,
					MinValue = unchecked((ulong)sbyte.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
					Is32Bit = true
				};
			}
		}

		private Value SignExtend8x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, false) : Values[node.Operand1.Index];

			if (value1.AreLower8BitsKnown)
			{
				return new Value(value1.BitsSet16 | ((((value1.BitsSet >> 7) & 1) == 1) ? Upper56BitsSet : 0), false);
			}

			bool knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

			if (!knownSignedBit)
			{
				return new Value()
				{
					MaxValue = 0,
					MinValue = 0,
					AreRangeValuesDeterminate = false,
					BitsSet = value1.BitsSet8,
					BitsClear = value1.BitsClear8,
					Is64Bit = true
				};
			}

			bool signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;

			if (!signed && value1.AreRangeValuesDeterminate)
			{
				return new Value()
				{
					MaxValue = value1.MaxValue,
					MinValue = value1.MinValue,
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
					Is64Bit = true
				};
			}

			if (!signed)
			{
				return new Value()
				{
					MaxValue = (ulong)sbyte.MaxValue,
					MinValue = unchecked((ulong)sbyte.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
					Is64Bit = true
				};
			}
			else
			{
				return new Value()
				{
					MaxValue = (ulong)sbyte.MaxValue,
					MinValue = unchecked((ulong)sbyte.MinValue),
					AreRangeValuesDeterminate = false, // ??
					BitsSet = value1.BitsSet8 | (signed ? Upper56BitsSet : 0),
					BitsClear = value1.BitsClear8 | (signed ? 0 : Upper56BitsSet),
					Is64Bit = true
				};
			}
		}

		private Value To64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];
			var value2 = node.Operand2.IsConstant ? new Value(node.Operand2.ConstantUnsigned64, true) : Values[node.Operand2.Index];

			if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
			{
				return new Value(value2.MaxValue << 32 | (value1.MaxValue & uint.MaxValue), false);
			}

			return new Value()
			{
				MaxValue = (value2.MaxValue << 32) | (value1.MaxValue & uint.MaxValue),
				MinValue = (value2.MinValue << 32) | (value1.MinValue & uint.MaxValue),
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate && value2.AreRangeValuesDeterminate,
				BitsSet = (value2.BitsSet << 32) | value1.BitsSet32,
				BitsClear = (value2.BitsClear << 32) | (value1.BitsClear32),
				Is64Bit = true
			};
		}

		private Value Truncate64x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			return new Value()
			{
				MaxValue = value1.MaxValue,
				MinValue = value1.MinValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet,
				BitsClear = value1.BitsClear | Upper32BitsSet,
				Is32Bit = true
			};
		}

		private Value ZeroExtend16x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower16BitsKnown)
			{
				return new Value(value1.BitsSet16, true);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & ushort.MaxValue,
				MinValue = value1.MinValue & ushort.MaxValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet16,
				BitsClear = value1.BitsClear | Upper48BitsSet,
				Is32Bit = true
			};
		}

		private Value ZeroExtend16x64(InstructionNode node)
		{
			return ZeroExtend16x32(node);
		}

		private Value ZeroExtend32x64(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower32BitsKnown)
			{
				return new Value(value1.BitsSet32, false);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & uint.MaxValue,
				MinValue = value1.MinValue & uint.MaxValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet32,
				BitsClear = value1.BitsClear | Upper32BitsSet,
				Is64Bit = true
			};
		}

		private Value ZeroExtend8x32(InstructionNode node)
		{
			var value1 = node.Operand1.IsConstant ? new Value(node.Operand1.ConstantUnsigned64, true) : Values[node.Operand1.Index];

			if (value1.AreLower8BitsKnown)
			{
				return new Value(value1.BitsSet8, true);
			}

			return new Value()
			{
				MaxValue = value1.MaxValue & byte.MaxValue,
				MinValue = value1.MinValue & byte.MaxValue,
				AreRangeValuesDeterminate = value1.AreRangeValuesDeterminate,
				BitsSet = value1.BitsSet8,
				BitsClear = value1.BitsClear | Upper56BitsSet,
				Is32Bit = true
			};
		}

		private Value ZeroExtend8x64(InstructionNode node)
		{
			return ZeroExtend8x32(node);
		}

		#endregion IR Instructions
	}
}
