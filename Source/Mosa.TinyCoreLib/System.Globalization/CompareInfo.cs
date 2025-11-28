using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace System.Globalization;

public sealed class CompareInfo : IDeserializationCallback
{
	public int LCID
	{
		get
		{
			throw null;
		}
	}

	public string Name
	{
		get
		{
			throw null;
		}
	}

	public SortVersion Version
	{
		get
		{
			throw null;
		}
	}

	internal CompareInfo()
	{
	}

	public int Compare(ReadOnlySpan<char> string1, ReadOnlySpan<char> string2, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public int Compare(string? string1, int offset1, int length1, string? string2, int offset2, int length2)
	{
		throw null;
	}

	public int Compare(string? string1, int offset1, int length1, string? string2, int offset2, int length2, CompareOptions options)
	{
		throw null;
	}

	public int Compare(string? string1, int offset1, string? string2, int offset2)
	{
		throw null;
	}

	public int Compare(string? string1, int offset1, string? string2, int offset2, CompareOptions options)
	{
		throw null;
	}

	public int Compare(string? string1, string? string2)
	{
		throw null;
	}

	public int Compare(string? string1, string? string2, CompareOptions options)
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public static CompareInfo GetCompareInfo(int culture)
	{
		throw null;
	}

	public static CompareInfo GetCompareInfo(int culture, Assembly assembly)
	{
		throw null;
	}

	public static CompareInfo GetCompareInfo(string name)
	{
		throw null;
	}

	public static CompareInfo GetCompareInfo(string name, Assembly assembly)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public int GetHashCode(ReadOnlySpan<char> source, CompareOptions options)
	{
		throw null;
	}

	public int GetHashCode(string source, CompareOptions options)
	{
		throw null;
	}

	public int GetSortKey(ReadOnlySpan<char> source, Span<byte> destination, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public SortKey GetSortKey(string source)
	{
		throw null;
	}

	public SortKey GetSortKey(string source, CompareOptions options)
	{
		throw null;
	}

	public int GetSortKeyLength(ReadOnlySpan<char> source, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public int IndexOf(ReadOnlySpan<char> source, ReadOnlySpan<char> value, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public int IndexOf(ReadOnlySpan<char> source, ReadOnlySpan<char> value, CompareOptions options, out int matchLength)
	{
		throw null;
	}

	public int IndexOf(ReadOnlySpan<char> source, Rune value, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public int IndexOf(string source, char value)
	{
		throw null;
	}

	public int IndexOf(string source, char value, CompareOptions options)
	{
		throw null;
	}

	public int IndexOf(string source, char value, int startIndex)
	{
		throw null;
	}

	public int IndexOf(string source, char value, int startIndex, CompareOptions options)
	{
		throw null;
	}

	public int IndexOf(string source, char value, int startIndex, int count)
	{
		throw null;
	}

	public int IndexOf(string source, char value, int startIndex, int count, CompareOptions options)
	{
		throw null;
	}

	public int IndexOf(string source, string value)
	{
		throw null;
	}

	public int IndexOf(string source, string value, CompareOptions options)
	{
		throw null;
	}

	public int IndexOf(string source, string value, int startIndex)
	{
		throw null;
	}

	public int IndexOf(string source, string value, int startIndex, CompareOptions options)
	{
		throw null;
	}

	public int IndexOf(string source, string value, int startIndex, int count)
	{
		throw null;
	}

	public int IndexOf(string source, string value, int startIndex, int count, CompareOptions options)
	{
		throw null;
	}

	public bool IsPrefix(ReadOnlySpan<char> source, ReadOnlySpan<char> prefix, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public bool IsPrefix(ReadOnlySpan<char> source, ReadOnlySpan<char> prefix, CompareOptions options, out int matchLength)
	{
		throw null;
	}

	public bool IsPrefix(string source, string prefix)
	{
		throw null;
	}

	public bool IsPrefix(string source, string prefix, CompareOptions options)
	{
		throw null;
	}

	public static bool IsSortable(char ch)
	{
		throw null;
	}

	public static bool IsSortable(ReadOnlySpan<char> text)
	{
		throw null;
	}

	public static bool IsSortable(string text)
	{
		throw null;
	}

	public static bool IsSortable(Rune value)
	{
		throw null;
	}

	public bool IsSuffix(ReadOnlySpan<char> source, ReadOnlySpan<char> suffix, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public bool IsSuffix(ReadOnlySpan<char> source, ReadOnlySpan<char> suffix, CompareOptions options, out int matchLength)
	{
		throw null;
	}

	public bool IsSuffix(string source, string suffix)
	{
		throw null;
	}

	public bool IsSuffix(string source, string suffix, CompareOptions options)
	{
		throw null;
	}

	public int LastIndexOf(ReadOnlySpan<char> source, ReadOnlySpan<char> value, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public int LastIndexOf(ReadOnlySpan<char> source, ReadOnlySpan<char> value, CompareOptions options, out int matchLength)
	{
		throw null;
	}

	public int LastIndexOf(ReadOnlySpan<char> source, Rune value, CompareOptions options = CompareOptions.None)
	{
		throw null;
	}

	public int LastIndexOf(string source, char value)
	{
		throw null;
	}

	public int LastIndexOf(string source, char value, CompareOptions options)
	{
		throw null;
	}

	public int LastIndexOf(string source, char value, int startIndex)
	{
		throw null;
	}

	public int LastIndexOf(string source, char value, int startIndex, CompareOptions options)
	{
		throw null;
	}

	public int LastIndexOf(string source, char value, int startIndex, int count)
	{
		throw null;
	}

	public int LastIndexOf(string source, char value, int startIndex, int count, CompareOptions options)
	{
		throw null;
	}

	public int LastIndexOf(string source, string value)
	{
		throw null;
	}

	public int LastIndexOf(string source, string value, CompareOptions options)
	{
		throw null;
	}

	public int LastIndexOf(string source, string value, int startIndex)
	{
		throw null;
	}

	public int LastIndexOf(string source, string value, int startIndex, CompareOptions options)
	{
		throw null;
	}

	public int LastIndexOf(string source, string value, int startIndex, int count)
	{
		throw null;
	}

	public int LastIndexOf(string source, string value, int startIndex, int count, CompareOptions options)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	public override string ToString()
	{
		throw null;
	}
}
