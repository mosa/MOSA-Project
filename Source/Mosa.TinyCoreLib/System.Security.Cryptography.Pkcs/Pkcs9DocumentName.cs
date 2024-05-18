namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs9DocumentName : Pkcs9AttributeObject
{
	public string DocumentName
	{
		get
		{
			throw null;
		}
	}

	public Pkcs9DocumentName()
	{
	}

	public Pkcs9DocumentName(byte[] encodedDocumentName)
	{
	}

	public Pkcs9DocumentName(string documentName)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
