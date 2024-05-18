using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
public sealed class ChaCha20Poly1305 : IDisposable
{
	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public ChaCha20Poly1305(byte[] key)
	{
	}

	public ChaCha20Poly1305(ReadOnlySpan<byte> key)
	{
	}

	public void Decrypt(byte[] nonce, byte[] ciphertext, byte[] tag, byte[] plaintext, byte[]? associatedData = null)
	{
	}

	public void Decrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> ciphertext, ReadOnlySpan<byte> tag, Span<byte> plaintext, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
	}

	public void Dispose()
	{
	}

	public void Encrypt(byte[] nonce, byte[] plaintext, byte[] ciphertext, byte[] tag, byte[]? associatedData = null)
	{
	}

	public void Encrypt(ReadOnlySpan<byte> nonce, ReadOnlySpan<byte> plaintext, Span<byte> ciphertext, Span<byte> tag, ReadOnlySpan<byte> associatedData = default(ReadOnlySpan<byte>))
	{
	}
}
