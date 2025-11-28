using System.Diagnostics.CodeAnalysis;

namespace System.Text;

public sealed class EncodingInfo
{
	public int CodePage
	{
		get
		{
			throw null;
		}
	}

	public string DisplayName
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

	public EncodingInfo(EncodingProvider provider, int codePage, string name, string displayName)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? value)
	{
		throw null;
	}

	public Encoding GetEncoding()
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}
}
