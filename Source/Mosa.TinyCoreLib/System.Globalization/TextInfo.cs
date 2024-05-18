using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace System.Globalization;

public sealed class TextInfo : ICloneable, IDeserializationCallback
{
	public int ANSICodePage
	{
		get
		{
			throw null;
		}
	}

	public string CultureName
	{
		get
		{
			throw null;
		}
	}

	public int EBCDICCodePage
	{
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		get
		{
			throw null;
		}
	}

	public bool IsRightToLeft
	{
		get
		{
			throw null;
		}
	}

	public int LCID
	{
		get
		{
			throw null;
		}
	}

	public string ListSeparator
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public int MacCodePage
	{
		get
		{
			throw null;
		}
	}

	public int OEMCodePage
	{
		get
		{
			throw null;
		}
	}

	internal TextInfo()
	{
	}

	public object Clone()
	{
		throw null;
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static TextInfo ReadOnly(TextInfo textInfo)
	{
		throw null;
	}

	void IDeserializationCallback.OnDeserialization(object? sender)
	{
	}

	public char ToLower(char c)
	{
		throw null;
	}

	public string ToLower(string str)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}

	public string ToTitleCase(string str)
	{
		throw null;
	}

	public char ToUpper(char c)
	{
		throw null;
	}

	public string ToUpper(string str)
	{
		throw null;
	}
}
