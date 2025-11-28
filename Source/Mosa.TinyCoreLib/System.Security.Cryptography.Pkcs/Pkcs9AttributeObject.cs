namespace System.Security.Cryptography.Pkcs;

public class Pkcs9AttributeObject : AsnEncodedData
{
	public new Oid? Oid
	{
		get
		{
			throw null;
		}
	}

	public Pkcs9AttributeObject()
	{
	}

	public Pkcs9AttributeObject(AsnEncodedData asnEncodedData)
	{
	}

	public Pkcs9AttributeObject(Oid oid, byte[] encodedData)
	{
	}

	public Pkcs9AttributeObject(string oid, byte[] encodedData)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
