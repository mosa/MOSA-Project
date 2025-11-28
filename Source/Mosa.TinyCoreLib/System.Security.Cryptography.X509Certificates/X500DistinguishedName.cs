using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X500DistinguishedName : AsnEncodedData
{
	public string Name
	{
		get
		{
			throw null;
		}
	}

	public X500DistinguishedName(byte[] encodedDistinguishedName)
	{
	}

	public X500DistinguishedName(ReadOnlySpan<byte> encodedDistinguishedName)
	{
	}

	public X500DistinguishedName(AsnEncodedData encodedDistinguishedName)
	{
	}

	public X500DistinguishedName(X500DistinguishedName distinguishedName)
	{
	}

	public X500DistinguishedName(string distinguishedName)
	{
	}

	public X500DistinguishedName(string distinguishedName, X500DistinguishedNameFlags flag)
	{
	}

	public string Decode(X500DistinguishedNameFlags flag)
	{
		throw null;
	}

	public IEnumerable<X500RelativeDistinguishedName> EnumerateRelativeDistinguishedNames(bool reversed = true)
	{
		throw null;
	}

	public override string Format(bool multiLine)
	{
		throw null;
	}
}
