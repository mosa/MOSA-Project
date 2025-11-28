namespace System.Security.Cryptography;

public class Rfc2898DeriveBytes : DeriveBytes
{
	public HashAlgorithmName HashAlgorithm
	{
		get
		{
			throw null;
		}
	}

	public int IterationCount
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	public byte[] Salt
	{
		get
		{
			throw null;
		}
		set
		{
		}
	}

	[Obsolete("The default hash algorithm and iteration counts in Rfc2898DeriveBytes constructors are outdated and insecure. Use a constructor that accepts the hash algorithm and the number of iterations.", DiagnosticId = "SYSLIB0041", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations)
	{
	}

	public Rfc2898DeriveBytes(byte[] password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm)
	{
	}

	[Obsolete("The default hash algorithm and iteration counts in Rfc2898DeriveBytes constructors are outdated and insecure. Use a constructor that accepts the hash algorithm and the number of iterations.", DiagnosticId = "SYSLIB0041", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public Rfc2898DeriveBytes(string password, byte[] salt)
	{
	}

	[Obsolete("The default hash algorithm and iteration counts in Rfc2898DeriveBytes constructors are outdated and insecure. Use a constructor that accepts the hash algorithm and the number of iterations.", DiagnosticId = "SYSLIB0041", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public Rfc2898DeriveBytes(string password, byte[] salt, int iterations)
	{
	}

	public Rfc2898DeriveBytes(string password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm)
	{
	}

	[Obsolete("The default hash algorithm and iteration counts in Rfc2898DeriveBytes constructors are outdated and insecure. Use a constructor that accepts the hash algorithm and the number of iterations.", DiagnosticId = "SYSLIB0041", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public Rfc2898DeriveBytes(string password, int saltSize)
	{
	}

	[Obsolete("The default hash algorithm and iteration counts in Rfc2898DeriveBytes constructors are outdated and insecure. Use a constructor that accepts the hash algorithm and the number of iterations.", DiagnosticId = "SYSLIB0041", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public Rfc2898DeriveBytes(string password, int saltSize, int iterations)
	{
	}

	public Rfc2898DeriveBytes(string password, int saltSize, int iterations, HashAlgorithmName hashAlgorithm)
	{
	}

	[Obsolete("Rfc2898DeriveBytes.CryptDeriveKey is obsolete and is not supported. Use PasswordDeriveBytes.CryptDeriveKey instead.", DiagnosticId = "SYSLIB0033", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
	public byte[] CryptDeriveKey(string algname, string alghashname, int keySize, byte[] rgbIV)
	{
		throw null;
	}

	protected override void Dispose(bool disposing)
	{
	}

	public override byte[] GetBytes(int cb)
	{
		throw null;
	}

	public static byte[] Pbkdf2(byte[] password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm, int outputLength)
	{
		throw null;
	}

	public static byte[] Pbkdf2(ReadOnlySpan<byte> password, ReadOnlySpan<byte> salt, int iterations, HashAlgorithmName hashAlgorithm, int outputLength)
	{
		throw null;
	}

	public static void Pbkdf2(ReadOnlySpan<byte> password, ReadOnlySpan<byte> salt, Span<byte> destination, int iterations, HashAlgorithmName hashAlgorithm)
	{
	}

	public static byte[] Pbkdf2(ReadOnlySpan<char> password, ReadOnlySpan<byte> salt, int iterations, HashAlgorithmName hashAlgorithm, int outputLength)
	{
		throw null;
	}

	public static void Pbkdf2(ReadOnlySpan<char> password, ReadOnlySpan<byte> salt, Span<byte> destination, int iterations, HashAlgorithmName hashAlgorithm)
	{
	}

	public static byte[] Pbkdf2(string password, byte[] salt, int iterations, HashAlgorithmName hashAlgorithm, int outputLength)
	{
		throw null;
	}

	public override void Reset()
	{
	}
}
