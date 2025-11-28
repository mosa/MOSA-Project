using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[UnsupportedOSPlatform("browser")]
public class RSAPKCS1SignatureFormatter : AsymmetricSignatureFormatter
{
	public RSAPKCS1SignatureFormatter()
	{
	}

	public RSAPKCS1SignatureFormatter(AsymmetricAlgorithm key)
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
