using System.Collections.Generic;
using System.Net;

namespace System.Security.Cryptography.X509Certificates;

public sealed class X509SubjectAlternativeNameExtension : X509Extension
{
	public X509SubjectAlternativeNameExtension()
	{
	}

	public X509SubjectAlternativeNameExtension(byte[] rawData, bool critical = false)
	{
	}

	public X509SubjectAlternativeNameExtension(ReadOnlySpan<byte> rawData, bool critical = false)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}

	public IEnumerable<string> EnumerateDnsNames()
	{
		throw null;
	}

	public IEnumerable<IPAddress> EnumerateIPAddresses()
	{
		throw null;
	}
}
