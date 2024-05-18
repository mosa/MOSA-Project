using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public sealed class CngProvider : IEquatable<CngProvider>
{
	public static CngProvider MicrosoftPlatformCryptoProvider
	{
		get
		{
			throw null;
		}
	}

	public static CngProvider MicrosoftSmartCardKeyStorageProvider
	{
		get
		{
			throw null;
		}
	}

	public static CngProvider MicrosoftSoftwareKeyStorageProvider
	{
		get
		{
			throw null;
		}
	}

	public string Provider
	{
		get
		{
			throw null;
		}
	}

	public CngProvider(string provider)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] CngProvider? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CngProvider? left, CngProvider? right)
	{
		throw null;
	}

	public static bool operator !=(CngProvider? left, CngProvider? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
