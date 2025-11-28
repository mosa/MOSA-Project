using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public abstract class HashAlgorithm : IDisposable, ICryptoTransform
{
	protected int HashSizeValue;

	protected internal byte[]? HashValue;

	protected int State;

	public virtual bool CanReuseTransform
	{
		get
		{
			throw null;
		}
	}

	public virtual bool CanTransformMultipleBlocks
	{
		get
		{
			throw null;
		}
	}

	public virtual byte[]? Hash
	{
		get
		{
			throw null;
		}
	}

	public virtual int HashSize
	{
		get
		{
			throw null;
		}
	}

	public virtual int InputBlockSize
	{
		get
		{
			throw null;
		}
	}

	public virtual int OutputBlockSize
	{
		get
		{
			throw null;
		}
	}

	public void Clear()
	{
	}

	public byte[] ComputeHash(byte[] buffer)
	{
		throw null;
	}

	public byte[] ComputeHash(byte[] buffer, int offset, int count)
	{
		throw null;
	}

	public byte[] ComputeHash(Stream inputStream)
	{
		throw null;
	}

	public Task<byte[]> ComputeHashAsync(Stream inputStream, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[Obsolete("The default implementation of this cryptography algorithm is not supported.", DiagnosticId = "SYSLIB0007", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static HashAlgorithm Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static HashAlgorithm? Create(string hashName)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	protected abstract void HashCore(byte[] array, int ibStart, int cbSize);

	protected virtual void HashCore(ReadOnlySpan<byte> source)
	{
	}

	protected abstract byte[] HashFinal();

	public abstract void Initialize();

	public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[]? outputBuffer, int outputOffset)
	{
		throw null;
	}

	public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
	{
		throw null;
	}

	public bool TryComputeHash(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	protected virtual bool TryHashFinal(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
