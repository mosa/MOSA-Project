using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public abstract class RandomNumberGenerator : IDisposable
{
	public static RandomNumberGenerator Create()
	{
		throw null;
	}

	[RequiresUnreferencedCode("The default algorithm implementations might be removed, use strong type references like 'RSA.Create()' instead.")]
	[Obsolete("Cryptographic factory methods accepting an algorithm name are obsolete. Use the parameterless Create factory method on the algorithm type instead.", DiagnosticId = "SYSLIB0045", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public static RandomNumberGenerator? Create(string rngName)
	{
		throw null;
	}

	public void Dispose()
	{
	}

	protected virtual void Dispose(bool disposing)
	{
	}

	public static void Fill(Span<byte> data)
	{
	}

	public abstract void GetBytes(byte[] data);

	public virtual void GetBytes(byte[] data, int offset, int count)
	{
	}

	public static byte[] GetBytes(int count)
	{
		throw null;
	}

	public virtual void GetBytes(Span<byte> data)
	{
	}

	public static string GetHexString(int stringLength, bool lowercase = false)
	{
		throw null;
	}

	public static void GetHexString(Span<char> destination, bool lowercase = false)
	{
	}

	public static int GetInt32(int toExclusive)
	{
		throw null;
	}

	public static int GetInt32(int fromInclusive, int toExclusive)
	{
		throw null;
	}

	public static T[] GetItems<T>(ReadOnlySpan<T> choices, int length)
	{
		throw null;
	}

	public static void GetItems<T>(ReadOnlySpan<T> choices, Span<T> destination)
	{
	}

	public virtual void GetNonZeroBytes(byte[] data)
	{
	}

	public virtual void GetNonZeroBytes(Span<byte> data)
	{
	}

	public static string GetString(ReadOnlySpan<char> choices, int length)
	{
		throw null;
	}

	public static void Shuffle<T>(Span<T> values)
	{
	}
}
