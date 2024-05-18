namespace System.Security.Cryptography;

public abstract class AsymmetricSignatureFormatter
{
	public abstract byte[] CreateSignature(byte[] rgbHash);

	public virtual byte[] CreateSignature(HashAlgorithm hash)
	{
		throw null;
	}

	public abstract void SetHashAlgorithm(string strName);

	public abstract void SetKey(AsymmetricAlgorithm key);
}
