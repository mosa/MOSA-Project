using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography.Cose;

public sealed class CoseMultiSignMessage : CoseMessage
{
	public ReadOnlyCollection<CoseSignature> Signatures
	{
		get
		{
			throw null;
		}
	}

	internal CoseMultiSignMessage()
	{
	}

	public void AddSignatureForDetached(byte[] detachedContent, CoseSigner signer, byte[]? associatedData = null)
	{
	}

	public void AddSignatureForDetached(Stream detachedContent, CoseSigner signer, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
	}

	public void AddSignatureForDetached(ReadOnlySpan<byte> detachedContent, CoseSigner signer, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
	}

	public Task AddSignatureForDetachedAsync(Stream detachedContent, CoseSigner signer, ReadOnlyMemory<byte> associatedData = default(ReadOnlyMemory<byte>), CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public void AddSignatureForEmbedded(CoseSigner signer, byte[]? associatedData = null)
	{
	}

	public void AddSignatureForEmbedded(CoseSigner signer, ReadOnlySpan<byte> associatedData)
	{
	}

	public override int GetEncodedLength()
	{
		throw null;
	}

	public void RemoveSignature(int index)
	{
	}

	public void RemoveSignature(CoseSignature signature)
	{
	}

	public static byte[] SignDetached(byte[] detachedContent, CoseSigner signer, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, byte[]? associatedData = null)
	{
		throw null;
	}

	public static byte[] SignDetached(Stream detachedContent, CoseSigner signer, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public static byte[] SignDetached(ReadOnlySpan<byte> detachedContent, CoseSigner signer, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public static Task<byte[]> SignDetachedAsync(Stream detachedContent, CoseSigner signer, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, ReadOnlyMemory<byte> associatedData = default(ReadOnlyMemory<byte>), CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	public static byte[] SignEmbedded(byte[] embeddedContent, CoseSigner signer, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, byte[]? associatedData = null)
	{
		throw null;
	}

	public static byte[] SignEmbedded(ReadOnlySpan<byte> embeddedContent, CoseSigner signer, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public override bool TryEncode(Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}

	public static bool TrySignDetached(ReadOnlySpan<byte> detachedContent, Span<byte> destination, CoseSigner signer, out int bytesWritten, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}

	public static bool TrySignEmbedded(ReadOnlySpan<byte> embeddedContent, Span<byte> destination, CoseSigner signer, out int bytesWritten, CoseHeaderMap? protectedHeaders = null, CoseHeaderMap? unprotectedHeaders = null, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
		throw null;
	}
}
