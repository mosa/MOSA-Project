using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace System;

public static class MemoryExtensions
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[InterpolatedStringHandler]
	public ref struct TryWriteInterpolatedStringHandler
	{
		private readonly object _dummy;

		private readonly int _dummyPrimitive;

		public TryWriteInterpolatedStringHandler(int literalLength, int formattedCount, Span<char> destination, out bool shouldAppend)
		{
			throw null;
		}

		public TryWriteInterpolatedStringHandler(int literalLength, int formattedCount, Span<char> destination, IFormatProvider? provider, out bool shouldAppend)
		{
			throw null;
		}

		public bool AppendLiteral(string value)
		{
			throw null;
		}

		public bool AppendFormatted(scoped ReadOnlySpan<char> value)
		{
			throw null;
		}

		public bool AppendFormatted(scoped ReadOnlySpan<char> value, int alignment = 0, string? format = null)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value, string? format)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value, int alignment)
		{
			throw null;
		}

		public bool AppendFormatted<T>(T value, int alignment, string? format)
		{
			throw null;
		}

		public bool AppendFormatted(object? value, int alignment = 0, string? format = null)
		{
			throw null;
		}

		public bool AppendFormatted(string? value)
		{
			throw null;
		}

		public bool AppendFormatted(string? value, int alignment = 0, string? format = null)
		{
			throw null;
		}
	}

	public static ReadOnlyMemory<char> AsMemory(this string? text)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> AsMemory(this string? text, Index startIndex)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> AsMemory(this string? text, int start)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> AsMemory(this string? text, int start, int length)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> AsMemory(this string? text, Range range)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this ArraySegment<T> segment)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this ArraySegment<T> segment, int start, int length)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this T[]? array)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this T[]? array, Index startIndex)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this T[]? array, int start)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this T[]? array, int start, int length)
	{
		throw null;
	}

	public static Memory<T> AsMemory<T>(this T[]? array, Range range)
	{
		throw null;
	}

	public static ReadOnlySpan<char> AsSpan(this string? text)
	{
		throw null;
	}

	public static ReadOnlySpan<char> AsSpan(this string? text, int start)
	{
		throw null;
	}

	public static ReadOnlySpan<char> AsSpan(this string? text, int start, int length)
	{
		throw null;
	}

	public static ReadOnlySpan<char> AsSpan(this string? text, Index startIndex)
	{
		throw null;
	}

	public static ReadOnlySpan<char> AsSpan(this string? text, Range range)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this ArraySegment<T> segment)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this ArraySegment<T> segment, Index startIndex)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this ArraySegment<T> segment, int start, int length)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this ArraySegment<T> segment, Range range)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this T[]? array)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this T[]? array, Index startIndex)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this T[]? array, int start)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this T[]? array, int start, int length)
	{
		throw null;
	}

	public static Span<T> AsSpan<T>(this T[]? array, Range range)
	{
		throw null;
	}

	public static int BinarySearch<T>(this ReadOnlySpan<T> span, IComparable<T> comparable)
	{
		throw null;
	}

	public static int BinarySearch<T>(this Span<T> span, IComparable<T> comparable)
	{
		throw null;
	}

	public static int BinarySearch<T, TComparer>(this ReadOnlySpan<T> span, T value, TComparer comparer) where TComparer : IComparer<T>
	{
		throw null;
	}

	public static int BinarySearch<T, TComparable>(this ReadOnlySpan<T> span, TComparable comparable) where TComparable : IComparable<T>
	{
		throw null;
	}

	public static int BinarySearch<T, TComparer>(this Span<T> span, T value, TComparer comparer) where TComparer : IComparer<T>
	{
		throw null;
	}

	public static int BinarySearch<T, TComparable>(this Span<T> span, TComparable comparable) where TComparable : IComparable<T>
	{
		throw null;
	}

	public static int CommonPrefixLength<T>(this Span<T> span, ReadOnlySpan<T> other)
	{
		throw null;
	}

	public static int CommonPrefixLength<T>(this Span<T> span, ReadOnlySpan<T> other, IEqualityComparer<T>? comparer)
	{
		throw null;
	}

	public static int CommonPrefixLength<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
	{
		throw null;
	}

	public static int CommonPrefixLength<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, IEqualityComparer<T>? comparer)
	{
		throw null;
	}

	public static int CompareTo(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType)
	{
		throw null;
	}

	public static bool Contains(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
	{
		throw null;
	}

	public static bool Contains<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool Contains<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this ReadOnlySpan<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this Span<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAny<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this ReadOnlySpan<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this Span<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExcept<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool ContainsAnyExceptInRange<T>(this ReadOnlySpan<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static bool ContainsAnyExceptInRange<T>(this Span<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static bool ContainsAnyInRange<T>(this ReadOnlySpan<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static bool ContainsAnyInRange<T>(this Span<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static void CopyTo<T>(this T[]? source, Memory<T> destination)
	{
	}

	public static void CopyTo<T>(this T[]? source, Span<T> destination)
	{
	}

	public static int Count<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int Count<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int Count<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int Count<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool EndsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
	{
		throw null;
	}

	public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static SpanLineEnumerator EnumerateLines(this ReadOnlySpan<char> span)
	{
		throw null;
	}

	public static SpanLineEnumerator EnumerateLines(this Span<char> span)
	{
		throw null;
	}

	public static SpanRuneEnumerator EnumerateRunes(this ReadOnlySpan<char> span)
	{
		throw null;
	}

	public static SpanRuneEnumerator EnumerateRunes(this Span<char> span)
	{
		throw null;
	}

	public static bool Equals(this ReadOnlySpan<char> span, ReadOnlySpan<char> other, StringComparison comparisonType)
	{
		throw null;
	}

	public static int IndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
	{
		throw null;
	}

	public static int IndexOfAny<T>(this ReadOnlySpan<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this Span<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this Span<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this ReadOnlySpan<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExcept<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyExceptInRange<T>(this ReadOnlySpan<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static int IndexOfAnyExceptInRange<T>(this Span<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static int IndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOf<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOf<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOf<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int IndexOfAnyInRange<T>(this ReadOnlySpan<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static int IndexOfAnyInRange<T>(this Span<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static bool IsWhiteSpace(this ReadOnlySpan<char> span)
	{
		throw null;
	}

	public static int LastIndexOf(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this Span<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this Span<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this Span<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this Span<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this Span<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this ReadOnlySpan<T> span, T value0, T value1) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this ReadOnlySpan<T> span, SearchValues<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExcept<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyExceptInRange<T>(this ReadOnlySpan<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static int LastIndexOfAnyExceptInRange<T>(this Span<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static int LastIndexOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOf<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T?>
	{
		throw null;
	}

	public static int LastIndexOf<T>(this Span<T> span, T value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int LastIndexOfAnyInRange<T>(this ReadOnlySpan<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static int LastIndexOfAnyInRange<T>(this Span<T> span, T lowInclusive, T highInclusive) where T : IComparable<T>
	{
		throw null;
	}

	public static bool Overlaps<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
	{
		throw null;
	}

	public static bool Overlaps<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, out int elementOffset)
	{
		throw null;
	}

	public static bool Overlaps<T>(this Span<T> span, ReadOnlySpan<T> other)
	{
		throw null;
	}

	public static bool Overlaps<T>(this Span<T> span, ReadOnlySpan<T> other, out int elementOffset)
	{
		throw null;
	}

	public static void Replace<T>(this Span<T> span, T oldValue, T newValue) where T : IEquatable<T>?
	{
	}

	public static void Replace<T>(this ReadOnlySpan<T> source, Span<T> destination, T oldValue, T newValue) where T : IEquatable<T>?
	{
	}

	public static void Reverse<T>(this Span<T> span)
	{
	}

	public static int SequenceCompareTo<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) where T : IComparable<T>?
	{
		throw null;
	}

	public static int SequenceCompareTo<T>(this Span<T> span, ReadOnlySpan<T> other) where T : IComparable<T>?
	{
		throw null;
	}

	public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, IEqualityComparer<T>? comparer = null)
	{
		throw null;
	}

	public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other, IEqualityComparer<T>? comparer = null)
	{
		throw null;
	}

	public static void Sort<T>(this Span<T> span)
	{
	}

	public static void Sort<T>(this Span<T> span, Comparison<T> comparison)
	{
	}

	public static void Sort<TKey, TValue>(this Span<TKey> keys, Span<TValue> items)
	{
	}

	public static void Sort<TKey, TValue>(this Span<TKey> keys, Span<TValue> items, Comparison<TKey> comparison)
	{
	}

	public static void Sort<T, TComparer>(this Span<T> span, TComparer comparer) where TComparer : IComparer<T>?
	{
	}

	public static void Sort<TKey, TValue, TComparer>(this Span<TKey> keys, Span<TValue> items, TComparer comparer) where TComparer : IComparer<TKey>?
	{
	}

	public static int Split(this ReadOnlySpan<char> source, Span<Range> destination, char separator, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public static int Split(this ReadOnlySpan<char> source, Span<Range> destination, ReadOnlySpan<char> separator, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public static int SplitAny(this ReadOnlySpan<char> source, Span<Range> destination, ReadOnlySpan<char> separators, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public static int SplitAny(this ReadOnlySpan<char> source, Span<Range> destination, ReadOnlySpan<string> separators, StringSplitOptions options = StringSplitOptions.None)
	{
		throw null;
	}

	public static bool StartsWith(this ReadOnlySpan<char> span, ReadOnlySpan<char> value, StringComparison comparisonType)
	{
		throw null;
	}

	public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> value) where T : IEquatable<T>?
	{
		throw null;
	}

	public static int ToLower(this ReadOnlySpan<char> source, Span<char> destination, CultureInfo? culture)
	{
		throw null;
	}

	public static int ToLowerInvariant(this ReadOnlySpan<char> source, Span<char> destination)
	{
		throw null;
	}

	public static int ToUpper(this ReadOnlySpan<char> source, Span<char> destination, CultureInfo? culture)
	{
		throw null;
	}

	public static int ToUpperInvariant(this ReadOnlySpan<char> source, Span<char> destination)
	{
		throw null;
	}

	public static Memory<char> Trim(this Memory<char> memory)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> Trim(this ReadOnlyMemory<char> memory)
	{
		throw null;
	}

	public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span)
	{
		throw null;
	}

	public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, char trimChar)
	{
		throw null;
	}

	public static ReadOnlySpan<char> Trim(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
	{
		throw null;
	}

	public static Span<char> Trim(this Span<char> span)
	{
		throw null;
	}

	public static Memory<char> TrimEnd(this Memory<char> memory)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> TrimEnd(this ReadOnlyMemory<char> memory)
	{
		throw null;
	}

	public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span)
	{
		throw null;
	}

	public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, char trimChar)
	{
		throw null;
	}

	public static ReadOnlySpan<char> TrimEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
	{
		throw null;
	}

	public static Span<char> TrimEnd(this Span<char> span)
	{
		throw null;
	}

	public static Memory<T> TrimEnd<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Memory<T> TrimEnd<T>(this Memory<T> memory, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlyMemory<T> TrimEnd<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlyMemory<T> TrimEnd<T>(this ReadOnlyMemory<T> memory, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlySpan<T> TrimEnd<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlySpan<T> TrimEnd<T>(this ReadOnlySpan<T> span, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Span<T> TrimEnd<T>(this Span<T> span, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Span<T> TrimEnd<T>(this Span<T> span, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Memory<char> TrimStart(this Memory<char> memory)
	{
		throw null;
	}

	public static ReadOnlyMemory<char> TrimStart(this ReadOnlyMemory<char> memory)
	{
		throw null;
	}

	public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span)
	{
		throw null;
	}

	public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, char trimChar)
	{
		throw null;
	}

	public static ReadOnlySpan<char> TrimStart(this ReadOnlySpan<char> span, ReadOnlySpan<char> trimChars)
	{
		throw null;
	}

	public static Span<char> TrimStart(this Span<char> span)
	{
		throw null;
	}

	public static Memory<T> TrimStart<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Memory<T> TrimStart<T>(this Memory<T> memory, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlyMemory<T> TrimStart<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlyMemory<T> TrimStart<T>(this ReadOnlyMemory<T> memory, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlySpan<T> TrimStart<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlySpan<T> TrimStart<T>(this ReadOnlySpan<T> span, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Span<T> TrimStart<T>(this Span<T> span, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Span<T> TrimStart<T>(this Span<T> span, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Memory<T> Trim<T>(this Memory<T> memory, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Memory<T> Trim<T>(this Memory<T> memory, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlyMemory<T> Trim<T>(this ReadOnlyMemory<T> memory, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlyMemory<T> Trim<T>(this ReadOnlyMemory<T> memory, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static ReadOnlySpan<T> Trim<T>(this ReadOnlySpan<T> span, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Span<T> Trim<T>(this Span<T> span, ReadOnlySpan<T> trimElements) where T : IEquatable<T>?
	{
		throw null;
	}

	public static Span<T> Trim<T>(this Span<T> span, T trimElement) where T : IEquatable<T>?
	{
		throw null;
	}

	public static bool TryWrite(this Span<char> destination, [InterpolatedStringHandlerArgument("destination")] ref TryWriteInterpolatedStringHandler handler, out int charsWritten)
	{
		throw null;
	}

	public static bool TryWrite(this Span<char> destination, IFormatProvider? provider, [InterpolatedStringHandlerArgument(new string[] { "destination", "provider" })] ref TryWriteInterpolatedStringHandler handler, out int charsWritten)
	{
		throw null;
	}

	public static bool TryWrite<TArg0>(this Span<char> destination, IFormatProvider? provider, CompositeFormat format, out int charsWritten, TArg0 arg0)
	{
		throw null;
	}

	public static bool TryWrite<TArg0, TArg1>(this Span<char> destination, IFormatProvider? provider, CompositeFormat format, out int charsWritten, TArg0 arg0, TArg1 arg1)
	{
		throw null;
	}

	public static bool TryWrite<TArg0, TArg1, TArg2>(this Span<char> destination, IFormatProvider? provider, CompositeFormat format, out int charsWritten, TArg0 arg0, TArg1 arg1, TArg2 arg2)
	{
		throw null;
	}

	public static bool TryWrite(this Span<char> destination, IFormatProvider? provider, CompositeFormat format, out int charsWritten, params object?[] args)
	{
		throw null;
	}

	public static bool TryWrite(this Span<char> destination, IFormatProvider? provider, CompositeFormat format, out int charsWritten, ReadOnlySpan<object?> args)
	{
		throw null;
	}
}
