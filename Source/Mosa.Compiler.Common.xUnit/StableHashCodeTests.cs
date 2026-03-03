// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class StableHashCodeTests
{
	[Fact]
	public void Constructor_InitializesWithOffsetBasis()
	{
		var hash1 = new StableHashCode();
		var hash2 = new StableHashCode();

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_Bool_True()
	{
		var hash = new StableHashCode();
		hash.Add(true);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Bool_False()
	{
		var hash = new StableHashCode();
		hash.Add(false);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Bool_DifferentValues()
	{
		var hash1 = new StableHashCode();
		hash1.Add(true);

		var hash2 = new StableHashCode();
		hash2.Add(false);

		Assert.NotEqual(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_Byte()
	{
		var hash = new StableHashCode();
		hash.Add((byte)42);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Short()
	{
		var hash = new StableHashCode();
		hash.Add((short)42);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_UShort()
	{
		var hash = new StableHashCode();
		hash.Add((ushort)42);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Int()
	{
		var hash = new StableHashCode();
		hash.Add(42);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_UInt()
	{
		var hash = new StableHashCode();
		hash.Add(42u);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Long()
	{
		var hash = new StableHashCode();
		hash.Add(42L);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_ULong()
	{
		var hash = new StableHashCode();
		hash.Add(42UL);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Char()
	{
		var hash = new StableHashCode();
		hash.Add('A');

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_String()
	{
		var hash = new StableHashCode();
		hash.Add("test");

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_String_Null()
	{
		var hash = new StableHashCode();
		hash.Add((string?)null);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_String_Empty()
	{
		var hash = new StableHashCode();
		hash.Add(string.Empty);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_String_DifferentStrings_ProduceDifferentHashes()
	{
		var hash1 = new StableHashCode();
		hash1.Add("hello");

		var hash2 = new StableHashCode();
		hash2.Add("world");

		Assert.NotEqual(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_String_SameStrings_ProduceSameHashes()
	{
		var hash1 = new StableHashCode();
		hash1.Add("test");

		var hash2 = new StableHashCode();
		hash2.Add("test");

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_Generic_NullValue()
	{
		var hash = new StableHashCode();
		hash.Add<string?>(null);

		var expectedHash = new StableHashCode();
		expectedHash.Add(0u);

		Assert.Equal(expectedHash.ToHashCode(), hash.ToHashCode());
	}

	[Fact]
	public void Add_Generic_WithComparer()
	{
		var hash = new StableHashCode();
		hash.Add("TEST", StringComparer.OrdinalIgnoreCase);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Generic_WithComparer_Null()
	{
		var hash = new StableHashCode();
		hash.Add<string?>(null, StringComparer.OrdinalIgnoreCase);

		var expectedHash = new StableHashCode();
		expectedHash.Add(0u);

		Assert.Equal(expectedHash.ToHashCode(), hash.ToHashCode());
	}

	[Fact]
	public void Add_MultipleValues_ProducesConsistentHash()
	{
		var hash1 = new StableHashCode();
		hash1.Add(1);
		hash1.Add(2);
		hash1.Add(3);

		var hash2 = new StableHashCode();
		hash2.Add(1);
		hash2.Add(2);
		hash2.Add(3);

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_Order_Matters()
	{
		var hash1 = new StableHashCode();
		hash1.Add(1);
		hash1.Add(2);

		var hash2 = new StableHashCode();
		hash2.Add(2);
		hash2.Add(1);

		Assert.NotEqual(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Combine_OneValue()
	{
		var result = StableHashCode.Combine(42);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_TwoValues()
	{
		var result = StableHashCode.Combine(42, "test");
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_ThreeValues()
	{
		var result = StableHashCode.Combine(42, "test", 3.14);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_FourValues()
	{
		var result = StableHashCode.Combine(42, "test", 3.14, true);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_FiveValues()
	{
		var result = StableHashCode.Combine(42, "test", 3.14, true, 'A');
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_SixValues()
	{
		var result = StableHashCode.Combine(42, "test", 3.14, true, 'A', 100L);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_SevenValues()
	{
		var result = StableHashCode.Combine(42, "test", 3.14, true, 'A', 100L, (byte)255);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_EightValues()
	{
		var result = StableHashCode.Combine(42, "test", 3.14, true, 'A', 100L, (byte)255, (short)32000);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_SameValues_ProduceSameHash()
	{
		var result1 = StableHashCode.Combine(42, "test");
		var result2 = StableHashCode.Combine(42, "test");

		Assert.Equal(result1, result2);
	}

	[Fact]
	public void Combine_DifferentValues_ProduceDifferentHashes()
	{
		var result1 = StableHashCode.Combine(42, "test");
		var result2 = StableHashCode.Combine(43, "test");

		Assert.NotEqual(result1, result2);
	}

	[Fact]
	public void Add_LargeNumbers()
	{
		var hash = new StableHashCode();
		hash.Add(int.MaxValue);
		hash.Add(int.MinValue);
		hash.Add(long.MaxValue);
		hash.Add(long.MinValue);
		hash.Add(uint.MaxValue);
		hash.Add(ulong.MaxValue);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void StableHashCode_IsStable()
	{
		var values = new[] { 1, 2, 3, 4, 5 };

		var hash1 = new StableHashCode();
		foreach (var value in values)
		{
			hash1.Add(value);
		}

		var hash2 = new StableHashCode();
		foreach (var value in values)
		{
			hash2.Add(value);
		}

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_ComplexObject_WithGenericMethod()
	{
		var hash = new StableHashCode();
		var obj = new { Name = "Test", Value = 42 };
		hash.Add(obj);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_Generic_PrimitiveTypes_UseOptimizedPaths()
	{
		var hashInt = new StableHashCode();
		hashInt.Add<int>(42);

		var hashUInt = new StableHashCode();
		hashUInt.Add<uint>(42u);

		var hashLong = new StableHashCode();
		hashLong.Add<long>(42L);

		var hashULong = new StableHashCode();
		hashULong.Add<ulong>(42UL);

		var hashShort = new StableHashCode();
		hashShort.Add<short>(42);

		var hashUShort = new StableHashCode();
		hashUShort.Add<ushort>(42);

		var hashByte = new StableHashCode();
		hashByte.Add<byte>(42);

		var hashBool = new StableHashCode();
		hashBool.Add<bool>(true);

		var hashChar = new StableHashCode();
		hashChar.Add<char>('A');

		var hashString = new StableHashCode();
		hashString.Add<string>("test");

		Assert.NotEqual(0, hashInt.ToHashCode());
		Assert.NotEqual(0, hashUInt.ToHashCode());
		Assert.NotEqual(0, hashLong.ToHashCode());
		Assert.NotEqual(0, hashULong.ToHashCode());
		Assert.NotEqual(0, hashShort.ToHashCode());
		Assert.NotEqual(0, hashUShort.ToHashCode());
		Assert.NotEqual(0, hashByte.ToHashCode());
		Assert.NotEqual(0, hashBool.ToHashCode());
		Assert.NotEqual(0, hashChar.ToHashCode());
		Assert.NotEqual(0, hashString.ToHashCode());
	}

	[Fact]
	public void Add_Long_SplitsIntoTwoUInts()
	{
		var hash1 = new StableHashCode();
		hash1.Add(0x0000000100000002L);

		var hash2 = new StableHashCode();
		hash2.Add(0x00000002u);
		hash2.Add(0x00000001u);

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_ULong_SplitsIntoTwoUInts()
	{
		var hash1 = new StableHashCode();
		hash1.Add(0x0000000100000002UL);

		var hash2 = new StableHashCode();
		hash2.Add(0x00000002u);
		hash2.Add(0x00000001u);

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void ToHashCode_ReturnsInt32()
	{
		var hash = new StableHashCode();
		hash.Add(42);

		var result = hash.ToHashCode();
		Assert.IsType<int>(result);
	}

	[Fact]
	public void Add_StringLength_Matters()
	{
		var hash1 = new StableHashCode();
		hash1.Add("a");

		var hash2 = new StableHashCode();
		hash2.Add("aa");

		Assert.NotEqual(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Combine_WithNulls()
	{
		var result = StableHashCode.Combine<string?, string?>(null, null);
		Assert.NotEqual(0, result);
	}

	[Fact]
	public void Combine_MixedTypes()
	{
		var result1 = StableHashCode.Combine(42, "test", 3.14, true);
		var result2 = StableHashCode.Combine(42, "test", 3.14, true);

		Assert.Equal(result1, result2);
	}

	[Fact]
	public void Add_Generic_FallsBackToGetHashCode()
	{
		var hash = new StableHashCode();
		var value = 3.14;
		hash.Add(value);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_WithComparer_UsesComparer()
	{
		var hash1 = new StableHashCode();
		hash1.Add("test", StringComparer.OrdinalIgnoreCase);

		var hash2 = new StableHashCode();
		hash2.Add("TEST", StringComparer.OrdinalIgnoreCase);

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_WithoutComparer_CaseSensitive()
	{
		var hash1 = new StableHashCode();
		hash1.Add("test");

		var hash2 = new StableHashCode();
		hash2.Add("TEST");

		Assert.NotEqual(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_NegativeNumbers()
	{
		var hash1 = new StableHashCode();
		hash1.Add(-42);

		var hash2 = new StableHashCode();
		hash2.Add(42);

		Assert.NotEqual(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void Add_Zero_ProducesNonZeroHash()
	{
		var hash = new StableHashCode();
		hash.Add(0);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_MultipleZeros()
	{
		var hash = new StableHashCode();
		hash.Add(0);
		hash.Add(0);
		hash.Add(0);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_SequentialIntegers()
	{
		var hashes = new int[10];
		for (int i = 0; i < 10; i++)
		{
			var hash = new StableHashCode();
			hash.Add(i);
			hashes[i] = hash.ToHashCode();
		}

		for (int i = 0; i < 10; i++)
		{
			for (int j = i + 1; j < 10; j++)
			{
				Assert.NotEqual(hashes[i], hashes[j]);
			}
		}
	}

	[Fact]
	public void Combine_SingleValue_EqualsAddMethod()
	{
		var combineResult = StableHashCode.Combine(42);

		var hash = new StableHashCode();
		hash.Add(42);
		var addResult = hash.ToHashCode();

		Assert.Equal(combineResult, addResult);
	}

	[Fact]
	public void Combine_TwoValues_EqualsAddMethod()
	{
		var combineResult = StableHashCode.Combine(42, "test");

		var hash = new StableHashCode();
		hash.Add(42);
		hash.Add("test");
		var addResult = hash.ToHashCode();

		Assert.Equal(combineResult, addResult);
	}

	[Fact]
	public void Add_MaxValues()
	{
		var hash = new StableHashCode();
		hash.Add(byte.MaxValue);
		hash.Add(short.MaxValue);
		hash.Add(ushort.MaxValue);
		hash.Add(int.MaxValue);
		hash.Add(uint.MaxValue);
		hash.Add(long.MaxValue);
		hash.Add(ulong.MaxValue);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_MinValues()
	{
		var hash = new StableHashCode();
		hash.Add(byte.MinValue);
		hash.Add(short.MinValue);
		hash.Add(ushort.MinValue);
		hash.Add(int.MinValue);
		hash.Add(uint.MinValue);
		hash.Add(long.MinValue);
		hash.Add(ulong.MinValue);

		Assert.NotEqual(0, hash.ToHashCode());
	}

	[Fact]
	public void Add_String_UnicodeSafe()
	{
		var hash1 = new StableHashCode();
		hash1.Add("Hello 世界");

		var hash2 = new StableHashCode();
		hash2.Add("Hello 世界");

		Assert.Equal(hash1.ToHashCode(), hash2.ToHashCode());
	}

	[Fact]
	public void StableHashCode_Deterministic()
	{
		var results = new int[100];
		for (int i = 0; i < 100; i++)
		{
			var hash = new StableHashCode();
			hash.Add(42);
			hash.Add("test");
			hash.Add(3.14);
			results[i] = hash.ToHashCode();
		}

		var firstResult = results[0];
		foreach (var result in results)
		{
			Assert.Equal(firstResult, result);
		}
	}

	[Fact]
	public void Combine_AllEightValues()
	{
		var result1 = StableHashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8);
		var result2 = StableHashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8);

		Assert.Equal(result1, result2);
	}

	[Fact]
	public void Combine_SevenValues_VsEightValues_Different()
	{
		var result7 = StableHashCode.Combine(1, 2, 3, 4, 5, 6, 7);
		var result8 = StableHashCode.Combine(1, 2, 3, 4, 5, 6, 7, 8);

		Assert.NotEqual(result7, result8);
	}
}
