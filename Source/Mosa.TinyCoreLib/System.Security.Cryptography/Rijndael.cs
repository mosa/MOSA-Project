using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("The Rijndael and RijndaelManaged types are obsolete. Use Aes instead.", DiagnosticId = "SYSLIB0022", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
public abstract class Rijndael : SymmetricAlgorithm
{
	[UnsupportedOSPlatform("browser")]
	public new static Rijndael Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static Rijndael? Create(string algName)
	{
		throw null;
	}
}
