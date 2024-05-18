namespace System.Security.Cryptography.X509Certificates;

public class X509Extension : AsnEncodedData
{
	public bool Critical
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	protected X509Extension()
	{
	}

	public X509Extension(AsnEncodedData encodedExtension, bool critical)
	{
	}

	public X509Extension(Oid oid, byte[] rawData, bool critical)
	{
	}

	public X509Extension(Oid oid, ReadOnlySpan<byte> rawData, bool critical)
	{
	}

	public X509Extension(string oid, byte[] rawData, bool critical)
	{
	}

	public X509Extension(string oid, ReadOnlySpan<byte> rawData, bool critical)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
