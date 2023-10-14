// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;

namespace Mosa.Compiler.Framework;

public sealed class BitValue
{
	private const ulong Upper32BitsSet = ~(ulong)uint.MaxValue;

	#region Static Values

	public static readonly BitValue Any32 = new BitValue(0, Upper32BitsSet, uint.MaxValue, 0, true);
	public static readonly BitValue Any64 = new BitValue(0, 0, ulong.MaxValue, 0, false);

	public static readonly BitValue AnyExceptZero32 = new BitValue(0, Upper32BitsSet, uint.MaxValue, 1, true);
	public static readonly BitValue AnyExceptZero64 = new BitValue(0, 0, ulong.MaxValue, 1, false);

	public static readonly BitValue Zero32 = new BitValue(0, true);
	public static readonly BitValue Zero64 = new BitValue(0, false);

	public static readonly BitValue One32 = new BitValue(1, true);
	public static readonly BitValue One64 = new BitValue(1, false);

	public static readonly BitValue I2_32 = new BitValue(2, true);
	public static readonly BitValue I2_64 = new BitValue(2, false);

	public static readonly BitValue I3_32 = new BitValue(3, true);
	public static readonly BitValue I3_64 = new BitValue(3, false);

	public static readonly BitValue I4_32 = new BitValue(4, true);
	public static readonly BitValue I4_64 = new BitValue(4, false);

	public static readonly BitValue I5_32 = new BitValue(5, true);
	public static readonly BitValue I5_64 = new BitValue(5, false);

	public static readonly BitValue I7_32 = new BitValue(7, true);
	public static readonly BitValue I7_64 = new BitValue(7, false);

	public static readonly BitValue I8_32 = new BitValue(8, true);
	public static readonly BitValue I8_64 = new BitValue(8, false);

	public static readonly BitValue I10_32 = new BitValue(10, true);
	public static readonly BitValue I10_64 = new BitValue(10, false);

	public static readonly BitValue I16_32 = new BitValue(16, true);
	public static readonly BitValue I16_64 = new BitValue(16, false);

	public static readonly BitValue I31_32 = new BitValue(31, true);
	public static readonly BitValue I31_64 = new BitValue(31, false);

	public static readonly BitValue I32_32 = new BitValue(32, true);
	public static readonly BitValue I32_64 = new BitValue(32, false);

	public static readonly BitValue I50_32 = new BitValue(50, true);
	public static readonly BitValue I50_64 = new BitValue(50, false);

	public static readonly BitValue I64_32 = new BitValue(64, true);
	public static readonly BitValue I64_64 = new BitValue(64, false);

	public static readonly BitValue I256_32 = new BitValue(256, true);
	public static readonly BitValue I256_64 = new BitValue(256, false);

	public static readonly BitValue xF32 = new BitValue(0xF, true);
	public static readonly BitValue xF64 = new BitValue(0xF, false);

	public static readonly BitValue xFF32 = new BitValue(0xFF, true);
	public static readonly BitValue xFF64 = new BitValue(0xFF, false);

	public static readonly BitValue xFFFF32 = new BitValue(0xFFFF, true);
	public static readonly BitValue xFFFF64 = new BitValue(0xFFFF, false);

	public static readonly BitValue IntMin32 = new BitValue(unchecked((ulong)int.MinValue), true);
	public static readonly BitValue IntMax32 = new BitValue(int.MaxValue, true);
	public static readonly BitValue UIntMax32 = new BitValue(uint.MaxValue, true);

	public static readonly BitValue IntMax64 = new BitValue(int.MaxValue, false);
	public static readonly BitValue IntMin64 = new BitValue(unchecked((ulong)int.MinValue), false);
	public static readonly BitValue LongMin64 = new BitValue(unchecked((ulong)long.MinValue), false);
	public static readonly BitValue UIntMax64 = new BitValue(uint.MaxValue, false);
	public static readonly BitValue ULongMax64 = new BitValue(ulong.MaxValue, false);

	public static readonly BitValue Upper48BitsSet = new BitValue(~(ulong)ushort.MaxValue, false);
	public static readonly BitValue Upper56BitsSet = new BitValue(~(ulong)byte.MaxValue, false);

	#endregion Static Values

	public ulong BitsClear { get; private set; }

	public ulong BitsSet { get; private set; }

	public ulong MaxValue { get; private set; }

	public ulong MinValue { get; private set; }

	public bool Is32Bit { get; private set; }

	#region Future

	//public ulong A { get; private set; }

	//public ulong B { get; private set; }

	//public bool IsSetValid => A != B || IsSetEmptySet || IsFullSet);

	//public bool IsSetEmptySet => A == 0 && B == 0;

	//public bool IsSetWrapped => A > B;

	//public bool IsFullSet => IsSetFull64BitSet || (IsSetFull32BitSet && Is32Bit);

	//public bool IsFullSet32 => (A == uint.MaxValue && B == uint.MaxValue) || (A == 0 && B > uint.MaxValue);

	//public bool IsFullSet64 => A == ulong.MaxValue && B == ulong.MaxValue;

	#endregion Future

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

	private BitValue(ulong value, bool is32Bit)
	{
		BitsSet = value;
		BitsClear = ~value;
		MaxValue = value;
		MinValue = value;
		Is32Bit = is32Bit;

		if (is32Bit)
		{
			MaxValue &= uint.MaxValue;
			MinValue &= uint.MaxValue;
			BitsSet &= uint.MaxValue;
			BitsClear |= Upper32BitsSet;
		}
	}

	private BitValue(ulong bitsSet, ulong bitsClear, ulong maxValue, ulong minValue, bool is32Bit)
	{
		BitsSet = bitsSet;
		BitsClear = bitsClear;
		MaxValue = maxValue;
		MinValue = minValue;
		Is32Bit = is32Bit;
	}

	public static BitValue CreateValue(ulong bitsSet, ulong bitsClear, ulong maxValue, ulong minValue, bool rangeDeterminate, bool is32Bit)
	{
		var _maxValue = rangeDeterminate ? maxValue : ulong.MaxValue;
		var _minValue = rangeDeterminate ? minValue : 0;

		if (is32Bit)
		{
			_maxValue &= uint.MaxValue;
			_minValue &= uint.MaxValue;
			bitsSet &= uint.MaxValue;
			bitsClear |= Upper32BitsSet;
		}

		var bitsUnknown = ~(bitsSet | bitsClear);
		var maxPossible = bitsSet | bitsUnknown;
		var minPossible = bitsSet & bitsUnknown;

		_minValue = Math.Max(_minValue, minPossible);
		_maxValue = Math.Min(_maxValue, maxPossible);

		if (_maxValue == _minValue)
			return CreateValue(_maxValue, is32Bit);

		if ((bitsSet | bitsClear) == ulong.MaxValue)
			return CreateValue(bitsSet, is32Bit);

		if (bitsSet == 0 && bitsClear == Upper32BitsSet && _maxValue == uint.MaxValue && _minValue == 0 && is32Bit)
			return Any32;

		if (bitsSet == 0 && bitsClear == 0 && _maxValue == ulong.MaxValue && _minValue == 0 && !is32Bit)
			return Any64;

		return new BitValue(bitsSet, bitsClear, _maxValue, _minValue, is32Bit);
	}

	public static BitValue CreateValue(bool value, bool is32Bit)
	{
		return CreateValue(value ? 1u : 0, is32Bit);
	}

	public static BitValue CreateValue(ulong value, bool is32Bit)
	{
		if (is32Bit)
		{
			switch ((uint)value)
			{
				case 0: return Zero32;
				case 1: return One32;
				case 2: return I2_32;
				case 3: return I3_32;
				case 4: return I4_32;
				case 5: return I5_32;
				case 7: return I7_32;
				case 8: return I8_32;
				case 10: return I10_32;
				case 16: return I16_32;
				case 32: return I32_32;
				case 50: return I50_32;
				case 64: return I64_32;
				case 256: return I256_32;
				case 0xF: return xF32;
				case 0xFF: return xFF32;
				case 0xFFFF: return xFFFF32;
				case uint.MaxValue: return UIntMax32;
				case int.MaxValue: return IntMax32;
				case unchecked((uint)int.MinValue): return IntMin32;
			}
		}
		else
		{
			switch (value)
			{
				case 0: return Zero64;
				case 1: return One64;
				case 2: return I2_64;
				case 3: return I3_64;
				case 4: return I4_64;
				case 5: return I5_64;
				case 7: return I7_64;
				case 8: return I8_64;
				case 10: return I10_64;
				case 16: return I16_64;
				case 32: return I32_64;
				case 50: return I50_64;
				case 64: return I64_64;
				case 256: return I256_64;
				case 0xF: return xF64;
				case 0xFF: return xFF64;
				case 0xFFFF: return xFFFF64;
				case ulong.MaxValue: return ULongMax64;
				case int.MaxValue: return IntMax64;
				case unchecked((ulong)int.MinValue): return IntMin64;
				case uint.MaxValue: return UIntMax64;
				case unchecked((ulong)long.MinValue): return LongMin64;
				case ~(ulong)ushort.MaxValue: return Upper48BitsSet;
				case ~(ulong)byte.MaxValue: return Upper48BitsSet;
			}
		}

		return new BitValue(value, is32Bit);
	}

	public override string ToString()
	{
		var sb = new StringBuilder();

		sb.Append($" MaxValue: {MaxValue}");
		sb.Append($" MinValue: {MinValue}");

		sb.Append($" BitsSet: {Convert.ToString((long)BitsSet, 2).PadLeft(64, '0')}");
		sb.Append($" BitsClear: {Convert.ToString((long)BitsClear, 2).PadLeft(64, '0')}");
		sb.Append($" BitsKnown: {Convert.ToString((long)BitsKnown, 2).PadLeft(64, '0')}");

		return sb.ToString();
	}
}
