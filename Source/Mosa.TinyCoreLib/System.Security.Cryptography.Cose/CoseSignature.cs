using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography.Cose;

public sealed class CoseSignature
{
	public CoseHeaderMap ProtectedHeaders
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> RawProtectedHeaders
	{
		get
		{
			throw null;
		}
	}

	public ReadOnlyMemory<byte> Signature
	{
		get
		{
			throw null;
		}
	}

	public CoseHeaderMap UnprotectedHeaders
	{
		get
		{
			throw null;
		}
	}

	internal CoseSignature()
	{
	}

	public bool VerifyDetached(AsymmetricAlgorithm key, byte[] detachedContent, byte[]? associatedData = null)
	{
		throw null;
	}

	public bool VerifyDetached(AsymmetricAlgorithm key, Stream detachedContent, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public bool VerifyDetached(AsymmetricAlgorithm key, ReadOnlySpan<byte> detachedContent, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public Task<bool> VerifyDetachedAsync(AsymmetricAlgorithm key, Stream detachedContent, ReadOnlyMemory<byte> associatedData = default(ReadOnlyMemory<byte>), CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public bool VerifyEmbedded(AsymmetricAlgorithm key, byte[]? associatedData = null)
	{
		throw null;
	}

	public bool VerifyEmbedded(AsymmetricAlgorithm key, ReadOnlySpan<byte> associatedData)
	{
		throw null;
	}
}
