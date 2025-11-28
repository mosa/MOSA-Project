using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public abstract class HMAC : KeyedHashAlgorithm
{
	protected int BlockSizeValue
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

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

	[Obsolete("The default implementation of this cryptography algorithm is not supported.", DiagnosticId = "SYSLIB0007", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static HMAC Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static HMAC? Create(string algorithmName)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	protected override void HashCore(byte[] rgb, int ib, int cb)
	{
	}

	protected override void HashCore(ReadOnlySpan<byte> source)
	{
	}

	protected override byte[] HashFinal()
	{
		throw null;
	}

	public override void Initialize()
	{
	}

	protected override bool TryHashFinal(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
