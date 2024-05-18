namespace System.Security.Cryptography.Pkcs;

public sealed class Pkcs9LocalKeyId : Pkcs9AttributeObject
{
	public ReadOnlyMemory<byte> KeyId
	{
		get
		{
			throw null;
		}
	}

	public Pkcs9LocalKeyId()
	{
	}

	public Pkcs9LocalKeyId(byte[] keyId)
	{
	}

	public Pkcs9LocalKeyId(ReadOnlySpan<byte> keyId)
	{
	}
}
