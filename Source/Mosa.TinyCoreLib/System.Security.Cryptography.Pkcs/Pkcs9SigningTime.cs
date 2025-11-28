namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs9SigningTime : Pkcs9AttributeObject
{
	public DateTime SigningTime
	{
		get
		{
			throw null;
		}
	}

	public Pkcs9SigningTime()
	{
	}

	public Pkcs9SigningTime(byte[] encodedSigningTime)
	{
	}

	public Pkcs9SigningTime(DateTime signingTime)
	{
	}

	public override void CopyFrom(AsnEncodedData asnEncodedData)
	{
	}
}
