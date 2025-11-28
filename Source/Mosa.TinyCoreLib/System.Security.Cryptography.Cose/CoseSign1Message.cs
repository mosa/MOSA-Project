using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography.Cose;

public sealed class CoseSign1Message : CoseMessage
{
	public ReadOnlyMemory<byte> Signature
	{
		get
		{
			throw null;
		}
	}

	internal CoseSign1Message()
	{
	}

	public override int GetEncodedLength()
	{
		throw null;
	}

	public static byte[] SignDetached(byte[] detachedContent, CoseSigner signer, byte[]? associatedData = null)
	{
		throw null;
	}

	public static byte[] SignDetached(Stream detachedContent, CoseSigner signer, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public static byte[] SignDetached(ReadOnlySpan<byte> detachedContent, CoseSigner signer, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public static Task<byte[]> SignDetachedAsync(Stream detachedContent, CoseSigner signer, ReadOnlyMemory<byte> associatedData = default(ReadOnlyMemory<byte>), CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static byte[] SignEmbedded(byte[] embeddedContent, CoseSigner signer, byte[]? associatedData = null)
	{
		throw null;
	}

	public static byte[] SignEmbedded(ReadOnlySpan<byte> embeddedContent, CoseSigner signer, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public override bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static bool TrySignDetached(ReadOnlySpan<byte> detachedContent, Span<byte> destination, CoseSigner signer, out int bytesWritten, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public static bool TrySignEmbedded(ReadOnlySpan<byte> embeddedContent, Span<byte> destination, CoseSigner signer, out int bytesWritten, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
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
