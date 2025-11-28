using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public class SignatureDescription
{
	public string? DeformatterAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? DigestAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? FormatterAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public string? KeyAlgorithm
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public SignatureDescription()
	{
	}

	public SignatureDescription(SecurityElement el)
	{
	}

	[RequiresUnreferencedCode("CreateDeformatter is not trim compatible because the algorithm implementation referenced by DeformatterAlgorithm might be removed.")]
	public virtual AsymmetricSignatureDeformatter CreateDeformatter(AsymmetricAlgorithm key)
	{
		throw null;
	}

	[RequiresUnreferencedCode("CreateDigest is not trim compatible because the algorithm implementation referenced by DigestAlgorithm might be removed.")]
	public virtual HashAlgorithm? CreateDigest()
	{
		throw null;
	}

	[RequiresUnreferencedCode("CreateFormatter is not trim compatible because the algorithm implementation referenced by FormatterAlgorithm might be removed.")]
	public virtual AsymmetricSignatureFormatter CreateFormatter(AsymmetricAlgorithm key)
	{
		throw null;
	}
}
