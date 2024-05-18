namespace System.Security.Cryptography.X509Certificates;

public sealed class X509KeyUsageExtension : X509Extension
{
	public X509KeyUsageFlags KeyUsages
	{
		get
		{
			throw null;
		}
	}

	public X509KeyUsageExtension()
	{
	}

	public X509KeyUsageExtension(AsnEncodedData encodedKeyUsage, bool critical)
	{
	}

	public X509KeyUsageExtension(X509KeyUsageFlags keyUsages, bool critical)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
