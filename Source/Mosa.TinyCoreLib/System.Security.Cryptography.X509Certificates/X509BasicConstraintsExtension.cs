namespace System.Security.Cryptography.X509Certificates;

public sealed class X509BasicConstraintsExtension : X509Extension
{
	public bool CertificateAuthority
	{
		get
		{
			throw null;
		}
	}

	public bool HasPathLengthConstraint
	{
		get
		{
			throw null;
		}
	}

	public int PathLengthConstraint
	{
		get
		{
			throw null;
		}
	}

	public X509BasicConstraintsExtension()
	{
	}

	public X509BasicConstraintsExtension(bool certificateAuthority, bool hasPathLengthConstraint, int pathLengthConstraint, bool critical)
	{
	}

	public X509BasicConstraintsExtension(AsnEncodedData encodedBasicConstraints, bool critical)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}

	public static X509BasicConstraintsExtension CreateForCertificateAuthority(int? pathLengthConstraint = null)
	{
		throw null;
	}

	public static X509BasicConstraintsExtension CreateForEndEntity(bool critical = false)
	{
		throw null;
	}
}
