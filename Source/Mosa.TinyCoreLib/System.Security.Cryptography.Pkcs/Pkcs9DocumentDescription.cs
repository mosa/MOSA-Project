namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs9DocumentDescription : Pkcs9AttributeObject
{
	public string DocumentDescription
	{
		get
		{
			throw null;
		}
	}

	public Pkcs9DocumentDescription()
	{
	}

	public Pkcs9DocumentDescription(byte[] encodedDocumentDescription)
	{
	}

	public Pkcs9DocumentDescription(string documentDescription)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
