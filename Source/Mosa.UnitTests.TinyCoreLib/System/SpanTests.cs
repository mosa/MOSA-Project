// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.TinyCoreLib;

public static class SpanTests
{
	// == Constructor tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Span_Constructor_FromArray(int first)
	{
		int[] array = { first, 2, 3, 4, 5 };
		Span<int> span = array;
		return span.Length == 5 && span[0] == first;
	}

	[MosaUnitTest]
	public static int Test_Span_Constructor_Explicit()
	{
		int[] array = { 1, 2, 3, 4, 5 };
		var span = new Span<int>(array);
		return span.Length;
	}

	[MosaUnitTest]
	public static bool Test_Span_Constructor_Empty()
	{
		var span = Span<int>.Empty;
		return span.Length == 0 && span.IsEmpty;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_Constructor_WithStartAndLength(int v1, int v2, int v3)
	{
		int[] array = { 0, v1, v2, v3, 0 };
		var span = new Span<int>(array, 1, 3);
		return span.Length == 3 && span[0] == v1 && span[1] == v2 && span[2] == v3;
	}

	// == Length tests

	[MosaUnitTest]
	public static int Test_Span_Length_FromArray()
	{
		int[] array = { 1, 2, 3, 4, 5 };
		Span<int> span = array;
		return span.Length;
	}

	[MosaUnitTest]
	public static int Test_Span_Length_Empty()
	{
		var span = Span<int>.Empty;
		return span.Length;
	}

	[MosaUnitTest]
	public static int Test_Span_Length_Sliced()
	{
		int[] array = { 1, 2, 3, 4, 5 };
		var span = new Span<int>(array).Slice(1, 3);
		return span.Length;
	}

	// == IsEmpty tests

	[MosaUnitTest]
	public static bool Test_Span_IsEmpty_EmptySpan()
	{
		var span = Span<int>.Empty;
		return span.IsEmpty;
	}

	[MosaUnitTest]
	public static bool Test_Span_IsEmpty_NonEmptySpan()
	{
		int[] array = { 1, 2, 3 };
		Span<int> span = array;
		return !span.IsEmpty;
	}

	// == Indexer tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_Indexer_Get(int v0, int v2, int v4)
	{
		int[] array = { v0, 0, v2, 0, v4 };
		Span<int> span = array;
		return span[0] == v0 && span[2] == v2 && span[4] == v4;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Span_Indexer_Set(int value)
	{
		int[] array = { 1, 2, 3, 4, 5 };
		Span<int> span = array;
		span[2] = value;
		return span[2] == value && array[2] == value;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static bool Test_Span_Indexer_FirstLast(int first, int last)
	{
		int[] array = { first, 0, 0, 0, last };
		Span<int> span = array;
		return span[0] == first && span[^1] == last;
	}

	// == Slice tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Span_Slice_WithStart(int v2)
	{
		int[] array = { 1, 2, v2, 4, 5 };
		var sliced = new Span<int>(array).Slice(2);
		return sliced.Length == 3 && sliced[0] == v2;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_Slice_WithStartAndLength(int v1, int v2, int v3)
	{
		int[] array = { 0, v1, v2, v3, 0 };
		var sliced = new Span<int>(array).Slice(1, 3);
		return sliced.Length == 3 && sliced[0] == v1 && sliced[2] == v3;
	}

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Span_Slice_EntireSpan(int first)
	{
		int[] array = { first, 2, 3, 4, 5 };
		var span = new Span<int>(array);
		var sliced = span.Slice(0, span.Length);
		return sliced.Length == 5 && sliced[0] == first;
	}

	[MosaUnitTest(Series = "I4I4")]
	public static bool Test_Span_Slice_ToEnd(int v3, int v4)
	{
		int[] array = { 0, 0, 0, v3, v4 };
		var sliced = new Span<int>(array).Slice(3);
		return sliced.Length == 2 && sliced[0] == v3 && sliced[1] == v4;
	}

	// == Fill tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Span_Fill_EntireSpan(int value)
	{
		var array = new int[5];
		Span<int> span = array;
		span.Fill(value);
		return array[0] == value && array[2] == value && array[4] == value;
	}

	[MosaUnitTest]
	public static bool Test_Span_Fill_EmptySpan()
	{
		var span = Span<int>.Empty;
		span.Fill(99);
		return true;
	}

	// == Clear tests

	[MosaUnitTest]
	public static bool Test_Span_Clear_EntireSpan()
	{
		int[] array = { 1, 2, 3, 4, 5 };
		Span<int> span = array;
		span.Clear();
		return array[0] == 0 && array[2] == 0 && array[4] == 0;
	}

	[MosaUnitTest]
	public static bool Test_Span_Clear_ValueTypes()
	{
		double[] array = { 1.5, 2.5, 3.5 };
		Span<double> span = array;
		span.Clear();
		return array[0] == 0.0 && array[1] == 0.0 && array[2] == 0.0;
	}

	// == CopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_CopyTo_ValidDestination(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[5];
		Span<int> sourceSpan = source;
		Span<int> destSpan = dest;
		sourceSpan.CopyTo(destSpan);
		return dest[0] == v0 && dest[1] == v1 && dest[2] == v2 && dest[3] == 0;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_CopyTo_ExactSize(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[3];
		Span<int> sourceSpan = source;
		Span<int> destSpan = dest;
		sourceSpan.CopyTo(destSpan);
		return dest[0] == v0 && dest[1] == v1 && dest[2] == v2;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_CopyTo_LargerDestination(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[10];
		Span<int> sourceSpan = source;
		Span<int> destSpan = dest;
		sourceSpan.CopyTo(destSpan);
		return dest[0] == v0 && dest[2] == v2 && dest[9] == 0;
	}

	// == TryCopyTo tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_TryCopyTo_Success(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		var dest = new int[5];
		Span<int> sourceSpan = source;
		Span<int> destSpan = dest;
		var result = sourceSpan.TryCopyTo(destSpan);
		return result && dest[0] == v0 && dest[1] == v1 && dest[2] == v2;
	}

	[MosaUnitTest]
	public static bool Test_Span_TryCopyTo_DestinationTooSmall()
	{
		int[] source = { 1, 2, 3, 4, 5 };
		var dest = new int[3];
		Span<int> sourceSpan = source;
		Span<int> destSpan = dest;
		return !sourceSpan.TryCopyTo(destSpan);
	}

	[MosaUnitTest]
	public static bool Test_Span_TryCopyTo_EmptySpan()
	{
		var sourceSpan = Span<int>.Empty;
		var dest = new int[5];
		Span<int> destSpan = dest;
		return sourceSpan.TryCopyTo(destSpan);
	}

	// == ToArray tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_ToArray_ValidSpan(int v0, int v1, int v2)
	{
		int[] source = { v0, v1, v2 };
		Span<int> span = source;
		var result = span.ToArray();
		return result.Length == 3 && result[0] == v0 && result[1] == v1 && result[2] == v2;
	}

	[MosaUnitTest]
	public static int Test_Span_ToArray_EmptySpan()
	{
		var span = Span<int>.Empty;
		return span.ToArray().Length;
	}

	[MosaUnitTest(Series = "I4I4I4")]
	public static bool Test_Span_ToArray_SlicedSpan(int v1, int v2, int v3)
	{
		int[] source = { 0, v1, v2, v3, 0 };
		var span = new Span<int>(source, 1, 3);
		var result = span.ToArray();
		return result.Length == 3 && result[0] == v1 && result[1] == v2 && result[2] == v3;
	}

	// == Equality tests

	[MosaUnitTest]
	public static bool Test_Span_Equals_SameArray()
	{
		int[] array = { 1, 2, 3 };
		Span<int> span1 = array;
		Span<int> span2 = array;
		return span1 == span2;
	}

	[MosaUnitTest]
	public static bool Test_Span_Equals_DifferentArrays()
	{
		int[] array1 = { 1, 2, 3 };
		int[] array2 = { 1, 2, 3 };
		Span<int> span1 = array1;
		Span<int> span2 = array2;
		return span1 != span2;
	}

	// == Enumerator tests

	[MosaUnitTest(Series = "I4I4I4")]
	public static int Test_Span_Enumerator_ForEach(int v0, int v1, int v2)
	{
		int[] array = { v0, v1, v2 };
		Span<int> span = array;
		int sum = 0;
		foreach (int item in span)
			sum += item;
		return sum;
	}

	[MosaUnitTest]
	public static int Test_Span_Enumerator_EmptySpan()
	{
		var span = Span<int>.Empty;
		int count = 0;
		foreach (int item in span)
			count++;
		return count;
	}

	// == Conversion tests

	[MosaUnitTest(Series = "I4")]
	public static bool Test_Span_ImplicitConversion_ToReadOnlySpan(int first)
	{
		int[] array = { first, 2, 3, 4, 5 };
		Span<int> span = array;
		ReadOnlySpan<int> readOnlySpan = span;
		return readOnlySpan.Length == 5 && readOnlySpan[0] == first;
	}

	// == Struct tests

	[MosaUnitTest(Series = "I4I4")]
	public static bool Test_Span_WithStructs_Operations(int v0, int newV1)
	{
		TestStruct[] array =
		[
			new() { Value = v0 },
			new() { Value = 0 },
			new() { Value = 0 }
		];
		Span<TestStruct> span = array;
		var a1 = span[0].Value == v0;
		span[1] = new() { Value = newV1 };
		var a2 = array[1].Value == newV1;
		var sliced = span.Slice(1, 2);
		var a3 = sliced[0].Value == newV1;
		return a1 && a2 && a3;
	}

	private struct TestStruct
	{
		public int Value { get; set; }
	}
}
