namespace System.Security.Cryptography.Cose;

public sealed class CoseSigner
{
	public HashAlgorithmName HashAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public AsymmetricAlgorithm Key
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderMap ProtectedHeaders
	{
		get
		{
			throw null;
		}
	}

	public RSASignaturePadding? RSASignaturePadding
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderMap UnprotectedHeaders
	{
		get
		{
			throw null;
		}
	}

	public CoseSigner(AsymmetricAlgorithm key, HashAlgorithmName hashAlgorithm, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null)
	{
	}

	public CoseSigner(RSA key, RSASignaturePadding signaturePadding, HashAlgorithmName hashAlgorithm, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null)
	{
	}
}
