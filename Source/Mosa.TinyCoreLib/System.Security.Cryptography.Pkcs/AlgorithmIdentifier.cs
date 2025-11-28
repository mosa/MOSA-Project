namespace System.Security.Cryptography.Pkcs;

public sealed class AlgorithmIdentifier
{
	public int KeyLength
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public Oid Oid
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] Parameters
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public AlgorithmIdentifier()
	{
	}

	public AlgorithmIdentifier(Oid oid)
	{
	}

	public AlgorithmIdentifier(Oid oid, int keyLength)
	{
	}
}
