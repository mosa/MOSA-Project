using System.Runtime.Versioning;

namespace System.Security.Cryptography;

[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
public sealed class AesGcm : IDisposable
{
	public static bool IsSupported
	{
		get
		{
			throw null;
		}
	}

	public static KeySizes NonceByteSizes
	{
		get
		{
			throw null;
		}
	}

	public static KeySizes TagByteSizes
	{
		get
		{
			throw null;
		}
	}

	public int? TagSizeInBytes
	{
		get
		{
			throw null;
		}
	}

	[Obsolete("AesGcm should indicate the required tag size for encryption and decryption. Use a constructor that accepts the tag size.", DiagnosticId = "SYSLIB0053", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public AesGcm(byte[] key)
	{
	}

	public AesGcm(byte[] key, int tagSizeInBytes)
	{
	}

	[Obsolete("AesGcm should indicate the required tag size for encryption and decryption. Use a constructor that accepts the tag size.", DiagnosticId = "SYSLIB0053", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public AesGcm(ReadOnlySpan<byte> key)
	{
	}

	public AesGcm(ReadOnlySpan<byte> key, int tagSizeInBytes)
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
