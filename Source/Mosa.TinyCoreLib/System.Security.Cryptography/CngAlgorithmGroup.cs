using System.Diagnostics.CodeAnalysis;

namespace System.Security.Cryptography;

public sealed class CngAlgorithmGroup : IEquatable<CngAlgorithmGroup>
{
	public string AlgorithmGroup
	{
		get
		{
			throw null;
		}
	}

	public static CngAlgorithmGroup DiffieHellman
	{
		get
		{
			throw null;
		}
	}

	public static CngAlgorithmGroup Dsa
	{
		get
		{
			throw null;
		}
	}

	public static CngAlgorithmGroup ECDiffieHellman
	{
		get
		{
			throw null;
		}
	}

	public static CngAlgorithmGroup ECDsa
	{
		get
		{
			throw null;
		}
	}

	public static CngAlgorithmGroup Rsa
	{
		get
		{
			throw null;
		}
	}

	public CngAlgorithmGroup(string algorithmGroup)
	{
	}

	public override bool Equals([NotNullWhen(true)] object? obj)
	{
		throw null;
	}

	public bool Equals([NotNullWhen(true)] CngAlgorithmGroup? other)
	{
		throw null;
	}

	public override int GetHashCode()
	{
		throw null;
	}

	public static bool operator ==(CngAlgorithmGroup? left, CngAlgorithmGroup? right)
	{
		throw null;
	}

	public static bool operator !=(CngAlgorithmGroup? left, CngAlgorithmGroup? right)
	{
		throw null;
	}

	public override string ToString()
	{
		throw null;
	}
}
