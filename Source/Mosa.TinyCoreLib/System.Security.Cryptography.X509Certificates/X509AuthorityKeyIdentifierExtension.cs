namespace System.Security.Cryptography.X509Certificates;

public sealed class X509AuthorityKeyIdentifierExtension : X509Extension
{
	public ReadOnlyMemory<byte>? KeyIdentifier
	{
		get
		{
			throw null;
		}
	}

	public X500DistinguishedName? NamedIssuer
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte>? RawIssuer
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte>? SerialNumber
	{
		get
		{
			throw null;
		}
	}

	public X509AuthorityKeyIdentifierExtension()
	{
	}

	public X509AuthorityKeyIdentifierExtension(byte[] rawData, bool critical = false)
	{
	}

	public X509AuthorityKeyIdentifierExtension(ReadOnlySpan<byte> rawData, bool critical = false)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}

	public static X509AuthorityKeyIdentifierExtension Create(byte[] keyIdentifier, X500DistinguishedName issuerName, byte[] serialNumber)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension Create(ReadOnlySpan<byte> keyIdentifier, X500DistinguishedName issuerName, ReadOnlySpan<byte> serialNumber)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension CreateFromCertificate(X509Certificate2 certificate, bool includeKeyIdentifier, bool includeIssuerAndSerial)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension CreateFromIssuerNameAndSerialNumber(X500DistinguishedName issuerName, byte[] serialNumber)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension CreateFromIssuerNameAndSerialNumber(X500DistinguishedName issuerName, ReadOnlySpan<byte> serialNumber)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension CreateFromSubjectKeyIdentifier(byte[] subjectKeyIdentifier)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension CreateFromSubjectKeyIdentifier(ReadOnlySpan<byte> subjectKeyIdentifier)
	{
		throw null;
	}

	public static X509AuthorityKeyIdentifierExtension CreateFromSubjectKeyIdentifier(X509SubjectKeyIdentifierExtension subjectKeyIdentifier)
	{
		throw null;
	}
}
