namespace System.Security.Cryptography.X509Certificates;

public abstract class X509SignatureGenerator
{
	public PublicKey PublicKey
	{
		get
		{
			throw null;
		}
	}

	protected abstract PublicKey BuildPublicKey();

	public static X509SignatureGenerator CreateForECDsa(ECDsa key)
	{
		throw null;
	}

	public static X509SignatureGenerator CreateForRSA(RSA key, RSASignaturePadding signaturePadding)
	{
		throw null;
	}

	public abstract byte[] GetSignatureAlgorithmIdentifier(HashAlgorithmName hashAlgorithm);

	public abstract byte[] SignData(byte[] data, HashAlgorithmName hashAlgorithm);
}
