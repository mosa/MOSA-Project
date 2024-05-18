namespace System.Security.Cryptography.X509Certificates;

public sealed class X500RelativeDistinguishedName
{
	public bool HasMultipleElements
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> RawData
	{
		get
		{
			throw null;
		}
	}

	internal X500RelativeDistinguishedName()
	{
	}

	public Oid GetSingleElementType()
	{
		throw null;
	}

	public string? GetSingleElementValue()
	{
		throw null;
	}
}
