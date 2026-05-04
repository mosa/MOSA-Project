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
