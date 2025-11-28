namespace System.Security.Cryptography;

public class RSAOAEPKeyExchangeDeformatter : AsymmetricKeyExchangeDeformatter
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

	public RSAOAEPKeyExchangeDeformatter()
	{
	}

	public RSAOAEPKeyExchangeDeformatter(AsymmetricAlgorithm key)
	{
	}

	public override byte[] DecryptKeyExchange(byte[] rgbData)
	{
		throw null;
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
	}
}
