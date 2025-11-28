using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public abstract class KeyedHashAlgorithm : HashAlgorithm
{
	protected byte[] KeyValue;

	public virtual byte[] Key
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("The default implementation of this cryptography algorithm is not supported.", DiagnosticId = "SYSLIB0007", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static KeyedHashAlgorithm Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static KeyedHashAlgorithm? Create(string algName)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}
}
