namespace System.Security.Cryptography;

public class DSASignatureFormatter : AsymmetricSignatureFormatter
{
	public DSASignatureFormatter()
	{
	}

	public DSASignatureFormatter(AsymmetricAlgorithm key)
	{
	}

	public override byte[] CreateSignature(byte[] rgbHash)
	{
		throw null;
	}

	public override void SetHashAlgorithm(string strName)
	{
	}

	public override void SetKey(AsymmetricAlgorithm key)
	{
	}
}
