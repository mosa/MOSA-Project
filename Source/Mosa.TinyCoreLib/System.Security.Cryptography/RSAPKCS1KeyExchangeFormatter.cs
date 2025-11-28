namespace System.Security.Cryptography;

public class RSAPKCS1KeyExchangeFormatter : AsymmetricKeyExchangeFormatter
{
	public override string Parameters
	{
		get
		{
			throw null;
		}
	}

	public RandomNumberGenerator? Rng
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RSAPKCS1KeyExchangeFormatter()
	{
	}

	public RSAPKCS1KeyExchangeFormatter(AsymmetricAlgorithm key)
	{
	}

	public override byte[] CreateKeyExchange(byte[] rgbData)
	{
		throw null;
	}

	public override byte[] CreateKeyExchange(byte[] rgbData, Type? symAlgType)
	{
		throw null;
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
	}
}
