using System.Diagnostics.CodeAnalysis;

namespace System.Globalization;

public class StringInfo
{
	public int LengthInTextElements
	{
		get
		{
			throw null;
		}
	}

	public string String
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public StringInfo()
	{
	}

	public StringInfo(string value)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static string GetNextTextElement(string str)
	{
		throw null;
	}

	public static string GetNextTextElement(string str, int index)
	{
		throw null;
	}

	public static int GetNextTextElementLength(ReadOnlySpan<char> str)
	{
		throw null;
	}

	public static int GetNextTextElementLength(string str)
	{
		throw null;
	}

	public static int GetNextTextElementLength(string str, int index)
	{
		throw null;
	}

	public static TextElementEnumerator GetTextElementEnumerator(string str)
	{
		throw null;
	}

	public static TextElementEnumerator GetTextElementEnumerator(string str, int index)
	{
		throw null;
	}

	public static int[] ParseCombiningCharacters(string str)
	{
		throw null;
	}

	public string SubstringByTextElements(int startingTextElement)
	{
		throw null;
	}

	public string SubstringByTextElements(int startingTextElement, int lengthInTextElements)
	{
		throw null;
	}
}
