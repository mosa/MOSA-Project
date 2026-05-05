// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework.Stages;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BitTrackerOperationsAdd32Tests
{
	[Fact]
	public void Add32_BothAllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 42);
		var value2 = new BitValue(true, 8);

		BitTrackerOperations.Add32(result, value1, value2);

		Assert.Equal(50u, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void Add32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 123);

		BitTrackerOperations.Add32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void Add32_SecondIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 456);
		var value2 = new BitValue(true, 0);

		BitTrackerOperations.Add32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void Add32_Unstable_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true);
		var value2 = new BitValue(true);

		BitTrackerOperations.Add32(result, value1, value2);

		Assert.False(result.IsStable);
	}
}

public class BitTrackerOperationsAdd64Tests
{
	[Fact]
	public void Add64_BothAllBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 1000UL);
		var value2 = new BitValue(false, 2000UL);

		BitTrackerOperations.Add64(result, value1, value2);

		Assert.Equal(3000UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void Add64_FirstIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 5000UL);

		BitTrackerOperations.Add64(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void Add64_SecondIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 7000UL);
		var value2 = new BitValue(false, 0UL);

		BitTrackerOperations.Add64(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

public class BitTrackerOperationsSub32Tests
{
	[Fact]
	public void Sub32_BothAllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 100);
		var value2 = new BitValue(true, 30);

		BitTrackerOperations.Sub32(result, value1, value2);

		Assert.Equal(70u, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void Sub32_SecondIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 456);
		var value2 = new BitValue(true, 0);

		BitTrackerOperations.Sub32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void Sub32_BothUnknown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true);
		var value2 = new BitValue(true);

		BitTrackerOperations.Sub32(result, value1, value2);

		Assert.False(result.IsStable);
	}
}

public class BitTrackerOperationsSub64Tests
{
	[Fact]
	public void Sub64_BothAllBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 5000UL);
		var value2 = new BitValue(false, 1000UL);

		BitTrackerOperations.Sub64(result, value1, value2);

		Assert.Equal(4000UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void Sub64_SecondIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 8000UL);
		var value2 = new BitValue(false, 0UL);

		BitTrackerOperations.Sub64(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

public class BitTrackerOperationsAnd32Tests
{
	[Fact]
	public void And32_BothBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xFF);
		var value2 = new BitValue(true, 0x0F);

		BitTrackerOperations.And32(result, value1, value2);

		Assert.Equal(0x0Fu, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void And32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 0xFFFF);

		BitTrackerOperations.And32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void And32_SecondIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xDEADBEEF);
		var value2 = new BitValue(true, 0);

		BitTrackerOperations.And32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}
}

public class BitTrackerOperationsAnd64Tests
{
	[Fact]
	public void And64_BothBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0xFFFFUL);
		var value2 = new BitValue(false, 0x00FFUL);

		BitTrackerOperations.And64(result, value1, value2);

		Assert.Equal(0x00FFUl, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void And64_FirstIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 0xFFFFFFFFUL);

		BitTrackerOperations.And64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}
}

public class BitTrackerOperationsOr32Tests
{
	[Fact]
	public void Or32_BothBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xF0);
		var value2 = new BitValue(true, 0x0F);

		BitTrackerOperations.Or32(result, value1, value2);

		Assert.Equal(0xFFu, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void Or32_FirstAllOnes()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, uint.MaxValue);
		var value2 = new BitValue(true, 0x1234);

		BitTrackerOperations.Or32(result, value1, value2);

		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

public class BitTrackerOperationsOr64Tests
{
	[Fact]
	public void Or64_BothBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0xF0UL);
		var value2 = new BitValue(false, 0x0FUL);

		BitTrackerOperations.Or64(result, value1, value2);

		Assert.Equal(0xFFUL, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void Or64_FirstAllOnes()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, ulong.MaxValue);
		var value2 = new BitValue(false, 0x1234UL);

		BitTrackerOperations.Or64(result, value1, value2);

		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

public partial class BitTrackerOperationsXor32Tests
{
	[Fact]
	public void Xor32_BothBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xFF);
		var value2 = new BitValue(true, 0xFF);

		BitTrackerOperations.Xor32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void Xor32_DifferentBits()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xF0);
		var value2 = new BitValue(true, 0x0F);

		BitTrackerOperations.Xor32(result, value1, value2);

		Assert.Equal(0xFFu, result.BitsSet);
		Assert.True(result.IsStable);
	}
}

public partial class BitTrackerOperationsXor64Tests
{
	[Fact]
	public void Xor64_SameBits()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0x12345678UL);
		var value2 = new BitValue(false, 0x12345678UL);

		BitTrackerOperations.Xor64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
		Assert.True(result.IsStable);
	}
}

public class BitTrackerOperationsNot32Tests
{
	[Fact]
	public void Not32_AllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xFF);

		BitTrackerOperations.Not32(result, value1);

		var expected = ~0xFFU & uint.MaxValue;
		Assert.Equal(expected, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void Not32_AllOnes()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, uint.MaxValue);

		BitTrackerOperations.Not32(result, value1);

		Assert.Equal(0u, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}
}

public class BitTrackerOperationsNot64Tests
{
	[Fact]
	public void Not64_AllBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0xFFUL);

		BitTrackerOperations.Not64(result, value1);

		Assert.Equal(~0xFFUL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}
}

public class BitTrackerOperationsNeg32Tests
{
	[Fact]
	public void Neg32_Positive()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 42);

		BitTrackerOperations.Neg32(result, value1);

		var expected = unchecked((uint)((ulong)(-(int)42)));
		Assert.Equal(expected, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void Neg32_Zero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);

		BitTrackerOperations.Neg32(result, value1);

		Assert.Equal(0u, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}
}

public class BitTrackerOperationsNeg64Tests
{
	[Fact]
	public void Neg64_Positive()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 1000UL);

		BitTrackerOperations.Neg64(result, value1);

		var expected = unchecked((ulong)(-(long)1000));
		Assert.Equal(expected, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}
}

public class BitTrackerOperationsMove32Tests
{
	[Fact]
	public void Move32_CopiesBitValue()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0x12345678);

		BitTrackerOperations.Move32(result, value1);

		Assert.Equal(value1.BitsSet, result.BitsSet);
		Assert.True(result.IsStable);
	}
}

public class BitTrackerOperationsMove64Tests
{
	[Fact]
	public void Move64_CopiesBitValue()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0x123456789ABCDEFFUL);

		BitTrackerOperations.Move64(result, value1);

		Assert.Equal(value1.BitsSet, result.BitsSet);
		Assert.True(result.IsStable);
	}
}

public class BitTrackerOperationsGetHigh32Tests
{
	[Fact]
	public void GetHigh32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(false, 0x123456789ABCDEFFUL);

		BitTrackerOperations.GetHigh32(result, value1);

		Assert.Equal(0x12345678UL, result.BitsSet);
		Assert.True(result.AreUpper32BitsKnown);
	}

	[Fact]
	public void GetHigh32_LessThan32Bit()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(false, 0xFFFFFFFFUL);

		BitTrackerOperations.GetHigh32(result, value1);

		Assert.Equal(0u, result.BitsSet);
	}
}

public class BitTrackerOperationsGetLow32Tests
{
	[Fact]
	public void GetLow32_AllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(false, 0x123456789ABCDEFFUL);

		BitTrackerOperations.GetLow32(result, value1);

		Assert.Equal(0x9ABCDEFFUL, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}
}

public class BitTrackerOperationsMulUnsigned32Tests
{
	[Fact]
	public void MulUnsigned32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 10);
		var value2 = new BitValue(true, 20);

		BitTrackerOperations.MulUnsigned32(result, value1, value2);

		Assert.Equal(200u, result.BitsSet);
	}

	[Fact]
	public void MulUnsigned32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 999);

		BitTrackerOperations.MulUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void MulUnsigned32_FirstIsOne()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 1);
		var value2 = new BitValue(true, 456);

		BitTrackerOperations.MulUnsigned32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}
}

public class BitTrackerOperationsMulUnsigned64Tests
{
	[Fact]
	public void MulUnsigned64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 100UL);
		var value2 = new BitValue(false, 50UL);

		BitTrackerOperations.MulUnsigned64(result, value1, value2);

		Assert.Equal(5000UL, result.BitsSet);
	}

	[Fact]
	public void MulUnsigned64_FirstIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 9999UL);

		BitTrackerOperations.MulUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
	}
}

public class BitTrackerOperationsShiftLeft32Tests
{
	[Fact]
	public void ShiftLeft32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 5);
		var value2 = new BitValue(true, 3);

		BitTrackerOperations.ShiftLeft32(result, value1, value2);

		Assert.Equal(40u, result.BitsSet);
	}

	[Fact]
	public void ShiftLeft32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 10);

		BitTrackerOperations.ShiftLeft32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
	}

	[Fact]
	public void ShiftLeft32_ShiftByZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0x12345678);
		var value2 = new BitValue(true, 0);

		BitTrackerOperations.ShiftLeft32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

public class BitTrackerOperationsShiftLeft64Tests
{
	[Fact]
	public void ShiftLeft64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 7UL);
		var value2 = new BitValue(false, 4UL);

		BitTrackerOperations.ShiftLeft64(result, value1, value2);

		Assert.Equal(112UL, result.BitsSet);
	}

	[Fact]
	public void ShiftLeft64_FirstIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 20UL);

		BitTrackerOperations.ShiftLeft64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
	}
}

public class BitTrackerOperationsShiftRight32Tests
{
	[Fact]
	public void ShiftRight32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 64);
		var value2 = new BitValue(true, 2);

		BitTrackerOperations.ShiftRight32(result, value1, value2);

		Assert.Equal(16u, result.BitsSet);
	}

	[Fact]
	public void ShiftRight32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 5);

		BitTrackerOperations.ShiftRight32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
	}
}

public class BitTrackerOperationsShiftRight64Tests
{
	[Fact]
	public void ShiftRight64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 256UL);
		var value2 = new BitValue(false, 3UL);

		BitTrackerOperations.ShiftRight64(result, value1, value2);

		Assert.Equal(32UL, result.BitsSet);
	}

	[Fact]
	public void ShiftRight64_FirstIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 10UL);

		BitTrackerOperations.ShiftRight64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
	}
}

public class BitTrackerOperationsZeroExtend32To64Tests
{
	[Fact]
	public void ZeroExtend32x64_AllBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0xDEADBEEFu);

		BitTrackerOperations.ZeroExtend32x64(result, value1);

		Assert.Equal(0xDEADBEEFUL, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}
}

public class BitTrackerOperationsTruncate64To32Tests
{
	[Fact]
	public void Truncate64x32_AllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(false, 0x123456789ABCDEFFUL);

		BitTrackerOperations.Truncate64x32(result, value1);

		Assert.Equal(0x9ABCDEFFUL, result.BitsSet);
		Assert.True(result.AreLower32BitsKnown);
	}
}

public class BitTrackerOperationsSignExtendTests
{
	[Fact]
	public void SignExtend8x32_Positive()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0x42);

		BitTrackerOperations.SignExtend8x32(result, value1);

		Assert.True(result.AreLower8BitsKnown);
	}

	[Fact]
	public void SignExtend8x32_Negative()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0x80);

		BitTrackerOperations.SignExtend8x32(result, value1);

		Assert.True(result.AreLower8BitsKnown);
	}

	[Fact]
	public void SignExtend16x32_Value()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0x1234);

		BitTrackerOperations.SignExtend16x32(result, value1);

		Assert.True(result.AreLower16BitsKnown);
	}

	[Fact]
	public void SignExtend32x64_Value()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0xDEADBEEFu);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void SignExtend32x64_AllBitsKnown_Positive()
	{
		// 0x7FFFFFFF — sign bit clear, all lower 32 known → upper 32 must be zero
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0x7FFFFFFFu);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0x000000007FFFFFFFul, result.BitsSet);
		Assert.Equal(0xFFFFFFFF80000000ul, result.BitsClear);
	}

	[Fact]
	public void SignExtend32x64_AllBitsKnown_NegativeMinInt()
	{
		// 0x80000000 — sign bit set, all lower 32 known → upper 32 must be all set
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0x80000000u);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0xFFFFFFFF80000000ul, result.BitsSet);
		Assert.Equal(0x000000007FFFFFFFul, result.BitsClear);
	}

	[Fact]
	public void SignExtend32x64_AllBitsKnown_Zero()
	{
		// 0x00000000 — all lower bits clear → result must be 0
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0u);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0ul, result.BitsSet);
	}

	[Fact]
	public void SignExtend32x64_AllBitsKnown_AllOnesNegative()
	{
		// 0xFFFFFFFF — all bits set including sign bit → result must be 0xFFFFFFFFFFFFFFFF
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0xFFFFFFFFu);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0xFFFFFFFFFFFFFFFFul, result.BitsSet);
		Assert.Equal(0ul, result.BitsClear);
	}

	[Fact]
	public void SignExtend32x64_SignBitKnownSet_LowerPartiallyKnown()
	{
		// Sign bit known set, only some lower bits known → upper 32 must be all set
		var result = new BitValue(false);
		var value1 = new BitValue(false); // 64-bit container for a 32-bit value
		value1.NarrowSetBits(0x80000005u);    // sign bit + bits 0 and 2 set
		value1.NarrowClearBits(0x7FFFFFF0u);  // upper nibble of lower 32 clear

		BitTrackerOperations.SignExtend32x64(result, value1);

		// Upper 32 bits must all be set (sign-extended)
		Assert.Equal(0xFFFFFFFF00000000ul, result.BitsSet & 0xFFFFFFFF00000000ul);
	}

	[Fact]
	public void SignExtend32x64_SignBitKnownClear_LowerPartiallyKnown()
	{
		// Sign bit known clear, some lower bits known → upper 32 must be all clear
		var result = new BitValue(false);
		var value1 = new BitValue(false);
		value1.NarrowClearBits(0x80000000u);  // sign bit known clear
		value1.NarrowSetBits(0x00000003u);    // bits 0 and 1 set

		BitTrackerOperations.SignExtend32x64(result, value1);

		// Upper 32 bits must be known clear
		Assert.Equal(0xFFFFFFFF00000000ul, result.BitsClear & 0xFFFFFFFF00000000ul);
		// Known set bits in lower 32 must be propagated
		Assert.True((result.BitsSet & 0x3ul) == 0x3ul);
	}

	[Fact]
	public void SignExtend32x64_SignBitUnknown_UpperBitsUnknown()
	{
		// Sign bit unknown → upper 32 bits must remain unknown
		var result = new BitValue(false);
		var value1 = new BitValue(false);
		// Set some bits below bit 31 known, but bit 31 unknown
		value1.NarrowSetBits(0x00000001u);
		value1.NarrowClearBits(0x00000002u);

		BitTrackerOperations.SignExtend32x64(result, value1);

		// Upper 32 bits should be neither fully set nor fully clear
		Assert.NotEqual(0xFFFFFFFF00000000ul, result.BitsSet & 0xFFFFFFFF00000000ul);
		Assert.NotEqual(0xFFFFFFFF00000000ul, result.BitsClear & 0xFFFFFFFF00000000ul);
		// Known lower bits must still be propagated
		Assert.True((result.BitsSet & 0x1ul) == 0x1ul);
		Assert.True((result.BitsClear & 0x2ul) == 0x2ul);
	}

	[Fact]
	public void SignExtend32x64_FullyUnknown_NoUpperBitsSet()
	{
		// Completely unknown input → no upper bits should be known set or clear
		var result = new BitValue(false);
		var value1 = new BitValue(false);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.Equal(0ul, result.BitsSet & 0xFFFFFFFF00000000ul);
		Assert.Equal(0ul, result.BitsClear & 0xFFFFFFFF00000000ul);
	}

	[Fact]
	public void SignExtend32x64_AllBitsKnown_MaxPositiveInt32()
	{
		// 0x7FFFFFFF is the largest positive int32 → result 0x000000007FFFFFFF
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0x7FFFFFFFu);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.Equal(0x000000007FFFFFFFul, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void SignExtend32x64_AllBitsKnown_One()
	{
		// 0x00000001 → result 0x0000000000000001
		var result = new BitValue(false);
		var value1 = new BitValue(true, 1u);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(1ul, result.BitsSet);
	}

	[Fact]
	public void SignExtend32x64_StableInputPropagated()
	{
		// A stable, fully known input should yield a stable result
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0x12345678u);

		BitTrackerOperations.SignExtend32x64(result, value1);

		Assert.True(result.IsStable);
	}

	[Fact]
	public void SignExtend8x64_AllBitsKnown_Positive()
	{
		// 0x7F — sign bit clear, all lower 8 known → upper 56 must be zero
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0x7Fu);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0x000000000000007Ful, result.BitsSet);
		Assert.Equal(0xFFFFFFFFFFFFFF80ul, result.BitsClear);
	}

	[Fact]
	public void SignExtend8x64_AllBitsKnown_Negative()
	{
		// 0x80 — sign bit set, all lower 8 known → upper 56 must be all set
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0x80u);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0xFFFFFFFFFFFFFF80ul, result.BitsSet);
		Assert.Equal(0x000000000000007Ful, result.BitsClear);
	}

	[Fact]
	public void SignExtend8x64_AllBitsKnown_Zero()
	{
		// 0x00 — all bits clear → result must be 0
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0u);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0ul, result.BitsSet);
	}

	[Fact]
	public void SignExtend8x64_AllBitsKnown_AllOnesNegative()
	{
		// 0xFF — all 8 bits set including sign bit → result must be 0xFFFFFFFFFFFFFFFF
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0xFFu);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0xFFFFFFFFFFFFFFFFul, result.BitsSet);
		Assert.Equal(0ul, result.BitsClear);
	}

	[Fact]
	public void SignExtend8x64_AllBitsKnown_One()
	{
		// 0x01 → result 0x0000000000000001
		var result = new BitValue(false);
		var value1 = new BitValue(false, 1u);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(1ul, result.BitsSet);
	}

	[Fact]
	public void SignExtend8x64_SignBitKnownSet_LowerPartiallyKnown()
	{
		// Sign bit (bit 7) known set, only some lower bits known → upper 56 must be all set
		var result = new BitValue(false);
		var value1 = new BitValue(false);
		value1.NarrowSetBits(0x85u);   // bit 7 + bits 0 and 2 set
		value1.NarrowClearBits(0x70u); // bits 4-6 clear

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.Equal(0xFFFFFFFFFFFFFF00ul, result.BitsSet & 0xFFFFFFFFFFFFFF00ul);
	}

	[Fact]
	public void SignExtend8x64_SignBitKnownClear_LowerPartiallyKnown()
	{
		// Sign bit (bit 7) known clear, some lower bits known → upper 56 must be all clear
		var result = new BitValue(false);
		var value1 = new BitValue(false);
		value1.NarrowClearBits(0x80u); // sign bit known clear
		value1.NarrowSetBits(0x03u);   // bits 0 and 1 set

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.Equal(0xFFFFFFFFFFFFFF00ul, result.BitsClear & 0xFFFFFFFFFFFFFF00ul);
		Assert.True((result.BitsSet & 0x3ul) == 0x3ul);
	}

	[Fact]
	public void SignExtend8x64_SignBitUnknown_UpperBitsUnknown()
	{
		// Sign bit (bit 7) unknown → upper 56 bits must remain unknown
		var result = new BitValue(false);
		var value1 = new BitValue(false);
		value1.NarrowSetBits(0x01u);  // bit 0 known set
		value1.NarrowClearBits(0x02u); // bit 1 known clear

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.NotEqual(0xFFFFFFFFFFFFFF00ul, result.BitsSet & 0xFFFFFFFFFFFFFF00ul);
		Assert.NotEqual(0xFFFFFFFFFFFFFF00ul, result.BitsClear & 0xFFFFFFFFFFFFFF00ul);
		Assert.True((result.BitsSet & 0x1ul) == 0x1ul);
		Assert.True((result.BitsClear & 0x2ul) == 0x2ul);
	}

	[Fact]
	public void SignExtend8x64_FullyUnknown_NoUpperBitsSet()
	{
		// Completely unknown input → no upper bits should be known set or clear
		var result = new BitValue(false);
		var value1 = new BitValue(false);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.Equal(0ul, result.BitsSet & 0xFFFFFFFFFFFFFF00ul);
		Assert.Equal(0ul, result.BitsClear & 0xFFFFFFFFFFFFFF00ul);
	}

	[Fact]
	public void SignExtend8x64_AllBitsKnown_MaxPositiveByte()
	{
		// 0x7F is the largest positive signed byte → result 0x000000000000007F
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0x7Fu);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.Equal(0x000000000000007Ful, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void SignExtend8x64_StableInputPropagated()
	{
		// A stable, fully known input should yield a stable result
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0x42u);

		BitTrackerOperations.SignExtend8x64(result, value1);

		Assert.True(result.IsStable);
	}
}

public partial class BitTrackerOperationsRemUnsignedTests
{
	[Fact]
	public void RemUnsigned32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 17);
		var value2 = new BitValue(true, 5);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(2u, result.BitsSet);
	}

	[Fact]
	public void RemUnsigned32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 7);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
	}

	[Fact]
	public void RemUnsigned64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 100UL);
		var value2 = new BitValue(false, 30UL);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(10UL, result.BitsSet);
	}

	[Fact]
	public void RemUnsigned32_DivideByZero_DoesNotChangeResult()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 25);
		var value2 = new BitValue(true, 0);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(uint.MaxValue, result.MaxValue);
		Assert.False(result.IsStable);
	}

	[Fact]
	public void RemUnsigned64_DivideByZero_DoesNotChangeResult()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 25);
		var value2 = new BitValue(false, 0);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(ulong.MaxValue, result.MaxValue);
		Assert.False(result.IsStable);
	}

	[Fact]
	public void RemUnsigned64_DivisorRangeAlwaysGreaterThanDividend_CopiesDividendRange()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMax(10);
		var value2 = new BitValue(false).NarrowMin(20);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(value1.MinValue, result.MinValue);
		Assert.Equal(value1.MaxValue, result.MaxValue);
		Assert.False(result.IsStable);
	}

	[Fact]
	public void RemUnsigned32_BothZero_DoesNotChangeResult()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 0);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(uint.MaxValue, result.MaxValue);
		Assert.False(result.IsStable);
	}

	[Fact]
	public void RemUnsigned64_BothZero_DoesNotChangeResult()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0);
		var value2 = new BitValue(false, 0);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(ulong.MaxValue, result.MaxValue);
		Assert.False(result.IsStable);
	}

	[Fact]
	public void RemUnsigned32_UnknownDivisorRange_NarrowsByDivisorUpperBound()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMax(100);
		var value2 = new BitValue(true).NarrowMin(2).NarrowMax(8);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(7u, result.MaxValue);
	}

	[Fact]
	public void RemUnsigned64_UnknownDivisorRange_NarrowsByDivisorUpperBound()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMax(100);
		var value2 = new BitValue(false).NarrowMin(2).NarrowMax(8);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(7UL, result.MaxValue);
	}
}

public class BitTrackerOperationsDivUnsignedTests
{
	[Fact]
	public void DivUnsigned32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 40);
		var value2 = new BitValue(true, 8);

		BitTrackerOperations.DivUnsigned32(result, value1, value2);

		Assert.Equal(5u, result.BitsSet);
	}

	[Fact]
	public void DivUnsigned32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 5);

		BitTrackerOperations.DivUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet);
	}

	[Fact]
	public void DivUnsigned64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 1000UL);
		var value2 = new BitValue(false, 50UL);

		BitTrackerOperations.DivUnsigned64(result, value1, value2);

		Assert.Equal(20UL, result.BitsSet);
	}
}

public class BitTrackerOperationsIfThenElseTests
{
	[Fact]
	public void IfThenElse32_BothKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0);
		var value2 = new BitValue(true, 10);
		var value3 = new BitValue(true, 20);

		BitTrackerOperations.IfThenElse32(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void IfThenElse64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 200UL);
		var value3 = new BitValue(false, 200UL);

		BitTrackerOperations.IfThenElse64(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.True(result.AreAll64BitsKnown);
	}
}

public class BitTrackerOperationsNewObjectTests
{
	[Fact]
	public void NewObject_SetsNotNull()
	{
		var result = new BitValue(false);

		BitTrackerOperations.NewObject(result);

		Assert.True(result.IsNotZero);
	}

	[Fact]
	public void NewArray_SetsNotNull()
	{
		var result = new BitValue(false);

		BitTrackerOperations.NewArray(result);

		Assert.True(result.IsNotZero);
	}

	[Fact]
	public void NewString_SetsNotNull()
	{
		var result = new BitValue(false);

		BitTrackerOperations.NewString(result);

		Assert.True(result.IsNotZero);
	}
}

public class BitTrackerOperationsZeroExtendTests
{
	[Fact]
	public void ZeroExtend8x32_AllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xFF);

		BitTrackerOperations.ZeroExtend8x32(result, value1);

		Assert.Equal(0xFFu, result.BitsSet);
		Assert.True(result.AreLower8BitsKnown);
	}

	[Fact]
	public void ZeroExtend16x32_AllBitsKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0xFFFF);

		BitTrackerOperations.ZeroExtend16x32(result, value1);

		Assert.Equal(0xFFFFu, result.BitsSet);
		Assert.True(result.AreLower16BitsKnown);
	}

	[Fact]
	public void ZeroExtend8x64_AllBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0xFFu);

		BitTrackerOperations.ZeroExtend8x64(result, value1);

		Assert.Equal(0xFFUL, result.BitsSet);
	}
}

public class BitTrackerOperationsToTests
{
	[Fact]
	public void To64_BothKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0x12345678u);
		var value2 = new BitValue(true, 0x9ABCDEFFu);

		BitTrackerOperations.To64(result, value1, value2);

		Assert.True(result.IsStable);
	}
}

public class BitTrackerOperationsArithShiftRightTests
{
	[Fact]
	public void ArithShiftRight32_NegativeValue_SignExtends()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, unchecked((uint)-2));
		var value2 = new BitValue(true, 1);

		BitTrackerOperations.ArithShiftRight32(result, value1, value2);

		Assert.Equal(uint.MaxValue, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void ArithShiftRight32_ShiftMaskApplies()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 8);
		var value2 = new BitValue(true, 33); // 33 & 31 == 1

		BitTrackerOperations.ArithShiftRight32(result, value1, value2);

		Assert.Equal(4u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void ArithShiftRight64_NegativeValue_SignExtends()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, unchecked((ulong)-2L));
		var value2 = new BitValue(false, 1);

		BitTrackerOperations.ArithShiftRight64(result, value1, value2);

		Assert.Equal(ulong.MaxValue, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void ArithShiftRight64_ShiftMaskApplies()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 8);
		var value2 = new BitValue(false, 65); // 65 & 63 == 1

		BitTrackerOperations.ArithShiftRight64(result, value1, value2);

		Assert.Equal(4UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void ArithShiftRight32_NegativeKnownSign_KeepsSignBitSet()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowSetBits(0x80000000UL);
		var value2 = new BitValue(true, 1);

		BitTrackerOperations.ArithShiftRight32(result, value1, value2);

		Assert.NotEqual(0UL, result.BitsSet & 0x80000000UL);
		Assert.NotEqual(0UL, result.BitsKnown & 0x80000000UL);
	}
}

public partial class BitTrackerOperationsXor64Tests
{
	[Fact]
	public void Xor64_PartialKnownBits_TracksKnownIntersection()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowSetBits(0xF0UL).NarrowClearBits(0x0FUL);
		var value2 = new BitValue(false).NarrowSetBits(0xAAUL).NarrowClearBits(0x55UL);

		BitTrackerOperations.Xor64(result, value1, value2);

		Assert.Equal((0xF0UL ^ 0xAAUL) & 0xFFUL, result.BitsSet & 0xFFUL);
		Assert.Equal(0xFFUL, result.BitsKnown & 0xFFUL);
	}

	[Fact]
	public void Xor64_SameOperand_IsZero()
	{
		var result = new BitValue(false);
		var value = new BitValue(false).NarrowSetBits(0x100UL);

		BitTrackerOperations.Xor64(result, value, value);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0UL, result.BitsSet);
	}
}

public partial class BitTrackerOperationsRemUnsignedTests
{
	[Fact]
	public void RemUnsigned32_PartialKnownBits_DoesNotOverConstrain()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowSetBits(0b0001).NarrowClearBits(0b1110);
		var value2 = new BitValue(true).NarrowSetBits(0b0011).NarrowClearBits(0b1100);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.MinValue);
		Assert.False(result.IsStable);
	}
}

// ─── Regression: Fix 1 – Add64 dead-else removed ─────────────────────────────

public class BitTrackerOperationsAdd64RegressionTests
{
	[Fact]
	public void Add64_NoCarry_NarrowsMinMax()
	{
		// Both values have bounded ranges that don't overflow; result should be narrowed.
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMin(10).NarrowMax(20);
		var value2 = new BitValue(false).NarrowMin(5).NarrowMax(15);

		BitTrackerOperations.Add64(result, value1, value2);

		Assert.Equal(15UL, result.MinValue);
		Assert.Equal(35UL, result.MaxValue);
	}

	[Fact]
	public void Add64_CarryOnMax_DoesNotNarrow()
	{
		// MaxValue addition overflows — result must remain unconstrained.
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMin(ulong.MaxValue - 5).NarrowMax(ulong.MaxValue);
		var value2 = new BitValue(false).NarrowMin(10).NarrowMax(20);

		BitTrackerOperations.Add64(result, value1, value2);

		// Cannot narrow min/max when carry is possible — max must remain at ulong.MaxValue
		Assert.Equal(ulong.MaxValue, result.MaxValue);
	}
}

// ─── Regression: Fix 2 – And32 used BitsSet32 not (BitsSet & ulong.MaxValue) ─

public class BitTrackerOperationsAnd32RegressionTests
{
	[Fact]
	public void And32_Value1Lower32Zero_ReturnsZero()
	{
		// lower 32 bits are known zero — result must be zero regardless of upper bits.
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 0xABCDu);

		BitTrackerOperations.And32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void And32_Value2Lower32Zero_ReturnsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0x1234u);
		var value2 = new BitValue(true, 0u);

		BitTrackerOperations.And32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}
}

// ─── Regression: Fix 3 – ShiftRight64 SetStable was missing value2 ───────────

public class BitTrackerOperationsShiftRight64RegressionTests
{
	[Fact]
	public void ShiftRight64_KnownShift_PartialValue_IsStable()
	{
		// value1 partially known, shift known → result bits should be propagated correctly.
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowSetBits(0xFF00UL).NarrowClearBits(0x00FFUL);
		var value2 = new BitValue(false, 8UL);

		BitTrackerOperations.ShiftRight64(result, value1, value2);

		// FF00 >> 8 = FF — lower byte must be known set
		Assert.Equal(0xFFUL, result.BitsSet & 0xFFUL);
	}

	[Fact]
	public void ShiftRight64_KnownShift_CorrectBitPropagation()
	{
		// Shift 0xFF00 right by 8 → 0xFF known set in lower byte
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowSetBits(0xFF00UL).NarrowClearBits(0xFFFFFFFFFFFF00FFUL);
		var value2 = new BitValue(false, 8UL);

		BitTrackerOperations.ShiftRight64(result, value1, value2);

		Assert.Equal(0xFFUL, result.BitsSet & 0xFFUL);
	}
}

// ─── Regression: Fix 4 – RemUnsigned GetBitsOver used BitsSet instead of MaxValue ─

public class BitTrackerOperationsRemUnsignedBoundsRegressionTests
{
	[Fact]
	public void RemUnsigned32_KnownDivisor_MaxValueBoundCorrect()
	{
		// divisor = 10 (known), so result max must be 9
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMax(100);
		var value2 = new BitValue(true, 10u);

		BitTrackerOperations.RemUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(9u, result.MaxValue);
	}

	[Fact]
	public void RemUnsigned64_KnownDivisor_MaxValueBoundCorrect()
	{
		// divisor = 7 (known), so result max must be 6
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMax(100);
		var value2 = new BitValue(false, 7UL);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(6UL, result.MaxValue);
	}

	[Fact]
	public void RemUnsigned64_UnknownDivisor_MaxValueBoundCorrect()
	{
		// divisor range [3, 8], so result max must be 7 (MaxValue - 1)
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMax(200);
		var value2 = new BitValue(false).NarrowMin(3).NarrowMax(8);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(7UL, result.MaxValue);
	}
}

// ─── Regression: Fix 5 – RemUnsigned64 consolidated divide-by-zero guard ─────

public class BitTrackerOperationsRemUnsigned64GuardRegressionTests
{
	[Fact]
	public void RemUnsigned64_IsZero_DoesNotChangeResult()
	{
		// value2 is known zero via IsZero path
		var result = new BitValue(false);
		var value1 = new BitValue(false, 99UL);
		var value2 = new BitValue(false, 0UL);

		BitTrackerOperations.RemUnsigned64(result, value1, value2);

		// Early return — result must remain the default (all bits unknown, not stable)
		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(ulong.MaxValue, result.MaxValue);
		Assert.False(result.IsStable);
	}
}

// ─── AddCarryIn ───────────────────────────────────────────────────────────────

public class BitTrackerOperationsAddCarryIn32Tests
{
	[Fact]
	public void AddCarryIn32_AllKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 10u);
		var value2 = new BitValue(true, 20u);
		var value3 = new BitValue(true, 1u);

		BitTrackerOperations.AddCarryIn32(result, value1, value2, value3);

		Assert.Equal(31u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void AddCarryIn32_Value1AndCarryZero_NarrowsToValue2()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 42u);
		var value3 = new BitValue(true, 0u);

		BitTrackerOperations.AddCarryIn32(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void AddCarryIn32_Value2AndCarryZero_NarrowsToValue1()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 99u);
		var value2 = new BitValue(true, 0u);
		var value3 = new BitValue(true, 0u);

		BitTrackerOperations.AddCarryIn32(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void AddCarryIn32_Partial_IsStable()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true);
		var value2 = new BitValue(true);
		var value3 = new BitValue(true);

		BitTrackerOperations.AddCarryIn32(result, value1, value2, value3);

		Assert.False(result.IsStable);
	}
}

public class BitTrackerOperationsAddCarryIn64Tests
{
	[Fact]
	public void AddCarryIn64_AllKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 100UL);
		var value2 = new BitValue(false, 200UL);
		var value3 = new BitValue(false, 1UL);

		BitTrackerOperations.AddCarryIn64(result, value1, value2, value3);

		Assert.Equal(301UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void AddCarryIn64_Value1AndCarryZero_NarrowsToValue2()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 500UL);
		var value3 = new BitValue(false, 0UL);

		BitTrackerOperations.AddCarryIn64(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void AddCarryIn64_Value2AndCarryZero_NarrowsToValue1()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 777UL);
		var value2 = new BitValue(false, 0UL);
		var value3 = new BitValue(false, 0UL);

		BitTrackerOperations.AddCarryIn64(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

// ─── SubCarryIn ───────────────────────────────────────────────────────────────

public class BitTrackerOperationsSubCarryIn32Tests
{
	[Fact]
	public void SubCarryIn32_AllKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 100u);
		var value2 = new BitValue(true, 30u);
		var value3 = new BitValue(true, 5u);

		BitTrackerOperations.SubCarryIn32(result, value1, value2, value3);

		Assert.Equal(65u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void SubCarryIn32_BorrowAndSubtrahendZero_NarrowsToValue1()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 77u);
		var value2 = new BitValue(true, 0u);
		var value3 = new BitValue(true, 0u);

		BitTrackerOperations.SubCarryIn32(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void SubCarryIn32_Partial_NotStable()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true);
		var value2 = new BitValue(true);
		var value3 = new BitValue(true);

		BitTrackerOperations.SubCarryIn32(result, value1, value2, value3);

		Assert.False(result.IsStable);
	}
}

public class BitTrackerOperationsSubCarryIn64Tests
{
	[Fact]
	public void SubCarryIn64_AllKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 1000UL);
		var value2 = new BitValue(false, 400UL);
		var value3 = new BitValue(false, 100UL);

		BitTrackerOperations.SubCarryIn64(result, value1, value2, value3);

		Assert.Equal(500UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void SubCarryIn64_BorrowAndSubtrahendZero_NarrowsToValue1()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 321UL);
		var value2 = new BitValue(false, 0UL);
		var value3 = new BitValue(false, 0UL);

		BitTrackerOperations.SubCarryIn64(result, value1, value2, value3);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

// ─── MulSigned ────────────────────────────────────────────────────────────────

public class BitTrackerOperationsMulSigned32Tests
{
	[Fact]
	public void MulSigned32_BothKnownPositive()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 6u);
		var value2 = new BitValue(true, 7u);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.Equal(42u, result.BitsSet32);
	}

	[Fact]
	public void MulSigned32_FirstIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 123u);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void MulSigned32_SecondIsZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 456u);
		var value2 = new BitValue(true, 0u);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void MulSigned32_FirstIsOne_NarrowsToSecond()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 1u);
		var value2 = new BitValue(true, 99u);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void MulSigned32_SecondIsOne_NarrowsToFirst()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 55u);
		var value2 = new BitValue(true, 1u);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void MulSigned32_BothKnownNegativeResult()
	{
		// (int)-3 * (int)2 = -6
		var result = new BitValue(true);
		var value1 = new BitValue(true, unchecked((uint)-3));
		var value2 = new BitValue(true, 2u);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.Equal(unchecked((uint)((int)unchecked((uint)-3) * 2)), result.BitsSet32);
	}
}

public class BitTrackerOperationsMulSigned64Tests
{
	[Fact]
	public void MulSigned64_BothKnownPositive()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 11UL);
		var value2 = new BitValue(false, 11UL);

		BitTrackerOperations.MulSigned64(result, value1, value2);

		Assert.Equal(121UL, result.BitsSet);
	}

	[Fact]
	public void MulSigned64_FirstIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 9999UL);

		BitTrackerOperations.MulSigned64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void MulSigned64_FirstIsOne_NarrowsToSecond()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 1UL);
		var value2 = new BitValue(false, 42UL);

		BitTrackerOperations.MulSigned64(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void MulSigned64_SecondIsOne_NarrowsToFirst()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 7UL);
		var value2 = new BitValue(false, 1UL);

		BitTrackerOperations.MulSigned64(result, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}
}

// ─── RemSigned ────────────────────────────────────────────────────────────────

public class BitTrackerOperationsRemSigned32Tests
{
	[Fact]
	public void RemSigned32_BothPositiveKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 17u);
		var value2 = new BitValue(true, 5u);

		BitTrackerOperations.RemSigned32(result, value1, value2);

		Assert.Equal(2u, result.BitsSet32);
	}

	[Fact]
	public void RemSigned32_DividendZero_ResultZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 7u);

		BitTrackerOperations.RemSigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void RemSigned32_DivisorZero_DoesNotChangeResult()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 50u);
		var value2 = new BitValue(true, 0u);

		BitTrackerOperations.RemSigned32(result, value1, value2);

		Assert.False(result.IsStable);
	}

	[Fact]
	public void RemSigned32_BothKnownNegativeDividend()
	{
		// -17 % 5 = -2 in C# signed semantics
		var result = new BitValue(true);
		var value1 = new BitValue(true, unchecked((uint)-17));
		var value2 = new BitValue(true, 5u);

		BitTrackerOperations.RemSigned32(result, value1, value2);

		var expected = unchecked((uint)(int)((long)(int)unchecked((uint)-17) % (long)(int)5u));
		Assert.Equal(expected, result.BitsSet32);
	}
}

public class BitTrackerOperationsRemSigned64Tests
{
	[Fact]
	public void RemSigned64_BothPositiveKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 100UL);
		var value2 = new BitValue(false, 30UL);

		BitTrackerOperations.RemSigned64(result, value1, value2);

		Assert.Equal(10UL, result.BitsSet);
	}

	[Fact]
	public void RemSigned64_DividendZero_ResultZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 9UL);

		BitTrackerOperations.RemSigned64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void RemSigned64_DivisorZero_DoesNotChangeResult()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 99UL);
		var value2 = new BitValue(false, 0UL);

		BitTrackerOperations.RemSigned64(result, value1, value2);

		Assert.False(result.IsStable);
	}
}

// ─── DivSigned ────────────────────────────────────────────────────────────────

public class BitTrackerOperationsDivSigned32Tests
{
	[Fact]
	public void DivSigned32_BothPositiveKnown()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 40u);
		var value2 = new BitValue(true, 8u);

		BitTrackerOperations.DivSigned32(result, value1, value2);

		Assert.Equal(5u, result.BitsSet32);
	}

	[Fact]
	public void DivSigned32_DividendZero_ResultZero()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 4u);

		BitTrackerOperations.DivSigned32(result, value1, value2);

		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void DivSigned32_DivisorZero_DoesNotChangeResult()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 100u);
		var value2 = new BitValue(true, 0u);

		BitTrackerOperations.DivSigned32(result, value1, value2);

		Assert.False(result.IsStable);
	}

	[Fact]
	public void DivSigned32_BothKnownNegativeDivisor()
	{
		// 40 / -8 = -5 in signed
		var result = new BitValue(true);
		var value1 = new BitValue(true, 40u);
		var value2 = new BitValue(true, unchecked((uint)-8));

		BitTrackerOperations.DivSigned32(result, value1, value2);

		var expected = unchecked((uint)(int)((long)(int)40 / (long)(int)unchecked((uint)-8)));
		Assert.Equal(expected, result.BitsSet32);
	}
}

public class BitTrackerOperationsDivSigned64Tests
{
	[Fact]
	public void DivSigned64_BothPositiveKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 500UL);
		var value2 = new BitValue(false, 25UL);

		BitTrackerOperations.DivSigned64(result, value1, value2);

		Assert.Equal(20UL, result.BitsSet);
	}

	[Fact]
	public void DivSigned64_DividendZero_ResultZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 0UL);
		var value2 = new BitValue(false, 10UL);

		BitTrackerOperations.DivSigned64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
		Assert.True(result.AreAll64BitsKnown);
	}

	[Fact]
	public void DivSigned64_DivisorZero_DoesNotChangeResult()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 100UL);
		var value2 = new BitValue(false, 0UL);

		BitTrackerOperations.DivSigned64(result, value1, value2);

		Assert.False(result.IsStable);
	}
}

// ─── DivUnsigned additional ───────────────────────────────────────────────────

public class BitTrackerOperationsDivUnsignedAdditionalTests
{
	[Fact]
	public void DivUnsigned32_DivisorZero_DoesNotChangeResult()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 50u);
		var value2 = new BitValue(true, 0u);

		BitTrackerOperations.DivUnsigned32(result, value1, value2);

		Assert.False(result.IsStable);
	}

	[Fact]
	public void DivUnsigned32_PartialKnowledge_NarrowsRange()
	{
		// dividend [0..100], divisor [5..10] → quotient in [0..20]
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMax(100);
		var value2 = new BitValue(true).NarrowMin(5).NarrowMax(10);

		BitTrackerOperations.DivUnsigned32(result, value1, value2);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(20u, result.MaxValue);
	}

	[Fact]
	public void DivUnsigned64_DivisorZero_DoesNotChangeResult()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(false, 50UL);
		var value2 = new BitValue(false, 0UL);

		BitTrackerOperations.DivUnsigned64(result, value1, value2);

		Assert.False(result.IsStable);
	}

	[Fact]
	public void DivUnsigned64_PartialKnowledge_NarrowsRange()
	{
		// dividend [0..200], divisor [4..8] → quotient in [0..50]
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMax(200);
		var value2 = new BitValue(false).NarrowMin(4).NarrowMax(8);

		BitTrackerOperations.DivUnsigned64(result, value1, value2);

		Assert.Equal(0UL, result.MinValue);
		Assert.Equal(50UL, result.MaxValue);
	}
}

// ─── Compare ──────────────────────────────────────────────────────────────────

public class BitTrackerOperationsCompareTests
{
	[Fact]
	public void Compare_Equal_BothKnownSameValue_ReturnsTrue()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 42u);
		var value2 = new BitValue(true, 42u);

		BitTrackerOperations.Compare(result, value1, value2, ConditionCode.Equal);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(1UL, result.BitsSet);
	}

	[Fact]
	public void Compare_Equal_BothKnownDifferentValue_ReturnsFalse()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 1u);
		var value2 = new BitValue(true, 2u);

		BitTrackerOperations.Compare(result, value1, value2, ConditionCode.Equal);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0UL, result.BitsSet);
	}

	[Fact]
	public void Compare_NotEqual_BothKnownDifferent_ReturnsTrue()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 10u);
		var value2 = new BitValue(true, 20u);

		BitTrackerOperations.Compare(result, value1, value2, ConditionCode.NotEqual);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(1UL, result.BitsSet);
	}

	[Fact]
	public void Compare_UnsignedGreaterThan_BothKnown_ReturnsTrue()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true, 10u);
		var value2 = new BitValue(true, 5u);

		BitTrackerOperations.Compare(result, value1, value2, ConditionCode.UnsignedGreater);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(1UL, result.BitsSet);
	}

	[Fact]
	public void Compare_Unknown_NarrowsToBoolean()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true);
		var value2 = new BitValue(true);

		BitTrackerOperations.Compare(result, value1, value2, ConditionCode.Equal);

		// Must narrow to boolean — max value 1, min value 0
		Assert.Equal(0u, result.MinValue);
		Assert.Equal(1u, result.MaxValue);
	}
}

// ─── LoadParam ZeroExtend ─────────────────────────────────────────────────────

public class BitTrackerOperationsLoadParamZeroExtendTests
{
	[Fact]
	public void LoadParamZeroExtend8x32_NarrowsToByteRange()
	{
		var result = new BitValue(true);
		BitTrackerOperations.LoadParamZeroExtend8x32(result);
		Assert.Equal((ulong)byte.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)byte.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadParamZeroExtend16x32_NarrowsToUShortRange()
	{
		var result = new BitValue(true);
		BitTrackerOperations.LoadParamZeroExtend16x32(result);
		Assert.Equal((ulong)ushort.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)ushort.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadParamZeroExtend32x64_NarrowsToUIntRange()
	{
		var result = new BitValue(false);
		BitTrackerOperations.LoadParamZeroExtend32x64(result);
		Assert.Equal((ulong)uint.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)uint.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadParamZeroExtend8x64_NarrowsToByteRange()
	{
		var result = new BitValue(false);
		BitTrackerOperations.LoadParamZeroExtend8x64(result);
		Assert.Equal((ulong)byte.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)byte.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadParamZeroExtend16x64_NarrowsToUShortRange()
	{
		var result = new BitValue(false);
		BitTrackerOperations.LoadParamZeroExtend16x64(result);
		Assert.Equal((ulong)ushort.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)ushort.MaxValue, result.BitsClear);
	}
}

// ─── Load ZeroExtend ──────────────────────────────────────────────────────────

public class BitTrackerOperationsLoadZeroExtendTests
{
	[Fact]
	public void LoadZeroExtend8x32_NarrowsToByteRange()
	{
		var result = new BitValue(true);
		BitTrackerOperations.LoadZeroExtend8x32(result);
		Assert.Equal((ulong)byte.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)byte.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadZeroExtend16x32_NarrowsToUShortRange()
	{
		var result = new BitValue(true);
		BitTrackerOperations.LoadZeroExtend16x32(result);
		Assert.Equal((ulong)ushort.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)ushort.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadZeroExtend32x64_NarrowsToUIntRange()
	{
		var result = new BitValue(false);
		BitTrackerOperations.LoadZeroExtend32x64(result);
		Assert.Equal((ulong)uint.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)uint.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadZeroExtend8x64_NarrowsToByteRange()
	{
		var result = new BitValue(false);
		BitTrackerOperations.LoadZeroExtend8x64(result);
		Assert.Equal((ulong)byte.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)byte.MaxValue, result.BitsClear);
	}

	[Fact]
	public void LoadZeroExtend16x64_NarrowsToUShortRange()
	{
		var result = new BitValue(false);
		BitTrackerOperations.LoadZeroExtend16x64(result);
		Assert.Equal((ulong)ushort.MaxValue, result.MaxValue);
		Assert.Equal(~(ulong)ushort.MaxValue, result.BitsClear);
	}
}

// ─── ZeroExtend16x64 ──────────────────────────────────────────────────────────

public class BitTrackerOperationsZeroExtend16x64Tests
{
	[Fact]
	public void ZeroExtend16x64_AllBitsKnown()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0xABCDu);

		BitTrackerOperations.ZeroExtend16x64(result, value1);

		Assert.Equal(0xABCDUL, result.BitsSet);
		Assert.True(result.AreLower16BitsKnown);
	}

	[Fact]
	public void ZeroExtend16x64_MaxUShort()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, (uint)ushort.MaxValue);

		BitTrackerOperations.ZeroExtend16x64(result, value1);

		Assert.Equal((ulong)ushort.MaxValue, result.BitsSet);
		Assert.Equal(~(ulong)ushort.MaxValue, result.BitsClear);
	}

	[Fact]
	public void ZeroExtend16x64_Partial_NarrowsUpperBits()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true).NarrowMax(1000);

		BitTrackerOperations.ZeroExtend16x64(result, value1);

		Assert.True(result.MaxValue <= ushort.MaxValue);
		Assert.Equal(~(ulong)ushort.MaxValue, result.BitsClear & ~(ulong)ushort.MaxValue);
	}
}

// ─── IfThenElse additional ────────────────────────────────────────────────────

public class BitTrackerOperationsIfThenElseAdditionalTests
{
	[Fact]
	public void IfThenElse32_ConditionNonZero_NarrowsToValue1()
	{
		var result = new BitValue(true);
		var condition = new BitValue(true, 1u);
		var value1 = new BitValue(true, 42u);
		var value2 = new BitValue(true, 99u);

		BitTrackerOperations.IfThenElse32(result, condition, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void IfThenElse32_ConditionZero_NarrowsToValue2()
	{
		var result = new BitValue(true);
		var condition = new BitValue(true, 0u);
		var value1 = new BitValue(true, 42u);
		var value2 = new BitValue(true, 99u);

		BitTrackerOperations.IfThenElse32(result, condition, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void IfThenElse64_ConditionNonZero_NarrowsToValue1()
	{
		var result = new BitValue(false);
		var condition = new BitValue(false, 1UL);
		var value1 = new BitValue(false, 777UL);
		var value2 = new BitValue(false, 888UL);

		BitTrackerOperations.IfThenElse64(result, condition, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value1.BitsSet, result.BitsSet);
	}

	[Fact]
	public void IfThenElse64_ConditionZero_NarrowsToValue2()
	{
		var result = new BitValue(false);
		var condition = new BitValue(false, 0UL);
		var value1 = new BitValue(false, 777UL);
		var value2 = new BitValue(false, 888UL);

		BitTrackerOperations.IfThenElse64(result, condition, value1, value2);

		Assert.True(result.IsStable);
		Assert.Equal(value2.BitsSet, result.BitsSet);
	}

	[Fact]
	public void IfThenElse32_ConditionUnknown_EncompassesBothValues()
	{
		var result = new BitValue(true);
		var condition = new BitValue(true);
		var value1 = new BitValue(true, 10u);
		var value2 = new BitValue(true, 20u);

		BitTrackerOperations.IfThenElse32(result, condition, value1, value2);

		Assert.Equal(10u, result.MinValue);
		Assert.Equal(20u, result.MaxValue);
	}
}

// ─── Result2NarrowToBoolean ───────────────────────────────────────────────────

public class BitTrackerOperationsResult2NarrowToBooleanTests
{
	[Fact]
	public void Result2NarrowToBoolean_NarrowsToBoolRange()
	{
		var result = new BitValue(true);
		result.NarrowMax(1000); // give it a wide range first

		BitTrackerOperations.Result2NarrowToBoolean(result);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(1u, result.MaxValue);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void Result2NarrowToBoolean_AlreadyBoolean_RemainsBoolean()
	{
		var result = new BitValue(true);
		result.NarrowMax(1);

		BitTrackerOperations.Result2NarrowToBoolean(result);

		Assert.Equal(0u, result.MinValue);
		Assert.Equal(1u, result.MaxValue);
	}
}

// ─── To64 additional ─────────────────────────────────────────────────────────

public class BitTrackerOperationsTo64AdditionalTests
{
	[Fact]
	public void To64_Low32Zero_High32Known()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 0xABCDu);

		BitTrackerOperations.To64(result, value1, value2);

		Assert.Equal(0xABCD00000000UL, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void To64_BothNonZero_Combines()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0x00000001u);
		var value2 = new BitValue(true, 0x00000002u);

		BitTrackerOperations.To64(result, value1, value2);

		Assert.Equal(0x0000000200000001UL, result.BitsSet);
	}

	[Fact]
	public void To64_Partial_NarrowsMinMaxBits()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true).NarrowMin(0).NarrowMax(0xFF);
		var value2 = new BitValue(true).NarrowMin(0).NarrowMax(0xF);

		BitTrackerOperations.To64(result, value1, value2);

		Assert.Equal(0x0F000000FFUL, result.MaxValue);
	}
}

// ─── Xor32 additional ────────────────────────────────────────────────────────

public partial class BitTrackerOperationsXor32Tests
{
	[Fact]
	public void Xor32_SameOperand_IsZero()
	{
		var result = new BitValue(true);
		var value = new BitValue(true).NarrowSetBits(0xABCDu);

		BitTrackerOperations.Xor32(result, value, value);

		Assert.True(result.AreAll64BitsKnown);
		Assert.Equal(0u, result.BitsSet32);
	}

	[Fact]
	public void Xor32_PartialKnownBits_TracksKnownIntersection()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowSetBits(0xF0u).NarrowClearBits(0x0Fu);
		var value2 = new BitValue(true).NarrowSetBits(0xAAu).NarrowClearBits(0x55u);

		BitTrackerOperations.Xor32(result, value1, value2);

		Assert.Equal((0xF0u ^ 0xAAu) & 0xFFu, result.BitsSet & 0xFFu);
		Assert.Equal(0xFFu, result.BitsKnown & 0xFFu);
	}
}

// ─── Add32: edge cases ────────────────────────────────────────────────────────

public class BitTrackerOperationsAdd32EdgeCaseTests
{
	[Fact]
	public void Add32_MaxValuePlusOne_OverflowNotNarrowed()
	{
		// uint.MaxValue + 1 wraps → result cannot be narrowed
		var result = new BitValue(true);
		var value1 = new BitValue(true, uint.MaxValue);
		var value2 = new BitValue(true, 1u);

		BitTrackerOperations.Add32(result, value1, value2);

		// Exact value wraps to 0 since both known
		Assert.Equal(0u, result.BitsSet32);
		Assert.True(result.AreLower32BitsKnown);
	}

	[Fact]
	public void Add32_BothRangeBounded_NarrowsMinMax()
	{
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin(10).NarrowMax(20);
		var value2 = new BitValue(true).NarrowMin(5).NarrowMax(15);

		BitTrackerOperations.Add32(result, value1, value2);

		Assert.Equal(15u, result.MinValue);
		Assert.Equal(35u, result.MaxValue);
	}

	[Fact]
	public void Add32_MaxRangeOverflows_NotNarrowed()
	{
		// Max of range addition overflows 32 bits → can't narrow
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin((ulong)(uint.MaxValue - 5)).NarrowMax(uint.MaxValue);
		var value2 = new BitValue(true).NarrowMin(10).NarrowMax(20);

		BitTrackerOperations.Add32(result, value1, value2);

		// Overflow: MaxValue should remain uint.MaxValue
		Assert.Equal(uint.MaxValue, result.MaxValue);
	}
}

// ─── MulUnsigned32: overflow case ────────────────────────────────────────────

public class BitTrackerOperationsMulUnsigned32OverflowTests
{
	[Fact]
	public void MulUnsigned32_Overflow_OnlyUpperBitsClear()
	{
		// 0x10000 * 0x10000 = 0x100000000 which overflows 32 bits
		// Only the upper 32 bits of the result must be asserted clear; lower 32 are unknown
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMax(0x10000);
		var value2 = new BitValue(true).NarrowMax(0x10000);

		BitTrackerOperations.MulUnsigned32(result, value1, value2);

		// Upper 32 bits must be clear (it's a 32-bit operation)
		Assert.Equal(0UL, result.BitsSet & ~(ulong)uint.MaxValue);
	}

	[Fact]
	public void MulUnsigned32_NoOverflow_NarrowsRange()
	{
		// 3 * 4 = 12; no overflow, range and bits should be narrowed
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin(2).NarrowMax(3);
		var value2 = new BitValue(true).NarrowMin(4).NarrowMax(5);

		BitTrackerOperations.MulUnsigned32(result, value1, value2);

		Assert.Equal(8u, result.MinValue);   // 2*4
		Assert.Equal(15u, result.MaxValue);  // 3*5
	}
}

// ─── MulSigned32: range narrowing ────────────────────────────────────────────

public class BitTrackerOperationsMulSigned32RangeTests
{
	[Fact]
	public void MulSigned32_BothPositiveRange_CorrectMinMax()
	{
		// value1 in [2,10], value2 in [3,5] → result in [2*3=6, 10*5=50]
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin(2).NarrowMax(10);
		var value2 = new BitValue(true).NarrowMin(3).NarrowMax(5);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		Assert.Equal(6u, result.MinValue);
		Assert.Equal(50u, result.MaxValue);
	}

	[Fact]
	public void MulSigned32_NoOverflow_MaxBitsCleared()
	{
		// max product = 50 → bits above 50 must be clear
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin(2).NarrowMax(10);
		var value2 = new BitValue(true).NarrowMin(3).NarrowMax(5);

		BitTrackerOperations.MulSigned32(result, value1, value2);

		// 50 = 0b110010 → bits 6 and higher must be cleared
		Assert.Equal(0UL, result.BitsSet & ~63UL);
	}
}

// ─── MulSigned64: range narrowing ────────────────────────────────────────────

public class BitTrackerOperationsMulSigned64RangeTests
{
	[Fact]
	public void MulSigned64_BothPositiveRange_CorrectMinMax()
	{
		// value1 in [2,10], value2 in [3,5] → result in [6, 50]
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMin(2).NarrowMax(10);
		var value2 = new BitValue(false).NarrowMin(3).NarrowMax(5);

		BitTrackerOperations.MulSigned64(result, value1, value2);

		Assert.Equal(6UL, result.MinValue);
		Assert.Equal(50UL, result.MaxValue);
	}

	[Fact]
	public void MulSigned64_NoOverflow_MaxBitsCleared()
	{
		// max product = 50 → bits above 50 must be clear
		var result = new BitValue(false);
		var value1 = new BitValue(false).NarrowMin(2).NarrowMax(10);
		var value2 = new BitValue(false).NarrowMin(3).NarrowMax(5);

		BitTrackerOperations.MulSigned64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet & ~63UL);
	}
}

// ─── To64: 64-bit operands with only lower 32 bits known ─────────────────────

public class BitTrackerOperationsTo64EdgeCaseTests
{
	[Fact]
	public void To64_64BitOperands_UsesBitsSet32NotMaxValue()
	{
		// value1: 64-bit, AreLower32BitsKnown=true, BitsSet32=5 but MaxValue could be wider
		// value2: 64-bit, AreLower32BitsKnown=true, BitsSet32=3 but MaxValue could be wider
		// The known-path result must use BitsSet32, not MaxValue.
		var result = new BitValue(false);
		var value1 = new BitValue(false, 5UL); // all bits known; BitsSet=5, MaxValue=5
		var value2 = new BitValue(false, 3UL); // all bits known; BitsSet=3, MaxValue=3

		BitTrackerOperations.To64(result, value1, value2);

		// Expected: (3UL << 32) | 5UL = 0x0000000300000005UL
		Assert.Equal(0x0000000300000005UL, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void To64_MaxUInt32Parts_NoTruncation()
	{
		// Verify boundary: value1 = uint.MaxValue, value2 = uint.MaxValue
		var result = new BitValue(false);
		var value1 = new BitValue(true, uint.MaxValue);
		var value2 = new BitValue(true, uint.MaxValue);

		BitTrackerOperations.To64(result, value1, value2);

		// Expected: (uint.MaxValue << 32) | uint.MaxValue = ulong.MaxValue
		Assert.Equal(ulong.MaxValue, result.BitsSet);
		Assert.True(result.IsStable);
	}

	[Fact]
	public void To64_ZeroAndZero_ResultIsZero()
	{
		var result = new BitValue(false);
		var value1 = new BitValue(true, 0u);
		var value2 = new BitValue(true, 0u);

		BitTrackerOperations.To64(result, value1, value2);

		Assert.Equal(0UL, result.BitsSet);
		Assert.True(result.IsStable);
	}
}

// ─── Add32: range-narrowing regression ───────────────────────────────────────
// Previously had dead conditions: value1/2.MaxValue > uint.MaxValue (always false for 32-bit).
// These tests verify the carry/non-carry branching is still correct after cleanup.

public class BitTrackerOperationsAdd32CarryRegressionTests
{
	[Fact]
	public void Add32_MinRangeOverflows_NotNarrowed()
	{
		// MinValue addition also overflows 32 bits → range spans wrap-around, can't narrow
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin(uint.MaxValue - 2).NarrowMax(uint.MaxValue);
		var value2 = new BitValue(true).NarrowMin(5).NarrowMax(10);

		BitTrackerOperations.Add32(result, value1, value2);

		// Both min and max additions overflow → MaxValue must remain uint.MaxValue
		Assert.Equal(uint.MaxValue, result.MaxValue);
	}

	[Fact]
	public void Add32_MaxCarryMinNoCarry_MaxNotNarrowed()
	{
		// Max addition overflows but min addition does not → stable but not narrowed
		var result = new BitValue(true);
		var value1 = new BitValue(true).NarrowMin(10).NarrowMax(uint.MaxValue);
		var value2 = new BitValue(true).NarrowMin(1).NarrowMax(5);

		BitTrackerOperations.Add32(result, value1, value2);

		// Max overflows → cannot narrow MaxValue
		Assert.Equal(uint.MaxValue, result.MaxValue);
	}
}
