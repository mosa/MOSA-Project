namespace System.Security.Cryptography;

public abstract class AsymmetricSignatureDeformatter
{
	public abstract void SetHashAlgorithm(string strName);

	public abstract void SetKey(AsymmetricAlgorithm key);

	public abstract bool VerifySignature(byte[] rgbHash, byte[] rgbSignature);

	public virtual bool VerifySignature(HashAlgorithm hash, byte[] rgbSignature)
	{
		throw null;
	}
}
