// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework.Analysis;

/// <summary>
/// Bit Tracker Operations - Contains all bit tracking operation logic for updating BitValues.
/// Each method takes BitValue operands directly (no Node dependency), making them testable in isolation.
/// </summary>
public static class BitTrackerOperations
{
	private const ulong Upper32BitsSet = ~(ulong)uint.MaxValue;
	private const ulong Upper48BitsSet = ~(ulong)ushort.MaxValue;
	private const ulong Upper56BitsSet = ~(ulong)byte.MaxValue;

	public static void Add32(BitValue result, BitValue value1, BitValue value2)
	{
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
		else if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue))
		{
			result.SetStable(value1, value2);
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MinValue, (uint)value2.MinValue))
		{
			result.SetStable(value1, value2);
		}
		else
		{
			result
				.NarrowMin(value1.MinValue + value2.MinValue)
				.NarrowMax(value1.MaxValue + value2.MaxValue)
				.SetStable(value1, value2);
		}
	}

	public static void Add64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet + value2.BitsSet);
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
			result.SetStable(value1, value2);
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry(value1.MinValue, value2.MinValue))
		{
			result.SetStable(value1, value2);
		}
		else
		{
			result
				.NarrowMin(value1.MinValue + value2.MinValue)
				.NarrowMax(value1.MaxValue + value2.MaxValue)
				.SetStable(value1, value2);
		}
	}

	public static void AddCarryIn32(BitValue result, BitValue value1, BitValue value2, BitValue value3)
	{
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

	public static void AddCarryIn64(BitValue result, BitValue value1, BitValue value2, BitValue value3)
	{
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value3.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet + value2.BitsSet + value3.BitsSet);
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 0 && value3.AreAll64BitsKnown && value3.BitsSet == 0)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0 && value3.AreAll64BitsKnown && value3.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2, value3);
		}
	}

	public static void Sub32(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 - value2.BitsSet32);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void Sub64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet - value2.BitsSet);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void SubCarryIn32(BitValue result, BitValue value1, BitValue value2, BitValue value3)
	{
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && value3.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 - value2.BitsSet32 - value3.BitsSet32);
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

	public static void SubCarryIn64(BitValue result, BitValue value1, BitValue value2, BitValue value3)
	{
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && value3.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet - value2.BitsSet - value3.BitsSet);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0 && value3.AreAll64BitsKnown && value3.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2, value3);
		}
	}

	public static void ArithShiftRight32(BitValue result, BitValue value1, BitValue value2)
	{
		var shift = (int)(value2.BitsSet & 0b11111);
		var knownSignedBit = ((value1.BitsKnown >> 31) & 1) == 1;
		var signed = ((value1.BitsSet >> 31) & 1) == 1 || ((value1.BitsClear >> 31) & 1) != 1;
		var signMask = ~(uint.MaxValue >> shift);
		var highbits = knownSignedBit && signed ? signMask : 0;

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
				.NarrowSetBits((value1.BitsSet32 >> shift) | highbits)
				.NarrowClearBits(Upper32BitsSet | ((ulong)value1.BitsClear32 >> shift) | (!signed ? signMask : 0))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void ArithShiftRight64(BitValue result, BitValue value1, BitValue value2)
	{
		var shift = (int)(value2.BitsSet & 0b111111);
		var knownSignedBit = ((value1.BitsKnown >> 63) & 1) == 1;
		var signed = ((value1.BitsSet >> 63) & 1) == 1 || ((value1.BitsClear >> 63) & 1) != 1;
		var signMask = ~(ulong.MaxValue >> shift);
		var highbits = knownSignedBit && signed ? signMask : 0;

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
				.NarrowClearBits((value1.BitsClear >> shift) | (!signed ? signMask : 0))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void Compare(BitValue result, BitValue value1, BitValue value2, ConditionCode conditionCode)
	{
		var compare = BaseTransform.EvaluateCompare(value1, value2, conditionCode);

		if (compare.HasValue)
		{
			result.SetValue(compare.Value);
		}
		else
		{
			result
				.NarrowToBoolean()
				.SetStable(value1, value2);
		}
	}

	public static void GetHigh32(BitValue result, BitValue value1)
	{
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

	public static void GetLow32(BitValue result, BitValue value1)
	{
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

	public static void LoadParamZeroExtend16x32(BitValue result)
	{
		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	public static void LoadParamZeroExtend16x64(BitValue result)
	{
		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	public static void LoadParamZeroExtend8x32(BitValue result)
	{
		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	public static void LoadParamZeroExtend8x64(BitValue result)
	{
		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	public static void LoadParamZeroExtend32x64(BitValue result)
	{
		result
			.NarrowMax(uint.MaxValue)
			.NarrowClearBits(~uint.MaxValue);
	}

	public static void LoadZeroExtend16x32(BitValue result)
	{
		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	public static void LoadZeroExtend16x64(BitValue result)
	{
		result
			.NarrowMax(ushort.MaxValue)
			.NarrowClearBits(~(ulong)ushort.MaxValue);
	}

	public static void LoadZeroExtend8x32(BitValue result)
	{
		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	public static void LoadZeroExtend8x64(BitValue result)
	{
		result
			.NarrowMax(byte.MaxValue)
			.NarrowClearBits(~(ulong)byte.MaxValue);
	}

	public static void LoadZeroExtend32x64(BitValue result)
	{
		result
			.NarrowMax(uint.MaxValue)
			.NarrowClearBits(~uint.MaxValue);
	}

	public static void And32(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsSet & value2.BitsSet)
				.NarrowClearBits(value2.BitsClear | value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	public static void And64(BitValue result, BitValue value1, BitValue value2)
	{
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
				.NarrowSetBits(value1.BitsSet & value2.BitsSet)
				.NarrowClearBits(value2.BitsClear | value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	public static void Neg32(BitValue result, BitValue value1)
	{
		if (value1.AreLower32BitsKnown)
		{
			result.SetValue((ulong)(-(int)value1.BitsSet32));
		}
		else
		{
			result.SetStable(value1);
		}
	}

	public static void Neg64(BitValue result, BitValue value1)
	{
		if (value1.AreAll64BitsKnown)
		{
			result.SetValue((ulong)(-(long)value1.BitsSet));
		}
		else
		{
			result.SetStable(value1);
		}
	}

	public static void Not32(BitValue result, BitValue value1)
	{
		if (value1.AreLower32BitsKnown)
		{
			result.SetValue(~value1.BitsSet32);
		}
		else
		{
			result
				.NarrowSetBits(value1.BitsClear32)
				.NarrowClearBits(value1.BitsSet32)
				.SetStable(value1);
		}
	}

	public static void Not64(BitValue result, BitValue value1)
	{
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

	public static void Or32(BitValue result, BitValue value1, BitValue value2)
	{
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
				.NarrowSetBits(value1.BitsSet | value2.BitsSet)
				.NarrowClearBits(value2.BitsClear & value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	public static void Or64(BitValue result, BitValue value1, BitValue value2)
	{
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
				.NarrowSetBits(value1.BitsSet | value2.BitsSet)
				.NarrowClearBits(value2.BitsClear & value1.BitsClear)
				.SetStable(value1, value2);
		}
	}

	public static void Xor32(BitValue result, BitValue value1, BitValue value2)
	{
		if (ReferenceEquals(value1, value2))
		{
			result.SetValue(0);
		}
		else
		{
			result
			.NarrowBits(value1.BitsSet ^ value2.BitsSet, value1.BitsKnown & value2.BitsKnown)
			.SetStable(value1, value2);
		}
	}

	public static void Xor64(BitValue result, BitValue value1, BitValue value2)
	{
		if (ReferenceEquals(value1, value2))
		{
			result.SetValue(0);
		}
		else
		{
			result
				.NarrowBits(value1.BitsSet ^ value2.BitsSet, value1.BitsKnown & value2.BitsKnown)
				.SetStable(value1, value2);
		}
	}

	public static void Move32(BitValue result, BitValue value1)
	{
		result.Narrow(value1).SetStable();
	}

	public static void Move64(BitValue result, BitValue value1)
	{
		result.Narrow(value1).SetStable();
	}

	public static void MulSigned32(BitValue result, BitValue value1, BitValue value2)
	{
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
			var uppermax = value1.MaxValue * value2.MaxValue;

			result
				.NarrowMin(value1.MinValue * value2.MinValue)
				.NarrowMax(uppermax)
				.NarrowClearBits(Upper32BitsSet | BitTwiddling.GetBitsOver((uint)uppermax))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void MulSigned64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
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
			var uppermax = value1.MaxValue * value2.MaxValue;

			result
				.NarrowMin(value1.MinValue * value2.MinValue)
				.NarrowMax(uppermax)
				.NarrowClearBits(BitTwiddling.GetBitsOver(uppermax))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void MulUnsigned32(BitValue result, BitValue value1, BitValue value2)
	{
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
				.NarrowClearBits(Upper32BitsSet)
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

	public static void MulUnsigned64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
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

	public static void RemUnsigned32(BitValue result, BitValue value1, BitValue value2)
	{
		if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			return;
		}
		else if (value2.IsZero) // value2 range is [0,0]; bit tracking may not have narrowed it yet
		{
			return;
		}
		else if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 % value2.BitsSet32);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 != 0)
		{
			result
				.NarrowMin(0)
				.NarrowMax(value2.BitsSet32 - 1)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.BitsSet32 - 1))
				.SetStable(value1, value2);
		}
		else if (value2.MinValue > value1.MaxValue)
		{
			result.Narrow(value1).SetStable(value1, value2);
		}
		else
		{
			result
				.NarrowMin(0)
				.NarrowMax(value2.MaxValue - 1)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.MaxValue - 1))
				.SetStable(value1, value2);
		}
	}

	public static void RemUnsigned64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value2.IsZero || value2.MaxValue == 0) // divide by zero
		{
			return;
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet % value2.BitsSet);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet != 0)
		{
			result
				.NarrowMin(0)
				.NarrowMax(value2.MaxValue - 1)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.MaxValue - 1))
				.SetStable(value1, value2);
		}
		else if (value2.MinValue > value1.MaxValue)
		{
			result.Narrow(value1).SetStable(value1, value2);
		}
		else
		{
			result
				.NarrowMin(0)
				.NarrowMax(value2.MaxValue - 1)
				.NarrowClearBits(BitTwiddling.GetBitsOver(value2.MaxValue - 1))
				.SetStable(value1, value2);
		}
	}

	public static void RemSigned32(BitValue result, BitValue value1, BitValue value2)
	{
		var isUnsigned = !IntegerTwiddling.HasSignBitSet((int)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MinValue);

		if (isUnsigned)
		{
			RemUnsigned32(result, value1, value2);
			return;
		}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			return;
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && !value2.IsZero)
		{
			var dividend = (long)(int)value1.BitsSet32;
			var divisor = (long)(int)value2.BitsSet32;
			var remainder = dividend % divisor;
			result.SetValue((ulong)(int)remainder);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void RemSigned64(BitValue result, BitValue value1, BitValue value2)
	{
		var isUnsigned = !IntegerTwiddling.HasSignBitSet((long)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value2.MinValue);

		if (isUnsigned)
		{
			RemUnsigned64(result, value1, value2);
			return;
		}

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			return;
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && !value2.IsZero)
		{
			result.SetValue((ulong)((long)value1.BitsSet % (long)value2.BitsSet));
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void ShiftLeft32(BitValue result, BitValue value1, BitValue value2)
	{
		var shift = (int)(value2.BitsSet & 0b11111);

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
			var shiftedMin = value1.MinValue << shift;
			var shiftedMax = value1.MaxValue << shift;
			var narrowMin = shiftedMin <= uint.MaxValue ? shiftedMin : 0;
			var narrowMax = shiftedMax <= uint.MaxValue ? shiftedMax : uint.MaxValue;

			result
				.NarrowMin(narrowMin)
				.NarrowMax(narrowMax)
				.NarrowSetBits(value1.BitsSet << shift)
				.NarrowClearBits(Upper32BitsSet | (value1.BitsClear << shift) | ~(ulong.MaxValue << shift))
				.SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void ShiftLeft64(BitValue result, BitValue value1, BitValue value2)
	{
		var shift = (int)(value2.BitsSet & 0b111111);

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreAll64BitsKnown && value2.AreLower6BitsKnown)
		{
			result.SetValue(value1.BitsSet << shift);
		}
		else if (value2.AreLower6BitsKnown && shift == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else if (value2.AreLower6BitsKnown && shift != 0)
		{
			var shiftedMin = value1.MinValue << shift;
			var shiftedMax = value1.MaxValue << shift;
			var narrowMin = shiftedMin >= value1.MinValue ? shiftedMin : 0;
			var narrowMax = shiftedMax >= value1.MaxValue ? shiftedMax : ulong.MaxValue;

			result
				.NarrowMin(narrowMin)
				.NarrowMax(narrowMax)
				.NarrowSetBits(value1.BitsSet << shift)
				.NarrowClearBits((value1.BitsClear << shift) | ~(ulong.MaxValue << shift))
				.SetStable(value1, value2);
		}
		else if (value1.AreLower32BitsKnown && (value1.BitsSet & uint.MaxValue) == 0)
		{
			result.NarrowClearBits(uint.MaxValue).SetStable(value1, value2);
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void ShiftRight32(BitValue result, BitValue value1, BitValue value2)
	{
		var shift = (int)(value2.BitsSet & 0b11111);

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

	public static void ShiftRight64(BitValue result, BitValue value1, BitValue value2)
	{
		var shift = (int)(value2.BitsSet & 0b111111);

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
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
				.SetStable(value1, value2);
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

	public static void SignExtend8x32(BitValue result, BitValue value1)
	{
		var knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;
		var signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;

		if (value1.AreLower8BitsKnown)
		{
			result.SetValue(value1.BitsSet8 | (((value1.BitsSet >> 7) & 1) == 1 ? Upper56BitsSet : 0));
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

	public static void SignExtend16x32(BitValue result, BitValue value1)
	{
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

	public static void SignExtend16x64(BitValue result, BitValue value1)
	{
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

	public static void SignExtend8x64(BitValue result, BitValue value1)
	{
		var signed = ((value1.BitsSet >> 7) & 1) == 1 || ((value1.BitsClear >> 7) & 1) != 1;
		var knownSignedBit = ((value1.BitsKnown >> 7) & 1) == 1;

		if (value1.AreLower8BitsKnown)
		{
			result.SetValue(value1.BitsSet8 | (((value1.BitsSet >> 7) & 1) == 1 ? Upper56BitsSet : 0));
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

	public static void SignExtend32x64(BitValue result, BitValue value1)
	{
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
				.NarrowSetBits(value1.BitsSet32 | (signed ? Upper32BitsSet : 0))
				.NarrowClearBits(value1.BitsClear32 | (signed ? 0 : Upper32BitsSet))
				.SetStable(value1);
		}
	}

	public static void To64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue((ulong)value2.BitsSet32 << 32 | value1.BitsSet32);
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

	public static void Truncate64x32(BitValue result, BitValue value1)
	{
		result
			.NarrowMin(value1.MinValue > uint.MaxValue ? 0 : value1.MinValue)
			.NarrowMax(Math.Min(uint.MaxValue, value1.MaxValue))
			.NarrowSetBits(value1.BitsSet & uint.MaxValue)
			.NarrowClearBits(Upper32BitsSet | value1.BitsClear)
			.SetStable(value1);
	}

	public static void ZeroExtend8x32(BitValue result, BitValue value1)
	{
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

	public static void ZeroExtend16x32(BitValue result, BitValue value1)
	{
		if (value1.AreLower16BitsKnown)
		{
			result.SetValue(value1.BitsSet16);
		}
		else
		{
			result
				.NarrowMin(value1.MinValue > ushort.MaxValue ? 0 : value1.MinValue)
				.NarrowMax(Math.Min(ushort.MaxValue, value1.MaxValue))
				.NarrowSetBits(value1.BitsSet16)
				.NarrowClearBits(value1.BitsClear | Upper48BitsSet)
				.SetStable(value1);
		}
	}

	public static void ZeroExtend16x64(BitValue result, BitValue value1)
	{
		ZeroExtend16x32(result, value1);
	}

	public static void ZeroExtend8x64(BitValue result, BitValue value1)
	{
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

	public static void ZeroExtend32x64(BitValue result, BitValue value1)
	{
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

	public static void IfThenElse32(BitValue result, BitValue condition, BitValue value1, BitValue value2)
	{
		if (condition.IsZero)
		{
			result.Narrow(value2).SetStable(condition, value2);
		}
		else if (condition.IsNotZero)
		{
			result.Narrow(value1).SetStable(condition, value1);
		}
		else
		{
			result
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.BitsSet & value2.BitsSet)
				.NarrowClearBits(value1.BitsClear & value2.BitsClear)
				.SetStable(condition, value1, value2);
		}
	}

	public static void IfThenElse64(BitValue result, BitValue condition, BitValue value1, BitValue value2)
	{
		if (condition.IsZero)
		{
			result.Narrow(value2).SetStable(condition, value2);
		}
		else if (condition.IsNotZero)
		{
			result.Narrow(value1).SetStable(condition, value1);
		}
		else
		{
			result
				.NarrowMin(Math.Min(value1.MinValue, value2.MinValue))
				.NarrowMax(Math.Max(value1.MaxValue, value2.MaxValue))
				.NarrowSetBits(value1.BitsSet & value2.BitsSet)
				.NarrowClearBits(value1.BitsClear & value2.BitsClear)
				.SetStable(condition, value1, value2);
		}
	}

	public static void NewString(BitValue result)
	{
		result.SetNotNull();
	}

	public static void NewObject(BitValue result)
	{
		result.SetNotNull();
	}

	public static void NewArray(BitValue result)
	{
		result.SetNotNull();
	}

	public static void DivUnsigned32(BitValue result, BitValue value1, BitValue value2)
	{
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

	public static void DivUnsigned64(BitValue result, BitValue value1, BitValue value2)
	{
		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			return;
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
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

	public static void DivSigned32(BitValue result, BitValue value1, BitValue value2)
	{
		var isUnsigned = !IntegerTwiddling.HasSignBitSet((int)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((int)value2.MinValue);

		if (isUnsigned)
		{
			DivUnsigned32(result, value1, value2);
			return;
		}

		if (value1.AreLower32BitsKnown && value1.BitsSet32 == 0)
		{
			result.SetValue(0);
		}
		else if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown && !value2.IsZero)
		{
			var dividend = (long)(int)value1.BitsSet32;
			var divisor = (long)(int)value2.BitsSet32;
			var quotient = dividend / divisor;
			result.SetValue((ulong)(int)quotient);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			return;
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void DivSigned64(BitValue result, BitValue value1, BitValue value2)
	{
		var isUnsigned = !IntegerTwiddling.HasSignBitSet((long)value1.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value2.MaxValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value1.MinValue)
			&& !IntegerTwiddling.HasSignBitSet((long)value2.MinValue);

		if (isUnsigned)
		{
			DivUnsigned64(result, value1, value2);
			return;
		}

		if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.SetValue(0);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			return;
		}
		else if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown && !value2.IsZero)
		{
			result.SetValue((ulong)((long)value1.BitsSet / (long)value2.BitsSet));
		}
		else
		{
			result.SetStable(value1, value2);
		}
	}

	public static void Result2NarrowToBoolean(BitValue result2)
	{
		result2.NarrowToBoolean().SetStable();
	}

	public static void AddCarryOut32(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the sum) using the same logic as Add32
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
		else if (!IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue))
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

		// Update result2 (the carry flag)
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsAddUnsignedCarry(value1.BitsSet32, value2.BitsSet32));
		}
		else if (!IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue))
		{
			result2.SetValue(0);
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MinValue, (uint)value2.MinValue))
		{
			result2.SetValue(1);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void AddCarryOut64(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the sum) using the same logic as Add64
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet + value2.BitsSet);
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
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

		// Update result2 (the carry flag)
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsAddUnsignedCarry(value1.BitsSet, value2.BitsSet));
		}
		else if (!IntegerTwiddling.IsAddUnsignedCarry(value1.MaxValue, value2.MaxValue))
		{
			result2.SetValue(0);
		}
		else if (IntegerTwiddling.IsAddUnsignedCarry(value1.MinValue, value2.MinValue))
		{
			result2.SetValue(1);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void AddOverflowOut32(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the sum) — same logic as Add32
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
		else if (!IntegerTwiddling.IsAddUnsignedCarry((uint)value1.MaxValue, (uint)value2.MaxValue))
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

		// Update result2 (the signed overflow flag)
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsAddSignedOverflow((int)value1.BitsSet32, (int)value2.BitsSet32));
		}
		else if (!IntegerTwiddling.IsAddSignedOverflow((int)(uint)value1.MaxValue, (int)(uint)value2.MaxValue)
			&& !IntegerTwiddling.IsAddSignedOverflow((int)(uint)value1.MinValue, (int)(uint)value2.MinValue))
		{
			result2.SetValue(0);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void AddOverflowOut64(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the sum) — same logic as Add64
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet + value2.BitsSet);
		}
		else if (value1.AreAll64BitsKnown && value1.BitsSet == 0)
		{
			result.Narrow(value2).SetStable(value2);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
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

		// Update result2 (the signed overflow flag)
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsAddSignedOverflow((long)value1.BitsSet, (long)value2.BitsSet));
		}
		else if (!IntegerTwiddling.IsAddSignedOverflow((long)value1.MaxValue, (long)value2.MaxValue)
			&& !IntegerTwiddling.IsAddSignedOverflow((long)value1.MinValue, (long)value2.MinValue))
		{
			result2.SetValue(0);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void SubCarryOut32(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the difference) — same logic as Sub32
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 - value2.BitsSet32);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2);
		}

		// Update result2 (the borrow/carry flag): set when op2 > op1 unsigned
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsSubUnsignedCarry(value1.BitsSet32, value2.BitsSet32));
		}
		else if (!IntegerTwiddling.IsSubUnsignedCarry((uint)value1.MinValue, (uint)value2.MaxValue))
		{
			// Even in the worst case (min1 vs max2), no borrow is possible
			result2.SetValue(0);
		}
		else if (IntegerTwiddling.IsSubUnsignedCarry((uint)value1.MaxValue, (uint)value2.MinValue))
		{
			// Even in the best case (max1 vs min2), borrow always occurs
			result2.SetValue(1);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void SubCarryOut64(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the difference) — same logic as Sub64
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet - value2.BitsSet);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2);
		}

		// Update result2 (the borrow/carry flag): set when op2 > op1 unsigned
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsSubUnsignedCarry(value1.BitsSet, value2.BitsSet));
		}
		else if (!IntegerTwiddling.IsSubUnsignedCarry(value1.MinValue, value2.MaxValue))
		{
			// Even in the worst case (min1 vs max2), no borrow is possible
			result2.SetValue(0);
		}
		else if (IntegerTwiddling.IsSubUnsignedCarry(value1.MaxValue, value2.MinValue))
		{
			// Even in the best case (max1 vs min2), borrow always occurs
			result2.SetValue(1);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void SubOverflowOut32(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the difference) — same logic as Sub32
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result.SetValue(value1.BitsSet32 - value2.BitsSet32);
		}
		else if (value2.AreLower32BitsKnown && value2.BitsSet32 == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2);
		}

		// Update result2 (the signed overflow flag)
		if (value1.AreLower32BitsKnown && value2.AreLower32BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsSubSignedOverflow((int)value1.BitsSet32, (int)value2.BitsSet32));
		}
		else if (!IntegerTwiddling.IsSubSignedOverflow((int)(uint)value1.MaxValue, (int)(uint)value2.MinValue)
			&& !IntegerTwiddling.IsSubSignedOverflow((int)(uint)value1.MinValue, (int)(uint)value2.MaxValue))
		{
			result2.SetValue(0);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}

	public static void SubOverflowOut64(BitValue result, BitValue result2, BitValue value1, BitValue value2)
	{
		// Update result1 (the difference) — same logic as Sub64
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result.SetValue(value1.BitsSet - value2.BitsSet);
		}
		else if (value2.AreAll64BitsKnown && value2.BitsSet == 0)
		{
			result.Narrow(value1).SetStable(value1);
		}
		else
		{
			result.SetStable(value1, value2);
		}

		// Update result2 (the signed overflow flag)
		if (value1.AreAll64BitsKnown && value2.AreAll64BitsKnown)
		{
			result2.SetValue(IntegerTwiddling.IsSubSignedOverflow((long)value1.BitsSet, (long)value2.BitsSet));
		}
		else if (!IntegerTwiddling.IsSubSignedOverflow((long)value1.MaxValue, (long)value2.MinValue)
			&& !IntegerTwiddling.IsSubSignedOverflow((long)value1.MinValue, (long)value2.MaxValue))
		{
			result2.SetValue(0);
		}
		else
		{
			result2.NarrowToBoolean().SetStable(value1, value2);
		}
	}
}
