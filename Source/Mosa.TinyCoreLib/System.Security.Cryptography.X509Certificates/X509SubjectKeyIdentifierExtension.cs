namespace System.Security.Cryptography.X509Certificates;

public sealed class X509SubjectKeyIdentifierExtension : X509Extension
{
	public string? SubjectKeyIdentifier
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> SubjectKeyIdentifierBytes
	{
		get
		{
			throw null;
		}
	}

	public X509SubjectKeyIdentifierExtension()
	{
	}

	public X509SubjectKeyIdentifierExtension(byte[] subjectKeyIdentifier, bool critical)
	{
	}

	public X509SubjectKeyIdentifierExtension(ReadOnlySpan<byte> subjectKeyIdentifier, bool critical)
	{
	}

	public X509SubjectKeyIdentifierExtension(AsnEncodedData encodedSubjectKeyIdentifier, bool critical)
	{
	}

	public X509SubjectKeyIdentifierExtension(PublicKey key, bool critical)
	{
	}

	public X509SubjectKeyIdentifierExtension(PublicKey key, X509SubjectKeyIdentifierHashAlgorithm algorithm, bool critical)
	{
	}

	public X509SubjectKeyIdentifierExtension(string subjectKeyIdentifier, bool critical)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
