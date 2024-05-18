using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace System.IO.Hashing;

public abstract class NonCryptographicHashAlgorithm
{
	public int HashLengthInBytes
	{
		get
		{
			throw null;
		}
	}

	protected NonCryptographicHashAlgorithm(int hashLengthInBytes)
	{
	}

	public void Append(byte[] source)
	{
	}

	public void Append(Stream stream)
	{
	}

	public abstract void Append(ReadOnlySpan<byte> source);

	public Task AppendAsync(Stream stream, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public byte[] GetCurrentHash()
	{
		throw null;
	}

	public int GetCurrentHash(Span<byte> destination)
	{
		throw null;
	}

	protected abstract void GetCurrentHashCore(Span<byte> destination);

	public byte[] GetHashAndReset()
	{
		throw null;
	}

	public int GetHashAndReset(Span<byte> destination)
	{
		throw null;
	}

	protected virtual void GetHashAndResetCore(Span<byte> destination)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Obsolete("Use GetCurrentHash() to retrieve the computed hash code.", true)]
	public override int GetHashCode()
	{
		throw null;
	}

	public abstract void Reset();

	public bool TryGetCurrentHash(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public bool TryGetHashAndReset(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
