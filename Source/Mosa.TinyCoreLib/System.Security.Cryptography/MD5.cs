using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace System.Security.Cryptography;

public abstract class MD5 : HashAlgorithm
{
	public const int HashSizeInBits = 128;

	public const int HashSizeInBytes = 16;

	[UnsupportedOSPlatform("browser")]
	public new static MD5 Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public new static MD5? Create(string algName)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(byte[] source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(Stream source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static int HashData(Stream source, Span<byte> destination)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static byte[] HashData(ReadOnlySpan<byte> source)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static int HashData(ReadOnlySpan<byte> source, Span<byte> destination)
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ValueTask<int> HashDataAsync(Stream source, Memory<byte> destination, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static ValueTask<byte[]> HashDataAsync(Stream source, CancellationToken cancellationToken = default(CancellationToken))
	{
		throw null;
	}

	[UnsupportedOSPlatform("browser")]
	public static bool TryHashData(ReadOnlySpan<byte> source, Span<byte> destination, out int bytesWritten)
	{
		throw null;
	}
}
