namespace System.Security.Cryptography;

public class RSAOAEPKeyExchangeFormatter : AsymmetricKeyExchangeFormatter
{
	public byte[]? Parameter
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override string? Parameters
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

	public RSAOAEPKeyExchangeFormatter()
	{
	}

	public RSAOAEPKeyExchangeFormatter(AsymmetricAlgorithm key)
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
