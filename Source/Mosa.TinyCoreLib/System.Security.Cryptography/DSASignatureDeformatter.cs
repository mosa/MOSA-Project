namespace System.Security.Cryptography;

public class DSASignatureDeformatter : AsymmetricSignatureDeformatter
{
	public DSASignatureDeformatter()
	{
	}

	public DSASignatureDeformatter(AsymmetricAlgorithm key)
	{
	}

	public override void SetHashAlgorithm(string strName)
	{
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
	}

	public override bool VerifySignature(byte[] rgbHash, byte[] rgbSignature)
	{
		throw null;
	}
}
