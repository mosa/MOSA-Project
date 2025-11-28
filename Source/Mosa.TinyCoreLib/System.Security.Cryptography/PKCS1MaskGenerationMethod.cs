using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public class PKCS1MaskGenerationMethod : MaskGenerationMethod
{
	public string HashName
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[RequiresUnreferencedCode("PKCS1MaskGenerationMethod is not trim compatible because the algorithm implementation referenced by HashName might be removed.")]
	public PKCS1MaskGenerationMethod()
	{
	}

	public override byte[] GenerateMask(byte[] rgbSeed, int cbReturn)
	{
		throw null;
	}
}
