using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[UnsupportedOSPlatform("browser")]
public class RSAPKCS1SignatureDeformatter : AsymmetricSignatureDeformatter
{
	public RSAPKCS1SignatureDeformatter()
	{
	}

	public RSAPKCS1SignatureDeformatter(AsymmetricAlgorithm key)
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
