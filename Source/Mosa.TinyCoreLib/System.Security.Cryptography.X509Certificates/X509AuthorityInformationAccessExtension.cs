using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509AuthorityInformationAccessExtension : X509Extension
{
	public X509AuthorityInformationAccessExtension()
	{
	}

	public X509AuthorityInformationAccessExtension(byte[] rawData, bool critical = false)
	{
	}

	public X509AuthorityInformationAccessExtension(IEnumerable<string>? ocspUris, IEnumerable<string>? caIssuersUris, bool critical = false)
	{
	}

	public X509AuthorityInformationAccessExtension(ReadOnlySpan<byte> rawData, bool critical = false)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}

	public IEnumerable<string> EnumerateCAIssuersUris()
	{
		throw null;
	}

	public IEnumerable<string> EnumerateOcspUris()
	{
		throw null;
	}

	public IEnumerable<string> EnumerateUris(Oid accessMethodOid)
	{
		throw null;
	}

	public IEnumerable<string> EnumerateUris(string accessMethodOid)
	{
		throw null;
	}
}
