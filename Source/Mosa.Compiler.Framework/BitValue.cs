// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using System.Text;
using Mosa.Compiler.Common;

namespace Mosa.Compiler.Framework;

public sealed class BitValue
{
	private const ulong Upper32BitsSet = ~(ulong)uint.MaxValue;

	#region Property Fields - Bits

	public ulong BitsClear { get; private set; }

	public ulong BitsSet { get; private set; }

	public ulong MaxValue { get; private set; }

	public ulong MinValue { get; private set; }

	public readonly bool Is32Bit;

	#endregion Property Fields - Bits

	#region Status Fields

	public bool IsFixed { get; private set; }

	public bool IsStable { get; private set; }

	public bool IsResolved => (MinValue == MaxValue) || AreAll64BitsKnown;

	#endregion Status Fields

	#region Property Values

	public bool Is64Bit => !Is32Bit;

	public bool AreAll64BitsKnown => (BitsKnown & ulong.MaxValue) == ulong.MaxValue;

	public bool AreLower16BitsKnown => (BitsKnown & ushort.MaxValue) == ushort.MaxValue;

	public bool AreLower32BitsKnown => (BitsKnown & uint.MaxValue) == uint.MaxValue;

	public bool AreLower5BitsKnown => (BitsKnown & 0b11111) == 0b11111;

	public bool AreLower6BitsKnown => (BitsKnown & 0b111111) == 0b111111;

	public bool AreLower8BitsKnown => (BitsKnown & byte.MaxValue) == byte.MaxValue;

	public bool AreUpper32BitsKnown => (BitsKnown & Upper32BitsSet) == Upper32BitsSet;

	public ulong BitsKnown => BitsSet | BitsClear;

	public ulong BitsUnknown => ~BitsKnown;

	public ulong BitsUnknown32 => ~BitsKnown & uint.MaxValue;

	public bool AreAnyBitsKnown => BitsClear != 0 || BitsSet != 0;

	public uint BitsClear32 => (uint)BitsClear;

	public uint BitsSet32 => (uint)BitsSet;

	public byte BitsClear8 => (byte)BitsClear;

	public byte BitsSet8 => (byte)BitsSet;

	public ushort BitsClear16 => (ushort)BitsClear;

	public ushort BitsSet16 => (ushort)BitsSet;

	public bool IsSignBitKnown32 => ((BitsKnown >> 31) & 1) != 0;

	public bool IsSignBitKnown64 => ((BitsKnown >> 63) & 1) != 0;

	public bool IsSignBitSet32 => ((BitsSet >> 31) & 1) != 0;

	public bool IsSignBitSet64 => ((BitsSet >> 63) & 1) != 0;

	public bool IsSignBitClear32 => ((BitsClear >> 31) & 1) == 1;

	public bool IsSignBitClear64 => ((BitsClear >> 63) & 1) == 1;

	public bool IsZeroOrOne => MinValue <= 1 || BitsClear == ~((ulong)1);

	public bool IsZero => (MinValue == 0 && MaxValue == 0) || (BitsClear == ~0ul);

	public bool IsOne => (MinValue == 1 && MaxValue == 1) || (BitsClear == ~1ul && BitsSet == 1);

	public bool IsNotZero => MinValue >= 1 || BitsSet != 0;

	#endregion Property Values

	#region Constructors

	public BitValue(bool is32Bit)
	{
		Is32Bit = is32Bit;

		IsFixed = false;
		IsStable = false;

		BitsSet = 0;
		MinValue = 0;

		if (Is32Bit)
		{
			BitsClear = Upper32BitsSet;
			MaxValue = uint.MaxValue;
		}
		else
		{
			BitsClear = 0;
			MaxValue = ulong.MaxValue;
		}
	}

	public BitValue(bool is32Bit, ulong value)
	{
		Is32Bit = is32Bit;

		if (Is32Bit)
			value &= uint.MaxValue;

		IsFixed = true;
		IsStable = true;

		BitsSet = value;
		BitsClear = ~value;
		MaxValue = value;
		MinValue = value;
	}

	#endregion Constructors

	#region Public Methods

	public BitValue SetValue(BitValue value)
	{
		return Narrow(value).SetStable();
	}

	public BitValue SetValue(ulong value)
	{
		if (Is32Bit)
			value &= uint.MaxValue;

		Debug.Assert(value >= MinValue && value <= MaxValue);
		Debug.Assert((value & BitsClear) == 0);

		if (IsFixed)
			return this;

		IsStable = true;

		BitsSet = value;
		BitsClear = ~value;
		MaxValue = value;
		MinValue = value;

		return this;
	}

	public BitValue NarrowMax(ulong maxValue)
	{
		if (IsFixed)
			return this;

		Debug.Assert(maxValue >= MinValue);

		MaxValue = Math.Min(MaxValue, maxValue);

		return Narrow();
	}

	public BitValue NarrowMin(ulong minValue)
	{
		if (IsFixed)
			return this;

		Debug.Assert(minValue <= MaxValue);

		MinValue = Math.Max(MinValue, minValue);

		return Narrow();
	}

	public BitValue NarrowSetBits(ulong bitsSet)
	{
		if (IsFixed)
			return this;

		if (Is32Bit)
		{
			bitsSet &= uint.MaxValue;
		}

		BitsSet |= bitsSet;

		Debug.Assert((BitsSet & BitsClear) == 0);

		return Narrow();
	}

	public BitValue NarrowClearBits(ulong bitsClear)
	{
		if (IsFixed)
			return this;

		if (Is32Bit)
		{
			bitsClear |= Upper32BitsSet;
		}

		BitsClear |= bitsClear;

		Debug.Assert((BitsSet & BitsClear) == 0);

		return Narrow();
	}

	public BitValue SetNotNull()
	{
		return NarrowMin(1);
	}

	public BitValue Set32BitValue()
	{
		return NarrowMax(uint.MaxValue)
			.NarrowClearBits(Upper32BitsSet).Narrow();
	}

	public BitValue SetValue(bool value)
	{
		return SetValue(value ? 1u : 0);
	}

	public BitValue Narrow(BitValue value)
	{
		return NarrowMin(value.MinValue)
			.NarrowMax(value.MaxValue)
			.NarrowSetBits(value.BitsSet)
			.NarrowClearBits(value.BitsClear);
	}

	public BitValue NarrowToBoolean()
	{
		return
			NarrowMax(1)
			.NarrowClearBits(~1ul);
	}

	public BitValue SetStable(bool stable)
	{
		if (!IsStable && stable)
			IsStable = stable;

		return this;
	}

	public BitValue SetStable(BitValue stable)
	{
		return SetStable(stable.IsStable);
	}

	public BitValue SetStable(BitValue a, BitValue b)
	{
		return SetStable(a.IsStable && b.IsStable);
	}

	public BitValue SetStable(BitValue a, BitValue b, BitValue c)
	{
		return SetStable(a.IsStable && b.IsStable & c.IsStable);
	}

	public BitValue SetStable()
	{
		IsStable = true;

		return this;
	}

	#endregion Public Methods

	#region Private Methods

	private BitValue Narrow()
	{
		var bitsUnknown = ~(BitsSet | BitsClear);
		var maxPossible = BitsSet | bitsUnknown;
		var minPossible = BitsSet & bitsUnknown;

		MinValue = Math.Max(MinValue, minPossible);
		MaxValue = Math.Min(MaxValue, maxPossible);

		BitsClear |= BitTwiddling.GetBitsOver(MaxValue);

		if (MinValue == ulong.MaxValue)
			BitsSet = ulong.MaxValue;
		else if (MinValue == uint.MaxValue && Is32Bit)
			BitsSet = uint.MaxValue;

		Debug.Assert(MinValue <= MaxValue);
		Debug.Assert((BitsSet & BitsClear) == 0);

		return CheckStable();
	}

	private BitValue CheckStable()
	{
		IsStable |= IsResolved;

		return this;
	}

	#endregion Private Methods

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append($"MaxValue: {MaxValue}");
		sb.Append($"MinValue: {MinValue}");
		sb.Append($"BitsSet: {Convert.ToString((long)BitsSet, 2).PadLeft(64, '0')}");
		sb.Append($"BitsClear: {Convert.ToString((long)BitsClear, 2).PadLeft(64, '0')}");
		sb.Append($"BitsKnown: {Convert.ToString((long)BitsKnown, 2).PadLeft(64, '0')}");

		return sb.ToString();
	}
}
