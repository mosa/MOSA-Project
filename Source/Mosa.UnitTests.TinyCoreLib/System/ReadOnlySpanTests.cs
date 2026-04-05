// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.TinyCoreLib;

public static class ReadOnlySpanTests
{
	// == Constructor tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_ReadOnlySpan_Constructor_FromArray(int first)
	{
		int[] array = { first, 2, 3, 4, 5 };
		ReadOnlySpan<int> span = array;
		return span.Length == 5 && span[0] == first;
	}

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_Constructor_Empty()
	{
		var span = ReadOnlySpan<int>.Empty;
		return span.Length == 0 && span.IsEmpty;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_Constructor_WithStartAndLength(int v1, int v2, int v3)
	{
		int[] array = { 0, v1, v2, v3, 0 };
		var span = new ReadOnlySpan<int>(array, 1, 3);
		return span.Length == 3 && span[0] == v1 && span[1] == v2 && span[2] == v3;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_ReadOnlySpan_Constructor_FromSpan(int first)
	{
		int[] array = { first, 2, 3, 4, 5 };
		Span<int> span = array;
		ReadOnlySpan<int> readOnlySpan = span;
		return readOnlySpan.Length == 5 && readOnlySpan[0] == first;
	}

	// == Length tests

	[MosaUnitTest]
	public static int Test_ReadOnlySpan_Length_FromArray()
	{
		int[] array = { 1, 2, 3, 4, 5 };
		ReadOnlySpan<int> span = array;
		return span.Length;
	}

	[MosaUnitTest]
	public static int Test_ReadOnlySpan_Length_Empty()
	{
		var span = ReadOnlySpan<int>.Empty;
		return span.Length;
	}

	// == IsEmpty tests

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_IsEmpty_EmptySpan()
	{
		var span = ReadOnlySpan<int>.Empty;
		return span.IsEmpty;
	}

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_IsEmpty_NonEmptySpan()
	{
		int[] array = { 1, 2, 3 };
		ReadOnlySpan<int> span = array;
		return !span.IsEmpty;
	}

	// == Indexer tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_Indexer_Get(int v0, int v2, int v4)
	{
		int[] array = { v0, 0, v2, 0, v4 };
		ReadOnlySpan<int> span = array;
		return span[0] == v0 && span[2] == v2 && span[4] == v4;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static bool Test_ReadOnlySpan_Indexer_FirstLast(int first, int last)
	{
		int[] array = { first, 0, 0, 0, last };
		ReadOnlySpan<int> span = array;
		return span[0] == first && span[^1] == last;
	}

	// == Slice tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_ReadOnlySpan_Slice_WithStart(int v2)
	{
		int[] array = { 1, 2, v2, 4, 5 };
		var sliced = new ReadOnlySpan<int>(array).Slice(2);
		return sliced.Length == 3 && sliced[0] == v2;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_Slice_WithStartAndLength(int v1, int v2, int v3)
	{
		int[] array = { 0, v1, v2, v3, 0 };
		var sliced = new ReadOnlySpan<int>(array).Slice(1, 3);
		return sliced.Length == 3 && sliced[0] == v1 && sliced[2] == v3;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_ReadOnlySpan_Slice_EntireSpan(int first)
	{
		int[] array = { first, 2, 3, 4, 5 };
		var span = new ReadOnlySpan<int>(array);
		var sliced = span.Slice(0, span.Length);
		return sliced.Length == 5 && sliced[0] == first;
	}

	// == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_CopyTo_ValidDestination(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[5];
		ReadOnlySpan<int> sourceSpan = source;
		Span<int> destSpan = dest;
		sourceSpan.CopyTo(destSpan);
		return dest[0] == v0 && dest[1] == v1 && dest[2] == v2 && dest[3] == 0;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_CopyTo_ExactSize(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[3];
		ReadOnlySpan<int> sourceSpan = source;
		Span<int> destSpan = dest;
		sourceSpan.CopyTo(destSpan);
		return dest[0] == v0 && dest[1] == v1 && dest[2] == v2;
	}

	// == TryCopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_TryCopyTo_Success(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[5];
		ReadOnlySpan<int> sourceSpan = source;
		Span<int> destSpan = dest;
		var result = sourceSpan.TryCopyTo(destSpan);
		return result && dest[0] == v0 && dest[1] == v1 && dest[2] == v2;
	}

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_TryCopyTo_DestinationTooSmall()
	{
		int[] source = { 1, 2, 3, 4, 5 };
		var dest = new int[3];
		ReadOnlySpan<int> sourceSpan = source;
		Span<int> destSpan = dest;
		return !sourceSpan.TryCopyTo(destSpan);
	}

	// == ToArray tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_ReadOnlySpan_ToArray_ValidSpan(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		ReadOnlySpan<int> span = source;
		var result = span.ToArray();
		return result.Length == 3 && result[0] == v0 && result[1] == v1 && result[2] == v2;
	}

	[MosaUnitTest]
	public static int Test_ReadOnlySpan_ToArray_EmptySpan()
	{
		var span = ReadOnlySpan<int>.Empty;
		return span.ToArray().Length;
	}

	// == Equality tests

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_Equals_SameArray()
	{
		int[] array = { 1, 2, 3 };
		ReadOnlySpan<int> span1 = array;
		ReadOnlySpan<int> span2 = array;
		return span1 == span2;
	}

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_Equals_DifferentArrays()
	{
		int[] array1 = { 1, 2, 3 };
		int[] array2 = { 1, 2, 3 };
		ReadOnlySpan<int> span1 = array1;
		ReadOnlySpan<int> span2 = array2;
		return span1 != span2;
	}

	// == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_ReadOnlySpan_Enumerator_ForEach(int v0, int v1, int v2)
	{
		int[] array = { v0, v1, v2 };
		ReadOnlySpan<int> span = array;
		int sum = 0;
		foreach (int item in span)
			sum += item;
		return sum;
	}

	[MosaUnitTest]
	public static int Test_ReadOnlySpan_Enumerator_EmptySpan()
	{
		var span = ReadOnlySpan<int>.Empty;
		int count = 0;
		foreach (int item in span)
			count++;
		return count;
	}

	// == String tests

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_FromString_AsSpan()
	{
		var span = new ReadOnlySpan<char>("Hello".ToCharArray());
		return span.Length == 5 && span[0] == 'H' && span[4] == 'o';
	}

	[MosaUnitTest]
	public static bool Test_ReadOnlySpan_FromString_Slice()
	{
		var span = new ReadOnlySpan<char>("Hello World".ToCharArray());
		var sliced = span.Slice(0, 5);
		return sliced.Length == 5 && sliced[0] == 'H' && sliced[4] == 'o';
	}
}
