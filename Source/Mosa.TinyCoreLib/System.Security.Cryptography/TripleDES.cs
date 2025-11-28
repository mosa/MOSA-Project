using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

public abstract class TripleDES : SymmetricAlgorithm
{
	public override byte[] Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("browser")]
	public new static TripleDES Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static TripleDES? Create(string str)
	{
		throw null;
	}

	public static bool IsWeakKey(byte[] rgbKey)
	{
		throw null;
	}
}
