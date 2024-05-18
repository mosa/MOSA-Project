using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
public abstract class RC2 : SymmetricAlgorithm
{
	protected int EffectiveKeySizeValue;

	public virtual int EffectiveKeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public override int KeySize
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[UnsupportedOSPlatform("android")]
	[UnsupportedOSPlatform("browser")]
	public new static RC2 Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static RC2? Create(string AlgName)
	{
		throw null;
	}
}
