namespace System.Security.Cryptography;

public class RSAPKCS1KeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
{
	public override string? Parameters
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RandomNumberGenerator? RNG
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public RSAPKCS1KeyExchangeDeformatter()
	{
	}

	public RSAPKCS1KeyExchangeDeformatter(AsymmetricAlgorithm key)
	{
	}

	public override byte[] DecryptKeyExchange(byte[] rgbIn)
	{
		throw null;
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
	}
}
