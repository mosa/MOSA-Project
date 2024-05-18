namespace System.Security.Cryptography.X509Certificates;

public sealed class X509EnhancedKeyUsageExtension : X509Extension
{
	public OidCollection EnhancedKeyUsages
	{
		get
		{
			throw null;
		}
	}

	public X509EnhancedKeyUsageExtension()
	{
	}

	public X509EnhancedKeyUsageExtension(AsnEncodedData encodedEnhancedKeyUsages, bool critical)
	{
	}

	public X509EnhancedKeyUsageExtension(OidCollection enhancedKeyUsages, bool critical)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
